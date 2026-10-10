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

        private static List<int> axesX = new List<int> { Config.GAMESPACE_WIDTH / 6, Config.GAMESPACE_WIDTH / 2, Config.GAMESPACE_WIDTH / 6 * 5 };
        private static List<int> axesY = new List<int> { Config.GAMESPACE_HEIGHT / 5 * 1, Config.GAMESPACE_HEIGHT / 5 * 2 };

        // Points entre lesquels les ennemis se déplacent
        private static Location[,] objectifs =
        {
           {new Location(axesX[0], axesY[0]), new Location(axesX[1], axesY[0]), new Location(axesX[2], axesY[0])},
           {new Location(axesX[0], axesY[1]), new Location(axesX[1], axesY[1]), new Location(axesX[2], axesY[1])}
        };

        // Endroit où les ennemis peuvent apparaitre, en dehors de l'écran
        private static Location[,] spawners =
        {
            {new Location(-width, axesY[0]), new Location(Config.GAMESPACE_WIDTH + width ,axesY[1])},
            {new Location(-width, axesY[1]), new Location(Config.GAMESPACE_WIDTH + width ,axesY[1])}
        };

        public double Hp
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
            this.Position = spawners[RndValueHelpers.Next(0, 2), RndValueHelpers.Next(0, 2)];
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

            if (objectif.X > Position.X)
                Position.X += speed;
            else if (objectif.X < Position.X)
                Position.X -= speed;
            else if (objectif.Y > Position.Y)
                Position.Y += speed;
            else if (objectif.Y < Position.Y)
                Position.Y -= speed;
        }

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
            // Essayer 50 fois de choisir un objectif qui n'est pas en diagonale de la position actuelle.
            for (int i = 0; i < 50; i++)
            {
                int objX = RndValueHelpers.Next(0, 3);
                int objY = RndValueHelpers.Next(0, 2);
                Location obj = objectifs[RndValueHelpers.Next(0, 2), RndValueHelpers.Next(0, 3)];

                if (obj.Y != Position.Y && obj.X != Position.X)
                    continue;
                else
                    return objectifs[objY, objX];
            }
            // si échoué après 50 essais, retourner le premier objectif par défaut, afin d'éviter une boucle infinie.
            return objectifs[0, 0];
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