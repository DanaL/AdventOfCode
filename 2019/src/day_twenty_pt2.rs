use std::fs;
use std::cmp::Ordering;
use std::collections::BinaryHeap;
use std::collections::HashMap;
use std::collections::HashSet;
use std::collections::VecDeque;

#[derive(Debug)]
struct Node {
	name: String,
	neighbours: Vec<(String, u32, i8)>,
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
fn find_all_nodes(grid: &Vec<Vec<char>>) -> HashMap<String, Vec<(usize, usize)>> {
    let mut nodes: HashMap<String, Vec<(usize, usize)>> = HashMap::new();

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
    			nodes.entry(name).or_insert(Vec::new()).push(portal);
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
    			nodes.entry(name).or_insert(Vec::new()).push(portal);
            }
            r += 1;
        }
    }
	
    nodes
}

fn is_inner(r: i32, c: i32, grid: &Vec<Vec<char>>) -> bool {
	r > 2 && c > 2 && r < (grid.len() - 2) as i32 && c < (grid[0].len() - 2) as i32
}

fn build_graph(grid: &Vec<Vec<char>>, nodes: &HashMap<String, Vec<(usize, usize)>>) -> HashMap<String, Node> {
	// Okay we have node name -> locations in a hash map but it'll also be useful to 
	// have location -> name
	let mut graph: HashMap<String, Node> = HashMap::new();
	let mut locations: HashMap<(usize, usize), String> = HashMap::new();
	for n in nodes.keys() {
		graph.insert(n.to_string(), Node::new(n.to_string()));
		let locs = nodes.get(&n.to_string()).unwrap();
		for loc in locs {
			locations.insert(*loc, n.to_string());
		}
	}

	let dirs = vec![(-1 ,0), (1, 0), (0, -1), (0, 1)];
	// Okay, for each location, we flood fill through the maze and find any reachable nodes
	for n0 in nodes.keys() {
		let locs = nodes.get(&n0.to_string()).unwrap();
		for loc in locs {
			let mut visited: HashSet<(usize, usize)> = HashSet::new();
			let mut to_visit: VecDeque<((usize, usize), u32)> = VecDeque::new();
			to_visit.push_back((*loc, 0));
			
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
						if node_name != n0 {
							let lvl_delta = if is_inner(nr, nc, grid) {
								1
							} else {
								-1
							};

							graph.entry(n0.to_string())
								.or_insert(Node::new(n0.to_string()))
								.neighbours.push((node_name.to_string(), d + 1, lvl_delta));
						}
					} else if !visited.contains(&nloc) && grid[nr as usize][nc as usize] == '.' {
						to_visit.push_back((nloc, d + 1));	
					}
				}
			}	
		}
	}

	graph
}

/*
fn djikstra(graph: HashMap<String, Node>, start: &str, end: &str) -> u32 {
	let mut distances: HashMap<String, u32> = HashMap::new();
	let mut visited: HashSet<String> = HashSet::new();
	let mut nodes = BinaryHeap::new();

	for key in graph.keys() {
		distances.insert(key.to_string(), u32::max_value());
	}	
	distances.insert(start.to_string(), 0);
	let node = graph.get(start).unwrap();
	nodes.push(Foo::new(node.name.to_string(), i32::min_value()));

	while nodes.len() > 0 {
		let n = nodes.pop().unwrap();
		visited.insert(n.name.to_string());
		let curr_d = *distances.get(&n.name).unwrap();
		let node = graph.get(&n.name).unwrap();	
		let adj = &node.neighbours;
		for edge in adj {
			let x = curr_d + edge.1;
			if !visited.contains(&edge.0.to_string()) {
				let y = *distances.get(&edge.0).unwrap();
				if x < y {
					distances.insert(edge.0.to_string(), x + 1);
				}
				nodes.push(Foo::new(edge.0.to_string(), -((edge.1 + curr_d) as i32)));
			}
		}
	}
	
	*distances.get(end).unwrap() - 1
}
*/

pub fn solve() {
    let grid = fetch_map("./inputs/day20_test.txt");
 	let nodes = find_all_nodes(&grid);
	let graph = build_graph(&grid, &nodes);

	println!("{:?}", graph);
}
