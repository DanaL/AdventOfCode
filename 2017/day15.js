
let q1 = (a, b) => {
	let matches = 0;
	for (let j = 0; j < 40000000; j++) {
		a = (a * 16807) % 2147483647;
		b = (b * 48271) % 2147483647;

		if ((a & 65535) === (b & 65535)) ++matches;
	}

	console.log("Q1: " + matches);
}

let q2 = (a, b) => {
	let qa = [];
	let qb = [];
		
	for (let j = 0; j < 100; j++) { 
		a = (a * 16807) % 2147483647;
		b = (b * 48271) % 2147483647;

		if (a % 4 == 0) qa.push(a);
		if (b % 8 == 0) qb.push(b);

		if (b % 8 == 0)
			console.log(b);
		//console.log(a.toString().padStart(15, " ") + b.toString().padStart(15, " "));
		//console.log((a & 65535).toString().padStart(15, " ") + (b & 65535).toString().padStart(15, " "));
		//console.log("");
	}
}

let main = () => {
	q1(873, 583);
	q2(65, 8921);
}

main();
