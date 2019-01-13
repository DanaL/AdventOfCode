const fs = require("fs").promises;
const util = require("./common.js");

let readInput = async (f) => {
    let stuff = await fs.readFile(f);
    let lines = stuff.toString().split('\n').map(l => l.trim()).filter(l => l.length > 0);

    return lines;
}

let parse = (line) => {
	const m = line.match(/^(\d+)\/(\d+)$/).map(Number);
	return { a:m[1], b:m[2], total: m[1] + m[2] }
}

let port_eq = (p0, p1) => p0.a === p1.a && p0.b === p1.b;

let gen_bridges = (end, unused, total, depth) => {
	bridges.push([total, depth]);
	for (part of unused) {
		if (part.a === end || part.b === end) {
			let leftover = unused.filter(p => !port_eq(part, p));
			let next_end = part.a === end ? part.b : part.a;
			gen_bridges(next_end, leftover, total+part.total, depth+1);
		}
	}
}

let q1 = () => {
	let strongest = 0;
	for (bridge of bridges) {
		if (bridge[0] > strongest) strongest = bridge[0];
	}
	console.log("Q1: " + strongest);
}

let q2 = () => {
	let longest = 0;
	let strongest = 0;
	for (bridge of bridges) {
		if (bridge[1] > longest) {
			longest = bridge[1];
			strongest = bridge[0];
		}
		else if (bridge[1] == longest && bridge[0] > strongest) 
			strongest = bridge[0];		
	}
	console.log(`Q2: ${strongest} (${longest})`);
}

let main = async () => {
    const lines = await readInput("components.txt");
	const parts = lines.map(parse);
	for (p of parts.filter(p => p.a === 0 || p.b === 0)) {
		const unused = parts.filter(pt => !port_eq(p, pt));
		gen_bridges(p.a === 0 ? p.b : p.a, unused, p.total, 0);	
	}
	q1();
	q2();
}

let bridges = [];
main();
