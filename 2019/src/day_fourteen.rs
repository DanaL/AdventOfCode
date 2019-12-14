use std::fs;

#[derive(Debug)]
struct Reaction {
	pub product: (String, u8),
	pub reagents: Vec<(String, u8)>,
}

impl Reaction {
	fn new(r: String, q: u8) -> Reaction {
		Reaction { product: (r, q), reagents: Vec::new() }
	}

	fn only_ore() -> bool {
		false
	}
}

fn parse_line(line: String) -> Reaction {
	println!("{}", line);
	Reaction::new("FOO".to_string(), 0)
}

pub fn solve_q1() {
	let lines: Vec<Reaction> = fs::read_to_string("./inputs/day14.txt")
		.unwrap()
		.split('\n')
		.map(|s| parse_line(s.to_string()))
		.collect();

	//println!("{:?}", lines);
	
}
