const fs = require('fs').promises;

let valid = (phrase) => {
	const words = phrase.split(" ");
	const unique = new Set(words);
	
	return words.length == unique.size;
}

let q1 = (phrases) => {
	console.log("Q1: " + phrases.filter(valid).length);
}

let q2 = (phrases) => {
	const sorted = [];  
	phrases.forEach((phrase) => {
		sorted.push(phrase.split(" ").map(w => w.split("").sort().join("")).join(" "));
	});
	
	console.log("Q2: " + sorted.filter(valid).length);
}

let readInput = async () => {
    let stuff = await fs.readFile('passphrases.txt');
    let lines = stuff.toString().split('\n');
	
    return lines;
}

let main = async () => {
    const lines = await readInput();
	
	q1(lines);
	q2(lines);
}

main();
