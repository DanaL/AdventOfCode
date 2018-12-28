const fs = require('fs').promises;

let readInput = async () => {
    let stuff = await fs.readFile('programs_sm.txt');
    let lines = stuff.toString().split('\n');
	lines = lines.filter(l => l.length > 0);

    return lines;
}

let add_program = (programs, prgm) => {
	if (!programs.includes(prgm))
		programs[prgm] = { name:prgm, dependencies:[] };
}

// Format will be:
//  { name:"ajdf", dependencies:[] }
let q1 = (prgrms) => {
	programs = [];
	for (line of prgrms) {
		let words = line.split(" ");
		add_program(programs, words[0]);
		words.slice(3).forEach(w => {
			let depends_on = w.slice(0, 4);
			add_program(programs, depends_on);
			programs[depends_on].dependencies.push(words[0]);
		});
	}
	
	console.log(programs);
}

let main = async () => {
    const lines = await readInput();

	q1(lines);
}

main();
