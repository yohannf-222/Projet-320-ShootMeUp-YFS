using Drones.Helpers;
using Drones.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drones.Model
{
    internal class Ennemi
    {
        static int totHp = Config.ENNEMI_HP;
        private double _x;                          // Position en X depuis la gauche de l'espace 
        private double _y;                          // Position en Y depuis le haut de l'espace 
        private double _hp;                            // Le nombre de projectiles qui peuvent encore être         
        public static readonly int width = 37 * Config.PIXEL_SIZE_MULTIPLYER;
        public static readonly int height = 38 * Config.PIXEL_SIZE_MULTIPLYER;

        public double Hp
        {
            get => _hp;
            set
            {
                if (Hp > Config.ENNEMI_HP)
                    _hp = Config.ENNEMI_HP;
                else if (Hp < 0)
                    _hp = 0;
                else
                    _hp = value;
            }
        }

        public double X { get => _x; set => _x = value; }
        public double Y { get => _y; set => _y = value; }

        public Ennemi(int x, int y)
        {
            this.X = x;
            this.Y = y;
            this._hp = Config.ENNEMI_HP;
        }
        #region ================ Modelisation de l'ennemi et de son comportement ================
        public void Update(int interval)
        {

        }

        public void GetHit(int damage)
        {
            Hp -= damage;
        }
        #endregion

        #region  ================ Rendu graphique  ================

        public void Render(BufferedGraphics drawingSpace)
        {                        
            drawingSpace.Graphics.DrawImage(Resources.PirateClark, Convert.ToSingle(X), Convert.ToSingle(Y), width, height);
        }
        #endregion
    }
}