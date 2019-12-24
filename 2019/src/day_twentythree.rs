use std::fs;
use std::collections::HashMap;
use std::collections::VecDeque;
use std::process;
use crate::intcode_vm;

const NUM_OF_VMS: usize = 50;

#[derive(Debug)]
struct NAT {
	last_x: i64,
	last_y: i64,
	idle: Vec<bool>,
}

impl NAT {
	pub fn new(count: usize) -> NAT {
		NAT { last_x: -1, last_y: -1, idle: vec![false; count] }
	}

	fn is_idle(&self) -> bool {
		self.idle.iter().filter(|n| **n).count() == self.idle.len()
	}

	fn send(&mut self, x: i64, y: i64) {
		self.last_x = x;
		self.last_y = y;
	}
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
	let mut net = Vec::new();

	// create/initialize the VMs
	for j in 0..NUM_OF_VMS {
		let mut vm = intcode_vm::IntcodeVM::new();
		vm.load(prog_txt.trim());
		vm.write_to_buff(j as i64);
		vm.run();
		net.push(vm);
	}

	'outer: loop {
		// What's each machine doing?
		for j in 0..NUM_OF_VMS {
			let done = exec(j, &mut net, &mut msg_q);
			if done {
				break 'outer;
			}
		}
	}
}

fn exec2(m_num: usize, net: &mut Vec<intcode_vm::IntcodeVM>, msg_q: &mut Vec<VecDeque<i64>>,
			nat: &mut NAT) {
	nat.idle[m_num] = false;
	match net[m_num].state {
		intcode_vm::VMState::AwaitInput => {
			if msg_q[m_num].len() == 0 {
				net[m_num].write_to_buff(-1);
				net[m_num].run();
				nat.idle[m_num] = true;
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
				nat.send(x, y);
			} else {
				println!("Paused to send output! ({}) {} {} {}", m_num, receiver, x ,y);
				msg_q[receiver as usize].push_back(x);
				msg_q[receiver as usize].push_back(y);
			}
		},
		_ => todo!("I haven't implemented this yet! {:?}", net[m_num].state),
	}
}

pub fn solve_q2() {
	let prog_txt = fs::read_to_string("./inputs/day23.txt").unwrap();
	let mut msg_q = vec![VecDeque::<i64>::new(); NUM_OF_VMS];
	let mut net = Vec::new();
	let mut nat = NAT::new(NUM_OF_VMS);

	// create/initialize the VMs
	for j in 0..NUM_OF_VMS {
		let mut vm = intcode_vm::IntcodeVM::new();
		vm.load(prog_txt.trim());
		vm.write_to_buff(j as i64);
		vm.run();
		net.push(vm);
	}

	let (mut prev_x, mut prev_y) = (-1, -1);	
	let mut idle_count = 0;
	'outer: loop {
		// What's each machine doing?
		for j in 0..NUM_OF_VMS {
			exec2(j, &mut net, &mut msg_q, &mut nat);
			if nat.is_idle() {
				idle_count += 1;
			}

			if idle_count > 100 {
				println!("nat sending to 0");
				if nat.last_x == prev_x && nat.last_y == prev_y {
					println!("Repeated signals! {} {}", prev_x, prev_y);
					break 'outer;
				}

				net[0].write_to_buff(nat.last_x);
				net[0].write_to_buff(nat.last_y);
				prev_x = nat.last_x;
				prev_y = nat.last_y;
	
				idle_count = 0;
			}
		}
	}
}
