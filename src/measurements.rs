#![allow(dead_code)]
#![deny(warnings)]
#![forbid(unsafe_code)]
use my_derive::{FromLuaTable, ToLuaTable};
use rlua::Value;
use std::convert::Into;
use std::ops::{Add, Div, Mul, Sub};

#[derive(Clone, Copy, Debug, PartialEq, PartialOrd, Default, ToLuaTable, FromLuaTable)]
pub struct Mass {
    milligrams: f64,
}

impl Mass {
    pub fn from_milligrams(milligrams: f64) -> Self {
        Mass { milligrams }
    }

    pub fn as_milligrams(&self) -> f64 {
        self.milligrams
    }

    pub fn from_grams(grams: f64) -> Self {
        Mass {
            milligrams: grams * 1000.0,
        }
    }

    pub fn as_grams(&self) -> f64 {
        self.milligrams / 1000.0
    }

    pub fn from_kilograms(kg: f64) -> Self {
        Mass::from_grams(kg * 1000.0)
    }

    pub fn as_kilograms(&self) -> f64 {
        self.as_grams() / 1000.0
    }

    pub fn from_tonnes(tones: f64) -> Self {
        Mass::from_kilograms(tones * 1000.0)
    }

    pub fn as_tonnes(&self) -> f64 {
        self.as_kilograms() / 1000.0
    }
}

impl Add for Mass {
    type Output = Self;

    fn add(self, other: Self) -> Self {
        Self {
            milligrams: self.milligrams + other.milligrams,
        }
    }
}

impl Sub for Mass {
    type Output = Self;

    fn sub(self, other: Self) -> Self {
        Self {
            milligrams: self.milligrams - other.milligrams,
        }
    }
}

impl Div for Mass {
    type Output = Self;

    fn div(self, other: Self) -> Self {
        Self {
            milligrams: self.milligrams / other.milligrams,
        }
    }
}

impl Mul for Mass {
    type Output = Self;

    fn mul(self, other: Self) -> Self {
        Self {
            milligrams: self.milligrams * other.milligrams,
        }
    }
}

impl Into<f64> for Mass {
    fn into(self) -> f64 {
        self.as_milligrams()
    }
}

impl Into<f32> for Mass {
    fn into(self) -> f32 {
        self.as_milligrams() as f32
    }
}

#[derive(Clone, Copy, Debug, PartialEq, PartialOrd, Default, ToLuaTable, FromLuaTable)]
pub struct Length {
    millimeters: f64,
}

impl Length {
    pub fn from_millimeters(millimeters: f64) -> Self {
        Length {
            millimeters: millimeters,
        }
    }

    pub fn as_millimeters(&self) -> f64 {
        self.millimeters
    }

    pub fn from_centimeters(centimeters: f64) -> Self {
        Length {
            millimeters: centimeters * 10.0,
        }
    }

    pub fn as_centimeters(&self) -> f64 {
        self.millimeters / 10.0
    }

    pub fn from_meters(meters: f64) -> Self {
        Length::from_centimeters(meters * 100.0)
    }

    pub fn as_meters(&self) -> f64 {
        self.as_centimeters() / 100.0
    }
}

impl Add for Length {
    type Output = Self;

    fn add(self, other: Self) -> Self {
        Self {
            millimeters: self.millimeters + other.millimeters,
        }
    }
}

impl Sub for Length {
    type Output = Self;

    fn sub(self, other: Self) -> Self {
        Self {
            millimeters: self.millimeters - other.millimeters,
        }
    }
}

impl Div for Length {
    type Output = Self;

    fn div(self, other: Self) -> Self {
        Self {
            millimeters: self.millimeters / other.millimeters,
        }
    }
}

impl Mul for Length {
    type Output = Self;

    fn mul(self, other: Self) -> Self {
        Self {
            millimeters: self.millimeters * other.millimeters,
        }
    }
}

impl Add<f64> for Length {
    type Output = Self;

    fn add(self, other: f64) -> Self {
        Self {
            millimeters: self.millimeters + other,
        }
    }
}

impl Sub<f64> for Length {
    type Output = Self;

    fn sub(self, other: f64) -> Self {
        Self {
            millimeters: self.millimeters - other,
        }
    }
}

impl Div<f64> for Length {
    type Output = Self;

    fn div(self, other: f64) -> Self {
        Self {
            millimeters: self.millimeters / other,
        }
    }
}

impl Mul<f64> for Length {
    type Output = Self;

    fn mul(self, other: f64) -> Self {
        Self {
            millimeters: self.millimeters * other,
        }
    }
}

impl Into<f64> for Length {
    fn into(self) -> f64 {
        self.as_millimeters()
    }
}

impl Into<f32> for Length {
    fn into(self) -> f32 {
        self.as_millimeters() as f32
    }
}
