namespace Lib.Cosmos.Scenes.Views
{
    using Animation;
    using HelixToolkit.Wpf;
    using Visual3Ds;
    using Prism.Regions;
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media.Animation;
    using System.Windows.Media.Media3D;

    /// <summary> Interaction logic for LorentzView.xaml </summary>
    public partial class LorentzView : IRegionMemberLifetime
    {
        public LorentzView()
        {
            InitializeComponent();

            var builder = new PolarCoordinateBuilder();
            builder.WithTubeVisual(this.MyTube).Build();
            this.TogglePolarTubes(null, null);

            var sphereBuilder = new SphericalCoordinateBuilder();

            this.MySphericals.Content =
                sphereBuilder.WithShowLatitudes(true)
                             .WithShowLongitudes(true)
                             .WithRadius(20)
                             .Build();

            var blackHoleBuilder = new EmbeddedBlackHoleBuilder();
            this.MyBlackHole.Content = blackHoleBuilder.WithMass(2d).WithRingRadius(5).Build();

            this.TogglePolarTubes(null, null);
            this.ToggleSphere(null, null);
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.SnapMove();
        }

        private void Button_Glide(object sender, RoutedEventArgs e)
        {
            this.MoveGradually();
        }

        private void Button_Reset(object sender, RoutedEventArgs e)
        {
            this.MyCamera.Transform = null;
            this.MyViewPort.ResetCamera();
        }

        private void ButtonResetGlide(object sender, RoutedEventArgs e)
        {
            this.MoveToSide();
        }

        private void ButtonOrbit(object sender, RoutedEventArgs e)
        {
            this.OrbitThenMove();
        }

        private void ButtonKeyFrame(object sender, RoutedEventArgs e)
        {
            this.MoveKeyFrame();
        }

        private void SnapMove()
        {
            var position = this.MyViewPort.Camera.Position;
            var newPosition = new Point3D(position.X, position.Y, position.Z - 10);
            this.MyViewPort.Camera.Position = newPosition;
        }

        private void MoveGradually()
        {
            var position = this.MyViewPort.Camera.Position;
            var newPosition = new Point3D(position.X, position.Y, position.Z - 20);
            this.MyViewPort.SetView(newPosition, this.MyViewPort.Camera.LookDirection, this.MyViewPort.Camera.UpDirection, 2000);
        }

        private void OrbitThenMove()
        {
            var center = new Point3D(0, 0, 0);
            double angle = 0;
            var axis = new Vector3D(0, 0, 1);
            var rotation = new AxisAngleRotation3D(axis, angle);
            this.MyCamera.Transform = new RotateTransform3D(rotation, center);

            var a1 = new DoubleAnimation(0, 360, new Duration(TimeSpan.FromMilliseconds(4000)))
            {
                AccelerationRatio = 0.3,
                DecelerationRatio = 0.5,
                FillBehavior = FillBehavior.Stop,
                AutoReverse = true
            };

            a1.Completed += (s, a) =>
            {
                this.MyViewPort.SetView(
                    new Point3D(0, 0, 120),
                    new Vector3D(0, 0, -120),
                    new Vector3D(-0.5, -0.5, 0), // This is really important
                    1000);
            };

            rotation.BeginAnimation(AxisAngleRotation3D.AngleProperty, a1);
        }

        private void MoveToSide()
        {
            const double side = 52;

            this.MyViewPort.SetView(
                new Point3D(side, side, 80),
                new Vector3D(-side, -side, -80),
                new Vector3D(0, 0, 1),
                1000);
        }

        private void MoveKeyFrame()
        {
            var center = new Point3D(0, 0, 0);
            double angle = 0;
            var axis = new Vector3D(0, 0, 1);
            var rotation = new AxisAngleRotation3D(axis, angle);
            this.MyCamera.Transform = new RotateTransform3D(rotation, center);

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

        private void ButtonLorentz(object sender, RoutedEventArgs e)
        {
            var matrixAnimation = new LorentzAnimation(0, 0.4, new Duration(TimeSpan.FromMilliseconds(5000)))
            {
                AccelerationRatio = 0.3,
                DecelerationRatio = 0.5,
                FillBehavior = FillBehavior.HoldEnd,
                AutoReverse = true
            };

            this.MyGrid.BeginAnimation(GridLinesVisual3D.TransformProperty, matrixAnimation);
        }

        private void MoveLorentz(object sender, RoutedEventArgs e)
        {
            var center = new Point3D(0, 0, 0);
            double angle = 0;
            var axis = new Vector3D(0, 0, 1);
            var rotation = new AxisAngleRotation3D(axis, angle);
            this.MyCamera.Transform = new RotateTransform3D(rotation, center);

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



                this.MyViewPort.SetView(
                    new Point3D(0, -side, this.MyCamera.Position.Z + 10),
                    new Vector3D(0, side, -this.MyCamera.Position.Z - 10),
                    new Vector3D(0, 0, 1),
                    1000);
            };

            rotation.BeginAnimation(AxisAngleRotation3D.AngleProperty, a1);
        }

        private void CheckPolars_Click(object sender, RoutedEventArgs e)
        {
            var check = (CheckBox)sender;

            if ((bool)check.IsChecked)
            {
                var points = new Point3DCollection(GeneratePolarLines());
                // points.Freeze();
                this.PolarLines.Points = points;
            }
            else
            {
                this.PolarLines.Points.Clear();
            }
        }

        private void EnterPolar(object sender, RoutedEventArgs e)
        {
            this.CheckPolarMesh.IsChecked = true;
            this.TogglePolarTubes(null, null);

            this.MyTube.EnterGrow();
        }

        private void TogglePolarTubes(object sender, RoutedEventArgs e)
        {
            if ((bool)this.CheckPolarMesh.IsChecked)
            {
                if (!this.MyViewPort.Children.Contains(this.MyTube))
                {
                    this.MyViewPort.Children.Insert(0, this.MyTube);
                }
            }
            else
            {
                if (this.MyViewPort.Children.Contains(this.MyTube))
                {
                    this.MyViewPort.Children.Remove(this.MyTube);
                }
            }
        }

        private void EnterSphere(object sender, RoutedEventArgs e)
        {
            this.CheckSphere.IsChecked = true;
            this.ToggleSphere(null, null);

            this.MySphericals.EnterGrow();
        }

        private void ToggleSphere(object sender, RoutedEventArgs e)
        {
            if ((bool)this.CheckSphere.IsChecked)
            {
                if (!this.MyViewPort.Children.Contains(this.MySphericals))
                {
                    this.MyViewPort.Children.Insert(0, this.MySphericals);
                }
            }
            else
            {
                if (this.MyViewPort.Children.Contains(this.MySphericals))
                {
                    this.MyViewPort.Children.Remove(this.MySphericals);
                }
            }
        }

        private void EnterPolarPlusSphere(object sender, RoutedEventArgs e)
        {
            this.EnterPolar(null, null);
            this.EnterSphere(null, null);
        }

        private void ExitSphere(object sender, RoutedEventArgs e)
        {
            this.MySphericals.ExitShrink(() =>
            {
                this.CheckSphere.IsChecked = false;
                this.ToggleSphere(null, null);
            });
        }

        private void CreateTangentSpace()
        {
            var tangentPlane = new GridLinesVisual3D
            {
                Length = 24,
                Width = 24,
                MinorDistance = 1,
                Material = this.MyGrid.Material
            };

            /*
            <ht:SunLight x:Name="MyLight"
                                     Altitude="-90"
                                     Ambient="0.9"
                                     Brightness="0.5"
                                     ShowLights="False" />
            

            var sunLight = new SunLight
            {
                Altitude = 90,
                Ambient = 0.9,
                Brightness = 0.5,
                ShowLights = false,
            };
            */

            tangentPlane.Model.Geometry.Freeze();
            tangentPlane.Model.Material.Freeze();
            tangentPlane.Model.Freeze();

            var arrowAxisX = new FlatArrowVisual3D
            {
                Point1 = new Point3D(-12, 0, 0),
                Point2 = new Point3D(12, 0, 0),
                Material = this.arrow4.Material,
                BackMaterial = null,
                Width = 0.2,
                HeadLength = 1,
            };

            var arrowAxisY = new FlatArrowVisual3D
            {
                Point1 = new Point3D(0, -12, 0),
                Point2 = new Point3D(0, 12, 0),
                Material = this.arrow4.Material,
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
                    Material = this.arrow2.Material,
                    BackMaterial = null,
                    Width = 0.3,
                    HeadLength = 1,
                    Transform = arrowRotateTransform
                };

                this.TangentSpace.Children.Add(arrow);
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

            this.TangentSpace.Children.Add(tangentPlane);
            this.TangentSpace.Children.Add(arrowAxisX);
            this.TangentSpace.Children.Add(arrowAxisY);
        }

        private void RotateTangent(object sender, RoutedEventArgs e)
        {
            this.CreateTangentSpace();
            this.RotateTangentSpace();
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

            this.TangentSpace.Transform = transformGroup;

            // Define an animation over the final transformation
            var angleAnimation = new DoubleAnimation(0, 360, new Duration(TimeSpan.FromSeconds(12)))
            {
                FillBehavior = FillBehavior.Stop,
                AutoReverse = false,
                RepeatBehavior = RepeatBehavior.Forever
            };

            rotation.BeginAnimation(AxisAngleRotation3D.AngleProperty, angleAnimation);
        }
    }
}
