using Drones.Helpers;
using Drones.Model;

namespace Drones
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            Player player = new Player(Config.GAMESPACE_WIDTH / 2, Config.GAMESPACE_HEIGHT - 150);

            List<Gouvernail> gouvernails = new List<Gouvernail>();            

            // Démarrage
            Application.Run(new GameSpace(player, gouvernails));
        }
    }
}