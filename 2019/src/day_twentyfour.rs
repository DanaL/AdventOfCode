use std::collections::VecDeque;
use std::collections::HashSet;

fn check(r: usize, c: usize, grid: &[[char; 5]; 5]) -> char {
	let mut bug_count = 0;
	if r > 0 && grid[r - 1][c] == '#' { bug_count += 1 }
	if r < grid.len() - 1 && grid[r + 1][c] == '#' { bug_count += 1 }
	if c > 0 && grid[r][c - 1] == '#' { bug_count += 1 }
	if c < grid[0].len() - 1 && grid[r][c + 1] == '#' { bug_count += 1 }

	if grid[r][c] == '#' && bug_count != 1 {
		return '.'
	}

	if grid[r][c] == '.' && (bug_count == 1 || bug_count == 2) {
		return '#'
	}

	grid[r][c]
}

fn iterate(grid: &[[char; 5]; 5]) -> [[char; 5]; 5] {
	let mut ng = grid.clone();
	for r in 0..grid.len() {
		for c in 0..grid.len() {
			ng[r][c] = check(r, c, grid);
		}
	}

	ng
}

fn score(grid: &[[char; 5]; 5]) -> u64 {
	let mut v = Vec::new();
	grid.into_iter().map(|row| v.extend_from_slice(row));
	let mut s = 0;
	let mut ex = 0;
	for r in 0..grid.len() {
		for c in 0..grid[r].len() {
			if grid[r][c] == '#' {
				s += u64::pow(2, ex);
			}
			ex += 1;
		}
	}
	
	s
}

pub fn solve_q1() {
	let mut ratings = HashSet::<u64>::new();	
	let mut state: [[char; 5]; 5] = [
		['#', '.', '#', '.', '.'],
		['.', '#', '.', '#', '.'],
		['#', '.', '.', '.', '#'],
		['.', '#', '.', '.', '#'],
		['#', '#', '.', '#', '.']];
	ratings.insert(score(&state));

	loop {
		state = iterate(&state);
		let sc = score(&state);
		if ratings.contains(&sc) {
			println!("Q1: {}", sc);
			break;
		}
		ratings.insert(sc);
	}
}

fn dump_q2(layers: &VecDeque<[[bool; 5]; 5]>) {
	for j in 0..layers.len() {
		println!("Layer {}:", j);
		for row in &layers[j] {
			let s: String = row.into_iter().map(|c| if *c { '#' } else { '.' }).collect();
			println!("{}", s);
		}
	}
}

fn get_empty_grid() -> [[bool; 5]; 5] {
	let mut grid: [[bool; 5]; 5] = [
		[false, false, false, false, false],
		[false, false, false, false, false],
		[false, false, false, false, false],
		[false, false, false, false, false],
		[false, false, false, false, false]];

	grid
}

fn check_q2(layer_num: usize, r: usize, c: usize, layers: &mut VecDeque<[[bool; 5]; 5]>) {
	let mut bug_count = 0;
	let layer = &mut layers[layer_num];	

	if layer_num > 0 {
		// If we are above layer 0, the first row of this grid is adjacent to the middle column 
		// of the 2nd row of the previous layer
		if r == 0 && layers[layer_num - 1][1][2] { bug_count += 1 };
		// Ditto the bottom row and the middle column of the 4th row of previous.
		if r == 4 && layers[layer_num - 1][3][2] { bug_count += 1 };
		// Similarly for the encompassing columns
		if c == 0 && layers[layer_num - 1][2][1] { bug_count += 1 };
		if c == 0 && layers[layer_num - 1][2][3] { bug_count += 1 };
	}

	// if we are not at the last layer in the queue, the squares surround the centre (2, 2)
	// are adjacent to the squares in the recursed layer
	if layer_num < layers.len() - 1 {
		if r == 1 && c == 2 {
			for j in 0..5 { 
				if layers[layer_num + 1][0][j] { bug_count += 1 }
			}
		} 
		if r == 3 && c == 2 {
			for j in 0..5 { 
				if layers[layer_num + 1][4][j] { bug_count += 1 }
			}
		} 
		if r == 2 && c == 1 {
			for k in 0..5 { 
				if layers[layer_num + 1][k][0] { bug_count += 1 }
			}
		} 
		if r == 2 && c == 3 {
			for k in 0..5 { 
				if layers[layer_num + 1][k][4] { bug_count += 1 }
			}
		} 
	}

	// Okay, after potentially checking the layers encompassing and 
	// encompassed by this later, later's consider the the point's neighbours
	// on this later (which is basically the same as q1)
}

pub fn solve_q2() {
	let mut state: [[bool; 5]; 5] = [
		[false, false, false, false, true],
		[true, false, false, true, false],
		[true, false, false, true, true],
		[false, false, true, false, false],
		[true, false, false, false, false]];
	
	let mut layers = VecDeque::new();
	layers.push_back(state);
	
	for _ in 0..10 {
		// Since I'm always going to have to add a blank grid to the beginning and end
		// of the queue of layers. (I think...)
		layers.push_front(get_empty_grid());
		layers.push_back(get_empty_grid());
		check_q2(0, 0, 0, &mut layers);
		dump_q2(&layers);
		break;
	}	
}
