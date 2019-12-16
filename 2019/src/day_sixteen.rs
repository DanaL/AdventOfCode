use std::fs;

fn gen_pat(base: &Vec<i32>, round: i32, len: i32) -> Vec<i32> {
	let mut pat: Vec<i32> = Vec::new();
	let pat_size = (len + 1) as usize;

	while pat.len() < pat_size {
		for d in base {
			for n in 0..round {
				pat.push(*d);
			}
		}
	}
	pat.remove(0);

	pat
}

fn do_fft(signal: Vec<i32>, pattern: &Vec<i32>) -> Vec<i32> {
	let mut next: Vec<i32> = vec![0; signal.len()];
	let mut round = 0;
	while round < signal.len() {
		let p = gen_pat(&pattern, (round + 1) as i32, signal.len() as i32);
		let mut sum = 0;
		for j in 0..signal.len() {
			sum += signal[j] * p[j];
		}
		next[round] = sum.abs() % 10;
		round += 1;	
	}

	next
}

pub fn solve_q1() {
	//let signal_txt = "80871224585914546619083218645595".to_string();
	let signal_txt = fs::read_to_string("./inputs/day16.txt").unwrap();
	let mut signal: Vec<i32> = signal_txt.trim()
		.chars()
		.map(|d| d.to_digit(10).unwrap() as i32)
		.collect();
	
	let pattern = vec![0, 1, 0, -1];
	for j in 0..100 {
		signal = do_fft(signal, &pattern);
	}
	println!("{:?}", signal);
}
