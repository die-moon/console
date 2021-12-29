namespace snake {
    public class Food : GameObject {
        private int offsetX, offsetY, limit = 1;
        private int[] rangeHorizontal, rangeVertical;
        private string tileSymbol;
        public Food (int offsetX, int offsetY, int[] rangeHorizontal, int[] rangeVertical, int limit, List<Tile> excludes, string tileSymbol = TileSymbol.FULL_BLOCK2) {
            this.mutable = true;

            this.offsetX = offsetX;
            this.offsetY = offsetY;
            this.limit = limit;
            this.rangeHorizontal = rangeHorizontal;
            this.rangeVertical = rangeVertical;
            this.tileSymbol = tileSymbol;

            Update (excludes);
        }

        private Tile GenerateFood (List<Tile> excludes) {
            Tile newFood;
            Random random = new Random ();
            do {
                int foodX = offsetX + random.Next (rangeHorizontal[0], rangeHorizontal[1]) * 2;
                int foodY = offsetY + random.Next (rangeVertical[0], rangeVertical[1]);
                newFood = new Tile (foodX, foodY, tileSymbol);
            } while (excludes.Contains (newFood) || tiles.Contains (newFood));
            newFood.Render ();
            return newFood;
        }

        public void Eaten (Tile food) {
            tiles.Remove (food);
        }

        public void Update (List<Tile> excludes) {
            while (tiles.Count < limit) {
                tiles.Add (GenerateFood (excludes));
            }
        }
    }
}