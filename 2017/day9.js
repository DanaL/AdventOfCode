const fs = require('fs').promises;

let readInput = async (f) => {
    let stuff = await fs.readFile(f);
    let lines = stuff.toString().split('\n').filter(l => l.length > 0);

    return lines;
}

let parse = (stream) => {
	let depth = 0, score = 0, garbage_count = 0;
	let stack = [];
	let in_garbage = false;
	for (let pos = 0; pos < stream.length; pos++) {
		if (in_garbage) {
			switch(stream[pos]) {
				case ">":
					in_garbage = false;
					break;
				case "!":
					++pos;
					break;
				default:
					++garbage_count;
			}
		}
		else {
			switch (stream[pos]) {
				case "{":
					stack.push(++depth);
					break;
				case "}":
					score += depth--;
					break;
				case "!":
					++pos;
					break;
				case "<":
					in_garbage = true;
					break;
			}
		}
	}

	console.log("Q1: " + score);
	console.log("Q2: " + garbage_count);
}

let main = async () => {
    const lines = await readInput("stream.txt");

	parse(lines[0]);
}

main();
