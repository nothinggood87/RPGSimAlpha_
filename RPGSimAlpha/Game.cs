using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace RPGSimAlpha
{
    class Game : GameWindow
    {
        public short tick = 0;
        public short TICK
        {
            get
            {
                return tick;
            }
            set
            {
                bool stillTrying = true;
                while(stillTrying)
                {
                    try
                    {
                        tick = value;
                        stillTrying = false;
                    }
                    catch { }
                }
            }
        }
        public const int TileSize = 16;
        public Galaxy Gal { get; }
        public ObjectHandler ObjHandler { get; } = new ObjectHandler();
        View View { get; }
        public Game(int width,int height,Galaxy gal)
            :base(width,height)
        {
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.BlendEquation(BlendEquationMode.Max);
            View = new View(Vector2.Zero, 0, 1);
            Resources.Master.Initualize();
            Input.Initialize(this);
            Gal = gal;
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Resources.Master.Initualize();
        }
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            View.Refresh();
            TICK++;
            base.OnUpdateFrame(e);
            //RefreshWindowFrame();
            ObjHandler.Update(View,WindowFrame,Gal);
            //player.Update(level, Input.GetKeyInputVector2(Input.KeysDown), Input.GetKeyInputVector2(Input.KeysDownLast));
            //View.SetPosition(player.Position, View.TweenType.QuarticOut, 60);
            Input.Update();
        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.ClearColor(Color.Black);
            SpriteBatch.Begin(this.Width, this.Height);
            View.ApplyTransform();
            RefreshWindowFrame();
            ObjHandler.Render(WindowFrame,View);
            this.SwapBuffers();
        }
        public RectangleF WindowFrame { get; private set; }
        private void RefreshWindowFrame()
        {
            WindowFrame = new RectangleF(
                (float)(View.Position.X - ((Size.Width / 2) / View.Zoom)),(float)(View.Position.Y - ((Size.Height / 2) / View.Zoom)), 
                Size.Width / View.Zoom, Size.Height/View.Zoom);
        }
    }
}
