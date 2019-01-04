const fs = require('fs').promises;

let readInput = async (f) => {
    let stuff = await fs.readFile(f);
    let lines = stuff.toString().split('\n').filter(l => l.length > 0);

    return lines;
}

let parse_line = (line) => {
	let pieces = line.split(": ").map(Number);

	return { layer:pieces[0], depth:pieces[1], scanner:0, travel:1};
}

let print_firewall = (firewall, max_layer, max_depth, at_picosecond) => {
	console.log("Picosecond: " + at_picosecond);
	let header = "";
	for (let j = 0; j <= max_layer; j++) {
		header += " " + j + "  ";
	}
	console.log(header);

	for (let d = 0; d < max_depth; d++) {
		let row = "";
		for (let l = 0; l <= max_layer; l++) {
			if (!(l in firewall) || d >= firewall[l].depth)
				row += "    ";
			else if (d == firewall[l].scanner)
				row += d == 0 && l == at_picosecond ? "(S) " : "[S] ";    	
			else
				row += d == 0 && l == at_picosecond ? "( ) " : "[ ] ";    	
		}
		console.log(row);
	}
}

let q1 = (firewall, max_layer, max_depth, verbose) => {
	let packet = 0;
	let penalty = 0;

	while (packet <= max_layer) {
		if (verbose)
				print_firewall(firewall, max_layer, max_depth, packet);
		if (packet in firewall && firewall[packet].scanner == 0) {
			penalty += firewall[packet].layer * firewall[packet].depth;
		}

		for (let l in firewall) {
			firewall[l].scanner += firewall[l].travel;

			if (firewall[l].scanner == 0 || firewall[l].scanner == (firewall[l].depth - 1))
				firewall[l].travel *= -1;
		}
	
		++packet;
	}

	console.log("Q1: " + penalty);
}

let main = async () => {
    const lines = await readInput("firewall.txt");
	const firewall = {};
	let max_layer = 0;
	let max_depth = 0;
	for (let line of lines) {
		let layer = parse_line(line);
		firewall[layer.layer] = layer;
		if (layer.layer > max_layer)
			max_layer = layer.layer;
		if (layer.depth > max_depth)
			max_depth = layer.depth;
	}

	q1(firewall, max_layer, max_depth, false);
}

main();
