use std::{
    env,
    path::{Path, PathBuf},
};

use fs_extra::{dir, dir::CopyOptions};

fn get_output_path() -> PathBuf {
    //<root or manifest path>/target/<profile>/
    let manifest_dir_string = env::var("CARGO_MANIFEST_DIR").unwrap();
    let build_type = env::var("PROFILE").unwrap();
    let path = Path::new(&manifest_dir_string)
        .join("target")
        .join(build_type);
    return PathBuf::from(path);
}

// and then inside my build.rs main() method:
fn main() {
    let target_dir = get_output_path();
    let dest = Path::join(Path::new(&target_dir), Path::new("data"));
    let src = Path::join(
        Path::new(&std::env::current_dir().unwrap()),
        Path::new("data"),
    );

    assert_eq!(src.is_dir(), true);

    let _ = std::fs::remove_dir_all(dest.clone());

    let options = CopyOptions {
        overwrite: true,
        skip_exist: false,
        buffer_size: 64000,
        copy_inside: true,
        content_only: false,
        depth: 0,
    };
    let _ = dir::copy(src.clone(), dest.clone(), &options);
}
