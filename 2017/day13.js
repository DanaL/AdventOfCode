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

let print_firewall = (firewall, max_layer, max_depth, packet, at_picosecond) => {
	let header = "";
	for (let j = 0; j <= max_layer; j++) {
		header += " " + j + "  ";
	}
	console.log(header);

	for (let d = 0; d < max_depth; d++) {
		let row = "";
		for (let l = 0; l <= max_layer; l++) {
			if (!(l in firewall) || d >= firewall[l].depth) {
				if (d === 0 && packet === l)
					row += "( ) ";
				else
					row += "    ";
			}
			else if (d == firewall[l].scanner)
				row += d == 0 && l == packet ? "(S) " : "[S] ";    	
			else
				row += d == 0 && l == packet ? "( ) " : "[ ] ";    	
		}
		console.log(row);
	}
}

let run = (firewall, max_layer, max_depth, verbose, delay) => {
	let packet = 0;
	let penalty = 0;
	let picosecond = 0;

	while (packet <= max_layer) {
		if (verbose) {
			console.log("Picosecond: " + picosecond);
			console.log("Packet at: " + packet);
			if (picosecond >= delay)
				print_firewall(firewall, max_layer, max_depth, packet, picosecond);
			else
				print_firewall(firewall, max_layer, max_depth, -1, picosecond);
		}

		if (picosecond >= delay) {
			if (packet in firewall && firewall[packet].scanner == 0) 
				penalty += firewall[packet].layer * firewall[packet].depth;
		}

		for (let l in firewall) {
			firewall[l].scanner += firewall[l].travel;
			if (firewall[l].scanner == 0 || firewall[l].scanner == (firewall[l].depth - 1))
				firewall[l].travel *= -1;
		}

		if (verbose) {
			console.log("");
			if (picosecond >= delay)
				print_firewall(firewall, max_layer, max_depth, packet, picosecond);
			else
				print_firewall(firewall, max_layer, max_depth, -1, picosecond);
		}
	
		if (picosecond >= delay)
			++packet;
	
		++picosecond;	
	}

	return penalty;
}

let main = async () => {
    const lines = await readInput("firewall.txt");
	const test_lines = [ "0: 3", "1: 2", "4: 4", "6: 4"];
	const firewall = {};
	let max_layer = 0;
	let max_depth = 0;
	for (let line of test_lines) {
		let layer = parse_line(line);
		firewall[layer.layer] = layer;
		if (layer.layer > max_layer)
			max_layer = layer.layer;
		if (layer.depth > max_depth)
			max_depth = layer.depth;
	}

	let fw = JSON.parse(JSON.stringify(firewall));
	console.log("Q1: " + run(fw, max_layer, max_depth, false, 0));

	
	fw = JSON.parse(JSON.stringify(firewall));
	console.log(run(fw, max_layer, max_depth, false, 1));

	fw = JSON.parse(JSON.stringify(firewall));
	console.log(run(fw, max_layer, max_depth, false, 2));

	fw = JSON.parse(JSON.stringify(firewall));
	console.log(run(fw, max_layer, max_depth, true, 4));
	

	/* For Q2 we want to find the lowest delay that'll have penalty 0 */
	/*
	for (let delay = 8; ; delay++) {
		fw = JSON.parse(JSON.stringify(firewall));
		penalty = run(fw, max_layer, max_depth, false, delay);
		//console.log(delay + " " + penalty);
		if (penalty === 0) {
			console.log("Q2: " + delay);
			break;
		}
	}
	*/
}

main();
