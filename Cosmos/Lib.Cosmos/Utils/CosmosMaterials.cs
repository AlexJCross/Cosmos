namespace Lib.Cosmos.Utils
{
    using System;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Media3D;
    using HelixToolkit.Wpf;

    public static class CosmosResources
    {
        public static readonly FontFamily LatexFont;

        static CosmosResources()
        {
            Uri uri = new Uri("pack://application:,,,/Resources/#CMU Serif Italic", UriKind.Absolute);

            LatexFont = new FontFamily(uri, "CMU Serif Italic");
        }
    }

    public static class CosmosMaterials
    {
        public static readonly Material Material1;
        public static readonly Material Material2;
        public static readonly Material Material3;
        public static readonly Material Material4;
        public static readonly Material Material5;
        public static readonly Material Material8;
        public static readonly Material Material9;

        public static readonly Color Color1;
        public static readonly Color Color2;
        public static readonly Color Color3;
        public static readonly Color Color4;
        public static readonly Color Color5;
        public static readonly Color Color8;
        public static readonly Color Color9;

        public static readonly SolidColorBrush Brush1;
        public static readonly SolidColorBrush Brush2;
        public static readonly SolidColorBrush Brush3;
        public static readonly SolidColorBrush Brush4;
        public static readonly SolidColorBrush Brush5;
        public static readonly SolidColorBrush Brush8;
        public static readonly SolidColorBrush Brush9;

        static CosmosMaterials()
        {
            var uri = new Uri("/Lib.Cosmos;component/Resources/Styles.xaml", UriKind.Relative);
            var dict = new ResourceDictionary { Source = uri };

            Color1 = (Color)dict["Kuler1"];
            Color2 = (Color)dict["Kuler2"];
            Color3 = (Color)dict["Kuler3"];
            Color4 = (Color)dict["Kuler4"];
            Color5 = (Color)dict["Kuler5"];
            Color8 = (Color)dict["Kuler8"];
            Color9 = (Color)dict["Kuler9"];

            Brush1 = new SolidColorBrush(Color1);
            Brush2 = new SolidColorBrush(Color2);
            Brush3 = new SolidColorBrush(Color3);
            Brush4 = new SolidColorBrush(Color4);
            Brush5 = new SolidColorBrush(Color5);
            Brush8 = new SolidColorBrush(Color8);
            Brush9 = new SolidColorBrush(Color9);

            Brush1.Freeze();
            Brush2.Freeze();
            Brush3.Freeze();
            Brush4.Freeze();
            Brush5.Freeze();
            Brush8.Freeze();
            Brush9.Freeze();

            Material1 = GetValue(Color1);
            Material2 = GetValue(Color2);
            Material3 = GetValue(Color3);
            Material4 = GetValue(Color4);
            Material5 = GetValue(Color5);
            Material8 = GetValue(Color8);
            Material9 = GetValue(Color9);

            Material1.Freeze();
            Material2.Freeze();
            Material3.Freeze();
            Material4.Freeze();
            Material5.Freeze();
            Material8.Freeze();
            Material9.Freeze();
        }

        private static Material GetValue(Color color)
        {
            var diffuse = new SolidColorBrush(color);
            var specular = BrushHelper.CreateGrayBrush(0.1);
            return MaterialHelper.CreateMaterial(diffuse, 0, 100);
        }
    }
}
