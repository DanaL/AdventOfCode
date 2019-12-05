use std::fs;
use crate::intcode_vm;

pub fn solve_q1() {
	//let prog_txt = fs::read_to_string("./inputs/day5.txt").unwrap();
	//let prog_txt = "3,0,4,0,99";
	//let prog_txt = "1002,4,3,4,33";
	//let prog_txt = "1101,100,-1,4,0";
	//let prog_txt = "3,9,8,9,10,9,4,9,99,-1,8";
	let prog_txt = "3,9,7,9,10,9,4,9,99,-1,-8";
	let mut vm = intcode_vm::IntcodeVM::new();
	vm.input_buffer = -44;

	vm.load(prog_txt.trim());
	vm.run();
	println!("Out buffer: {}", vm.output_buffer);
	vm.dump();
}
