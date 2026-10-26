using ShootMeUp.Helpers;
using ShootMeUp.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ShootMeUp.Model
{
    public class Player
    {

        private double _x;                                      // Position en X depuis la gauche de l'espace
        private double _y;                                      // Position en Y depuis le haut de l'espace
        private int _score;                                  // Score du joueur

        private static readonly int WIDTH = 26 * Config.PIXEL_SIZE_MULTIPLYER;          // Dimension du joueur, largeur
        private static readonly int HEIGHT = 43 * Config.PIXEL_SIZE_MULTIPLYER;         // Dimension du joueur, hauteur

        private State _state;                                   // l'état dans lequel le joueur est
        private int speedMultiplyer = 1;                        // Augmente la vitesse continuellement lorsque le joueur maintient le mouvement
        private int _cooldown = Config.GOUVERNAIL_COOLDOWN;     // Compteur de ticks depuis le dernier tir de gouvernail

        public enum State { ALIVE, DEAD, STOPPED };             // les états possibles dans lequel le joueur peut être

        private int _hp = Config.PLAYER_HP;

        public double X
        {
            get => _x;
            // Empêcher le joueur de sortir du jeu
            set
            {
                if (value > Config.GAMESPACE_WIDTH - WIDTH)
                    _x = Config.GAMESPACE_WIDTH - WIDTH;
                else if (value < 0)
                    _x = 0;
                else
                    _x = value;
            }
        }
        public double Y { get => _y; set => _y = value; }
        public int Hp
        {
            get => _hp;
            set
            {
                if (value > Config.PLAYER_HP)
                    _hp = Config.PLAYER_HP;
                else if (value <= 0)
                {
                    _state = State.DEAD;
                    _hp = value;
                }                    
                else
                    _hp = value;
            }
        }

        // Constructeur
        public Player(int x, int y)
        {
            this.X = x;
            this.Y = y;
            this._state = State.ALIVE;
        }

        #region ================ Modelisation du joueur et de son comportement ================
        public void Update(int interval, MouseEventArgs? mouse, ref List<Gouvernail> gouvernails)
        {
            // Ne faire le reste que si le joueur est vivant
            if (_state != State.ALIVE)
                return;

            _score++;

            // Au clic gauche, instancier un gouvernail au milieu du personnage
            if (mouse != null && _cooldown >= Config.GOUVERNAIL_COOLDOWN && Convert.ToString(mouse.Button) == "Left")
            {
                gouvernails.Add(new Gouvernail(X + WIDTH / 2, Y + WIDTH / 2, mouse.X, mouse.Y));
                _cooldown = 0;
            }
            _cooldown++;

            #region ============= Mouvements ===========

            // Facteur d'accélération du joueur lorsqu'il avance en ligne droite
            double speed = Config.SPEED * speedMultiplyer;

            // Bouger à droite ou gauche selon les touches pressées
            if (GameSpace.keysPressed.Contains('D'))
            {
                if (X < Config.GAMESPACE_WIDTH - speed - WIDTH)
                    X += speed;
                else
                {
                    X = Config.GAMESPACE_WIDTH - WIDTH;
                    speedMultiplyer = 1;
                }
            }
            if (GameSpace.keysPressed.Contains('A'))
            {
                if (X > speed)
                    X -= speed;
                else
                {
                    X = 0;
                    speedMultiplyer = 1;
                }
            }

            // Le joueur accélère tant qu'il maintient, redevient lent lorsqu'il arrête
            if (GameSpace.keysPressed.Contains('A') || GameSpace.keysPressed.Contains('D'))
                speedMultiplyer++;
            else
                speedMultiplyer = 1;

            #endregion

        }
        #endregion

        #region  ================ Rendu graphique  ================

        // Afficher le joueur
        public void Render(BufferedGraphics drawingSpace)
        {
            drawingSpace.Graphics.DrawImage(Resources.PirateClark, Convert.ToSingle(X), Convert.ToSingle(Y), WIDTH, HEIGHT);
            drawingSpace.Graphics.DrawString($"{Hp}/{Config.PLAYER_HP}", TextHelpers.HpFont, TextHelpers.redBrush, Convert.ToSingle(Config.GAMESPACE_WIDTH - 180), Convert.ToSingle(Config.GAMESPACE_HEIGHT - 70));
        }
        #endregion
    }
}
