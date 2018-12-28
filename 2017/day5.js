const fs = require('fs').promises;

let readInput = async () => {
    let stuff = await fs.readFile('jmps.txt');
    let lines = stuff.toString().split('\n');
	
    return lines;
}

let q2 = (jmps) => {
	var steps = 0;
	var pos = 0;
	while (pos >= 0 && pos < jmps.length) {
		let prev = pos;
		let offset = jmps[pos]
		pos += jmps[pos]; 
		jmps[prev] += offset >= 3 ? -1 : 1;
		++steps;
	}

	console.log("Q2: ", steps);
}

let q1 = (jmps) => {
	var steps = 0;
	var pos = 0;
	while (pos >= 0 && pos < jmps.length) {
		let prev = pos;
		pos += jmps[pos]; 
		jmps[prev]++;
		++steps;
	}

	console.log("Q1: ", steps);
}

let main = async () => {
    const lines = await readInput();

	q1(lines.map(Number));
	q2(lines.map(Number));
}

main();
