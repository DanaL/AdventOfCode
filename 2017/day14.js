const util = require("./common.js");
const ds = require("../DisjointSet.js");

let c_to_bit_str = (c) => {
	return parseInt(c, 16).toString(2).padStart(4, "0");
}

let gen_grid = (seed) => {
	const grid = [];
	for (let j = 0; j < 128; j++) {
		let hash = util.knot_hash(seed + "-" + j);
		grid.push(hash.split("").map(c_to_bit_str).join(""));
	}

	return grid;
}

let q1 = (grid) => {
	let total_used = 0;
	for (let row of grid)
		total_used += row.split("").filter(b => b === "1").length;
	
	console.log("Q1: " + total_used);
}

let union_adj = (nodes, grid, row, col, key) => {
	let coords = [ [-1, 0], [1, 0], [0, -1], [0, 1] ];
	for (let coord of coords) {
		let r = row + coord[0];
		let c = col + coord[1];

		if (r < 0 || r >= grid.length) continue;
		if (c < 0 || c >= grid[r].length) continue;
		if (grid[r][c] == "1") {
			let adj = r + "," + c;
			if (!(adj in nodes))
				nodes[adj] = ds.DSNode(adj);
			nodes[key].union(nodes[adj]);
		}
	}
}

let q2 = (grid) => {
	const nodes = {};
	for (let r = 0; r < grid.length; r++) {
		for (let c = 0; c < grid[r].length; c++) {
			if (grid[r][c] == "0") continue;

			const key = r + "," + c;
			if (!(key in nodes))
				nodes[key] = ds.DSNode(key);

			// Union with all the adjacent sqrs
			union_adj(nodes, grid, r, c, key);
		}
	}

	// Now, count the distinct groups
	const groups = new Set();
	for (let key in nodes) {
		if (!groups.has(nodes[key].find())) 
			groups.add(nodes[key].find());	
	}

	console.log("Q2: " + groups.size);
}

let main = () => {
	const grid = gen_grid("wenycdww");
	
	q1(grid);
	q2(grid);
}

main();
