using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BusyBeekeeper
{
    public static class SpriteFontExtensions
    {
        public static string FitTextToWidth(this SpriteFont spriteFont, string text, float width)
        {
            var lTextSize = spriteFont.MeasureString(text);
            if (lTextSize.X < width)
            {
                return text;
            }

            var lTextBuilder = new StringBuilder();
            var lLineBuilder = new StringBuilder();

            var lSpaceSize = spriteFont.MeasureString(" ");
            var lWords = text.Split(' ');
            var lLineSize = Vector2.Zero;

            foreach (var lWord in lWords)
            {
                var lWordSize = spriteFont.MeasureString(lWord);

                if (lLineSize.X + lSpaceSize.X + lWordSize.X > width)
                {
                    lTextBuilder.AppendLine(lLineBuilder.ToString());
                    lLineBuilder.Clear();
                }

                lLineBuilder.Append(lWord).Append(' ');
                lLineSize = spriteFont.MeasureString(lLineBuilder);
            }

            if (lLineBuilder.Length > 0)
            {
                lTextBuilder.Append(lLineBuilder);
            }

            return lTextBuilder.ToString();
        }
    }
}
