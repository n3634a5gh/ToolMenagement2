using Aspose.BarCode.Generation;
using Microsoft.VisualBasic;
namespace Tool_Menagement.Helpers
{
    public class Barcode
    {
        public byte[] GenerateBarcode(string data)
        {
            BarcodeGenerator generator = new BarcodeGenerator(EncodeTypes.Code128, data);
            using (MemoryStream stream = new MemoryStream())
            {
                generator.Save(stream, BarCodeImageFormat.Jpeg);
                return stream.ToArray();
            }
        }

    }
}
