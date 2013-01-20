namespace BusyBeekeeper.Data.Graphics.BeeWorld
{
    public sealed class BeeWorldInfo
    {
        public BeeWorldMarketInfo MarketInfo { get; set; }
        public BeeWorldShopInfo ShopInfo { get; set; }
        public BeeWorldHoneyHouseInfo HoneyHouseInfo { get; set; }
        public BeeWorldYardInfo[] WorldYardInfos { get; set; }
    }
}
