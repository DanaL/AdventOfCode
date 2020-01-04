use std::fs;

// For Part 2, I followed this blog's tutorial for how to approach simplifying the giant
// values they want us to deal with: https://codeforces.com/blog/entry/72593
// So this function takes each line of the file and converts it to the form: f(x) = (a * x  + b) % m
// x is a card number so applying one of the transforms tells you where card x ends up.
// Steps in the input file can be combined. So if we have f(x) = (a * x + b) % m and
// g(x) = (c * x + d) % m, then g(f(x)) = (c(a*x + b) + d) % m or (c*a*x + c*b + d) % m, which
// gives us new coefficients that I return.
fn compose_lgc(a: i128, b: i128, m: i128, line: String) -> (i128, i128) {
	let pieces: Vec<&str> = line.split(' ').collect();
	let (mut c, mut d) = (0, 0);
	if pieces[0] == "cut" {
		let n = pieces[1].parse::<i128>().unwrap();
		c = 1;
		d = -n;
	} else if pieces[1] == "with" {
		let n = pieces[3].parse::<i128>().unwrap();
		c = n;
		d = 0;
	} else {
		c = -1;
		d = -1;
	}

	let mut coeff1 = a * c;
	coeff1 = coeff1.rem_euclid(m);
	let mut coeff2 = b * c + d;
	coeff2 = coeff2.rem_euclid(m);
	(coeff1, coeff2)
}

fn modular_pow(base: i128, exponent: i128, modulus: i128) -> i128 {
	if modulus == 1 {
		return 0;
	}

	let mut result = 1;
	let mut base_new = base.rem_euclid(modulus);
	let mut exp = exponent;
	while exp > 0 {
		if exp % 2 == 1 {
			result = (result * base_new).rem_euclid(modulus);
		}
		exp = exp >> 1;
		base_new = (base_new * base_new).rem_euclid(modulus);
	}

	result
}

// Calcualte the sum of the geomatric series:
// a^k * x + (a ^ k-1 + a ^ k-2 + ... + a + 1) * b mod m, or:
//
// F^k(x) = (a^k * x + b(1 - a^k) / 1 - a) mod m
//
// But we need to use modular inverse for division because this is
// all modula m, so p / q = p * q^-1 and q^-1 is done by modular_pow(q, m - 2 , m),
// which apparently works because m is a prime???
fn sum_series(x: i128, a: i128, b: i128, k: i128, m: i128) -> i128 {
	let ak = modular_pow(a, k, m);
	let p = (b * (1 - ak)).rem_euclid(m);
	let q = (1 - a).rem_euclid(m);

	// p * q^-1, calcualte by modular inverse
	let q_inv = modular_pow(q, m - 2, m);
	//let p_div_q = ((p as u128) * (q_inv as u128));
	let p_div_q = p * q_inv;

	//let f = (ak as u128) * (x as u128) + p_div_q;
	let f = ak * x + p_div_q as i128;

	f.rem_euclid(m) 
}

fn invert_f(x: i128, a: i128, b: i128, k: i128, m: i128) -> i128 {
	let ak = modular_pow(a, k, m);
	let p = (b * (1 - ak)).rem_euclid(m);
	let q = (1 - a).rem_euclid(m);

	// p * q^-1, calculate by modular inverse
	let mut p_div_q = p * modular_pow(q, m - 2, m);

	// using modular inverse again to calculate F^-k(x), or
	// x - B / A mod m, where B = b * (1 - a^k) / 1 - a and a = a^k
	let inv_ak = modular_pow(ak, m - 2, m).rem_euclid(m);
	p_div_q = p_div_q.rem_euclid(m);
	let inv = (x - p_div_q) * inv_ak;

	inv.rem_euclid(m)
}

pub fn solve_q1_mathy() {
	let input = fs::read_to_string("./inputs/day22.txt").unwrap();
	let mut coeffs = (1, 0);
	let m = 10007;
	for line in input.trim().split('\n') {
		coeffs = compose_lgc(coeffs.0, coeffs.1, m, line.trim().to_string());
	}

	let k = 1;
	let res = sum_series(2019, coeffs.0, coeffs.1, k, m);
	println!("Loc of {}: {}", 2019, res);
	println!("Test inversion: {}", invert_f(res, coeffs.0, coeffs.1, k, m));
	println!("(ie., where was the card in {} before the shuffle?)\n", res);
}

pub fn solve_q2() {
	let input = fs::read_to_string("./inputs/day22.txt").unwrap();
	let mut coeffs = (1, 0);
	let m = 119_315_717_514_047;
	let k = 101_741_582_076_661;
	for line in input.trim().split('\n') {
		coeffs = compose_lgc(coeffs.0, coeffs.1, m, line.trim().to_string());
	}

	let res = invert_f(2020, coeffs.0, coeffs.1, k, m);
	println!("Q2: {}", res);
	println!("... {}", sum_series(res, coeffs.0, coeffs.1, k, m));
}
