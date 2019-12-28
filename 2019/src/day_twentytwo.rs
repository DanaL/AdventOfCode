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
			let count = pieces[1].parse::<i32>().unwrap();
			deck.cut(count);
		} else if pieces[1] == "with" {
			let inc = pieces[3].parse::<usize>().unwrap();
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
