use std::fs;
use std::i32;
use std::collections::HashMap;
use crate::intcode_vm;

#[derive(Debug)]
enum Dir {
	N,
	S,
	E,
	W,
}

#[derive(Debug)]
struct Bot {
	sqs_painted: HashMap<(i32, i32), char>,
	dir: Dir,
	loc: (i32, i32),
}

impl Bot {
	pub fn new(initial: char) -> Bot {
		let mut b = Bot { loc: (0, 0), dir: Dir::N, sqs_painted: HashMap::new() };
		b.sqs_painted.insert(b.loc, initial);
		b
	}

	pub fn paint(&mut self, iv: i64) {
		if iv == 1 {
			*self.sqs_painted.entry(self.loc).or_insert('#') = '#';
		} else {
			*self.sqs_painted.entry(self.loc).or_insert('.') = '.';
		}
	}

	fn turn(&mut self, iv: i64) {
		match self.dir {
			Dir::N => self.dir = if iv == 0 { Dir::W } else { Dir::E },
			Dir::S => self.dir = if iv == 0 { Dir::E } else { Dir::W },
			Dir::E => self.dir = if iv == 0 { Dir::N } else { Dir::S },
			Dir::W => self.dir = if iv == 0 { Dir::S } else { Dir::N },
		}
	}

	pub fn do_move(&mut self, iv: i64) {
		self.turn(iv);
		match self.dir {
			Dir::N => self.loc.1 -= 1,
			Dir::S => self.loc.1 += 1,
			Dir::E => self.loc.0 += 1,
			Dir::W => self.loc.0 -= 1,
		}
	}

	pub fn curr_panel(&self) -> i64 {
		match self.sqs_painted.get(&self.loc) {
			Some(ch) => if ch == &'.' { 0 } else { 1 },
			None => 0
		}
	}

	pub fn total_painted(&self) -> usize {
		self.sqs_painted.len()
	}

	pub fn print_art(&self) {
		let mut furthest_w: i32 = i32::MAX;
		let mut furthest_e = i32::MIN;
		let mut furthest_n = i32::MAX;
		let mut furthest_s = i32::MIN;

		for loc in self.sqs_painted.keys().into_iter() {
			if loc.0 < furthest_w { furthest_w = loc.0 }
			if loc.0 > furthest_e { furthest_e = loc.0 }
			if loc.1 < furthest_n { furthest_n = loc.1 }
			if loc.1 > furthest_s { furthest_s = loc.1 }
		}

		for r in furthest_n..furthest_s+1 {
			let mut row = String::new();
			for c in furthest_w..furthest_e+1 {
				match self.sqs_painted.get(&(c, r)) {
					Some(v) => if v == &'#' { row.push('#') } else { row.push(' ') },
					None => row.push(' '),
				}
			}
			println!("{}", row);
		}
	}
}

pub fn solve(initial: char, print: bool) {
	let prog_txt = fs::read_to_string("./inputs/day11.txt").unwrap();
	let mut vm = intcode_vm::IntcodeVM::new();
	vm.load(prog_txt.trim());
	let mut bot = Bot::new(initial);
	vm.write_to_buff(bot.curr_panel());

	loop {
		vm.run();
		match vm.state {
			intcode_vm::VMState::Halted => break,
			_ => {
				bot.paint(vm.output_buffer);
				vm.run();
				bot.do_move(vm.output_buffer);
				vm.write_to_buff(bot.curr_panel());
			}
		}		
	}

	println!("Sqs painted: {}", bot.total_painted());
	if print {
		bot.print_art();
	}
}
