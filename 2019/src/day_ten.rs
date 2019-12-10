use std::fs;
use std::f32;

fn angle_between_pts(x0: i32, y0: i32, x1: i32, y1: i32) -> f32 {
	let dx = (x1 - x0) as f32;
	let dy = (y1 - y0) as f32;

	f32::atan2(dy, dx) * 180.0 / 3.14156
}

fn is_visible(map: &mut Vec<Vec<char>>, x0: i32, y0: i32, x1: i32, y1: i32) -> bool {
	// Hmm the problem says that asteroids are blocked when there one exactly 
	// between them. Can I just calculate the angle and distance between the 
	// asteroids. Any asteroid with the same angle blocks other asteroids with
	// the same angle but further away?? 
	// Will Slope of the line suffice??
	let slope: f32 = (y1 - y0) as f32 / (x1 - x0) as f32;
	println!("{}", slope);
	true
}

fn dump(map: &Vec<Vec<char>>) {
	for row in map {
		println!("{}", row.iter().cloned().collect::<String>());
	}
}

pub fn solve_q1() {
	//let mut map_txt = fs::read_to_string("./inputs/day10.txt").expect("Missing input file");
	//map_txt = map_txt.trim().to_string();
	let mut map_txt = ".#..#\n.....\n#####\n....#\n...##\n";
	map_txt = map_txt.trim();
	let width = map_txt.find('\n').unwrap();
	let length = map_txt.trim().matches('\n').count() + 1;
	let mut map = vec![vec!['*'; width]; length];
	
	
	let (mut x, mut y) = (0, 0);
	for ch in map_txt.chars() {
		if ch == '\n' {
			x = 0;
			y += 1;
		} else {
			map[y][x] = ch;
			x = (x + 1) % width;	
		}
	}	

	println!("{}", angle_between_pts(0, 0, 4, 3));
	println!("{}", angle_between_pts(0, 0, 4, 4));
	println!("{}", angle_between_pts(0, 0, 3, 4));
	println!("");
	println!("{}", angle_between_pts(0, 0, 1, 0));
	println!("{}", angle_between_pts(0, 0, 1, 1));
	println!("{}", angle_between_pts(0, 0, 0, 1));
	println!("");
	
	println!("{}", angle_between_pts(3, 4, 0, 2));
	println!("{}", angle_between_pts(3, 4, 1, 2));
	println!("{}", angle_between_pts(3, 4, 2, 2));
	println!("{}", angle_between_pts(3, 4, 3, 2));
	println!("{}", angle_between_pts(3, 4, 4, 2));
	println!("");
	println!("{} *", angle_between_pts(3, 4, 1, 0));
	println!("{} *", angle_between_pts(3, 4, 4, 0));
	//is_visible(&mut map,3, 4, 2, 0);
	//dump(&map);
}
