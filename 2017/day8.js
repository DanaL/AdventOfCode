const fs = require('fs').promises;

let readInput = async (f) => {
    let stuff = await fs.readFile(f);
    let lines = stuff.toString().split('\n').filter(l => l.length > 0);

    return lines;
}

let parse_line = (line, regs) => {
	const i = line.match(/([a-z]+) ([a-z]+) (-?\d+) if ([a-z]+) ([<>=!]+) (-?\d+)/);
	const instr = { out_reg:i[1], val: parseInt(i[3]),
				expr:`regs["${i[4]}"] ${i[5]} ${parseInt(i[6])}` };

	if (i[2] === "dec")
		instr.val *= -1;
	if (!(instr.out_reg in regs))
		regs[instr.out_reg] = 0;	
	if (!(i[4] in regs))
		regs[i[4]] = 0;	

	return instr;
}

let q1 = (instructions, regs) => {
	for (instr of instructions) {
		if (eval(instr.expr))
			regs[instr.out_reg] += instr.val;
	}

	console.log("Q1: " + Math.max(...Object.values(regs)));
}

let q2 = (instructions, regs) => {
	let hi = 0;
	for (instr of instructions) {
		if (eval(instr.expr))
			regs[instr.out_reg] += instr.val;
		let m = Math.max(...Object.values(regs));
		if (m > hi)
			hi = m;
	}
	
	console.log("Q2: " + hi);
}

let main = async () => {
    const lines = await readInput("instructions.txt");
	const regs = { };
	const instructions = lines.map(line => parse_line(line, regs));
	
	q1(instructions, JSON.parse(JSON.stringify(regs)));	
	q2(instructions, JSON.parse(JSON.stringify(regs)));	
}

main();
