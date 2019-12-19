use std::fs;
use std::collections::HashMap;
use std::collections::HashSet;
use std::collections::VecDeque;

#[derive(Debug, Clone)]
struct KeyInfo {
    d: usize,
    locks: u32,
}

impl KeyInfo {
    pub fn new(d: usize, locks: u32) -> KeyInfo {
        KeyInfo { d, locks }
    }
}

fn dump_grid(grid: &Vec<Vec<char>>) {
	for row in grid {
		let line: String = row.into_iter().collect();
		println!("{}", line);
	}
}

fn fetch_grid() -> Vec<Vec<char>> {
    let mut grid = Vec::new();
    let prog_txt = fs::read_to_string("./inputs/day18_test.txt").unwrap();

    for line in prog_txt.split('\n') {
        grid.push(line.trim().chars().map(|ch| ch).collect::<Vec<char>>());
    }

    grid
}

// Breadth first search to compute all paths + locks in the way
fn compute_distances(sr: usize, sc: usize, grid: &Vec<Vec<char>>, paths: &mut HashMap<(char, char), KeyInfo>) {
	let sch = grid[sr][sc];
	let mut visited: HashSet<(usize, usize)> = HashSet::new();
	visited.insert((sr, sc));
    let mut queue: Vec<((usize, usize), (usize, u32))> = vec![((sr, sc), (0, 0))];

    while queue.len() > 0 {
        let v = queue.pop().unwrap();
        let dist = (v.1).0 + 1;
        let mut locks = (v.1).1;
        let (r, c) = v.0;
		let mut ch = grid[r - 1][c];

		if ch != '#' && !visited.contains(&(r - 1, c)) {
			if ch.is_ascii_lowercase() {
				paths.insert((sch, ch), KeyInfo::new(dist, locks));
			} else if ch.is_ascii_uppercase() {
                locks |= u32::pow(2, ch as u32 - 'A' as u32);
            }
			queue.push(((r - 1, c), (dist, locks)));
			visited.insert((r - 1, c));
		}
		ch = grid[r + 1][c];
		if ch != '#' && !visited.contains(&(r + 1, c)) {
            if ch.is_ascii_lowercase() {
				paths.insert((sch, ch), KeyInfo::new(dist, locks));
			} else if ch.is_ascii_uppercase() {
                locks |= u32::pow(2, ch as u32 - 'A' as u32);
            }
			queue.push(((r + 1, c), (dist, locks)));
			visited.insert((r + 1, c));
		}
		ch = grid[r][c - 1];
		if ch != '#' && !visited.contains(&(r, c - 1)) {
            if ch.is_ascii_lowercase() {
				paths.insert((sch, ch), KeyInfo::new(dist, locks));
			} else if ch.is_ascii_uppercase() {
                locks |= u32::pow(2, ch as u32 - 'A' as u32);
            }
			queue.push(((r, c - 1), (dist, locks)));
			visited.insert((r, c - 1));
		}
		ch = grid[r][c + 1];
		if ch != '#' && !visited.contains(&(r, c + 1)) {
            if ch.is_ascii_lowercase() {
				paths.insert((sch, ch), KeyInfo::new(dist, locks));
			} else if ch.is_ascii_uppercase() {
                locks |= u32::pow(2, ch as u32 - 'A' as u32);
            }
			queue.push(((r, c + 1), (dist, locks)));
			visited.insert((r, c + 1));
		}
    }
}

fn find_shortest(sch: char, grid: &Vec<Vec<char>>, distances: &HashMap<(char, char), KeyInfo>,
        visited: &mut HashSet<char>,
        keys_found: u32) -> usize {
    // First, what nodes should be visit
    let mut to_visit: VecDeque<(char, usize)> = VecDeque::new();
    for k in distances.keys() {
        if k.0 == sch && !visited.contains(&k.1) {
            let ki: &KeyInfo = distances.get(k).unwrap();
            if ki.locks == 0 || ki.locks & keys_found > 0 {
                to_visit.push_back((k.1, ki.d));
            }
        }
    }

    for key in to_visit {
        visited.insert(key.0);
        let found = keys_found | u32::pow(2, key.0 as u32 - 'a' as u32);

        println!("{:?} {}", visited, found);
    }

    0
}

pub fn solve_q1() {
    let mut grid = fetch_grid();
	dump_grid(&grid);

    let mut distances: HashMap<(char, char), KeyInfo> = HashMap::new();
    let mut start_r: usize = 0;
	let mut start_c: usize = 0;
    for r in 0..grid.len() {
        for c in 0..grid[r].len() {
			if grid[r][c] == '@' {
				start_r = r;
				start_c = c;
				compute_distances(r, c, &grid, &mut distances);
			} else if grid[r][c].is_ascii_lowercase() {
				//keys.insert(grid[r][c]);
				compute_distances(r, c, &grid, &mut distances);
			}
		}
	}

    let mut visited: HashSet<char> = HashSet::new();
    visited.insert('@');
    find_shortest('@', &grid, &distances, &mut visited, 0);
    //println!("{:?}", distances);
}