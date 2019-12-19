use std::fs;
use std::collections::HashMap;
use std::collections::HashSet;

#[derive(Debug)]
struct MazeInfo {
	key_locs: HashMap<char, (usize, usize)>,
	door_locs: HashMap<char, (usize, usize)>,
	sqr_to_coord: HashMap<usize, (usize, usize)>,
	coord_to_sqr: HashMap<(usize, usize), usize>,
	path_costs: HashMap<String, usize>,
}

impl MazeInfo {
	pub fn new() -> MazeInfo {
		MazeInfo { key_locs: HashMap::new(), door_locs: HashMap::new(),
			sqr_to_coord: HashMap::new(), coord_to_sqr: HashMap::new(),
			path_costs: HashMap::new() }
	}

	pub fn key_id(&self, k: char) -> usize {
		let k_loc = self.key_locs.get(&k).unwrap();
		*self.coord_to_sqr.get(&k_loc).unwrap()
	}
}

fn spacing_for_depth(depth: usize) -> String {
	let spaces = vec![' '; depth * 4];
	spaces.into_iter().collect()
}

fn dump_grid(grid: &Vec<Vec<char>>, depth: usize) {
	for row in grid {
		let line: String = row.into_iter().collect();
		println!("{}{}", spacing_for_depth(depth), line); 
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

fn is_open(r: usize, c: usize, grid: &Vec<Vec<char>>) -> bool {
    grid[r][c] == '.' || grid[r][c] == '@' || (grid[r][c] >= 'a' && grid[r][c] <= 'z')
}

fn ff_curr_min_paths_to_keys(sr: usize, sc: usize, grid: Vec<Vec<char>>, keys_left: HashSet<char>, 
		mi: &mut MazeInfo, depth: usize) -> usize {
	if keys_left.len() == 0 {
		return 0;
	}

    // Breadth-first search through the maze. We have an unweighted graph
    // (at least for Part 1) so we needn't fuss with Dijkstra's
    let mut distances: HashMap<usize, usize> = HashMap::new();

    // Buld the table of connections between nodes and store the IDs
    // of all the keys as the goals we want to reach in the maze
    // Rust is just such a verbose language T_T
    let mut queue: Vec<(usize, usize)> = vec![(*mi.coord_to_sqr.get(&(sr, sc)).unwrap(), 0)];
    while queue.len() > 0 {
        let v = queue.pop().unwrap();
        let sq_id = v.0;
        let dist = v.1 + 1;
        let (r, c) = mi.sqr_to_coord.get(&sq_id).unwrap();

        // Check if the adjacent squares are open
        if is_open(r - 1, *c, &grid) {
            let n_id = *mi.coord_to_sqr.get(&(r - 1, *c)).unwrap();
            if !distances.contains_key(&n_id) {
                distances.insert(n_id, dist);
                queue.push((n_id, dist));
            }
        }
        if is_open(r + 1, *c, &grid) {
            let n_id = *mi.coord_to_sqr.get(&(r + 1, *c)).unwrap();
            if !distances.contains_key(&n_id) {
                distances.insert(n_id, dist);
                queue.push((n_id, dist));
            }
        }
        if is_open(*r, c - 1, &grid) {
            let n_id = *mi.coord_to_sqr.get(&(*r, c - 1)).unwrap();
            if !distances.contains_key(&n_id) {
                distances.insert(n_id, dist);
                queue.push((n_id, dist));
            }
        }
        if is_open(*r, c + 1, &grid) {
            let n_id = *mi.coord_to_sqr.get(&(*r, c + 1)).unwrap();
            if !distances.contains_key(&n_id) {
                distances.insert(n_id, dist);
                queue.push((n_id, dist));
            }
        }
    }

	let mut shortest = usize::max_value();
	for k in &keys_left {
		let k_id = mi.key_id(*k);
		if distances.contains_key(&k_id) {
			let k_coord = mi.sqr_to_coord.get(&k_id).unwrap();
			let dist = distances.get(&k_id).unwrap();
			let mut next_grid = grid.clone();

			// Do we need to open a door?
			if mi.door_locs.contains_key(&k.to_ascii_uppercase()) {
				let door_coord = mi.door_locs.get(&k.to_ascii_uppercase()).unwrap();
				next_grid[door_coord.0][door_coord.1] = '.';
			}
			let mut next_keys_left = keys_left.clone(); 
			next_keys_left.remove(&k);
			
			let path_key: String = next_keys_left.clone().into_iter().collect();
			println!("{}", path_key);
			match mi.path_costs.get(&path_key) {
				Some(pc) => return *pc,
				// We haven't been down this route yet
				None => { 
					let path_cost = dist + ff_curr_min_paths_to_keys(k_coord.0, k_coord.1, next_grid, next_keys_left, mi, depth+1);
					if path_cost < shortest {
						shortest = path_cost;
					}
					mi.path_costs.insert(path_key, path_cost);
				},
			}
		}
	}
	
	shortest
}

fn compute_paths(sr: usize, sc: usize, grid: &Vec<Vec<char>>, paths: &mut HashMap<(char, char), usize>) {
	let sch = grid[sr][sc];
	let mut visited: HashSet<(usize, usize)> = HashSet::new();
	visited.insert((sr, sc));
    let mut queue: Vec<((usize, usize), usize)> = vec![((sr, sc), 0)];
    while queue.len() > 0 {
        let v = queue.pop().unwrap();
        let dist = v.1 + 1;
        let (r, c) = v.0;
		let mut ch = grid[r - 1][c];
		if ch != '#' && !visited.contains(&(r - 1, c)) {
			if ch >=  'a' && ch <= 'z' {
				paths.insert((sch, ch), dist);
			}
			queue.push(((r - 1, c), dist));
			visited.insert((r - 1, c));
		}
		ch = grid[r + 1][c];
		if ch != '#' && !visited.contains(&(r + 1, c)) {
			if ch >=  'a' && ch <= 'z' {
				paths.insert((sch, ch), dist);
			}
			queue.push(((r + 1, c), dist));
			visited.insert((r + 1, c));
		}
		ch = grid[r][c - 1];
		if ch != '#' && !visited.contains(&(r, c - 1)) {
			if ch >=  'a' && ch <= 'z' {
				paths.insert((sch, ch), dist);
			}
			queue.push(((r, c - 1), dist));
			visited.insert((r, c - 1));
		}
		ch = grid[r][c + 1];
		if ch != '#' && !visited.contains(&(r, c + 1)) {
			if ch >=  'a' && ch <= 'z' {
				paths.insert((sch, ch), dist);
			}
			queue.push(((r, c + 1), dist));
			visited.insert((r, c + 1));
		}
    }
}

pub fn solve_q1_v3() {
    let mut grid = fetch_grid();
	dump_grid(&grid, 0);

	let mut distances: HashMap<(char, char), usize> = HashMap::new();	
	let mut start_r: usize = 0;
	let mut start_c: usize = 0;
    for r in 0..grid.len() {
        for c in 0..grid[r].len() {
			if grid[r][c] == '@' {
				start_r = r;
				start_c = c;
				compute_paths(r, c, &grid, &mut distances);
			} else if grid[r][c] == '@' || (grid[r][c] >= 'a' && grid[r][c] <= 'z') {
				compute_paths(r, c, &grid, &mut distances);
			}
		}
	}

	println!("{:?}", distances);
}

pub fn solve_q1() {
    let mut grid = fetch_grid();
	dump_grid(&grid, 0);

	let mut id = 0;
	let mut mi = MazeInfo::new();
    let mut start_r: usize = 0;
    let mut start_c: usize = 0;
	let mut keys: HashSet<char> = HashSet::new();
    for r in 0..grid.len() {
        for c in 0..grid[r].len() {
			let ch = grid[r][c];
            if ch == '@' {
                start_r = r;
                start_c = c;
            }
			if ch != '#' {
                mi.coord_to_sqr.insert((r, c), id);
                mi.sqr_to_coord.insert(id, (r, c));
				id += 1;
			}
			if ch >= 'a' && ch <= 'z' {
				mi.key_locs.insert(ch, (r, c));
				keys.insert(ch);
			}
			if ch >= 'A' && ch <= 'Z' {
				mi.door_locs.insert(ch, (r, c));
			}
        }
    }

    let path = ff_curr_min_paths_to_keys(start_r, start_c, grid.clone(), keys, &mut mi, 0);
	println!("Q1: {}", path);
	println!("\n\n");
	println!("{:?}", mi.path_costs);
}

