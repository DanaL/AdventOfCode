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

let q1 = (lengths, range) => {
	let pos = 0;
	let skip = 0;
	let arr = [];
	for (let j = 0; j < range; j++)
		arr.push(j);
	
	for (l of lengths) {
		arr = reverse(arr, pos, pos + l - 1);
		pos = (pos + l + skip++) % arr.length;
	}

	console.log("Q1: " + (arr[0] * arr[1]));
}

let main = async () => {
    const input = "199,0,255,136,174,254,227,16,51,85,1,2,22,17,7,192";
    //const input = "3, 4, 1, 5";
	const lengths = input.split(",").map(Number);

	q1(lengths, 256);
}

main();
