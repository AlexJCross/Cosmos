namespace Lib.Cosmos.Utils
{
    using System;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Media3D;
    using HelixToolkit.Wpf;

    public static class CosmosMaterials
    {
        public static readonly Material Material1;
        public static readonly Material Material2;
        public static readonly Material Material3;
        public static readonly Material Material4;
        public static readonly Material Material5;
        public static readonly Material Material8;
        public static readonly Material Material9;

        static CosmosMaterials()
        {
            Uri uri = new Uri("/Lib.Cosmos;component/Resources/Styles.xaml", UriKind.Relative);
            ResourceDictionary dict = new ResourceDictionary { Source = uri };

            var color1 = (Color)dict["Kuler1"];
            var color2 = (Color)dict["Kuler2"];
            var color3 = (Color)dict["Kuler3"];
            var color4 = (Color)dict["Kuler4"];
            var color5 = (Color)dict["Kuler5"];
            var color8 = (Color)dict["Kuler8"];
            var color9 = (Color)dict["Kuler9"];

            Material1 = GetValue(color1);
            Material2 = GetValue(color2);
            Material3 = GetValue(color3);
            Material4 = GetValue(color4);
            Material5 = GetValue(color5);
            Material8 = GetValue(color8);
            Material9 = GetValue(color9);
        }

        private static Material GetValue(Color color)
        {
            var diffuse = new SolidColorBrush(color);
            var specular = BrushHelper.CreateGrayBrush(0.1);
            return MaterialHelper.CreateMaterial(diffuse, 0, 100);
        }
    }
}
