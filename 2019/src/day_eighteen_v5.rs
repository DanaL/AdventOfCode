use std::fs;
use std::collections::HashMap;
use std::collections::HashSet;
use std::collections::VecDeque;

fn dump_grid(grid: &Vec<Vec<char>>) {
	for row in grid {
		let line: String = row.into_iter().collect();
		println!("{}", line);
	}
}

fn to_bitmask(ch: char) -> u64 {
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

type Graph = HashMap<(char, u64), HashMap<(char, u64), u32>>;
static DIRS: [(i32, i32); 4] = [(-1, 0), (1, 0), (0, 1), (0, -1)];

// Find the next keys from current key using a basic flood fill
pub fn find_keys_from(r: usize, c: usize, mask: u64, grid: &Vec<Vec<char>>, graph: &mut Graph) {
	let mut visited = HashSet::new();
	let mut queue = VecDeque::new();
	queue.push_back((r, c, 0));

	while queue.len() > 0 {
		let sq = queue.pop_front().unwrap();
		visited.insert((sq.0, sq.1));

		for d in &DIRS {
			let nr = (sq.0 as i32 + d.0) as usize;
			let nc = (sq.1 as i32 + d.1) as usize;
			let ch = if grid[nr][nc] == '@' { '.'} else { grid[nr][nc] };
			let distance = sq.2 + 1;
			
			if ch == '#' {
				continue;
			}
		
			if ch == '.' && !visited.contains(&(nr, nc)) {
				queue.push_back((nr, nc, distance));
			} else if ch >= 'a' && ch <= 'z' {
				// if the key we hit is part of the mask, we've visited it before 
				// and can treat it like a floor space. Otherwise, added the start 
				// and current vertexes to the graph and recurse on the key
				if mask & to_bitmask(ch) > 0 {
					queue.push_back((nr, nc, distance));
				} else {
					let prev_ch = grid[r][c];
					let new_mask = mask | to_bitmask(ch);
				
					let v = graph.entry((prev_ch, mask))
								 .or_insert(HashMap::new());
					v.insert((ch, new_mask), distance);

					find_keys_from(nr, nc, new_mask, &grid, graph);
				}
			} else {
				// We've reached a door -- if we have matching key, keep moving, otherwise
				// treat the door as though it is a wall
				let door = to_bitmask(ch.to_ascii_lowercase());
				if mask & door > 0 && !visited.contains(&(nr, nc)) {
					queue.push_back((nr, nc, distance));
				}
			}
		}
	}
}

pub fn solve_q1() {
	let grid = fetch_grid();
	let mut graph: Graph = HashMap::new();

	find_keys_from(3, 6, 0, &grid, &mut graph);

	for v in &graph {
		println!("{:?}", v);
	}
}
