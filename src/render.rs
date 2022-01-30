#![deny(warnings)]
#![forbid(unsafe_code)]

use euclid::default::{Point2D, Transform2D};
use legion::*;
use nannou::{color::WHITE, prelude::Vec2, App, Frame};

use crate::components::Camera;
use crate::{
    components::{Body, Direction, Position, Render},
    misc::length_to_unit,
};

enum Layers {
    Background = 0,
    Units = 1,
    _Overlay = 2,
}

impl Layers {
    fn value(self) -> f32 {
        self as i32 as f32
    }
}

pub fn render(world: &World, app: &App, frame: &Frame) {
    let draw = app.draw();

    // type DrawCmd = fn(draw: &mut Draw);
    // let draw_commands = Vec::<DrawCmd>::new();

    // Clear the background
    draw.background().color(WHITE);

    let camera = <(&Camera, &Position)>::query().iter(world).next().unwrap();

    let draw = draw.scale(camera.0.scale);

    let tx = camera.1.x;
    let ty = camera.1.y;
    let viewport = Transform2D::new(1.0, 0.0, 0.0, 1.0, tx, ty);

    let mut query = <(&Position, &Body, &Render, &Direction)>::query();
    for (position, body, render, _direction) in query.iter(world) {
        let mut point = Point2D::new(position.x, position.y);
        let width = length_to_unit(body.width);

        point = viewport.transform_point(point);

        draw.rect()
            .x_y(point.x, point.y)
            .rgb(render.bg.r, render.bg.g, render.bg.b)
            .w_h(width, width)
            .z(Layers::Units.value());
        draw.rect()
            .x_y(point.x, point.y)
            .rgb(render.fg.r, render.fg.g, render.fg.b)
            .w_h(width / 2.0, width / 2.0)
            .z(Layers::Units.value());
    }

    // Overlay
    for (position, _body, _render, direction) in query.iter(world) {
        let mut point = Point2D::new(position.x, position.y);

        point = viewport.transform_point(point);

        draw.line()
            .weight(0.25)
            .caps_round()
            .rgba8(238, 232, 170, 128)
            .points(
                Vec2::new(point.x, point.y),
                Vec2::new(
                    point.x + -(direction.x * 5.0),
                    point.y + -(direction.y * 5.0),
                ),
            )
            .z(Layers::Background.value());
    }

    // Write to the window frame.
    draw.to_frame(app, &frame).unwrap();
}
