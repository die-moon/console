namespace snake {
    public class Tile {
        public static string? BlankSymbol;

        public int X { get; }
        public int Y { get; }

        //public string TileType { get; }
        private string tileSymbol;

        public Tile (int x, int y, string tileSymbol = " ") {
            _ = BlankSymbol ??
                throw new ArgumentNullException (nameof (BlankSymbol));

            X = x;
            Y = y;
            this.tileSymbol = tileSymbol;
        }
        public Tile (int[] coord, string tileSymbol = " ") {
            _ = BlankSymbol ??
                throw new ArgumentNullException (nameof (BlankSymbol));
            X = coord[0];
            Y = coord[1];
            this.tileSymbol = tileSymbol;
        }

        public void UpdateSymbol (string symbol) {
            tileSymbol = symbol;
            Render ();
        }

        public void Clear () {
            Console.SetCursorPosition (X, Y);
            Console.Write (Tile.BlankSymbol);

        }

        public void Render () {
            Console.SetCursorPosition (X, Y);
            Console.Write (tileSymbol);
        }

        public override int GetHashCode () {
            return HashCode.Combine (X, Y);
        }

        public override bool Equals (object? obj) {
            return obj is Tile tile &&
                X == tile.X &&
                Y == tile.Y;
        }

        public static Tile operator + (Tile a, int[] b) {
            return new Tile (a.X + b[0], a.Y + b[1], a.tileSymbol);
        }
        public static Tile operator + (Tile t, Direction d) {
            return new Tile (t.X + d.X, t.Y + d.Y, t.tileSymbol);
        }
        public static Tile operator - (Tile t, Direction d) {
            return new Tile (t.X - d.X, t.Y - d.Y, t.tileSymbol);
        }
    }
}