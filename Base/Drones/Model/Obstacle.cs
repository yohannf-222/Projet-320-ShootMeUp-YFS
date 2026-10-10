using Drones.Helpers;
using Drones.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Drones.Model
{
    public class Obstacle
    {
        static int totHp = Config.OBSTACLE_HP;      // Nombre de Hp maximum d'un obstacle
        private double _x;                          // Position en X depuis la gauche de l'espace 
        private double _y;                          // Position en Y depuis le haut de l'espace 
        private double _hp;                         // Les points de vie d'un objet obstacle
        private int _state;                         // Les 4 états de l'obstacle correspondent à un quart des Hp totaux et changent également son apparence
        public static readonly int width = 37 * Config.PIXEL_SIZE_MULTIPLYER;   // Dimension d'un obstacle, largeur
        public static readonly int height = 38 * Config.PIXEL_SIZE_MULTIPLYER;  // Dimension d'un obstacle, hauteur

        private double Hp
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
            // Chaque changement de Hp peut changer l'état, chaque quart de totHp = plus 1 dans state
            State = Convert.ToInt16(Math.Round((double)Hp * 4 / totHp));
            
            //Régénération des obstacles dans le temps
            if (Hp < Config.OBSTACLE_HP)
                Hp += 0.5;
        }

        /// <summary>
        /// Ordonner à l'obstacle de décrémenter sa vie de <paramref name="damage"/> points afin de garder Hp privé
        /// </summary>
        /// <param name="damage">Le nombre de dégats à décrémenter</param>
        public void GetHit(int damage)
        {
            Hp -= damage;
        }

        /// <summary>
        /// Crée une liste d'obstacles
        /// </summary>
        /// <param name="nb"></param>
        /// <returns></returns>
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
