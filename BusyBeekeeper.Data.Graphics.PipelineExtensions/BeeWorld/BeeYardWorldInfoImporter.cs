using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;

namespace BusyBeekeeper.Data.Graphics.PipelineExtensions.BeeWorld
{
    [ContentImporter(".bhwi", DisplayName = "GraphicsData/BeeWorld/BeeHiveWorldInfo - Importer", DefaultProcessor = "BeeHiveWorldInfoProcessor")]
    public class BeeYardWorldInfoImporter : ContentImporter<string>
    {
        public override string Import(string filename, ContentImporterContext context)
        {
            return System.IO.File.ReadAllText(filename);
        }
    }
}
