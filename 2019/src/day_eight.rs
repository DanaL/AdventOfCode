use std::fs;

pub fn solve_q1() {
	let img = fs::read_to_string("./inputs/day8.txt").unwrap().trim().to_string();

	let mut count = 0;
	let mut fewest_zeroes = u32::max_value();
	let (mut zeroes, mut ones, mut twos) = (0, 0, 0);
	let mut calc = 0;
	for ch in img.chars() {
		count += 1;
		match ch  {
			'0' => zeroes += 1,
			'1' => ones += 1,
			'2' => twos += 1,
			_ => panic!("This shouldn't happen."),
		}
		if count % 150 == 0 {
			if zeroes < fewest_zeroes {
				calc = ones * twos;
				fewest_zeroes = zeroes;
			}
			zeroes = 0;
			ones = 0;
			twos = 0;
		}
	}

	println!("Q1: {}", calc);	
}

pub fn solve_q2() {
	let mut img = fs::read_to_string("./inputs/day8.txt").unwrap().trim().to_string();
	img = img.replace("0", " ").replace("1", "#").replace("2", "@");	

	let mut pixels = vec!['@'; 150];
	let mut i = 0;
	for ch in img.chars() {
		if pixels[i] == '@' {
			pixels[i] = ch;
		}
		i += 1;
		if i % 150 == 0 {
			i = 0;
		}	
	}

	for n in 1..7 {
		pixels.insert(n * 25 + n - 1, '\n');
	}

	println!("{}", pixels.iter().collect::<String>());
}
