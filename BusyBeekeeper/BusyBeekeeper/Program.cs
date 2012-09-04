using System;

namespace BusyBeekeeper
{
#if WINDOWS || XBOX
    /// <summary>
    /// Main entry point for the game.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// <param name="args">The command-line arguments for the program.</param>
        public static void Main(string[] args)
        {
            using (BusyBeekeeperGame game = new BusyBeekeeperGame())
            {
                game.Run();
            }
        }
    }
#endif
}