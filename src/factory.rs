#![deny(warnings)]

use std::{collections::VecDeque, path::Path, time::Duration};

use legion::{Entity, EntityStore, World};
use measurements::Length;
use rapier2d::prelude::*;
use rltk::RGBA;
use rlua::{Table, UserData};

use crate::{
    collision::CollisionData,
    components::*,
    scripting::{load_script, Scripting},
};

fn entity_as_u128(entity: Entity) -> u128 {
    let id: u64 = unsafe { std::mem::transmute(entity) };
    return id as u128;
}

fn u128_as_entity(id: u128) -> Entity {
    let tmp: u128 = unsafe { std::mem::transmute(id) };
    let entity: Entity = unsafe { std::mem::transmute(tmp as u64) };
    return entity;
}

#[derive(Copy, Clone)]
struct RGB(u8, u8, u8);

impl UserData for RGB {}

#[derive(Clone)]
struct CreateFootSoliderArgs {
    pub name: String,
    pub x: f32,
    pub y: f32,
    pub reading_distance_meters: f32,
    pub weight_kg: f32,
    pub width_meters: f32,
    pub height_meters: f32,
    pub hat_rgb: RGB,
    pub body_rgb: RGB,
    pub update_interval_ms: u64,
    pub leader: Option<u128>,
}

impl UserData for CreateFootSoliderArgs {}

impl CreateFootSoliderArgs {
    fn populate_table(table: &mut Table, name: String) {
        table.set("name", name.clone()).unwrap();
        table.set("x", 0.0).unwrap();
        table.set("y", 0.0).unwrap();
        table.set("reading_distance_meters", 4.0).unwrap();
        table.set("weight_kg", 79.6).unwrap();
        table.set("width_meters", 0.5).unwrap();
        table.set("height_meters", 1.73).unwrap();
        table.set("hat_rgb", RGB(0, 0, 0)).unwrap();
        table.set("body_rgb", RGB(0, 0, 0)).unwrap();
        table.set("update_interval_ms", 33).unwrap();
    }

    fn from(table: Table) -> Self {
        let mut result = CreateFootSoliderArgs {
            name: table.get("name").unwrap(),
            x: table.get("x").unwrap(),
            y: table.get("y").unwrap(),
            reading_distance_meters: table.get("reading_distance_meters").unwrap(),
            weight_kg: table.get("weight_kg").unwrap(),
            width_meters: table.get("width_meters").unwrap(),
            height_meters: table.get("height_meters").unwrap(),
            hat_rgb: table.get("hat_rgb").unwrap(),
            body_rgb: table.get("body_rgb").unwrap(),
            update_interval_ms: table.get("update_interval_ms").unwrap(),
            leader: None,
        };

        if let Ok(leader) = table.get("leader") {
            result.leader = leader;
        }

        result
    }
}

fn create_foot_solider(
    col: &mut CollisionData,
    args: &CreateFootSoliderArgs,
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
    Identity,
) {
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
        Position { x: 0.0, y: 0.0 },
        Velocity { dx: 0.0, dy: 0.0 },
        Eyes {
            reading_distance: measurements::Length::from_meters(
                args.reading_distance_meters as f64,
            ),
        },
        Body {
            weight: measurements::Mass::from_kilograms(args.weight_kg as f64),
            width: Length::from_meters(args.width_meters as f64),
            height: Length::from_meters(args.height_meters as f64),
        },
        Render {
            fg: RGBA::from_u8(args.hat_rgb.0, args.hat_rgb.1, args.hat_rgb.2, 255),
            bg: RGBA::from_u8(args.body_rgb.0, args.body_rgb.1, args.body_rgb.2, 255),
        },
        Collision {
            body_handler,
            col_handler,
        },
        Direction { x: 0.0, y: -1.0 },
        UnitPerception {
            update_interval: Duration::from_millis(args.update_interval_ms),
            time_to_update: Duration::from_nanos(0),
            can_see: Vec::new(),
            dirty: false,
        },
        UnitDecision {
            current_state: UnitDecisionState::Starting,
            last_states: VecDeque::new(),
        },
        Knowledge::default(),
        Identity {
            name: args.name.clone(),
        },
    );
}

pub fn run_create_scene(scripting: &Scripting, world: &mut World, col: &mut CollisionData) {
    scripting.lua.context(|lua_ctx| {
        let globals = lua_ctx.globals();

        let _ = globals.set(
            "rgb",
            lua_ctx
                .create_function(|_, (r, g, b): (u8, u8, u8)| Ok(RGB(r, g, b)))
                .unwrap(),
        );

        let _ = globals.set(
            "createFootSoliderArgs",
            lua_ctx
                .create_function(|lua_ctx, name: String| {
                    let mut table = lua_ctx.create_table()?;
                    CreateFootSoliderArgs::populate_table(&mut table, name);
                    Ok(table)
                })
                .unwrap(),
        );

        let _ = lua_ctx.scope(|scope| {
            // We create a 'sketchy' Lua callback that holds a mutable reference to the variable
            // `rust_val`.  Outside of a `Context::scope` call, this would not be allowed
            // because it could be unsafe.

            let _ = lua_ctx
                .globals()
                .set(
                    "create_foot_solider",
                    scope
                        .create_function_mut(|_, table: Table| {
                            let args = CreateFootSoliderArgs::from(table);

                            let mut set = create_foot_solider(col, &args);
                            set.0.x = args.x;
                            set.0.y = args.y;
                            let entity = world.push(set);

                            let mut leader: Option<IdentityObject> = None;
                            if let Some(leader_id) = args.leader {
                                let entity = u128_as_entity(leader_id);

                                if let Ok(entry) = world.entry_mut(entity) {
                                    // access the entity's components, returns `None` if the entity does not have the component
                                    let body = entry.get_component::<Body>().unwrap();
                                    let render = entry.get_component::<Render>().unwrap();
                                    leader = Some(IdentityObject::new(body, render));
                                }
                            }

                            if let Ok(mut entry) = world.entry_mut(entity) {
                                // access the entity's components, returns `None` if the entity does not have the component
                                let col_com = entry.get_component::<Collision>().unwrap();

                                let mut collider =
                                    col.colliders.get_mut(col_com.col_handler).unwrap();
                                collider.user_data = entity_as_u128(entity);

                                if let Some(leader) = leader {
                                    let knowledge = entry.get_component_mut::<Knowledge>().unwrap();
                                    knowledge.leaders.push(leader);
                                }
                            }

                            Ok(entity_as_u128(entity))
                        })
                        .unwrap(),
                )
                .unwrap();

            lua_ctx
                .load(&load_script(Path::new("create_scene.lua")))
                .eval::<bool>()
                .expect("lua failure");
        });
    });
}
