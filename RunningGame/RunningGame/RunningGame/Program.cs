using System;
using Microsoft.Xna.Framework;

namespace RunningGame
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (Game1 game = new Game1())
            {
                game.Run();
            }
        }

        private static bool isSoundOn = true;
        private static bool currentGameStatus = true;
        private static Vector2 currentPlatVelo = new Vector2(-2.5f, 0);
        public static bool IsSoundOn
        {
            get { return Program.isSoundOn; }
            set { Program.isSoundOn = value; }
        }

        private static bool isBgMusicOn = true;
        private static bool isTestMode;
        public static bool IsBgMusicOn
        {
            get { return Program.isBgMusicOn; }
            set { Program.isBgMusicOn = value; }
        }

        public static float jumpHeight
        {
            get { return 80; }
        }

        public static float firstJumpHeight
        {
            get { return 280; }
        }

        public static bool GameStatus
        {
            get { return currentGameStatus; }
            set { currentGameStatus = value; }
        }

        public static Vector2 PlatVelo
        {
            get { return currentPlatVelo; }
            set { currentPlatVelo = value; }
        }

        public static bool IsTestMode
        {
            get { return isTestMode; }
            set { isTestMode = value; }
        }
    }
#endif
}

