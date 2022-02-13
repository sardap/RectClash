#![deny(warnings)]
#![forbid(unsafe_code)]

use std::{collections::HashMap, hash::Hash, time::Duration};

use my_derive::{FromLuaTable, ToLuaTable};
use rapier2d::prelude::{ColliderHandle, RigidBodyHandle};
use rltk::prelude::*;
use rlua::{FromLua, Number, ToLua, Value};

use crate::{
    input::InputKey,
    measurements::{Length, Mass},
    misc::{color_dist, unit_to_length},
};

// a component is any type that is 'static, sized, send and sync
#[derive(Clone, Copy, Debug, PartialEq, Default, ToLuaTable, FromLuaTable)]
pub struct Position {
    pub x: f32,
    pub y: f32,
}

impl Position {
    pub fn distance_between(&self, other: &Position) -> Length {
        unit_to_length(((self.x - other.x).powf(2.0) + (self.y - other.y).powf(2.0)).sqrt())
    }
}

#[derive(Clone, Copy, Debug, PartialEq, Default)]
pub struct Direction {
    pub x: f32,
    pub y: f32,
}

pub enum Facing {
    Up,
    Left,
    Down,
    Right,
}

impl Direction {
    pub fn as_facing(&self) -> Facing {
        if self.x == 0.0 && self.y == -1.0 {
            return Facing::Up;
        }
        if self.x == -1.0 && self.y == 0.0 {
            return Facing::Left;
        }
        if self.x == 0.0 && self.y == 1.0 {
            return Facing::Down;
        }

        return Facing::Right;
    }

    pub fn face(&mut self, facing: Facing) {
        match facing {
            Facing::Up => {
                self.x = 0.0;
                self.y = -1.0;
            }
            Facing::Down => {
                self.x = 0.0;
                self.y = 1.0;
            }
            Facing::Right => {
                self.x = 1.0;
                self.y = 0.0;
            }
            Facing::Left => {
                self.x = -1.0;
                self.y = 0.0;
            }
        }
    }

    pub fn rotate_clockwise(&mut self) {
        match self.as_facing() {
            Facing::Up => self.face(Facing::Right),
            Facing::Right => self.face(Facing::Down),
            Facing::Down => self.face(Facing::Left),
            Facing::Left => self.face(Facing::Up),
        }
    }
}

impl rlua::UserData for Direction {}
impl rlua::UserData for &mut Direction {}

#[derive(Clone, Copy, Debug, PartialEq)]
pub struct Velocity {
    pub dx: f32,
    pub dy: f32,
}

#[derive(Clone, Copy, Debug, PartialEq)]
pub struct Render {
    pub fg: RGBA,
    pub bg: RGBA,
}

#[derive(Clone, Copy, Debug, PartialEq)]
pub struct Body {
    pub weight: Mass,
    pub width: Length,
    pub height: Length,
}

#[derive(Clone, Copy, Debug, PartialEq)]
pub struct Eyes {
    pub reading_distance: Length,
}

#[derive(Clone, Copy, Debug, PartialEq, Default, ToLuaTable, FromLuaTable)]
pub struct RGB {
    pub r: f32,
    pub g: f32,
    pub b: f32,
}

impl RGB {
    pub fn new(r: f32, g: f32, b: f32) -> Self {
        RGB { r: r, g: g, b: b }
    }
}

impl From<rltk::RGBA> for RGB {
    fn from(rgba: rltk::RGBA) -> Self {
        RGB {
            r: rgba.r,
            g: rgba.g,
            b: rgba.b,
        }
    }
}

#[derive(Clone, Copy, Debug, PartialEq, Default, ToLuaTable, FromLuaTable)]
pub struct Appearance {
    pub body: RGB,
    pub hat: RGB,
}

impl Appearance {
    pub fn new(hat: RGB, body: RGB) -> Self {
        Appearance { body, hat }
    }
}

#[derive(Clone, Copy, Debug, PartialEq, Default, ToLuaTable, FromLuaTable)]
pub struct VisionObject {
    pub approximant_weight: Option<Mass>,
    pub approximant_height: Option<Length>,
    pub approximant_appearance: Option<Appearance>,
    pub distance: Option<Length>,
    pub position: Position,
}

#[derive(Clone, Debug, PartialEq)]
pub struct UnitPerception {
    pub update_interval: Duration,
    pub time_to_update: Duration,
    pub can_see: Vec<VisionObject>,
    pub dirty: bool,
}

impl rlua::ToLua<'lua> for UnitPerception {
    fn to_lua(self, lua: rlua::Context<'lua>) -> rlua::Result<Value<'lua>> {
        let table = lua.create_table().unwrap();
        table.set("update_interval", self.update_interval.as_millis())?;
        table.set("time_to_update", self.time_to_update.as_millis())?;
        table.set("can_see", self.can_see)?;
        table.set("dirty", self.dirty)?;
        Ok(rlua::Value::Table(table))
    }
}

#[derive(Debug, PartialEq, Eq, Clone, Hash)]
pub enum InternalTableValue {
    String(String),
    Bool(bool),
    Float([u8; 8]),
    Nil,
}

impl From<Value<'lua>> for InternalTableValue {
    fn from(val: Value) -> Self {
        match val {
            Value::Nil => InternalTableValue::Nil,
            Value::Boolean(val) => InternalTableValue::Bool(val),
            Value::LightUserData(_) => todo!(),
            Value::Integer(val) => InternalTableValue::Float((val as f64).to_ne_bytes()),
            Value::Number(val) => InternalTableValue::Float(val.to_ne_bytes()),
            Value::String(val) => InternalTableValue::String(val.to_str().unwrap().to_string()),
            Value::Table(_) => todo!(),
            Value::Function(_) => todo!(),
            Value::Thread(_) => todo!(),
            Value::UserData(_) => todo!(),
            Value::Error(_) => todo!(),
        }
    }
}

impl<'lua> ToLua<'lua> for InternalTableValue {
    fn to_lua(self, lua: rlua::Context<'lua>) -> rlua::Result<Value<'lua>> {
        match self {
            InternalTableValue::String(val) => {
                Ok(Value::String(lua.create_string(val.as_str()).unwrap()))
            }
            InternalTableValue::Bool(val) => Ok(Value::Boolean(val)),
            InternalTableValue::Float(val) => Ok(Value::Number(f64::from_ne_bytes(val) as Number)),
            InternalTableValue::Nil => Ok(Value::Nil),
        }
    }
}

#[derive(Clone, Debug, PartialEq, Default, Eq)]
pub struct InternalTable {
    pub data: HashMap<InternalTableValue, InternalTableValue>,
}

#[derive(Clone, Debug, PartialEq)]
pub struct UnitDecision {
    pub script_key: String,
    pub internal_table: InternalTable,
}

#[derive(Clone, Copy, Debug, PartialEq)]
pub struct IdentityObject {
    pub weight: Mass,
    pub height: Length,
    pub appearance: Appearance,
}

impl IdentityObject {
    pub fn new(body: &Body, render: &Render) -> Self {
        return IdentityObject {
            weight: body.weight,
            height: body.height,
            appearance: Appearance::new(RGB::from(render.fg), RGB::from(render.bg)),
        };
    }

    pub fn distance(&self, vision_object: &VisionObject) -> f64 {
        let mut result = 0.0;

        result += (self.weight - vision_object.approximant_weight.unwrap())
            .as_milligrams()
            .abs();

        result += (self.height - vision_object.approximant_height.unwrap())
            .as_millimeters()
            .abs();

        result += color_dist(
            &self.appearance.body,
            &vision_object.approximant_appearance.unwrap().body,
        );

        {
            let mut hat_dist = color_dist(
                &self.appearance.hat,
                &vision_object.approximant_appearance.unwrap().hat,
            );
            if hat_dist != 0.0 {
                hat_dist /= 2.0;
            }
            result += hat_dist;
        }

        return result;
    }
}

impl rlua::UserData for IdentityObject {
    fn add_methods<'lua, M: rlua::UserDataMethods<'lua, Self>>(methods: &mut M) {
        methods.add_method(
            "distance",
            |lua_ctx, identity_object: &IdentityObject, vision_object_table: rlua::Table| {
                let val = rlua::Value::Table(vision_object_table);
                if let Ok(vision_object) = VisionObject::from_lua(val, lua_ctx) {
                    let dist = identity_object.distance(&vision_object);
                    return Ok(dist);
                }

                Ok(-1.0)
            },
        );
    }
}

#[derive(Clone, Debug, PartialEq, Default)]
pub struct Knowledge {
    pub leaders: Vec<IdentityObject>,
    pub comrades: Vec<IdentityObject>,
}

#[derive(Clone, Copy, Debug, PartialEq)]
pub struct Collision {
    pub body_handler: RigidBodyHandle,
    pub col_handler: ColliderHandle,
}

#[derive(Clone, Debug, PartialEq)]
pub struct Camera {
    pub speed: Length,
    pub scale: f32,
}

#[derive(Clone, Copy, Debug, PartialEq, Eq, Hash)]
pub enum InputCommand {
    MoveLeft,
    MoveRight,
    MoveUp,
    MoveDown,
    ScaleUp,
    ScaleDown,
}

#[derive(Clone, Debug, PartialEq)]
pub struct InputCom {
    pub key_map: HashMap<InputCommand, Vec<InputKey>>,
    pub pressed: HashMap<InputCommand, bool>,
    pub released: HashMap<InputCommand, bool>,
}

impl InputCom {
    pub fn default() -> Self {
        let key_map = HashMap::from([
            (InputCommand::MoveRight, vec![InputKey::KeyRight]),
            (InputCommand::MoveLeft, vec![InputKey::KeyLeft]),
            (InputCommand::MoveDown, vec![InputKey::KeyDown]),
            (InputCommand::MoveUp, vec![InputKey::KeyUp]),
            (InputCommand::ScaleUp, vec![InputKey::KeyEqual]),
            (InputCommand::ScaleDown, vec![InputKey::KeyMinus]),
        ]);

        let mut pressed = HashMap::<InputCommand, bool>::new();

        for (command, _) in &key_map {
            pressed.insert(*command, false);
        }

        return InputCom {
            key_map,
            pressed,
            released: HashMap::new(),
        };
    }
}

#[derive(Clone, Debug, PartialEq)]
pub struct Identity {
    pub name: String,
}
