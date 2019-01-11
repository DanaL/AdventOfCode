const fs = require("fs").promises;
const util = require("./common.js");

let readInput = async (f) => {
	let stuff = await fs.readFile(f);
	let lines = stuff.toString().split("\n").map(l => l.trim()).filter(l => l.length > 0);

	return lines;
}

let turn = (dir, turn) => {
	if ((dir === "n" && turn === "left") || (dir === "s" && turn === "right"))
		return "w";
	else if ((dir === "n" && turn === "right") || (dir === "s" && turn === "left"))
		return "e";
	else if ((dir === "e" && turn === "right") || (dir === "w" && turn === "left"))
		return "s";
	else if ((dir === "e" && turn === "left") || (dir === "w" && turn === "right"))
		return "n";
	
	if (turn === "rev") {
		if (dir === "n") return "s";
		if (dir === "s") return "n";
		if (dir === "w") return "e";
		if (dir === "e") return "w";
	}
}

let dump = (coords, virus) => {
	for (let r = -5; r < 5; r++) {
		let row = "";
		for (let c = -5; c < 5; c++) {
			let p = [r, c];
			if (p[0] === virus.co[0] && p[1] === virus.co[1])
				row += "*";
			else if (coords.has(p))
				row += coords.get(p);
			else
				row += ".";
		}
		console.log(row);
	}
}

let q1 = (coords) => {
	const virus = { co:[0,0], dir:"n" };	
	let infections = 0;
	for (let j = 0; j < 10000; j++ ) {
		if (coords.has(virus.co)) {
			virus.dir = turn(virus.dir, "right");
			coords.delete(virus.co);
		}
		else {
			virus.dir = turn(virus.dir, "left");
			coords.add(virus.co, true);
			++infections;
		}

		if (virus.dir === "n")
			virus.co[0] -= 1;
		else if (virus.dir === "s")
			virus.co[0] += 1;
		else if (virus.dir === "w")
			virus.co[1] -= 1;
		else if (virus.dir === "e")
			virus.co[1] += 1;
	}
	
	console.log("Q1: " + infections);
}

let q2 = (coords) => {
	const virus = { co:[0,0], dir:"n" };	
	let infections = 0;
	for (let j = 0; j < 10000000; j++ ) {
		if (coords.has(virus.co)) {
			let loc = coords.get(virus.co);
			coords.delete(virus.co);
			switch (loc) {
				case "W":
					coords.add(virus.co, "#");
					++infections;
					break;
				case "#":
					coords.add(virus.co, "F");
					virus.dir = turn(virus.dir, "right");
					break;
				case "F":
					virus.dir = turn(virus.dir, "rev");
					break;
			}
		}
		else {
			virus.dir = turn(virus.dir, "left");
			coords.add(virus.co, "W");
		}

		if (virus.dir === "n")
			virus.co[0] -= 1;
		else if (virus.dir === "s")
			virus.co[0] += 1;
		else if (virus.dir === "w")
			virus.co[1] -= 1;
		else if (virus.dir === "e")
			virus.co[1] += 1;
	}
	
	console.log("Q2: " + infections);
}

let parse = (lines) => {
	let lo_row = 0 - Math.floor(lines.length / 2);
	let lo_col = 0 - Math.floor(lines[0].length / 2);
	let coords = util.dict();

	for (let r = 0; r < lines.length; r++) {
		for (let c = 0; c < lines.length; c++) {
			if (lines[r][c] == "#") 
				coords.add([r + lo_row, c + lo_col], "#");			
		}
	}
	
	return coords;
}

let main = async () => {
	const lines = await readInput("day22_grid.txt");
	//const lines = [ "..#", "#..", "..." ];
	q1(parse(lines));
	q2(parse(lines));
}

main();
