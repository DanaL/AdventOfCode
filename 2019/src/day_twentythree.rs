use std::fs;
use std::collections::VecDeque;
use crate::intcode_vm;

const NUM_OF_VMS: usize = 50;

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

fn exec(m_num: usize, net: &mut Vec<intcode_vm::IntcodeVM>, msg_q: &mut Vec<VecDeque<i64>>) -> bool {
	match net[m_num].state {
		intcode_vm::VMState::AwaitInput => {
			if msg_q[m_num].len() == 0 {
				net[m_num].write_to_buff(-1);
				net[m_num].run();
				println!("Sending default (-1) to! {}", m_num);
			} else {
				println!("Sending to {}", m_num);
				let val = msg_q[m_num].pop_front().unwrap();
				println!("   ...{}", val);
				net[m_num].write_to_buff(val);
				net[m_num].run();
			}
		},
		intcode_vm::VMState::Paused => {
			// I think all output comes in threes?
			let receiver = net[m_num].output_buffer;
			net[m_num].run();
			let x = net[m_num].output_buffer;
			net[m_num].run();
			let y = net[m_num].output_buffer;
			net[m_num].run();
			if receiver == 255 {
				println!("Q1: {}", y);
				return true;
			}
			println!("Paused to send output! ({}) {} {} {}", m_num, receiver, x ,y);
			msg_q[receiver as usize].push_back(x);
			msg_q[receiver as usize].push_back(y);
		},
		_ => todo!("I haven't implemented this yet! {:?}", net[m_num].state),
	}

	false
}

pub fn solve_q1() {
	let prog_txt = fs::read_to_string("./inputs/day23.txt").unwrap();
	let mut msg_q = vec![VecDeque::<i64>::new(); NUM_OF_VMS];
	let mut net= Vec::new();

	// create/initialize the VMs
	for j in 0..NUM_OF_VMS {
		let mut vm = intcode_vm::IntcodeVM::new();
		vm.load(prog_txt.trim());
		vm.write_to_buff(j as i64);
		vm.run();
		net.push(vm);
	}

	let mut round = 0;
	'outer: loop {
		// What's each machine doing?
		for j in 0..NUM_OF_VMS {
			let done = exec(j, &mut net, &mut msg_q);
			if done {
				break 'outer;
			}
		}

		round += 1;
		//if round > 1 { break }
	}
}
