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
