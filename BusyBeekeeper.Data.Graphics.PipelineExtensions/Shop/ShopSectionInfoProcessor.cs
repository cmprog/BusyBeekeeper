using System;
using BusyBeekeeper.Data.Graphics.Shop;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;

namespace BusyBeekeeper.Data.Graphics.PipelineExtensions.Shop
{
    [ContentProcessor(DisplayName = "GraphicsData/Shop/ShopSectionInfo - Processor")]
    public class ShopSectionInfoProcessor : ContentProcessor<string, ShopSectionInfo[]>
    {
        private const string sTokenDelimiter = "||";

        private const int sSectionCountLineIndex = 0;
        private const int sFirstSectionLineIndex = 1;

        private const int sSectionIdIndex = 0;
        private const int sSectionNameIndex = 1;
        private const int sSectionDescriptionIndex = 2;
        private const int sSectionNamePositionXIndex = 3;
        private const int sSectionNamePositionYIndex = 4;
        private const int sSectionNameSizeWidthIndex = 5;
        private const int sSectionNameSizeHeightIndex = 6;

        public override ShopSectionInfo[] Process(string input, ContentProcessorContext context)
        {
            var lLines = input.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            var lSectionCount = int.Parse(lLines[sSectionCountLineIndex]);

            var lSectionInfos = new ShopSectionInfo[lSectionCount];
            for (int lLineIndex = sFirstSectionLineIndex; lLineIndex < sFirstSectionLineIndex + lSectionCount; lLineIndex++)
            {
                var lLine = lLines[lLineIndex];
                var lTokens = lLine.Split(new[] { sTokenDelimiter }, StringSplitOptions.RemoveEmptyEntries);

                var lSectionInfo = new ShopSectionInfo();

                lSectionInfo.Id = int.Parse(lTokens[sSectionIdIndex]);
                lSectionInfo.NameText = lTokens[sSectionNameIndex];
                lSectionInfo.DescriptionText = lTokens[sSectionDescriptionIndex];

                lSectionInfo.NamePosition = new Vector2(
                    float.Parse(lTokens[sSectionNamePositionXIndex]),
                    float.Parse(lTokens[sSectionNamePositionYIndex]));

                lSectionInfo.NameSize = new Vector2(
                    float.Parse(lTokens[sSectionNameSizeWidthIndex]),
                    float.Parse(lTokens[sSectionNameSizeHeightIndex]));

                var lArrayIndex = lLineIndex - sFirstSectionLineIndex;
                lSectionInfos[lArrayIndex] = lSectionInfo;
            }

            return lSectionInfos;
        }
    }
}