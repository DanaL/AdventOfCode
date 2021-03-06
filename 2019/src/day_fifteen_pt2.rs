use std::collections::HashMap;
use std::collections::BinaryHeap;
use std::collections::HashSet;
use std::fs;
use crate::intcode_vm;

// I split part two into a seprate file and the consts, Con enum, and Bot struct
// are duplicated. Of course I'd move them into a shared file in real code but
// I can tolerate a bit of mess for AoC and I'm still a day behind...
const NORTH: usize = 1;
const SOUTH: usize = 2;
const EAST: usize = 4;
const WEST: usize = 3;

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
	o2_sq: i32
}

impl Bot {
	fn new() -> Bot {
		let mut b = Bot { maze: HashMap::new(), path: Vec::new(), checked: HashMap::new(),
			sq_id: 0, o2_sq: -1 };
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

	fn map_whole_maze(&mut self, vm: &mut intcode_vm::IntcodeVM) {
		loop {
			let curr_sq = self.path[self.path.len() - 1];
			let sq = self.checked.get(&(curr_sq as i32)).unwrap();
			let mv = self.pick_move(&sq);

			if mv == 0 {
				if self.path.len() == 1 {
					// We're back at the start of the maze so it should be fully mapped out!
					break;
				}
				let curr = self.path.pop().unwrap();
				let back = self.path[self.path.len() - 1];
				let mv_back = self.maze.get(&(curr, back)).unwrap();
				vm.write_to_buff(*mv_back as i64);
				vm.run();
			}
			else {
				vm.write_to_buff(mv as i64);
				vm.run();
				if vm.output_buffer == 0 {
					self.checked.entry(curr_sq as i32)
						.and_modify(|arr| arr[mv] = Con::Wall);
				}
				else {
					self.checked.entry(curr_sq as i32)
						.and_modify(|arr| arr[mv] = Con::Path);
					self.sq_id += 1;
					if vm.output_buffer == 2 {
						self.o2_sq = self.sq_id;
					}
					let path_back = self.invert_move(mv);
					let mut next_sq = vec![Con::Unknown, Con::Unknown, Con::Unknown, Con::Unknown, Con::Unknown];
					next_sq[path_back] = Con::Path;
					self.checked.insert(self.sq_id, next_sq);
					self.maze.insert((curr_sq, self.sq_id), mv);
					self.maze.insert((self.sq_id, curr_sq), path_back);
					self.path.push(self.sq_id);
				}
			}
		}
	}
}

fn flood_fill(start: i32, graph: &mut HashMap<(i32, i32), usize>) {
	let mut minutes = 0;
	let nodes: Vec<(i32, i32)> = graph.keys().map(|k| *k).collect();
	let mut visited: HashSet<i32> = HashSet::new();
	let mut to_visit: Vec<i32> = Vec::new();
	to_visit.push(start);
	
	loop {
		let mut child_nodes: Vec<i32> = Vec::new();
		while to_visit.len() > 0 {
			let node = to_visit.pop().unwrap();
			visited.insert(node);
			for n in &nodes {
				if n.0 == node && !visited.contains(&n.1) {
					child_nodes.push(n.1);
				}
			}
		}

		if child_nodes.len() == 0 {
			break;
		}

		minutes += 1;

		for n in &child_nodes {
			to_visit.push(*n);
		}	
	}

	println!("{:?}", minutes);
}

pub fn solve() {
	let prog_txt = fs::read_to_string("./inputs/day15.txt").unwrap();
	let mut vm = intcode_vm::IntcodeVM::new();
	vm.load(prog_txt.trim());

	let mut bot = Bot::new();
	bot.map_whole_maze(&mut vm);
	flood_fill(bot.o2_sq, &mut bot.maze);
}
