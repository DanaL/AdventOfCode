use std::collections::HashMap;
use std::collections::HashSet;
use std::fs;
use crate::intcode_vm;

const NORTH: usize = 1;
const SOUTH: usize = 2;
const EAST: usize = 4;
const WEST: usize = 3;

#[derive(Debug, Clone)]
enum Dir {
	N,
	S,
	E,
	W,
}

#[derive(Debug, Clone, PartialEq)]
enum Con {
	Path,
	Wall,
	Unknown,
}

#[derive(Debug)]
struct Bot {
	maze: HashMap<(i32, i32), usize>,
	path: Vec<i32>,
	checked: HashMap<i32, Vec<Con>>,
	sq_id: i32,
}

// I need a data structure/table where I can mark a connection between two
// nodes. 0 -> 1 (North), 1 -> 0 (South). Can also say no or unknown
impl Bot {
	fn new() -> Bot {
		let mut b = Bot { maze: HashMap::new(), path: Vec::new(), checked: HashMap::new(), sq_id: 0 };
		// slightly wasteful since there are 4 directions but I wanted to be able to refer to them
		// by the numbers assigned in the problem (1 = north, 2 = south)
		b.checked.insert(0, vec![Con::Unknown, Con::Unknown, Con::Unknown, Con::Unknown, Con::Unknown]);
		b.path.push(0);
		b
	}

	fn pick_move(&self, curr: &Vec<Con>) -> usize {
		if curr[SOUTH] == Con::Unknown { return SOUTH };
		if curr[EAST] == Con::Unknown { return EAST };
		if curr[WEST] == Con::Unknown { return WEST };
		if curr[NORTH] == Con::Unknown { return NORTH };

		return 0
	}

	fn invert_move(&self, mv: usize) -> usize {
		let inv = match mv {
			NORTH => SOUTH,
			SOUTH => NORTH,
			EAST => WEST,
			WEST => EAST,
			_ => panic!("Illegal direction!"),
		};

		inv
	}

	fn explore(&mut self, vm: &mut intcode_vm::IntcodeVM) {
		loop {
			let curr_sq = self.path[self.path.len() - 1];
			let sq = self.checked.get(&(curr_sq as i32)).unwrap();
			let mv = self.pick_move(&sq);

			if mv == 0 {
				// We've exhausted all possibilities and need to backtrack
			}
			else {
				vm.write_to_buff(mv as i64);
				vm.run();
				if vm.output_buffer == 0 {
					println!("After trying {}, bumped into a wall.", mv);
					self.checked.entry(curr_sq as i32)
						.and_modify(|arr| arr[mv] = Con::Wall);
					println!("{:?}", self.checked);
				}
				else if vm.output_buffer == 1 {
					println!("Found a path!!");
					self.checked.entry(curr_sq as i32)
						.and_modify(|arr| arr[mv] = Con::Path);
					self.sq_id += 1;
					self.checked.insert(self.sq_id, vec![Con::Unknown, Con::Unknown, Con::Unknown, Con::Unknown, Con::Unknown]);
					self.maze.insert((curr_sq, self.sq_id), mv);
					self.maze.insert((self.sq_id, curr_sq), self.invert_move(mv));
					self.path.push(self.sq_id);
					println!("{:?}", self.checked);
					println!("{:?}", self.path);
					println!("{:?}", self.maze);
					break;
				}
				else if vm.output_buffer == 2 {
					// Hurrah we found the Oxygen Unit!
				}
				println!("After trying {}, result: {}", mv, vm.output_buffer);
			}
		}
	}
}

pub fn solve_q1() {
	let prog_txt = fs::read_to_string("./inputs/day15.txt").unwrap();
	let mut vm = intcode_vm::IntcodeVM::new();
	vm.load(prog_txt.trim());

	let mut bot = Bot::new();
	bot.explore(&mut vm);
}
