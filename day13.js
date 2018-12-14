
var cart_factory = {
    cart_id:0,
    make: function(tile) {
        var id = this.cart_id++;
        return { tile:tile, id:id, turns:0 }
    }
};

/* Quick and dirty priority queue */
const add_cart_to_queue = (q, cart, row, col) => {
    for (var spot = 0; spot < q.length; spot++) {
        if (row < q[spot].row || (row == q[spot].row && col < q[spot].col))
            break;
    }

    cart.row = row;
    cart.col = col;
    q.splice(spot, 0, cart);
};

const remove_cart = (q, cart) => {
    for (var j = 0; j < q.length; j++) {
        if (cart.id === q[j].id) {
            q.splice(j, 1);
            break;
        }
    }
};

const dump_map = (map) => {
    for (let j = 0; j < map.length; j++) {
        let s = "";
        for (let k = 0; k < map[j].length; k++) {
    	       s += map[j][k].cart === "" ? map[j][k].track : map[j][k].cart.tile;
        }
        console.log(s);
    }
};

const left_turn = (cart) => {
    if (cart.tile == "^")
        cart.tile = "<";
    else if (cart.tile == "v")
        cart.tile = ">";
    else if (cart.tile == ">")
        cart.tile = "^";
    else
        cart.tile = "v";
};

const right_turn = (cart) => {
    if (cart.tile == "^")
        cart.tile = ">";
    else if (cart.tile == "v")
        cart.tile = "<";
    else if (cart.tile == ">")
        cart.tile = "v";
    else
        cart.tile = "^";
};

const do_turn = (map, cart, sq) => {
    switch (sq.track) {
        case "/":
            if (cart.tile == "^")
                right_turn(cart);
            else if (cart.tile == ">")
                left_turn(cart);
            else if (cart.tile == "v")
                right_turn(cart);
            else
                left_turn(cart);
            break;
        case "\\":
            if (cart.tile == "^")
                left_turn(cart);
            else if (cart.tile == "<")
                right_turn(cart);
            else if (cart.tile == "v")
                left_turn(cart);
            else
                right_turn(cart);
            break;
        case "+":
            /* A junction -- the slightly complicated case */
            if (cart.turns == 0)
                left_turn(cart);
            else if (cart.turns == 2)
                right_turn(cart);
            cart.turns = (cart.turns + 1) % 3;
            break;
    }
}

const do_tick = (world, q2, ticks) => {
    var next_q = [];

    for (let j = 0; j < world.carts.length; j++) {
	let cart = world.carts[j];
	if (cart.collided)
	    continue;
        world.map[cart.row][cart.col].cart = "";
        switch (cart.tile) {
            case "^":
                cart.row -= 1;
                break;
            case "v":
                cart.row += 1;
                break;
            case ">":
                cart.col += 1;
                break;
            case "<":
                cart.col -= 1;
                break;
        }
        var next_sq = world.map[cart.row][cart.col];

        /* Was there a collision? */
        if (next_sq.cart !== "") {
            if (!q2) {
                next_sq.cart = "";
                next_sq.track = "X";
                return { collision:true, row:cart.row, col:cart.col };
            }
            else {
                let victim = next_sq.cart;
                next_sq.cart = "";
		victim.collided = true;
                remove_cart(next_q, victim);
            }
        }
	else {
	    cart.collided = false;
	    next_sq.cart = cart;
            if (!(next_sq == "|" || next_sq == "-"))
		do_turn(world.map, cart, next_sq);
	    add_cart_to_queue(next_q, cart, cart.row, cart.col);
        }
    }
    
    world.carts = next_q.filter(c => !c.collided);
        
    return { collision:false };
}

var carts = [];
const map = [];
var fs = require("fs");
const lines = fs.readFileSync("tracks.txt").toString().split("\n");
for (let r = 0; r < lines.length; r++) {
    var sqs = lines[r].split("");
    var row = [];
    for (let c = 0; c < sqs.length; c++) {
        let cart = "";
        switch (sqs[c]) {
            case "^":
            case "v":
                cart = cart_factory.make(sqs[c]);
                add_cart_to_queue(carts, cart, r, c);
                row.push({ track:"|", cart: cart});
                break;
            case ">":
            case "<":
                cart = cart_factory.make(sqs[c]);
                add_cart_to_queue(carts, cart, r, c);
                row.push({ track:"-", cart:cart });
                break;
            default:
                row.push({ track:sqs[c], cart:cart} );
                break;
        }
    }
    map.push(row);
}

const world = { map:map, carts:carts };

var interactive = false;
var q2 = false;
for (let arg of process.argv) {
    if (arg === "--interactive")
        interactive = true;
    if (arg === "--q2")
        q2 = true;
}

if (interactive) {
    dump_map(map);
    const rl = require('readline');
    rl.emitKeypressEvents(process.stdin);
    process.stdin.setRawMode(true);
    process.stdin.on("keypress", (str, key) => {
        if (key.name === "q")
            process.exit();
        else {
            let res = do_tick(world, q2);
            dump_map(map);
            if (res.collision) {
                console.log("Collision!", res.col, res.row);
                process.exit();
            }
            else if (q2 && world.carts.length == 1) {
                console.log("One cart remaining!", world.carts[0].col, world.carts[0].row);
                process.exit();
            }
        }
    });
}
else {
    var ticks = 0;
    while (true) {
        let res = do_tick(world, q2, ticks);
	++ticks;
        if (!q2 && res.collision) {
            console.log("Collision!", res.col, res.row);
            break;
        }
        else if (q2 && world.carts.length == 1) {
            console.log("One cart remaining!", world.carts[0].col, world.carts[0].row);
            break;
        }	
    }
}
