
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

const dump_map = (map) => {
    for (let j = 0; j < map.length; j++) {
        let s = "";
        for (let k = 0; k < map[j].length; k++) {
    	       s += map[j][k].cart == "" ? map[j][k].track : map[j][k].cart.tile;
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
            break;
    }
}

const do_tick = (world) => {
    var next_q = [];
    for (let cart of world.carts) {
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
            world.map[cart.row][cart.col].track = "X";
            world.map[cart.row][cart.col].cart = "";
            return { collision:true, row:cart.row, col:cart.col };
        }

        if (!(next_sq == "|" || next_sq == "-"))
            do_turn(world.map, cart, next_sq);
        world.map[cart.row][cart.col].cart = cart;
        add_cart_to_queue(next_q, cart, cart.row, cart.col);
        world.carts = next_q;
    }

    return { collision:false };
}

var carts = [];
const map = [];
var fs = require("fs");
const lines = fs.readFileSync("tracks_sm.txt").toString().split("\n");
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

dump_map(map);

const rl = require('readline');
rl.emitKeypressEvents(process.stdin);
process.stdin.setRawMode(true);
process.stdin.on("keypress", (str, key) => {
    if (key.name === "q") {
        process.exit();
    }
    else {
        let res = do_tick(world);
        dump_map(map);

        if (res.collision) {
            console.log("Collision!", res.row, res.col);
            process.exit();
        }
    }
});
