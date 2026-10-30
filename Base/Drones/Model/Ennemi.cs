using ShootMeUp.Model;
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
        private Location _position = new Location();                            // Position de l'ennemi
        private double _hp;
        public static readonly int width = 14 * Config.PIXEL_SIZE_MULTIPLYER;   // Largeur de l'ennemi
        public static readonly int height = 30 * Config.PIXEL_SIZE_MULTIPLYER;  // Hauteur de l'ennemi
        public int counter = RndValueHelpers.Next(0, 40);
        public Location _objectif;
        private static int count = 0;
        private int _id;

        public static readonly int TOT_HP = 1;              // Dégats que l'ennemi peut encaisser avant d'être éliminé.
        public static readonly int SPEED = 10;              // Vitesse des ennemis en px par frame
        public static readonly int BULLET_SPEED = 50;       // Vitesse des projectiles ennemis en px par frame
        public static readonly int DAMAGE = 50;             // Dégats infligés par un projectile ennemi

        // Axes sur lesquels les ennemis peuvent se déplacer
        // axesX : positions X possibles (droites verticales)
        // axesY : positions Y possibles (droites horizontales)
        private static List<int> axesX = new List<int> { Config.GAMESPACE_WIDTH / 10, Config.GAMESPACE_WIDTH / 2, Config.GAMESPACE_WIDTH / 10 * 9 };
        private static List<int> axesY = new List<int> { Config.GAMESPACE_HEIGHT / 7 * 1, Config.GAMESPACE_HEIGHT / 7 * 2, Config.GAMESPACE_HEIGHT / 7 * 3 };

        // Points entre lesquels les ennemis se déplacent
        public static Location[,] objectifs = MathHelpers.DefineIntersections(axesX, axesY);

        // Endroit où les ennemis peuvent apparaitre, en dehors de l'écran
        private static List<Location> spawners = DefineSpawners();

        public double Hp
        {
            get => _hp;
            set => _hp = Math.Clamp(value, 0, TOT_HP);
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
            this._hp = TOT_HP;
            this._objectif = ChooseObjective();
            count++;
            this._id = count;
        }


        #region ================ Modelisation de l'ennemi et de son comportement ================
        public void Update(int interval)
        {
            if (_objectif == null || MathHelpers.CalculateDistance(Position.X, Position.Y, _objectif.X, _objectif.Y) <= SPEED)
                _objectif = ChooseObjective();

            // Si on est assez proche de l'objectif, on arrondis X et Y pour être dessus
            if (Math.Abs(Position.X - _objectif.X) <= SPEED)
            {
                Position.X = _objectif.X;
            }

            if (Math.Abs(Position.Y - _objectif.Y) <= SPEED)
            {
                Position.Y = _objectif.Y;
            }

            if (_objectif.X != Position.X)
            {
                if (_objectif.X > Position.X)
                    Position.X += SPEED;
                else if (_objectif.X < Position.X)
                    Position.X -= SPEED;
            }
            else if (_objectif.Y != Position.Y)
            {
                if (_objectif.Y > Position.Y)
                    Position.Y += SPEED;
                else if (_objectif.Y < Position.Y)
                    Position.Y -= SPEED;
            }

        }

        /// <summary>
        /// Choisir un objectif aléatoire parmi les objectifs donnés, sur les points prédéfinis.
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
                int closestYIndex = axesY.IndexOf(MathHelpers.CloseValueIn(Convert.ToInt16(Math.Round(Position.Y)), axesY));
                return objectifs[objX, closestYIndex];
            }
            else
            {
                // Choisir un objectif sur la même ligne verticale
                int objY = RndValueHelpers.Next(0, axesY.Count);
                int closestXIndex = axesX.IndexOf(MathHelpers.CloseValueIn(Convert.ToInt16(Math.Round(Position.X)), axesX));
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

            for (int i = 0; i < axesX.Count; i++)
            {
                spawners.Add(new Location(axesX[i], -height));
            }

            for (int i = 0; i < axesY.Count; i++)
            {
                spawners.Add(new Location(-width, axesY[i]));
                spawners.Add(new Location(Config.GAMESPACE_WIDTH + width, axesY[i]));
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
            int closestX = MathHelpers.CloseValueIn(Convert.ToInt16(Math.Round(value.X)), axesX);
            int closestY = MathHelpers.CloseValueIn(Convert.ToInt16(Math.Round(value.Y)), axesY);

            // Calculer la différence pour savoir laquelle est la plus proche de l'axe
            double deltaX = Math.Abs(value.X - closestX);
            double deltaY = Math.Abs(value.Y - closestY);

            // Retourner la position la plus proche
            if (deltaX < deltaY)
                return new Location(closestX, Convert.ToInt16(Math.Round(value.Y)));
            else
                return new Location(Convert.ToInt16(Math.Round(value.X)), closestY);
        }

        #region  ================ Rendu graphique  ================

        // Afficher l'ennemi
        public void Render(BufferedGraphics drawingSpace)
        {
            drawingSpace.Graphics.DrawImage(Resources.Ennemi, Convert.ToSingle(Position.X), Convert.ToSingle(Position.Y), width, height);
            
        }
        #endregion
    }
}