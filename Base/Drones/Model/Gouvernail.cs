using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drones.Model
{
    internal class Gouvernail
    {

        private double _x;                              // Position en X depuis la gauche de l'espace 
        private double _y;                              // Position en Y depuis le haut de l'espace

        private double _direction;                        // Angle de direction, entre 0 et 180

        public Gouvernail(double x, double y, double direction)
        {
            _x = x;
            _y = y;
            _direction = direction;
        }
    }
}
