use std::fs;
use crate::util;

pub fn solve_q1() {
	let input = fs::read_to_string("./inputs/day3.txt").unwrap();
	let wires: Vec<&str> = input.trim().split("\n").collect();

	// I think my approach will be to fill a hashmap with points I've 
	// encountered as I process the wires. If I find a point already in
	// the hashmap, then it's an intersection. Store those in a vector
	// and then after I've processed both wires, find which intersection
	// is closest to the origin.
	
	// We don't care about wires crossing themselves
	for mv in wires[0].split(",") {
		let a = &mv[..1];
		let d = &mv[1..].parse::<i32>().unwrap();
		println!("{} -> {}", a, d);
	}
	//println!("{}", util::manhattan_d(1, 1, 4, 4));
}
