const fs = require('fs').promises;

const EXECUTING = 0;
const WAITING = 1;
const TERMINATED = 2;

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
			++vm_a.send_count;
			vm_b.mq.push(vm_a.registers[reg]);
			if (vm_b.state === WAITING)
				vm_b.state = EXECUTING;
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
			if (vm_a.mq.length === 0) {
				vm_a.state = WAITING;
				return;
			}
			vm_a.registers[reg] = vm_a.mq.shift();
			break;
		case "jgz":
			/* jump instructions can take the form jgz a b, jgz a -1,
				jgz 1 3, or (although I didn't have an exanple of this) jgz 4, a */
			let a = isNaN(reg) ? vm_a.registers[reg] : parseInt(reg);
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

let q2 = (lines) => {
	const vm0 = { registers: { p:0 }, pc:0, last_freq:0, 
		state:EXECUTING, mq:[], send_count:0 }; 
	const vm1 = { registers: { p:1 }, pc:0, last_freq:0, 
		state:EXECUTING, mq:[], send_count:0 };

	let x = 0;
	while (true) {
		while (vm0.state === EXECUTING) {
			//console.log("vm0 " + lines[vm0.pc]);
			execute(vm0, vm1, lines[vm0.pc], true);
			if (vm0.pc < 0 || vm0.pc >= lines.length) vm0.state = TERMINATED;
		}

		while (vm1.state === EXECUTING) {
			//console.log("vm1 " + lines[vm1.pc]);
			let last_instr = lines[vm1.pc];
			execute(vm1, vm0, lines[vm1.pc], true);
			if (vm1.pc < 0 || vm1.pc >= lines.length) {
				console.log("Terminating " + vm1.pc + " " + last_instr);
				console.log(vm1.registers);
				vm1.state = TERMINATED;
			}
		}

		if (!(vm0.state === EXECUTING || vm1.state === EXECUTING)) {
			console.log(vm0.state + " " + vm1.state);
			console.log(" (" + vm0.mq.length + ")  (" + vm1.state + ")");
			console.log("Deadlock!");
			break;
		}
	}
	
	console.log("Q2: " + vm1.send_count);
}

let main = async () => {
    const lines = await readInput("program.txt");
	q2(lines);
}

main();
