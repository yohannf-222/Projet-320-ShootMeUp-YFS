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
            List<Obstacle> obstacles = new List<Obstacle>();
            obstacles.Add(new Obstacle(200, 350));
            obstacles.Add(new Obstacle(400, 350));
            obstacles.Add(new Obstacle(600, 350));
            obstacles.Add(new Obstacle(800, 350));
            obstacles.Add(new Obstacle(1000, 350));

            // Démarrage
            Application.Run(new GameSpace(player, gouvernails, obstacles));
        }
    }
}