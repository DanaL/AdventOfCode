use std::collections::HashMap;
use std::fs;
use crate::util;

fn write_wire_path(wire_path: &str, wire_num: u32, wire: &mut HashMap<(i32, i32), u32>) {
	// We don't care about wires crossing themselves
	let mut x = 0;
	let mut y = 0;
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

		for j in (0..*d) {
			x += dir.0;
			y += dir.1;
			let key = (x, y);

			if wire.contains_key(&key) && wire.get(&key).unwrap() != &wire_num {
				println!("wires cross {:?}", key);
			} else {
				wire.insert((x, y), wire_num);
			}
			//match wire.get(&key) {
			//	Some(v) => println!("fuck off {}", v),
			//	None => wire.insert(&key, wire_num),
			//}
			//	Some(v) => {
			//		if v != wire_num {
			//			println!("Wire {} crossed with {} at {:?}", wire_num, v, key);
			//		}
			//	},
			//	None => wire.insert((x, y), wire_num),
			//}
		}
	}
}

pub fn solve_q1() {
	let input = fs::read_to_string("./inputs/day3.txt").unwrap();
	let wires: Vec<&str> = input.trim().split("\n").collect();

	// I think my approach will be to fill a hashmap with points I've 
	// encountered as I process the wires. If I find a point already in
	// the hashmap, then it's an intersection. Store those in a vector
	// and then after I've processed both wires, find which intersection
	// is closest to the origin.
	//write_wire_path(wires[0], 1);
	let mut wire = HashMap::new();
	write_wire_path("R8,U5,L5,D3", 1, &mut wire);
	write_wire_path("U7,R6,D4,L4", 2, &mut wire);
	
	let v = wire.get(&(0,6));
	println!("{:?}", v.unwrap());
	let w = wire.get(&(1000,6));
	println!("{:?}", w);
	//println!("{:?}", wire);
	//println!("{}", util::manhattan_d(1, 1, 4, 4));
}
