using Drones.Helpers;
using Drones.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drones.Model
{
    public class Obstacle
    {
        static int totHp = Config.OBSTACLE_HP;
        private double _x;                          // Position en X depuis la gauche de l'espace 
        private double _y;                          // Position en Y depuis le haut de l'espace 
        private int _hp;                            // Le nombre de projectiles qui peuvent encore être 
        private int _state;
        private int _width;
        private int _height;

        public int Hp
        {
            get => _hp;
            set{
                if (Hp > Config.OBSTACLE_HP)
                    Hp = Config.OBSTACLE_HP;
                else if (Hp < 0)
                    Hp = 0;
                else
                    _hp = value;
            }
        }
        public int State
        {
            get => _state;
            set{
                if (State < 0)
                    _state = 0;
                else
                    _state = value;
            }
        }

        public Obstacle(int x, int y)
        {
            this._x = x;
            this._y = y;
            this._hp = Config.OBSTACLE_HP;
        }
        #region ================ Modelisation du joueur et de son comportement ================
        public void Update(int interval)
        {
            // Prendre les hp et trouver dans quel état l'obstacle est, chaque quart de vie augmente l'état
            State = Convert.ToInt16(Math.Round((double)Hp * 4 / totHp));
            Console.WriteLine(State);
            //Régénération des obstacles
            //if (Hp < Config.OBSTACLE_HP)
            //    Hp++;
            Hp -= 2;

        }        

        private void GetHit(int damage)
        {
            Hp -= damage;
        }
        #endregion

        #region  ================ Rendu graphique  ================

        public void Render(BufferedGraphics drawingSpace)
        {
            switch (State)
            {
                case 0:
                    break;
                case 1:
                    drawingSpace.Graphics.DrawImage(Resources.Obstacle4, Convert.ToSingle(_x), Convert.ToSingle(_y), 37 * Config.PIXEL_SIZE_MULTIPLYER, 38 * Config.PIXEL_SIZE_MULTIPLYER);
                    break;
                case 2:
                    drawingSpace.Graphics.DrawImage(Resources.Obstacle3, Convert.ToSingle(_x), Convert.ToSingle(_y), 37 * Config.PIXEL_SIZE_MULTIPLYER, 38 * Config.PIXEL_SIZE_MULTIPLYER);
                    break;
                case 3:
                    drawingSpace.Graphics.DrawImage(Resources.Obstacle2, Convert.ToSingle(_x), Convert.ToSingle(_y), 37 * Config.PIXEL_SIZE_MULTIPLYER, 38 * Config.PIXEL_SIZE_MULTIPLYER);
                    break;
                case 4:
                default:
                    drawingSpace.Graphics.DrawImage(Resources.Obstacle1, Convert.ToSingle(_x), Convert.ToSingle(_y), 37 * Config.PIXEL_SIZE_MULTIPLYER, 38 * Config.PIXEL_SIZE_MULTIPLYER);
                    break;
            }
        }
        #endregion
    }
}
