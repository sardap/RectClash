#![deny(warnings)]
#![forbid(unsafe_code)]

use nannou::winit::event::VirtualKeyCode;
use std::collections::HashMap;

use legion::*;

use crate::components::InputCom;
use crate::world::SubWorld;

#[derive(Clone, Debug, PartialEq, Eq, Hash)]
pub enum InputKey {
    KeyLeft,
    KeyRight,
    KeyUp,
    KeyDown,
    KeyEqual,
    KeyMinus,
    Unknown,
}

impl From<VirtualKeyCode> for InputKey {
    fn from(key: VirtualKeyCode) -> Self {
        match key {
            VirtualKeyCode::Key1 => InputKey::Unknown,
            VirtualKeyCode::Key2 => InputKey::Unknown,
            VirtualKeyCode::Key3 => InputKey::Unknown,
            VirtualKeyCode::Key4 => InputKey::Unknown,
            VirtualKeyCode::Key5 => InputKey::Unknown,
            VirtualKeyCode::Key6 => InputKey::Unknown,
            VirtualKeyCode::Key7 => InputKey::Unknown,
            VirtualKeyCode::Key8 => InputKey::Unknown,
            VirtualKeyCode::Key9 => InputKey::Unknown,
            VirtualKeyCode::Key0 => InputKey::Unknown,
            VirtualKeyCode::A => InputKey::Unknown,
            VirtualKeyCode::B => InputKey::Unknown,
            VirtualKeyCode::C => InputKey::Unknown,
            VirtualKeyCode::D => InputKey::Unknown,
            VirtualKeyCode::E => InputKey::Unknown,
            VirtualKeyCode::F => InputKey::Unknown,
            VirtualKeyCode::G => InputKey::Unknown,
            VirtualKeyCode::H => InputKey::Unknown,
            VirtualKeyCode::I => InputKey::Unknown,
            VirtualKeyCode::J => InputKey::Unknown,
            VirtualKeyCode::K => InputKey::Unknown,
            VirtualKeyCode::L => InputKey::Unknown,
            VirtualKeyCode::M => InputKey::Unknown,
            VirtualKeyCode::N => InputKey::Unknown,
            VirtualKeyCode::O => InputKey::Unknown,
            VirtualKeyCode::P => InputKey::Unknown,
            VirtualKeyCode::Q => InputKey::Unknown,
            VirtualKeyCode::R => InputKey::Unknown,
            VirtualKeyCode::S => InputKey::Unknown,
            VirtualKeyCode::T => InputKey::Unknown,
            VirtualKeyCode::U => InputKey::Unknown,
            VirtualKeyCode::V => InputKey::Unknown,
            VirtualKeyCode::W => InputKey::Unknown,
            VirtualKeyCode::X => InputKey::Unknown,
            VirtualKeyCode::Y => InputKey::Unknown,
            VirtualKeyCode::Z => InputKey::Unknown,
            VirtualKeyCode::Escape => InputKey::Unknown,
            VirtualKeyCode::F1 => InputKey::Unknown,
            VirtualKeyCode::F2 => InputKey::Unknown,
            VirtualKeyCode::F3 => InputKey::Unknown,
            VirtualKeyCode::F4 => InputKey::Unknown,
            VirtualKeyCode::F5 => InputKey::Unknown,
            VirtualKeyCode::F6 => InputKey::Unknown,
            VirtualKeyCode::F7 => InputKey::Unknown,
            VirtualKeyCode::F8 => InputKey::Unknown,
            VirtualKeyCode::F9 => InputKey::Unknown,
            VirtualKeyCode::F10 => InputKey::Unknown,
            VirtualKeyCode::F11 => InputKey::Unknown,
            VirtualKeyCode::F12 => InputKey::Unknown,
            VirtualKeyCode::F13 => InputKey::Unknown,
            VirtualKeyCode::F14 => InputKey::Unknown,
            VirtualKeyCode::F15 => InputKey::Unknown,
            VirtualKeyCode::F16 => InputKey::Unknown,
            VirtualKeyCode::F17 => InputKey::Unknown,
            VirtualKeyCode::F18 => InputKey::Unknown,
            VirtualKeyCode::F19 => InputKey::Unknown,
            VirtualKeyCode::F20 => InputKey::Unknown,
            VirtualKeyCode::F21 => InputKey::Unknown,
            VirtualKeyCode::F22 => InputKey::Unknown,
            VirtualKeyCode::F23 => InputKey::Unknown,
            VirtualKeyCode::F24 => InputKey::Unknown,
            VirtualKeyCode::Snapshot => InputKey::Unknown,
            VirtualKeyCode::Scroll => InputKey::Unknown,
            VirtualKeyCode::Pause => InputKey::Unknown,
            VirtualKeyCode::Insert => InputKey::Unknown,
            VirtualKeyCode::Home => InputKey::Unknown,
            VirtualKeyCode::Delete => InputKey::Unknown,
            VirtualKeyCode::End => InputKey::Unknown,
            VirtualKeyCode::PageDown => InputKey::Unknown,
            VirtualKeyCode::PageUp => InputKey::Unknown,
            VirtualKeyCode::Left => InputKey::KeyLeft,
            VirtualKeyCode::Up => InputKey::KeyUp,
            VirtualKeyCode::Right => InputKey::KeyRight,
            VirtualKeyCode::Down => InputKey::KeyDown,
            VirtualKeyCode::Back => InputKey::Unknown,
            VirtualKeyCode::Return => InputKey::Unknown,
            VirtualKeyCode::Space => InputKey::Unknown,
            VirtualKeyCode::Compose => InputKey::Unknown,
            VirtualKeyCode::Caret => InputKey::Unknown,
            VirtualKeyCode::Numlock => InputKey::Unknown,
            VirtualKeyCode::Numpad0 => InputKey::Unknown,
            VirtualKeyCode::Numpad1 => InputKey::Unknown,
            VirtualKeyCode::Numpad2 => InputKey::Unknown,
            VirtualKeyCode::Numpad3 => InputKey::Unknown,
            VirtualKeyCode::Numpad4 => InputKey::Unknown,
            VirtualKeyCode::Numpad5 => InputKey::Unknown,
            VirtualKeyCode::Numpad6 => InputKey::Unknown,
            VirtualKeyCode::Numpad7 => InputKey::Unknown,
            VirtualKeyCode::Numpad8 => InputKey::Unknown,
            VirtualKeyCode::Numpad9 => InputKey::Unknown,
            VirtualKeyCode::NumpadAdd => InputKey::Unknown,
            VirtualKeyCode::NumpadDivide => InputKey::Unknown,
            VirtualKeyCode::NumpadDecimal => InputKey::Unknown,
            VirtualKeyCode::NumpadComma => InputKey::Unknown,
            VirtualKeyCode::NumpadEnter => InputKey::Unknown,
            VirtualKeyCode::NumpadEquals => InputKey::Unknown,
            VirtualKeyCode::NumpadMultiply => InputKey::Unknown,
            VirtualKeyCode::NumpadSubtract => InputKey::Unknown,
            VirtualKeyCode::AbntC1 => InputKey::Unknown,
            VirtualKeyCode::AbntC2 => InputKey::Unknown,
            VirtualKeyCode::Apostrophe => InputKey::Unknown,
            VirtualKeyCode::Apps => InputKey::Unknown,
            VirtualKeyCode::Asterisk => InputKey::Unknown,
            VirtualKeyCode::At => InputKey::Unknown,
            VirtualKeyCode::Ax => InputKey::Unknown,
            VirtualKeyCode::Backslash => InputKey::Unknown,
            VirtualKeyCode::Calculator => InputKey::Unknown,
            VirtualKeyCode::Capital => InputKey::Unknown,
            VirtualKeyCode::Colon => InputKey::Unknown,
            VirtualKeyCode::Comma => InputKey::Unknown,
            VirtualKeyCode::Convert => InputKey::Unknown,
            VirtualKeyCode::Equals => InputKey::KeyEqual,
            VirtualKeyCode::Grave => InputKey::Unknown,
            VirtualKeyCode::Kana => InputKey::Unknown,
            VirtualKeyCode::Kanji => InputKey::Unknown,
            VirtualKeyCode::LAlt => InputKey::Unknown,
            VirtualKeyCode::LBracket => InputKey::Unknown,
            VirtualKeyCode::LControl => InputKey::Unknown,
            VirtualKeyCode::LShift => InputKey::Unknown,
            VirtualKeyCode::LWin => InputKey::Unknown,
            VirtualKeyCode::Mail => InputKey::Unknown,
            VirtualKeyCode::MediaSelect => InputKey::Unknown,
            VirtualKeyCode::MediaStop => InputKey::Unknown,
            VirtualKeyCode::Minus => InputKey::KeyMinus,
            VirtualKeyCode::Mute => InputKey::Unknown,
            VirtualKeyCode::MyComputer => InputKey::Unknown,
            VirtualKeyCode::NavigateForward => InputKey::Unknown,
            VirtualKeyCode::NavigateBackward => InputKey::Unknown,
            VirtualKeyCode::NextTrack => InputKey::Unknown,
            VirtualKeyCode::NoConvert => InputKey::Unknown,
            VirtualKeyCode::OEM102 => InputKey::Unknown,
            VirtualKeyCode::Period => InputKey::Unknown,
            VirtualKeyCode::PlayPause => InputKey::Unknown,
            VirtualKeyCode::Plus => InputKey::Unknown,
            VirtualKeyCode::Power => InputKey::Unknown,
            VirtualKeyCode::PrevTrack => InputKey::Unknown,
            VirtualKeyCode::RAlt => InputKey::Unknown,
            VirtualKeyCode::RBracket => InputKey::Unknown,
            VirtualKeyCode::RControl => InputKey::Unknown,
            VirtualKeyCode::RShift => InputKey::Unknown,
            VirtualKeyCode::RWin => InputKey::Unknown,
            VirtualKeyCode::Semicolon => InputKey::Unknown,
            VirtualKeyCode::Slash => InputKey::Unknown,
            VirtualKeyCode::Sleep => InputKey::Unknown,
            VirtualKeyCode::Stop => InputKey::Unknown,
            VirtualKeyCode::Sysrq => InputKey::Unknown,
            VirtualKeyCode::Tab => InputKey::Unknown,
            VirtualKeyCode::Underline => InputKey::Unknown,
            VirtualKeyCode::Unlabeled => InputKey::Unknown,
            VirtualKeyCode::VolumeDown => InputKey::Unknown,
            VirtualKeyCode::VolumeUp => InputKey::Unknown,
            VirtualKeyCode::Wake => InputKey::Unknown,
            VirtualKeyCode::WebBack => InputKey::Unknown,
            VirtualKeyCode::WebFavorites => InputKey::Unknown,
            VirtualKeyCode::WebForward => InputKey::Unknown,
            VirtualKeyCode::WebHome => InputKey::Unknown,
            VirtualKeyCode::WebRefresh => InputKey::Unknown,
            VirtualKeyCode::WebSearch => InputKey::Unknown,
            VirtualKeyCode::WebStop => InputKey::Unknown,
            VirtualKeyCode::Yen => InputKey::Unknown,
            VirtualKeyCode::Copy => InputKey::Unknown,
            VirtualKeyCode::Paste => InputKey::Unknown,
            VirtualKeyCode::Cut => InputKey::Unknown,
        }
    }
}

#[derive(Clone, Debug, Default)]
pub struct Inputs {
    pub pressed: HashMap<InputKey, bool>,
    pub last_tick_pressed: HashMap<InputKey, bool>,
}

#[system]
#[write_component(InputCom)]
pub fn update_input(world: &mut SubWorld, #[resource] inputs: &mut Inputs) {
    let mut query = <&mut InputCom>::query();

    for input_com in query.iter_mut(world) {
        for (command, keys) in &input_com.key_map {
            for key in keys {
                if let Some(pressed) = inputs.pressed.get(&key) {
                    if let Some(last_tick_pressed) = inputs.last_tick_pressed.get(&key) {
                        input_com
                            .released
                            .insert(*command, !pressed && *last_tick_pressed);
                    }
                    input_com.pressed.insert(*command, *pressed);
                }
            }
        }
    }

    inputs.last_tick_pressed = inputs.pressed.clone();
}
