using ShootMeUp.Helpers;
using ShootMeUp.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ShootMeUp.Model
{
    public class Ennemi
    {
        static int totHp = Config.ENNEMI_HP;                                    // Points de vie
        private Location _position = new Location();                            // Position de l'ennemi
        private double _hp;
        public static readonly int width = 14 * Config.PIXEL_SIZE_MULTIPLYER;   // Largeur de l'ennemi
        public static readonly int height = 30 * Config.PIXEL_SIZE_MULTIPLYER;  // Hauteur de l'ennemi
        private Location _objectif;
        private static int speed = Config.ENNEMI_SPEED;

        private static int count = 0;
        private int _id;

        // Axes sur lesquels les ennemis peuvent se déplacer
        // axesX : positions X possibles (droites verticales)
        // axesY : positions Y possibles (droites horizontales)
        private static List<int> axesX = new List<int> { Config.GAMESPACE_WIDTH / 6, Config.GAMESPACE_WIDTH / 2, Config.GAMESPACE_WIDTH / 6 * 5 };
        private static List<int> axesY = new List<int> { Config.GAMESPACE_HEIGHT / 7 * 1, Config.GAMESPACE_HEIGHT / 7 * 2, Config.GAMESPACE_HEIGHT / 7 * 3 };

        // Points entre lesquels les ennemis se déplacent
        private static Location[,] objectifs = MathHelpers.DefineIntersections(axesX, axesY);

        // Endroit où les ennemis peuvent apparaitre, en dehors de l'écran
        private static List<Location> spawners = DefineSpawners();

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
                // vérifier si x et y sont sur des axes valides
                bool xOnAxis = axesX.Contains(Convert.ToInt16(Math.Round(value.X)));
                bool yOnAxis = axesY.Contains(Convert.ToInt16(Math.Round(value.Y)));

                // si x et y ne sont pas sur des axes valides, trouver la position valide la plus proche
                if (!xOnAxis && !yOnAxis)
                {       
                   _position = ClosestValidLocation(value);
                }            
                // sinon, si seul x OU y est hors axes, on accepte le changement
                else 
                    _position.X = value.X;

            }
        }

        public Ennemi()
        {
            //apparition aléatoire dans l'un des spawners
            this._position = spawners[RndValueHelpers.Next(0, spawners.Count)];
            this._hp = Config.ENNEMI_HP;
            this._objectif = ChooseObjective();
            count++;
            this._id = count;
        }


        #region ================ Modelisation de l'ennemi et de son comportement ================
        public void Update(int interval)
        {
            if (_objectif == null || MathHelpers.CalculateDistance(Position.X, Position.Y, _objectif.X, _objectif.Y) <= speed)
                _objectif = ChooseObjective();

            // Si on est assez proche de l'objectif, on arrondis X et Y pour être dessus
            if (Math.Abs(Position.X - _objectif.X) <= speed)
            {
                Position.X = _objectif.X;
            }
            
            if (Math.Abs(Position.Y - _objectif.Y) <= speed)
            {
                Position.Y = _objectif.Y;
            }

            if (_objectif.X != Position.X)
            {
                if (_objectif.X > Position.X)
                    Position.X += speed;
                else if (_objectif.X < Position.X)
                    Position.X -= speed;
            }
            else if (_objectif.Y != Position.Y)
            {
                if (_objectif.Y > Position.Y)
                    Position.Y += speed;
                else if (_objectif.Y < Position.Y)
                    Position.Y -= speed;
            }
        }

        /// <summary>
        /// Ordonner à l'ennemi de prendre des dégats, afin de garder Hp privé.
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

        /// <summary>
        /// Crée une liste de spawners pour les ennemis, en dehors de l'écran, sur les axes définis, en haut et sur les côtés.
        /// </summary>
        /// <returns>Une liste de Location où les ennemis peuvent apparaître</returns>
        private static List<Location> DefineSpawners()
        {
            List<Location> spawners = new List<Location>();

            foreach (int x in axesX)
            {
                spawners.Add(new Location { X = x, Y = -height });
            }

            foreach (int y in axesY)
            {
                spawners.Add(new Location { X = -width, Y = y });
                spawners.Add(new Location { X = Config.GAMESPACE_WIDTH + width, Y = y });
            }

            return spawners;
        }

        /// <summary>
        /// Prends une position et retourne la position la plus proche sur les axes définis.
        /// </summary>
        /// <param name="value">La position à rapprocher des axes</param>
        /// <returns>La position la plus proche sur les axes définis</returns>
        private static Location ClosestValidLocation(Location value)
        {
            // Trouver la valeur la plus proche pour X et Y
            int closestX = MathHelpers.ClosestValue(Convert.ToInt16(Math.Round(value.X)), axesX);
            int closestY = MathHelpers.ClosestValue(Convert.ToInt16(Math.Round(value.Y)), axesY);

            // Calculer la différence pour savoir laquelle est la plus proche de l'axe
            double deltaX = Math.Abs(value.X - closestX);
            double deltaY = Math.Abs(value.Y - closestY);

            // Retourner la position la plus proche
            if (deltaX < deltaY)
                return new Location { X = closestX, Y = value.Y };
            else
                return new Location { X = value.X, Y = closestY };
        }

        #region  ================ Rendu graphique  ================

        // Afficher l'ennemi
        public void Render(BufferedGraphics drawingSpace)
        {
            drawingSpace.Graphics.DrawImage(Resources.Ennemi, Convert.ToSingle(Position.X), Convert.ToSingle(Position.Y), width, height);
            drawingSpace.Graphics.DrawRectangle(Pens.Red, Convert.ToSingle(_objectif.X), Convert.ToSingle(_objectif.Y), width, height);
            drawingSpace.Graphics.DrawString($"{_id}", TextHelpers.drawFont, TextHelpers.writingBrush, Convert.ToSingle(Position.X), Convert.ToSingle(Position.Y));
            drawingSpace.Graphics.DrawString($"{_id}", TextHelpers.drawFont, TextHelpers.writingBrush, Convert.ToSingle(_objectif.X), Convert.ToSingle(_objectif.Y));
        }
        #endregion
    }
}