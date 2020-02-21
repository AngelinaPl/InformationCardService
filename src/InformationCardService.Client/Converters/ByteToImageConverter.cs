using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace InformationCardService.Client.Converters
{
    internal class ByteToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var byteImg = (byte[]) value;
            if (byteImg != null)
            {
                var ms = new MemoryStream(byteImg);
                var bim = new BitmapImage();
                bim.BeginInit();
                bim.StreamSource = ms;
                bim.EndInit();
                return bim;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("The method or operation is not implemented.");
        }
    }
}