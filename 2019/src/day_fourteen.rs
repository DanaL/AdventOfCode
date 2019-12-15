use std::fs;
use std::collections::HashMap;
use regex::Captures;
use regex::Regex;

#[derive(Debug, Clone)]
struct Reaction {
	pub product: (String, u64),
	pub reagents: Vec<(String, u64)>,
}

impl Reaction {
	fn new(r: String, q: u64) -> Reaction {
		Reaction { product: (r, q), reagents: Vec::new() }
	}

	fn insert(&mut self, reagent: (String, u64)) {
		for j in 0..self.reagents.len() {
			if self.reagents[j].0 == reagent.0 {
				self.reagents[j].1 += reagent.1;
				return;
			}
		}

		self.reagents.push(reagent);	
	}

	fn only_ore() -> bool {
		false
	}
}

fn to_reagent(m: &Captures) -> (String, u64) {
	(m.get(2).unwrap().as_str().to_string(), 
		m.get(1).unwrap().as_str().parse::<u64>().unwrap())
}

fn parse_line(line: String) -> Reaction {
	let re: Regex = Regex::new(r"(\d+) ([A-Z]+)").unwrap();
	let reagents: Vec<_> = re.captures_iter(&line).collect();
	let product = to_reagent(&reagents[reagents.len() - 1]);
	let mut reaction = Reaction::new(product.0, product.1);

	for r in 0..reagents.len() - 1 {
		reaction.insert(to_reagent(&reagents[r]));
	}

	reaction
}

fn only_raw_ore(raw: &HashMap<String, Reaction>, recipe: &Reaction) -> bool {
	for r in &recipe.reagents {
		if !raw.contains_key(&r.0) {
			return false;
		}
	}

	return true
}

fn reduce(reactions: &HashMap<String, Reaction>, recipe: &Reaction) -> Reaction {
	let mut new_recipe = Reaction::new(recipe.product.0.to_string(), recipe.product.1);
	for reagent in &recipe.reagents {
		match reactions.get(&reagent.0) {
			None => new_recipe.insert(reagent.clone()),
			Some(v) => 
				for r in &v.reagents {
					new_recipe.insert((r.0.to_string(), r.1 * reagent.1));
				},  
		}
	}

	new_recipe
}

fn reduce_recipes(recipes: Vec<Reaction>) {
	let mut raw: HashMap<String, Reaction> = HashMap::new();
	let mut complex: HashMap<String, Reaction> = HashMap::new();
	for r in &recipes {
		if r.reagents.len() == 1 && r.reagents[0].0 == "ORE" {
			raw.insert(r.product.0.to_string(), r.clone());
		} else {
			complex.insert(r.product.0.to_string(), r.clone());
		}
	}

	let keys: Vec<String> = complex.keys().into_iter().map(|k| k.to_string()).collect();
	loop {
		for k in &keys {
			let recipe = complex.get(k).unwrap();
			if !only_raw_ore(&raw, &recipe) {
				complex.insert(k.to_string(), reduce(&complex, &recipe)); 
			}
		}

		if only_raw_ore(&raw, complex.get("FUEL").unwrap()) {
			break;
		}
	}

	sum_ore_needed(&raw, &complex.get("FUEL").unwrap());
}

fn sum_ore_needed(raw: &HashMap<String, Reaction>, fuel_recipe: &Reaction) {
	println!("{:?}", fuel_recipe);
	let mut sum = 0;
	for r in &fuel_recipe.reagents {
		match raw.get(&r.0) {
			None => panic!("Hmm this shouldn't have happened...{}", r.0),
			Some(v) => sum += ((r.1 as f64 / v.product.1 as f64).ceil() * v.reagents[0].1 as f64) as u64,
		}
	}

	println!("Q1: {} ore needed.", sum);
}

static TEST_INPUT: &str = "9 ORE => 2 A\n8 ORE => 3 B\n7 ORE => 5 C\n3 A, 4 B => 1 AB\n5 B, 7 C => 1 BC\n4 C, 1 A => 1 CA\n2 AB, 3 BC, 4 CA => 1 FUEL";
static TEST_INPUT_2: &str = "157 ORE => 5 NZVS\n165 ORE => 6 DCFZ\n44 XJWVT, 5 KHKGT, 1 QDVJ, 29 NZVS, 9 GPVTF, 48 HKGWZ => 1 FUEL\n12 HKGWZ, 1 GPVTF, 8 PSHF => 9 QDVJ\n179 ORE => 7 PSHF\n177 ORE => 5 HKGWZ\n7 DCFZ, 7 PSHF => 2 XJWVT\n165 ORE => 2 GPVTF\n3 DCFZ, 7 NZVS, 5 HKGWZ, 10 PSHF => 8 KHKGT";

pub fn solve_q1() {
	let input = fs::read_to_string("./inputs/day14.txt")
		.unwrap();
	let recipes: Vec<Reaction> = TEST_INPUT_2
		.trim()
		.split('\n')
		.map(|s| parse_line(s.to_string()))
		.collect();

	reduce_recipes(recipes);
}
