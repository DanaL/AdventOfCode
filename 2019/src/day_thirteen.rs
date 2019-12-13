use std::fs;
use crate::intcode_vm;

const SCR_HEIGHT: usize = 26;
const SCR_WIDTH: usize = 37;

struct Game {
	board: Vec<Vec<char>>,
	ball: (usize, usize),
	paddle: (usize, usize),
	score: u32,
}

impl Game {
	pub fn new() -> Game {
		Game { board: vec![vec!['.'; SCR_WIDTH]; SCR_HEIGHT],
			ball: (0, 0), paddle: (0, 0), score: 0 }
	}

	pub fn display(&self) {
		println!("Score: {}", self.score);
		for r in 0..SCR_HEIGHT {
			let mut row = String::new();
			for c in 0..SCR_WIDTH {
				let ch = if r == self.ball.0 && c == self.ball.1 {
					'o'
				} else if r == self.paddle.0 && c == self.paddle.1 {
					'-'
				} else {
					self.board[r][c]
				};
				row.push(ch);
			}
			println!("{}", row);
		}
		println!("");
	}
}

pub fn solve_q2() {
	let prog_txt = fs::read_to_string("./inputs/day13_hacked.txt").unwrap();
	let mut vm = intcode_vm::IntcodeVM::new();
	vm.load(prog_txt.trim());
	vm.write(0, 2);

	let mut game = Game::new();
	let mut x = -1;
	let mut y = -1;
	let mut count = 0;
	game.display();

	loop {
		vm.run();
		match vm.state {
			intcode_vm::VMState::Halted => break,
			intcode_vm::VMState::AwaitInput => {
				if game.score > 4000 && game.score < 4100 {
					game.display();
				}
				vm.write_to_buff(-1);
			},
			_ => {
				match count {
					0 => x = vm.output_buffer,
					1 => y = vm.output_buffer,
					2 => {
						if x == -1 && y == 0 {
							game.score = vm.output_buffer as u32
						} else {
							let lx = x as usize;
							let ly = y as usize;
							match vm.output_buffer {
								0 => game.board[ly][lx] = ' ',
								1 => game.board[ly][lx] = '#',
								2 => game.board[ly][lx] = '▄',
								3 => game.paddle = (ly, lx),
								4 => game.ball = (ly, lx),
								_ => panic!("This shouldn't happen?? {} {} {}", vm.output_buffer, x, y),
							}
						}
					},
					_ => panic!("This shouldn't happen..."),
				}
				count = (count + 1) % 3;
			},
		}
	}

	game.display();
	println!("Q2: Final score was {}", game.score);
}

pub fn solve_q1() {
	let prog_txt = fs::read_to_string("./inputs/day13.txt").unwrap();
	let mut vm = intcode_vm::IntcodeVM::new();
	vm.load(prog_txt.trim());

	let mut count = 0;
	let mut locs = Vec::new();
	let mut sprites = Vec::new();
	let mut x = -1;
	let mut y = -1;
	loop {
		vm.run();
		if vm.state == intcode_vm::VMState::Halted {
			break;
		}

		match count {
			0 => x = vm.output_buffer,
			1 => {
				y = vm.output_buffer;
				locs.push((x, y));
			},
			2 => sprites.push(vm.output_buffer),
			_ => panic!("This shouldn't happen..."),
		}
		count = (count + 1) % 3;
	}

	let num_of_blocks = sprites.iter().filter(|s| **s == 2).count();
	println!("Q1 {}", num_of_blocks);
}
