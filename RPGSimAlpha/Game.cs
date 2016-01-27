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
        public const int TileSize = 16;
        public ObjectHandler ObjHandler { get; } = new ObjectHandler();
        View View { get; }
        public Game(int width,int height)
            :base(width,height)
        {
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.BlendEquation(BlendEquationMode.Max);
            View = new View(Vector2.Zero, 0.0, 1);
            Input.Initialize(this);
            Resources.Master.Initualize();
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Resources.Master.Initualize();
        }
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            RefreshViewBounds();
            ObjHandler.Update(View);
            //player.Update(level, Input.GetKeyInputVector2(Input.KeysDown), Input.GetKeyInputVector2(Input.KeysDownLast));
            //View.SetPosition(player.Position, View.TweenType.QuarticOut, 60);
            View.Update();
            Input.Update();
        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.ClearColor(Color.Black);
            SpriteBatch.Begin(this.Width, this.Height);
            View.ApplyTransform();
            //player.Draw();
            ObjHandler.Render(ViewBounds);
            this.SwapBuffers();
        }
        public RectangleF ViewBounds { get; private set; }
        private void RefreshViewBounds()
        {
            ViewBounds = new RectangleF(
                (View.Position.X - ((Size.Width / 2) / (float)View.Zoom)),(View.Position.Y - ((Size.Height / 2) / (float)View.Zoom)), 
                Size.Width / (float)View.Zoom, Size.Height/(float)View.Zoom);
        }
    }
}
