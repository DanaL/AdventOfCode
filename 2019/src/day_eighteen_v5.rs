use std::fs;
use std::collections::BinaryHeap;
use std::collections::HashMap;
use std::collections::HashSet;
use std::collections::VecDeque;
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
				
				// Need to only update the distance if it is less than the current distance.
				// (Which will probably be almost fucking impossible in Rust because of borrowing rules)
				let v = vertexes.entry(*n.0)
					.and_modify(|neighbour| { neighbour.known = true; neighbour.distance = delta; });
				pq.push(&n.0);
			}
		}
		
	}

	println!("{:?}", vertexes);	
	//println!("{:?}", vertexes);
}

pub fn solve_q1() {
	let grid = fetch_grid();
	let mut graph: Graph = HashMap::new();

	find_keys_from(3, 6, 0, &grid, &mut graph);
	dijkstras(&mut graph);
}
