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
        /// <summary>
        /// refreshes all of the general values in this class
        /// </summary>
        public void Refresh()
        {
            if (Zoom * 16 <= 1)
                TextureSize = 1;
            else if (Zoom * 4 <= 1)
                TextureSize = 4;
            else TextureSize = MaxTextureSize;
            TextureDensity = (byte)(1 / (Zoom * MaxTextureSize));

            if (TextureDensity == 0)
                TextureDensity = 1;

        }
        public const byte MaxTextureSize = 16;
        public Vector2 Position { get; private set; }
        /// <summary>
        /// in radians + = clockwise
        /// </summary>
        public float Rotation { get; set; }
        public float Zoom { get; set; }
        public byte TextureSize { get; private set; } = 16;
        public byte TextureDensity { get; private set; } = 1;
        public View(Vector2 startPosition,float startRotation = 1,float startZoom = 1.0f)
        {
            Position = startPosition;
            Rotation = startRotation;
            Zoom = startZoom;
        }
        public void SetPosition(Vector2 newPosition) => this.Position = newPosition;
        public void ApplyTransform()
        {
            //adjusts for texture size changes
            float zoom = (float)(Zoom * (16 /TextureSize));
            try
            {
                Matrix4 transform = Matrix4.Identity;
                transform = Matrix4.Mult(transform,Matrix4.CreateTranslation(-Position.X*TextureSize, -Position.Y * TextureSize, 0));
                transform = Matrix4.Mult(transform,Matrix4.CreateRotationZ(-Rotation));
                transform = Matrix4.Mult(transform,Matrix4.CreateScale(zoom, zoom, 1.0f));
                GL.MultMatrix(ref transform);
            }
            catch
            {
                //Console.WriteLine(this.ToString() + ".ApplyTransform()--Faild");
                throw null;
            }
        }
    }
}
