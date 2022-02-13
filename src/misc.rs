#![deny(warnings)]
#![forbid(unsafe_code)]

use crate::{components::RGB, measurements::Length};

pub fn unit_to_length(val: f32) -> Length {
    Length::from_meters(val as f64)
}

pub fn length_to_unit(val: Length) -> f32 {
    Length::as_meters(&val) as f32
}

struct LAB {
    pub l: f32,
    pub a: f32,
    pub b: f32,
}

fn rgb_to_lab(rgb: &RGB) -> LAB {
    //  r = (r > 0.04045) ? Math.pow((r + 0.055) / 1.055, 2.4) : r / 12.92;
    let r = if rgb.r > 0.04045 {
        ((rgb.r + 0.055) / 1.055).powf(2.4)
    } else {
        rgb.r / 12.92
    };

    // g = (g > 0.04045) ? Math.pow((g + 0.055) / 1.055, 2.4) : g / 12.92;
    let g = if rgb.g > 0.04045 {
        ((rgb.g + 0.055) / 1.055).powf(2.4)
    } else {
        rgb.g / 12.92
    };

    // b = (b > 0.04045) ? Math.pow((b + 0.055) / 1.055, 2.4) : b / 12.92;
    let b = if rgb.b > 0.04045 {
        ((rgb.b + 0.055) / 1.055).powf(2.4)
    } else {
        rgb.b / 12.92
    };

    // x = (r * 0.4124 + g * 0.3576 + b * 0.1805) / 0.95047;
    let mut x = (r * 0.4124 + g * 0.3576 + b * 0.1805) / 0.95047;

    // y = (r * 0.2126 + g * 0.7152 + b * 0.0722) / 1.00000;
    let mut y = (r * 0.2126 + g * 0.7152 + b * 0.0722) / 1.00000;

    // z = (r * 0.0193 + g * 0.1192 + b * 0.9505) / 1.08883;
    let mut z = (r * 0.0193 + g * 0.1192 + b * 0.9505) / 1.08883;

    // x = (x > 0.008856) ? Math.pow(x, 1/3) : (7.787 * x) + 16/116;
    x = if x > 0.008856 {
        x.powf(1.0 / 3.0)
    } else {
        (7.787 * x) + 16.0 / 116.0
    };

    // y = (y > 0.008856) ? Math.pow(y, 1/3) : (7.787 * y) + 16/116;
    y = if y > 0.008856 {
        y.powf(1.0 / 3.0)
    } else {
        (7.787 * y) + 16.0 / 116.0
    };

    // z = (z > 0.008856) ? Math.pow(z, 1/3) : (7.787 * z) + 16/116;
    z = if z > 0.008856 {
        z.powf(1.0 / 3.0)
    } else {
        (7.787 * z) + 16.0 / 116.0
    };

    // return [(116 * y) - 16, 500 * (x - y), 200 * (y - z)];
    return LAB {
        l: (116.0 * y) - 16.0,
        a: 500.0 * (x - y),
        b: 200.0 * (y - z),
    };
}

pub fn color_dist(l: &RGB, r: &RGB) -> f64 {
    let lab_a = rgb_to_lab(l);
    let lab_b = rgb_to_lab(r);
    let delta_l = lab_a.l - lab_b.l;
    let delta_a = lab_a.a - lab_b.a;
    let delta_b = lab_a.b - lab_b.b;
    let c1 = (lab_a.a * lab_a.a + lab_a.b * lab_a.b).sqrt();
    let c2 = (lab_b.a * lab_b.a + lab_b.b * lab_b.b).sqrt();
    let delta_c = c1 - c2;
    let mut delta_h = delta_a * delta_a + delta_b * delta_b - delta_c * delta_c;
    delta_h = if delta_h < 0.0 { 0.0 } else { (delta_h).sqrt() };
    let sc = 1.0 + 0.045 * c1;
    let sh = 1.0 + 0.015 * c1;
    let delta_lklsl = delta_l / (1.0);
    let delta_ckcsc = delta_c / (sc);
    let delta_hkhsh = delta_h / (sh);
    let i = delta_lklsl * delta_lklsl + delta_ckcsc * delta_ckcsc + delta_hkhsh * delta_hkhsh;

    if i < 0.0 {
        return 0.0;
    }

    return (i as f64).sqrt();
}
