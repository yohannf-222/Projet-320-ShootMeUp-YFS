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
        private double _hp;
        private int _state;
        public static readonly int width = 37 * Config.PIXEL_SIZE_MULTIPLYER;
        public static readonly int height = 38 * Config.PIXEL_SIZE_MULTIPLYER;

        public double Hp
        {
            get => _hp;
            set
            {
                if (Hp > Config.OBSTACLE_HP)
                    _hp = Config.OBSTACLE_HP;
                else if (Hp < 0)
                    _hp = 0;
                else
                    _hp = value;
            }
        }
        public int State
        {
            get => _state;
            set
            {
                if (State < 0)
                    _state = 0;
                else
                    _state = value;
            }
        }

        public double X { get => _x; set => _x = value; }
        public double Y { get => _y; set => _y = value; }

        public Obstacle(int x, int y)
        {
            this.X = x;
            this.Y = y;
            this._hp = Config.OBSTACLE_HP;
        }
        #region ================ Modelisation de l'obstacle et de son comportement ================
        public void Update(int interval)
        {
            // Prendre les hp et trouver dans quel état l'obstacle est, chaque quart de vie augmente l'état
            State = Convert.ToInt16(Math.Round((double)Hp * 4 / totHp));
            //Régénération des obstacles
            if (Hp < Config.OBSTACLE_HP)
                Hp += 0.5;
        }

        public void GetHit(int damage)
        {
            Hp -= damage;
        }

        public static List<Obstacle> GenerateObstacles(int nb)
        {
            List<Obstacle> obstacles = new List<Obstacle>();
            for (int i = 0; i < nb; i++)
            {
                obstacles.Add(new Obstacle((Config.GAMESPACE_WIDTH - 100) / nb * (i + 1) - 50 - Ennemi.width / 2, Config.GAMESPACE_HEIGHT / 3 * 2));
            }
            return obstacles;
        }
        #endregion

        #region  ================ Rendu graphique  ================

        public void Render(BufferedGraphics drawingSpace)
        {
            Image obstacle = null;
            switch (State)
            {
                case 0:
                    obstacle = null;
                    break;
                case 1:
                    obstacle = Resources.Obstacle4;
                    break;
                case 2:
                    obstacle = Resources.Obstacle3;
                    break;
                case 3:
                    obstacle = Resources.Obstacle2;
                    break;
                case 4:
                    obstacle = Resources.Obstacle1;
                    break;
            }
            if (obstacle != null)
                drawingSpace.Graphics.DrawImage(obstacle, Convert.ToSingle(X), Convert.ToSingle(Y), width, height);
        }
        #endregion
    }
}
