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
        public void Update(View view)
        {
            Leader.Update(this,view);
            positionTopLeft += Velocity;
            CurrentRotation += Rotation;
            if(CurrentRotation >= 6.28319)
            {
                CurrentRotation -= 6.28319f;
            }
            //if (CurrentRotation > 8) { CurrentRotation = 1; }
            //Console.WriteLine("UpdatedCluster");
        }
        public Vector2 CenterOfMass { get; private set; }
        public float Rotation { get; private set; }
        private float CurrentRotation { get; set; }
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
            bool done = false;
            for(short i = 0;i<valueA;i++)
            {
                for(short j = 0;j< valueB;j++)
                {
                    if(circle)
                    {
                        if (Resources.Helper.GetVector(centerCoords, new short[] { i, j }) > valueA)
                            continue;
                    }
                    if(!done)
                    {
                        Grid[i, j] = new Block(Resources.BlockRegistry.BlockTypes.Ladder, i, j);
                        done = true;
                    }
                    else
                    {
                        Grid[i, j] = new Block(type, i, j);
                        done = false;
                    }
                }
            }
            RefreshMass();
            RefreshThrust();
            RefreshCenterOfMass();
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
        public void Draw(RectangleF windowFrame,byte textureSize)
        {
            for (short x = 0; x < Grid.GetLength(0); x++)
            {
                for (short y = 0; y < Grid.GetLength(1); y++)
                {
                    Draw(Grid[x,y], CalculateRotaion(Grid[x,y],x,y),windowFrame, textureSize);
                }
            }
        }
        private void Draw(Block block,Vector2 localPosition,RectangleF windowFrame,byte textureSize)
        {
            Texture2D texture = Resources.IO.GetTexture(block.Type, textureSize);
            Vector2 currentPosition = (localPosition * textureSize) + (positionTopLeft * textureSize);
            if (windowFrame.IntersectsWith(new RectangleF(currentPosition.X, currentPosition.Y, Resources.IO.TextureSizeLarge * 1.5f, textureSize * 1.5f)))
                SpriteBatch.Draw(texture, currentPosition, new Vector2(1f), Color.White, Vector2.Zero, null, -CurrentRotation);
        }
        private Vector2 CalculateRotaion(Block block,short x,short y)
        {
            //about half
            //getting base values
            float X = x - CenterOfMass.X;
            float Y = y - CenterOfMass.Y;
            float distance = (float)Math.Sqrt((X * X) + (Y * Y));
            //converting to polar coordinates
            double theta = Math.Atan(Y / X);
            //if(theta == double.NaN) { theta = 0;Console.WriteLine("Found NaN"); }
            if (theta / theta != 1 && theta != 0) { theta = 0; }
            theta += CurrentRotation;
            if (X < 0 || Y < 0)
            {
                distance *= -1;
                if (X < 0 && Y < 0 && 8 == 15)
                { theta = Resources.Helper.GetPositive(theta); }
            }
            if (X >= 0 && Y < 0)
            {
                distance *= -1;
                //theta = Resources.Helper.GetPositive(theta);
            }
            Vector2 returnCoords;
            //converting to cartesian coordinates
            returnCoords = new Vector2(
                distance * (float)Math.Cos(theta),
                distance * (float)Math.Sin(theta)
                );
            return returnCoords;
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
        private double Mass { get; set; }
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
    }
}
