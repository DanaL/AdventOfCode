module.exports = {
    DSNode : (val) => {
        return {
            parent:null,
            rank:0,
            value:val,

            find() {
                if (this.parent === null || this.parent === this)
                    return this;
                else {
                    let n = this.parent;
                    while (n.parent !== null && n.parent !== n)
                        n = n.parent;
                    this.parent = n;
                    return n;
                }
            },

            union(n) {
                let root1 = this.find();
                let root2 = n.find();

                if (root1.rank > root2.rank)
                    root2.parent = root1;
                else if (root1.rank < root2.rank)
                    root1.parent = root2;
                else
                    root2.parent = root1;
                    ++root1.rank;
            }
        }
    }
}
