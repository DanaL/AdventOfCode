use std::fs;
use crate::intcode_vm;

pub fn solve_q1() {
	let prog_txt = fs::read_to_string("./inputs/day5.txt").unwrap();
	let mut vm = intcode_vm::IntcodeVM::new();
	vm.load(prog_txt.trim());
	vm.dump();
}
