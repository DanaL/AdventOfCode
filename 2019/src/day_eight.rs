use std::fs;

fn count_ch(v: &[char], t: char) -> u32 {
	v.iter().filter(|&&ch| ch == t).count() as u32
}

pub fn solve_q1() {
	let pixels = fs::read_to_string("./inputs/day8.txt").unwrap().trim().chars().collect::<Vec<_>>();

	// I learned about chunks() and min_by_key() from another reddit solution and replaced some
	// clunky looking loops with them. Commenting about it so that I remember how these work :P

	// chunks() splits my vector into a list of vectors of the size passed to it, next iterate 
	// over it and call min_by_key(), which takes a lambda expression that calculates the score
	// for the particular element. The element with the lowest score is returned.
	let layers: Vec<_> = pixels.chunks(25 * 6).collect();
	let fewest_zeroes = layers.iter().min_by_key(|l| count_ch(&l, '0')).unwrap();
	println!("Q1: {:?}", count_ch(&fewest_zeroes, '1') * count_ch(&fewest_zeroes, '2'));
}

pub fn solve_q2() {
	let mut img = fs::read_to_string("./inputs/day8.txt").unwrap().trim().to_string();
	img = img.replace("0", " ");	
	img = img.replace("1", "#");	

	// I could probalby iterator this up, but I like how simple and readable this loop is.
	// I store the final pixels in another array. One pass over the input, and when
	// I encounter a clear pixel (2 in the input) in final pixels array, copy the character
	// in the same position from the input string. (The input string is given with the top
	// layer at the beginning of the string)
	let mut pixels = vec!['2'; 150];
	let mut i = 0;
	for ch in img.chars() {
		if pixels[i] == '2' {
			pixels[i] = ch;
		}
		i = (i + 1) % 150;
	}

	for n in 1..7 {
		pixels.insert(n * 25 + n - 1, '\n');
	}

	println!("{}", pixels.iter().collect::<String>());
}
