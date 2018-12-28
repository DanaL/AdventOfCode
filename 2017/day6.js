const fs = require('fs').promises;

let readInput = async () => {
    let stuff = await fs.readFile('jmps.txt');
    let lines = stuff.toString().split('\n');
	
    return lines;
}

let redistribute = (mem_banks, index) => {
	blocks = mem_banks[index];
	mem_banks[index++] = 0;
	index %= mem_banks.length;

	while (blocks > 0) {
		mem_banks[index++]++;
		index %= mem_banks.length;
		--blocks;
	}
}

let q1 = (mem_banks) => {
	const seen_configs = new Set();
	let config_id = mem_banks.join(",");
	seen_configs.add(config_id);
	
	let cycles = 0;
	do {
		let hi_blocks = Math.max(...mem_banks);
		let index = mem_banks.indexOf(hi_blocks);
		redistribute(mem_banks, index);
		config_id = mem_banks.join(",");
		++cycles;
		if (seen_configs.has(config_id))
			break;
		else
			seen_configs.add(config_id);
	} while (true); 

	console.log("Q2: " + cycles);
}

let main = async () => {
    const lines = await readInput();
	const initial = "11	11	13	7	0	15	5	5	4	4	1	1	7	1	15	11";
	//const initial = "0	2	7	0";
	const mem_banks = initial.split("\t").map(Number);
	
	q1(mem_banks);
}

main();
