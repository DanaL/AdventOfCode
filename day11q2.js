const calc_power_lvl = (serial_num, x , y) => {
    const rack_id = x + 10;
    var pwr_lvl = (rack_id * y + serial_num) * rack_id;
    if (Math.abs(pwr_lvl) < 100)
        return -5;
    else
        return Math.floor((pwr_lvl % 1000) / 100) - 5;
};

const dump_matrix_section = (m, x, y, size) => {
    for (let r = y; r < y + size; r++) {
        var s = "";
        for (let c = x; c < x + size; c++)
            s += m[r][c].toString().padStart(3, ' ');
        console.log(s);
    }
}

/* So what we are doing is this: y, x (y is row, x is column) form the bottom
    right corner of the sub-matrix we are checking and its length/width is size.

    So:
        4  -1   7   0
        3  -2  -4   5
        5   0   0   1
        1  -1  -1   2

    If we want the sum of the 2x2 matrix comprising the bottom right corner,
    we take m[y, x] which is 2. We want to subtract m[y - 2, x], which -1 and
    m[y, x - 2], which is 5, THEN add m[y - 2, x - 2]. Basically, we add
    the two matrix sub-totals comprising the cells we want to ignore. However,
    we've subtracted their intersection twice, so we need to add them back. Ie:

        C   C    A   A
        C   C    A   A
        B   B    0   1
        B   B   -1   2

    The cells marked C are where the cells overlap and were double counted.

    Of course, if we have a situation were we want the 2x2 matrix whose lower
    right corner is 1, 3, we only need to subtract m[y, x - 2] from its total.
*/
const sum_of_matrix(sub_sums, x, y, size) {
    var sum = sub_sums[y][x];
    const ya = y - 2;
    const xb = x - 2;
    if (ya >= 0) sum -= sub_sums[ya][x];
    if (xb >= 0) sum -= sub_sums[y][xb];
    if (ya >= 0 && xb >= 0) sum == sub_sumbs[ya][xb];

    return sum;
}

const serial_num = 1133;
const matrix_size = 5;

/* Calculate our matrix of power levels */
const grid = [];
for (let r = 0; r < matrix_size; r++) {
    let row = [];
    for (let c = 0; c < matrix_size; c++) {
        row.push(calc_power_lvl(serial_num, c, r));
    }
    grid.push(row);
}

/* Now calcuate our sub-matrix sums */
const sub_sums = [];
for (let r = 0; r < matrix_size; r++) {
    let row = [];
    let row_sum = 0;
    for (let c = 0; c < matrix_size; c++) {
        row_sum += grid[r][c];
        //if (r > 0)
        //    row_sum += sub_sums[r-1][c];
        row.push(r == 0 ? row_sum : row_sum + sub_sums[r-1][c]);
    }
    sub_sums.push(row);
}

dump_matrix_section(grid, 0, 0, 5);
console.log("");
dump_matrix_section(sub_sums, 0, 0, 5);
//console.log(calc_sub_matrix_sum(grid, 3, 4, 3));
