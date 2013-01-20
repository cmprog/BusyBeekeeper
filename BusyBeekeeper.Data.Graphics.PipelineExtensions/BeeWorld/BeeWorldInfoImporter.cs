using Microsoft.Xna.Framework.Content.Pipeline;

namespace BusyBeekeeper.Data.Graphics.PipelineExtensions.BeeWorld
{
    [ContentImporter(".bwi", DisplayName = "GraphicsData/BeeWorld/BeeWorldInfo - Importer", DefaultProcessor = "BeeWorldInfoProcessor")]
    public class BeeWorldInfoImporter : ContentImporter<string>
    {
        public override string Import(string filename, ContentImporterContext context)
        {
            return System.IO.File.ReadAllText(filename);
        }
    }
}
