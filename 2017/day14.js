const util = require("./common.js");

let c_to_bit_str = (c) => {
	return parseInt(c, 16).toString(2).padStart(4, "0");
}

let q1 = (seed) => {
	let total_used = 0;
	for (let j = 0; j < 128; j++) {
		let hash = util.knot_hash(seed + "-" + j);
		let bits = hash.split("").map(c_to_bit_str).join("");
		total_used += bits.split("").filter(b => b == "1").length;
	}
	
	console.log("Q1: " + total_used);
}

let main = () => {
	const seed = "wenycdww";

	q1(seed);
}

main();
