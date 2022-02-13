#![deny(warnings)]
#![forbid(unsafe_code)]
#![allow(unused_must_use)]

use crate::scripting::create_lua;
use crate::world::SubWorld;
use crate::{components::*, scripting::Scripts};

use legion::*;
use rand::{prelude::StdRng, Rng};
use rlua::{Table, Value};

#[system]
#[write_component(UnitDecision)]
#[write_component(Direction)]
#[read_component(UnitPerception)]
#[read_component(Knowledge)]
pub fn update_decision(
    world: &mut SubWorld,
    #[resource] rng: &mut StdRng,
    #[resource] scripts: &mut Scripts,
) {
    let _: bool = rng.gen();

    let mut query = <(
        &mut UnitDecision,
        &mut Direction,
        &UnitPerception,
        &Knowledge,
    )>::query();

    let lua = create_lua();

    for (unit_decision, direction, unit_perception, knowledge) in query.iter_mut(world) {
        if !unit_perception.dirty {
            continue;
        }

        let script = scripts
            .loaded_scripts
            .get(&unit_decision.script_key)
            .expect("script not loaded");

        lua.context(|lua_ctx| {
            let globals = lua_ctx.globals();

            let save_table = lua_ctx.create_table().unwrap();
            for (key, value) in &unit_decision.internal_table.data {
                save_table.set(key.clone(), value.clone());
            }
            globals.set("save_table", save_table).unwrap();
            globals.set("perception", unit_perception.clone());

            lua_ctx.scope(|scope: &rlua::Scope| {
                globals.set(
                    "get_leader",
                    scope
                        .create_function_mut(|_, idx: i32| {
                            if knowledge.leaders.len() > 0 {
                                return Ok(Some(knowledge.leaders[idx as usize]));
                            }
                            return Ok(None);
                        })
                        .unwrap(),
                );

                globals.set(
                    "direction_rotate_clockwise",
                    scope
                        .create_function_mut(|_, ()| {
                            direction.rotate_clockwise();
                            return Ok(());
                        })
                        .unwrap(),
                );

                lua_ctx.load(script).eval::<bool>().expect("lua failure");
            });

            let mut result = InternalTable::default();

            let save_table: Table = globals.get("save_table").unwrap();
            for pair in save_table.pairs::<Value, Value>() {
                let (key, value) = pair.unwrap();
                result.data.insert(
                    InternalTableValue::from(key),
                    InternalTableValue::from(value),
                );
            }

            unit_decision.internal_table = result;
        });

        // match unit_decision.current_state {
        //     UnitDecisionState::Starting => {
        //         change_state(unit_decision, UnitDecisionState::Searching)
        //     }
        //     UnitDecisionState::Searching => {
        //         if !unit_perception.dirty {
        //             return;
        //         }

        //         if knowledge.leaders.len() <= 0 {
        //             change_state(unit_decision, UnitDecisionState::Leading);
        //             return;
        //         }

        //         let target = &knowledge.leaders[0];

        //         let min_dist = 5.0;

        //         let mut best = f64::MAX;
        //         let mut best_object: Option<&VisionObject> = None;
        //         for vision_object in &unit_perception.can_see {
        //             let dist = target.distance(vision_object);
        //             if dist < best && dist <= min_dist {
        //                 best = dist;
        //                 best_object = Some(vision_object);
        //             }
        //         }

        //         // Min distance
        //         if let Some(object) = best_object {
        //             println!(
        //                 "Found leader at {:?} {:?}",
        //                 object.position.x, object.position.y
        //             );
        //             change_state(unit_decision, UnitDecisionState::Following);
        //         } else {
        //             // look around until you find a leader
        //             direction.rotate_clockwise()
        //         }
        //     }
        //     UnitDecisionState::Leading => {}
        //     UnitDecisionState::Following => {}
        // }
    }
}
