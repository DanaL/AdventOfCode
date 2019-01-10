const fs = require('fs').promises;
const util = require("./common.js");

let readInput = async (f) => {
    let stuff = await fs.readFile(f);
    let lines = stuff.toString().split('\n').map(l => l.trim()).filter(l => l.length > 0);

    return lines;
}

let parse = (line) => {
	const m = line.match(/^([\.#\/]+) => ([\.#\/]+)$/);
	return [m[1], m[2]];
}

/* rule is of the form, [pattern, result]. We want to store the verticle and horizontal flips
	of the rule in our dictionary of transforms */
let transform = (transforms, rule) => {
	transforms.add(rule[0], rule[1]);
	let pieces = rule[0].split("/");
	let flipped_h = util.copy_obj(pieces).map(p => p.split("").reverse().join("")).join("/");
	transforms.add(flipped_h, rule[1]);
	let flipped_v = util.copy_obj(pieces).reverse().join("/");
	transforms.add(flipped_v, rule[1]);
}

let expand_rules = (rules) => {
	const expanded = util.dict();

	for (let rule of rules) {
		transform(expanded, rule);

		let r = rule[0];
		for (let j = 0; j < 3; j++) {
			let pieces = r.split("/");
			let rot = new Array(pieces.length).fill("");
			let rev = util.copy_obj(pieces).reverse();
			for (let r = 0; r < rev.length; r++) {
				for (let c = 0; c < rev.length; c++) {
					rot[c] += rev[r][c];
				}
			}
			r = rot.join("/");
			transform(expanded, [r, rule[1]]);
		}
	}

	return expanded;
}

let pattern_size = (pattern) => pattern.split("").filter(c => c === "/").length + 1;

let get_sub_sq = (grid, row, col, size) => {
	let sq = [];
	for (let r = row; r < row + size; r++) {
		let str = "";
		for (let c = col; c < col + size; c++) {
			str += grid[r][c];	
		} 
		sq.push(str);
	}	

	return sq.join("/");
}

let merge = (a, b) => {
	const arr_a = a.split("/");
	const arr_b = b.split("/");
	const m = []
	for (let j = 0; j < arr_a.length; j++)
		m.push(arr_a[j] + arr_b[j]);
		
	return m.join("/");
}

let dump = (grid) => grid.split("/").map(l => console.log(l));

let q1 = (rules) => {
	const initial = ".#./..#/###";
	
	/* I need to do five generations of transforming the initial state by the rules:
		if the current state's size is divisible by 2, I need to look up the transform of each 
		2x2 block, and likewise if it is divisible by 3. Essentially, the size is either
		the height or the width of the block. */
	let curr_state = initial;
	for (let gen = 0; gen < 18; gen++) {
		const pieces = curr_state.split("/");
		const divvy = pattern_size(curr_state) % 2 === 0 ? 2 : 3;
		const next_state = []
		for (let r = 0; r < pieces[0].length; r += divvy) {
			let replacement = "";
			for (let c = 0; c < pieces[0].length; c += divvy) {
				let next = rules.get(get_sub_sq(pieces, r, c, divvy)); 
				replacement = replacement.length === 0 ? next : merge(replacement, next);
			}
			next_state.push(replacement);
		}
		curr_state = next_state.join("/");
		console.log(curr_state.split("").filter(c => c === "#").length);
	}
	console.log("Q1: " + curr_state.split("").filter(c => c === "#").length);
}

let main = async () => {
    const lines = await readInput("transforms.txt");
	rules = expand_rules(lines.map(parse));
	q1(rules);
}

main();
