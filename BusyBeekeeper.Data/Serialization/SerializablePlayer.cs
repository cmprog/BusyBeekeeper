using System;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BusyBeekeeper.Data.Serialization
{
    public class SerializablePlayer
    {
        public int SlotKey { get; set; }
        public string Name { get; set; }
        public TimeSpan TotalRealTimePlayed { get; set; }
        public int AvailableCoins { get; set; }
        public int TotalCoinsEarned { get; set; }
        public int TotalCoinsSpent { get; set; }
        public int BeeSuitId { get; set; }
        public int BeeYardId { get; set; }
        public int BeeHiveId { get; set; }
        public int HoneyExtractorId { get; set; }
        public int LawnMowerId { get; set; }
        public int MarketBoothId { get; set; }
        public int PrimaryMarketHelperId { get; set; }
        public int SecondaryMarketHelperId { get; set; }
        public int AvatarId { get; set; }
        public int SmokerId { get; set; }
        public int TruckId { get; set; }
        public int UncapingKnifeId { get; set; }
        public List<int> AwardIds { get; set; }
        public List<SerializableBeeYard> UnlockedBeeYardIds { get; set; }
        public List<int> FilledBottleIds { get; set; }
        public List<int> EmptyBottleIds { get; set; }
        public List<int> QueenBeeIds { get; set; }
        public List<int> SuperIds { get; set; }
        public List<int> SuperPaintIds { get; set; }

        /// <summary>
        /// Creates a new SerializablePlayer from the given Player.
        /// </summary>
        /// <param name="player">The Player to convert.</param>
        /// <returns>A new SerializablePlayer representing the given Player.</returns>
        public static SerializablePlayer FromPlayer(Player player)
        {
            return new SerializablePlayer
            {
                SlotKey = player.SlotKey,
                Name = player.Name,
                TotalRealTimePlayed = player.TotalRealTimePlayed.Value,
                AvailableCoins = player.AvailableCoins.Value,
                TotalCoinsEarned = player.TotalCoinsEarned.Value,
                TotalCoinsSpent = player.TotalCoinsSpent.Value,
                BeeSuitId = player.BeeSuit.ResourceId,
                BeeYardId = player.BeeYard.ResourceId,
                BeeHiveId = player.BeeHive.YardLocationId,
                HoneyExtractorId = player.HoneyExtractor.ResourceId,
                LawnMowerId = player.LawnMower.ResourceId,
                MarketBoothId = player.MarketBooth.ResourceId,
                PrimaryMarketHelperId = player.PrimaryMarketHelper.ResourceId,
                SecondaryMarketHelperId = player.SecondaryMarketHelper.ResourceId, 
                AvatarId = player.Avatar.ResourceId,
                SmokerId = player.Smoker.ResourceId,
                TruckId = player.Truck.ResourceId,
                UncapingKnifeId = player.UncapingKnife.ResourceId,
                AwardIds = player.Awards.Select(x => x.ResourceId).ToList(),
                UnlockedBeeYardIds = player.UnlockedBeeYards.Select(x => SerializableBeeYard.FromBeeYard(x)).ToList(),
                FilledBottleIds = player.FilledBottles.Select(x => x.ResourceId).ToList(),
                EmptyBottleIds = player.EmptyBottles.Select(x => x.ResourceId).ToList(),
                QueenBeeIds = player.QueenBees.Select(x => x.ResourceId).ToList(),
                SuperIds = player.Supers.Select(x => x.ResourceId).ToList(),
                SuperPaintIds = player.SuperPaints.Select(x => x.ResourceId).ToList()
            };
        }
    }
}
