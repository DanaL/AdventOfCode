const fs = require('fs').promises;

const EXECUTING = 0;
const WAITING = 1;
const TERMINATED = 2;

let readInput = async (f) => {
    let stuff = await fs.readFile(f);
    let lines = stuff.toString().split('\n').map(l => l.trim()).filter(l => l.length > 0);

    return lines;
}

let execute = (vm, line) => {
	const bits = line.split(" ");
	const op = bits[0];
	const reg = bits[1];

	/* The value of the operation can be either a numeric constant or
		a register letter */
	if (bits.length === 3)
		var val = isNaN(bits[2]) ? vm.registers[bits[2]] : parseInt(bits[2]);
	
	if (isNaN(reg) && !(reg in vm.registers))
		vm.registers[reg] = 0;

	switch (op) {
		case "set":
			vm.registers[reg] = val;
			break;
		case "sub":
			vm.registers[reg] -= val;
			break;
		case "mul":
			vm.registers[reg] *= val;
			++vm.mults;
			break;
		case "jnz":
			/* jump instructions can take the form jnz a b, jnz a -1,
				jnz 1 3, or (although I didn't have an exanple of this) jnz 4, a */
			let a = isNaN(reg) ? vm.registers[reg] : parseInt(reg);
			if (a !== 0) {
				vm.pc += val;
				return;
			}
			break;
		default:
			console.log("Op " + bits[0] + " is unknown.");
			return;
	}
	
	++vm.pc;
}

let q1 = (lines) => {
	const vm0 = { registers: { }, pc:0, mults:0, state:EXECUTING }; 

	while (vm0.state === EXECUTING) {
		execute(vm0, lines[vm0.pc]);
		if (vm0.pc < 0 || vm0.pc >= lines.length) vm0.state = TERMINATED;
	}
	
	console.log("Q1: " + vm0.mults);
}

// I'm going to try to translate the assembly program into JS for part 2
let q2 = () => {
	var non_primes = 0;
	for (let b = 109900; b <= 126900; b += 17) {
		for (let d = 2; d < b; d++) {
			if (b % d === 0) {
				++non_primes;
				break;
			}
		}
	}

	console.log("Q2: " + non_primes);
}

let main = async () => {
    const lines = await readInput("day23_prog.txt");
	q1(lines);
	q2();
}

main();
