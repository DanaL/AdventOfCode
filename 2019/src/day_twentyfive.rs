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

fn send_cmd(cmd: &str, vm: &mut intcode_vm::IntcodeVM) {
	for ch in cmd.chars() {
		vm.write_to_buff(ch as i64);
	}
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

		if cmd.trim() == "quit" {
			break;
		}
		send_cmd(cmd.trim(), &mut vm);
		display_msg(&mut vm);
	}	
}
