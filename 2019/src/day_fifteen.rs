use std::collections::HashMap;
use std::collections::HashSet;
use std::fs;
use crate::intcode_vm;


#[derive(Debug, Clone)]
enum Connection {
	N,
	S,
	E,
	W,
	U,
}

#[derive(Debug)]
struct Bot {
	maze: Vec<Vec<Connection>>
}

// I need a data structure/table where I can mark a connection between two
// nodes. 0 -> 1 (North), 1 -> 0 (South). Can also say no or unknown
impl Bot {
	fn new() -> Bot {
		Bot { maze: vec![vec![Connection::U]] }
	}

	fn explore(&mut self, vm: &mut intcode_vm::IntcodeVM) {
		//self.visited.insert((self.curr.0, self.curr.1));
		// Will have to deal with the situation where there are no paths to take
		vm.write_to_buff(1);
		vm.run();
		println!("Result: {}", vm.output_buffer);
	}
}

pub fn solve_q1() {
	let prog_txt = fs::read_to_string("./inputs/day15.txt").unwrap();
	let mut vm = intcode_vm::IntcodeVM::new();
	vm.load(prog_txt.trim());

	let mut bot = Bot::new();
	bot.explore(&mut vm);
}
