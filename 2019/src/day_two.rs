use std::fs;
use crate::intcode_vm;

pub fn solve_q1() {
	let prog_txt = fs::read_to_string("./inputs/day2.txt").unwrap();
	let mut vm = intcode_vm::IntcodeVM::new();
	vm.load(prog_txt.trim());
	vm.run(12, 2);

	println!("Q1: {}", vm.read(0));
}

pub fn solve_q2() {
	let prog_txt = fs::read_to_string("./inputs/day2.txt").unwrap();
	let mut vm = intcode_vm::IntcodeVM::new();
	vm.load(prog_txt.trim());
	vm.run(12, 2);
	
	vm.dump();
}
