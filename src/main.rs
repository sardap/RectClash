#![deny(warnings)]

mod camera;
mod collision;
mod common;
mod components;
mod decision;
mod input;
mod misc;
mod movement;
mod perception;
mod render;

use std::collections::VecDeque;
use std::time::Duration;
use std::time::SystemTime;

use legion::*;
use measurements::Length;
use measurements::Mass;
use nannou::prelude::*;
use rand::{prelude::StdRng, Rng, SeedableRng};
use rapier2d::prelude::*;

use rltk::RED;
use rltk::RGBA;

use crate::camera::update_camera_system;
use crate::collision::{update_collision_system, CollisionData};
use crate::common::*;
use crate::components::*;
use crate::decision::update_decision_system;
use crate::input::*;
use crate::misc::length_to_unit;
use crate::movement::*;
use crate::perception::update_perception_system;

pub struct UnitCreator {
    reading_distance_min: Length,
    reading_distance_max: Length,
    weight_min: Mass,
    weight_max: Mass,
    perception_min: Duration,
    perception_max: Duration,
}

impl UnitCreator {
    pub fn create_unit(
        &self,
        col: &mut CollisionData,
        rng: &mut StdRng,
    ) -> (
        Position,
        Velocity,
        Eyes,
        Body,
        Render,
        Collision,
        Direction,
        UnitPerception,
        UnitDecision,
        Knowledge,
    ) {
        let mut rand = rand::thread_rng();

        // make handler
        let rigid_body = RigidBodyBuilder::new_dynamic()
            .translation(vector![0.0, 0.0])
            .build();
        let body_handler = col.bodies.insert(rigid_body);
        let collider = ColliderBuilder::cuboid(0.0, 0.0).build();
        let col_handler = col
            .colliders
            .insert_with_parent(collider, body_handler, &mut col.bodies);

        return (
            Position { x: 10.0, y: 20.0 },
            Velocity { dx: 0.0, dy: 0.0 },
            Eyes {
                reading_distance: measurements::Length::from_centimeters(rng.gen_range(
                    self.reading_distance_min.as_centimeters()
                        ..self.reading_distance_max.as_centimeters(),
                )),
            },
            Body {
                weight: measurements::Mass::from_grams(
                    rand.gen_range(self.weight_min.as_grams()..self.weight_max.as_grams()),
                ),
                width: Length::from_millimeters(460.0),
                height: Length::from_millimeters(1765.0),
            },
            Render {
                fg: RGBA::named(RED),
                bg: RGBA::named(RED),
            },
            Collision {
                body_handler,
                col_handler,
            },
            Direction { x: 0.0, y: -1.0 },
            UnitPerception {
                update_interval: Duration::from_millis(
                    rand.gen_range(self.perception_min.as_millis()..self.perception_max.as_millis())
                        as u64,
                ),
                time_to_update: Duration::from_nanos(0),
                can_see: Vec::new(),
                dirty: false,
            },
            UnitDecision {
                current_state: UnitDecisionState::Starting,
                last_states: VecDeque::new(),
            },
            Knowledge::default(),
        );
    }
}

pub fn create_camera() -> (Camera, Position, InputCom) {
    return (
        Camera {
            speed: Length::from_meters(100.0),
            scale: 1.0,
        },
        Position::default(),
        InputCom::default(),
    );
}

struct Model {
    world: World,
    resources: Resources,
    schedule: Schedule,
    last_time: SystemTime,
}

impl Model {
    fn new() -> Self {
        let mut world = World::default();

        let unit_creator = UnitCreator {
            reading_distance_min: measurements::Length::from_meters(4.0),
            reading_distance_max: measurements::Length::from_meters(8.0),
            weight_min: measurements::Mass::from_kilograms(50.0),
            weight_max: measurements::Mass::from_kilograms(90.0),
            perception_min: Duration::from_millis(40),
            perception_max: Duration::from_millis(70),
        };

        let mut resources = Resources::default();
        resources.insert(DeltaTime::default());
        resources.insert(CollisionData::default());
        resources.insert(StdRng::seed_from_u64(10));
        resources.insert(Inputs::default());

        world.push(create_camera());

        let mut create = |rank_size: i32, x_start: f32, y_start: f32| {
            let mut col = resources.get_mut::<CollisionData>().unwrap();
            let mut rng = resources.get_mut::<StdRng>().unwrap();

            let mut leader = None;

            let mut x_off = x_start;
            for i in 0..rank_size {
                let mut unit = unit_creator.create_unit(&mut col, &mut rng);

                let y = (i / rank_size) as f32;
                unit.0.x = x_off;
                unit.0.y = y_start + (y * length_to_unit(unit.3.height));

                x_off += length_to_unit(unit.3.width + Length::from_meters(1.0)).ceil();

                let entity = world.push(unit);
                if let Ok(mut entry) = world.entry_mut(entity) {
                    // access the entity's components, returns `None` if the entity does not have the component
                    let col_com = entry.get_component::<Collision>().unwrap();

                    let mut collider = col.colliders.get_mut(col_com.col_handler).unwrap();
                    let id: u64 = unsafe { std::mem::transmute(entity) };
                    collider.user_data = id as u128;

                    if let Some(leader_object) = &leader {
                        let knowledge = entry.get_component_mut::<Knowledge>().unwrap();
                        knowledge.leaders.push(*leader_object);
                    } else {
                        leader = Some(IdentityObject::new(&world, entity));
                    }
                }
            }
        };
        create(5, 10.0, 10.0);
        create(5, 10.0, 40.0);

        // construct a schedule (you should do this on init)
        let schedule = Schedule::builder()
            .add_system(update_input_system())
            .flush()
            .add_system(update_positions_system())
            .add_system(update_camera_system())
            .flush()
            .add_system(update_collision_system())
            .flush()
            .add_system(update_perception_system())
            .flush()
            .add_system(update_decision_system())
            .build();

        return Model {
            world: world,
            resources: resources,
            schedule: schedule,
            last_time: SystemTime::now(),
        };
    }
}

impl Model {
    fn tick(&mut self) {
        // Update delta time
        {
            let delta = SystemTime::now().duration_since(self.last_time).unwrap();
            self.last_time = SystemTime::now();
            let mut time = self.resources.get_mut::<DeltaTime>().unwrap();
            time.0 = delta;
            println!("{:?}", delta);
        }

        // run our schedule (you should do this each update)
        self.schedule.execute(&mut self.world, &mut self.resources);
    }
}

fn window_event(_app: &App, model: &mut Model, event: WindowEvent) {
    match event {
        KeyPressed(key) => {
            let mut inputs = model.resources.get_mut::<Inputs>().unwrap();
            let input_key = input::InputKey::from(key);
            inputs.pressed.insert(input_key, true);
        }
        KeyReleased(key) => {
            let mut inputs = model.resources.get_mut::<Inputs>().unwrap();
            let input_key = input::InputKey::from(key);
            inputs.pressed.insert(input_key, false);
        }
        ReceivedCharacter(_char) => {}
        MouseMoved(_pos) => {}
        MousePressed(_button) => {}
        MouseReleased(_button) => {}
        MouseEntered => {}
        MouseExited => {}
        MouseWheel(_amount, _phase) => {}
        Moved(_pos) => {}
        Resized(_size) => {}
        Touch(_touch) => {}
        TouchPressure(_pressure) => {}
        HoveredFile(_path) => {}
        DroppedFile(_path) => {}
        HoveredFileCancelled => {}
        Focused => {}
        Unfocused => {}
        Closed => {}
    }
}

fn view(app: &App, model: &Model, frame: Frame) {
    render::render(&model.world, app, &frame);
}

fn model(app: &App) -> Model {
    let _window = app
        .new_window()
        .size(480, 360)
        .view(view)
        .event(window_event)
        .build()
        .unwrap();

    Model::new()
}

fn update(_app: &App, model: &mut Model, _update: Update) {
    model.tick()
}

fn main() {
    // {
    //     let mut query_pipeline = rapier2d::pipeline::QueryPipeline::new();

    //     let mut colliders = ColliderSet::new();

    //     let islands = IslandManager::new();
    //     let mut bodies = RigidBodySet::new();

    //     let rigid_body = RigidBodyBuilder::new_dynamic()
    //         .translation(vector![0.0, 15.0])
    //         .build();
    //     let collider = ColliderBuilder::cuboid(4.0, 4.0).build();
    //     let ball_body_handle = bodies.insert(rigid_body);
    //     colliders.insert_with_parent(collider, ball_body_handle, &mut bodies);

    //     query_pipeline.update(&islands, &bodies, &colliders);

    //     let ray = Ray::new(
    //         rapier2d::math::Point::from([0.0, 0.0]),
    //         Vector::new(0.0, 1.0),
    //     );
    //     if let Some((handle, toi)) = query_pipeline.cast_ray(
    //         &colliders,
    //         &ray,
    //         Real::MAX,
    //         true,
    //         InteractionGroups::all(),
    //         None,
    //     ) {
    //         println!("Collider Handle: {:?}, Real: {}", handle, ray.point_at(toi));
    //     } else {
    //         println!("No collision");
    //     }
    // }

    nannou::app(model).update(update).run();
}
