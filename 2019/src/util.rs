pub fn manhattan_d(ax: i32, ay: i32, bx: i32, by: i32) -> u32 {
	let d = (ax - bx).abs() + (ay - by).abs();
	d as u32
}
