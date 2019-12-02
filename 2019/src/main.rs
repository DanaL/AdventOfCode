use std::fs;

mod day_two;
//mod intcode_vm;

pub struct IntcodeVM {
	memory: Vec<usize>,
	ptr: usize,
}

impl IntcodeVM {
	pub fn new() -> IntcodeVM {
		IntcodeVM { ptr: 0, memory: Vec::new() }
	}

	fn load(&mut self, prog_txt: &str) {
		self.memory = prog_txt.split(",")
			.map(|a| a.parse::<usize>().unwrap()).collect();
	}
}

fn main() {
	let day2_input = fs::read_to_string("./inputs/day2.txt").unwrap();
	day_two::solve(day2_input.trim());
	//day_two::solve("1,9,10,3,2,3,11,0,99,30,40,50");

	let mut vm = IntcodeVM::new();
	println!("{}", vm.ptr);
}
