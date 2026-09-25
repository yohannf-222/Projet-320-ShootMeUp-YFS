using Drones.Model;
using Drones.Helpers;
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

        BufferedGraphicsContext currentContext;
        BufferedGraphics gamespace;

        // Initialisation de l'espace du jeu avec le joueur
        public GameSpace(Player player, List<Gouvernail> gouvernails)
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
            gamespace.Render();
        }

        // Calcul du nouvel état après que 'interval' millisecondes se sont écoulées
        private void Update(int interval)
        {
            foreach (Gouvernail gouvernail in _gouvernails)
            {
                gouvernail.Update();
            }

            _player.Update(interval, _mouse, ref _gouvernails);

            _mouse = null;
        }

        // Méthode appelée à chaque frame
        private void NewFrame(object sender, EventArgs e)
        {
            this.Update(ticker.Interval);
            this.Render();
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