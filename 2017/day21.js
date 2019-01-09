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
		let flipped_h = util.copy_obj(pieces).map(p => p.split("").reverse().join("")).join("/");
		let flipped_v = util.copy_obj(pieces).reverse().join("/");
		/* now for rotation */
		let rot = new Array(pieces.length).fill("");
		let rev = util.copy_obj(pieces).reverse();
		for (let r = 0; r < rev.length; r++) {
			for (let c = 0; c < rev.length; c++) {
				rot[c] += rev[r][c];
			}
		}
		let rotated = rot.join("/");
		console.log(rule[0]);
		console.log(flipped_h);
		console.log(flipped_v);
		console.log(rotated);
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
