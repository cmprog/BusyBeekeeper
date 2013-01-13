using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using BusyBeekeeper.Data;

namespace BusyBeekeeper.Data.PipelineExtensions
{
    [ContentProcessor(DisplayName = "Data/LawnMowerData - Processor")]
    public class LawnMowerDataProcessor : ContentProcessor<string, LawnMower[]>
    {
        private const int sIdIndex = 0;
        private const int sNameIndex = 1;
        private const int sDescriptionIndex = 2;
        private const int sPurchasePriceIndex = 3;
        private const int sLevelIndex = 4;
        private const int sSpeedFactorIndex = 5;

        public override LawnMower[] Process(string input, ContentProcessorContext context)
        {
            var lLines = input.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            var lInfoCount = int.Parse(lLines[0]);
            var lLawnMowers = new LawnMower[lInfoCount];

            for (int lLineIndex = 1; lLineIndex <= lInfoCount; lLineIndex++)
            {
                var lLine = lLines[lLineIndex];
                var lTokens = lLine.Split(new[] { "||" }, StringSplitOptions.None);

                var lBeeYard = new LawnMower();

                lBeeYard.Id = int.Parse(lTokens[sIdIndex]);
                lBeeYard.Name = lTokens[sNameIndex];
                lBeeYard.Description = lTokens[sDescriptionIndex];
                lBeeYard.PurchasePrice = int.Parse(lTokens[sPurchasePriceIndex]);
                lBeeYard.Level = int.Parse(lTokens[sLevelIndex]);
                lBeeYard.SpeedFactor = int.Parse(lTokens[sSpeedFactorIndex]);

                var lArrayIndex = lLineIndex - 1;
                lLawnMowers[lArrayIndex] = lBeeYard;
            }

            return lLawnMowers;
        }
    }
}