using Microsoft.Xna.Framework.Content.Pipeline;

namespace BusyBeekeeper.Data.PipelineExtensions
{
    [ContentImporter(".spd", DisplayName = "Data/SuperPaint - Importer", DefaultProcessor = "SuperPaintDataProcessor")]
    public class SuperPaintDataImporter : ContentImporter<string>
    {
        public override string Import(string filename, ContentImporterContext context)
        {
            return System.IO.File.ReadAllText(filename);
        }
    }
}
