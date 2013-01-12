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
    [ContentProcessor(DisplayName = "Data/BeeYardData - Processor")]
    public class BeeYardDataProcessor : ContentProcessor<string, BeeYard[]>
    {
        private const int sIdIndex = 0;
        private const int sNameIndex = 1;
        private const int sDescriptionIndex = 2;
        private const int sPurchasePriceIndex = 3;
        private const int sDangerFactorIndex = 4;
        private const int sRegrowthFactorIndex = 5;
        private const int sProductivityFactorIndex = 6;
        private const int sMaxHiveCountIndex = 7;
        private const int sMaxHiveHeightIndex = 8;

        public override BeeYard[] Process(string input, ContentProcessorContext context)
        {
            var lLines = input.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            var lInfoCount = int.Parse(lLines[0]);
            var lInfos = new BeeYard[lInfoCount];

            for (int lLineIndex = 1; lLineIndex <= lInfoCount; lLineIndex++)
            {
                var lLine = lLines[lLineIndex];
                var lTokens = lLine.Split(new[] { "||" }, StringSplitOptions.None);

                var lBeeYard = new BeeYard();

                lBeeYard.Id = int.Parse(lTokens[sIdIndex]);
                lBeeYard.Name = lTokens[sNameIndex];
                lBeeYard.Description = lTokens[sDescriptionIndex];
                lBeeYard.PurchasePrice = int.Parse(lTokens[sPurchasePriceIndex]);
                lBeeYard.DangerFactor = int.Parse(lTokens[sDangerFactorIndex]);
                lBeeYard.RegrowthFactor = int.Parse(lTokens[sRegrowthFactorIndex]);
                lBeeYard.ProductivityFactor = int.Parse(lTokens[sProductivityFactorIndex]);
                lBeeYard.MaxHiveCount = int.Parse(lTokens[sMaxHiveCountIndex]);
                lBeeYard.MaxHiveHeight = int.Parse(lTokens[sMaxHiveHeightIndex]);

                var lArrayIndex = lLineIndex - 1;
                lInfos[lArrayIndex] = lBeeYard;
            }

            return lInfos;
        }
    }
}