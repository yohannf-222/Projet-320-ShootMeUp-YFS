using Drones.Helpers;
using Drones.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Drones.Model
{
    public class Player
    {

        private double _x;                          // Position en X depuis la gauche de l'espace 
        private double _y;                          // Position en Y depuis le haut de l'espace 
        private State _state;

        public enum State { ALIVE, DEAD, STOPPED};

        // Constructeur
        public Player(int x, int y)
        {
            this._x = x;
            this._y = y;           
            this._state = State.ALIVE;
        }

        #region ================ Modelisation du joueur et de son comportement ================
        public void Update(int interval)
        {
            _x++;            
        }
        #endregion

        #region  ================ Rendu graphique  ================

        private const int SIZE = 50;
        private Pen droneBrush = new Pen(new SolidBrush(Color.Purple), 3);

        // De manière graphique
        public void Render(BufferedGraphics drawingSpace)
        {
            drawingSpace.Graphics.DrawImage(Resources.player, Convert.ToSingle(_x) - 50, Convert.ToSingle(_y) - 50, 26 * 3, 43 * 3);
        }
        #endregion
    }
}
