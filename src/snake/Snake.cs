namespace snake {
    class Snake : GameObject {
        public bool dead = false;
        private Direction direction;

        private readonly string headSymbol, bodySymbol, tailSymbol, blankSymbol;

        private Tile? nextHead;

        public List<Tile> Bodies { get { return tiles; } }

        public Snake (
            int[, ] tilePos,
            Direction? direction,
            string headSymbol = TileSymbol.FULL_BLOCK2,
            string bodySymbol = TileSymbol.FULL_BLOCK2,
            string tailSymbol = TileSymbol.FULL_BLOCK2,
            string blankSymbol = TileSymbol.BLANK2
        ) : base (tilePos, tileSymbol : bodySymbol, mutable : true) {
            this.headSymbol = headSymbol;
            this.bodySymbol = bodySymbol;
            this.tailSymbol = tailSymbol;
            this.blankSymbol = blankSymbol;

            this.direction = direction ?? Directions.LEFT;

            tiles.First ().UpdateSymbol (headSymbol);
            tiles.Last ().UpdateSymbol (tailSymbol);
        }

        public Snake (
            List<int[]> tilePos,
            Direction? direction,
            string headSymbol = TileSymbol.FULL_BLOCK2,
            string bodySymbol = TileSymbol.FULL_BLOCK2,
            string tailSymbol = TileSymbol.FULL_BLOCK2,
            string blankSymbol = TileSymbol.BLANK2
        ) : base (tilePos, tileSymbol : bodySymbol, mutable : true) {
            this.headSymbol = headSymbol;
            this.bodySymbol = bodySymbol;
            this.tailSymbol = tailSymbol;
            this.blankSymbol = blankSymbol;

            this.direction = direction ?? Directions.LEFT;

            tiles.First ().UpdateSymbol (headSymbol);
            tiles.Last ().UpdateSymbol (tailSymbol);
        }

        public new bool Collide (Tile tile) {
            return tiles.Take (tiles.Count - 1).Contains (tile);
        }

        public void Update (Func<Tile, string> CheckNext) {
            nextHead = tiles.First () + direction;
            switch (CheckNext (nextHead)) {
                case "food":
                    Eat ();
                    break;
                case "dead":
                    dead = true;
                    break;
                default:
                    Move ();
                    break;
            }
        }

        private void Move () {
            if (nextHead is not null) {
                tiles.Last ().UpdateSymbol (blankSymbol);
                tiles.RemoveAt (tiles.Count - 1);
                tiles.Last ().UpdateSymbol (tailSymbol);

                tiles.First ().UpdateSymbol (bodySymbol);
                nextHead.Render ();
                tiles.Insert (0, nextHead);
            }
        }

        private void Eat () {
            if (nextHead is not null) {
                tiles.First ().UpdateSymbol (bodySymbol);
                nextHead.Render ();
                tiles.Insert (0, nextHead);
            }
        }

        // TODO if one arrow key got pushed many times, next different key press will be blocked
        public void Turn (ConsoleKey key) {
            switch (key) {
                case ConsoleKey.UpArrow:
                    if (direction != Directions.UP && direction != Directions.DOWN) {
                        direction = Directions.UP;
                    }
                    break;
                case ConsoleKey.RightArrow:
                    if (direction != Directions.LEFT && direction != Directions.RIGHT) {
                        direction = Directions.RIGHT;
                    }
                    break;
                case ConsoleKey.DownArrow:
                    if (direction != Directions.UP && direction != Directions.DOWN) {
                        direction = Directions.DOWN;
                    }
                    break;
                case ConsoleKey.LeftArrow:
                    if (direction != Directions.LEFT && direction != Directions.RIGHT) {
                        direction = Directions.LEFT;
                    }
                    break;
                default:
                    break;
            }
        }
    }
}