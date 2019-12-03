use std::collections::HashMap;
use std::fs;
use crate::util;

fn write_wire_path(wire_path: &str, wire_num: u32, wire: &mut HashMap<(i32, i32), u32>) -> u32 {
	// We don't care about wires crossing themselves
	let mut x = 0;
	let mut y = 0;
	let mut nearest = std::u32::MAX;

	for mv in wire_path.split(",") {
		let a = &mv[..1];
		let d = &mv[1..].parse::<i32>().unwrap();

		// Make a tuple for which direction we are moving in
		let dir = match a {
			"U" => (1, 0),
			"D" => (-1, 0),
			"L" => (0, -1),
			"R" => (0, 1),
			_ => (0, 0), // this shouldn't happen :o
		};

		for _j in 0..*d {
			x += dir.0;
			y += dir.1;
			let key = (x, y);

			if wire.contains_key(&key) && wire.get(&key).unwrap() != &wire_num {
				let vd = util::manhattan_d(0, 0, key.0, key.1);
				if vd < nearest {
					nearest = vd;
				}	
			} else {
				wire.insert((x, y), wire_num);
			}
		}
	}
	
	nearest
}

pub fn solve_q1() {
	let input = fs::read_to_string("./inputs/day3.txt").unwrap();
	let wires: Vec<&str> = input.trim().split("\n").collect();

	let mut wire = HashMap::new();
	//write_wire_path("R8,U5,L5,D3", 1, &mut wire);
	//let nearest = write_wire_path("U7,R6,D4,L4", 2, &mut wire);
	write_wire_path(wires[0], 1, &mut wire);
	let nearest = write_wire_path(wires[1], 2, &mut wire);
	println!("Q1: {}", nearest);	
}
