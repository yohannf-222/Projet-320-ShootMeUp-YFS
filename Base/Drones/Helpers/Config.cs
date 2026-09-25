using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drones.Helpers
{
    internal class Config
    {
        public static int GAMESPACE_WIDTH = 1200;       // Dimensions du gamespace, X
        public static int GAMESPACE_HEIGHT = 600;       // Dimensions du gamespace, Y

        public static int SPEED = 10;                   
        public static int GOUVERNAIL_SPEED = 10;        // En px par frame

        public static int PIXEL_SIZE_MULTIPLYER = 3;    // Ratio de taille des sprites dans le jeu par rapport à leur nombre de pixel réel
        public static int GOUVERNAIL_COOLDOWN = 20;                // En nombre de frames

    }
}
