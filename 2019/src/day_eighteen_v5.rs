use std::fs;
//use std::collections::HashMap;
//use std::collections::HashSet;
use std::collections::VecDeque;

fn dump_grid(grid: &Vec<Vec<char>>) {
	for row in grid {
		let line: String = row.into_iter().collect();
		println!("{}", line);
	}
}

fn to_bitmask(ch: char) -> u8 {
	let j = ch.to_ascii_lowercase() as u8 - 'a' as u8;
	let mut x = 1;
	for _ in 0..j {
		x <<= 1;
	}

	x
}

fn fetch_grid() -> Vec<Vec<char>> {
    let mut grid = Vec::new();
    let prog_txt = fs::read_to_string("./inputs/day18_test.txt").unwrap();

    for line in prog_txt.split('\n') {
        grid.push(line.trim().chars().map(|ch| ch).collect::<Vec<char>>());
    }

    grid
}

fn flood_fill(r: usize, c: usize, grid: &Vec<Vec<char>>) {
	let mut queue = VecDeque::new();
	queue.push_back((r, c, 0));

	while queue.len() > 0 {
		let node = queue.pop_front().unwrap();
		// check neighbouring squares
		
		println!("{:?}", node);
	}
}

pub fn solve_q1() {
	let mut grid = fetch_grid();
	flood_fill(3, 6, &grid);
	println!("{:#08b}", to_bitmask('a'));
	println!("{:#08b}", to_bitmask('b'));
	println!("{:#08b}", to_bitmask('c'));
	println!("{:#08b}", to_bitmask('d'));
}
