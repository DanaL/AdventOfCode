use std::fs;
use crate::intcode_vm;

pub fn solve_q1() {
	let prog_txt = fs::read_to_string("./inputs/day11.txt").unwrap();
	let mut vm = intcode_vm::IntcodeVM::new();
	vm.load(prog_txt.trim());

	let mut loc = (0, 0);
	println!("{:?}", loc);
	//loop {
		vm.write_to_buff(0);
		vm.run();
		println!("{}", vm.output_buffer);
		vm.run();
		println!("{}", vm.output_buffer);
	//	if vm.halted { break }
	//}
			
}
