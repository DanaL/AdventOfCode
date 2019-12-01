use std::fs;

fn calc_fuel_req(mod_weight: i32) -> i32 {
	mod_weight / 3 - 2
}

fn recurse_calc(a: i32) -> i32 {
	if a > 0 {
		a + recurse_calc(calc_fuel_req(a))
	} else {
		0
	}
}

fn main() {
	let contents = fs::read_to_string("./day1.txt")
		.expect("Something went wrong with the file.");

	let mut sum_q1 = 0;
	let mut sum_q2 = 0;

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
