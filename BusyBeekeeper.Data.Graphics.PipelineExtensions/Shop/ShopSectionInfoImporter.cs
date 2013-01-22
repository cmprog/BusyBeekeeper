using Microsoft.Xna.Framework.Content.Pipeline;

namespace BusyBeekeeper.Data.Graphics.PipelineExtensions.Shop
{
    [ContentImporter(".ssi", DisplayName = "GraphicsData/Shop/ShopSectionInfo - Importer", DefaultProcessor = "ShopSectionInfoProcessor")]
    public class ShopSectionInfoImporter : ContentImporter<string>
    {
        public override string Import(string filename, ContentImporterContext context)
        {
            return System.IO.File.ReadAllText(filename);
        }
    }
}
