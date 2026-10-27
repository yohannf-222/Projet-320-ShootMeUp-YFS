using ShootMeUp.Helpers;
using ShootMeUp.Model;
using System.Diagnostics.Metrics;
using System.Numerics;
using System.Reflection;
namespace ShootMeUp
{
    // La classe GameSpace représente la zone du jeu
    // Il s'agit d'un formulaire (une fenêtre) qui montre une vue 2D depuis en dessus
    // Il n'y a donc pas de notion d'altitude qui intervient
    public partial class GameSpace : Form
    {
        public bool isPaused = false;                                       // Booléen qui indique si le jeu est en pause ou non
        public static List<char> keysPressed = new List<char>();            // Liste contenant les touches qui sont en train d'être pressées 

        public static readonly int WIDTH = Config.GAMESPACE_WIDTH;          // Dimensions du gamespace, largeur
        public static readonly int HEIGHT = Config.GAMESPACE_HEIGHT;        // Dimensions du gamespace, hauteur

        //les clics de la souris
        public static MouseEventArgs? _mouse;

        // Le joueur
        private Player _player;

        // Les projectiles amis
        private List<Gouvernail> _gouvernails;

        // Les projectiles ennemis
        private List<Balle> _balles;

        //les obstacles
        private List<Obstacle> _obstacles;

        //les ennemis
        private List<Ennemi> _ennemis;

        BufferedGraphicsContext currentContext;
        BufferedGraphics gamespace;

        // Initialisation de l'espace du jeu avec le joueur
        public GameSpace(Player player, List<Gouvernail> gouvernails, List<Balle> balles, List<Obstacle> obstacles, List<Ennemi> ennemis)
        {
            InitializeComponent();
            ClientSize = new Size(WIDTH, HEIGHT);

            // Gets a reference to the current BufferedGraphicsContext
            currentContext = BufferedGraphicsManager.Current;

            // Creates a BufferedGraphics instance associated with this form, and with
            // dimensions the same size as the drawing surface of the form.
            gamespace = currentContext.Allocate(this.CreateGraphics(), this.DisplayRectangle);

            // Ajoute les entités du jeu qui sont reçues en paramêtre dans les attributs de Gamespace
            this._player = player;
            this._gouvernails = gouvernails;
            this._balles = balles;
            this._obstacles = obstacles;
            this._ennemis = ennemis;
        }

        // Affichage de la situation actuelle
        private void Render()
        {
            // Réinitialisation (visuelle) de l'environnement Gamespace
            gamespace.Graphics.Clear(Color.AliceBlue);

            // Affichage du joueur
            _player.Render(gamespace);

            // Affichage des projectiles amis
            foreach (Gouvernail gouvernail in _gouvernails)
            {
                gouvernail.Render(gamespace);
            }

            // Affichage des projectiles ennemis
            foreach (Balle balle in _balles)
            {
                balle.Render(gamespace);
            }

            // Affichage des obstacles
            foreach (Obstacle obstacle in _obstacles)
            {
                obstacle.Render(gamespace);
            }

            // Affichage des ennemis
            foreach (Ennemi ennemi in _ennemis)
            {
                ennemi.Render(gamespace);
            }
            gamespace.Render();
        }

        // Calcul du nouvel état après que 'interval' millisecondes se sont écoulées
        private void Update(int interval)
        {
            if (!isPaused)
            {
                // Update du joueur
                _player.Update(interval, _mouse, ref _gouvernails);

                // Update des projectiles amis
                foreach (Gouvernail gouvernail in _gouvernails)
                {
                    gouvernail.Update();
                }

                // Update des projectiles ennemis
                foreach (Balle balle in _balles)
                {
                    balle.Update();
                }

                // Update des obstacles
                foreach (Obstacle obstacle in _obstacles)
                {
                    obstacle.Update(interval);
                }

                // Update des ennemis
                foreach (Ennemi ennemi in _ennemis)
                {
                    ennemi.Update(interval);

                    if (ennemi.counter < 50)
                    {
                        ennemi.counter++;
                    }
                    else
                    {
                        ennemi.counter = 0;
                        _balles.Add(new Balle(ennemi.Position.X, ennemi.Position.Y));
                    }
                }

                // Appel de la méthode qui gère les impacts de projectiles
                ManageHits(ref _gouvernails, ref _balles, _player, _obstacles, _ennemis);

            }

            // Vide les inputs de la souris
            _mouse = null;
        }

        // Méthode appelée à chaque frame
        private void NewFrame(object sender, EventArgs e)
        {
            this.Update(ticker.Interval);
            this.Render();
        }

        /// <summary>
        /// Gère les projectiles et les collisions entre ces derniers et les objets. Supprime ceux qui sortent du terrain. 
        /// </summary>
        /// <param name="gouvernails">Liste des gouvernails, modifiée directement</param>
        /// <param name="balles">Liste des projectiles ennemis, modifiée directement</param>
        /// <param name="player">Objet joueur</param>
        /// <param name="obstacles">liste des obstacles</param>
        private void ManageHits(ref List<Gouvernail> gouvernails, ref List<Balle> balles, Player player, List<Obstacle> obstacles, List<Ennemi> ennemis)
        {
            for (int i = obstacles.Count - 1; i >= 0; i--)
            {
                for (int j = gouvernails.Count - 1; j >= 0; j--)
                {
                    if (MathHelpers.IsTouching(obstacles[i].X, obstacles[i].Y, Obstacle.width, Obstacle.height, gouvernails[j].X, gouvernails[j].Y, Config.GOUVERNAIL_RADIUS)
                        && obstacles[i].State > 0)
                    {
                        gouvernails.RemoveAt(j);
                        obstacles[i].GetHit(Config.GOUVERNAIL_DAMAGE);
                        return;
                    }
                }
            }

            for (int i = ennemis.Count - 1; i >= 0; i--)
            {
                for (int j = gouvernails.Count - 1; j >= 0; j--)
                {
                    if (MathHelpers.IsTouching(ennemis[i].Position.X, ennemis[i].Position.Y, Ennemi.width, Ennemi.height, gouvernails[j].X, gouvernails[j].Y, Config.GOUVERNAIL_RADIUS))
                    {
                        gouvernails.RemoveAt(j);
                        ennemis[i].Hp -= Config.GOUVERNAIL_DAMAGE;
                        if (ennemis[i].Hp < 0)
                        {
                            ennemis.RemoveAt(i);
                            ennemis.Add(new Ennemi());
                            player._score += 100;
                        }
                        return;
                    }

                }
            }

            for (int i = gouvernails.Count - 1; i >= 0; i--)
            {
                if (gouvernails[i].X > Config.GAMESPACE_WIDTH + Config.OBJECT_DELETION_MARGIN || gouvernails[i].X < -(Config.OBJECT_DELETION_MARGIN)
                        || gouvernails[i].Y > Config.GAMESPACE_WIDTH + Config.OBJECT_DELETION_MARGIN || gouvernails[i].Y < -(Config.OBJECT_DELETION_MARGIN))
                {
                    gouvernails.RemoveAt(i);
                }
            }

            for (int i = obstacles.Count - 1; i >= 0; i--)
            {
                for (int j = balles.Count - 1; j >= 0; j--)
                {
                    if (MathHelpers.IsTouching(obstacles[i].X, obstacles[i].Y, Obstacle.width, Obstacle.height, balles[j].X, balles[j].Y, Balle.width * Config.PIXEL_SIZE_MULTIPLYER, Balle.height * Config.PIXEL_SIZE_MULTIPLYER)
                        && obstacles[i].State > 0)
                    {
                        balles.RemoveAt(j);
                        obstacles[i].GetHit(Config.ENNEMI_DAMAGE);
                        return;
                    }
                }
            }

            for (int j = balles.Count - 1; j >= 0; j--)
            {
                if (MathHelpers.IsTouching(player.X, player.Y, Ennemi.width, Ennemi.height, balles[j].X, balles[j].Y, Balle.width * Config.PIXEL_SIZE_MULTIPLYER, Balle.height * Config.PIXEL_SIZE_MULTIPLYER))
                {
                    balles.RemoveAt(j);
                    player.Hp -= Config.ENNEMI_DAMAGE;
                    return;
                }

            }

            for (int i = balles.Count - 1; i >= 0; i--)
            {
                if (balles[i].X > Config.GAMESPACE_WIDTH + Config.OBJECT_DELETION_MARGIN || balles[i].X < -(Config.OBJECT_DELETION_MARGIN)
                        || balles[i].Y > Config.GAMESPACE_WIDTH + Config.OBJECT_DELETION_MARGIN || balles[i].Y < -(Config.OBJECT_DELETION_MARGIN))
                {
                    balles.RemoveAt(i);
                }
            }
        }

        #region ================= Comptabilisation des inputs sur les périphériques =======================

        /// <summary>
        /// Lorsqu'on clique sur une touche, ajoute la touche dans la liste des touches pressées
        /// </summary>
        private void GameSpace_KeyDown(object sender, KeyEventArgs e)
        {
            // Si la liste des touches pressées ne la contient pas déja,
            if (!keysPressed.Contains(Convert.ToChar(e.KeyValue)))
            {
                // ajouter la nouvelle touche
                keysPressed.Add(Convert.ToChar(e.KeyValue));
            }
        }

        /// <summary>
        /// Lorsqu'on arrête de cliquer sur une touche, enlève la touche de la liste des touches pressées
        /// </summary>
        private void GameSpace_KeyUp(object sender, KeyEventArgs e)
        {
            if (keysPressed.Contains(Convert.ToChar(e.KeyValue)))
            {
                keysPressed.Remove(Convert.ToChar(e.KeyValue));
            }
        }

        /// <summary>
        /// Prends compte des clics de la souris et les mets dans _mouse
        /// </summary>
        private void PlayerMouseClick(object sender, MouseEventArgs e)
        {
            _mouse = e;
        }
        #endregion
    }
}