using System.Diagnostics.CodeAnalysis;

namespace snake {
    internal class SnakeGame : ConsoleGame {
        /*Manually set values:
        width: snake movable area width
        height: snake movable area height
        infoAreaHeight: game statistic information area height
        borderSize: main frame border size
        TILE_SYMBOLS: symbols used to render game objects
        foodLimit: max number of foods exists the same time
        */
        private static readonly int width = 50,
            height = 30,
            infoAreaHeight = 13,
            borderSize = 1;
        private static class TILE_SYMBOLS {
            public const string BLANK = TileSymbol.BLANK2,
                FRAME_HORIZONTAL = TileSymbol.BOX_DOUBLE_HORIZONTAL,
                FRAME_VERTICAL = TileSymbol.BOX_DOUBLE_VERTICAL,
                FRAME_TOP_LEFT = TileSymbol.BOX_DOUBLE_TOP_LEFT,
                FRAME_TOP_RIGHT = TileSymbol.BOX_DOUBLE_TOP_RIGHT,
                FRAME_BOTTOM_LEFT = TileSymbol.BOX_DOUBLE_BOTTOM_LEFT,
                FRAME_BOTTOM_RIGHT = TileSymbol.BOX_DOUBLE_BOTTOM_RIGHT,
                FRAME_VERTICAL_RIGHT = TileSymbol.BOX_DOUBLE_VERTICAL_RIGHT,
                FRAME_VERTICAL_LEFT = TileSymbol.BOX_DOUBLE_VERTICAL_LEFT,
                HEAD = TileSymbol.FULL_BLOCK2,
                BODY = TileSymbol.FULL_BLOCK2,
                TAIL = TileSymbol.FULL_BLOCK2,
                FOOD = TileSymbol.BLACK_SQUARE,
                WALL = TileSymbol.FULL_BLOCK2;
        }
        private int foodLimit = 1;

        private static readonly int innerHeight = height + infoAreaHeight,
            innerWidth = width * 2,
            totalHeight = innerHeight + borderSize * 2,
            totalWidth = innerWidth + borderSize * 2;

        private readonly int left, top, gameAreaStartX, gameAreaStartY;

        private int highScore, score;
        private string? playerName;

        public Wall wall;
        public Snake snake;
        public Food food;
        private Dialog? dialog;

        public SnakeGame (int left = 1, int top = 1) : base (totalWidth + left * 4, totalHeight + top * 2, "Snake") {

            this.left = left * 2;
            this.top = top;

            gameAreaStartX = this.left + borderSize;
            gameAreaStartY = this.top + infoAreaHeight + borderSize;

            Initialize ();
            GenerateObjects ();
            BindKeyHandler ();
            Render ();

            GetPlayerName ();
            ShowStatistics ();
        }

        private void Initialize () {
            Tile.BlankSymbol = TILE_SYMBOLS.BLANK;
            ShowTitle ();
            ShowRules ();
            GenerateFrame ();
        }

        [MemberNotNull (nameof (wall), nameof (snake), nameof (food))]
        private void GenerateObjects () {
            wall = GenerateWall ();
            snake = GenerateSnake ();
            food = GenerateFood ();
            AddObjects (wall, snake, food);
        }

        private void ShowTitle () {
            string title = @"
                                                            **
                                                           /**
                                  ****** *******   ******  /**  **  *****
                                 **//// //**///** //////** /** **  **///**
                                //*****  /**  /**  ******* /****  /*******
                                 /////** /**  /** **////** /**/** /**////
                                 ******  ***  /**//********/**//**//******
                                //////  ///   //  //////// //  //  //////
";
            Console.SetCursorPosition (0, 0);
            Console.WriteLine (title);
        }

        private void ShowRules () {
            Console.SetCursorPosition (left + borderSize, top + infoAreaHeight - 4);
            Console.Write ("Use arrow keys to change snake's direction.\n   \"p\" - pause game; \"r\" - restart game; \"q\" - exit");
        }

        private void GenerateFrame () {
            List<int[]> frameHorizontal = new List<int[]> ();
            List<int[]> frameVertical = new List<int[]> ();
            for (int x = 1; x < totalWidth - 1; x++) {
                frameHorizontal.Add (new int[] { x + left, top });
                frameHorizontal.Add (new int[] { x + left, gameAreaStartY + height });

                frameHorizontal.Add (new int[] { x + left, gameAreaStartY - 1 });
            }
            for (int y = 1; y < totalHeight - 1; y++) {
                if (y != infoAreaHeight) {
                    frameVertical.Add (new int[] { left, y + top + borderSize - 1 });
                    frameVertical.Add (new int[] { gameAreaStartX + innerWidth, y + top + borderSize - 1 });
                }
            }

            this.AddObjects (
                new Wall (frameHorizontal, TILE_SYMBOLS.FRAME_HORIZONTAL),
                new Wall (frameVertical, TILE_SYMBOLS.FRAME_VERTICAL),
                new Wall (new int[, ] { { left, top } }, TILE_SYMBOLS.FRAME_TOP_LEFT),
                new Wall (new int[, ] { { left + innerWidth + borderSize, top } }, TILE_SYMBOLS.FRAME_TOP_RIGHT),
                new Wall (new int[, ] { { left, top + innerHeight + borderSize } }, TILE_SYMBOLS.FRAME_BOTTOM_LEFT),
                new Wall (new int[, ] { { left + innerWidth + borderSize, top + innerHeight + borderSize } }, TILE_SYMBOLS.FRAME_BOTTOM_RIGHT),
                new Wall (new int[, ] { { left, gameAreaStartY - 1 } }, TILE_SYMBOLS.FRAME_VERTICAL_RIGHT),
                new Wall (new int[, ] { { left + innerWidth + borderSize, gameAreaStartY - 1 } }, TILE_SYMBOLS.FRAME_VERTICAL_LEFT)
            );
        }

        private Wall GenerateWall () {

            List<int[]> wall = new List<int[]> ();

            for (int x = 0; x < width; x++) {
                wall.Add (new int[] { gameAreaStartX + x * 2, gameAreaStartY });
                wall.Add (new int[] { gameAreaStartX + x * 2, gameAreaStartY + height - 1 });
            }
            for (int y = 1; y < height - 1; y++) {
                wall.Add (new int[] { gameAreaStartX, gameAreaStartY + y });
                wall.Add (new int[] { gameAreaStartX + innerWidth - 2, gameAreaStartY + y });
            }

            return new Wall (wall, TILE_SYMBOLS.WALL);
        }

        private Snake GenerateSnake () {
            int initialHeadX = gameAreaStartX + 2 + width / 4 * 4;
            int initialHeadY = gameAreaStartY + 1 + height / 4 * 2;
            Direction initialDirection = Directions.RandomDirection ();
            return new Snake (
                new int[, ] { { initialHeadX, initialHeadY }, { initialHeadX - initialDirection.X, initialHeadY - initialDirection.Y } },
                initialDirection,
                TILE_SYMBOLS.HEAD,
                TILE_SYMBOLS.BODY,
                TILE_SYMBOLS.TAIL,
                TILE_SYMBOLS.BLANK
            );
        }

        private Food GenerateFood () {
            return new Food (
                gameAreaStartX,
                gameAreaStartY,
                new int[] { 1, width - 1 },
                new int[] { 1, height - 1 },
                foodLimit,
                snake.Bodies,
                TILE_SYMBOLS.FOOD
            );
        }

        private void GetPlayerName () {
            do {
                Console.SetCursorPosition (left + borderSize, top + infoAreaHeight - 1);
                Console.Write ("Enter your name: ");
                playerName = Console.ReadLine ();
            } while (string.IsNullOrEmpty (playerName));
            Console.CursorVisible = false;
        }

        private void ShowStatistics () {
            Console.SetCursorPosition (left + borderSize, top + infoAreaHeight - 1);
            Console.Write ("Player: {0}    SCORE: {1}    HIGH SCORE: {2}", playerName, score, highScore);
        }

        public override void Start () {
            while (true) {
                dialog = new Dialog (new int[] { gameAreaStartX + 2 + width / 4 * 4, gameAreaStartY + 1 + height / 4 * 2 }, 30, 3, "press \"SPACE\" to start");
                dialog.Render ();
                ReadKey (ConsoleKey.Spacebar, ConsoleKey.Q);

                if (started) {
                    Loop ();
                }
            }
        }
        protected override void Loop () {
            while (true) {
                if (Console.KeyAvailable)
                    ReadKey ();

                if (started) {
                    if (!paused && !snake.dead) Update ();
                } else {
                    break;
                }

                Thread.Sleep (200);
            }
        }
        public override void Update () {
            snake.Update (CheckSnake);
            food.Update (snake.Bodies);
        }

        private string CheckSnake (Tile head) {
            // Console.WriteLine ("check snake: {0}-{1}", head.X, head.Y);
            if (wall.Collide (head) || snake.Collide (head)) {
                // started = false;
                return "dead";
            }
            if (food.Collide (head)) {
                food.Eaten (head);
                score++;
                RecordHighScore ();
                ShowStatistics ();
                return "food";
            }
            return "";
        }

        private void RecordHighScore () {
            highScore = score > highScore ? score : highScore;
        }

        // key handlers
        protected override void StartLoop (ConsoleKey key) {
            if (!started) {
                if (dialog is not null) dialog.Clear ();
                started = true;
            }
        }
        protected override void Pause (ConsoleKey key) {
            if (paused) {
                if (dialog is not null) dialog.Clear ();
                snake.Render ();
                food.Render ();
            } else {
                dialog = new Dialog (new int[] { gameAreaStartX + 2 + width / 4 * 4, gameAreaStartY + 1 + height / 4 * 2 }, 35, 5, "press \"p\" again to resume", "P A U S E D");
                dialog.Render ();
            }
            paused = !paused;
        }
        protected override void Restart (ConsoleKey key) {
            if (dialog is not null) dialog.Clear ();

            started = false;
            paused = false;
            score = 0;
            ShowStatistics ();

            Clear ();
            GenerateObjects ();
            BindKeyHandler ();
            Render ();
        }
        protected override void BindKeyHandler () {
            base.BindKeyHandler ();

            keyHandlers.Add (ConsoleKey.UpArrow, snake.Turn);
            keyHandlers.Add (ConsoleKey.RightArrow, snake.Turn);
            keyHandlers.Add (ConsoleKey.DownArrow, snake.Turn);
            keyHandlers.Add (ConsoleKey.LeftArrow, snake.Turn);
        }
    }
}