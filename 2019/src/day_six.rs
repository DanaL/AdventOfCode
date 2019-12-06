use std::collections::HashMap;
use std::fs;

static TEST_MAP: &str = "COM)B\nB)C\nC)D\nD)E\nE)F\nB)G\nG)H\nD)I\nE)J\nJ)K\nK)L\n";

#[derive(Hash, Eq, PartialEq, Debug)]
struct Node<'a> {
	name: &'a str,
	children: Vec<&'a str>,
}

impl Node<'_> {
	pub fn new(name: &str) -> Node {
		Node { name, children: Vec::new() }
	}
}

fn split_orbit(s: &str) -> (&str, &str) {
	let pieces: Vec<&str> = s.split(")").collect();
	(pieces[0], pieces[1])
}

/*
fn walk_graph(nodes: &HashMap<&str, &Node>, n: &str) {
	println!("{}", n);
	let node = nodes.get(n).unwrap();
	for c in node.children {
		walk_graph(nodes, c);
	}
}
*/

pub fn solve_q1() {
	//let lines =  fs::read_to_string("./inputs/day6.txt").unwrap();
	let lines = TEST_MAP;
	let m: Vec<(&str, &str)> = lines.trim().split("\n")
		.map(|a| split_orbit(a))
		.collect();

	//let mut orbits: HashMap<&str, &Node> = HashMap::new();
	
	//for a in m {
		/*if orbits.contains_key(a.0) {
			let mut n = orbits.get(a.0).unwrap();
			n.children.push(&a.1);
		}*/
		//if !orbits.contains_key(a.0) {
		//	let mut node = Node::new(&a.0);
		//	orbits.insert(a.0, &node);
		//}
		//let n = orbits.entry(a.0).or_insert(&Node::new(a.0));
		//n.children.push(&a.1);
	//}
	
	//walk_graph(&orbits, "COM");
	/*
	let mut n = Node::new("Foo");
	let mut orbits = HashSet::new();

	n.children.push("blah");
	n.children.push("X");
	orbits.insert(&n);
	println!("{:?}", orbits);	
	*/
	//println!("{:?}", m);
}
