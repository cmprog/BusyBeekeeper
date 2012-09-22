using System;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Serialization;
using Microsoft.Xna.Framework.Content;
using BusyBeekeeper.Data.Meta;

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

        public Player ToPlayer(ContentManager contentManager)
        {
            var metaPlayerAvatar = contentManager.Load<MetaPlayerAvatar>("MetaPlayerAvatar" + this.AvatarId);
            var metaBeeSuit = contentManager.Load<MetaBeeSuit>("MetaBeeSuit" + this.BeeSuitId);
            var metaHoneyExtractor = contentManager.Load<MetaHoneyExtractor>("MetaHoneyExtractor" + this.HoneyExtractorId);
            var metaLawnMower = contentManager.Load<MetaLawnMower>("MetaLawnMower" + this.LawnMowerId);
            var metaMarketBooth = contentManager.Load<MetaMarketBooth>("MetaMarketBooth" + this.MarketBoothId);
            var primaryMetaMarketHelper = contentManager.Load<MetaMarketHelper>("MetaMarketHelper" + this.PrimaryMarketHelperId);
            var secondaryMetaMarketHelper = contentManager.Load<MetaMarketHelper>("MetaMarketHelper" + this.SecondaryMarketHelperId);
            var metaSmoker = contentManager.Load<MetaSmoker>("MetaSmoker" + this.SmokerId);
            var metaTruck = contentManager.Load<MetaTruck>("MetaTruck" + this.TruckId);
            var metaUncapingKnife = contentManager.Load<MetaUncapingKnife>("MetaUncapingKnife" + this.UncapingKnifeId);

            var player = new Player(this.SlotKey, this.Name, metaPlayerAvatar.ToPlayerAvatar());
            player.TotalRealTimePlayed.Value = this.TotalRealTimePlayed;
            player.AvailableCoins.Value = this.AvailableCoins;
            player.TotalCoinsEarned.Value = this.TotalCoinsEarned;
            player.TotalCoinsSpent.Value = this.TotalCoinsSpent;
            player.BeeSuit = metaBeeSuit.ToBeeSuit();
            player.HoneyExtractor = metaHoneyExtractor.ToHoneyExtractor();
            player.LawnMower = metaLawnMower.ToLawnMower();
            player.MarketBooth = metaMarketBooth.ToMarketBooth();
            player.PrimaryMarketHelper = primaryMetaMarketHelper.ToMarkerHelper();
            player.SecondaryMarketHelper = secondaryMetaMarketHelper.ToMarkerHelper();
            player.Smoker = metaSmoker.ToSmoker();
            player.Truck = metaTruck.ToTruck();
            player.UncapingKnife = metaUncapingKnife.ToUncapingKnife();

            foreach (var id in this.FilledBottleIds)
            {
                var metaBottle = contentManager.Load<MetaBottle>("MetaBottle" + id);
                player.FilledBottles.Add(metaBottle.ToBottle());
            }
            
            foreach (var id in this.EmptyBottleIds)
            {
                var metaBottle = contentManager.Load<MetaBottle>("MetaBottle" + id);
                player.EmptyBottles.Add(metaBottle.ToBottle());
            }

            foreach (var id in this.QueenBeeIds)
            {
                var metaQueenBee = contentManager.Load<MetaQueenBee>("MetaQueenBee" + id);
                player.QueenBees.Add(metaQueenBee.ToQueenBee());
            }

            foreach (var id in this.SuperIds)
            {
                var metaSuper = contentManager.Load<MetaSuper>("MetaSuper" + id);
                player.Supers.Add(metaSuper.ToSuper());
            }

            foreach (var id in this.SuperPaintIds)
            {
                var metaSuperPaint = contentManager.Load<MetaSuperPaint>("MetaSuperPaint" + id);
                player.SuperPaints.Add(metaSuperPaint.ToSuperPaint());
            }

            return player;
        }

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
