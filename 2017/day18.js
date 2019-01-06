const fs = require('fs').promises;

let readInput = async (f) => {
    let stuff = await fs.readFile(f);
    let lines = stuff.toString().split('\n').filter(l => l.length > 0);

    return lines;
}

let execute = (vm, line) => {
	console.log(line);
	const bits = line.split(" ");
	const op = bits[0];
	const reg = bits[1];	
	/* The value of the operation can be either a numeric constant or
		a register letter */

	if (bits.length === 3)
		var val = isNaN(bits[2]) ? vm.registers[bits[2]] : parseInt(bits[2]);
	
	if (!(reg in vm.registers))
		vm.registers[reg] = 0;

	switch (op) {
		case "snd":
			vm.last_freq = vm.registers[reg];
			break;
		case "set":
			vm.registers[reg] = val;
			break;
		case "add":
			vm.registers[reg] += val;
			break;
		case "mul":
			vm.registers[reg] *= val;
			break;
		case "mod":
			vm.registers[reg] %= val;
			break;
		case "rcv":
			if (vm.registers[reg] !== 0) {
				console.log("Q1: " + vm.last_freq);
				process.exit();
			}
			break;
		case "jgz":
			if (vm.registers[reg] > 0) {
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
	const vm = { registers: { }, pc:0, last_freq:0 };

	while (vm.pc >= 0 && vm.pc < lines.length) {
		execute(vm, lines[vm.pc]);
	}
}

let main = async () => {
    const lines = await readInput("program.txt");
	q1(lines);
}

main();
