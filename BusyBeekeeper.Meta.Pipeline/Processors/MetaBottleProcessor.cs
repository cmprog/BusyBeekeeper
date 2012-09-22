using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;

namespace BusyBeekeeper.Data.Meta.Pipeline.Processors
{
    [ContentProcessor(DisplayName = "BB - MetaBottle Processor")]
    public class MetaBottleProcessor : ContentProcessor<XDocument, IList<MetaBottle>>
    {
        public override IList<MetaBottle> Process(XDocument document, ContentProcessorContext context)
        {
            return new MetaBottle[0];
        }
    }
}
