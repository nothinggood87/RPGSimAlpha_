using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Xml;

namespace RPGSimAlpha
{
    namespace Resources
    {
        static class IO
        {
            public const string ResourceFileLocation = @"C:\Repos\RPGSim\RPGSimAlpha\RPGSimAlpha\Resources\";
            private static Texture2D LoadTexture(byte textureSize,string path)
            {
                if (!File.Exists(ResourceFileLocation + textureSize + @"\" + path))
                {
                    throw new FileNotFoundException(@"File not found at '" + ResourceFileLocation + textureSize + @"\" + path + "'");
                }
                int id = GL.GenTexture();
                GL.BindTexture(TextureTarget.Texture2D, id);
                Bitmap bmp = new Bitmap(ResourceFileLocation + textureSize + @"\" + path);

                BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly,
                    System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                    OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
                bmp.UnlockBits(data);

                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS,
                    (int)TextureWrapMode.Clamp);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT,
                    (int)TextureWrapMode.Clamp);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
                    (int)TextureMinFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter,
                    (int)TextureMagFilter.Linear);

                return new Texture2D(id, bmp.Width, bmp.Height);
            }
            public const byte TextureSizeLarge = 16;
            public const byte TextureSizeSmall = 4;
            private static Texture2D[][] TexturesLarge { get; set; }
            private static Texture2D[][] TexturesSmall { get; set; }
            public static void Initualize()
            {
                {
                    TexturesLarge = new Texture2D[3][];
                    int size = Enum.GetNames(typeof(Craft)).Length;
                    TexturesLarge[0] = new Texture2D[size];
                    for (short i = 0; i < size; i++)
                    {
                        TexturesLarge[0][i] = LoadTexture(TextureSizeLarge,@"Craft\" + Enum.GetName(typeof(Craft), i) + ".bmp");
                    }
                    size = Enum.GetNames(typeof(Player)).Length;
                    TexturesLarge[1] = new Texture2D[size];
                    for (short i = 0; i < size; i++)
                    {
                        TexturesLarge[1][i] = LoadTexture(TextureSizeLarge, @"Player\" + Enum.GetName(typeof(Player), i) + ".png");
                    }
                    size = Enum.GetNames(typeof(Generic)).Length;
                    TexturesLarge[2] = new Texture2D[size];
                    for (short i = 0; i < size; i++)
                    {
                        TexturesLarge[2][i] = LoadTexture(TextureSizeLarge, @"Generic\" + Enum.GetName(typeof(Generic), i) + ".bmp");
                    }
                }
                {
                    TexturesSmall = new Texture2D[3][];
                    int size = Enum.GetNames(typeof(Craft)).Length;
                    TexturesSmall[0] = new Texture2D[size];
                    for (short i = 0; i < size; i++)
                    {
                        TexturesSmall[0][i] = LoadTexture(TextureSizeSmall, @"Craft\" + Enum.GetName(typeof(Craft), i) + ".bmp");
                    }
                    size = Enum.GetNames(typeof(Player)).Length;
                    TexturesSmall[1] = new Texture2D[size];
                    for (short i = 0; i < size; i++)
                    {
                        TexturesSmall[1][i] = LoadTexture(TextureSizeSmall, @"Player\" + Enum.GetName(typeof(Player), i) + ".png");
                    }
                    size = Enum.GetNames(typeof(Generic)).Length;
                    TexturesSmall[2] = new Texture2D[size];
                    for (short i = 0; i < size; i++)
                    {
                        TexturesSmall[2][i] = LoadTexture(TextureSizeSmall, @"Generic\" + Enum.GetName(typeof(Generic), i) + ".bmp");
                    }
                }
            }
            public static Texture2D GetTexture(BlockRegistry.BlockTypes name,byte textureSize)
            {
                if (textureSize == 16)
                {
                    if (name > BlockRegistry.BlockTypes.WingRightEdge)
                        return TexturesLarge[(int)CoreTypes.Generic][(int)(name - BlockRegistry.BlockTypes.WingRightEdge)];
                    return TexturesLarge[(int)CoreTypes.Craft][(int)name];
                }
                else
                {
                    if (name > BlockRegistry.BlockTypes.WingRightEdge)
                        return TexturesSmall[(int)CoreTypes.Generic][(int)(name - BlockRegistry.BlockTypes.WingRightEdge)];
                    return TexturesSmall[(int)CoreTypes.Craft][(int)name];
                }
            }
            /// <summary>
            /// core texture types
            /// </summary>
            public enum CoreTypes
            {
                Craft = 0,
                Player,
                Generic,
            }
            public enum Craft
            {
                ThrusterIon = 0,
                WingBack,
                WingLeftEdge,
                WingMid,
                WingRightEdge,
            }
            public enum Player
            {
                PlayerClimbing = 0,
                PlayerFlyingLeft,
                PlayerFlyingRight,
                PlayerLeft,
                PlayerRight,
            }
            public enum Generic
            {
                BadGuy = 0,
                Bullet,
                Empty,
                Ladder,
                LadderPlatform,
                Platform,
                Solid
            }
            public static Block[,] LoadMultiBlock(string filePath)
            {
                short line = 0;
                short subline = 0;
                Block[,] grid;
                try
                {
                    line++;//1
                    using (FileStream stream = new FileStream(ResourceFileLocation + filePath, FileMode.Open, FileAccess.Read))
                    {
                        XmlDocument doc = new XmlDocument();
                        doc.Load(stream);
                        int width = int.Parse(doc.DocumentElement.GetAttribute("width"));
                        int height = int.Parse(doc.DocumentElement.GetAttribute("height"));
                        line++;//2
                        grid = new Block[width, height];
                        line++;//3
                        XmlNode tileLayer = doc.DocumentElement.SelectSingleNode("layer[@name='Tile Layer 1']");
                        XmlNodeList tiles = tileLayer.SelectSingleNode("data").SelectNodes("tile");
                        line++;//4
                        short x = 0, y = 0;
                        for (int i = 0; i < tiles.Count; i++)
                        {
                            int gid = int.Parse(tiles[i].Attributes["gid"].Value);
                            subline += 10;
                            subline += (short)gid;
                            grid[x, y] = new Block((BlockRegistry.BlockTypes)gid-1, x, y);
                            x++;
                            if (x >= width)
                            {
                                x = 0;
                                y++;
                            }
                            subline = 0;
                        }
                        line++;//5
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Level.Level(string)---Faild!--" + e.Message);
                    Console.WriteLine("Line = " + line);
                    Console.WriteLine("Subline = " + subline);
                    Console.WriteLine("File.Exists = " + File.Exists(ResourceFileLocation + filePath));
                    grid = new Block[0,0];
                }
                return grid;
            }
        }
    }
}
