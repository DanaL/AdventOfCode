use std::fs;
use std::collections::HashMap;
use std::collections::HashSet;

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

fn is_open(ch: char) -> bool {
    ch == '.' || ch == '@' || (ch >= 'a' && ch <= 'z')
}

fn find_locked(sr: usize, sc: usize, grid: &Vec<Vec<char>>, keys: &HashSet<char>) -> HashSet<char> {
	let mut found: HashSet<char> = HashSet::new();
	let mut visited: HashSet<(usize, usize)> = HashSet::new();
	visited.insert((sr, sc));
    let mut queue: Vec<(usize, usize)> = vec![(sr, sc)];
    while queue.len() > 0 {
        let v = queue.pop().unwrap();
        let (r, c) = v;

		let mut ch = grid[r - 1][c];
		if !visited.contains(&(r - 1, c)) && is_open(ch)  {
			if ch >=  'a' && ch <= 'z' {
				found.insert(ch);
			}
			queue.push((r - 1, c));
			visited.insert((r - 1, c));
		}
		ch = grid[r + 1][c];
		if !visited.contains(&(r + 1, c)) && is_open(ch)  {
			if ch >=  'a' && ch <= 'z' {
				found.insert(ch);
			}
			queue.push((r + 1, c));
			visited.insert((r + 1, c));
		}
		ch = grid[r][c - 1];
		if !visited.contains(&(r, c - 1)) && is_open(ch)  {
			if ch >=  'a' && ch <= 'z' {
				found.insert(ch);
			}
			queue.push((r, c - 1));
			visited.insert((r, c - 1));
		}
		ch = grid[r][c + 1];
		if !visited.contains(&(r, c + 1)) && is_open(ch)  {
			if ch >=  'a' && ch <= 'z' {
				found.insert(ch);
			}
			queue.push((r, c + 1));
			visited.insert((r, c + 1));
		}
    }

	let locked: HashSet<char> = keys.difference(&found).map(|k| *k).collect();
	locked
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


fn shortest_path(sch: char, grid: &Vec<Vec<char>>, paths: &HashMap<(char, char), usize>, 
		to_visit: &HashSet<char>, doors: &HashMap<char, (usize, usize)>) -> usize {
	if to_visit.len() == 0 {
		return 0;
	}

	let mut shortest = usize::max_value();
	for k in to_visit {
		let mut next = to_visit.clone();
		next.remove(&k);
		let mut path_cost = *paths.get(&(sch, *k)).unwrap();
		path_cost += shortest_path(*k, grid, paths, &next, doors);
		if path_cost < shortest {
			shortest = path_cost;
		}	
	}

	shortest
}

pub fn solve_q1_v3() {
    let mut grid = fetch_grid();
	dump_grid(&grid, 0);

	let mut distances: HashMap<(char, char), usize> = HashMap::new();	
	let mut keys: HashSet<char> = HashSet::new();
	let mut doors: HashMap<char, (usize, usize)> = HashMap::new();
	let mut start_r: usize = 0;
	let mut start_c: usize = 0;
    for r in 0..grid.len() {
        for c in 0..grid[r].len() {
			if grid[r][c] == '@' {
				start_r = r;
				start_c = c;
				compute_paths(r, c, &grid, &mut distances);
			} else if grid[r][c] >= 'a' && grid[r][c] <= 'z' {
				keys.insert(grid[r][c]);
				compute_paths(r, c, &grid, &mut distances);
			} else if grid[r][c] >= 'A' && grid[r][c] <= 'Z' {
				doors.insert(grid[r][c], (r, c));
			}
		}
	}

	let mut locked = find_locked(start_r, start_c, &grid, &keys);
	let mut avail: HashSet<char> = keys.difference(&locked).map(|k| *k).collect();
	/*
	let mut avail: HashSet<char> = HashSet::new();
	avail.insert('b');
	avail.insert('c');
	avail.insert('e');
	avail.insert('f');
	avail.insert('g');
	avail.insert('a');
	avail.insert('h');
	*/
	println!("{:?}", doors);
	println!("{}", shortest_path('@', &grid, &distances, &avail, &doors));
}

