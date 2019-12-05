use std::fs;
use crate::intcode_vm;

pub fn solve_q1() {
	let prog_txt = fs::read_to_string("./inputs/day2.txt").unwrap();
	let mut vm = intcode_vm::IntcodeVM::new();
	vm.load(prog_txt.trim());
	vm.write(1, 12);
	vm.write(2, 2);
	vm.run();

	println!("Q1: {}", vm.read(0));
}

pub fn solve_q2() {
	let prog_txt = fs::read_to_string("./inputs/day2.txt").unwrap();
	let mut vm = intcode_vm::IntcodeVM::new();

	// For Part Two, we had to determine what input values (called noun and verb) 
	// would produce 19690720 (and were told they'd both be in the range 0..99).
	// Trying all 10,000 possibilities in a couple of nested loops in Rust
	// probably still would have been instantaneous but I figured there was
	// probably a pattern to be found so I ran a few iterations to see what
	// different numbers gave and found incrementing noun bumped the output by
	// 248832 and the verb by just 1. And (0, 0) gave 530607 so any "program"'s
	// output is just 530607 + (248832 * noun) + verb. So it was just simple 
	// calculation that my answers would be 77 and 49.
	vm.load(prog_txt.trim());
	vm.write(1, 77);
	vm.write(2, 49);
	vm.run();
	println!("{}", vm.read(0)); // Just verify that that's what we wanted
	println!("Q2: {}", 100 * 77 + 49);
}
