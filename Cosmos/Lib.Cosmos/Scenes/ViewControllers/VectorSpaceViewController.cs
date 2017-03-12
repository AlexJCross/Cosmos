using System.Windows.Controls;
using System.Windows.Data;
using Lib.Cosmos.Visual3Ds;

namespace Lib.Cosmos.Scenes.ViewControllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Media.Animation;
    using System.Windows.Media.Media3D;
    using HelixToolkit.Wpf;
    using Lib.Cosmos.Extensions;
    using Lib.Cosmos.Scenes.Infrastructure;
    using Lib.Cosmos.Scenes.Views;
    using Lib.Cosmos.Utils;

    public class VectorSpaceViewController : ISceneViewController
    {
        private readonly ArrowVisual3D arrow1;
        private readonly ArrowVisual3D arrow2;

        private readonly VectorSpaceView view;

        public VectorSpaceViewController(VectorSpaceView view)
        {
            this.view = view;

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
        }

        public IList<ISceneClip> CreateSceneClips()
        {
            var toggleArrow1 = new ToggleSceneClip("Arrow 1", isChecked => {
                this.ToggleVisual(this.arrow1, isChecked);
            });

            var toggleArrow2 = new ToggleSceneClip("Arrow 2", isChecked => {
                this.ToggleVisual(this.arrow2, isChecked);
            });

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

                toggleArrow1,
                toggleArrow2,

            };
        }

        private async void IntroduceArrow1()
        {
            this.arrow1.Point1 = default(Point3D);
            await this.arrow1.PointTo(new Point3D(30, 15, 0), TimeSpan.FromSeconds(0.5));
        }

        private async void IntroduceArrow2()
        {
            this.arrow2.Point1 = default(Point3D);

            var parallelogram = new ParallelogramVisual3D
            {
                Point1 = this.arrow1.Point2,
                Point2 = new Point3D(10, 25, 0),
                Material = CosmosMaterials.Material8,
                BackMaterial = CosmosMaterials.Material8,
            };

            Binding binding1 = new Binding("Point2")
            {
                Source = this.arrow1
            };

            Binding binding2 = new Binding("Point2")
            {
                Source = this.arrow2
            };

            BindingOperations.SetBinding(parallelogram, ParallelogramVisual3D.Point1Property, binding1);
            BindingOperations.SetBinding(parallelogram, ParallelogramVisual3D.Point2Property, binding2);


            this.view.MyViewPort.Children.Add(parallelogram);

            await this.arrow2.PointTo(new Point3D(10, 25, 0), TimeSpan.FromSeconds(0.5));
        }

        private async void MoveArrows()
        {
            await this.arrow2.PointTo(new Point3D(-10, 15, 0));

            await Task.Delay(200);

            await this.arrow1.PointTo(new Point3D(-10, -15, 0));
        }

        private async void IntroduceArrows()
        {
            const int NumArrows = 17;
            const double Phase = 40;

            var mtrix = Matrix3D.Identity;
            var quaternion = new Quaternion(new Vector3D(0, 0, 1), Phase);
            mtrix.Rotate(quaternion);

            for (int i = 0; i < NumArrows; i++)
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

                var pointAnimation = new Point3DAnimation(new Point3D(2, 2, 0), point3D, new Duration(TimeSpan.FromSeconds(0.5)))
                {
                    AccelerationRatio = 0.3,
                    DecelerationRatio = 0.5,
                    FillBehavior = FillBehavior.HoldEnd,
                    AutoReverse = false,
                };

                ellipseArrow.BeginAnimation(ArrowVisual3D.Point2Property, pointAnimation);

                await Task.Delay(TimeSpan.FromMilliseconds(60));
            }
        }

        private void ToggleVisual(Visual3D visual, bool isChecked)
        {
            if (isChecked)
            {
                this.view.MyViewPort.Children.InsertIfMissing(visual);
            }
            else
            {
                this.view.MyViewPort.Children.RemoveIfPresent(visual);
            }
        }
    }

    public static class ArrowMoveExtensions
    {
        public static Task PointTo(this ArrowVisual3D arrow, Point3D point)
        {
            return arrow.PointTo(point, TimeSpan.FromSeconds(1));
        }

        public static Task PointTo(this ArrowVisual3D arrow, Point3D point, TimeSpan timeSpan)
        {
            var startPoint = arrow.Point2;

            var pointAnimation2 = new Point3DAnimation(startPoint, point, new Duration(timeSpan))
            {
                AccelerationRatio = 0.3,
                DecelerationRatio = 0.5,
                FillBehavior = FillBehavior.HoldEnd,
                AutoReverse = false,
            };

            var tcs = new TaskCompletionSource<int>();
            pointAnimation2.Completed += (s, e) => tcs.SetResult(0);
            arrow.BeginAnimation(ArrowVisual3D.Point2Property, pointAnimation2);

            return tcs.Task;

        }
    }


}