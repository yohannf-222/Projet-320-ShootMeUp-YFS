using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drones.Helpers
{
    internal static class RndValueHelpers
    {
        private static Random alea = new Random();

        /// <summary>
        /// Retourne une valeur aleatoire entre 0 compris et max non compris
        /// </summary>
        /// <param name="max">Valeur maximum non comprise</param>
        /// <returns></returns>
        public static int Next(int max) => alea.Next(max);

        /// <summary>
        /// Retourne une valeur aleatoire entre min compris et max non compris
        /// </summary>
        /// <param name="min">Valeur minimum comprise</param>
        /// <param name="max">Valeur maximum non comprise</param>
        /// <returns></returns>
        public static int Next(int min, int max) => alea.Next(min, max);

    }
}