use std::fs;

fn calc_fuel_req(mod_weight: i32) -> i32 {
	mod_weight / 3 - 2
}

fn recurse_calc(a: i32) -> i32 {
	if a > 0 {
		return a + recurse_calc(calc_fuel_req(a))
	} 

	0
}

fn main() {
	let contents = fs::read_to_string("./inputs/day1.txt")
		.expect("Something went wrong with the file.");

	let mut sum_q1 = 0;
	let mut sum_q2 = 0;

	// A version using map() and sum() that I don't like as must as my
	// boring for loop version because I can do the string parsing and
	// both sums in a single loop. (Lots of the answers on reddit used
	// iterators and map() and sum() though so I thought I'd try them out.
	//let weights: Vec<i32> = contents.trim().lines()
	//	.map(|line| i32::from_str(line).expect("Not an i32!!")).collect();
	//let sw: i32 = weights.iter().map(|a| calc_fuel_req(*a)).sum();
	//println!("Q1: {}", sw);

	for line in contents.trim().lines() {
		let mod_weight: i32 = line.parse()
			.expect("Invalid integer!");

		let fuel_req = calc_fuel_req(mod_weight);
		sum_q1 += fuel_req;
		sum_q2 += recurse_calc(fuel_req);
	}

	println!("Day 1 Q1 answer: {}", sum_q1);
	println!("Day 1 Q2 answer: {}", sum_q2);
}
