using Drones.Helpers;
using Drones.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drones.Model
{
    public class Gouvernail
    {
        private double _x;                              // Position en_xdepuis la gauche de l'espace 
        private double _y;                              // Position en Y depuis le haut de l'espace
        private double _xIncrement;
        private double _yIncrement;
        public double X { get => _x; set => _x = value; }
        public double Y { get => _y; set => _y = value; }

        /// <summary>
        /// Projectiles du joueur, qui se déplacent vers la position de la souris quand on clique
        /// </summary>
        /// <param name="x">position x</param>
        /// <param name="y">position y</param>
        /// <param name="targetX">Position X de la souris quand on clique.</param>
        /// <param name="targetY">Position Y de la souris quand on clique.</param>
        public Gouvernail(double x, double y, double targetX, double targetY)
        {
            X = x;
            Y = y;

            double deltaX = targetX - X;
            double deltaY = targetY - Y;
            double distance = MathHelpers.CalculateDistance(X, Y, targetX, targetY);
            this._xIncrement = deltaX / distance * Config.GOUVERNAIL_SPEED;
            this._yIncrement = deltaY / distance * Config.GOUVERNAIL_SPEED;
        }

        #region ================ Modelisation du gouvernail et de son comportement ================
        public void Update()
        {
            X += _xIncrement;
            Y += _yIncrement;
        }
        #endregion

        #region  ================ Rendu graphique  ================

        // De manière graphique
        public void Render(BufferedGraphics drawingSpace)
        {
            drawingSpace.Graphics.DrawImage(Resources.gouvernail, Convert.ToSingle(X) - Config.GOUVERNAIL_RADIUS * Config.PIXEL_SIZE_MULTIPLYER / 2, Convert.ToSingle(Y) - Config.GOUVERNAIL_RADIUS * Config.PIXEL_SIZE_MULTIPLYER / 2, 42 * Config.PIXEL_SIZE_MULTIPLYER, 42 * Config.PIXEL_SIZE_MULTIPLYER);
        }
        #endregion
    }
}
