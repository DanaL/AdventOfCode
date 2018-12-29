const fs = require('fs').promises;

let readInput = async (f) => {
    let stuff = await fs.readFile(f);
    let lines = stuff.toString().split('\n').filter(l => l.length > 0);

    return lines;
}

let parse_line = (line, regs) => {
	const i = line.match(/([a-z]+) ([a-z]+) (-?\d+) if ([a-z]+) ([<>=!]+) (-?\d+)/);
	let instr = `if (regs["${i[4]}"] ${i[5]} ${parseInt(i[6])}) regs["${i[1]}"] `;
	instr += i[2] === "inc" ? "+= " : "-= ";
	instr += parseInt(i[3]); 
		
	regs[i[1]] = 0;	
	regs[i[4]] = 0;	

	return instr;
}

let q1 = (instructions, regs) => {
	instructions.map(i => eval(i));
	console.log("Q1: " + Math.max(...Object.values(regs)));
}

let q2 = (instructions, regs) => {
	let hi = 0;
	for (instr of instructions) {
		eval(instr);
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
