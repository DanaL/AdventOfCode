use std::fs;
use std::collections::HashMap;

fn fetch_grid() -> Vec<Vec<char>> {
    let mut grid = Vec::new();
    let prog_txt = fs::read_to_string("./inputs/day18_test.txt").unwrap();

    for line in prog_txt.split('\n') {
        grid.push(line.trim().chars().map(|ch| ch).collect::<Vec<char>>());
    }

    grid
}

fn is_open(r: usize, c: usize, grid: &Vec<Vec<char>>) -> bool {
    grid[r][c] == '.' || grid[r][c] == '@' || (grid[r][c] >= 'a' && grid[r][c] <= 'z')
}

fn ff_curr_min_paths_to_keys(sr: usize, sc: usize, grid: &Vec<Vec<char>>) {
    // For my own conceptual ease, I assign an ID to each square so that I can
    // think of (4,3) as 1, (4,4) as 2, etc
    let mut coord_to_sqr: HashMap<(usize, usize), usize> = HashMap::new();
    let mut sqr_to_coord: HashMap<usize, (usize, usize)> = HashMap::new();
    let mut goals: HashMap<char, usize> = HashMap::new();
    let mut id = 0;
    for r in 1..grid.len() - 1 {
        for c in 1..grid[r].len() - 1 {
            if grid[r][c] != '#' {
                coord_to_sqr.insert((r, c), id);
                sqr_to_coord.insert(id, (r, c));
                if grid[r][c] >= 'a' && grid[r][c] <= 'z' {
                    goals.insert(grid[r][c], id);
                }
                id+= 1;
            }
        }
    }

    // Breadth-first search through the maze. We have an unweighted graph
    // (at least for Part 1) so we needn't fuss with Dijkstra's
    let mut distances: HashMap<usize, usize> = HashMap::new();

    // Buld the table of connections between nodes and store the IDs
    // of all the keys as the goals we want to reach in the maze
    // Rust is just such a verbose language T_T
    let mut queue: Vec<(usize, usize)> = vec![(*coord_to_sqr.get(&(sr, sc)).unwrap(), 0)];
    while queue.len() > 0 {
        let v = queue.pop().unwrap();
        let sq_id = v.0;
        let dist = v.1 + 1;
        let (r, c) = sqr_to_coord.get(&sq_id).unwrap();

        // Check if the adjacent squares are open
        if is_open(r - 1, *c, &grid) {
            let n_id = *coord_to_sqr.get(&(r - 1, *c)).unwrap();
            if !distances.contains_key(&n_id) {
                distances.insert(n_id, dist);
                queue.push((n_id, dist));
            }
        }
        if is_open(r + 1, *c, &grid) {
            let n_id = *coord_to_sqr.get(&(r + 1, *c)).unwrap();
            if !distances.contains_key(&n_id) {
                distances.insert(n_id, dist);
                queue.push((n_id, dist));
            }
        }
        if is_open(*r, c - 1, &grid) {
            let n_id = *coord_to_sqr.get(&(*r, c - 1)).unwrap();
            if !distances.contains_key(&n_id) {
                distances.insert(n_id, dist);
                queue.push((n_id, dist));
            }
        }
        if is_open(*r, c + 1, &grid) {
            let n_id = *coord_to_sqr.get(&(*r, c + 1)).unwrap();
            if !distances.contains_key(&n_id) {
                distances.insert(n_id, dist);
                queue.push((n_id, dist));
            }
        }
    }

    for g in goals {
        if distances.contains_key(&g.1) {
            println!("{} {:?}", g.0, distances.get(&g.1).unwrap());
        }
    }

    /*
    let mut cons = vec![vec![false; id]; id];
    for r in 1..grid.len() - 1 {
        for c in 1..grid[r].len() - 1 {
            if is_open(r, c, &grid) {
                // Note down the IDs of the keys (a, b, c, etc)
                if grid[r][c] >= 'a' && grid[r][c] <= 'z' {
                    goals.insert(grid[r][c], *sqrs.get(&(r, c)).unwrap());
                }

                // Check if the adjacent squares are open
                let mut sq_id = *sqrs.get(&(r, c)).unwrap();
                if is_open(r - 1, c, &grid) {
                    let n_id = *sqrs.get(&(r - 1, c)).unwrap();
                    cons[sq_id][n_id] = true;
                }
                sq_id = *sqrs.get(&(r, c)).unwrap();
                if is_open(r + 1, c, &grid) {
                    let n_id = *sqrs.get(&(r + 1, c)).unwrap();
                    cons[sq_id][n_id] = true;
                }
                sq_id = *sqrs.get(&(r, c)).unwrap();
                if is_open(r, c - 1, &grid) {
                    let n_id = *sqrs.get(&(r, c - 1)).unwrap();
                    cons[sq_id][n_id] = true;
                }
                sq_id = *sqrs.get(&(r, c)).unwrap();
                if is_open(r, c + 1, &grid) {
                    let n_id = *sqrs.get(&(r, c + 1)).unwrap();
                    cons[sq_id][n_id] = true;
                }
            }
        }
    }
    */
    /*
    println!("{:?}", sqrs);
    println!("{}, {} -> {}", r, c, *sqrs.get(&(r, c)).unwrap()  );
    println!("{}", cons[6][5]);
    println!("{}", cons[6][7]);
    println!("{}", cons[6][3]);
    println!("{:?}", goals);
    */
    // Step one, flood fill to find all possible connections from (r, c)
    //let mut paths: HashMap<(char, char), Vec<(u32, u32)>> = HashMap::new();
}

pub fn solve_q1() {
    let mut grid = fetch_grid();

    let mut start_r: usize = 0;
    let mut start_c: usize = 0;
    'outer: for r in 0..grid.len() {
        for c in 0..grid[r].len() {
            if grid[r][c] == '@' {
                start_r = r;
                start_c = c;
                break 'outer;
            }
        }
    }

    ff_curr_min_paths_to_keys(start_r, start_c, &grid);
}