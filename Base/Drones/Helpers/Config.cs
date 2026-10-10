using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drones.Helpers
{
    internal class Config
    {
        public static int GAMESPACE_WIDTH = 1440;       // Dimensions du gamespace, largeur
        public static int GAMESPACE_HEIGHT = 800;       // Dimensions du gamespace, hauteur

        public static int SPEED = 10;                   // Vitesse du joueur, en px par frame
        public static int GOUVERNAIL_SPEED = 50;        // Vitesse des projectiles amis, en px par frame

        public static int PIXEL_SIZE_MULTIPLYER = 3;    // Ratio de taille des sprites dans le jeu par rapport à leur nombre de pixel lors du dessin
        public static int GOUVERNAIL_COOLDOWN = 10;     // Interval minimum entre les tirs, en nombre de ticks
        public static int GOUVERNAIL_DAMAGE = 90;       // Dégats engendrés par un impact de gouvernail
        public static int GOUVERNAIL_RADIUS = 42;       // Rayon d'un gouvernail, pour que la zone de collision soit ronde

        public static int OBSTACLE_HP = 250;            // Dégats que l'obstacle peut encaisser avant de disparaitre temporairement.
        public static int OBJECT_DELETION_MARGIN = 200; // Marge de suppression des objets hors écran, en px     

        public static int ENNEMI_HP = 250;              // Dégats que l'ennemi peut encaisser avant d'être éliminé.
        public static int ENNEMI_SPEED = 10;            // Vitesse des ennemis en px par frame
    }
}
