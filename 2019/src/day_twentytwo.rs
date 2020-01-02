use std::collections::VecDeque;
use std::fs;
use std::i32;

struct Deck {
	cards: VecDeque<usize>,
	size: usize,
}

impl Deck {
	fn new(size: usize) -> Deck {
		Deck { size, cards: (0..size).collect() }
	}

	fn deal(&mut self) {
		let mut stack = VecDeque::with_capacity(self.size);

		for j in 0..self.size {
			let x = self.cards.pop_back().unwrap();
			stack.push_back(x);
		}

		self.cards = stack;
	}

	fn cut(&mut self, count: i32) {
		if count > 0 {
			self.cards.rotate_left(count as usize);
		} else if count < 0 {
			self.cards.rotate_right(i32::abs(count) as usize);
		}
	}

	fn deal_with_inc(&mut self, inc: usize) {
		let mut stack: VecDeque<usize> = (0..self.size).map(|_| 0).collect();
		let mut pos = 0;

		while self.cards.len() > 0 {
			let a = self.cards.pop_front().unwrap();
			if let Some(b) = stack.get_mut(pos) {
				*b = a;
			}
			pos = (pos + inc) % self.size;
		}

		self.cards = stack;
	}
}

pub fn solve_q1() {
	let input = fs::read_to_string("./inputs/day22.txt").unwrap();
	let mut deck = Deck::new(10007);

	for line in input.trim().split('\n') {
		let pieces: Vec<&str> = line.split(' ').collect();
		if pieces[0] == "cut" {
			let count = pieces[1].trim().parse::<i32>().unwrap();
			deck.cut(count);
		} else if pieces[1] == "with" {
			let inc = pieces[3].trim().parse::<usize>().unwrap();
			deck.deal_with_inc(inc);
		} else {
			deck.deal();
		}
	}

	for j in 0..10007 {
		if deck.cards[j] == 2019 {
			println!("Q1: {}", j);
		}
	}
}

// From https://en.wikipedia.org/wiki/Modular_exponentiation#Right-to-left_binary_method
fn modular_pow(base: i128, exp_val: i128, modulus: i128) -> i128 {
	if modulus == 1 { return 0 }
	let mut result = 1;
	let mut b = base % modulus;
	let mut exp = exp_val;
	while exp > 0 {
		if (exp % 2 == 1) {
			result = (result * b) % modulus;
		}
		exp >>= 1;
		b = (b * b) % modulus;
	}

	result
}

// For Part 2, I followed this blog's tutorial for how to approach simplifying the giant
// values they want us to deal with: https://codeforces.com/blog/entry/72593
// So this function takes each line of the file and converts it to the form: f(x) = (a * x  + b) % m
// x is a card number so applying one of the transforms tells you where card x ends up.
// Steps in the input file can be combined. So if we have f(x) = (a * x + b) % m and
// g(x) = (c * x + d) % m, then g(f(x)) = (c(a*x + b) + d) % m or (c*a*x + c*b + d) % m, which
// gives us new coefficients that I return.
fn compose_lgc(a: i128, b: i128, m: i128, line: String) -> (i128, i128) {
	let pieces: Vec<&str> = line.split(' ').collect();
	let (mut c, mut d) = (0, 0);
	if pieces[0] == "cut" {
		let n = pieces[1].parse::<i128>().unwrap();
		c = 1;
		d = -n;
	} else if pieces[1] == "with" {
		let n = pieces[3].parse::<i128>().unwrap();
		c = n;
		d = 0;
	} else {
		c = -1;
		d = -1;
	}

	let mut coeff1 = a * c;
	coeff1 = coeff1.rem_euclid(m);
	let mut coeff2 = b * c + d;
	coeff2 = coeff2.rem_euclid(m);
	(coeff1, coeff2)
}

pub fn solve_q2() {
	let input = fs::read_to_string("./inputs/day22.txt").unwrap();
	let mut coeffs = (1, 0);
	let m = 119_315_717_514_047;
	for line in input.trim().split('\n') {
		coeffs = compose_lgc(coeffs.0, coeffs.1, m, line.trim().to_string());
	}

	println!("{:?}", coeffs);

	let k: i128 = 101_741_582_076_661;

	// Now, we are shuffling our deck of m cards many trillion (k) times
	let a: i128 = coeffs.0;
	let b: i128 = coeffs.1;
	let ap = modular_pow(a, k, m);
	let v = (ap * 2020    ); //+ b * (ap + m - 1) * (modular_pow(a - 1, m - 2, m)) + m) % m;

	let ff = modular_pow(a - 1, m -2 , m);
	println!("{}", (b * (ap + m - 1)) % m ) * ff;
	println!("{}", v);
}
