// I think what I'll want to do is change solve() to be run_prog()
// It'll take a vector of integers, the noun, the verb and run as below
// I can use the basic test input with noun, verb = 0, q1 has noun and verb of 12, 2
// Q2 I gotta find a specific noun and verb
// From the solve_q2() I'll need to pass a copy of the vector I guess
pub fn solve(input_str: &str) {
	let mut prog: Vec<usize> = input_str.trim().split(",")
		.map(|a| a.parse::<usize>().unwrap()).collect();

	// instructions say before we run we need to replace pos 1 with 12
	// and replace pos 2 with 2
	prog[1] = 12;
	prog[2] = 2;

	let mut ptr = 0;
	loop {
		match prog[ptr] {
			// add and write back
			1  => {
				let wi = prog[ptr+3];
				prog[wi] = prog[prog[ptr+1]] + prog[prog[ptr+2]];
			},
			// multiply and write back
			2  => {
				let wi = prog[ptr+3];
				prog[wi] = prog[prog[ptr+1]] * prog[prog[ptr+2]];
			},
			// halt!
			99 => break,
			// I don't think this should ever happen with our input?
			_  => panic!("Hmm this shouldn't happen..."),
		}
		ptr += 4;
	}

	println!("Pos zero: {}", prog[0]);
}
