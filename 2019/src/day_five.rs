use std::fs;
use crate::intcode_vm;

pub fn solve_q1() {
	let prog_txt = fs::read_to_string("./inputs/day5.txt").unwrap();	
	let mut vm = intcode_vm::IntcodeVM::new();

	vm.load(prog_txt.trim());
	vm.input_buffer = 1;
	vm.run();
	println!("Q1: {}", vm.output_buffer);

	vm.load(prog_txt.trim());
	vm.input_buffer = 5;
	vm.run();
	println!("Q2: {}", vm.output_buffer);	
}
