use std::fs;

fn count_ch(v: &[char], t: char) -> u32 {
	v.iter().filter(|&&ch| ch == t).count() as u32
}

pub fn solve_q1() {
	let pixels = fs::read_to_string("./inputs/day8.txt").unwrap().trim().chars().collect::<Vec<_>>();
	let layers: Vec<_> = pixels.chunks(25 * 6).collect();
	let fewest_zeroes = layers.iter().min_by_key(|l| count_ch(&l, '0')).unwrap();
	println!("Q1: {:?}", count_ch(&fewest_zeroes, '1') * count_ch(&fewest_zeroes, '2'));
}

pub fn solve_q2() {
	let mut img = fs::read_to_string("./inputs/day8.txt").unwrap().trim().to_string();
	img = img.replace("0", " ");	
	img = img.replace("1", "#");	
	img = img.replace("2", "@");	

	let mut pixels = vec!['@'; 150];
	let mut i = 0;
	for ch in img.chars() {
		if pixels[i] == '@' {
			pixels[i] = ch;
		}
		i = (i + 1) % 150;
	}

	for n in 1..7 {
		pixels.insert(n * 25 + n - 1, '\n');
	}

	println!("{}", pixels.iter().collect::<String>());
}
