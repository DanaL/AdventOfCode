use std::fs;
use std::collections::HashSet;
use crate::intcode_vm;

fn run_vm(vm: &mut intcode_vm::IntcodeVM, i0: i32, i1: i32) -> i32 {
	vm.write_to_buff(i0);
	vm.write_to_buff(i1);
	vm.run();
	vm.output_buffer
}

pub fn solve_q1() {
	let prog_txt = fs::read_to_string("./inputs/day7.txt").unwrap();
	//let prog_txt = "3,31,3,32,1002,32,10,32,1001,31,-2,31,1007,31,0,33,1002,33,7,33,1,33,31,31,1,32,31,31,4,31,99,0,0,0";
	let mut most_thrust = 0;

	// Behold the glory of me determining all permutations of [0, 1, 2, 3, 4, 5] 	
	for i in 0..5 {
		for j in 0..5 {
			for k in 0..5 {
				for l in 0..5 {
					for m in 0..5 {
						let phase_seq: HashSet<i32> = vec![i, j, k, l, m].into_iter().collect();	
						if phase_seq.len() == 5 {
							let mut vm_a = intcode_vm::IntcodeVM::new();
							vm_a.load(prog_txt.trim());
							let mut vm_b = intcode_vm::IntcodeVM::new();
							vm_b.load(prog_txt.trim());
							let mut vm_c = intcode_vm::IntcodeVM::new();
							vm_c.load(prog_txt.trim());
							let mut vm_d = intcode_vm::IntcodeVM::new();
							vm_d.load(prog_txt.trim());
							let mut vm_e = intcode_vm::IntcodeVM::new();
							vm_e.load(prog_txt.trim());

							let mut out = run_vm(&mut vm_a, i, 0);	
							let mut out = run_vm(&mut vm_b, j, out);	
							let mut out = run_vm(&mut vm_c, k, out);	
							let mut out = run_vm(&mut vm_d, l, out);	
							let mut out = run_vm(&mut vm_e, m, out);
							if out > most_thrust { most_thrust = out }
						}
					}
				}
			}
		}
	}

	println!("Q1: {}", most_thrust);
}
