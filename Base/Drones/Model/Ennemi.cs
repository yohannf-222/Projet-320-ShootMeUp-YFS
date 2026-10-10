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
        private Location _position;                                             // Position de l'ennemi
        private double _hp;
        public static readonly int width = 14 * Config.PIXEL_SIZE_MULTIPLYER;   // Largeur de l'ennemi
        public static readonly int height = 30 * Config.PIXEL_SIZE_MULTIPLYER;  // Hauteur de l'ennemi
        private Location objectif;
        private static int speed = Config.ENNEMI_SPEED;

        // Points entre lesquels les ennemis se déplacent
        private static Location[,] objectifs =
        {
           {new Location(Config.GAMESPACE_WIDTH / 6, Config.GAMESPACE_HEIGHT/5 * 1), new Location(Config.GAMESPACE_WIDTH/2,Config.GAMESPACE_HEIGHT/5 * 1), new Location(Config.GAMESPACE_WIDTH/ 6 * 5,Config.GAMESPACE_HEIGHT/5 * 1)},
           {new Location(Config.GAMESPACE_WIDTH / 6, Config.GAMESPACE_HEIGHT/5 * 2), new Location(Config.GAMESPACE_WIDTH/2,Config.GAMESPACE_HEIGHT/5 * 2), new Location(Config.GAMESPACE_WIDTH/ 6 * 5,Config.GAMESPACE_HEIGHT/5 * 2)}
        };

        // Endroit où les ennemis peuvent apparaitre, en dehors de l'écran
        private static Location[,] spawners =
        {
            {new Location(-width, Config.GAMESPACE_HEIGHT/5 * 1), new Location(Config.GAMESPACE_WIDTH + width ,Config.GAMESPACE_HEIGHT/5 * 1)},
            {new Location(-width, Config.GAMESPACE_HEIGHT/5 * 2), new Location(Config.GAMESPACE_WIDTH + width ,Config.GAMESPACE_HEIGHT/5 * 2)}
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

        public Location Position { get => _position; set => _position = value; }

        public Ennemi()
        {
            //apparition aléatoire dans l'un des spawners
            this.Position = spawners[RndValueHelpers.Next(0, 2), RndValueHelpers.Next(0, 2)];
            this._hp = Config.ENNEMI_HP;
        }


        #region ================ Modelisation de l'ennemi et de son comportement ================
        public void Update(int interval)
        {
            // Si pas d'objectif, choisir un objectif
            if (objectif == null)
            {
                objectif = ChooseObjective(objectifs, Position);
            }

            // Si on est assez proche de l'objectif, on arrondis X et Y pour être dessus
            if (MathHelpers.CalculateDistance(Position.X, Position.Y, objectif.X, objectif.Y) <= speed)
            {
                Position = objectif;
                // Choisir un nouvel objectif
                objectif = ChooseObjective(objectifs, Position);
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
        private Location ChooseObjective(Location[,] objectifs, Location position)
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