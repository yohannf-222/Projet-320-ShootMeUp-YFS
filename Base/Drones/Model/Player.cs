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
        private State _state;                       // l'état du joueur
        private int speedMultiplyer = 1;            // Augmente la vitesse continuellement lorsque le joueur maintient le mouvement

        public enum State { ALIVE, DEAD, STOPPED };

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
            if (_state == State.ALIVE)      //si vivant
            {                                               
                double speed = Config.SPEED * speedMultiplyer;

                // bouger à droite ou gauche selon les touches pressées
                if (GameSpace.keysPressed.Contains('D'))
                {
                    if (_x < Config.GAMESPACE_WIDTH - speed)
                        _x += speed;
                    else {
                        _x = Config.GAMESPACE_WIDTH;
                        speedMultiplyer = 1;
                    }
                }

                if (GameSpace.keysPressed.Contains('A'))
                {
                    if (_x > speed)
                        _x -= speed;
                    else {
                        _x = 0;
                        speedMultiplyer = 1;
                    }
                }                             

                // Le joueur accélère tant qu'il maintient, redevient lent lorsqu'il arrête
                if (GameSpace.keysPressed.Contains('A') || GameSpace.keysPressed.Contains('D'))
                {
                    speedMultiplyer++;
                }
                else
                {
                    speedMultiplyer = 1;
                }                
            }
        }
        #endregion

        #region  ================ Rendu graphique  ================

        private const int SIZE = 50;
        private Pen droneBrush = new Pen(new SolidBrush(Color.Purple), 3);

        // De manière graphique
        public void Render(BufferedGraphics drawingSpace)
        {
            drawingSpace.Graphics.DrawImage(Resources.PirateClark, Convert.ToSingle(_x) - 50, Convert.ToSingle(_y) - 50, 26 * 3, 43 * 3);
        }
        #endregion
    }
}
