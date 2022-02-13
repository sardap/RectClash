#![deny(warnings)]
#![feature(in_band_lifetimes)]

mod camera;
mod collision;
mod common;
mod components;
mod decision;
mod factory;
mod input;
mod measurements;
mod misc;
mod movement;
mod perception;
mod render;
mod scripting;

use std::time::SystemTime;

use legion::*;
use nannou::prelude::*;
use rand::{prelude::StdRng, SeedableRng};
use scripting::Scripts;

use crate::camera::update_camera_system;
use crate::collision::{update_collision_system, CollisionData};
use crate::common::*;
use crate::components::*;
use crate::decision::update_decision_system;
use crate::factory::run_create_scene;
use crate::input::*;
use crate::measurements::Length;
use crate::movement::*;
use crate::perception::update_perception_system;
extern crate derive_more;

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

        let mut resources = Resources::default();
        resources.insert(DeltaTime::default());
        resources.insert(CollisionData::default());
        resources.insert(StdRng::seed_from_u64(10));
        resources.insert(Inputs::default());
        resources.insert(Scripts::default());

        world.push(create_camera());

        {
            let mut col = resources.get_mut::<CollisionData>().unwrap();
            let mut scripts = resources.get_mut::<Scripts>().unwrap();

            run_create_scene(&mut world, &mut col, &mut scripts);
        }

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
