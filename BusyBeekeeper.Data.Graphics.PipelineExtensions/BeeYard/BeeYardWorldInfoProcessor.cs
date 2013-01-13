using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using BusyBeekeeper.Data.Graphics.BeeYard;

namespace BusyBeekeeper.Data.Graphics.PipelineExtensions.BeeHive
{
    [ContentProcessor(DisplayName = "GraphicsData/BeeYard/BeeYardHiveInfo - Processor")]
    public class BeeYardHiveInfoProcessor : ContentProcessor<string, BeeYardHiveInfo[]>
    {
        private const int sIdIndex = 0;
        private const int sPositionXIndex = 1;
        private const int sPositionYIndex = 2;

        public override BeeYardHiveInfo[] Process(string input, ContentProcessorContext context)
        {
            var lLines = input.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            var lInfoCount = int.Parse(lLines[0]);
            var lInfos = new BeeYardHiveInfo[lInfoCount];

            for (int lLineIndex = 1; lLineIndex <= lInfoCount; lLineIndex++)
            {
                var lLine = lLines[lLineIndex];
                var lTokens = lLine.Split(new[] { "||" }, StringSplitOptions.None);

                var lInfo = new BeeYardHiveInfo();

                lInfo.Id = int.Parse(lTokens[sIdIndex]);

                lInfo.Position = new Vector2(
                    float.Parse(lTokens[sPositionXIndex]),
                    float.Parse(lTokens[sPositionYIndex]));

                var lArrayIndex = lLineIndex - 1;
                lInfos[lArrayIndex] = lInfo;
            }

            return lInfos;
        }
    }
}