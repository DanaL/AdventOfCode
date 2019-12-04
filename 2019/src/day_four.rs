fn inc(arr: &mut [u32]) {
	let mut i = 5;
	while i >= 0 && arr[i] == 9 {
		i -= 1;
	}

	arr[i] += 1;

	for j in i+1..6 {
		arr[j] = arr[i];
	}	
}

// For Q1, password is valid if there are any two adjacent
// digits that are the same
fn is_valid_q1(arr: &[u32]) -> bool {
	for i in 0..5 {
		if arr[i] == arr[i+1] {
			return true;
		}
	}

	false
}

// For Q2, we want a password that has two adjacent digits that
// aren't part of a larger group of the same number.
//    112233 is valid but 144446 is not
fn is_valid_q2(arr: &[u32]) -> bool {
	for i in 0..5 {
		if arr[i] == arr[i+1] && 
				!(i > 0 && arr[i-1] == arr[i] || i < 4 && arr[i+2] == arr[i]) {
			return true;
		}
	}

	false
}

pub fn solve_q1() {
	let mut arr: [u32; 6] = [2,6,6,6,6,6];
	let mut valid_q1 = 0;
	let mut valid_q2 = 0;

	while arr[0] < 7 {
		if (is_valid_q1(&arr)) 
		{
			valid_q1 += 1;

			if (is_valid_q2(&arr)) {
				valid_q2 += 1;
			}
		}

		inc(&mut arr);
	}

	println!("Q1: {}", valid_q1);
	println!("Q2: {}", valid_q2);
}
