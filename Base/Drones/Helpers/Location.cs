using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drones.Helpers
{
    /// <summary>
    /// Un objet contenant des coordonées X et Y
    /// </summary>
    public class Location
    {
        private double x;
        private double y;

        public double X { get => x; set => x = value; }
        public double Y { get => y; set => y = value; }

        public Location(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
