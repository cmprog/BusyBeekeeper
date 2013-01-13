using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;

namespace BusyBeekeeper.Data.PipelineExtensions
{
    [ContentImporter(".lmd", DisplayName = "Data/LawnMowerData - Importer", DefaultProcessor = "LawnMowerDataProcessor")]
    public class LawnMowerDataImporter : ContentImporter<string>
    {
        public override string Import(string filename, ContentImporterContext context)
        {
            return System.IO.File.ReadAllText(filename);
        }
    }
}
