const fs = require('fs').promises;

let readInput = async () => {
    let stuff = await fs.readFile('programs_sm.txt');
    let lines = stuff.toString().split('\n');
	lines = lines.filter(l => l.length > 0);

    return lines;
}

let add_program = (programs, prgm, weight) => {
	if (!(prgm in programs)) 
		programs[prgm] = { name:prgm, dependencies:[] };

	if (weight > 0)
		programs[prgm].weight = weight;
}

// Format will be:
//  { name:"ajdf", dependencies:[] }
let q1 = (prgrms) => {
	programs = [];
	for (line of prgrms) {
		let words = line.split(" ");
		weight = parseInt(line.match(/(\d+)/g));
		add_program(programs, words[0], weight);
		words.slice(3).forEach(w => {
			let comma = w.indexOf(",");
			let depends_on = w.slice(0, comma > -1 ? comma : w.length);
			add_program(programs, depends_on);
			programs[depends_on].dependencies.push(words[0]);
		});
	}

	for (k in programs) {
		if (programs[k].dependencies.length === 0)
			var bottom = k;	
	}
	console.log("Q1: " + bottom);
}

let main = async () => {
    const lines = await readInput();

	q1(lines);
}

main();
