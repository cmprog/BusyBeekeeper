using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;

namespace BusyBeekeeper.Data.Meta.Pipeline.Importers
{
    [ContentImporter(".bbby", DisplayName = "BB - MetaBeeYard Importer", DefaultProcessor = "MetaBeeYardProcessor")]
    public class MetaBeeYardImporter : ContentImporter<XDocument>
    {
        public override XDocument Import(string filename, ContentImporterContext context)
        {
            return XDocument.Load(filename);
        }
    }
}
