using System;
using Microsoft.Xna.Framework.Content.Pipeline;

namespace BusyBeekeeper.Data.PipelineExtensions
{
    [ContentProcessor(DisplayName = "Data/QueenBeeData - Processor")]
    public class QueenBeeDataProcessor : ContentProcessor<string, MetaQueenBee[]>
    {
        private const int sIdIndex = 0;
        private const int sNameIndex = 1;
        private const int sDescriptionIndex = 2;
        private const int sPurchasePriceIndex = 3;
        private const int sPopulationFactorIndex = 4;
        private const int sHoneyCollectionFactorIndex = 5;
        private const int sColonyStrengthFactorIndex = 6;
        private const int sNaturalBeeAgressionFactorIndex = 7;
        private const int sSwarmLiklinessFactorIndex = 8;

        public override MetaQueenBee[] Process(string input, ContentProcessorContext context)
        {
            var lLines = input.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            var lInfoCount = int.Parse(lLines[0]);
            var lMetaQueenBees = new MetaQueenBee[lInfoCount];

            for (int lLineIndex = 1; lLineIndex <= lInfoCount; lLineIndex++)
            {
                var lLine = lLines[lLineIndex];
                var lTokens = lLine.Split(new[] { "||" }, StringSplitOptions.None);

                var lMetaQueenBee = new MetaQueenBee();

                lMetaQueenBee.Id = int.Parse(lTokens[sIdIndex]);
                lMetaQueenBee.Name = lTokens[sNameIndex];
                lMetaQueenBee.Description = lTokens[sDescriptionIndex];
                lMetaQueenBee.PurchasePrice = int.Parse(lTokens[sPurchasePriceIndex]);
                lMetaQueenBee.BeePopulationGrowthFactor = int.Parse(lTokens[sPopulationFactorIndex]);
                lMetaQueenBee.HoneyCollectionFactor = int.Parse(lTokens[sHoneyCollectionFactorIndex]);
                lMetaQueenBee.ColonyStrengthFactor = int.Parse(lTokens[sColonyStrengthFactorIndex]);
                lMetaQueenBee.NaturalBeeAgressionFactor = int.Parse(lTokens[sNaturalBeeAgressionFactorIndex]);
                lMetaQueenBee.SwarmLikelinessFactor = int.Parse(lTokens[sSwarmLiklinessFactorIndex]);

                var lArrayIndex = lLineIndex - 1;
                lMetaQueenBees[lArrayIndex] = lMetaQueenBee;
            }

            return lMetaQueenBees;
        }
    }
}