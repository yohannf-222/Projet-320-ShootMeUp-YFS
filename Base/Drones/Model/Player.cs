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

        private double _x;                                          // Position en X depuis la gauche de l'espace
        private double _y;                                          // Position en Y depuis le haut de l'espace
        public int _score;                                          // Score du joueur

        private static readonly int WIDTH = 26 * Config.PIXEL_SIZE_MULTIPLYER;          // Dimension du joueur, largeur
        private static readonly int HEIGHT = 43 * Config.PIXEL_SIZE_MULTIPLYER;         // Dimension du joueur, hauteur

        private State _state;                                       // l'état dans lequel le joueur est
        private int speedMultiplyer = 1;                            // Augmente la vitesse continuellement lorsque le joueur maintient le mouvement
        private int _cooldown = Gouvernail.COOLDOWN;                // Compteur de ticks depuis le dernier tir de gouvernail

        public enum State { ALIVE, DEAD, STOPPED };                 // les états possibles dans lequel le joueur peut être

        public static readonly int SPEED = 10;                      // Vitesse du joueur, en px par frame
        public static readonly int TOT_HP = 100;                    // Dégats que le joueur peut encaisser avant de mourir.     

        private int _hp = TOT_HP;

        public double X
        {
            get => _x;
            // Empêcher le joueur de sortir du jeu
            set => _x = Math.Clamp(value, 0, Config.GAMESPACE_WIDTH);
        }
        public double Y { get => _y; set => _y = value; }
        public int Hp
        {
            get => _hp;
            set
            {
                // Vérifier la mort
                if (value <= 0)
                    _state = State.DEAD;
                // Empecher les mauvaises valeurs Hp
                _hp = Math.Clamp(value, 0, TOT_HP);
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
            if (mouse != null && _cooldown >= Gouvernail.COOLDOWN && Convert.ToString(mouse.Button) == "Left")
            {
                gouvernails.Add(new Gouvernail(X + WIDTH / 2, Y + WIDTH / 2, mouse.X, mouse.Y));
                _cooldown = 0;
            }
            _cooldown++;

            #region ============= Mouvements ===========

            // Facteur d'accélération du joueur lorsqu'il avance en ligne droite
            double speed = SPEED * speedMultiplyer;

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
            drawingSpace.Graphics.DrawString($"SCORE : {_score}", TextHelpers.mediumFont, TextHelpers.blueBrush, 100, Convert.ToSingle(Config.GAMESPACE_HEIGHT - 70));
            drawingSpace.Graphics.FillRectangle(TextHelpers.redBrush, Convert.ToSingle(Config.GAMESPACE_WIDTH - 360), Convert.ToSingle(Config.GAMESPACE_HEIGHT - 70), Convert.ToSingle(300 * Hp / TOT_HP), 40);
            drawingSpace.Graphics.DrawRectangle(TextHelpers.whitePen, Convert.ToSingle(Config.GAMESPACE_WIDTH - 360), Convert.ToSingle(Config.GAMESPACE_HEIGHT - 70), 300, 40);
        }
        #endregion
    }
}
