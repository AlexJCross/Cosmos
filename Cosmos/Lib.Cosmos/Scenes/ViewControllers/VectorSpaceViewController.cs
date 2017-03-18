namespace Lib.Cosmos.Scenes.ViewControllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Media.Animation;
    using System.Windows.Media.Media3D;
    using System.Windows.Shapes;
    using Extensions;
    using HelixToolkit.Wpf;
    using Infrastructure;
    using Utils;
    using Views;
    using Visual3Ds;
    using WpfMath;
    using WpfMath.Controls;

    public class VectorSpaceViewController : ISceneViewController
    {
        private readonly VectorSpaceView view;
        private ArrowVisual3D arrow1;
        private ArrowVisual3D arrow2;
        private ParallelogramVisual3D parallelogram;
        private ArrowVisual3D resultant;
        private OneFormVisual3D oneForm;

        public VectorSpaceViewController(VectorSpaceView view)
        {
            this.view = view;
            this.Initialize();
        }

        public IList<ISceneClip> CreateSceneClips()
        {
            var toggleArrow1 = new ToggleSceneClip("Arrow 1",
                isChecked => { this.ToggleVisual(this.arrow1, isChecked); });

            var toggleArrow2 = new ToggleSceneClip("Arrow 2",
                isChecked => { this.ToggleVisual(this.arrow2, isChecked); });

            var toggleResultant = new ToggleSceneClip("Resultant",
                isChecked => { this.ToggleVisual(this.resultant, isChecked); });

            var toggleParallelogram = new ToggleSceneClip("Parallelogram",
                isChecked => { this.ToggleVisual(this.parallelogram, isChecked); });

            var toggleOneForm= new ToggleSceneClip("One Form",
                isChecked => { this.ToggleVisual(this.oneForm, isChecked); });

            return new List<ISceneClip>
            {
                new SceneClip("Add Arrow 1", () =>
                {
                    toggleArrow1.Value = true;
                    this.IntroduceArrow1();
                }),
                new SceneClip("Add Arrow 2", () =>
                {
                    toggleArrow2.Value = true;
                    this.IntroduceArrow2();
                }),
                new SceneClip("Add Many", this.IntroduceArrows),
                new SceneClip("Move Arrows", this.MoveArrows),
                new SceneClip("Vary One-Form", this.VaryMagnitude),
                toggleArrow1,
                toggleArrow2,
                toggleResultant,
                toggleParallelogram,
                toggleOneForm
            };
        }

        private void Initialize()
        {
            this.arrow1 = new ArrowVisual3D
            {
                Material = CosmosMaterials.Material4,
                Diameter = 0.8
            };

            this.arrow2 = new ArrowVisual3D
            {
                Material = CosmosMaterials.Material3,
                Diameter = 0.8
            };

            this.resultant = new ArrowVisual3D
            {
                Material = CosmosMaterials.Material2,
                Diameter = 0.8
            };

            this.parallelogram = new ParallelogramVisual3D
            {
                Point1 = this.arrow1.Point2,
                Point2 = new Point3D(10, 25, 0),
                Material = CosmosMaterials.Material8,
                BackMaterial = CosmosMaterials.Material8
            };

            this.oneForm = new OneFormVisual3D
            {
                Material = CosmosMaterials.Material9,
                Direction = new Vector3D(0, 1, 0),
                NumBars = 6,
                Width = 5,
                Magnitude = 2
            };

            var binding1 = new Binding("Point2")
            {
                Source = this.arrow1
            };

            var binding2 = new Binding("Point2")
            {
                Source = this.arrow2
            };

            var bindingResultant = new Binding("ResultantPoint3D")
            {
                Source = this.parallelogram
            };

            var text = new TextBlock
            {
                Text = "x+y",
                FontSize = 48,
                FontFamily = CosmosResources.LatexFont
            };

            var formulaParser = new TexFormulaParser();


            // TexFormula formula = formulaParser.Parse(@"\left(x^2 + \vec{v} +  \langle{\widetilde{\phi}} |\vec{\Psi} \rangle + 2 \cdot x \right) = 0");
            var formula1 = formulaParser.Parse(@"\vec{v}");
            var formula2 = formulaParser.Parse(@"\vec{u}");

            // formula.SetForeground(Brushes.Green);
            var renderer = formula1.GetRenderer(TexStyle.Display, 48);
            var geometry = renderer.RenderToGeometry(20, 40);
            var path = new Path {Data = geometry, Fill = CosmosMaterials.Brush4};

            var renderer2 = formula2.GetRenderer(TexStyle.Display, 48);
            var geometry2 = renderer2.RenderToGeometry(20, 40);
            var path2 = new Path {Data = geometry2, Fill = CosmosMaterials.Brush3};

            var canvas = this.view.overlay;

            canvas.Children.Add(path);
            Overlay.SetPosition3D(path, new Point3D(20, 10, 0));

            canvas.Children.Add(path2);
            Overlay.SetPosition3D(path2, new Point3D(20, 10, 0));

            var formulaControl = new FormulaControl
            {
                Formula = @"\vec{u}",
                Scale = 48,
                FontFamily = CosmosResources.LatexFont
            };


            // canvas.Children.Add(text);
            // Overlay.SetPosition3D(text, new Point3D(20, 10, 0));

            // canvas.Children.Add(formulaControl);
            // Overlay.SetPosition3D(formulaControl, new Point3D(10, 20, 0));


            BindingOperations.SetBinding(this.parallelogram, ParallelogramVisual3D.Point1Property, binding1);
            BindingOperations.SetBinding(this.parallelogram, ParallelogramVisual3D.Point2Property, binding2);
            BindingOperations.SetBinding(this.resultant, ArrowVisual3D.Point2Property, bindingResultant);
            BindingOperations.SetBinding(path, Overlay.Position3DProperty, binding1);
            BindingOperations.SetBinding(path2, Overlay.Position3DProperty, binding2);
        }

        private async void IntroduceArrow1()
        {
            this.arrow1.Point1 = default(Point3D);
            await this.arrow1.PointTo(new Point3D(30, 15, 0), TimeSpan.FromSeconds(0.5));
        }

        private async void IntroduceArrow2()
        {
            this.arrow2.Point1 = default(Point3D);
            await this.arrow2.PointTo(new Point3D(10, 25, 0), TimeSpan.FromSeconds(0.5));
        }

        private async void MoveArrows()
        {
            Point3D[] points =
            {
                new Point3D(-10, 15, 0), new Point3D(-10, -15, 0), new Point3D(25, -5, 0),
                new Point3D(-10, 15, 0)
            };

            for (var i = 0; i < points.Length; i++)
            {
                var arrow = i % 2 == 0 ? this.arrow2 : this.arrow1;
                await arrow.PointTo(points[i]);
            }
        }

        private void VaryMagnitude()
        {
            var startMagnitude = this.oneForm.Magnitude;

            var animation = new DoubleAnimation(startMagnitude, startMagnitude * 2, new Duration(TimeSpan.FromSeconds(2)))
            {
                FillBehavior = FillBehavior.HoldEnd,
                EasingFunction = new SineEase { EasingMode = EasingMode.EaseInOut },
                AutoReverse = true
            };

            this.oneForm.BeginAnimation(OneFormVisual3D.MagnitudeProperty, animation);
        }

        private async void IntroduceArrows()
        {
            const int NumArrows = 17;
            const double Phase = 40;

            var mtrix = Matrix3D.Identity;
            var quaternion = new Quaternion(new Vector3D(0, 0, 1), Phase);
            mtrix.Rotate(quaternion);

            for (var i = 0; i < NumArrows; i++)
            {
                Material material;
                switch (i % 2)
                {
                    case 0:
                    case 1:
                        material = CosmosMaterials.Material9;
                        break;
                    default:
                        throw new Exception();
                }

                var ellipseArrow = new ArrowVisual3D
                {
                    Material = material,
                    Diameter = 0.8
                };

                this.view.MyViewPort.Children.AddIfMissing(ellipseArrow);

                var x = 30 * Math.Cos(2 * Math.PI * i / NumArrows);
                var y = 15 * Math.Sin(2 * Math.PI * i / NumArrows);
                var point3D = new Point3D(x, y, 0);
                point3D = mtrix.Transform(point3D);

                var task = ellipseArrow.PointTo(point3D, TimeSpan.FromSeconds(0.5));

                await Task.Delay(TimeSpan.FromMilliseconds(60));
            }
        }

        private void ToggleVisual(Visual3D visual, bool isChecked)
        {
            if (isChecked)
                this.view.MyViewPort.Children.InsertIfMissing(visual);
            else
                this.view.MyViewPort.Children.RemoveIfPresent(visual);
        }
    }

    public static class ArrowMoveExtensions
    {
        public static Task<ArrowVisual3D> PointTo(this ArrowVisual3D arrow, Point3D point)
        {
            return arrow.PointTo(point, TimeSpan.FromSeconds(1));
        }

        public static Task<ArrowVisual3D> PointTo(this ArrowVisual3D arrow, Point3D point, TimeSpan timeSpan)
        {
            var startPoint = arrow.Point2;

            var pointAnimation2 = new Point3DAnimation(startPoint, point, new Duration(timeSpan))
            {
                AccelerationRatio = 0.3,
                DecelerationRatio = 0.5,
                FillBehavior = FillBehavior.HoldEnd,
                AutoReverse = false
            };

            var tcs = new TaskCompletionSource<ArrowVisual3D>();
            pointAnimation2.Completed += (s, e) => tcs.SetResult(arrow);
            arrow.BeginAnimation(ArrowVisual3D.Point2Property, pointAnimation2);

            return tcs.Task;
        }
    }
}