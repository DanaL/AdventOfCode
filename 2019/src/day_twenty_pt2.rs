use std::fs;
use std::collections::BinaryHeap;
use std::collections::HashMap;
use std::collections::HashSet;
use std::collections::VecDeque;

#[derive(Debug)]
struct Node {
	name: String,
	neighbours: Vec<(String, u32, i32)>,
}

impl Node {
	pub fn new(name: String) -> Node {
		Node { name, neighbours: Vec::new() }
	}
}

fn fetch_map(filename: &str) -> Vec<Vec<char>> {
    let mut grid = Vec::new();
    let prog_txt = fs::read_to_string(filename).unwrap();

    for line in prog_txt.split('\n') {
        grid.push(line.chars().filter(|ch| *ch != '\r').collect::<Vec<char>>());
    }

    grid
}

// My overly verbose code to parse the node names out of the file.  The simplest way 
// to find node names I could think of was to sweep sides across rows, then
// down the columns.
fn find_all_nodes(grid: &Vec<Vec<char>>) -> Vec<(String, (usize, usize))> {
    let mut nodes = Vec::new();

    for r in 0..grid.len() - 1 {
        let mut c = 0;
        let len = grid[r].len() - 1;
        while c < len {
            if grid[r][c].is_ascii_uppercase() && grid[r][c + 1].is_ascii_uppercase() {
                let mut name: String = String::from("");
                name.push(grid[r][c]);
                name.push(grid[r][c + 1]);

                let mut portal = (0, 0);
                if c > 0 && c < len && grid[r][c - 1] == '.' {
					portal.0 = r;
                	portal.1 = c - 1;
                }
                if c < len - 2 && grid[r][c + 2] == '.' {
					portal.0 = r;
					portal.1 = c + 2;
                }

                c+= 1;
				nodes.push((name, portal));	
            }
            c += 1;
        }
    }
	
	for c in 0..grid[0].len() - 1 {
        let mut r = 0;
        let len = grid.len() - 1;
        while r < len {
            if grid[r][c].is_ascii_uppercase() && grid[r + 1][c].is_ascii_uppercase() {
                let mut name: String = String::from("");
                name.push(grid[r][c]);
                name.push(grid[r + 1][c]);

                let mut portal = (0, 0);
                if r > 0 && r < len && grid[r - 1][c] == '.' {
                    portal.0 = r - 1;
                    portal.1 = c;
                }
                if r < len - 2 && grid[r + 2][c] == '.' {
                    portal.0 = r + 2;
					portal.1 = c;
                }

                r+= 1;
				nodes.push((name, portal));	
            }
            r += 1;
        }
    }
	
    nodes
}

fn is_inner(r: i32, c: i32, grid: &Vec<Vec<char>>) -> bool {
	r > 2 && c > 2 && r < (grid.len() - 4) as i32 && c < (grid[0].len() - 4) as i32
}

fn build_graph(grid: &Vec<Vec<char>>, nodes: &Vec<(String, (usize, usize))>) -> HashMap<String, Node> {
	// Okay we have node name -> locations in a hash map but it'll also be useful to 
	// have location -> name
	let mut graph: HashMap<String, Node> = HashMap::new();
	let mut locations: HashMap<(usize, usize), String> = HashMap::new();
	for n in nodes {
		let mut name = n.0.to_string();
		if is_inner((n.1).0 as i32, (n.1).1 as i32, grid) {
			name.push('i');
		} else {
			name.push('o');
		}
		graph.insert(name.to_string(), Node::new(name.to_string()));
		locations.insert(n.1, name.to_string());
	}

	let dirs = vec![(-1 ,0), (1, 0), (0, -1), (0, 1)];
	// Okay, for each location, we flood fill through the maze and find any reachable nodes
	for n0 in nodes {
		let mut name = n0.0.to_string();
		if is_inner((n0.1).0 as i32, (n0.1).1 as i32, grid) {
			name.push('i');
		} else {
			name.push('o');
		}
		
		let loc = n0.1;
		let mut visited: HashSet<(usize, usize)> = HashSet::new();
		let mut to_visit: VecDeque<((usize, usize), u32)> = VecDeque::new();
		to_visit.push_back((loc, 0));
		
		while to_visit.len() > 0 {
			let n1 = to_visit.pop_front().unwrap();
			let curr = n1.0;
			visited.insert(curr);
			let d = n1.1;
	
			// check the surrounding sqs (NSEW) to see if they are hallways
			for dir in &dirs {
				let nr = curr.0 as i32 + dir.0;
				let nc = curr.1 as i32 + dir.1;
				let nloc = (nr as usize, nc as usize);
				
				if locations.contains_key(&nloc) {
					let node_name = locations.get(&nloc).unwrap();	
					if node_name != &name {
						let lvl_delta = if is_inner(nr, nc, grid) {
							1
						} else {
							-1
						};
							
						graph.entry(name.to_string())
							.or_insert(Node::new(name.to_string()))
							.neighbours.push((node_name.to_string(), d + 1, lvl_delta));
					}
				} else if !visited.contains(&nloc) && grid[nr as usize][nc as usize] == '.' {
					to_visit.push_back((nloc, d + 1));	
				}
			}
		}	
	}

	graph
}

fn djikstra2(graph: HashMap<String, Node>, start: &str, end: &str) -> u32 {
	let mut visited: HashSet<(String, String, u32, u32)> = HashSet::new();
	let mut queue: BinaryHeap<(i32, String, u32, String, u32, i32)> = BinaryHeap::new();

	queue.push((0, "".to_string(), 0, start.to_string(), 0, 0));
	while queue.len() > 0 {
		let n = queue.pop().unwrap();
		let d = -1 * n.0 as i32;

		visited.insert((n.1.to_string(), n.3.to_string(), n.2, n.4));

		if n.4 == 0 && n.3 == end {
			println!("Hurrah we found the end!");
			return i32::abs(n.0) as u32 - 1;
		} else {
			let v = graph.get(&n.3).unwrap();
			for v2 in &v.neighbours {
				let v2_lvl = if v2.0 == end {
					0
				} else {
					v2.2 + n.4 as i32
				};

				if visited.contains(&(n.3.to_string(), v2.0.to_string(), n.4, v2_lvl as u32)) {
					continue;
				}

				// The outer gates on level 0 don't exist, except for AA and ZZ and likewise
				// AA and ZZ don't exist on levels above 0
				if v2.0 == "AAo" || (n.2 == 0 && v2.2 == -1) || (n.4 != 0 && v2.0 == "ZZo") {
					continue;
				}

				let neg_d = -1 * (v2.1 as i32 + d); // negative distance to trick rust's BinaryHeap

				// I named my games XQo and XQi to indicate if they are inner or outer. So if you 
				// go through an inner gate, you should appear at the corresponding outer gate
				let mut flipped_name = v2.0.to_string();
				flipped_name = match flipped_name.find('i') {
					Some(_) => flipped_name.replace("i", "o"),
					None => 
						if flipped_name != "ZZo" { 
							flipped_name.replace("o", "i")
						 } else {
							flipped_name
						},
				};
				queue.push((neg_d - 1, n.3.to_string(), n.4, flipped_name.to_string(), v2_lvl as u32, v2.2));
			}
		}
	}

	0
}

pub fn solve() {
    let grid = fetch_map("./inputs/day20.txt");
 	let nodes = find_all_nodes(&grid);
	let graph = build_graph(&grid, &nodes);
	
	println!("{}", djikstra2(graph, "AAo", "ZZo"));
}
