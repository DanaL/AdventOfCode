use std::fs;
use crate::intcode_vm;

fn input_not(x: char, y: char, vm: &mut intcode_vm::IntcodeVM) {
	vm.write_to_buff(78);
	vm.write_to_buff(79);
	vm.write_to_buff(84);
	vm.write_to_buff(32);
	vm.write_to_buff(x as i64);
	vm.write_to_buff(32);
	vm.write_to_buff(y as i64);
	vm.write_to_buff(10);
}

fn input_and(x: char, y: char, vm: &mut intcode_vm::IntcodeVM) {
	vm.write_to_buff(65);
	vm.write_to_buff(78);
	vm.write_to_buff(68);
	vm.write_to_buff(32);
	vm.write_to_buff(x as i64);
	vm.write_to_buff(32);
	vm.write_to_buff(y as i64);
	vm.write_to_buff(10);
}

fn input_or(x: char, y: char, vm: &mut intcode_vm::IntcodeVM) {
	vm.write_to_buff(79);
	vm.write_to_buff(82);
	vm.write_to_buff(32);
	vm.write_to_buff(x as i64);
	vm.write_to_buff(32);
	vm.write_to_buff(y as i64);
	vm.write_to_buff(10);
}

fn input_walk(vm: &mut intcode_vm::IntcodeVM) {
	vm.write_to_buff(87);
	vm.write_to_buff(65);
	vm.write_to_buff(76);
	vm.write_to_buff(75);
	vm.write_to_buff(10);
}

fn display_msg(vm: &mut intcode_vm::IntcodeVM) {
	let mut msg = String::from("");

	loop {
		vm.run();
		if vm.state == intcode_vm::VMState::Paused {
			msg.push(vm.output_buffer as u8 as char);
		} else {
			break;
		}
	}
	println!("{}", msg);
}

pub fn run_machine(vm: &mut intcode_vm::IntcodeVM) -> i64 {
	loop {
		println!("{:?}", vm.state);
		vm.run();
		match vm.state {
			intcode_vm::VMState::Paused => display_msg(vm),
			intcode_vm::VMState::Halted => break,
			intcode_vm::VMState::AwaitInput => panic!("I don't think this should happen! {:?}", vm.state),
			_ => continue,
		}
	}
	
	return vm.output_buffer
}

pub fn solve_q1() {
	let prog_txt = fs::read_to_string("./inputs/day21.txt").unwrap();
	let mut vm = intcode_vm::IntcodeVM::new();
	vm.load(prog_txt.trim());

	// skip the initial prompt
	display_msg(&mut vm);

	// if hole immediately in front, jump
	// or C is a hole and D is safe
	input_not('A', 'J', &mut vm);
	input_not('C', 'T', &mut vm);
	input_and('D', 'T', &mut vm);
	input_or('T', 'J', &mut vm);
	input_walk(&mut vm);
	
	println!("{}", run_machine(&mut vm));
}
