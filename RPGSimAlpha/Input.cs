using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Input;
using OpenTK;

namespace RPGSimAlpha
{
    static class Input
    {
        public static List<Key> KeysDown { get;private set; }
        public static List<Key> KeysDownLast { get;private set; }
        private static List<MouseButton> ButtonsDown { get; set; }
        private static List<MouseButton> ButtonsDownLast { get; set; }
        public static void Initialize(GameWindow game)
        {
            KeysDown = new List<Key>();
            KeysDownLast = new List<Key>();
            ButtonsDown = new List<MouseButton>();
            ButtonsDownLast = new List<MouseButton>();

            game.KeyDown += Game_KeyDown;
            game.KeyUp += Game_KeyUp;
            game.MouseDown += Game_MouseDown;
            game.MouseUp += Game_MouseUp;
        }

        private static void Game_MouseUp(object sender, MouseButtonEventArgs e)
        {
            while (ButtonsDown.Contains(e.Button))
                ButtonsDown.Remove(e.Button);
        }

        private static void Game_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ButtonsDown.Add(e.Button);
        }

        private static void Game_KeyUp(object sender, KeyboardKeyEventArgs e)
        {
            while (KeysDown.Contains(e.Key))
                KeysDown.Remove(e.Key);
        }

        private static void Game_KeyDown(object sender, KeyboardKeyEventArgs e)
        {
            KeysDown.Add(e.Key);
        }
        public static void Update()
        {
            KeysDownLast = new List<Key>(KeysDown);
            ButtonsDownLast = new List<MouseButton>(ButtonsDown);
        }
        /// <summary>
        /// down last tick and this tick
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool KeyPress(Key key) => (KeysDown.Contains(key) && KeysDownLast.Contains(key));
        /// <summary>
        /// not down last tick but down this tick
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool KeyRelease(Key key) => (!KeysDown.Contains(key) && KeysDownLast.Contains(key));
        /// <summary>
        /// down this tick
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool KeyDown(Key key) => KeysDown.Contains(key);

        public static bool MousePress(MouseButton button) => (ButtonsDown.Contains(button) && ButtonsDownLast.Contains(button));
        public static bool MouseRelease(MouseButton button) => (!ButtonsDown.Contains(button) && ButtonsDownLast.Contains(button));
        public static bool MouseDown(MouseButton button) => ButtonsDown.Contains(button);
        public static sbyte[] GetKeyInputVector2() => GetKeyInputVector2(KeysDown);
        public static sbyte[] GetKeyInputVector2(List<Key> keys)
        {
            sbyte[] vector = new sbyte[2];
            for(int i = 0;i<KeysInUseRegistry.GetLength(0);i++)
            {
                for(byte j = 0; j < KeysInUseRegistry.GetLength(1); j++)
                {
                    if (keys.Contains(KeysInUseRegistry[i,j]))
                    {
                        switch(GetAction(KeysInUseRegistry[i, j]))
                        {
                            case Action.MoveUp:
                                vector[1]--;
                                break;
                            case Action.MoveDown:
                                vector[1]++;
                                break;
                            case Action.MoveRight:
                                vector[0]++;
                                break;
                            case Action.MoveLeft:
                                vector[0]--;
                                break;
                        }
                        continue;
                    }
                }
            }
            return vector;
        }
        public static Action GetAction(OpenTK.Input.Key e)
        {
            switch (e)
            {
                case OpenTK.Input.Key.W:
                    return Action.MoveUp;
                case OpenTK.Input.Key.S:
                    return Action.MoveDown;
                case OpenTK.Input.Key.A:
                    return Action.MoveLeft;
                case OpenTK.Input.Key.D:
                    return Action.MoveRight;

                case OpenTK.Input.Key.Up:
                    return Action.MoveUp;
                case OpenTK.Input.Key.Down:
                    return Action.MoveDown;
                case OpenTK.Input.Key.Left:
                    return Action.MoveLeft;
                case OpenTK.Input.Key.Right:

                    return Action.MoveRight;
                case OpenTK.Input.Key.F:
                    return Action.Fire;
                default:
                    return Action.None;
            }
        }
        /// <summary>
        /// [action,variation]
        /// </summary>
        public static OpenTK.Input.Key[,] KeysInUseRegistry { get; } = new OpenTK.Input.Key[5, 2]
        {
            { OpenTK.Input.Key.W,OpenTK.Input.Key.Up },
            { OpenTK.Input.Key.S,OpenTK.Input.Key.Down },
            { OpenTK.Input.Key.A,OpenTK.Input.Key.Left },
            { OpenTK.Input.Key.D,OpenTK.Input.Key.Right },

            { OpenTK.Input.Key.F,OpenTK.Input.Key.ControlLeft },
        };
        public enum Action : byte
        {
            MoveUp = 0,
            MoveDown,
            MoveLeft,
            MoveRight,
            Fire,
            None,
        }
    }
}
