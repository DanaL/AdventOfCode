mod day_ten;
mod util;

fn main() {
	let (x, y, asteroids) = day_ten::solve_q1();
	day_ten::solve_q2(x, y, asteroids, 200);
}
