const fs = require('fs').promises;

let readInput = async (f) => {
    let stuff = await fs.readFile(f);
    let lines = stuff.toString().split('\n').filter(l => l.length > 0);

    return lines;
}

let q1 = (layers) => {
	packet = 0;
	picosecond = 0;
}

let main = async () => {
    const lines = await readInput("firewall.txt");
	let layers = lines.map(l => l.split(": ").map(Number));

	console.log(layers);
}

main();
