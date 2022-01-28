#![deny(warnings)]

use legion::*;
use rapier2d::prelude::*;

use crate::components::{Body, Collision, Position};
use crate::{misc::length_to_unit, world::SubWorld};

pub struct CollisionData {
    pub colliders: ColliderSet,
    pub islands: IslandManager,
    pub bodies: RigidBodySet,
    pub query: QueryPipeline,
}

impl Default for CollisionData {
    fn default() -> Self {
        return CollisionData {
            colliders: ColliderSet::new(),
            islands: IslandManager::new(),
            bodies: RigidBodySet::new(),
            query: QueryPipeline::new(),
        };
    }
}

#[inline]
pub fn user_data_to_entity(user_data: u128) -> Entity {
    let tmp: u128 = unsafe { std::mem::transmute(user_data) };
    let entity: Entity = unsafe { std::mem::transmute(tmp as u64) };
    return entity;
}

#[system]
#[read_component(Position)]
#[read_component(Body)]
#[read_component(Collision)]
pub fn update_collision(world: &SubWorld, #[resource] collision: &mut CollisionData) {
    let mut query = <(&Position, &Body, &Collision)>::query();

    for (pos, body, col) in query.iter(world) {
        if let Some(val) = collision.colliders.get_mut(col.col_handler) {
            val.set_shape(SharedShape::cuboid(
                length_to_unit(body.width),
                length_to_unit(body.height),
            ));
            val.set_translation(vector![pos.x, pos.y])
        }
    }

    collision
        .query
        .update(&collision.islands, &collision.bodies, &collision.colliders);
}
