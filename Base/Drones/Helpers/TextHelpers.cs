namespace ShootMeUp.Helpers
{
    // Outils pour écrire du texte dans un environnement graphique
    internal static class TextHelpers
    {
        public static Font drawFont = new Font("Arial", 10);
        public static SolidBrush writingBrush = new SolidBrush(Color.Black);

        // Police pour Hp
        public static Font HpFont = new Font(TextHelpers.drawFont.FontFamily, 24);
        public static Brush redBrush = new SolidBrush(Color.Red);
        public static Brush blueBrush = new SolidBrush(Color.Blue);
    }
}
