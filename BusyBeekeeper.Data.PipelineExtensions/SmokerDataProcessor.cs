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
    [ContentProcessor(DisplayName = "Data/SmokerData - Processor")]
    public class SmokerDataProcessor : ContentProcessor<string, Smoker[]>
    {
        private const int sIdIndex = 0;
        private const int sNameIndex = 1;
        private const int sDescriptionIndex = 2;
        private const int sPurchasePriceIndex = 3;
        private const int sLevelIndex = 4;
        private const int sAgressionFactorIndex = 5;

        public override Smoker[] Process(string input, ContentProcessorContext context)
        {
            var lLines = input.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            var lInfoCount = int.Parse(lLines[0]);
            var lMetaQueenBees = new Smoker[lInfoCount];

            for (int lLineIndex = 1; lLineIndex <= lInfoCount; lLineIndex++)
            {
                var lLine = lLines[lLineIndex];
                var lTokens = lLine.Split(new[] { "||" }, StringSplitOptions.None);

                var lSmokers = new Smoker();

                lSmokers.Id = int.Parse(lTokens[sIdIndex]);
                lSmokers.Name = lTokens[sNameIndex];
                lSmokers.Description = lTokens[sDescriptionIndex];
                lSmokers.PurchasePrice = int.Parse(lTokens[sPurchasePriceIndex]);
                lSmokers.Level = int.Parse(lTokens[sLevelIndex]);
                lSmokers.BeeAggressionFactor = int.Parse(lTokens[sAgressionFactorIndex]);

                var lArrayIndex = lLineIndex - 1;
                lMetaQueenBees[lArrayIndex] = lSmokers;
            }

            return lMetaQueenBees;
        }
    }
}