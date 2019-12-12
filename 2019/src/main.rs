mod day_seven;
mod intcode_vm;
mod util;

fn main() {
	day_seven::solve_q1();
	day_seven::solve_q2();

	for p in util::Permutations::new(&vec!['a', 'b', 'c']).into_iter() {
		println!("{:?}", p);
	}
}
