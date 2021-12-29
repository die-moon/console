namespace snake {
    public abstract class GameObject {
        public bool mutable;
        protected readonly List<Tile> tiles = new List<Tile> ();
        protected GameObject (int[, ] tilePos, string tileSymbol, bool mutable = false) {
            this.mutable = mutable;
            for (int idx = 0; idx < tilePos.GetLength (0); idx++) {
                tiles.Add (new Tile (tilePos[idx, 0], tilePos[idx, 1], tileSymbol));
            }
        }
        protected GameObject (List<int[]> tilePos, string tileSymbol, bool mutable = false) {
            this.mutable = mutable;
            for (int idx = 0; idx < tilePos.Count; idx++) {
                tiles.Add (new Tile (tilePos[idx], tileSymbol));
            }
        }
        protected GameObject () { }

        public void Render () {
            foreach (Tile tile in tiles) {
                tile.Render ();
            }
        }
        public void Clear () {
            foreach (Tile tile in tiles) {
                tile.Clear ();
            }
        }

        public bool Collide (Tile tile) {
            return tiles.Contains (tile);
        }
    }

}