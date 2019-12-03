pub fn manhattan_d(ax: i32, ay: i32, bx: i32, by: i32) -> i32 {
	(ax - bx).abs() + (ay - by).abs()
}
