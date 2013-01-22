namespace BusyBeekeeper.Data
{
    /// <summary>
    /// The basic hosing unit of a bee colony. A bee hive lives at a yard and is made up
    /// of various supers.
    /// </summary>
    public class BeeHive
    {
        private readonly HiveSuperCollection mSupers;

        public BeeHive()
        {
            this.mSupers = new HiveSuperCollection(this);
        }

        public int Id { get; set; }
        
        public QueenBee QueenBee { get; set; }

        public int Population { get; set; }
        public int ColonyStrength { get; set; }
        public int ColonyAgressiveness { get; set; }
        public int ColonySwarmLikeliness { get; set; }

        public HiveSuperCollection Supers
        {
            get { return this.mSupers; }
        }
    }
}
