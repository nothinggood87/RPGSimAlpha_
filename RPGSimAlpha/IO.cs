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

            public static string ResourceFileLocation { get; private set; }
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
            private static Texture2D[,][] Textures { get; set; }
            public static void Initualize()
            {
                Console.WriteLine("Resources.IO---Initualizing");
                char[] rawLocation = Directory.GetCurrentDirectory().ToCharArray();
                ResourceFileLocation = "";
                for (short i = 0; i < rawLocation.Length-9; i++)
                {
                    ResourceFileLocation += rawLocation[i];
                }
                ResourceFileLocation += @"Resources\";
                Textures = new Texture2D[3,3][];
                for (byte i = 0;i<3;i++)
                {
                    byte textureSize = 1;
                    for (byte j = 0; j < i; j++)
                    {
                        textureSize *= 4;
                    }
                    Console.WriteLine("TextureSize = " + textureSize);
                    int size = Enum.GetNames(typeof(Craft)).Length;
                    Textures[i,0] = new Texture2D[size];
                    for (short j = 0; j < size; j++)
                    {
                        Textures[i,0][j] = LoadTexture(textureSize, @"Craft\" + Enum.GetName(typeof(Craft), j) + ".bmp");
                    }
                    size = Enum.GetNames(typeof(Player)).Length;
                    Textures[i,1] = new Texture2D[size];
                    for (short j = 0; j < size; j++)
                    {
                        Textures[i,1][j] = LoadTexture(textureSize, @"Player\" + Enum.GetName(typeof(Player), j) + ".png");
                    }
                    size = Enum.GetNames(typeof(Generic)).Length;
                    Textures[i,2] = new Texture2D[size];
                    for (short j = 0; j < size; j++)
                    {
                        Textures[i,2][j] = LoadTexture(textureSize, @"Generic\" + Enum.GetName(typeof(Generic), j) + ".bmp");
                    }
                }
                Console.WriteLine("Resources.IO---Initualized");
            }
            private static byte GetTextureSize(byte i)
            {
                byte returnValue = 1;
                for(byte j = 0;j< i;j++)
                {
                    returnValue *= 4;
                }
                return returnValue;
            }
            public static Texture2D GetTexture(BlockRegistry.BlockTypes name,byte textureSize)
            {
                byte i = 0;
                while(textureSize > 1 && textureSize != 0)//gets the array equivalents {1 = 1,4 = 2,16 = 3}
                {
                    i++;
                    textureSize /= 4;
                }
                if (name > BlockRegistry.BlockTypes.Collector)
                    return Textures[i,(int)CoreTypes.Generic][(int)(name - BlockRegistry.BlockTypes.Collector)];
                return Textures[i,(int)CoreTypes.Craft][(int)name];
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
                SolarPanel,
                Collector,
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
                            grid[x, y] = new Block((BlockRegistry.BlockTypes)gid-1,grid,x,y);
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
