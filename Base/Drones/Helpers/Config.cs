using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootMeUp.Helpers
{
    internal class Config
    {
        public static int GAMESPACE_WIDTH = 1440;       // Dimensions du gamespace, largeur
        public static int GAMESPACE_HEIGHT = 800;       // Dimensions du gamespace, hauteur   

        public static int PIXEL_SIZE_MULTIPLYER = 3;    // Ratio de taille des sprites dans le jeu par rapport à leur nombre de pixel lors du dessin

        public static int OBJECT_DELETION_MARGIN = 200; // Marge de suppression des objets hors écran, en px     

    }
}
