mod intcode_vm;
mod util;
mod day_seven;

fn main() {
	day_seven::solve_q1();
	day_seven::solve_q2();
	/*
	let mut p = util::Permutations::new(vec![1,2,3,4,5]);
	for perm in p.into_iter() {
		println!("{:?}", perm);
	}
	*/
}
