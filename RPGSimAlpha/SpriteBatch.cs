using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace RPGSimAlpha
{
    class SpriteBatch
    {
        private static void Draw(Texture2D texture,Vector2 position,Vector2 scale,Color color,Vector2 origin
            ,RectangleF? sourceRec = null ,float rotation = 0.0f)
        {
            Vector2[] vertices = new Vector2[4]
            {
                new Vector2(0,0),
                new Vector2(1,0),
                new Vector2(1,1),
                new Vector2(0,1),
            };
            GL.MatrixMode(MatrixMode.Texture);
            GL.LoadIdentity();
            if (rotation != 0)
            {
                var matrix = Matrix4.CreateTranslation(-0.5f, -0.5f, 0.0f) *
                          Matrix4.CreateFromAxisAngle(new Vector3(0, 0, 1), rotation) *
                          Matrix4.CreateTranslation(0.5f, 0.5f, 0.0f);
                GL.LoadMatrix(ref matrix);
            }
            GL.MatrixMode(MatrixMode.Modelview);
            GL.BindTexture(TextureTarget.Texture2D, texture.ID);
            GL.Begin(PrimitiveType.Quads);
            GL.Color3(color);
            for (byte i = 0; i < 4; i++)
            {
                if (sourceRec == null)
                {
                    GL.TexCoord2(vertices[i]);
                }
                else
                {
                    GL.TexCoord2((sourceRec.Value.Left + vertices[i].X * sourceRec.Value.Width) / texture.Width,
                        sourceRec.Value.Top + vertices[i].Y * sourceRec.Value.Height);
                }
                vertices[i].X *= (sourceRec == null) ? texture.Width : sourceRec.Value.Width;
                vertices[i].Y *= (sourceRec == null) ? texture.Height : sourceRec.Value.Height;
                vertices[i] -= origin;
                vertices[i] *= scale;
                vertices[i] += position;
                GL.Vertex2(vertices[i]);
            }
            GL.End();
        }
        public static void Draw(Block[,] grid, Vector2 positionTopLeft,ref RectangleF windowFrame,View view, float rotation = 0.0f,bool ForceRender = false)
        {
            short width = (short)grid.GetLength(0);
            short height = (short)grid.GetLength(1);
            //origin = zero
            Vector2[] vertices = new Vector2[4]
            {
                new Vector2(0,0),
                new Vector2(1,0),
                new Vector2(1,1),
                new Vector2(0,1),
            };
            GL.MatrixMode(MatrixMode.Texture);
            GL.LoadIdentity();
            if (rotation != 0)
            {
                var matrix = Matrix4.CreateTranslation(-0.5f, -0.5f, 0.0f) *
                          Matrix4.CreateFromAxisAngle(new Vector3(0, 0, 1), rotation) *
                          Matrix4.CreateTranslation(0.5f, 0.5f, 0.0f);
                GL.LoadMatrix(ref matrix);
            }
            GL.MatrixMode(MatrixMode.Modelview);
            byte h;
            RectangleF checking;
            for (short i = 0; i < width; i++)
            {
                for (short j = 0; j < height; j++)
                {
                    if (!ForceRender)
                    {
                        if ((i % view.TextureDensity == 0 && j % view.TextureDensity == 0))
                        {
                            //checking = new RectangleF((grid[i, j].PreciseLocation.X + positionTopLeft.X), (grid[i, j].PreciseLocation.Y + positionTopLeft.Y), 1.5f, 1.5f);
                            checking = new RectangleF(
                                (grid[i, j].PreciseLocation.X + positionTopLeft.X), (grid[i, j].PreciseLocation.Y + positionTopLeft.Y),
                                1f * view.Zoom, 1f * view.Zoom);
                            if (!windowFrame.IntersectsWith(checking))
                                continue;

                        }
                    }
                    GL.BindTexture(TextureTarget.Texture2D, grid[i,j].Texture(view.TextureSize).ID);
                    GL.Begin(PrimitiveType.Quads);
                    for (h = 0; h < 4; h++)
                    {
                        GL.TexCoord2(vertices[h]);
                        GL.Vertex2((vertices[h] + positionTopLeft + grid[i,j].PreciseLocation)*view.TextureSize);
                    }
                    GL.End();
                }
            }
        }
        public static void Begin(int screenWidth,int screenHeight)
        {
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(-screenWidth / 2f, screenWidth / 2f, screenHeight / 2f, -screenHeight / 2f,0f,1f);

        }
    }
}
