use std::fs;

fn calc_fuel_req(mod_weight: i32) -> i32 {
	mod_weight / 3 - 2
}

fn main() {
	let contents = fs::read_to_string("./day1.txt")
		.expect("Something went wrong with the file.");

	let mut sum = 0;
	for line in contents.trim().split("\n") {
		let mod_weight: i32 = line.parse()
			.expect("Invalid integer!");

		sum += calc_fuel_req(mod_weight);
	}

	println!("Day 1 Q1 answer: {}", sum);
}
