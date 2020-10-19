using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Runtime.InteropServices;
using DKPBot.Properties;

namespace DKPBot.Definitions
{
    internal static class CONSTANTS
    {
        internal const string TOKEN_PATH = @"Data\DiscordAuthToken.txt";
        internal static readonly string DATA_DIR = @"Data";
        internal static readonly Graphics GRAPHICS;
        internal static readonly Font WHITNEY_FONT;
        internal static readonly double SPACE_LENGTH;
        private static readonly PrivateFontCollection PFC = new PrivateFontCollection();

        static CONSTANTS()
        {
            using var fontStream = new MemoryStream(Resources.whitney_book);

            var pfcData = fontStream.ToArray();
            var pinnedArr = GCHandle.Alloc(pfcData, GCHandleType.Pinned);
            PFC.AddMemoryFont(pinnedArr.AddrOfPinnedObject(), pfcData.Length);
            pinnedArr.Free();

            var bmp = new Bitmap(1, 1);
            GRAPHICS = Graphics.FromImage(bmp);
            var ff = PFC.Families[0];
            WHITNEY_FONT = new Font(ff, 16, FontStyle.Regular);
            SPACE_LENGTH = GRAPHICS.MeasureString(" ", WHITNEY_FONT)
                               .Width * 0.7;
        }
    }
}