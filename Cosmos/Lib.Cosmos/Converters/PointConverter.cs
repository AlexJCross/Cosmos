namespace Lib.Cosmos.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using System.Windows.Media.Media3D;

    class PointConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var number = (double)value;
            return new Point3D(0, 0, number);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
