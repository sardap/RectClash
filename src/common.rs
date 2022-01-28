#![deny(warnings)]
#![forbid(unsafe_code)]

use std::time::Duration;

#[derive(Default)]
pub struct DeltaTime(pub Duration);

impl DeltaTime {
    pub fn elapsed_seconds(&self) -> f32 {
        self.0.as_secs() as f32 + self.0.subsec_nanos() as f32 * 1e-9
    }

    pub fn duration(&self) -> Duration {
        self.0
    }
}
