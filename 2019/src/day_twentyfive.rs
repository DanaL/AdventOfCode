use std::fs;
use std::io;
use crate::intcode_vm;

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

fn do_north(vm: &mut intcode_vm::IntcodeVM) {
	vm.write_to_buff(110);
	vm.write_to_buff(111);
	vm.write_to_buff(114);
	vm.write_to_buff(116);
	vm.write_to_buff(104);
	vm.write_to_buff(10);
	vm.run();
}

fn do_south(vm: &mut intcode_vm::IntcodeVM) {
	vm.write_to_buff(115);
	vm.write_to_buff(111);
	vm.write_to_buff(117);
	vm.write_to_buff(116);
	vm.write_to_buff(104);
	vm.write_to_buff(10);
	vm.run();
}

fn do_east(vm: &mut intcode_vm::IntcodeVM) {
	vm.write_to_buff(101);
	vm.write_to_buff(97);
	vm.write_to_buff(115);
	vm.write_to_buff(116);
	vm.write_to_buff(10);
	vm.run();
}

fn do_west(vm: &mut intcode_vm::IntcodeVM) {
	vm.write_to_buff(119);
	vm.write_to_buff(101);
	vm.write_to_buff(115);
	vm.write_to_buff(116);
	vm.write_to_buff(10);
	vm.run();
}

fn do_inv(vm: &mut intcode_vm::IntcodeVM) {
	vm.write_to_buff(105);
	vm.write_to_buff(110);
	vm.write_to_buff(118);
	vm.write_to_buff(10);
	vm.run();
}

pub fn solve() {
	let prog_txt = fs::read_to_string("./inputs/day25.txt").unwrap();
	let mut vm = intcode_vm::IntcodeVM::new();
	vm.load(prog_txt.trim());

	vm.run();
	display_msg(&mut vm);

	
	loop {
		let mut cmd = String::new();
		io::stdin().read_line(&mut cmd)
			.expect("Failed to read line");
		match cmd.trim() {
			"quit" => break,
			"north" => do_north(&mut vm),
			"south" => do_south(&mut vm),
			"east" => do_east(&mut vm),
			"west" => do_west(&mut vm),
			"inv" => do_inv(&mut vm),
			_ => println!("Unknown cmd: {}", cmd.trim()),
		}	
		display_msg(&mut vm);
	}	
}
