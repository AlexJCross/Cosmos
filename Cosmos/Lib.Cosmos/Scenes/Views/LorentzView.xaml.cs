namespace Lib.Cosmos.Scenes.Views
{
    using Animation;
    using HelixToolkit.Wpf;
    using Visual3Ds;
    using Prism.Regions;
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Media.Animation;
    using System.Windows.Media.Media3D;
    using Cosmos.ViewModels;
    using Extensions;
    using Infrastructure;

    public class LorentzViewController
    {
        private readonly LorentzView view;

        public LorentzViewController(LorentzView view)
        {
            this.view = view;
        }

        private void ToggleSphericalMesh(bool isChecked)
        {
            this.ToggleVisual(this.view.MySphericals, isChecked);
        }

        private void TogglePolars(bool isChecked)
        {
            this.ToggleVisual(this.view.MyTube, isChecked);
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

        private void EnterSphere()
        {
            this.view.MySphericals.EnterGrow();
        }

        private void CreateTangentSpace()
        {
            var tangentPlane = new GridLinesVisual3D
            {
                Length = 24,
                Width = 24,
                MinorDistance = 1,
                Material = this.view.MajorGrid.Material
            };

            tangentPlane.Model.Geometry.Freeze();
            tangentPlane.Model.Material.Freeze();
            tangentPlane.Model.Freeze();

            var arrowAxisX = new FlatArrowVisual3D
            {
                Point1 = new Point3D(-12, 0, 0),
                Point2 = new Point3D(12, 0, 0),
                Material = this.view.arrow4.Material,
                BackMaterial = null,
                Width = 0.2,
                HeadLength = 1,
            };

            var arrowAxisY = new FlatArrowVisual3D
            {
                Point1 = new Point3D(0, -12, 0),
                Point2 = new Point3D(0, 12, 0),
                Material = this.view.arrow4.Material,
                BackMaterial = null,
                Width = 0.2,
                HeadLength = 1,
            };

            arrowAxisX.Model.Freeze();
            arrowAxisY.Model.Freeze();

            var zAxis = new Vector3D(0, 0, 1);
            var quaternion = new Quaternion(zAxis, 30);
            var rotation = new QuaternionRotation3D(quaternion);
            Transform3D arrowRotateTransform = new RotateTransform3D(rotation);

            int NumberArrows = 16;

            for (int i = 0; i < NumberArrows; i++)
            {
                double theta = i * (360d / NumberArrows);

                var arrow = new FlatArrowVisual3D
                {
                    Point1 = new Point3D(0, 0, 0),
                    Point2 = new Point3D(8 * Math.Cos(theta * Math.PI / 180), 4 * Math.Sin(theta * Math.PI / 180), 0),
                    Material = this.view.arrow2.Material,
                    BackMaterial = null,
                    Width = 0.3,
                    HeadLength = 1,
                    Transform = arrowRotateTransform
                };

                this.view.TangentSpace.Children.Add(arrow);
            }



            var arrowAnimation2 = new Point3DAnimationUsingKeyFrames()
            {
                // AccelerationRatio = 0.5,
                // DecelerationRatio = 0.5,
                FillBehavior = FillBehavior.HoldEnd,
                AutoReverse = true,
                RepeatBehavior = RepeatBehavior.Forever
            };

            var points = new[] { new Point3D(5, 6, 0), new Point3D(2, -8, 0), new Point3D(-5, -3, 0), new Point3D(5, -7, 0) };

            for (int i = 0; i < points.Length; i++)
            {
                Point3DKeyFrame point3DKeyFrame = new SplinePoint3DKeyFrame
                {
                    KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(i * 2)),
                    Value = points[i],
                    KeySpline = new KeySpline(0.8, 0.2, 0.2, 0.8)
                };

                arrowAnimation2.KeyFrames.Add(point3DKeyFrame);
            }

            // arrow.BeginAnimation(ArrowVisual3D.Point2Property, arrowAnimation2);

            this.view.TangentSpace.Children.Add(tangentPlane);
            this.view.TangentSpace.Children.Add(arrowAxisX);
            this.view.TangentSpace.Children.Add(arrowAxisY);
        }

        private void RotateTangentSpace()
        {
            // Drop it down the theta direction
            var axis = new Vector3D(1, 0, 0);
            var staticRotation = new AxisAngleRotation3D(axis, 60);
            var center = default(Point3D);

            // Then we rotate about the z axis
            var zAxis = new Vector3D(0, 0, 1);
            var rotation = new AxisAngleRotation3D(zAxis, 0);

            // Build the transform from the three individual transforms
            Transform3D linearShift = new TranslateTransform3D(0, 0, 20);
            Transform3D staticRotateTransform = new RotateTransform3D(staticRotation, center);
            Transform3D rotateTransform = new RotateTransform3D(rotation, center);

            var transformGroup = new Transform3DGroup();
            transformGroup.Children.Add(linearShift);
            transformGroup.Children.Add(staticRotateTransform);
            transformGroup.Children.Add(rotateTransform);

            this.view.TangentSpace.Transform = transformGroup;

            // Define an animation over the final transformation
            var angleAnimation = new DoubleAnimation(0, 360, new Duration(TimeSpan.FromSeconds(12)))
            {
                FillBehavior = FillBehavior.Stop,
                AutoReverse = false,
                RepeatBehavior = RepeatBehavior.Forever
            };

            rotation.BeginAnimation(AxisAngleRotation3D.AngleProperty, angleAnimation);
        }

        private void MoveKeyFrame()
        {
            var center = new Point3D(0, 0, 0);
            double angle = 0;
            var axis = new Vector3D(0, 0, 1);
            var rotation = new AxisAngleRotation3D(axis, angle);
            this.view.MyCamera.Transform = new RotateTransform3D(rotation, center);

            var keyFrameAnimation = new DoubleAnimationUsingKeyFrames
            {
                FillBehavior = FillBehavior.Stop,
                AutoReverse = true
            };

            var myEasingDoubleKeyFrame1 = new EasingDoubleKeyFrame
            {
                EasingFunction = new SineEase { EasingMode = EasingMode.EaseInOut },
                KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0)),
                Value = 0
            };

            var myEasingDoubleKeyFrame2 = new EasingDoubleKeyFrame
            {
                EasingFunction = new SineEase { EasingMode = EasingMode.EaseInOut },
                KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(1)),
                Value = 100
            };

            var myEasingDoubleKeyFrame3 = new EasingDoubleKeyFrame
            {
                EasingFunction = new SineEase { EasingMode = EasingMode.EaseInOut },
                KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(2)),
                Value = 80
            };

            var myEasingDoubleKeyFrame4 = new EasingDoubleKeyFrame
            {
                EasingFunction = new SineEase { EasingMode = EasingMode.EaseInOut },
                KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(3)),
                Value = 180
            };

            var myEasingDoubleKeyFrame5 = new EasingDoubleKeyFrame
            {
                KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(4)),
                Value = 160
            };

            keyFrameAnimation.KeyFrames.Add(myEasingDoubleKeyFrame1);
            keyFrameAnimation.KeyFrames.Add(myEasingDoubleKeyFrame2);
            keyFrameAnimation.KeyFrames.Add(myEasingDoubleKeyFrame3);
            keyFrameAnimation.KeyFrames.Add(myEasingDoubleKeyFrame4);
            keyFrameAnimation.KeyFrames.Add(myEasingDoubleKeyFrame5);

            rotation.BeginAnimation(AxisAngleRotation3D.AngleProperty, keyFrameAnimation);
        }

        private void MoveLorentz()
        {
            var center = new Point3D(0, 0, 0);
            double angle = 0;
            var axis = new Vector3D(0, 0, 1);
            var rotation = new AxisAngleRotation3D(axis, angle);
            this.view.MyCamera.Transform = new RotateTransform3D(rotation, center);

            var a1 = new DoubleAnimation(0, 45 + 90, new Duration(TimeSpan.FromMilliseconds(1000)))
            {
                AccelerationRatio = 0.3,
                DecelerationRatio = 0.5,
                FillBehavior = FillBehavior.HoldEnd,
                AutoReverse = false
            };


            a1.Completed += (s, a) =>
            {
                const double side = 40;



                this.view.MyViewPort.SetView(
                    new Point3D(0, -side, this.view.MyCamera.Position.Z + 10),
                    new Vector3D(0, side, -this.view.MyCamera.Position.Z - 10),
                    new Vector3D(0, 0, 1),
                    1000);
            };

            rotation.BeginAnimation(AxisAngleRotation3D.AngleProperty, a1);
        }

        private void SimulateLorentzTransform()
        {
            var matrixAnimation = new LorentzAnimation(0, 0.4, new Duration(TimeSpan.FromMilliseconds(5000)))
            {
                AccelerationRatio = 0.3,
                DecelerationRatio = 0.5,
                FillBehavior = FillBehavior.HoldEnd,
                AutoReverse = true
            };

            this.view.MajorGrid.BeginAnimation(GridLinesVisual3D.TransformProperty, matrixAnimation);
        }

        public IList<ISceneClip> CreateSceneClips()
        {
            var togglePolars = new ToggleSceneClip("Polars", this.TogglePolars);
            var toggleSphericalMesh = new ToggleSceneClip("Sphere", this.ToggleSphericalMesh);
            var toggleSphereFill = new ToggleSceneClip("Sphere fill", b => this.view.SphereFill.Visible = b);

            var toggleMajorGrid = new ToggleSceneClip("Major Grid", b => this.view.MajorGrid.Visible = b);
            var toggleMinorGrid = new ToggleSceneClip("Minor Grid", b => this.view.MinorGrid.Visible = b);
            var togglePhotonLines = new ToggleSceneClip("Photon lines", b => 
                {
                    this.view.PhotonPlus.Visible = b;
                    this.view.PhotonMinus.Visible = b;
                });

            return new List<ISceneClip>
            {
                new SceneClip("Lorentz", this.SimulateLorentzTransform),
                new SceneClip("Move Lorentz", this.MoveLorentz),
                new SceneClip("Move Key Frame", this.MoveKeyFrame),
                new SceneClip("Rotate tangent", () =>
                    {
                        this.CreateTangentSpace();
                        this.RotateTangentSpace();
                    }),
                new SceneClip("Enter Polars", () =>
                {
                    togglePolars.Value = true;
                    this.view.MyTube.EnterGrow();
                }),
                new SceneClip("Enter sphere", () =>
                {
                    toggleSphericalMesh.Value = true;
                    this.view.MySphericals.EnterGrow();
                }),
                new SceneClip("Enter Both", () =>
                {
                    togglePolars.Value = true;
                    toggleSphericalMesh.Value = true;
                    this.view.MyTube.EnterGrow();
                    this.view.MySphericals.EnterGrow();
                }),
                new SceneClip("Exit sphere", () =>
                {
                    this.view.MySphericals.ExitShrink(() => toggleSphericalMesh.Value = false);
                }),

                toggleMajorGrid,
                toggleMinorGrid,
                togglePhotonLines,
                togglePolars,
                toggleSphericalMesh,
                toggleSphereFill,
            };
        }
    }

    /// <summary> Interaction logic for LorentzView.xaml </summary>
    public partial class LorentzView : IRegionMemberLifetime
    {
        public LorentzView()
        {
            InitializeComponent();

            var builder = new PolarCoordinateBuilder();
            builder.WithTubeVisual(this.MyTube).Build();

            var sphereBuilder = new SphericalCoordinateBuilder();

            this.MySphericals.Content =
                sphereBuilder.WithShowLatitudes(true)
                             .WithShowLongitudes(true)
                             .WithRadius(20)
                             .Build();

            this.Loaded += (s, e) =>
            {
                var vm = (ISceneAware)this.DataContext;
                var controller = new LorentzViewController(this);

                vm.CameraViewModel = this.MyViewPort.ToCameraActions().AsViewModel();
                vm.SceneClips = controller.CreateSceneClips();
            };
        }

        public bool KeepAlive
        {
            get { return false; }
        }

        private static IEnumerable<Point3D> GeneratePolarLines()
        {
            const int Points = 60;
            const int BaseRadius = 10;

            for (int j = 1; j < 20; j++)
            {
                var numPoints = Points + (2 * j);// * j;

                for (int i = 0; i <= numPoints; i++)
                {
                    var pt = new Point3D(
                        j * BaseRadius * Math.Cos(i * Math.PI * 2 / numPoints),
                        j * BaseRadius * Math.Sin(i * Math.PI * 2 / numPoints),
                        0);

                    yield return pt;

                    if (i > 0 && i < numPoints)
                    {
                        yield return pt;
                    }
                }
            }

            const int Radius = 200;

            for (int i = 0; i < 180; i += 15)
            {
                var pt1 = new Point3D(Radius * Math.Cos(i * Math.PI / 180), Radius * Math.Sin(i * Math.PI / 180), 0);
                var pt2 = new Point3D(Radius * Math.Cos((i * Math.PI / 180) + Math.PI), Radius * Math.Sin((i * Math.PI / 180) + Math.PI), 0);
                yield return pt1;
                yield return pt2;
                yield return pt1;
            }
        }
    }
}
