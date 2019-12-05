use std::fs;
use crate::intcode_vm;

pub fn solve_q1() {
	//let prog_txt = fs::read_to_string("./inputs/day5.txt").unwrap();
	let prog_txt = "3,0,4,0,99";

	let mut vm = intcode_vm::IntcodeVM::new();
	vm.input_buffer = 44444;

	vm.load(prog_txt.trim());
	vm.run();
	println!("Out buffer: {}", vm.output_buffer);
	vm.dump();
}
