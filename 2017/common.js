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

let dense_hash = (arr, start, end) => {
	let result = arr[start];
	for (let j = start + 1; j < end; j++)
		result ^= arr[j];

	return result;
}

module.exports = {
	knot_hash: ascii_str => {
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

		return dh.map(d => d < 16 ? "0" + d.toString(16) : d.toString(16)).join("");
	},
	copy_obj: obj => JSON.parse(JSON.stringify(obj)),
	dict: () => {
		return {
			items: [],
			add(key, val) {
				let k = JSON.stringify(key);
				this.items[k] = val;
			},
			get(key) {
				let k = JSON.stringify(key);
				return this.items[k];
			},
			has(key) {
				return JSON.stringify(key) in this.items;
			},
			delete(key) {
				let k = JSON.stringify(key);
				delete this.items[k];
			}
		}
	}
}
