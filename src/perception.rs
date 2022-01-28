#![deny(warnings)]
#![forbid(unsafe_code)]

use std::collections::HashMap;
use std::time::Duration;

use crate::common::DeltaTime;
use crate::world::SubWorld;
use crate::{
    collision::{user_data_to_entity, CollisionData},
    components::*,
};

use legion::*;
use measurements::{Length, Mass};
use rand::{prelude::StdRng, Rng};
use rapier2d::prelude::*;
use rltk::RGB;

fn modify_mass(view_dist: Length, dist: Length, val: Mass, rng: &mut StdRng) -> Mass {
    if dist < view_dist {
        return Mass::from_grams(0.0);
    }

    let mut modifier: Mass;
    let max = Mass::as_grams(&val) * (1.0 - view_dist / dist);
    modifier = Mass::from_grams(rng.gen_range(0.0..max));
    if rng.gen() {
        modifier = Mass::from_grams(-Mass::as_grams(&modifier));
    }
    return modifier;
}

fn modify_length(view_dist: Length, dist: Length, val: Length, rng: &mut StdRng) -> Length {
    if dist < view_dist {
        return Length::from_centimeters(0.0);
    }

    let mut modifier: Length;
    let max = Length::as_millimeters(&val) * (1.0 - view_dist / dist);
    modifier = Length::from_millimeters(rng.gen_range(0.0..max));
    if rng.gen() {
        modifier = Length::from_millimeters(-Length::as_millimeters(&modifier));
    }
    return modifier;
}

#[system]
#[write_component(UnitPerception)]
#[read_component(Position)]
#[read_component(Direction)]
#[read_component(Body)]
#[read_component(Eyes)]
#[read_component(Render)]
pub fn update_perception(
    world: &mut SubWorld,
    #[resource] time: &DeltaTime,
    #[resource] collision: &CollisionData,
    #[resource] rng: &mut StdRng,
) {
    let mut query = <(Entity, &mut UnitPerception, &Position, &Direction, &Eyes)>::query();

    let mut in_vision = HashMap::<Entity, Vec<Entity>>::new();

    // get everything which can be viewed
    for (entity, perception, pos, dir, _eyes) in query.iter_mut(world) {
        match perception.time_to_update.checked_sub(time.duration()) {
            Some(val) => perception.time_to_update = val,
            None => perception.time_to_update = Duration::ZERO,
        }

        if perception.time_to_update > Duration::ZERO {
            perception.dirty = false;
            continue;
        }
        perception.time_to_update = perception.update_interval;
        perception.dirty = true;

        in_vision.insert(entity.clone(), Vec::<Entity>::new());

        let ray = Ray::new(
            rapier2d::math::Point::from([pos.x, pos.y]),
            Vector::new(dir.x, dir.y),
        );

        let filter = |handle: ColliderHandle| {
            return match collision.colliders.get(handle) {
                Some(val) => {
                    // check it is not the looking entity
                    user_data_to_entity(val.user_data) != *entity
                }
                None => false,
            };
        };
        if let Some((handle, _toi)) = collision.query.cast_ray(
            &collision.colliders,
            &ray,
            Real::MAX,
            true,
            InteractionGroups::all(),
            Some(&filter),
        ) {
            if let Some(val) = collision.colliders.get(handle) {
                in_vision
                    .get_mut(&entity)
                    .unwrap()
                    .push(user_data_to_entity(val.user_data));
            };
        }
    }

    for (viewer, subjects) in &in_vision {
        if subjects.len() <= 0 {
            continue;
        }

        let source_pos = match world.entry_ref(*viewer) {
            Ok(_) => match world.entry_ref(*viewer) {
                Ok(object) => *object.get_component::<Position>().unwrap(),
                Err(_) => Position::default(),
            },
            Err(_) => Position::default(),
        };
        let eyes = *world
            .entry_ref(*viewer)
            .unwrap()
            .get_component::<Eyes>()
            .unwrap();

        let color_perception_dist = eyes.reading_distance * 100.0;

        let mut vision_objects = Vec::<VisionObject>::new();
        for subject in subjects {
            let mut vision_object = VisionObject::default();
            // Process what they can see
            if let Ok(object) = world.entry_ref(*subject) {
                let position = object.get_component::<Position>().unwrap();
                vision_object.position = *position;

                let dist = source_pos.distance_between(position);

                vision_object.distance = Some(dist);

                // Add add height and weight to vision object
                if let Ok(body) = object.get_component::<Body>() {
                    let attributes_distance = eyes.reading_distance;

                    vision_object.approximant_weight = Some(
                        body.weight + modify_mass(attributes_distance, dist, body.weight, rng),
                    );
                    vision_object.approximant_height = Some(
                        body.height + modify_length(attributes_distance, dist, body.height, rng),
                    );
                }

                // Add appearance to vision object
                if let Ok(render) = object.get_component::<Render>() {
                    let modifier: f32;
                    if color_perception_dist < dist {
                        modifier = 1.0 - (color_perception_dist / dist) as f32;
                    } else {
                        modifier = 0.0;
                    }

                    let modify = |current: f32, modifier: f32, plus: bool| {
                        if plus {
                            return current + (1.0 * modifier);
                        }
                        return current - (1.0 * modifier);
                    };

                    let hat = RGB::from_f32(
                        modify(render.fg.r, modifier, rng.gen()),
                        modify(render.fg.g, modifier, rng.gen()),
                        modify(render.fg.b, modifier, rng.gen()),
                    );
                    let body = RGB::from_f32(
                        modify(render.bg.r, modifier, rng.gen()),
                        modify(render.bg.g, modifier, rng.gen()),
                        modify(render.bg.b, modifier, rng.gen()),
                    );
                    vision_object.approximant_appearance = Some(Appearance::new(hat, body));
                }
            }
            vision_objects.push(vision_object);
        }

        // Update what the unit can see
        if let Ok(mut object) = world.entry_mut(*viewer) {
            if let Ok(perception) = object.get_component_mut::<UnitPerception>() {
                perception.can_see = vision_objects;
            }
        }
    }
}
