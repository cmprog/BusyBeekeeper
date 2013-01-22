namespace BusyBeekeeper.Data
{
    public sealed class MetaSuperPaint
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public int PurchasePrice { get; set; }

        public int ColorValueA { get; set; }
        public int ColorValueR { get; set; }
        public int ColorValueG { get; set; }
        public int ColorValueB { get; set; }
    }
}
