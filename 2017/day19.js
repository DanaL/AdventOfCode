const fs = require('fs').promises;

let readInput = async (f) => {
    let stuff = await fs.readFile(f);
    let lines = stuff.toString().split('\n').filter(l => l.length > 0);

    return lines;
}

const dirs = {
	"n": [-1, 0],
	"e": [0, 1],
	"s": [1, 0],
	"w": [0, -1]
};

let dump_map = (lines, loc) => {
	for (let r = 0; r < lines.length; r++) {
		let row = "";
		for (let c = 0; c < lines[r].length; c++) {
			if (r === loc[0] && c === loc[1])
				row += "*";
			else
				row += lines[r][c];	
		}
		console.log(row);
	}	
	console.log("");
}

let cmp = (a, b) => a[0] === b[0] && a[1] === b[1];

let check_for_turn = (map, curr, dir) => {
	let next_loc = (curr[0] + dir[0]) + "," + (curr[1] + dir[1]);
	if (next_loc in map && map[next_loc] !== " ")
		return dir;
	
	if (cmp(dir, dirs.n) || cmp(dir, dirs.s)) {
		let w = (curr[0] + dirs.w[0]) + "," + (curr[1] + dirs.w[1]);
		if (w in map && map[w] !== " ") 
			return dirs.w;
		let e = (curr[0] + dirs.e[0]) + "," + (curr[1] + dirs.e[1]);
		if (e in map && map[e] !== " ") 
			return dirs.e;
	}
	if (cmp(dir, dirs.e) || cmp(dir, dirs.w)) {
		let n = (curr[0] + dirs.n[0]) + "," + (curr[1] + dirs.n[1]);
		if (n in map && map[n] !== " ")
			return dirs.n;
		let s = (curr[0] + dirs.s[0]) + "," + (curr[1] + dirs.s[1]);
		if (s in map && map[s] !== " ")
			return dirs.s;
	}

	return dir;
}

let trace_route = (map, start, lines, show_map) => {
	const letters = [];
	const dirs = {
		"n": [-1, 0],
		"e": [0, 1],
		"s": [1, 0],
		"w": [-1, ]
	};
	let dir = dirs.s;
	let curr = start;
	let steps = 0;

	do {
		curr[0] += dir[0];
		curr[1] += dir[1];
		let loc = curr[0] + "," + curr[1];	
		if (map[loc] >= "A" && map[loc] <= "Z")
			letters.push(map[loc]);
		else if (map[loc] === "+")
			dir = check_for_turn(map, curr, dir);
		if (show_map) dump_map(lines, curr);
		++steps;
	} while ((curr[0] + "," + curr[1]) in map);

	console.log("Q1: " + letters.join(""));
	console.log("Q2: " + steps);
}

let main = async () => {
    const lines = await readInput("route.txt");
	const map = { };

	for (let r = 0; r < lines.length; r++) {
		for (let c = 0; c < lines[r].length; c++) {
			if (r === 0 && lines[r][c] === "|")
				var start = [r, c];
			if (lines[r][c] !== " ")
				map[r + "," + c] = lines[r][c];
		}
	}

	trace_route(map, start, lines, false);
}

main();
