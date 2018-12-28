const fs = require('fs').promises;

let q1 = (sheet) => {
	var checks = sheet.map(row =>  Math.max.apply(null, row) - Math.min.apply(null, row));
	console.log("Q1: " + checks.reduce((a, n) => a + n));
}

let q2 = (sheet) => {
	sum = 0;
	for (let row of sheet) {
		mean = row.reduce((a, n) => a + n) / row.length;
		hi = row.filter(n => n >= mean);
		lo = row.filter(n => n <= mean);
		
		for (l of lo) {
			for (h of hi) {
				if (h % l == 0)
					sum += h / l;
			}
		}
	}

	console.log("Q2: " + sum);
}

let readInput = async () => {
    let stuff = await fs.readFile('spreadsheet.txt');
    let lines = stuff.toString().split('\n');
	
    return lines;
}

let main = async () => {
    const lines = await readInput();

	sheet = [];	
	lines.forEach((row) => {
		sheet.push(row.split("\t").map(c => parseInt(c)));
	});

	q1(sheet);
	q2(sheet);		
}

main();
