const fs = require("fs").promises;
const util = require("./common.js");

let readInput = async (f) => {
    let stuff = await fs.readFile(f);
    let lines = stuff.toString().split('\n').map(l => l.trim()).filter(l => l.length > 0);

    return lines;
}

let parse_rule = (lines, j) => {
	let write = lines[j].match(/^- Write the value (\d+)\.$/).map(Number)[1];
	let move = lines[j+1].slice(23, 24);
	let next = lines[j+2].slice(22, 23);

	return { write:write, move:move, next:next };
}

let parse = (lines) => {
	const states = util.dict();
	let m = lines[0].match(/^Begin in state ([A-Z])\.$/);
	const initial_state = m[1];
	m = lines[1].match(/^Perform a diagnostic checksum after (\d+) steps\.$/).map(Number);
	const steps = m[1];
	for (let j = 2; j < lines.length; j++) {
		if (lines[j].startsWith("In state")) {
			const state = lines[j].slice(9, 10);
			const rule0 = parse_rule(lines, j+2);
			const rule1 = parse_rule(lines, j+6);
			states.add(state, [rule0, rule1]);
		}
	}

	return [initial_state, steps, states];
}

let new_slot = () => {
	return { val:0, left:null, right:null };
}

let q1 = (turing_machine) => {
	let curr_state = turing_machine[0];
	const steps = turing_machine[1];
	const states = turing_machine[2];
	let tape = new_slot();

	let checksum = 0;	
	for (let s = 0; s < steps; s++) {
		let opts = states.get(curr_state);
		let rule = opts[tape.val];
		tape.val = rule.write;
		curr_state = rule.next;
		if (rule.move === "l") {
			if (tape.left === null)
				tape.left = new_slot();
			let tmp = tape;
			tape = tape.left;
			tape.right = tmp;
		}
		else {
			if (tape.right === null)
				tape.right = new_slot();
			let tmp = tape;
			tape = tape.right;
			tape.left = tmp;
		}
	}

	/* Calculate the checksum */
	let sum = tape.val;
	let ptr = tape.left;
	while (ptr !== null) {
		sum += ptr.val;
		ptr = ptr.left;
	}	
	ptr = tape.right;
	while (ptr !== null) {
		sum += ptr.val;
		ptr = ptr.right;
	}	
	console.log("Q1: " + sum);
}

let main = async () => {
    const lines = await readInput("blueprint.txt");
	q1(parse(lines));
}

main();
