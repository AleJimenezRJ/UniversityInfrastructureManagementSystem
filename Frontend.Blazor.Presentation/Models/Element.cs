namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Presentation.Models
{
    public class Element
    {
        public int Number { get; set; }
        public string Sign { get; set; } = null!;
        public string Name { get; set; } = null!;
        public int Position { get; set; }
        public double Molar { get; set; }
    }
}