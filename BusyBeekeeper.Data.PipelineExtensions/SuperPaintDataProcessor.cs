using System;
using Microsoft.Xna.Framework.Content.Pipeline;

namespace BusyBeekeeper.Data.PipelineExtensions
{
    [ContentProcessor(DisplayName = "Data/SuperPaint - Processor")]
    public class SuperPaintDataProcessor : ContentProcessor<string, MetaSuperPaint[]>
    {
        private const string sTokenDelimiter = "||";

        private const int sSectionCountLineIndex = 0;
        private const int sFirstSectionLineIndex = 1;

        private const int sIdIndex = 0;
        private const int sNameIndex = 1;
        private const int sDescriptionIndex = 2;
        private const int sPurchasePriceIndex = 3;
        private const int sColorAIndex = 4;
        private const int sColorRIndex = 5;
        private const int sColorGIndex = 6;
        private const int sColorBIndex = 7;

        public override MetaSuperPaint[] Process(string input, ContentProcessorContext context)
        {
            var lLines = input.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            var lMetaSuperPaintCount = int.Parse(lLines[sSectionCountLineIndex]);
            var lMetaSuperPaints = new MetaSuperPaint[lMetaSuperPaintCount];

            for (int lLineIndex = sFirstSectionLineIndex; lLineIndex < sFirstSectionLineIndex + lMetaSuperPaintCount; lLineIndex++)
            {
                var lLine = lLines[lLineIndex];
                var lTokens = lLine.Split(new[] {sTokenDelimiter}, StringSplitOptions.None);

                var lMetaSuperPaint = new MetaSuperPaint();

                lMetaSuperPaint.Id = int.Parse(lTokens[sIdIndex]);
                lMetaSuperPaint.Name = lTokens[sNameIndex];
                lMetaSuperPaint.Description = lTokens[sDescriptionIndex];
                lMetaSuperPaint.PurchasePrice = int.Parse(lTokens[sPurchasePriceIndex]);
                lMetaSuperPaint.ColorValueA = int.Parse(lTokens[sColorAIndex]);
                lMetaSuperPaint.ColorValueR = int.Parse(lTokens[sColorRIndex]);
                lMetaSuperPaint.ColorValueG = int.Parse(lTokens[sColorGIndex]);
                lMetaSuperPaint.ColorValueB = int.Parse(lTokens[sColorBIndex]);

                var lArrayIndex = lLineIndex - sFirstSectionLineIndex;
                lMetaSuperPaints[lArrayIndex] = lMetaSuperPaint;
            }

            return lMetaSuperPaints;
        }
    }
}