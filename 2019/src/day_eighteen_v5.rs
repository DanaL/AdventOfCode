use std::fs;
//use std::collections::HashMap;
use std::collections::HashSet;
use std::collections::VecDeque;

fn dump_grid(grid: &Vec<Vec<char>>) {
	for row in grid {
		let line: String = row.into_iter().collect();
		println!("{}", line);
	}
}

fn to_bitmask(ch: char) -> u8 {
	if ch < 'a' || ch > 'z' {
		return 0;
	}

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
	let mut visited = HashSet::new();
	let mut queue = VecDeque::new();
	let dirs = vec![(-1, 0), (1, 0), (0, -1), (0, 1)];
	queue.push_back((r, c, 0));

	while queue.len() > 0 {
		let node = queue.pop_front().unwrap();
		println!("{:?}", node);
		visited.insert(node.clone());

		// check neighbouring squares for keys or doors
		for d in &dirs {
			let nr = (node.0 as i32 + d.0) as usize;
			let nc = (node.1 as i32 + d.1) as usize;
			let ch = if grid[nr][nc] == '@' { '.'} else { grid[nr][nc] };
			if (ch == '#') { continue }
			else if (ch == '.' || ch >= 'a' && ch <= 'z') {
				let mask = node.2 | to_bitmask(ch);
				let new_node = (nr, nc, mask);
				if !visited.contains(&new_node) {
					queue.push_back(new_node);
				}

				// NEXT GOTTA ADD THE KEYS (OR DOORS?) TO THE GRAPH THAT I'M STORING
				// The idea is to build all the connections between nodes (with key poession
				// as a dimension) and then I can do a hopefully smart search through the graph
				// for the shortest path
			}
			else {
				println!("  -> {}", ch);
			}
		}
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
