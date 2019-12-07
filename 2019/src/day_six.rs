use std::collections::HashMap;
use std::collections::HashSet;
use std::fs;

static TEST_MAP: &str = "COM)B\nB)C\nC)D\nD)E\nE)F\nB)G\nG)H\nD)I\nE)J\nJ)K\nK)L\nK)YOU\nI)SAN";

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

fn make_pairs(input : &str) -> Vec<(String, String)> {
	input.split("\n")
		.map(|a| split_orbit(a.to_string()))
		.collect()
}

pub fn solve_q1() {
	let lines =  fs::read_to_string("./inputs/day6.txt").unwrap();
	let mut orbits: HashMap<String, Node> = HashMap::new();	
	for a in make_pairs(lines.trim()) {		
		orbits.entry(a.0.to_string())
			.or_insert(Node::new(a.0.to_string()))
			.children.push(a.1);	
	}
	println!("Q1: {}", walk_graph(&orbits, "COM", 0));	
}

fn upward_path(uptree: &HashMap<String, String>, node: &str) -> HashMap<String, u32> {
	let mut path: HashMap<String, u32> = HashMap::new();
	let mut steps = 0;
	let mut x: String = node.to_string();

	while uptree.contains_key(&x.to_string()) {
		let n = uptree.get(&x.to_string()).unwrap();
		path.insert(n.to_string(), steps);
		steps += 1;
		x = n.to_string();
	}

	path
}

pub fn solve_q2() {
	//let lines = TEST_MAP;
	let lines =  fs::read_to_string("./inputs/day6.txt").unwrap();
	let mut uptree: HashMap<String, String> = HashMap::new();
	for leaf in make_pairs(lines.trim()) {
		uptree.insert(leaf.1, leaf.0);
	}

	let you = upward_path(&uptree, "YOU");
	let santa = upward_path(&uptree, "SAN");
	let you_set: HashSet<_> = you.keys().collect();
	let santa_set: HashSet<_> = santa.keys().collect();
	let mut shortest = 1_000_000;

	for k in you_set.intersection(&santa_set) {
		let sum = you.get(*k).unwrap() + santa.get(*k).unwrap();
		if sum < shortest {
			shortest = sum;
		}
	}
	println!("Q2: {}", shortest);	
}
