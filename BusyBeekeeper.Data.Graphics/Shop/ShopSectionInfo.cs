using Microsoft.Xna.Framework;

namespace BusyBeekeeper.Data.Graphics.Shop
{
    public sealed class ShopSectionInfo
    {
        public int Id { get; set; }

        public string NameText { get; set; }
        public Vector2 NamePosition { get; set; }
        public Vector2 NameSize { get; set; }

        public string DescriptionText { get; set; }
    }
}
