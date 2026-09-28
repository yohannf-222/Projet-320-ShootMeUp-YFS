using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drones.Model
{
    internal class Obstacle
    {

        private double _x;                          // Position en X depuis la gauche de l'espace 
        private double _y;                          // Position en Y depuis le haut de l'espace 
        public int hitPoints { get; set; }
        public Obstacle(int x, int y)
        {
            this._x = x;
            this._y = y;
            this.hitPoints = 5;                     // Exemple de valeur initiale
        }
    }
}
