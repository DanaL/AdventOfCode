use std::fs;
use crate::intcode_vm;

pub fn solve() {
	let prog_txt = fs::read_to_string("./inputs/day5.txt").unwrap();	
	let mut vm = intcode_vm::IntcodeVM::new();

	vm.load(prog_txt.trim());
	vm.write_to_buff(1);
	while !vm.halted {
		vm.run();
	}
	println!("Q1: {}", vm.output_buffer);
	
	vm.load(prog_txt.trim());
	vm.write_to_buff(5);
	while !vm.halted {
		vm.run();
	}
	println!("Q2: {}", vm.output_buffer);	
}
