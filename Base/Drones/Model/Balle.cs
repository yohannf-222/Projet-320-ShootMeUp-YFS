using ShootMeUp.Helpers;
using ShootMeUp.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootMeUp.Model
{
    public class Balle
    {
        private double _x;                              // Position en X depuis la gauche de l'espace 
        private double _y;                              // Position en Y depuis le haut de l'espace
        public double X { get => _x; set => _x = value; }
        public double Y { get => _y; set => _y = value; }
        public static readonly int width = 5 * Config.PIXEL_SIZE_MULTIPLYER;
        public static readonly int height = 9 * Config.PIXEL_SIZE_MULTIPLYER;

        /// <summary>
        /// Projectiles ennemis, part de l'ennemi et continuent d'avancer dans la direction donnée.
        /// </summary>
        public Balle(double x, double y)
        {
            X = x;
            Y = y;
        }

        #region ================ Modelisation des projectiles ennemis et de son comportement ================
        public void Update()
        {
            Y += Config.ENNEMI_BULLET_SPEED;
        }
        #endregion

        #region  ================ Rendu graphique  ================

        // Afficher le projectile
        public void Render(BufferedGraphics drawingSpace)
        {
            drawingSpace.Graphics.DrawImage(Resources.gouvernail, Convert.ToSingle(X), Convert.ToSingle(Y), width * Config.PIXEL_SIZE_MULTIPLYER, height * Config.PIXEL_SIZE_MULTIPLYER);
        }
        #endregion
    }
}
