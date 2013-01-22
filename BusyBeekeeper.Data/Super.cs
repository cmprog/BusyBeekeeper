namespace BusyBeekeeper.Data
{
    public sealed class Super
    {
        public Super()
        {
            this.Type = SuperType.Unassigned;
        }

        public int MetaId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        internal BeeHive BeeHive { get; set; }

        public SuperType Type { get; internal set; }
        public int Depth { get; set; }
        public int HoneyCollected { get; set; }

        public SuperPaint SuperPaint { get; set; }
    }

    public enum SuperType
    {
        Unassigned,
        BroodChamber,
        HoneyCollection,
    }
}
