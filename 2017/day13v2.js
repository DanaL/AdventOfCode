const fs = require('fs').promises;

let readInput = async (f) => {
    let stuff = await fs.readFile(f);
    let lines = stuff.toString().split('\n').filter(l => l.length > 0);

    return lines;
}

let main = async () => {
	const lines = await readInput("firewall.txt");
	const test_lines = [ "0: 3", "1: 2", "4: 4", "6: 4"];

	/* Q1 by straight calculation */
	const fw = lines.map(l => l.split(": ").map(Number));
	let result = 0;
	fw.filter(layer => layer[0] % ((layer[1] - 1) * 2 ) === 0).forEach(res => result += res[0] * res[1]);
	console.log("Q1: " + result);

	let delay = 1;
	while (fw.filter(layer => (layer[0] + delay) % ((layer[1] - 1) * 2 ) === 0).length > 0)
		++delay;
	console.log("Q2: " + delay);
}

main();
