// A "VM" for the intcode machine, which sounds like it's going to be a thing
// in at least a few of the problems this year

#[derive(Debug)]
pub struct IntcodeVM {
	memory: Vec<i32>,
	ptr: i32,
	pub input_buffer: i32,
	pub output_buffer: i32,
}

impl IntcodeVM {
	pub fn dump(self) {
		println!("{}", self.memory.len());
		println!("{:?}", self.memory);
	}

	pub fn read(&self, loc: i32) -> i32 {
		self.memory[loc as usize]
	}

	pub fn write (&mut self, loc: i32, val: i32) {
		self.memory[loc as usize] = val;
	}

	// how do determine the mode? 1002 is 
	// multiply with param 1 in address mode, param 2 in immediate mode.
	// 1102 has both parameters in immediate mode
	// The write parameter CANNOT be in immediate mode
	// opcode: instr - (inst / 100 * 100)
	// if inst / 100 is odd, immediate mode for param 1
	// if inst / 1000 is odd, immediate mode for param 2
	pub fn run(&mut self) {
		self.ptr = 0;
		loop {
			let mut jmp = 0;
			match self.read(self.ptr) {
				// add and write back
				1  => {
					let a = self.read(self.ptr+1);
					let b = self.read(self.ptr+2);
					let dest = self.read(self.ptr+3);
					self.write(dest, self.read(a) + self.read(b));
					jmp = 4;
				},
				// multiply and write back
				2  => {
					let a = self.read(self.ptr+1);
					let b = self.read(self.ptr+2);
					let dest = self.read(self.ptr+3);
					self.write(dest, self.read(a) * self.read(b));
					jmp = 4;
				},
				// read the input buffer
				3 => {
					let dest = self.read(self.ptr+1);
					self.write(dest, self.input_buffer);
					jmp = 2;
				},
				// write to the output buffer
				4 => {
					let a = self.read(self.ptr+1);
					self.output_buffer = self.read(a);
					jmp = 2;
				}
				// halt!
				99 => break,
				// I don't think this should ever happen with our input?
				_  => panic!("Hmm this shouldn't happen..."),
			}
			self.ptr += jmp;
		}
	}

	pub fn new() -> IntcodeVM {
		IntcodeVM { ptr: 0, memory: Vec::new(),
			input_buffer: 0, output_buffer: 0 }
	}

	pub fn load(&mut self, prog_txt: &str) {
		self.memory = prog_txt.split(",")
			.map(|a| a.parse::<i32>().unwrap()).collect();		
	}
}
