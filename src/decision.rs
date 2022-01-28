#![deny(warnings)]
#![forbid(unsafe_code)]

use crate::components::*;

use legion::*;
use rand::{prelude::StdRng, Rng};

const MAX_LAST_STATES: usize = 5;

fn change_state(unit_decision: &mut UnitDecision, new_state: UnitDecisionState) {
    if unit_decision.last_states.len() >= MAX_LAST_STATES {
        unit_decision.last_states.pop_front();
    }

    unit_decision
        .last_states
        .push_back(unit_decision.current_state);
    unit_decision.current_state = new_state;
}

#[system(for_each)]
pub fn update_decision(
    unit_decision: &mut UnitDecision,
    unit_perception: &UnitPerception,
    direction: &mut Direction,
    knowledge: &Knowledge,
    #[resource] rng: &mut StdRng,
) {
    let _fart: bool = rng.gen();
    match unit_decision.current_state {
        UnitDecisionState::Starting => change_state(unit_decision, UnitDecisionState::Searching),
        UnitDecisionState::Searching => {
            if !unit_perception.dirty {
                return;
            }

            if knowledge.leaders.len() <= 0 {
                change_state(unit_decision, UnitDecisionState::Leading);
                return;
            }

            let target = &knowledge.leaders[0];

            let mut best = f64::MAX;
            let mut best_object: Option<&VisionObject> = None;
            for vision_object in &unit_perception.can_see {
                let dist = target.distance(vision_object);
                if dist < best {
                    best = dist;
                    best_object = Some(vision_object);
                }
            }

            if let Some(object) = best_object {
                println!(
                    "Found leader at {:?} {:?}",
                    object.position.x, object.position.y
                );
                change_state(unit_decision, UnitDecisionState::Following);
            } else {
                // look around until you find a leader
                direction.rotate_clockwise()
            }
        }
        UnitDecisionState::Leading => {}
        UnitDecisionState::Following => {}
    }
}
