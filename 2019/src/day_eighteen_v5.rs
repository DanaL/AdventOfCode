use std::fs;
use std::collections::HashMap;
use std::collections::HashSet;
use std::collections::VecDeque;

// How do store the graph? My nodes are of the form (b,2) (where
// the 2 is the bitmask representing which keys have been acquired so far)
//
// So in one of the examples, (@, 0) would have edges to (b, 2) with distance of 22
// and (a, 1) with distance of 2. (b, 2) would have a vertex to (a, 3) with a d of 24.
// (And technically to (c, 3) w/ d of 28 but we want to ignore that one because we have to
// pass through a anyhow. Then: (a, 3) -> (c ,4), d = 4, etc
//
// Store it as a hash table with key (key, mask) and each entry storing a hash table with distances.
//
// So when I determine (@, 0) reaches (a, 1) w/ d=2 and (b, 2) w/ d = 22:
//
// [(@, 0)] -> [ [(a, 1)] -> 2, [b, 2] -> 22 ]
// [(b, 2)] -> [ [(a, 3)] -> 24]
//
// HashMap<(char, u8), HashMap<(char, u8), u32>>??? Should I stuff it into a Struct?

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
	//let mut graph = HashMap::new();
	let mut visited = HashSet::new();
	let mut queue = VecDeque::new();
	let dirs = vec![(-1, 0), (1, 0), (0, -1), (0, 1)];
	queue.push_back((r, c, 0, r, c, 0));

	while queue.len() > 0 {
		let node = queue.pop_front().unwrap();
		visited.insert((node.0, node.1, node.2));

		// check neighbouring squares for keys or doors
		for d in &dirs {
			let nr = (node.0 as i32 + d.0) as usize;
			let nc = (node.1 as i32 + d.1) as usize;
			let ch = if grid[nr][nc] == '@' { '.'} else { grid[nr][nc] };
			let mut passable = false;

			if (ch == '#') { 
				continue;
			}

			if ch == '.' {
				if !visited.contains(&(nr, nc, node.2)) {
					let new_node = (nr, nc, node.2, node.3, node.4, node.5 + 1);
					queue.push_back(new_node);
				}
			} else if ch >= 'a' && ch <= 'z' {
				let mask = node.2 | to_bitmask(ch);
				if !visited.contains(&(nr, nc, mask)) {
					println!("n0: {} {}", grid[node.3][node.4], node.2);
					println!("n1: {} {}", ch, mask);
					println!("d: {}", node.5+1);
					let new_node = (nr, nc, mask, nr, nc, 0);
					queue.push_back(new_node);

					//if (ch >= 'a' && ch <= 'z') {
					if (ch == 'g') {
						println!("Reached {} from ({}, {}, {}) d={}", ch, node.3, node.4, node.2, node.5 + 1);
					}
				}
				// NEXT GOTTA ADD THE KEYS (OR DOORS?) TO THE GRAPH THAT I'M STORING
				// The idea is to build all the connections between nodes (with key poession
				// as a dimension) and then I can do a hopefully smart search through the graph
				// for the shortest path
			} else {
				// We've reached a door -- do we have the key to keep moving?
				let mask = node.2;
				let door = to_bitmask(ch.to_ascii_lowercase());
				if (mask & door > 0) {
					let new_node = (nr, nc, mask, node.3, node.4, node.5 + 1);
					if !visited.contains(&(nr, nc, mask)) {
						queue.push_back(new_node);
						//println!("Passed through {}", ch);
					}
				}
			}
		}
	}
}

pub fn solve_q1() {
	let mut grid = fetch_grid();
	flood_fill(3, 6, &grid);
}
