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

pub fn solve(q1: bool, cheat: bool) {
	let mut file = "./inputs/day13.txt";
	if cheat {
		file = "./inputs/day13_hacked.txt"
	}
	let prog_txt = fs::read_to_string(file).unwrap();
	let mut vm = intcode_vm::IntcodeVM::new();
	vm.load(prog_txt.trim());
	if !q1 {
		vm.write(0, 2);
	}
	let mut game = Game::new();
	let mut x = -1;
	let mut y = -1;
	let mut count = 0;
	loop {
		vm.run();
		match vm.state {
			intcode_vm::VMState::Halted => break,
			intcode_vm::VMState::AwaitInput => {
				let mut nmove = 0;
				if game.ball.1 < game.paddle.1 {
					nmove = -1;
				} else if game.ball.1 > game.paddle.1 {
					nmove = 1;
				}
				if cheat {
					nmove = -1;
				}
				vm.write_to_buff(nmove);
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
	if q1 {
		let blocks = game.board
			.iter()
			.flat_map(|a| a.iter())
			.cloned()
			.collect::<Vec<char>>()
			.iter()
			.filter(|ch| **ch == '▄').count();
		println!("Q1: {:?}", blocks);
	}
	else {
		println!("Q2: Final score was {}", game.score);
	}
}