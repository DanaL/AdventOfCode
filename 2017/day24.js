const fs = require("fs").promises;
const util = require("./common.js");

let readInput = async (f) => {
    let stuff = await fs.readFile(f);
    let lines = stuff.toString().split('\n').map(l => l.trim()).filter(l => l.length > 0);

    return lines;
}

let pick_strongest = (connector, components) => {
	const options = components.filter(c => c.a === connector || c.b === connector);
	options.sort((x, y) => y.total - x.total);
	return options.length > 0 ? options[0] : null;
}

let q1 = (components) => {
	let best = 0;
	for (let start of components.filter(c => c.a === 0 || c.b === 0)) {
		let arr = components.slice(0).filter(c => !(c.a === start.a && c.b === start.b));
		const bridge = [start];
		let match = start.a === 0 ? start.b : start.a;

		do {
			let next = pick_strongest(match, arr);
			if (next === null) break;
			arr = arr.filter(c => c.a !== next.a && c.b !== next.b);
			bridge.push(next);
			match = next.a === match ? next.b : next.a; 
		} while (true);

		const str = bridge.map(c => c.total).reduce((total, c) => total + c);
		console.log(str);
		if (str > best) best = str;
	}

	console.log("Q1: " + best);
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
