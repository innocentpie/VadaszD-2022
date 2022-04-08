using System;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows;

namespace VersenyUI.DiceVisual
{
    class DiceVisual
    {
        public static readonly Point3D[] baseVertices =
        {
            // front
            new Point3D(-1, -1, 1),
            new Point3D(1, -1, 1),
            new Point3D(1, 1, 1),
            new Point3D(-1, 1, 1),
            // back
            new Point3D(-1, -1, -1),
            new Point3D(1, -1, -1),
            new Point3D(1, 1, -1),
            new Point3D(-1, 1, -1),
            // down
            new Point3D(-1, -1, -1),
            new Point3D(1, -1, -1),
            new Point3D(1, -1, 1),
            new Point3D(-1, -1, 1),
            // up
            new Point3D(-1, 1, -1),
            new Point3D(1, 1, -1),
            new Point3D(1, 1, 1),
            new Point3D(-1, 1, 1),
            // left
            new Point3D(-1, -1, -1),
            new Point3D(-1, -1, 1),
            new Point3D(-1, 1, 1),
            new Point3D(-1, 1, -1),
            // right
            new Point3D(1, -1, -1),
            new Point3D(1, -1, 1),
            new Point3D(1, 1, 1),
            new Point3D(1, 1, -1),

        };
        public static readonly Point[] baseUVs =
        {
            // front
            new Point(.25, .33333),
            new Point(.5, .33333),
            new Point(.5, .66666),
            new Point(.25, .66666),
            // back
            new Point(1, .33333),
            new Point(.75, .33333),
            new Point(.75, .66666),
            new Point(1, .66666),
            // down
            new Point(.25, 0),
            new Point(.5, 0),
            new Point(.5, .33333),
            new Point(.25, .33333),
            // up
            new Point(.25, 1),
            new Point(.5, 1),
            new Point(.5, .66666),
            new Point(.25, .66666),
            // left
            new Point(0, .33333),
            new Point(.25, .33333),
            new Point(.25, .66666),
            new Point(0, .66666),
            // right
            new Point(.75, .33333),
            new Point(.5, .33333),
            new Point(.5, .66666),
            new Point(.75, .66666),
        };
        public static readonly int[] baseIndicies =
        {
            // R: Reversed facing
            // front 0-1-2-3
            // back 4-5-6-7 R
            // down 8-9-10-11
            // up 12-13-14-15 R
            // left 16-17-18-19
            // right 20-21-22-23 R

            // front
            0,1,2,
            0,2,3,
            // back
            7,6,5,
            7,5,4,
            // down
            8,9,10,
            8,10,11,
            // up
            15,14,13,
            15,13,12,
            // left
            16,17,18,
            16,18,19,
            // right
            23,22,21,
            23,21,20,
        };
        public static readonly Vector3D[] baseNormals =
        {
            // front
            new Vector3D(0, 0, 1),
            new Vector3D(0, 0, 1),
            new Vector3D(0, 0, 1),
            new Vector3D(0, 0, 1),
            // back
            new Vector3D(0, 0, -1),
            new Vector3D(0, 0, -1),
            new Vector3D(0, 0, -1),
            new Vector3D(0, 0, -1),
            // down
            new Vector3D(0, -1, 0),
            new Vector3D(0, -1, 0),
            new Vector3D(0, -1, 0),
            new Vector3D(0, -1, 0),
            // up
            new Vector3D(0, 1, 0),
            new Vector3D(0, 1, 0),
            new Vector3D(0, 1, 0),
            new Vector3D(0, 1, 0),
            // left
            new Vector3D(1, 0, 0),
            new Vector3D(1, 0, 0),
            new Vector3D(1, 0, 0),
            new Vector3D(1, 0, 0),
            // right
            new Vector3D(-1, 0, 0),
            new Vector3D(-1, 0, 0),
            new Vector3D(-1, 0, 0),
            new Vector3D(-1, 0, 0),
        };

        private static readonly Random randomGen = new Random();

        public static readonly Quaternion[] sideRotations =
        {
            // Rotate down-right to look better
            new Quaternion(new Vector3D(1, 1, 0), 10),
            // 1
            new Quaternion(new Vector3D(1, 0, 0), 90),
            // 2
            Quaternion.Identity,
            // 3
            new Quaternion(new Vector3D(0, 1, 0), -90),
            // 4
            new Quaternion(new Vector3D(0, 1, 0), 90),
            // 5
            new Quaternion(new Vector3D(0, 1, 0), 180),
            // 6
            new Quaternion(new Vector3D(1, 0, 0), -90),
        };

        public readonly MatrixTransform3D transform;
        public readonly MeshGeometry3D mesh;
        public readonly DiffuseMaterial material;

        public readonly ModelVisual3D modelVisual;

        public int CurrentSide { get; private set; }

        public Vector3D Position
        {
            get => position;
            set => SetPosition(value);
        }
        private Vector3D position;


        public Vector3D Scale
        {
            get => scale;
            set => SetScale(value);
        }
        private Vector3D scale;

        public Quaternion Rotation
        {
            get => rotation;
            set => SetRotation(value);
        }
        private Quaternion rotation;

        public DiceVisual(Viewport3D vp, Vector3D position, Quaternion rotation, Vector3D scale)
        {
            mesh = new MeshGeometry3D();
            mesh.Positions = new Point3DCollection(baseVertices);
            mesh.TextureCoordinates = new PointCollection(baseUVs);
            mesh.Normals = new Vector3DCollection(baseNormals);
            mesh.TriangleIndices = new Int32Collection(baseIndicies);

            Matrix3D transformMatrix = Matrix3D.Identity;
            transformMatrix.Translate(position);
            transformMatrix.Rotate(rotation);
            transformMatrix.Scale(scale);
            transform = new MatrixTransform3D(transformMatrix);

            this.position = position;
            this.rotation = rotation;
            this.scale = scale;

            ImageBrush brush = new ImageBrush();
            brush.ImageSource = new BitmapImage(new Uri("pack://application:,,,/VersenyUI;component/diceTexture.png"));
            material = new DiffuseMaterial(brush);

            GeometryModel3D geomModel = new GeometryModel3D();
            geomModel.Geometry = mesh;
            geomModel.Transform = transform;
            geomModel.Material = material;

            modelVisual = new ModelVisual3D();
            modelVisual.Content = geomModel;
            vp.Children.Add(modelVisual);

            RollToInstant(1);
        }

        public void SetPosition(Vector3D position)
        {
            this.position = position;

            Matrix3D transformMatrix = Matrix3D.Identity;
            transformMatrix.Translate(position);
            transformMatrix.Rotate(rotation);
            transformMatrix.Scale(scale);
            transform.Matrix = transformMatrix;
        }

        public void SetScale(Vector3D scale)
        {
            this.scale = scale;

            Matrix3D transformMatrix = Matrix3D.Identity;
            transformMatrix.Translate(position);
            transformMatrix.Rotate(rotation);
            transformMatrix.Scale(scale);
            transform.Matrix = transformMatrix;
        }

        public void SetRotation(Quaternion rotation)
        {
            this.rotation = rotation;

            Matrix3D transformMatrix = Matrix3D.Identity;
            transformMatrix.Translate(position);
            transformMatrix.Rotate(rotation);
            transformMatrix.Scale(scale);
            transform.Matrix = transformMatrix;
        }

        private const int stepIntervalMs = 10;
        public async void RollToAnimate(int side, int timeFullMs, int toTargetRotationCount, int randomizedRotationCount)
        {
            int fullRotCount = toTargetRotationCount;
            int localRotCount = randomizedRotationCount;
            int stepCount = timeFullMs / stepIntervalMs;

            // Caculate direct rotation to target
            Quaternion invertedTarget = sideRotations[side];
            invertedTarget.Invert();
            Quaternion startToTargetDiff = sideRotations[CurrentSide] * invertedTarget;
            double diffAngle = startToTargetDiff.Angle;

            // Calculate global rotation angle by step
            double globalStepAngle = (fullRotCount * 360D - diffAngle) / stepCount;
            // Global rotater quaternion
            Quaternion globalRotater = new Quaternion(startToTargetDiff.Axis, globalStepAngle);

            Vector3D randomizer;

            // Get a randomizer vector
            randomizer = new Vector3D(randomGen.NextDouble() * 2D - 1D, randomGen.NextDouble() * 2D - 1D, randomGen.NextDouble() * 2D - 1D);
            if (randomizer.Length == 0)
                randomizer.X = 1;
            randomizer.Normalize();

            // Calculate local rotation (randomizer) angle by step
            double localStepAngle = localRotCount * 360D / stepCount;
            // Local rotater quaternion
            Quaternion localRotater = new Quaternion(randomizer, localStepAngle);

            // The start rotation without sideRotations[0] applied
            Quaternion rotationPlain = sideRotations[CurrentSide];

            for (int i = 0; i < stepCount; i++)
            {
                // Use sin [0, pi] for smoothing
                // Divide by Pi / 2 (since x [0; pi] intergral is pi, sin(x) [0, pi] intergral is 2)
                double currentAngleMultiplier = Math.PI / 2 * Math.Sin((double)i / stepCount * Math.PI);

                //rotationPlain = globalRotater * (rotationPlain * localRotater);
                rotationPlain = new Quaternion(globalRotater.Axis, currentAngleMultiplier * globalRotater.Angle) * (rotationPlain * new Quaternion(localRotater.Axis, currentAngleMultiplier * localRotater.Angle));
                Rotation = sideRotations[0] * rotationPlain;

                await Task.Delay(10);
            }
            // Ended animation, set to target
            Rotation = sideRotations[0] * sideRotations[side];
            CurrentSide = side;
        }

        public void RollToInstant(int side)
        {
            Rotation = sideRotations[0] * sideRotations[side];
            CurrentSide = side;
        }
    }
}
