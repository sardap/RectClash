#![deny(warnings)]
#![forbid(unsafe_code)]

use std::{fs, path::Path};

use rlua::Lua;

pub struct Scripting {
    pub lua: Lua,
}

pub fn load_script(script: &Path) -> String {
    let path = Path::new("data").join("scripts").join(script);
    return fs::read_to_string(path).expect("Something went wrong reading the file");
}

impl Scripting {
    pub fn new() -> Self {
        let lua = Lua::new();

        lua.context(|lua_ctx| {
            lua_ctx
                .load(&load_script(Path::new("test.lua")))
                .eval::<bool>()
                .expect("lua failure");
        });

        return Scripting { lua };
    }
}
