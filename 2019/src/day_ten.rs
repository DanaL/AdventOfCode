use std::fs;
use std::f32;
use std::f32::consts;
use std::collections::HashSet;

fn angle_between_pts(x0: i32, y0: i32, x1: i32, y1: i32) -> i32 {
	let dx = (x1 - x0) as f32;
	let dy = (y1 - y0) as f32;

	// Hmm I want to use these as entries in a hashset but I don't want to lose too
	// much accuaracy when I round
	let angle = f32::atan2(dy, dx) * 180.0 / f32::consts::PI;
	(angle * 1000.0).round() as i32
}

// After thinking of a few different approaches, I realized all I
// needed to do was to keep a Set of the relative angles of the
// other asteroids. I don't care about individual asteroids. With a
// Set, then asteroids with the same angle as others are filtered out.
fn count_visible(map: &str, x0: i32, y0: i32, width: i32) -> usize {
	let mut asteroids: HashSet<i32> = HashSet::new();

	let (mut x, mut y) = (0, 0);
	for ch in map.chars() {
		match ch {
			'.' => x = (x + 1) % width,
			'#' => {
				if x != x0 || y != y0 {
					let angle = angle_between_pts(x0, y0, x, y);
					asteroids.insert(angle);
				}
				x = (x + 1) % width;
			},
			_ => {
				x = 0;
				y += 1;
			}
		}
	}

	asteroids.len()
}

pub fn solve_q1() {
	let mut map_txt = fs::read_to_string("./inputs/day10.txt").expect("Missing input file");
	map_txt = map_txt.replace("\r", "").trim().to_string();
	//let mut map_txt = ".#..#\n.....\n#####\n....#\n...##\n";
	//map_txt = map_txt.trim();
	let width = map_txt.find('\n').unwrap();
	let length = map_txt.trim().matches('\n').count() + 1;

	let (mut x, mut y) = (0, 0);
	let mut counts: Vec<usize> = Vec::new();
	for ch in map_txt.chars() {
		match ch {
			'.' => x = (x + 1) % width,
			'#' => {
				counts.push(count_visible(&map_txt, x as i32, y as i32, width as i32));
				x = (x + 1) % width;
			},
			'\n' => {
				x = 0;
				y += 1;
			}

			_ => {
				println!("Hmm? '{}'", ch);
				panic!("Unexpected character in data!");
			},
		}
	}
	println!("Q1: {}", counts.iter().max().unwrap());
}
