using System.Text;
using utils;

namespace snake
{
    public class ConsoleGame {
        private List<GameObject> gameObjs = new List<GameObject> ();

        protected Dictionary<ConsoleKey, Action<ConsoleKey>> keyHandlers = new Dictionary<ConsoleKey, Action<ConsoleKey>> ();
        protected bool started = false;
        protected bool paused = false;

        public ConsoleGame (int width = 100, int height = 50, string title = "Console Game") {
            WindowUtility.RemoveWindowActions (
                WindowUtility.WINDOW_ACTIONS.MAXIMIZE,
                WindowUtility.WINDOW_ACTIONS.SIZE
            );
            Console.SetWindowSize (
                width > Console.LargestWindowWidth ? Console.LargestWindowWidth : width,
                height > Console.LargestWindowHeight ? Console.LargestWindowHeight : height
            );
            Console.SetBufferSize (
                width > Console.LargestWindowWidth ? Console.LargestWindowWidth : width,
                height > Console.LargestWindowHeight ? Console.LargestWindowHeight : height
            );
            WindowUtility.MoveWindowToCenter ();

            Console.OutputEncoding = Encoding.Unicode;
            Console.Title = title;
        }

        protected void AddObjects (params GameObject[] objs) {
            gameObjs.AddRange (objs);
        }

        public void Render () {
            foreach (GameObject obj in gameObjs) {
                obj.Render ();
            }
        }
        public void Clear () {
            foreach (GameObject obj in gameObjs.Where (obj => obj.mutable)) {
                obj.Clear ();
            }
            gameObjs = gameObjs.Where (obj => !obj.mutable).ToList ();
        }

        public virtual void Start () {
            throw new NotImplementedException ();
        }
        protected virtual void Loop () {
            throw new NotImplementedException ();
        }
        public virtual void Update () {
            throw new NotImplementedException ();
        }

        // key handlers
        protected virtual void StartLoop (ConsoleKey key) {
            if (!started)
                started = true;
        }
        protected virtual void Pause (ConsoleKey key) {
            paused = !paused;
        }
        protected void Exit (ConsoleKey key) {
            Environment.Exit (0);
        }
        protected virtual void Restart (ConsoleKey key) {
            started = false;
            paused = false;
        }

        protected void ReadKey (params ConsoleKey[] keys) {
            ConsoleKey key;
            if (keys.Length > 0) {
                while (true) {
                    key = Console.ReadKey (true).Key;
                    if (keys.Contains (key) && keyHandlers.ContainsKey (key)) {
                        keyHandlers[key] (key);
                        break;
                    }

                }
            } else {
                key = Console.ReadKey (true).Key;
                if (keyHandlers.ContainsKey (key)) {
                    keyHandlers[key] (key);
                }
            }
        }

        protected virtual void BindKeyHandler () {
            keyHandlers.Clear ();

            keyHandlers.Add (ConsoleKey.Spacebar, StartLoop);
            keyHandlers.Add (ConsoleKey.P, Pause);
            keyHandlers.Add (ConsoleKey.Q, Exit);
            keyHandlers.Add (ConsoleKey.R, Restart);
        }

    }
}