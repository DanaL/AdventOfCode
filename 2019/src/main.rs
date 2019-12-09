mod day_two;
mod day_five;
mod day_seven;
mod day_nine;
mod intcode_vm;

fn main() {
	println!("Day Two:");
	day_two::solve_q1();
	day_two::solve_q2();
	println!("Day Five:");
	day_five::solve();
	println!("Day Seven:");
	day_seven::solve_q1();
	day_seven::solve_q2();
	println!("Day Nine:");
	day_nine::solve();
}
