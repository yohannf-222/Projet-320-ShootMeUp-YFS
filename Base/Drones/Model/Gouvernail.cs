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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="targetX">Position X de la souris quand on clique.</param>
        /// <param name="targetY">Position Y de la souris quand on clique.</param>
        public Gouvernail(double x, double y, double targetX, double targetY)
        {
            _x = x;
            _y = y;

            double deltaX = targetX - _x;
            double deltaY = targetY - _y;
            double distance = MathHelpers.CalculateDistance(_x, _y, targetX, targetY);
            this._xIncrement = deltaX / distance * Config.GOUVERNAIL_SPEED;
            this._yIncrement = deltaY / distance * Config.GOUVERNAIL_SPEED;
        }

        #region ================ Modelisation du joueur et de son comportement ================
        public void Update()
        {
            _x += _xIncrement;
            _y += _yIncrement;
        }
        #endregion

        #region  ================ Rendu graphique  ================

        // De manière graphique
        public void Render(BufferedGraphics drawingSpace)
        {
            drawingSpace.Graphics.DrawImage(Resources.gouvernail, Convert.ToSingle(_x) - Config.GOUVERNAIL_RADIUS, Convert.ToSingle(_y) - Config.GOUVERNAIL_RADIUS, 42 * Config.PIXEL_SIZE_MULTIPLYER, 42 * Config.PIXEL_SIZE_MULTIPLYER);
        }
        #endregion
    }
}
