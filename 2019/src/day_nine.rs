use std::fs;
use crate::intcode_vm;
use std::time::SystemTime;

fn run_prog(prog_txt: &str, input: i64) -> i64 {
	let mut vm = intcode_vm::IntcodeVM::new();
	vm.load(prog_txt);
	vm.write_to_buff(input);

	while vm.state != intcode_vm::VMState::Halted {
		vm.run();
	}

	vm.output_buffer
}

pub fn solve() {
	let prog_txt = fs::read_to_string("./inputs/day9.txt").unwrap();
	//let prog_txt = "109,1,204,-1,1001,100,1,100,1008,100,16,101,1006,101,0,99";
	//let prog_txt = "1102,34463338,34463338,63,4,63,99";

	println!("Q1: {}", run_prog(prog_txt.trim(), 1));
	println!("Q2: {}", run_prog(prog_txt.trim(), 2));
}
