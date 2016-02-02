using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using System.Drawing;
using System.Xml;

namespace RPGSimAlpha
{
    class Cluster
    {
        public void Update(View view,ref RectangleF windowFrame)
        {
            Leader.Update(this,view);
            positionTopLeft += Velocity;
            CurrentRotation += Rotation;
            if (CurrentRotation >= 6.28319)
            {
                CurrentRotation -= 6.28319f;
            }
            else if(CurrentRotation <= -6.28319)
            {
                CurrentRotation += 6.28319f;
            }
            RefreshTooFarAway(view,ref windowFrame);
            if (!TooFarAway)
            {
                RefreshPreciseLocation();
            }
            //if (CurrentRotation > 8) { CurrentRotation = 1; }
            //Console.WriteLine("UpdatedCluster");
        }
        private void RefreshTooFarAway(View view,ref RectangleF windowFrame,bool forceRender = false)
        {
            if (forceRender)
            {
                //TooFarAway = false;
                return;
            }
            Vector2 position = ((PositionTopLeft + (CenterOfMass / View.MaxTextureSize)) * view.CurrentTextureSize);
            RectangleF renderFrame = new RectangleF(
                (position.X - ((Width / 2) / view.Zoom)), (position.Y - ((Height / 2) / view.Zoom)),
                (Width / view.Zoom), (Height / view.Zoom));
            if (windowFrame.IntersectsWith(renderFrame))
            {
                //Console.WriteLine(position);
                //Console.WriteLine();
                //Console.WriteLine(renderFrame);
                TooFarAway = false;
            }
            else
            {
                TooFarAway = true;
            }
        }
        public void Draw(View view,ref RectangleF windowFrame)
        {
            if (!TooFarAway)
            {
                SpriteBatch.Draw(Grid, PositionTopLeft,ref windowFrame, view, -CurrentRotation);
            }
        }
        public Vector2 CenterOfMass { get; private set; }
        /// <summary>
        /// will not render or update the precise location
        /// </summary>
        private bool TooFarAway { get; set; }
        public float Rotation { get; private set; }
        public float CurrentRotation { get; private set; }
        private Controller Leader { get; }
        public Vector2 Velocity => velocity;
        private Vector2 velocity;
        private Vector2 positionTopLeft;
        public Vector2 PositionTopLeft => positionTopLeft;
        public enum Shape : byte
        {
            Circle = 0,
            Rectangle,
        }
        public Cluster(Shape shape,Resources.BlockRegistry.BlockTypes type,Vector2 positionTopLeft,Vector2 velocity,float rotation, short valueA,short valueB,Controller.ControlType leader)
        {
            Leader = new Controller(leader,this);
            Rotation = rotation;
            this.velocity = velocity;
            bool circle = false;
            this.positionTopLeft = positionTopLeft;
            short[] centerCoords = new short[2];
            if(shape == Shape.Circle)
            {
                circle = true;
                centerCoords = new short[2]
                {
                    valueA,
                    valueB,
                };
                centerCoords[0] /= 2;
                centerCoords[1] /= 2;
            }
            Grid = new Block[valueA, valueB];
            for(short i = 0;i<valueA;i++)
            {
                for(short j = 0;j< valueB;j++)
                {
                    if(circle)
                    {
                        if (Resources.Helper.GetVector(centerCoords, new short[] { i, j }) > valueA)
                            continue;
                    }
                    Grid[i, j] = new Block(type);
                }
            }
            RefreshMass();
            RefreshThrust();
            RefreshCenterOfMass();
            RefreshRenderFrame(1,16);
            Radius = (float)Math.Sqrt(Math.Pow(Height, 2) + Math.Pow(Width, 2));
        }
        public Cluster(Block[,] grid,Vector2 positionTopLeft, Vector2 velocity, float rotation,Controller.ControlType leader)
        {
            Leader = new Controller(leader,this);
            Grid = grid;
            Rotation = rotation;
            this.velocity = velocity;
            this.positionTopLeft = positionTopLeft;
            short[] centerCoords = new short[2];
            RefreshMass();
            RefreshThrust();
            RefreshCenterOfMass();
            RefreshRenderFrame(1,16);
            Radius = (float)Math.Sqrt(Math.Pow(Height, 2) + Math.Pow(Width, 2));
        }
        public uint ClusterId { get; }
        private Block[,] Grid { get; }
        public Block this[int x, int y]
        {
            get
            {
                return Grid[x, y];
            }
            set
            {
                Grid[x, y] = value;
            }
        }
        public int Width
        {
            get
            {
                return Grid.GetLength(0);
            }
        }
        public int Height
        {
            get
            {
                return Grid.GetLength(1);
            }
        }
        private void RefreshPreciseLocation()
        {
            for (short x = 0; x < Grid.GetLength(0); x++)
            {
                for (short y = 0; y < Grid.GetLength(1); y++)
                {
                    //about half
                    //getting base values
                    float X = x - CenterOfMass.X;
                    float Y = y - CenterOfMass.Y;
                    float distance = (float)Math.Sqrt((X * X) + (Y * Y));
                    //converting to polar coordinates
                    double theta = Math.Atan(Y / X);
                    //if(theta == double.NaN) { theta = 0;Console.WriteLine("Found NaN"); }
                    //if (theta / theta != 1 && theta != 0) { theta = 0; }
                    theta += CurrentRotation;
                    if (X < 0 || Y < 0)
                    {
                        distance *= -1;
                        if (X < 0 && Y < 0 && 8 == 15)
                        { theta = Resources.Helper.GetPositive(theta); }
                    }
                    if (X >= 0 && Y < 0)
                    {
                        distance = -distance;
                        //theta = Resources.Helper.GetPositive(theta);
                    }
                    //converting to cartesian coordinates
                    Grid[x,y].SetPreciseLocation(new Vector2(
                        distance * (float)Math.Cos(theta),
                        distance * (float)Math.Sin(theta)
                        ));
                }
            }
        }
        private void RefreshCenterOfMass()
        {
            double[] centerOfMass = new double[2];
            for(short i = 0;i<Width;i++)
            {
                for(short j = 0;j<Height;j++)
                {
                    if (Grid[i, j].Mass != 0) 
                    {
                        centerOfMass[0] += Grid[i, j].Mass * i;
                        centerOfMass[1] += Grid[i, j].Mass * j;
                    }
                }
            }
            centerOfMass[0] /= Mass;
            centerOfMass[1] /= Mass;
            CenterOfMass = new Vector2((float)centerOfMass[0], (float)centerOfMass[1]);
            Console.WriteLine(CenterOfMass[0] + " | " + CenterOfMass[1]);
        }
        /// <summary>
        /// kiloNewtons
        /// </summary>
        private float Thrust { get; set; }
        /// <summary>
        /// kiloGrams
        /// </summary>
        private double Mass { get; set; }
        private RectangleF RenderFrame { get; set; }
        private float Radius { get; }
        private void RefreshRenderFrame(float zoom,byte textureSize)
        {
            Vector2 position = positionTopLeft * textureSize;
            RenderFrame = new RectangleF(
                (position.X - ((Width / 2) / (float)zoom)), (position.Y - ((Height / 2) / (float)zoom)),
                Width / (float)zoom, Height / (float)zoom);
        }
        private void RefreshMass()
        {
            Mass = 0;
            for (short i = 0; i < Width; i++)
            {
                for (short j = 0; j < Height; j++)
                {
                    Mass += Grid[i, j].Mass;
                }
            }
        }
        private void RefreshThrust()
        {
            Thrust = 0;
            for (short i = 0; i < Width; i++)
            {
                for (short j = 0; j < Height; j++)
                {
                    if (Grid[i, j].Type == Resources.BlockRegistry.BlockTypes.ThrusterIon)
                        Thrust += Resources.Physics.Thrust; 
                }
            }
        }
        private bool InView(RectangleF viewFrame) => viewFrame.IntersectsWith(new RectangleF(PositionTopLeft.X, PositionTopLeft.Y, Width, Height));
        public void AccelerateForward()
        {
            velocity.X += (float)((Thrust * Math.Sin(CurrentRotation))/(Mass));
            velocity.Y -= (float)((Thrust * Math.Cos(CurrentRotation))/(Mass));
        }
        /// <summary>
        /// placeHolder
        /// </summary>
        /// <param name="direction"></param>
        public void AccelerateTorque(sbyte direction)
        {
            if ((Rotation < -Resources.Physics.MaxTorque && direction < 0)||
                (Rotation > Resources.Physics.MaxTorque && direction > 0))
                return;
            Rotation += (float)(Resources.Helper.GetSign(direction)/Mass);
        }
        /// <summary>
        /// accelerates the torque in the direction that will slow it down. 
        /// </summary>
        public void ResetTorque()
        {
            if(CurrentRotation > 0)
                AccelerateTorque(-1);
            else if(CurrentRotation < 0)
                AccelerateTorque(1);
        }
    }
}
