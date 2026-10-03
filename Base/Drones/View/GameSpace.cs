using Drones.Helpers;
using Drones.Model;
using System.Numerics;
namespace Drones
{
    // La classe GameSpace représente la zone du jeu
    // Il s'agit d'un formulaire (une fenêtre) qui montre une vue 2D depuis en dessus
    // Il n'y a donc pas de notion d'altitude qui intervient

    public partial class GameSpace : Form
    {
        public static List<char> keysPressed = new List<char>();          // Liste contenant les touches qui sont en train d'être pressées 

        public static readonly int WIDTH = Config.GAMESPACE_WIDTH;        // Dimensions du gamespace
        public static readonly int HEIGHT = Config.GAMESPACE_HEIGHT;

        //les clics de la souris
        public static MouseEventArgs? _mouse;
        // Le joueur
        private Player _player;
        // Les gouvernails
        private List<Gouvernail> _gouvernails;
        //les obstacles
        private List<Obstacle> _obstacles;

        BufferedGraphicsContext currentContext;
        BufferedGraphics gamespace;

        // Initialisation de l'espace du jeu avec le joueur
        public GameSpace(Player player, List<Gouvernail> gouvernails, List<Obstacle> obstacles)
        {
            InitializeComponent();
            ClientSize = new Size(WIDTH, HEIGHT);

            // Gets a reference to the current BufferedGraphicsContext
            currentContext = BufferedGraphicsManager.Current;
            // Creates a BufferedGraphics instance associated with this form, and with
            // dimensions the same size as the drawing surface of the form.
            gamespace = currentContext.Allocate(this.CreateGraphics(), this.DisplayRectangle);
            this._player = player;
            this._gouvernails = gouvernails;
            this._obstacles = obstacles;
        }

        // Affichage de la situation actuelle
        private void Render()
        {
            gamespace.Graphics.Clear(Color.AliceBlue);

            _player.Render(gamespace);
            foreach (Gouvernail gouvernail in _gouvernails)
            {
                gouvernail.Render(gamespace);
            }
            foreach (Obstacle obstacle in _obstacles)
            {
                obstacle.Render(gamespace);
            }
            gamespace.Render();
        }

        // Calcul du nouvel état après que 'interval' millisecondes se sont écoulées
        private void Update(int interval)
        {
            foreach (Gouvernail gouvernail in _gouvernails)
            {
                gouvernail.Update();
            }
            foreach (Obstacle obstacle in _obstacles)
            {
                obstacle.Update(interval);
            }
            _player.Update(interval, _mouse, ref _gouvernails);
            ManageHits(ref _gouvernails, _player, _obstacles);
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
        /// <param name="gouvernails"></param>
        /// <param name="player"></param>
        /// <param name="obstacles"></param>
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
                    }

                    if (gouvernails[j].X > Config.GAMESPACE_WIDTH + Config.OBJECT_DELETION_MARGIN || gouvernails[j].X < -(Config.OBJECT_DELETION_MARGIN)
                        || gouvernails[j].Y > Config.GAMESPACE_WIDTH + Config.OBJECT_DELETION_MARGIN || gouvernails[j].Y < -(Config.OBJECT_DELETION_MARGIN))
                    {
                        gouvernails.RemoveAt(j);
                    }
                }
            }
        }

        private void GameSpace_KeyDown(object sender, KeyEventArgs e)
        {
            if (keysPressed.Contains(Convert.ToChar(e.KeyValue)) == false)
            {
                keysPressed.Add(Convert.ToChar(e.KeyValue));
            }
        }

        private void GameSpace_KeyUp(object sender, KeyEventArgs e)
        {
            if (keysPressed.Contains(Convert.ToChar(e.KeyValue)))
            {
                keysPressed.Remove(Convert.ToChar(e.KeyValue));
            }
        }

        public void mouseClick(object sender, MouseEventArgs mouse)
        {
            Console.Write(mouse.Button);
        }

        private void PlayerMouseClick(object sender, MouseEventArgs e)
        {
            _mouse = e;
        }
    }
}