use std::fs;
use crate::intcode_vm;

pub fn solve_q1() {
    let prog_txt = fs::read_to_string("./inputs/day19.txt").unwrap();
	let mut vm = intcode_vm::IntcodeVM::new();

    let mut sum = 0;
    for x in 0..50 {
        for y in 0..50 {
            vm.load(prog_txt.trim());
            vm.write_to_buff(x);
            vm.write_to_buff(y);
            vm.run();
            sum += vm.output_buffer;
        }
    }
    println!("Q1 {:?}", sum);
}