
let next_a = (a) => (a * 16807) % 2147483647;
let next_b = (b) => (b * 48271) % 2147483647;

let q1 = (a, b) => {
	let matches = 0;
	for (let j = 0; j < 40000000; j++) {
		a = next_a(a);
		b = next_b(b);

		if ((a & 65535) === (b & 65535)) ++matches;
	}

	console.log("Q1: " + matches);
}

let q2 = (a, b) => {
	let matches = 0;
	for (let cmps = 0; cmps <= 5000000; cmps++) {
		do { a = next_a(a) } while (a % 4 !== 0);
		do { b = next_b(b) } while (b % 8 !== 0);
		
		if ((a & 65535) === (b & 65535)) 
			++matches;
	}

	console.log("Q2: " + matches);
}

let main = () => {
	q1(873, 583);
	q2(873, 583);
}

main();
