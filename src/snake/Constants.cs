using System.Diagnostics.CodeAnalysis;

namespace snake {
    public struct Direction {
        public int X { get; }
        public int Y { get; }

        public Direction (int x, int y) {
            X = x;
            Y = y;
        }

        public static Direction operator + (Direction a, int[] b) {
            return new Direction (a.X + b[0], a.Y + b[1]);
        }
        public static bool operator == (Direction a, Direction b) => a.X == b.X && a.Y == b.Y;
        public static bool operator != (Direction a, Direction b) => !(a == b);

        public override bool Equals ([NotNullWhen (true)] object? obj) {
            return obj is Direction d && d.X == X && d.Y == Y;
        }

        public override int GetHashCode () {
            return HashCode.Combine (X, Y);
        }
    }
    public static class Directions {
        public static readonly Direction UP = new Direction (0, -1),
            RIGHT = new Direction (2, 0),
            DOWN = new Direction (0, 1),
            LEFT = new Direction (-2, 0);

        public static Direction RandomDirection () {
            Random random = new Random ();
            int randomX = random.Next (-1, 2);
            return randomX > 0 ? RIGHT : randomX == 0 ? random.Next (0, 2) * 2 - 1 > 0 ? DOWN : UP : LEFT;
        }
    }

    public static class TileSymbol {

        public const string BLANK = " ",
            BLANK2 = "  ",
            FULL_BLOCK = "█",
            FULL_BLOCK2 = "██",
            BOX_DOUBLE_TOP_LEFT = "╔",
            BOX_DOUBLE_TOP_RIGHT = "╗",
            BOX_DOUBLE_BOTTOM_LEFT = "╚",
            BOX_DOUBLE_BOTTOM_RIGHT = "╝",
            BOX_DOUBLE_HORIZONTAL = "═",
            BOX_DOUBLE_VERTICAL = "║",
            BOX_DOUBLE_VERTICAL_RIGHT = "╠",
            BOX_DOUBLE_VERTICAL_LEFT = "╣",
            BOX_DOUBLE_HORIZONTAL_VERTICAL = "╬",
            WHITE_STAR = "☆",
            BLACK_STAR = "★",
            LIGHT_SHADE = "░",
            MEDIUM_SHADE = "▒",
            DARK_SHADE = "▓",
            EGG = "🥚",
            BLACK_HEART = "♥",
            WHITE_HEART = "♡",
            BLACK_CIRCLE = "●",
            MEDIUM_BLACK_CIRCLE = "⚫",
            LARGE_BLACK_CIRCLE = "⬤",
            BLACK_SQUARE = "■",
            MEDIUM_BLACK_SQUARE = "◼",
            LARGE_BLACK_SQUARE = "⬛";

    }

    // public enum TileSymbol {
    //     /// <summary>
    //     /// 缺省符号
    //     /// </summary>
    //     DEFAULT = (UInt16) 0xA1A1,
    //     /// <summary>
    //     /// @@
    //     /// </summary>
    //     AT = (UInt16) 0x4040,
    //     /// <summary>
    //     /// ##
    //     /// </summary>
    //     WELL = (UInt16) 0x2323,
    //     /// <summary>
    //     /// ※
    //     /// </summary>
    //     RICE = (UInt16) 0xA1F9,
    //     /// <summary>
    //     /// ☆
    //     /// </summary>
    //     STAR_EMPTY = (UInt16) 0xA1EE,
    //     /// <summary>
    //     /// ★
    //     /// </summary>
    //     STAR_SOLID = (UInt16) 0xA1EF,
    //     /// <summary>
    //     /// ○
    //     /// </summary>
    //     RING_EMPTY = (UInt16) 0xA1F0,
    //     /// <summary>
    //     /// ●
    //     /// </summary>
    //     RING_SOLID = (UInt16) 0xA1F1,
    //     /// <summary>
    //     /// ◇
    //     /// </summary>
    //     RHOMB_EMPTY = (UInt16) 0xA1F3,
    //     /// <summary>
    //     /// ◆
    //     /// </summary>
    //     RHOMB_SOLID = (UInt16) 0xA1F4,
    //     /// <summary>
    //     /// □
    //     /// </summary>
    //     RECT_EMPTY = (UInt16) 0xA1F5,
    //     /// <summary>
    //     /// ■  █
    //     /// </summary>
    //     RECT_SOLID = (UInt16) 0xA1F6,
    //     /// <summary>
    //     /// △
    //     /// </summary>
    //     TRIANGLE_EMPTY = (UInt16) 0xA1F7,
    //     /// <summary>
    //     /// ▲
    //     /// </summary>
    //     TRIANGLE_SOLID = (UInt16) 0xA1F8,
    // }
    // internal sealed class TileSymbolHelper {
    // public static string GetStringFromSymbol (TileSymbol symbol) {
    //     UInt16 symbalValue = (UInt16) symbol;
    //     byte[] bytes = {
    //         (byte) ((symbalValue & 0xFF00) >> 8),
    //         (byte) (symbalValue & 0x00FF)
    //     };
    //     return Encoding.Default.GetString (bytes);
    // }

    //public static string GetSymbolHex(string symbol)
    //{
    //    if (MyText.GetLength(symbol) > 2 || symbol == "")
    //    {
    //        throw new ArgumentOutOfRangeException("符号不能为空且长度不能超过2字节！");
    //    }
    //    string hex = "0x";
    //    byte[] bytes = Encoding.Default.GetBytes(symbol);
    //    foreach (byte b in bytes)
    //    {
    //        hex += string.Format("{0:X}", b);
    //    }
    //    return hex;
    //}
    // }

}