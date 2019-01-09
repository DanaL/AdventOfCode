const fs = require('fs').promises;
const util = require("./common.js");

let readInput = async (f) => {
    let stuff = await fs.readFile(f);
    let lines = stuff.toString().split('\n').map(l => l.trim()).filter(l => l.length > 0);

    return lines;
}

/* rule is of the form, [pattern, result]. We want to store the verticle and horizontal flips
	of the rule in our dictionary of transforms */
let transform = (transforms, rule) => {
	transforms.add(rule[0], rule[1]);
	let pieces = rule[0].split("/");
	let flipped_h = util.copy_obj(pieces).map(p => p.split("").reverse().join("")).join("/");
	transforms.add(flipped_h, rule[1]);
	let flipped_v = util.copy_obj(pieces).reverse().join("/");
	transforms.add(flipped_v, rule[1]);
}

let expand_rules = (rules) => {
	const expanded = util.dict();

	for (let rule of rules) {
		transform(expanded, rule);
		/* now for each rotation */
		let r = rule[0];
		for (let j = 0; j < 3; j++) {
			let pieces = r.split("/");
			let rot = new Array(pieces.length).fill("");
			let rev = util.copy_obj(pieces).reverse();
			for (let r = 0; r < rev.length; r++) {
				for (let c = 0; c < rev.length; c++) {
					rot[c] += rev[r][c];
				}
			}
			r = rot.join("/");
			transform(expanded, [r, rule[1]]);
		}
	}

	return expanded;
}

let main = async () => {
    const lines = await readInput("transforms.txt");
	
	let rules = [["../.#", "##./#../..."], [".#./..#/###", "#..#/..../..../#..#"]];
	rules = expand_rules(rules);
	console.log(rules);
}

main();
