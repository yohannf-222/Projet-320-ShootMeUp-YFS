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

        /// <summary>
        /// Vérifie si 2 rectangles se touchent
        /// </summary>
        /// <param name="x1">position x du 1er rectangle, dans le coin supérieur gauche</param>
        /// <param name="y1">position y du 1er rectangle, dans le coin supérieur gauche</param>
        /// <param name="width1">largeur du 1er rectangle</param>
        /// <param name="height1">hauteur du 1er rectangle</param>
        /// <param name="x2">position x du 2nd rectangle, dans le coin supérieur gauche</param>
        /// <param name="y2">position y du 2nd rectangle, dans le coin supérieur gauche</param>
        /// <param name="width2">largeur du 2nd rectangle</param>
        /// <param name="height2">hauteur du 2nd rectangle</param>
        /// <returns></returns>
        public static bool IsTouching(double x1, double y1, int width1, int height1, double x2, double y2, int width2, int height2)
        {
            if (x1 > x2 && x1 < x2 + width2 &&
                y1 > y2 && y1 < y2 + height2
                || x2 > x1 && x2 < x1 + width1 &&
                   y2 > y1 && y2 < y1 + height1)
                return true;
            return false;
        }

        /// <summary>
        /// Vérifie si un rectangle et un cercle se touchent
        /// </summary>
        /// <param name="x1">position x du 1er rectangle, dans le coin supérieur gauche</param>
        /// <param name="y1">position y du 1er rectangle, dans le coin supérieur gauche</param>
        /// <param name="width1">largeur du 1er rectangle</param>
        /// <param name="height1">hauteur du 1er rectangle</param>
        /// <param name="x2">position x du 2nd rectangle, dans le coin supérieur gauche</param>
        /// <param name="y2">position y du 2nd rectangle, dans le coin supérieur gauche</param>
        /// <param name="width2">largeur du 2nd rectangle</param>
        /// <param name="height2">hauteur du 2nd rectangle</param>
        /// <returns></returns>
        public static bool IsTouching(double x1, double y1, int width1, int height1, double x2, double y2, int radius2)
        {
            if (x1 > x2 && x1 < x2 + radius2 &&
                y1 > y2 && y1 < y2 + radius2 ||
                x2 > x1 && x2 - radius2 < x1 + width1 &&
                   y2 > y1 && y2 - radius2 < y1 + height1)
                return true;
            return false;
        }

    }
}