use std::fs;
use crate::intcode_vm;

pub fn solve_q1() {
	//let prog_txt = fs::read_to_string("./inputs/day8.txt").unwrap();
	//let prog_txt = "1102,34915192,34915192,7,4,7,99,0";
	let prog_txt = "104,1125899906842624,99";
	let mut vm = intcode_vm::IntcodeVM::new();
	vm.load(prog_txt.trim());
	vm.run();
	
	println!("Hello, day nine!, {}", vm.output_buffer);
}
