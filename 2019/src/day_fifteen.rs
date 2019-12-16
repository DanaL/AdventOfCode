use std::fs;
use crate::intcode_vm;

pub fn solve_q1() {
	let prog_txt = fs::read_to_string("./inputs/day15.txt").unwrap();
	let mut vm = intcode_vm::IntcodeVM::new();
	vm.load(prog_txt.trim());
}
