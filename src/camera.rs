#![deny(warnings)]
#![forbid(unsafe_code)]

use euclid::Point2D;
use legion::*;
use num::clamp;

use crate::common::DeltaTime;
use crate::components::{Camera, InputCom, InputCommand, Position};
use crate::misc::length_to_unit;

#[system(for_each)]
pub fn update_camera(
    cam: &mut Camera,
    pos: &mut Position,
    input: &InputCom,
    #[resource] delta_time: &DeltaTime,
) {
    let mut movement = Point2D::<f32, f32>::default();

    let speed = length_to_unit(cam.speed);

    for (cmd, pressed) in &input.pressed {
        if !pressed {
            continue;
        }

        match cmd {
            InputCommand::MoveLeft => {
                movement.x += speed * delta_time.elapsed_seconds();
            }
            InputCommand::MoveRight => {
                movement.x -= speed * delta_time.elapsed_seconds();
            }
            InputCommand::MoveUp => {
                movement.y -= speed * delta_time.elapsed_seconds();
            }
            InputCommand::MoveDown => {
                movement.y += speed * delta_time.elapsed_seconds();
            }
            _ => {}
        }
    }

    for (cmd, pressed) in &input.released {
        if !pressed {
            continue;
        }

        let max_zoom = 256.0f32;
        let min_zoom = 0.03125f32;

        match cmd {
            InputCommand::ScaleUp => {
                cam.scale = clamp(cam.scale * 2.0, min_zoom, max_zoom);
            }
            InputCommand::ScaleDown => {
                cam.scale = clamp(cam.scale / 2.0, min_zoom, max_zoom);
            }
            _ => {}
        }
    }

    pos.x += movement.x;
    pos.y += movement.y;
}
