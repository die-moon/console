namespace snake {
    internal class Wall : GameObject {
        public Wall (int[, ] tilePos, string tileSymbol = TileSymbol.FULL_BLOCK2) : base (tilePos, tileSymbol) { }
        public Wall (List<int[]> tilePos, string tileSymbol = TileSymbol.FULL_BLOCK2) : base (tilePos, tileSymbol) { }
    }
}