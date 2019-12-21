use std::fs;
use std::collections::HashMap;

fn fetch_map() -> Vec<Vec<char>> {
    let mut grid = Vec::new();
    let prog_txt = fs::read_to_string("./inputs/day20.txt").unwrap();

    for line in prog_txt.split('\n') {
        grid.push(line.chars().filter(|ch| *ch != '\r').collect::<Vec<char>>());
    }

    grid
}

fn check_surrounds(sr: i32, sc: i32, grid: &Vec<Vec<char>>, nodes: &mut HashMap<String, Vec<(usize, usize)>>) {
    let mut name: String = String::from("");
    name.push(grid[sr as usize][sc as usize]);
    let mut portal = (0, 0);

    // Every portal will have a name and be beside a .
    for r in -1..2 {
        for c in -1..2 {
            if r != 0 || c != 0 {
                let sq = grid[(sr + r) as usize][(sc + c) as usize];
                if sq.is_ascii_uppercase() {
                    name.push(sq);
                }
                else if sq == '.' {
                    portal.0 = (sr + r) as usize;
                    portal.1 = (sc + c) as usize;
                }
            }
        }
    }

    nodes.entry(name).or_insert(Vec::new()).push(portal);
}

// My overly verbose code to parse the node names out of the file.  The simplest way 
// to find node names I could think of was to sweep sides across rows, then
// down the columns.
fn find_all_nodes(grid: &Vec<Vec<char>>) -> HashMap<String, Vec<(usize, usize)>> {
    let mut nodes: HashMap<String, Vec<(usize, usize)>> = HashMap::new();

    for r in 0..grid.len() - 1 {
        let mut c = 0;
        let len = grid[r].len() - 1;
        while c < len {
            if grid[r][c].is_ascii_uppercase() && grid[r][c + 1].is_ascii_uppercase() {
                let mut name: String = String::from("");
                name.push(grid[r][c]);
                name.push(grid[r][c + 1]);

                let mut portal = (0, 0);
                if c > 0 && c < len && grid[r][c - 1] == '.' {
					portal.0 = r;
                	portal.1 = c - 1;
                }
                if c < len - 2 && grid[r][c + 2] == '.' {
                    portal.0 = r;
                   portal.1 = c + 2;
                }

                c+= 1;
    			nodes.entry(name).or_insert(Vec::new()).push(portal);
            }
            c += 1;
        }
    }
	
	for c in 0..grid[0].len() - 1 {
        let mut r = 0;
        let len = grid.len() - 1;
        while r < len {
            if grid[r][c].is_ascii_uppercase() && grid[r + 1][c].is_ascii_uppercase() {
                let mut name: String = String::from("");
                name.push(grid[r][c]);
                name.push(grid[r + 1][c]);

                let mut portal = (0, 0);
                if r > 0 && r < len && grid[r - 1][c] == '.' {
                    portal.0 = r - 1;
                    portal.1 = c;
                }
                if r < len - 2 && grid[r + 2][c] == '.' {
                    portal.0 = r + 2;
					portal.1 = c;
                }

                r+= 1;
    			nodes.entry(name).or_insert(Vec::new()).push(portal);
            }
            r += 1;
        }
    }
	
    nodes
}

fn build_graph(grid: &Vec<Vec<char>>, nodes: &HashMap<String, Vec<(usize, usize)>>) {
	println!("{:?}", nodes);
}

pub fn solve_q1() {
    let grid = fetch_map();
 	let nodes = find_all_nodes(&grid);
	build_graph(&grid, &nodes);
}
