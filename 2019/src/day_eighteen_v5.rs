use std::fs;
use std::collections::{ BinaryHeap, HashMap, HashSet, VecDeque };
use std::cmp::Ordering;
use std::hash::{Hash, Hasher};

#[derive(Debug, Eq, PartialEq)]
struct Vertex {
	ch: char,
	key: u64,
	known: bool,
	distance: u64,
}

impl Vertex {
	fn new(ch: char, key: u64) -> Vertex {
		Vertex { ch, key, known: false, distance: u64::max_value() }
	}
}

impl Ord for Vertex {
	fn cmp(&self, other: &Vertex) -> Ordering {
		other.distance.cmp(&self.distance)
	}
}

impl PartialOrd for Vertex {
	fn partial_cmp(&self, other: &Vertex) -> Option<Ordering> {
		Some(self.cmp(other))
	}
}

impl Hash for Vertex {
	fn hash<H: Hasher>(&self, state: &mut H) {
		self.ch.hash(state);
		self.key.hash(state);
	}
}
 
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

fn find_start(grid: &Vec<Vec<char>>) -> Option<(usize, usize)> {
	for r in 0..grid.len() {
		for c in 0..grid[r].len() {
			if grid[r][c] == '@' {
				return Some((r, c));
			}
		}
	}

	None
}

fn fetch_grid(filename: &str) -> Vec<Vec<char>> {
    let mut grid = Vec::new();
    let prog_txt = fs::read_to_string(filename).unwrap();

    for line in prog_txt.split('\n') {
        grid.push(line.trim().chars().map(|ch| ch).collect::<Vec<char>>());
    }

    grid
}

type Graph = HashMap<(char, u64), HashMap<(char, u64), u32>>;
static DIRS: [(i32, i32); 4] = [(-1, 0), (1, 0), (0, 1), (0, -1)];

// Find the next keys from current key using a basic flood fill
// I need to cache/memoize this stuff for the larger problems, I think.
// For example in in test_2.txt, the path b-f-c is exactly the same as
// f-b-c. And b-f-c-e is the same as c-f-b-e and b-c-f-e, etc
// So store a cached path of masks traversed and don't pursue a path
// if the mask is already in the cache. Is it enough to store a HashSet<>
// of masks?
pub fn find_keys_from(r: usize, c: usize, mask: u64, grid: &Vec<Vec<char>>, 
		graph: &mut Graph, cache: &mut HashMap<u64, u32>) {
	let mut visited = HashSet::new();
	let mut queue = VecDeque::new();
	queue.push_back((r, c, 0));

	if graph.keys().len() > 20 {
		return;
	}

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
	
			if visited.contains(&(nr, nc)) {
				continue;
			}

			if ch == '.' {
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
					
					//if cache.contains_key(&new_mask) && distance > cache[&new_mask] {
						//continue;
					//}

					//let ce = cache.entry(new_mask).or_insert(distance);
					//*ce = distance;
					
					let v = graph.entry((prev_ch, mask))
								 .or_insert(HashMap::new());
					v.insert((ch, new_mask), distance);
					find_keys_from(nr, nc, new_mask, &grid, graph, cache);
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

pub fn dijkstras(graph: &mut Graph) {
	// I want to turn the graph I built in the flood fill into
	// a set of Vertexes so that it matches the textbook dijkstra's
	// algorithm descriptions more directly
	let mut pq = BinaryHeap::new();
	let mut vertexes = HashMap::new();
	for node in graph.keys() {
		let mut v = Vertex::new(node.0, node.1);
		if v.ch == '@' {
			v.distance = 0;
			v.known = true;
			pq.push(&('@', 0));
		} 

		if !vertexes.contains_key(&(v.ch, v.key)) {
			vertexes.insert((v.ch, v.key), v);
		}

		let neighbours = graph.get(&node).unwrap();
		for neighbour in neighbours {
			if !vertexes.contains_key(&neighbour.0) {
				let v = Vertex::new((neighbour.0).0, (neighbour.0).1);
				vertexes.insert((v.ch, v.key), v);
			}
		}
	}

	while pq.len() > 0 {
		let v_name = pq.pop().unwrap();
		let u = vertexes.get(v_name).unwrap();
		let ud = u.distance;

		if let Some(neighbours) = graph.get(&(u.ch, u.key)) {
			for n in neighbours {
				let delta = ud + *n.1 as u64;
				
				// Need to only update the distance only if it is less than the current distance.
				vertexes.entry(*n.0)
					.and_modify(|neighbour| { 
						neighbour.known = true; 
						if delta < neighbour.distance {
							neighbour.distance = delta; 
						}
				});
				pq.push(&n.0);
			}
		}
		
	}

	let mut furthest = 0;
	for v in vertexes.keys() {
		if vertexes[&v].distance > furthest {
			furthest = vertexes[&v].distance;
		}
	}

	println!("Steps to all keys: {}", furthest);
}

pub fn solve_q1() {
	let grid = fetch_grid("./inputs/day18_test_3.txt");
	dump_grid(&grid);
	if let Some(start) = find_start(&grid) {
		println!("{:?}", start);
		let mut graph: Graph = HashMap::new();
		let mut cache: HashMap<u64, u32> = HashMap::new();
		find_keys_from(start.0, start.1, 0, &grid, &mut graph, &mut cache);
		
		/*
		for g in &graph {
			println!("{:?}", g);
		}
		*/
		dijkstras(&mut graph);
	}
}
