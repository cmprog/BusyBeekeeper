using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using BusyBeekeeper.Data.Graphics.BeeWorld;

namespace BusyBeekeeper.Data.Graphics.PipelineExtensions.BeeWorld
{
    [ContentProcessor(DisplayName = "GraphicsData/BeeWorld/YardLocations - Processor")]
    public class BeeYardWorldInfoProcessor : ContentProcessor<string, BeeYardWorldInfo[]>
    {
        private const int sIdIndex = 0;
        private const int sPositionXIndex = 1;
        private const int sPositionYIndex = 2;
        private const int sSizeWidthIndex = 3;
        private const int sSizeHeightIndex = 4;

        public override BeeYardWorldInfo[] Process(string input, ContentProcessorContext context)
        {
            var lLines = input.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            var lInfoCount = int.Parse(lLines[0]);
            var lInfos = new BeeYardWorldInfo[lInfoCount];

            for (int lLineIndex = 1; lLineIndex <= lInfoCount; lLineIndex++)
            {
                var lLine = lLines[lLineIndex];
                var lTokens = lLine.Split(',');

                var lInfo = new BeeYardWorldInfo();

                lInfo.Id = int.Parse(lTokens[sIdIndex]);

                lInfo.NamePosition = new Vector2(
                    float.Parse(lTokens[sPositionXIndex]),
                    float.Parse(lTokens[sPositionYIndex]));
                
                lInfo.NameSize = new Vector2(
                    float.Parse(lTokens[sSizeWidthIndex]),
                    float.Parse(lTokens[sSizeHeightIndex]));

                var lArrayIndex = lLineIndex - 1;
                lInfos[lArrayIndex] = lInfo;
            }

            return lInfos;
        }
    }
}