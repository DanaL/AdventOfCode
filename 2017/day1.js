const fs = require('fs').promises;

let readInput = async () => {
    let stuff = await fs.readFile('captcha.txt');
    let lines = stuff.toString().split('\n');
	
    return lines;
}

let q1 = (captcha) => {
	let j = 0;
	let sum = 0;
	while (j < captcha.length) {
		if (captcha[j] == captcha[(j+1) % captcha.length])
			sum += parseInt(captcha[j]);
		++j;
	}
	
	console.log("Q1: " + sum);
}

let q2 = (captcha) => {
	let j = 0;
	let sum = 0;
	const skip = captcha.length / 2;
	while (j < captcha.length) {
		if (captcha[j] == captcha[(j + skip) % captcha.length])
			sum += parseInt(captcha[j]);
		++j;
	}
	
	console.log("Q2: " + sum);
}

let main = async () => {
    const lines = await readInput();

	q1(lines[0]);		
	q2(lines[0]);		
}

main();
