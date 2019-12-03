use std::collections::HashMap;
use std::fs;
use crate::util;

#[derive(Debug)]
struct Loc {
	wire_num: u32,
	steps: u32,
}

fn write_wire_path(wire_path: &str, wire_num: u32, visited: &mut HashMap<(i32, i32), Loc>) -> (u32, u32) {
	let mut x = 0;
	let mut y = 0;
	let mut nearest = std::u32::MAX;
	let mut steps = 0;
	let mut fewest_steps = std::u32::MAX;

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

		for _ in 0..*d {
			x += dir.0;
			y += dir.1;
			steps += 1;
			let key = (x, y);
			let loc = Loc { wire_num, steps, };

			let v = visited.entry(key).or_insert(Loc { wire_num, steps });
			if v.wire_num != wire_num {
				let vd = util::manhattan_d(0, 0, key.0, key.1);
				if vd < nearest {
					nearest = vd;
				}
				let steps_to_here = v.steps + steps;
				if steps_to_here < fewest_steps
				{
					fewest_steps = steps_to_here;
				}				
			}
		}
	}
	
	(nearest, fewest_steps)
}

pub fn solve_q1() {
	let input = fs::read_to_string("./inputs/day3.txt").unwrap();
	let wires: Vec<&str> = input.trim().split("\n").map(|l| l.trim()).collect();
	
	let mut visited = HashMap::new();
	//write_wire_path("R8,U5,L5,D3", 1, &mut visited);
	//let (nearest, fewest_steps) = write_wire_path("U7,R6,D4,L4", 2, &mut visited);
	write_wire_path(wires[0], 1, &mut visited);
	let (nearest, fewest_steps) = write_wire_path(wires[1], 2, &mut visited);
	println!("Q1: {}", nearest);
	println!("Q2: {}", fewest_steps);
}
