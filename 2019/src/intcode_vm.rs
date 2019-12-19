use std::collections::HashMap;
use std::collections::VecDeque;

// A "VM" for the intcode machine, which sounds like it's going to be a thing
// in at least a few of the problems this year

#[derive(Debug, PartialEq)]
pub enum VMState {
	Initialized,
	Ready,
	Running,
	Halted,
	AwaitInput,
	Paused,
}

#[derive(Debug)]
pub struct IntcodeVM {
	memory: HashMap<u64, i64>,
	ptr: i64,
	pub input_buffer: VecDeque<i64>,
	pub output_buffer: i64,
	pub state: VMState,
	pub rel_base: i64,
}

impl IntcodeVM {
	pub fn init(&mut self, prog_txt: &str, initial_input: i64) {
		self.load(prog_txt);
		self.write_to_buff(initial_input);
	}

	pub fn read(&self, loc: i64) -> i64 {
		match self.memory.get(&(loc as u64)) {
			Some(v) => *v,
			None => 0,
		}
		//*self.memory.get(&(loc as u64)).unwrap()
	}

	pub fn write_to_buff(&mut self, v: i64) {
		self.input_buffer.push_front(v);
	}

	pub fn write(&mut self, loc: i64, val: i64) {
		*self.memory.entry(loc as u64).or_insert(val) = val;
	}

	// Still assuming there's no immediate mode for writing
	fn get_write_dest(&self, d: i64, param_mode: i64) -> i64 {
		if param_mode == 0 { d } else { d + self.rel_base }
	}

	fn get_val(&self, p: i64, param_mode: i64) -> i64 {
		match param_mode {
			0 => self.read(p),
			1 => p,
			2 => self.read(p + self.rel_base),
			_ => panic!("Illegal parameter mode :o"),
		}
	}

	fn fetch_two_params(&self, loc:i64) -> (i64, i64) {
		(self.read(loc + 1), self.read(loc + 2))
	}

	fn fetch_three_params(&self, loc: i64) -> (i64, i64, i64) {
		(self.read(loc + 1), self.read(loc + 2), self.read(loc + 3))
	}

	pub fn run(&mut self) {
		self.state = VMState::Running;
		while self.state == VMState::Running {
			let instr = self.read(self.ptr);
			let opcode = instr - instr / 100 * 100;
			let mode1 = (instr - instr / 1000 * 1000) / 100 % 3;
			let mode2 = (instr - instr / 10000 * 10000) / 1000 % 3;
			let mode3 = instr / 10000 % 3;

			match opcode {
				// add and write back
				1  => {
					let (a, b, dest) = self.fetch_three_params(self.ptr);
					self.write(self.get_write_dest(dest, mode3),
						self.get_val(a, mode1) + self.get_val(b, mode2));
					self.ptr += 4;
				},
				// multiply and write back
				2  => {
					let (a, b, dest) = self.fetch_three_params(self.ptr);
					self.write(self.get_write_dest(dest, mode3),
						self.get_val(a, mode1) * self.get_val(b, mode2));
					self.ptr += 4;
				},
				// read the input buffer
				3 => {
					let mut dest = self.read(self.ptr+1);
					if mode1 == 2 { dest += self.rel_base; }

					match self.input_buffer.pop_back() {
						// Only increment the pointer when there is input
						// in the buffer. This way, we can interactively ask
						// for input and then resume the read instruction.
						Some(v) => {
							self.write(dest, v);
							self.ptr += 2;
						},
						None => self.state = VMState::AwaitInput,
					}
				},
				// write to the output buffer
				4 => {
					let a = self.read(self.ptr+1);
					self.output_buffer = self.get_val(a, mode1);
					self.ptr += 2;
					self.state = VMState::Paused;
				}
				// jump-if-true
				5 => {
					let (a, jmp) = self.fetch_two_params(self.ptr);
					if self.get_val(a, mode1) != 0 {
						self.ptr = self.get_val(jmp, mode2);
					} else {
						self.ptr += 3;
					}
				}
				// jump-if-false
				6 => {
					let (a, jmp) = self.fetch_two_params(self.ptr);
					if self.get_val(a, mode1) == 0 {
						self.ptr = self.get_val(jmp, mode2);
					} else {
						self.ptr += 3;
					}
				}
				// less than
				7 => {
					let (a, b, dest) = self.fetch_three_params(self.ptr);
					if self.get_val(a, mode1) < self.get_val(b, mode2) {
						self.write(self.get_write_dest(dest, mode3), 1);
					} else {
						self.write(self.get_write_dest(dest, mode3), 0);
					}
					self.ptr += 4;
				}
				// equals
				8 => {
					let (a, b, dest) = self.fetch_three_params(self.ptr);
					if self.get_val(a, mode1) == self.get_val(b, mode2) {
						self.write(self.get_write_dest(dest, mode3), 1);
					} else {
						self.write(self.get_write_dest(dest, mode3), 0);
					}
					self.ptr += 4;
				},
				// adjust relative base
				9 => {
					let a = self.read(self.ptr+1);
					self.rel_base += self.get_val(a, mode1);
					self.ptr += 2;
				},
				// halt!
				99 => self.state = VMState::Halted,
				// I don't think this should ever happen with our input?
				_  => panic!("Hmm this shouldn't happen..."),
			}
		}
	}

	pub fn new() -> IntcodeVM {
		IntcodeVM { ptr: 0, memory: HashMap::new(),	input_buffer: VecDeque::new(),
			output_buffer: 0, state: VMState::Initialized, rel_base: 0 }
	}

	pub fn load(&mut self, prog_txt: &str) {
		let arr: Vec<i64> = prog_txt.split(",").map(|a| a.parse::<i64>().unwrap()).collect();
		for loc in 0..arr.len() {
			self.memory.insert(loc as u64, arr[loc]);
		}

		self.ptr = 0;
		self.state = VMState::Ready;
	}
}
