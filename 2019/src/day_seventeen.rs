use std::fs;
use crate::intcode_vm;

fn dump_grid(grid: &Vec<Vec<char>>) {
    for r in 0..grid.len() {
        let row = grid[r].iter().cloned().collect::<String>();
        println!("{}", row);
    }
}

fn is_intersection(r: usize, c: usize, grid: &Vec<Vec<char>>) -> bool {
    if grid[r][c] == '#' && grid[r][c-1] == '#' && grid[r][c+1] == '#'
        && grid[r-1][c] == '#' && grid[r+1][c] == '#' {
        return true;
    }

    false
}

fn count_intersections(grid: &Vec<Vec<char>>) {
    let mut sum = 0;
    for r in 1..grid.len() - 1 {
        let width = grid[r].len();
        if width == 0 {
            continue;
        }
        for c in 1..width - 1 {
            if is_intersection(r, c, grid) {
                sum += r * c;
            }
        }
    }

    println!("Q1: {}", sum);
}

pub fn solve_q1() {
    let prog_txt = fs::read_to_string("./inputs/day17.txt").unwrap();
	let mut vm = intcode_vm::IntcodeVM::new();
	vm.load(prog_txt.trim());

    let mut grid: Vec<Vec<char>> = Vec::new();
    grid.push(Vec::new());
    let mut row = 0;
    while vm.state != intcode_vm::VMState::Halted {
        vm.run();
        if vm.output_buffer == 10 {
            row += 1;
            grid.push(Vec::new());
        }
        else {
            grid[row].push(vm.output_buffer as u8 as char);
        }
    }

    dump_grid(&grid);
    count_intersections(&grid);
}

pub fn solve_q2() {
    let prog_txt = fs::read_to_string("./inputs/day17.txt").unwrap();
	let mut vm = intcode_vm::IntcodeVM::new();
	vm.load(prog_txt.trim());
    vm.write(0, 2);

    // Set up program
    // A,B,A,C,A,B,C,B,C,A
    vm.write_to_buff(65);
    vm.write_to_buff(44);
    vm.write_to_buff(66);
    vm.write_to_buff(44);
    vm.write_to_buff(65);
    vm.write_to_buff(44);
    vm.write_to_buff(67);
    vm.write_to_buff(44);
    vm.write_to_buff(65);
    vm.write_to_buff(44);
    vm.write_to_buff(66);
    vm.write_to_buff(44);
    vm.write_to_buff(67);
    vm.write_to_buff(44);
    vm.write_to_buff(66);
    vm.write_to_buff(44);
    vm.write_to_buff(67);
    vm.write_to_buff(44);
    vm.write_to_buff(65);
    vm.write_to_buff(10);

    // A: L12 R4 R4 L6
    vm.write_to_buff(76);
    vm.write_to_buff(44);
    vm.write_to_buff(49);
    vm.write_to_buff(50);
    vm.write_to_buff(44);
    vm.write_to_buff(82);
    vm.write_to_buff(44);
    vm.write_to_buff(52);
    vm.write_to_buff(44);
    vm.write_to_buff(82);
    vm.write_to_buff(44);
    vm.write_to_buff(52);
    vm.write_to_buff(44);
    vm.write_to_buff(76);
    vm.write_to_buff(44);
    vm.write_to_buff(54);
    vm.write_to_buff(10);

    // B: L12 R4 R4 L12
    vm.write_to_buff(76);
    vm.write_to_buff(44);
    vm.write_to_buff(49);
    vm.write_to_buff(50);
    vm.write_to_buff(44);
    vm.write_to_buff(82);
    vm.write_to_buff(44);
    vm.write_to_buff(52);
    vm.write_to_buff(44);
    vm.write_to_buff(82);
    vm.write_to_buff(44);
    vm.write_to_buff(52);
    vm.write_to_buff(44);
    vm.write_to_buff(82);
    vm.write_to_buff(44);
    vm.write_to_buff(49);
    vm.write_to_buff(50);
    vm.write_to_buff(10);

    // C: L10 L6 R4
    vm.write_to_buff(76);
    vm.write_to_buff(44);
    vm.write_to_buff(49);
    vm.write_to_buff(48);
    vm.write_to_buff(44);
    vm.write_to_buff(76);
    vm.write_to_buff(44);
    vm.write_to_buff(54);
    vm.write_to_buff(44);
    vm.write_to_buff(82);
    vm.write_to_buff(44);
    vm.write_to_buff(52);
    vm.write_to_buff(10);

    // continuous display
    //vm.write_to_buff(121);
    vm.write_to_buff(110);
    vm.write_to_buff(10);

    let mut buffer = String::from("");
    while vm.state != intcode_vm::VMState::Halted {
        vm.run();
        if vm.output_buffer == 10 {
            println!("{}", buffer);
            buffer = String::from("");
        }
        else {
            buffer.push(vm.output_buffer as u8 as char);
        }
    }
    println!("Q2: {:?}", vm.output_buffer);
}
