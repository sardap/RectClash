#![deny(warnings)]
#![forbid(unsafe_code)]

use std::{collections::HashMap, fs, path::Path};

use rlua::Lua;

#[derive(Default)]
pub struct Scripts {
    pub loaded_scripts: HashMap<String, String>,
}

impl Scripts {
    pub fn load_script(&mut self, script: &Path) -> String {
        let key = script.to_str().unwrap().to_string();
        let script = load_script(script);
        self.loaded_scripts.insert(key.clone(), script);
        return key;
    }
}

pub fn load_script(script: &Path) -> String {
    let path = Path::new("data").join("scripts").join(script);
    return fs::read_to_string(path).expect("Something went wrong reading the file");
}

pub fn create_lua() -> Lua {
    let lua = Lua::new();
    lua.context(|_lua_ctx| {});
    lua
}
