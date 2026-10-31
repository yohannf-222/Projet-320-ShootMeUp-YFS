using ShootMeUp.Helpers;
using ShootMeUp.Model;
using System.Collections.Generic;

namespace ShootMeUp
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
            
            // Démarrage
            Application.Run(new GameSpace(new Player(Config.GAMESPACE_WIDTH / 2, Config.GAMESPACE_HEIGHT - 150), Obstacle.GenerateObstacles(5), new List<Ennemi>(new Ennemi[10])));
        }
    }
}