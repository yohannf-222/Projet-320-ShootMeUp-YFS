using ShootMeUp.Helpers;
using ShootMeUp.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootMeUp.Model
{
    public class Ennemi
    {
        static int totHp = Config.ENNEMI_HP;                                    // Points de vie
        private Location _position = new Location();                            // Position de l'ennemi
        private double _hp;
        public static readonly int width = 14 * Config.PIXEL_SIZE_MULTIPLYER;   // Largeur de l'ennemi
        public static readonly int height = 30 * Config.PIXEL_SIZE_MULTIPLYER;  // Hauteur de l'ennemi
        private Location objectif;
        private static int speed = Config.ENNEMI_SPEED;

        // Axes sur lesquels les ennemis peuvent se déplacer
        // axesX : positions X possibles (droites verticales)
        // axesY : positions Y possibles (droites horizontales)
        private static List<int> axesX = new List<int> { Config.GAMESPACE_WIDTH / 6, Config.GAMESPACE_WIDTH / 2, Config.GAMESPACE_WIDTH / 6 * 5 };
        private static List<int> axesY = new List<int> { Config.GAMESPACE_HEIGHT / 6 * 1, Config.GAMESPACE_HEIGHT / 6 * 2, Config.GAMESPACE_HEIGHT / 6 * 3 };

        // Points entre lesquels les ennemis se déplacent
        private static Location[,] objectifs = MathHelpers.DefineIntersections(axesX, axesY);

        // Endroit où les ennemis peuvent apparaitre, en dehors de l'écran
        private static Location[,] spawners = MathHelpers.DefineIntersections(new List<int> { -width, Config.GAMESPACE_WIDTH + width }, axesY);

        private double Hp
        {
            get => _hp;
            set
            {
                // Empêcher les Hp de sortir des limites
                if (Hp > Config.ENNEMI_HP)
                    _hp = Config.ENNEMI_HP;
                else if (Hp < 0)
                    _hp = 0;
                else
                    _hp = value;
            }
        }

        public Location Position
        {
            get => _position;
            set
            {
                // Empêcher les ennemis de sortir des axes définis
                if (axesX.Contains(Convert.ToInt16(Math.Round(value.X))))
                    _position = value;
                else
                    _position.X = MathHelpers.ClosestValue(Convert.ToInt16(Math.Round(value.X)), axesX);

                if (axesY.Contains(Convert.ToInt16(Math.Round(value.Y))))
                    _position = value;
                else
                    _position.Y = MathHelpers.ClosestValue(Convert.ToInt16(Math.Round(value.Y)), axesY);
            }
        }

        public Ennemi()
        {
            //apparition aléatoire dans l'un des spawners
            this.Position = spawners[RndValueHelpers.Next(0, 2), RndValueHelpers.Next(0, axesY.Count - 1)];
            this._hp = Config.ENNEMI_HP;
        }


        #region ================ Modelisation de l'ennemi et de son comportement ================
        public void Update(int interval)
        {
            if (objectif == null || MathHelpers.CalculateDistance(Position.X, Position.Y, objectif.X, objectif.Y) <= speed)
                objectif = ChooseObjective();

            // Si on est assez proche de l'objectif, on arrondis X et Y pour être dessus
            if (Math.Abs(Position.X - objectif.X) <= speed)
            {
                Position.X = objectif.X;
            }
            else if (Math.Abs(Position.Y - objectif.Y) <= speed)
            {
                Position.Y = objectif.Y;
            }

            if (objectif.X != Position.X)
            {
                if (objectif.X > Position.X)
                    Position.X += speed;
                else if (objectif.X < Position.X)
                    Position.X -= speed;
            }
            else if (objectif.Y != Position.Y)
            {
                if (objectif.Y > Position.Y)
                    Position.Y += speed;
                else if (objectif.Y < Position.Y)
                    Position.Y -= speed;
            }
        }

        /// <summary>
        /// Ordonner à l'ennmi de prendre des dégats, afin de garder Hp privé.
        /// </summary>
        /// <param name="damage"></param>
        public void GetHit(int damage)
        {
            Hp -= damage;
        }

        /// <summary>
        /// Choisir un objectif aléatoire parmi les objectifs donnés, seulement dans des directions cardinales.
        /// </summary>
        /// <param name="objectifs">Les objectifs possibles</param>
        /// <param name="position">Position actuelle de l'ennemi</param>
        /// <returns>Objectif choisi</returns>
        private Location ChooseObjective()
        {
            bool direction = RndValueHelpers.Next(0, 2) == 0;
            if (direction)
            {
                // Choisir un objectif sur la même ligne horizontale
                int objX = RndValueHelpers.Next(0, axesX.Count);
                int closestYIndex = axesY.IndexOf(MathHelpers.ClosestValue(Convert.ToInt16(Math.Round(Position.Y)), axesY));
                return objectifs[objX, closestYIndex];
            }
            else
            {
                // Choisir un objectif sur la même ligne verticale
                int objY = RndValueHelpers.Next(0, axesY.Count);
                int closestXIndex = axesX.IndexOf(MathHelpers.ClosestValue(Convert.ToInt16(Math.Round(Position.X)), axesX));
                return objectifs[closestXIndex, objY];
            }
        }
        #endregion

        #region  ================ Rendu graphique  ================

        // Afficher l'ennemi
        public void Render(BufferedGraphics drawingSpace)
        {
            drawingSpace.Graphics.DrawImage(Resources.Ennemi, Convert.ToSingle(Position.X), Convert.ToSingle(Position.Y), width, height);
        }
        #endregion
    }
}