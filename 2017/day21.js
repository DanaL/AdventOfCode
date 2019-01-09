const fs = require('fs').promises;
const util = require("./common.js");

let readInput = async (f) => {
    let stuff = await fs.readFile(f);
    let lines = stuff.toString().split('\n').map(l => l.trim()).filter(l => l.length > 0);

    return lines;
}

let expand_rules = (rules) => {
	const expanded = util.copy_obj(rules);
	for (let rule of rules) {
		let pieces = rule[0].split("/");
		let mirrored = pieces.map(p => p.split("").reverse().join("")).join("/")
		console.log(rule[0]);
		let flipped = pieces.reverse().join("/");
		//let mirrored = pieces.map(p => p.split("").reverse().join("")).join("/")
		let mirrored_flipped = flipped.split("/").map(p => p.split("").reverse().join("")).join("/")
		console.log(flipped);
		console.log(mirrored);
		console.log("");
	}

	return expanded;
}

let main = async () => {
    const lines = await readInput("transforms.txt");
	
	let rules = [["../.#", "##./#../..."], [".#./..#/###", "#..#/..../..../#..#"]];
	//console.log(rules);
	rules = expand_rules(rules);
}

main();
