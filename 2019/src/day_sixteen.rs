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

// So we know from what part of the signal we want that we
// are going to be multiplying our vector by a matrix that looks
// like:
//
//                 1 1 1 1 1 1 1 1
//                 0 1 1 1 1 1 1 1
//                 0 0 1 1 1 1 1 1
//                 0 0 0 1 1 1 1 1
//                 0 0 0 0 1 1 1 1
//                 0 0 0 0 0 1 1 1
//                 0 0 0 0 0 0 1 1
//                 0 0 0 0 0 0 0 1
//
// So we can start with the final digit, mod it and write it to the
// array. Then the 2nd last digit is the previous digit plus itself, % 10.
// Third last digit is the running sum % 10. So we can process our signal
// (for this particular problem at least) with a single loop
fn do_fft_q2(v: Vec<u8>, num_of_digits: usize) -> Vec<u8> {
	let mut nv: Vec<u8> = vec![0;num_of_digits];
	
	let mut sum: u64 = 0;
	for j in (0..num_of_digits).rev() {
		sum += v[j] as u64;
		nv[j] = (sum % 10) as u8;
	}

	nv
}

pub fn solve_q2() {
	let signal_txt = fs::read_to_string("./inputs/day16.txt").unwrap();
	let mut signal: Vec<u8> = signal_txt.trim()
		.chars()
		.map(|d| d.to_digit(10).unwrap() as u8)
		.collect();

	let num_of_digits = 520809;

	// I want the last 520809 digits so copy them into a big ol' array
	// in reverse order. 
	let mut v: Vec<u8> = vec![0;num_of_digits];
	let mut j = num_of_digits-1;
	'outer: loop {
		for x in (0..650).rev() {
			v[j] = signal[x];
			j -= 1;
			if j == 0 { break 'outer }
		}
	}
	v[0] = 7; // sigh....

	for j in 0..100 {	
		v = do_fft_q2(v, num_of_digits);
	}

	println!("{} {} {} {} {} {} {} {}",
		v[0], v[1], v[2], v[3], v[4], v[5], v[6], v[7]);	
}
