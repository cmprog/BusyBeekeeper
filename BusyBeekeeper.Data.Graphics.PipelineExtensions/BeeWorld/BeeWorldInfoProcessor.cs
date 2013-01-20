using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;
using BusyBeekeeper.Data.Graphics.BeeWorld;

namespace BusyBeekeeper.Data.Graphics.PipelineExtensions.BeeWorld
{
    [ContentProcessor(DisplayName = "GraphicsData/BeeWorld/BeeWorldInfo - Processor")]
    public class BeeWorldInfoProcessor : ContentProcessor<string, BeeWorldInfo>
    {
        private const string sTokenDelimiter = ",";

        private const int sYardInfoCountLineIndex = 0;
        private const int sMarketInfoLineIndex = 1;
        private const int sShopInfoLineIndex = 2;
        private const int sHoneyHouseInfoLineIndex = 3;
        private const int sFirstYardInfoLineIndex = 4;

        private const int sSpecialPositionXIndex = 0;
        private const int sSpecialPositionYIndex = 1;
        private const int sSpecialSizeWidthIndex = 2;
        private const int sSpecialSizeHeightIndex = 3;

        private const int sHiveIdIndex = 0;
        private const int sHivePositionXIndex = 1;
        private const int sHivePositionYIndex = 2;
        private const int sHiveSizeWidthIndex = 3;
        private const int sHiveSizeHeightIndex = 4;

        public override BeeWorldInfo Process(string input, ContentProcessorContext context)
        {
            var lLines = input.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            
            var lMarketInfoLine = lLines[sMarketInfoLineIndex];
            var lMarketInfoTokens = lMarketInfoLine.Split(new[] { sTokenDelimiter }, StringSplitOptions.RemoveEmptyEntries);
            var lMarketInfo = new BeeWorldMarketInfo();
            lMarketInfo.NamePosition = new Vector2(
                float.Parse(lMarketInfoTokens[sSpecialPositionXIndex]),
                float.Parse(lMarketInfoTokens[sSpecialPositionYIndex]));
            lMarketInfo.NameSize = new Vector2(
                float.Parse(lMarketInfoTokens[sSpecialSizeWidthIndex]),
                float.Parse(lMarketInfoTokens[sSpecialSizeHeightIndex]));

            var lShopInfoLine = lLines[sShopInfoLineIndex];
            var lShopInfoTokens = lShopInfoLine.Split(new[] { sTokenDelimiter }, StringSplitOptions.RemoveEmptyEntries);
            var lShopInfo = new BeeWorldShopInfo();
            lShopInfo.NamePosition = new Vector2(
                float.Parse(lShopInfoTokens[sSpecialPositionXIndex]),
                float.Parse(lShopInfoTokens[sSpecialPositionYIndex]));
            lShopInfo.NameSize = new Vector2(
                float.Parse(lShopInfoTokens[sSpecialSizeWidthIndex]),
                float.Parse(lShopInfoTokens[sSpecialSizeHeightIndex]));

            var lHoneyHouseInfoLine = lLines[sHoneyHouseInfoLineIndex];
            var lHoneyHouseTokens = lHoneyHouseInfoLine.Split(new[] { sTokenDelimiter }, StringSplitOptions.RemoveEmptyEntries);
            var lHoneyHouseInfo = new BeeWorldHoneyHouseInfo();
            lHoneyHouseInfo.NamePosition = new Vector2(
                float.Parse(lHoneyHouseTokens[sSpecialPositionXIndex]),
                float.Parse(lHoneyHouseTokens[sSpecialPositionYIndex]));
            lHoneyHouseInfo.NameSize = new Vector2(
                float.Parse(lHoneyHouseTokens[sSpecialSizeWidthIndex]),
                float.Parse(lHoneyHouseTokens[sSpecialSizeHeightIndex]));

            var lYardInfoCount = int.Parse(lLines[sYardInfoCountLineIndex]);
            var lYardInfos = new BeeWorldYardInfo[lYardInfoCount];
            for (int lLineIndex = sFirstYardInfoLineIndex; lLineIndex < sFirstYardInfoLineIndex + lYardInfoCount; lLineIndex++)
            {
                var lLine = lLines[lLineIndex];
                var lTokens = lLine.Split(new[] { sTokenDelimiter }, StringSplitOptions.RemoveEmptyEntries);

                var lInfo = new BeeWorldYardInfo();

                lInfo.Id = int.Parse(lTokens[sHiveIdIndex]);

                lInfo.NamePosition = new Vector2(
                    float.Parse(lTokens[sHivePositionXIndex]),
                    float.Parse(lTokens[sHivePositionYIndex]));

                lInfo.NameSize = new Vector2(
                    float.Parse(lTokens[sHiveSizeWidthIndex]),
                    float.Parse(lTokens[sHiveSizeHeightIndex]));

                var lArrayIndex = lLineIndex - sFirstYardInfoLineIndex;
                lYardInfos[lArrayIndex] = lInfo;
            }

            var lWorldInfo = new BeeWorldInfo();
            lWorldInfo.WorldYardInfos = lYardInfos;
            lWorldInfo.ShopInfo = lShopInfo;
            lWorldInfo.MarketInfo = lMarketInfo;
            lWorldInfo.HoneyHouseInfo = lHoneyHouseInfo;
            return lWorldInfo;
        }
    }
}