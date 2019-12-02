use std::fs;

mod day_two;

fn main() {
	let day2_input = fs::read_to_string("./inputs/day2.txt").unwrap();
	day_two::solve(day2_input.trim());
	//day_two::solve("1,9,10,3,2,3,11,0,99,30,40,50");
}
