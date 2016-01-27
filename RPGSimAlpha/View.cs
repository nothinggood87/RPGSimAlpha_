using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using System.Drawing;
using OpenTK.Graphics.OpenGL;

namespace RPGSimAlpha
{
    class View
    {
        public enum TweenType
        {
            Instant = 0,
            Linear,
            QuadraticInOut,
            CubicInOut,
            QuarticOut,
        }
        public Vector2 Position { get; private set; }
        /// <summary>
        /// in radians + = clockwise
        /// </summary>
        public double Rotation { get; set; }
        /// <summary>
        /// 1 = default
        /// </summary>
        public double Zoom { get; set; }
        public Vector2 PositionGoTo { get; private set; }
        private Vector2 PositionFrom { get; set; }
        private TweenType TwnType { get; set; }
        private int CurrentStep { get; set; }
        private int TweenSteps { get; set; }

        public View(Vector2 startPosition,double startRotation = 1.0,double startZoom = 1.0)
        {
            Position = startPosition;
            Rotation = startRotation;
            Zoom = startZoom;
        }
        public void Update()
        {
            if(CurrentStep < TweenSteps)
            {
                CurrentStep++;
                switch(TwnType)
                {
                    case TweenType.Instant:
                        {
                            Position = PositionGoTo;
                            break;
                        }
                    case TweenType.Linear:
                        {
                            Position = PositionFrom + (PositionGoTo - PositionFrom) * 
                                GetLinear((float)CurrentStep / TweenSteps);
                            break;
                        }
                    case TweenType.QuadraticInOut:
                        {
                            Position = PositionFrom + (PositionGoTo - PositionFrom) *
                                GetQuadraticInOut((float)CurrentStep / TweenSteps);
                            break;
                        }
                    case TweenType.CubicInOut:
                        {
                            Position = PositionFrom + (PositionGoTo - PositionFrom) *
                                GetCubicInOut((float)CurrentStep / TweenSteps);
                            break;
                        }
                    case TweenType.QuarticOut:
                        {
                            Position = PositionFrom + (PositionGoTo - PositionFrom) *
                                GetQuarticOut((float)CurrentStep / TweenSteps);
                            break;
                        }
                }
            }
            else
            {
                Position = PositionGoTo;
            }
        }
        public void SetPosition(Vector2 newPosition) => SetPosition(newPosition, TweenType.Instant, 0);
        public void SetPosition(Vector2 newPosition,TweenType type , int numSteps)
        {
            this.PositionFrom = Position;
            this.Position = newPosition;
            this.PositionGoTo = newPosition;
            TwnType = type;
            CurrentStep = 0;
            TweenSteps = numSteps;
        }
        public float GetLinear(float t)
        {
            return t;
        }

        public float GetQuadraticInOut(float t)
        {
            return (t * t) / ((2 * t * t) - (2 * t) + 1);
        }

        public float GetCubicInOut(float t)
        {
            return (t * t * t) / ((3 * t * t) - (3 * t) + 1);
        }

        public float GetQuarticOut(float t)
        {
            return -((t - 1) * (t - 1) * (t - 1) * (t - 1)) + 1;
        }
        public void ApplyTransform()
        {
            try
            {
                Matrix4 transform = Matrix4.Identity;

                transform = Matrix4.Mult(transform,Matrix4.CreateTranslation(-Position.X, -Position.Y, 0));
                transform = Matrix4.Mult(transform,Matrix4.CreateRotationZ(-(float)Rotation));
                transform = Matrix4.Mult(transform,Matrix4.CreateScale((float)Zoom, (float)Zoom, 1.0f));
                GL.MultMatrix(ref transform);
            }
            catch
            {
                //Console.WriteLine(this.ToString() + ".ApplyTransform()--Faild");
                throw null;
            }
        }
        public Vector2 ToWorld(Vector2 input)
        {
            input /= (float)Zoom;
            Vector2 dX = new Vector2((float)Math.Cos(Rotation), (float)Math.Sin(Rotation));
            Vector2 dY = new Vector2((float)Math.Cos(Rotation + MathHelper.PiOver2), (float)Math.Sin(Rotation + MathHelper.PiOver2));
            return (this.Position + dX * input.X + dY * input.Y);
        }
    }
}
