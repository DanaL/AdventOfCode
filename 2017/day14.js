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
		total_used += row.split("").filter(b => b == "1").length;
	
	console.log("Q1: " + total_used);
}

let q2 = (seed) => {
}

let main = () => {
	const seed = "wenycdww";
	const grid = gen_grid("flqrgnkx");
	
	q1(grid);
}

main();
