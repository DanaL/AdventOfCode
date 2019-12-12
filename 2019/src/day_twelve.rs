
#[derive(Debug, Copy, Clone)]
struct Moon {
	pos: (i32, i32, i32),
	vel: (i32, i32, i32),
}

impl Moon {
	fn new(pos: (i32, i32, i32)) -> Moon {
		Moon { pos, vel: (0, 0, 0) }
	}
}

fn apply_velocity(moons: &mut Vec<Moon>) {
	for m in moons {
		m.pos.0 += m.vel.0;
		m.pos.1 += m.vel.1;
		m.pos.2 += m.vel.2;
	}
}

fn apply_gravity(moon: usize, moons: &mut Vec<Moon>) {
	let dx: i32 = moons.clone().iter()
		.map(|m| 
			if m.pos.0 > moons[moon].pos.0 { 1 } 
			else if m.pos.0 < moons[moon].pos.0 { -1 }
			else { 0 })
		.collect::<Vec<i32>>().iter().sum();
	moons[moon].vel.0 += dx;
	
	let dy: i32 = moons.clone().iter()
		.map(|m| 
			if m.pos.1 > moons[moon].pos.1 { 1 } 
			else if m.pos.1 < moons[moon].pos.1 { -1 }
			else { 0 })
		.collect::<Vec<i32>>().iter().sum();
	moons[moon].vel.1 += dy;

	let dz: i32 = moons.clone().iter()
		.map(|m| 
			if m.pos.2 > moons[moon].pos.2 { 1 } 
			else if m.pos.2 < moons[moon].pos.2 { -1 }
			else { 0 })
		.collect::<Vec<i32>>().iter().sum();
	moons[moon].vel.2 += dz;
}

fn calc_energy(moons: &Vec<Moon>) -> i32 {
	moons.iter()
		 .map(|m| (m.pos.0.abs() + m.pos.1.abs() + m.pos.2.abs()) *
				(m.vel.0.abs() + m.vel.1.abs() + m.vel.2.abs()))
		 .collect::<Vec<i32>>().iter().sum()
}

pub fn solve_q1() {
	let starts = [(-1, 7, 3), (12, 2, -13), (14, 18, -8), (17, 4, -4)];
	//let mut starts = [(-1, 0, 2), (2, -10, -7), (4, -8, 8), (3, 5, -1)];
	let mut moons: Vec<Moon> = starts.iter().map(|m| Moon::new(*m)).collect();	
	
	for _ in 0..1000 {
		for k in 0..moons.len() {
			apply_gravity(k, &mut moons);
		}
		apply_velocity(&mut moons);
	}
	
	println!("Q1: {}", calc_energy(&moons));
}
