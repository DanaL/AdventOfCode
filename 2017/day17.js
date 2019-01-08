const SPINLOCK_SIZE = 2018;

let new_node = (val) => {
	return { val:val, next:null };
}

let new_list = () => {
	return { 
		size:0, 
		head:null,
		insert(pos, val) {
			let node = new_node(val);
			if (this.size === 0) {
				this.head = node;
			}
			else {
				let curr = this.head;
				for (let j = 0; j < pos; j++)
					curr = curr.next;
				node.next = curr.next;
				curr.next = node;
			}
			++this.size;
		},
		seek(pos) {
			let curr = this.head;
			for (let j = 0; j < pos; j++)
				curr = curr.next;
			return curr.val;
		},
		dump() {
			let curr = this.head;
			let str = "";
			while (curr != null) {
				str += curr.val + " ";
				curr = curr.next;
			}
			console.log(str);
		}
	}
}

let q1 = (steps) => {
	const list = new_list();
	list.insert(0, 0);

	let pos = 0;	
	for (let j = 1; j < 2018; j++) {
		pos = (pos + steps) % list.size;
		list.insert(pos, j);
		++pos;	
	}

	console.log("Q1: " + list.seek(pos+1));	
}

let q2 = (steps) => {
	let pos = 0;
	for (let j = 1; j < 50000001; j++) {
		pos = (pos + steps) % j + 1;
		if (pos === 1) 
			var val = j; 
	}

	console.log("Q2: " + val);
}

let main = () => {
	q1(356);
	q2(356);
}

main();
