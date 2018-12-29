const fs = require('fs').promises;

let readInput = async () => {
    let stuff = await fs.readFile('instructions.txt');
    let lines = stuff.toString().split('\n');
	lines = lines.filter(l => l.length > 0);

    return lines;
}

let parse_line = (line, regs) => {
	const i = line.match(/([a-z]+) ([a-z]+) (-?\d+) if ([a-z]+) ([<>=!]+) (-?\d+)/);
	const instr = { out_reg:i[1], delta:i[2], val:parseInt(i[3]), cmp_reg:i[4],
						cmp:i[5], cmp_val:parseInt(i[6]) }; 

	if (!(instr.out_reg in regs))
		regs[instr.out_reg] = 0;	
	if (!(instr.cmg_reg in regs))
		regs[instr.out_reg] = 0;	

	/* Then later I don't need to check if it was inc or dec. I can just add val */
	if (instr.delta === "dec")
		instr.val *= -1;

	return instr;
}

let eval = (instr, regs) => {
	switch (instr.cmp) {
		case "==":
			if (regs[instr.cmp_reg] === instr.cmp_val)
				regs[instr.out_reg] += instr.val;
			break;
		case "!=":
			if (regs[instr.cmp_reg] !== instr.cmp_val)
				regs[instr.out_reg] += instr.val;
			break;
		case ">=":
			if (regs[instr.cmp_reg] >= instr.cmp_val)
				regs[instr.out_reg] += instr.val;
			break;
		case ">":
			if (regs[instr.cmp_reg] > instr.cmp_val)
				regs[instr.out_reg] += instr.val;
			break;
		case "<=":
			if (regs[instr.cmp_reg] <= instr.cmp_val)
				regs[instr.out_reg] += instr.val;
			break;
		case "<":
			if (regs[instr.cmp_reg] < instr.cmp_val)
				regs[instr.out_reg] += instr.val;
			break;
		default:
			console.log("Unknown op: " + instr.cmp);
			break;
	}
}

let q1 = (instructions, regs) => {
	for (instr of instructions) {
		eval(instr, regs);
	}

	console.log("Q1: " + Math.max(...Object.values(regs)));
}

let q2 = (instructions, regs) => {
	let hi = 0;
	for (instr of instructions) {
		eval(instr, regs);
		let m = Math.max(...Object.values(regs));
		if (m > hi)
			hi = m;
	}
	
	console.log("Q2: " + hi);
}

let main = async () => {
    const lines = await readInput();
	const regs = { };
	const instructions = lines.map(line => parse_line(line, regs));

	q1(instructions, JSON.parse(JSON.stringify(regs)));	
	q2(instructions, JSON.parse(JSON.stringify(regs)));	
}

main();
