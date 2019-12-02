use std::fs;

mod day_two;

fn main() {
	let day2_input = fs::read_to_string("./inputs/day2.txt").unwrap();
	day_two::solve(day2_input.trim());
}
