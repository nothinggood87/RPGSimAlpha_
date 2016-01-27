using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using OpenTK;
using OpenTK.Input;

namespace RPGSimAlpha
{
    /*
    class Player
    {
        /// <summary>
        /// in tiles
        /// </summary>
        public Vector2 Position { get; set; } 
        public Vector2 Velocity { get; set; }
        private Vector2 Size { get; }

        private Facing CurrentFacing { get; set; } = Facing.Right;
        private enum Facing
        {
            Left = 0,
            Right,
        }
        private Situation CurrentSituation { get; set; }
        private enum Situation : byte
        {
            None = 0,
            Grounded,
            Climbing,
            OnLadder,
            Flying,
        }
        public RectangleF CollisionRectangle => new RectangleF
            ((Position.X - Size.X / 2f)+20, (Position.Y - Size.Y / 2f), Size.X-18, Size.Y);
        public RectangleF DrawRectangle
        {
            get
            {
                RectangleF colRec = CollisionRectangle;
                colRec.X -= 20;
                colRec.Width += 6;
                colRec.Width += 18;
                return colRec;
            }
        }
        public Player(Vector2 startPosition)
        {
            this.Position = startPosition;
            this.Velocity = Vector2.Zero;
            CurrentSituation = Situation.None;
            this.Size = new Vector2(40, 40);
        }
        public void Update(Vector2 input, Vector2 inputLast,ObjectHandler objHandler)
        {
            //this.Velocity += new Vector2(-(Resources.Friction[i] * Resources.GetSign((int)Math.Ceiling(this.Velocity.X))), Resources.Gravity);
            HandleInput(input,inputLast);
            this.Velocity += new Vector2(0, Resources.Physics.Gravity);
            this.Position += Velocity;
        }
        private void HandleInput(Vector2 input,Vector2 inputLast)
        {
            Vector2 newVelocity = new Vector2(Velocity.X, Velocity.Y);

            UpdateCurrentFacing(input);

            if (input.X == 0 && inputLast.X != 0)
            {
                newVelocity.X -= (Resources.Helper.GetSign((int)Math.Ceiling(newVelocity.X))*Resources.Physics.MovementSpeed);
            }
            else if (inputLast.X == 0)
            {
                newVelocity.X += input.X * Resources.Physics.MovementSpeed;
            }
            if (input.Y < 0 )
            {
                newVelocity.Y += input.Y * Resources.Physics.JumpSpeed;
            } 
            else if (input.Y > 0)
            {
                newVelocity.Y += input.Y*Resources.Physics.MovementSpeed;
            }
            Velocity = newVelocity;
        }
        public void Draw()
        {
            /*
            Texture2D newTexture = new Texture2D();
            switch (CurrentFacing)
            {
                case Facing.Right:
                    {
                        switch (CurrentSituation)
                        {
                        case Situation.Climbing:
                            {
                                newTexture = Resources.IO.Textures[(int)Resources.TexturesRegistry.PlayerClimbingRight];
                                break;
                            }
                        case Situation.Flying:
                            {
                                newTexture = Resources.IO.Textures[(int)Resources.TexturesRegistry.PlayerFlyingRight];
                                break;
                            }
                        case Situation.None:
                            {
                                newTexture = Resources.IO.Textures[(int)Resources.TexturesRegistry.PlayerRight];
                                break;
                            }
                        }
                        break;
                    }
                case Facing.Left:
                    {
                        switch (CurrentSituation)
                        {
                            case Situation.Climbing:
                                {
                                    newTexture = Resources.IO.Textures[(int)Resources.TexturesRegistry.PlayerClimbingLeft];
                                    break;
                                }
                            case Situation.Flying:
                                {
                                    newTexture = Resources.IO.Textures[(int)Resources.TexturesRegistry.PlayerFlyingLeft];
                                    break;
                                }
                            case Situation.None:
                                {
                                    newTexture = Resources.IO.Textures[(int)Resources.TexturesRegistry.PlayerLeft];
                                    break;
                                }
                        }
                        break;
                    }
            }

            SpriteBatch.Draw(newTexture, this.Position, new Vector2(DrawRectangle.Width / newTexture.Width, DrawRectangle.Height / newTexture.Height), Color.White,
                            new Vector2(newTexture.Width / 4f, newTexture.Height / 2f));/////////////////////////////////////////////////////////////////////////10:09
        }
        private void UpdateCurrentFacing(Vector2 currentInput)
        {
            if(currentInput.X != 0)
            {
                if(currentInput.X > 0)
                {
                    CurrentFacing = Facing.Right;
                    return;
                }
                CurrentFacing = Facing.Left;
            }
        }
    }*/
}
