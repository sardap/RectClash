#![deny(warnings)]
#![forbid(unsafe_code)]

use legion::*;

use crate::{common::DeltaTime, components::*};

#[system(for_each)]
pub fn update_positions(pos: &mut Position, vel: &Velocity, #[resource] time: &DeltaTime) {
    pos.x += vel.dx * time.elapsed_seconds();
    pos.y += vel.dy * time.elapsed_seconds();
}
