pub fn solve(input_str: &str) {
	let mut prog: Vec<i32> = input_str.trim().split(",")
		.map(|a| a.parse::<i32>().unwrap()).collect();
	//let prog: Vec<&str> = fs::read_to_string(filename).unwrap()
	//	.trim().split(",").iter().map

	println!("{:?}", prog[0]);
	println!("{:?}", prog[1]);
}
