using ShootMeUp.Helpers;
using ShootMeUp.Model;
using System.Numerics;
namespace ShootMeUp
{
    // La classe GameSpace représente la zone du jeu
    // Il s'agit d'un formulaire (une fenêtre) qui montre une vue 2D depuis en dessus
    // Il n'y a donc pas de notion d'altitude qui intervient
    public partial class GameSpace : Form
    {
        public static List<char> keysPressed = new List<char>();          // Liste contenant les touches qui sont en train d'être pressées 

        public static readonly int WIDTH = Config.GAMESPACE_WIDTH;      // Dimensions du gamespace, largeur
        public static readonly int HEIGHT = Config.GAMESPACE_HEIGHT;    // Dimensions du gamespace, hauteur

        //les clics de la souris
        public static MouseEventArgs? _mouse;

        // Le joueur
        private Player _player;

        // Les projectiles amis
        private List<Gouvernail> _gouvernails;

        //les obstacles
        private List<Obstacle> _obstacles;

        //les ennemis
        private List<Ennemi> _ennemis;

        BufferedGraphicsContext currentContext;
        BufferedGraphics gamespace;

        // Initialisation de l'espace du jeu avec le joueur
        public GameSpace(Player player, List<Gouvernail> gouvernails, List<Obstacle> obstacles, List<Ennemi> ennemis)
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
            // Update du joueur
            _player.Update(interval, _mouse, ref _gouvernails);

            // Update des projectiles amis
            foreach (Gouvernail gouvernail in _gouvernails)
            {
                gouvernail.Update();
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
            }

            // Appel de la méthode qui gère les impacts de projectiles
            ManageHits(ref _gouvernails, _player, _obstacles);

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
        /// Supprime les projectiles qui touchent un obstacle et font des dégats à l'élément touché. 
        /// </summary>
        /// <param name="gouvernails">Liste des gouvernails, modifiée directement</param>
        /// <param name="player">Objet joueur</param>
        /// <param name="obstacles">liste des obstacles</param>
        private void ManageHits(ref List<Gouvernail> gouvernails, Player player, List<Obstacle> obstacles)
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

                    if (gouvernails[j].X > Config.GAMESPACE_WIDTH + Config.OBJECT_DELETION_MARGIN || gouvernails[j].X < -(Config.OBJECT_DELETION_MARGIN)
                        || gouvernails[j].Y > Config.GAMESPACE_WIDTH + Config.OBJECT_DELETION_MARGIN || gouvernails[j].Y < -(Config.OBJECT_DELETION_MARGIN))
                    {
                        gouvernails.RemoveAt(j);
                    }
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