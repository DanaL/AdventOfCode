use std::fs;
use crate::intcode_vm;

pub fn solve_q1() {
	let prog_txt = fs::read_to_string("./inputs/day13.txt").unwrap();
	let mut vm = intcode_vm::IntcodeVM::new();
	vm.load(prog_txt.trim());

	let mut count = 0;
	let mut locs = Vec::new();
	let mut sprites = Vec::new();
	let mut x = -1;
	let mut y = -1;
	loop {
		vm.run();
		if vm.halted {
			break;
		}

		match count {
			0 => x = vm.output_buffer,
			1 => { 
				y = vm.output_buffer;
				locs.push((x, y));
			},
			2 => sprites.push(vm.output_buffer),
			_ => panic!("This shouldn't happen..."),
		}	
		count = (count + 1) % 3;	
	}

	let num_of_blocks = sprites.iter().filter(|s| **s == 2).count();
	println!("Q1 {}", num_of_blocks);
}
