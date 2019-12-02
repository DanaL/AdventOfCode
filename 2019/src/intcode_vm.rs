// A "VM" for the intcode machine, which sounds like it's going to be a thing
// in at least a few of the problems this year

#[derive(Debug)]
pub struct IntcodeVM {
	memory: Vec<usize>,
	ptr: usize,
}

impl IntcodeVM {
	fn load(&self, prog_txt: &str) {
		self.memory = prog_txt.split(",")
			.map(|a| a.parse::<usize>().unwrap()).collect();
	}
}
