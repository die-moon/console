namespace snake {
    public class Dialog : GameObject {
        private int[] center;
        private string message1;
        private string? message2;

        public Dialog (int[] center, int width, int height, string message1, string? message2 = null, string backgroundSymbol = TileSymbol.MEDIUM_SHADE) {
            this.center = center;
            this.message1 = " " + message1 + " ";
            this.message2 = message2 is null ? message2 : " " + message2 + " ";

            int minWidth = (this.message2 is null ? this.message1.Length : this.message2.Length > this.message1.Length ? this.message2.Length : this.message1.Length) + 2;
            width = width > minWidth ? width : minWidth;
            height = height > 3 ? height : 3;
            for (int x = center[0] - width / 2; x < center[0] + width / 2 + 1; x++) {
                for (int y = center[1] - height / 2; y < center[1] + height / 2 + 1; y++) {
                    tiles.Add (new Tile (x, y, backgroundSymbol));
                }
            }
        }

        public new void Render () {
            base.Render ();

            int message1StartX = center[0] - message1.Length / 2;
            int message1StartY = message2 is null ? center[1] : center[1] + 1;

            Console.SetCursorPosition (message1StartX, message1StartY);
            Console.Write (message1);

            if (message2 is not null) {
                Console.SetCursorPosition (center[0] - message2.Length / 2, center[1] - 1);
                Console.Write (message2.ToUpper ());
            }
        }
    }
}