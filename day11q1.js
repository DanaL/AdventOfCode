const fuel_level = (sn, y, x) => {
    const rack_id = x + 10;
    var power = (y * rack_id + sn) * rack_id;
    if (Math.abs(power) < 100)
	power = -5;
    else
	power = Math.floor((power % 1000) / 100) - 5;

    return power;
};

const sn = 1133

var biggest = Number.MIN_SAFE_INTEGER;
for (let row = 2; row < 300; row++) {
    for (let col = 2; col < 300; col++) {
	let pl = fuel_level(sn, row - 1, col - 1) + fuel_level(sn, row - 1, col) + fuel_level(sn, row - 1, col + 1) +
	    fuel_level(sn, row, col - 1) + fuel_level(sn, row, col) + fuel_level(sn, row, col + 1) +
	    fuel_level(sn, row + 1, col - 1) + fuel_level(sn, row + 1, col) + fuel_level(sn, row + 1, col + 1);
	
	if (pl > biggest) {
	    biggest = pl;
	    var x = col - 1;
	    var y = row - 1;
	}
    }
}

console.log(x, y);
