const fs = require("fs").promises;
const util = require("./common.js");

let readInput = async (f) => {
    let stuff = await fs.readFile(f);
    let lines = stuff.toString().split('\n').map(l => l.trim()).filter(l => l.length > 0);

    return lines;
}

let best_path = (components, connector) => {
	const options = components.filter(c => c.a === connector || c.b === connector);
	
	if (options.length === 0)
		return 0;

	const results = [];
	for (let op of options) {
		results.push(op.total + best_path(
			components.filter(c => c.a !== op.a || c.b !== op.b),
				op.a === connector ? op.b : op.a));
	}

	return Math.max(...results);
}

let q1 = (components) => {
	const results = []
	for (let start of components.filter(c => c.a === 0 || c.b === 0)) {
		results.push(start.total + 
			best_path(components.filter(c=>c.a !== start.a || c.b !== start.b), 
				start.a === 0 ? start.b : start.a));
	}

	console.log("Q1: " + Math.max(...results));
}

let parse = (line) => {
	const m = line.match(/^(\d+)\/(\d+)$/).map(Number);
	return { a:m[1], b:m[2], total: m[1] + m[2] }
}

let main = async () => {
    const lines = await readInput("components.txt");
	q1(lines.map(parse));
}

main();
