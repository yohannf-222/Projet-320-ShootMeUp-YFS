using ShootMeUp.Helpers;
using ShootMeUp.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootMeUp
{
    public class Gouvernail
    {

        static int SPEED = 50;                        // Vitesse des projectiles amis, en px par frame
        public static readonly int COOLDOWN = 10;     // Interval minimum entre les tirs, en nombre de ticks
        public static readonly int DAMAGE = 90;       // Dégats engendrés par un impact de gouvernail
        public static readonly int RADIUS = 42;       // Rayon d'un gouvernail, pour que la zone de collision soit ronde


        private double _x;                              // Position en X depuis la gauche de l'espace 
        private double _y;                              // Position en Y depuis le haut de l'espace
        private double _xIncrement;
        private double _yIncrement;
        public double X { get => _x; set => _x = value; }
        public double Y { get => _y; set => _y = value; }

        /// <summary>
        /// Projectiles du joueur, part du joueur et continuent d'avancer dans la direction donnée (souris).
        /// </summary>
        /// <param name="x">position x</param>
        /// <param name="y">position y</param>
        /// <param name="targetX">Position X de la souris quand on clique.</param>
        /// <param name="targetY">Position Y de la souris quand on clique.</param>
        public Gouvernail(double x, double y, double targetX, double targetY)
        {
            X = x;
            Y = y;

            // calculer l'incrément de déplacement x et y en fonction de la direction de la souris lors de l'instanciation du gouvernail.
            double deltaX = targetX - X;
            double deltaY = targetY - Y;
            double distance = MathHelpers.CalculateDistance(X, Y, targetX, targetY);
            this._xIncrement = deltaX / distance * SPEED;
            this._yIncrement = deltaY / distance * SPEED;
        }

        #region ================ Modelisation du gouvernail et de son comportement ================
        public void Update()
        {
            // Déplacer le gouvernail dans la direction calculée lors de son instanciation
            X += _xIncrement;
            Y += _yIncrement;
        }
        #endregion

        #region  ================ Rendu graphique  ================

        // Afficher le gouvernail, centré sur sa position X/Y
        public void Render(BufferedGraphics drawingSpace)
        {
            drawingSpace.Graphics.DrawImage(Resources.gouvernail, Convert.ToSingle(X) - RADIUS * Config.PIXEL_SIZE_MULTIPLYER / 2, Convert.ToSingle(Y) - RADIUS * Config.PIXEL_SIZE_MULTIPLYER / 2, RADIUS * Config.PIXEL_SIZE_MULTIPLYER, RADIUS * Config.PIXEL_SIZE_MULTIPLYER);
        }
        #endregion
    }
}
