const fs = require('fs').promises;

let readInput = async (f) => {
    let stuff = await fs.readFile(f);
    let lines = stuff.toString().split('\n').filter(l => l.length > 0);

    return lines;
}

let main = async () => {
	const lines = await readInput("firewall.txt");
	const test_lines = [ "0: 3", "1: 2", "4: 4", "6: 4"];

	const fw = test_lines.map(l => l.split(": ").map(Number));
	let result = 0;
	fw.filter(layer => layer[0] % ((layer[1] - 1) * 2 ) === 0).forEach(res => result += res[0] * res[1]);
	console.log("Q2: " + result);
}

main();
