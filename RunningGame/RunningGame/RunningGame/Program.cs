using System;

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

        private static bool isSoundOn;

        public static bool IsSoundOn
        {
            get { return Program.isSoundOn; }
            set { Program.isSoundOn = value; }
        }

        private static bool isBgMusicOn;

        public static bool IsBgMusicOn
        {
            get { return Program.isBgMusicOn; }
            set { Program.isBgMusicOn = value; }
        }
    }
#endif
}

