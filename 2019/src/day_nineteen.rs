use std::fs;
use crate::intcode_vm;

fn show_grid(grid: &Vec<Vec<char>>) {
    for row in grid {
        let line: String =  row.into_iter().collect();
        println!("{}", line);
    }
}

pub fn solve_q1() {
    let prog_txt = fs::read_to_string("./inputs/day19.txt").unwrap();
	let mut vm = intcode_vm::IntcodeVM::new();

    let dim = 50;
    let mut grid = vec![vec!['.'; dim]; dim];
    let mut sum = 0;
    for r in 0..dim {
        for c in 0..dim {
            vm.load(prog_txt.trim());
            vm.write_to_buff(c as i64);
            vm.write_to_buff(r as i64);
            vm.run();
            if vm.output_buffer == 1 {
                sum += 1;
                grid[r as usize][c as usize] = '#';
            }
        }
    }
    show_grid(&grid);
    println!("Q1 {:?}", sum);
}

fn pt_in_beam(x: i64, y: i64, prog_txt: &str) -> bool {
    let mut vm = intcode_vm::IntcodeVM::new();
    vm.load(prog_txt);
    vm.write_to_buff(x);
    vm.write_to_buff(y);
    vm.run();
    vm.output_buffer == 1
}

fn check_sq(x: i64, y: i64, prog_txt: &str) -> bool {
    if !pt_in_beam(x - 2, y - 2, &prog_txt) { return false; }
    if !pt_in_beam(x - 2, y + 1, &prog_txt) { return false; }
    if !pt_in_beam(x + 1, y - 2, &prog_txt) { return false; }
    if !pt_in_beam(x + 1, y + 1, &prog_txt) { return false; }
    true
}

pub fn solve_q2() {
    let prog_txt = fs::read_to_string("./inputs/day19.txt").unwrap();

    // Aritraryily chosen start values
    let mut x = 100;
    let mut y = 300;
    'outer: loop {
        // find the leftmost point in the row inside the beam
        while !pt_in_beam(x, y, &prog_txt) {
            x += 1;
        }
        let start_x = x + 1;

        // check row to see if there's a point on it that's the top left corner of the square
        while pt_in_beam(x + 99, y, &prog_txt) {
            if pt_in_beam(x, y + 99, &prog_txt) && pt_in_beam(x + 99, y + 99, &prog_txt) {
                println!("Q2: {}", x * 10_000 + y);
                break 'outer;
            }
            x += 1;
        }

        y += 1;
    }

}