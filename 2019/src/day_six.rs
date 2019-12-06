use std::collections::HashMap;
use std::fs;

static TEST_MAP: &str = "COM)B\nB)C\nC)D\nD)E\nE)F\nB)G\nG)H\nD)I\nE)J\nJ)K\nK)L\n";

// For Part 1, my first instinct was to put a top-down tree that 
// starts at the root node and each node has a list of child nodes
// and then figure out the total number of orbits by walking through
// all the routes. 

// I later decided an up-tree (the leaf nodes have pointers to their
// parent nodes) was a better way to do. Especially for Part Two

// But I got tripped up on how Rust does borrowing, references, and 
// lifetimes so I decided to try to stick it out with the top-down 
// tree and hopefully come to grok that stuff. (I think it helped)
#[derive(Hash, Eq, PartialEq, Debug)]
struct Node {
	name: String,
	children: Vec<String>,
}

impl Node {
	pub fn new(name: String) -> Node {
		Node { name, children: Vec::new() }
	}
}

fn walk_graph(nodes: &HashMap<String, Node>, n: &str, depth: u32) -> u32 {	
	depth + match nodes.get(n) {
		Some(node) => 
			node.children.iter().map(|c|
				walk_graph(nodes, &c, depth + 1)
			).sum(),
		None => 0
	}	
}

fn split_orbit(s: String) -> (String, String) {
	let pieces: Vec<&str> = s.split(")").collect();
	(pieces[0].trim().to_string(), pieces[1].trim().to_string())
}

pub fn solve_q1() {
	let lines =  fs::read_to_string("./inputs/day6.txt").unwrap();
	//let lines = TEST_MAP;
	let m: Vec<(String, String)> = lines.trim().split("\n")
		.map(|a| split_orbit(a.to_string()))
		.collect();

	let mut orbits: HashMap<String, Node> = HashMap::new();	
	for a in m {		
		orbits.entry(a.0.to_string())
			.or_insert(Node::new(a.0.to_string()))
			.children.push(a.1);	
	}
	println!("Q1: {}", walk_graph(&orbits, "COM", 0));	
}
