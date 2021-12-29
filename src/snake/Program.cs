using System.Diagnostics;
using System.Runtime.InteropServices;

namespace snake {
    class Program {
        static void Main (string[] args) {
            // TODO check cross platform
            if (RuntimeInformation.IsOSPlatform (OSPlatform.Windows)) {
                Process.Start ("CMD.exe", "/C chcp 65001 > nul");
            }

            SnakeGame game = new SnakeGame ();
            game.Start ();

            Console.ReadKey ();
        }

    }

}