use std::fs;
use crate::intcode_vm;
use crate::util;

fn run_vm(vm: &mut intcode_vm::IntcodeVM, i0: i64, i1: i64) -> i64 {
	vm.write_to_buff(i0);
	vm.write_to_buff(i1);
	vm.run();
	vm.output_buffer
}

pub fn solve_q1() {
	let prog_txt = fs::read_to_string("./inputs/day7.txt").unwrap();
	//let prog_txt = "3,31,3,32,1002,32,10,32,1001,31,-2,31,1007,31,0,33,1002,33,7,33,1,33,31,31,1,32,31,31,4,31,99,0,0,0";
	let mut most_thrust = 0;

	for p in util::Permutations::new(&vec![0, 1, 2, 3, 4]).into_iter() {
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

		let mut out = run_vm(&mut vm_a, p[0], 0);
		out = run_vm(&mut vm_b, p[1], out);
		out = run_vm(&mut vm_c, p[2], out);
		out = run_vm(&mut vm_d, p[3], out);
		out = run_vm(&mut vm_e, p[4], out);
		if out > most_thrust { most_thrust = out }
	}

	println!("Q1: {}", most_thrust);
}

fn run_vm_q2(vm: &mut intcode_vm::IntcodeVM, input: i64) -> i64 {
	vm.write_to_buff(input);
	vm.run();
	vm.output_buffer
}

pub fn run_feedback_loop(prog_txt: &str, seq: Vec<i64>) -> i64 {
	let mut vm_a = intcode_vm::IntcodeVM::new();
	vm_a.init(prog_txt, seq[0]);
	let mut vm_b = intcode_vm::IntcodeVM::new();
	vm_b.init(prog_txt, seq[1]);
	let mut vm_c = intcode_vm::IntcodeVM::new();
	vm_c.init(prog_txt, seq[2]);
	let mut vm_d = intcode_vm::IntcodeVM::new();
	vm_d.init(prog_txt, seq[3]);
	let mut vm_e = intcode_vm::IntcodeVM::new();
	vm_e.init(prog_txt, seq[4]);

	let mut a_feedback = 0;
	while vm_e.state != intcode_vm::VMState::Halted {
		let mut out = run_vm_q2(&mut vm_a, a_feedback);
		out = run_vm_q2(&mut vm_b, out);
		out = run_vm_q2(&mut vm_c, out);
		out = run_vm_q2(&mut vm_d, out);
		out = run_vm_q2(&mut vm_e, out);
		a_feedback = out;
	}

	vm_e.output_buffer
}

pub fn solve_q2() {
	let prog_txt = fs::read_to_string("./inputs/day7.txt").unwrap();
	//let prog_txt = "3,26,1001,26,-4,26,3,27,1002,27,2,27,1,27,26,27,4,27,1001,28,-1,28,1005,28,6,99,0,0,5";
	//let prog_txt = "3,52,1001,52,-5,52,3,53,1,52,56,54,1007,54,5,55,1005,55,26,1001,54,-5,54,1105,1,12,1,53,54,53,1008,54,0,55,1001,55,1,55,2,53,55,53,4,53,1001,56,-1,56,1005,56,6,99,0,0,0,0,10";

	let mut most_thrust = 0;
	for p in util::Permutations::new(&vec![5, 6, 7, 8, 9]).into_iter() {
		let result = run_feedback_loop(prog_txt.trim(), p);
		if result > most_thrust { most_thrust = result }
	}

	println!("Q2: {}", most_thrust);
}
