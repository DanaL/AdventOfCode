pub fn manhattan_d(ax: i32, ay: i32, bx: i32, by: i32) -> u32 {
	((ax - bx).abs() + (ay - by).abs()) as u32
}

pub fn lcm(mut a: i64, mut b: i64) -> i64 {
	(a * b).abs() / gcd(a, b)
}

pub fn gcd(mut a: i64, mut b: i64) -> i64 {
	while b != 0 {
		let tmp = a;
		a = b;
		b = tmp % b;
	}
	a
}

pub struct Permutations {
	arr: Vec<i64>,
	i: usize,
	c: Vec<usize>,
	started: bool
}

impl Permutations {
	pub fn new(a: Vec<i64>) -> Permutations {
		Permutations { arr: a.to_vec(), i:0,
			c: vec![0; a.len()], started: false
		}
	}	
}

impl Iterator for Permutations {
	type Item = Vec<i64>;

	fn next(&mut self) -> Option<Self::Item> {
		if !self.started {
			self.started = true;
			return Some(self.arr.to_vec());
		}

		while self.i < self.arr.len() {
			if self.c[self.i] < self.i {
				if self.i % 2 == 0 {
					self.arr.swap(0, self.i);
				} else {
					self.arr.swap(self.c[self.i], self.i);
				}
				self.c[self.i] += 1;
				self.i = 0;
				return Some(self.arr.to_vec());
			} else {
				self.c[self.i] = 0;
				self.i += 1
			}
		}

		None
	}
}

