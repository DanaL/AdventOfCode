pub fn manhattan_d(ax: i32, ay: i32, bx: i32, by: i32) -> u32 {
	((ax - bx).abs() + (ay - by).abs()) as u32
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

	fn swap(arr: &mut Vec<i64>, j: usize, k: usize) {
		let tmp = arr[j];
		arr[j] = arr[k];
		arr[k] = tmp;
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
					Permutations::swap(&mut self.arr, 0, self.i);
				} else {
					Permutations::swap(&mut self.arr, self.c[self.i], self.i);
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

