using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows;
using BarcodeStandard;
using SkiaSharp;
using Type = BarcodeStandard.Type;

namespace Tool_Menagement.Helpers
{
    public class Barcode
    {
        /*private static SKBitmap GetBarCode(Barcode b, string codeNumber, Type type, int length = 1000, int height = 200, int fontSize = 40)
        {
            try
            {
                b.IncludeLabel = false;
                b.Alignment = AlignmentPositions.Center;
                b.LabelFont = new SKFont(SKTypeface.Default, fontSize);

                var barcodeImage = b.Encode(type, codeNumber, SKColors.Black, SKColors.White, length, height);
                return SKBitmap.FromImage(barcodeImage);
            }
            catch (Exception)
            {
                return null;
            }
        }*/
    }
}
