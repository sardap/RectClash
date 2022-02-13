use proc_macro::{self, TokenStream};
use quote::quote;
use syn::{parse_macro_input, DataEnum, DataUnion, DeriveInput, FieldsNamed, FieldsUnnamed};

#[proc_macro_derive(ToLuaTable)]
pub fn populate_table(input: TokenStream) -> TokenStream {
    let DeriveInput { ident, data, .. } = parse_macro_input!(input);

    let idents = match &data {
        syn::Data::Struct(s) => match &s.fields {
            syn::Fields::Named(FieldsNamed { named, .. }) => {
                let mut result = vec![];
                for f in named.iter().filter(|f| f.ident.is_some()) {
                    result.push(f.ident.clone().unwrap());
                }
                result
            }
            syn::Fields::Unnamed(FieldsUnnamed { .. }) => todo!(),
            syn::Fields::Unit => todo!(),
        },
        syn::Data::Enum(DataEnum { .. }) => todo!(),
        syn::Data::Union(DataUnion {
            fields: FieldsNamed { .. },
            ..
        }) => todo!(),
    };

    let mut keys = Vec::new();
    for ident in idents.iter() {
        keys.push(ident.to_string());
    }

    let output = quote! {
        // Change this to use the toLua trait instend
        impl rlua::ToLua<'lua> for #ident {
            fn to_lua(self, lua: rlua::Context<'lua>) -> rlua::Result<Value<'lua>> {
                let table = lua.create_table().unwrap();
                #[allow(unused_mut)]
                #(
                    let  _ = table.set(#keys, self.#idents.clone());
                )*
                Ok(rlua::Value::Table(table))
            }
        }
    };

    output.into()
}

#[proc_macro_derive(FromLuaTable)]
pub fn from_lua_table(input: TokenStream) -> TokenStream {
    let DeriveInput { ident, data, .. } = parse_macro_input!(input);

    let idents = match &data {
        syn::Data::Struct(s) => match &s.fields {
            syn::Fields::Named(FieldsNamed { named, .. }) => {
                let mut result = vec![];
                for f in named.iter().filter(|f| f.ident.is_some()) {
                    result.push(f.ident.clone().unwrap());
                }
                result
            }
            syn::Fields::Unnamed(FieldsUnnamed { .. }) => todo!(),
            syn::Fields::Unit => todo!(),
        },
        syn::Data::Enum(DataEnum { .. }) => todo!(),
        syn::Data::Union(DataUnion {
            fields: FieldsNamed { .. },
            ..
        }) => todo!(),
    };

    let mut keys = Vec::new();
    for ident in idents.iter() {
        keys.push(ident.to_string());
    }

    let output = quote! {
        // Change this to use the toLua trait instend
        impl rlua::FromLua<'lua> for #ident {
            fn from_lua(lua_value: Value<'lua>, _: rlua::Context<'lua>) -> rlua::Result<Self> {
                let mut result = Self::default();
                if let rlua::Value::Table(table) = lua_value {
                    #(
                        result.#idents = table.get(#keys).unwrap();
                    )*
                }
                Ok(result)
            }
        }
    };

    output.into()
}
