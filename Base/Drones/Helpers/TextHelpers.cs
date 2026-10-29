namespace ShootMeUp.Helpers
{
    // Outils pour écrire du texte dans un environnement graphique
    internal static class TextHelpers
    {
        public static Font drawFont = new Font("Arial", 10);
        public static SolidBrush writingBrush = new SolidBrush(Color.Black);

        // Police utilisée pour afficher des textes en assez grands.
        public static Font tallFont = new Font(TextHelpers.drawFont.FontFamily, 24);        
        public static Font mediumFont = new Font(TextHelpers.drawFont.FontFamily, 20);        
        public static Brush redBrush = new SolidBrush(Color.Red);
        public static Brush blueBrush = new SolidBrush(Color.Blue);
        public static Brush blackBrush = new SolidBrush(Color.Black);
        public static Brush lightBlueBrush = new SolidBrush(Color.LightBlue);
    }
}
