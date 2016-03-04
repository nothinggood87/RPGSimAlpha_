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
        public void Update(View view,Galaxy gal,ref RectangleF windowFrame)
        {
            positionTopLeft += Velocity;
            CurrentRotation += Rotation;
            const float circle = 2 * (float)Math.PI;
            if (CurrentRotation >= circle)
            {
                CurrentRotation -= circle;
            }
            else if(CurrentRotation < 0)
            {
                CurrentRotation += circle;
            }
            RefreshTooFarAway(view,ref windowFrame);
            UpdateGalacticLocation(gal);
            if (!TooFarAway)
            {
                RefreshPreciseLocation();
            }
        }
        private void RefreshTooFarAway(View view,ref RectangleF windowFrame,bool forceRender = false)
        {
            if (forceRender)
            {
                //TooFarAway = false;
                return;
            }
            Vector2 position = ((PositionTopLeft + (CenterOfMass / (View.MaxTextureSize/View.MaxTextureSize)) - Velocity));
            RectangleF renderFrame = new RectangleF(
                (position.X - ((Width / 2) / view.Zoom)), (position.Y - ((Height / 2) / view.Zoom)),
                (Width / view.Zoom), (Height / view.Zoom));
            if (windowFrame.IntersectsWith(renderFrame))
            {
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
                SpriteBatch.Draw(Grid, PositionTopLeft, ref windowFrame, view, -CurrentRotation);
        }
        public Vector2 CenterOfMass { get; private set; }
        /// <summary>
        /// will not render or update the precise location
        /// </summary>
        private bool TooFarAway { get; set; }
        public float Rotation { get; private set; }
        public float CurrentRotation { get; private set; }
        public Vector2 Velocity;
        private Vector2 positionTopLeft;
        public Vector2 PositionTopLeft => positionTopLeft;
        public enum Shape : byte
        {
            Circle = 0,
            Rectangle,
        }
        public Cluster(Block[,] grid,Vector2 positionTopLeft, Vector2 velocity, float rotation,ushort[] sectorBlock,Galaxy gal)
        {
            
            Grid = grid;
            Rotation = rotation;
            this.Velocity = velocity;
            this.positionTopLeft = positionTopLeft;
            short[] centerCoords = new short[2];
            RefreshMass();
            RefreshCenterOfMass();
            RefreshRenderFrame(1,16);
            Radius = (float)Math.Sqrt(Math.Pow(Height, 2) + Math.Pow(Width, 2));
            CurrentSectorBlock = sectorBlock;
            CurrentSector = SectorBlock.GetNearestSectorCoords(ref positionTopLeft);
            gal.SetSectorBlock(this);
        }
        private void UpdateGalacticLocation(Galaxy gal)
        {
                bool OutOfSectorBounds = SectorBlock.OutOfSectorBounds(ref CurrentSector[0], ref CurrentSector[1], ref positionTopLeft);

            if (OutOfSectorBounds)
                gal.RefreshCurrentSector(this, ref positionTopLeft);
                  
        }
        public ushort[] CurrentSectorBlock { get; set; }
        public ushort[] CurrentSector { get; set; }
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
            float X , Y;
            float distance;
            double theta;
            short x , y;
            for (x = 0; x < Grid.GetLength(0); x++)
            {
                for (y = 0; y < Grid.GetLength(1); y++)
                {
                    //about half
                    //getting base values
                    X = x - CenterOfMass.X;
                    Y = y - CenterOfMass.Y;
                    distance = (float)Math.Sqrt((X * X) + (Y * Y));
                    if (X < 0)
                        distance = -distance;
                    //converting to polar coordinates
                    theta = Math.Atan(Y / X);
                    theta += CurrentRotation;
                    Grid[x, y].SetPreciseLocation(new Vector2(
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
                        centerOfMass[0] += Grid[i, j].Mass * (i + 1);
                        centerOfMass[1] += Grid[i, j].Mass * (j + 1);
                    }
                }
            }
            centerOfMass[0] /= Mass;
            centerOfMass[1] /= Mass;
            centerOfMass[0]--;
            centerOfMass[1]--;
            CenterOfMass = new Vector2((float)centerOfMass[0], (float)centerOfMass[1]);
            Console.WriteLine( "Center Of Mass = " + CenterOfMass[0] + " | " + CenterOfMass[1]);
        }
        /// <summary>
        /// kiloGrams
        /// </summary>
        public double Mass { get; private set; }
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
        /// <summary>
        /// placeHolder
        /// </summary>
        /// <param name="direction"></param>
        public void AccelerateTorque(sbyte direction)
        {
            if ((Rotation < -Resources.Physics.MaxTorque && direction < 0)||
                (Rotation > Resources.Physics.MaxTorque && direction > 0))
                return;
            Rotation += (float)(direction/Mass);
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
