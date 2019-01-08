const fs = require('fs').promises;

const TERMINATED = 0;

let readInput = async (f) => {
    let stuff = await fs.readFile(f);
    let lines = stuff.toString().split('\n').filter(l => l.length > 0);

    return lines;
}

let execute = (vm_a, vm_b, line, q2) => {
	const bits = line.split(" ");
	const op = bits[0];
	const reg = bits[1];
	
	/* The value of the operation can be either a numeric constant or
		a register letter */
	if (bits.length === 3)
		var val = isNaN(bits[2]) ? vm_a.registers[bits[2]] : parseInt(bits[2]);
	
	if (!(reg in vm_a.registers))
		vm_a.registers[reg] = 0;

	switch (op) {
		case "snd":
			vm_a.last_freq = vm_a.registers[reg];
			break;
		case "set":
			vm_a.registers[reg] = val;
			break;
		case "add":
			vm_a.registers[reg] += val;
			break;
		case "mul":
			vm_a.registers[reg] *= val;
			break;
		case "mod":
			vm_a.registers[reg] %= val;
			break;
		case "rcv":
			if (vm_a.registers[reg] !== 0) {
				console.log("Q1: " + vm_a.last_freq);
				vm_a.state = TERMINATED;
			}
			break;
		case "jgz":
			/* jump instructions can take the form jgz a b, jgz a -1,
				jgz 1 3, or (although I didn't have an exanple of this) jgz 4, a */
			if (isNaN(reg))
				var a = vm_a.registers[reg];
			else
				var a = parseInt(reg);

			if (a > 0) {
				vm_a.pc += val;
				return;
			}
			break;
		default:
			console.log("Op " + bits[0] + " is unknown.");
			return;
	}
	
	++vm_a.pc;
}

let q1 = (lines) => {
	const vm = { registers: { }, pc:0, last_freq:0 };

	while (vm.pc >= 0 && vm.pc < lines.length) {
		execute(vm, null, lines[vm.pc], false);
		if (vm.state === TERMINATED) break;
	}
}

let main = async () => {
    const lines = await readInput("program.txt");
	
	q1(lines);
}

main();
