const fs = require('fs').promises;

let readInput = async () => {
    let stuff = await fs.readFile('programs.txt');
    let lines = stuff.toString().split('\n');
	lines = lines.filter(l => l.length > 0);

    return lines;
}

let add_program = (programs, prgm, weight=0) => {
	if (!(prgm in programs)) 
		programs[prgm] = { name:prgm, children:[], parent:null };

	if (weight > 0)
		programs[prgm].weight = weight;
}

// Format will be:
//  { name:"ajdf", dependencies:[] }
let parse_programs = (lines) => {
	programs = [];
	for (line of lines) {
		let words = line.split(" ");
		let weight = parseInt(line.match(/(\d+)/g));
		add_program(programs, words[0], weight);
		words.slice(3).forEach(w => {
			let comma = w.indexOf(",");
			let child = w.slice(0, comma > -1 ? comma : w.length);
			add_program(programs, child);
			programs[words[0]].children.push(child);
			programs[child].parent = words[0];
		});
	}

	return programs;
}

let q1 = (programs) => {
	for (k in programs) {
		if (programs[k].parent === null) {
			return k;
		}
	}
}

let sum_branch = (programs, branch) => {
	let sum = programs[branch].weight;

	for (let child of programs[branch].children)
		sum += sum_branch(programs, child);

	return sum;
}

let find_oddball_child = (programs, n) => {
	const children = programs[n].children;
	if (children.length < 2) return null;
	const weights = {};
	for (let child of children) {
		let sum = sum_branch(programs, child);
		if (!(sum in weights))
			weights[sum] = [child];
		else
			weights[sum].push(child);
	}

	for (let w in weights) {
		if (weights[w].length == 1)
			return weights[w][0];
	}
	
	return null;
}

let calc_balance = (programs, parent, unbalanced) => {
	const children = programs[parent].children;
	for (let child of children) {
		if (child !== unbalanced) {
			var bw = sum_branch(programs, child);
			break;
		}
	}

	let cw = sum_branch(programs, unbalanced);
	let diff = cw > bw ? cw - bw : bw - cw;
	console.log("Q2: " + unbalanced + " needs to be " + (programs[unbalanced].weight - diff));
}

let seek_imbalance = (programs, n) => {
	// check children of current node. If they are in balance, this node is the unbalanced one.
	const unbalanced = find_oddball_child(programs, n);
	if (unbalanced !== null)
		seek_imbalance(programs, unbalanced);
	else 
		calc_balance(programs, programs[n].parent, n);
}

let main = async () => {
    const lines = await readInput();
	const programs = parse_programs(lines);

	head = q1(programs);
	console.log("Q1: " + head);
	seek_imbalance(programs, head);
}

main();
