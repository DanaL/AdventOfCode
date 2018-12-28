const fs = require('fs').promises;

let readInput = async () => {
    let stuff = await fs.readFile('jmps.txt');
    let lines = stuff.toString().split('\n');
	
    return lines;
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
	const jmps = lines.map(Number);

	q1(jmps);
}

main();
