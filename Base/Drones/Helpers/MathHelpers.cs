using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drones.Helpers
{
    internal class MathHelpers
    {
        /// <summary>
        /// Calcule une distance entre 2 points sur un système de coordonées
        /// </summary>
        /// <param name="x1">Coordonée x du 1er point</param>
        /// <param name="y1">Coordonée y du 1er point</param>
        /// <param name="x2">Coordonée x du 2nd point</param>
        /// <param name="y2">Coordonée y du 2nd point</param>
        /// <returns></returns>
        public static double CalculateDistance(double x1, double y1, double x2, double y2)
        {
            double deltaX = x2 - x1;
            double deltaY = y2 - y1;
            return Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
        }                 
        
    }
}
