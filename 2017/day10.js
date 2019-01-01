const fs = require('fs').promises;

let reverse = (arr, lo, hi) => {
	while (lo < hi) {
		let actual_lo = lo % arr.length;
		let actual_hi = hi % arr.length;
		let tmp = arr[actual_lo];
		arr[actual_lo] = arr[actual_hi];
		arr[actual_hi] = tmp;
		++lo;
		--hi;
	}

	return arr;
}

let knot_encrypt = (cypher_text, range, iterations) => {
	let pos = 0;
	let skip = 0;
	let arr = [];
	for (let j = 0; j < range; j++)
		arr.push(j);

	for (let j = 0; j < iterations; j++) {	
		for (l of cypher_text) {
			arr = reverse(arr, pos, pos + l - 1);
			pos = (pos + l + skip++) % arr.length;
		}
	}

	return arr;
}

let q1 = (lengths, range) => {
	let arr = knot_encrypt(lengths, range, 1);
	console.log("Q1: " + (arr[0] * arr[1]));
}

let dense_hash = (arr, start, end) => {
	let result = arr[start];
	for (let j = start + 1; j < end; j++)
		result ^= arr[j];

	return result;
}

let q2 = (ascii_str) => {
	// convert each character to its ascii value (0..255)
	let lengths = ascii_str.split("").map(c => c.charCodeAt(0));
	lengths = lengths.concat([17, 31, 73, 47, 23]);
	let arr = knot_encrypt(lengths, 256, 64);

	let dh = [];
	for (j = 0; j < 16; j++) {
		let start = 0 + (j * 16);
		let end = start + 16;
		dh.push(dense_hash(arr, start, end));
	}

	console.log("Q2: " + dh.map(d => d < 16 ? "0" + d.toString(16) : d.toString(16)).join(""));
}

let main = async () => {
    const input = "199,0,255,136,174,254,227,16,51,85,1,2,22,17,7,192";
	const lengths = input.split(",").map(Number);

	q1(lengths, 256);
	q2(input);
}

main();
