using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using System.Drawing;
using System.IO;
using System.Xml;

namespace RPGSimAlpha
{
    /*
    struct Level
    {
        private Block[,] Grid { get; }
        private string FileName { get; }
        public Point PlayerStartPosition { get; }
        public Block this[int x,int y]
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
        public Level(int width,int height)
        {
            Grid = new Block[width, height];
            FileName = "none";
            PlayerStartPosition = new Point(1, 1);
            for(int x = 0;x<width;x++)
            {
                for(int y = 0;y<height;y++)
                {
                    if (x == 0 || y == 0 || x == width - 1 || y == height - 1)
                    {
                        Grid[x, y] = new Block(BlockType.Solid, x, y);
                    }
                    else
                    {
                        Grid[x, y] = new Block(BlockType.Empty, x, y);
                    }
                }
            }
        }
        public Level(string filePath)
        {
            short line = 0;
            short subline = 0;
            try
            {
                line++;
                using (FileStream stream = new FileStream(Resources.ResourceFileLocation + filePath,FileMode.Open,FileAccess.Read))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(stream);
                    int width = int.Parse(doc.DocumentElement.GetAttribute("width"));
                    int height = int.Parse(doc.DocumentElement.GetAttribute("height"));
                    line++;
                    Grid = new Block[width, height];
                    this.FileName = filePath;
                    PlayerStartPosition = new Point(1, 1);
                    line++;
                    XmlNode tileLayer = doc.DocumentElement.SelectSingleNode("layer[@name='Tile Layer 1']");
                    XmlNodeList tiles = tileLayer.SelectSingleNode("data").SelectNodes("tile");
                    line++;
                    int x = 0, y = 0;
                    for(int i = 0;i< tiles.Count;i++)
                    {
                        int gid = int.Parse(tiles[i].Attributes["gid"].Value);
                        subline += 10;
                        subline += (short)gid;
                        switch(gid)
                        {
                            case 1:
                                {
                                    Grid[x, y] = new Block(BlockType.Solid, x, y);
                                    break;
                                }
                            case 2:
                                {
                                    Grid[x, y] = new Block(BlockType.Empty, x, y);
                                    break;
                                }
                            case 3:
                                {
                                    Grid[x, y] = new Block(BlockType.Platform, x, y);
                                    break;
                                }
                            case 4:
                                {
                                    Grid[x, y] = new Block(BlockType.Ladder, x, y);
                                    break;
                                }
                            case 5:
                                {
                                    Grid[x, y] = new Block(BlockType.LadderPlatform, x, y);
                                    break;
                                }
                            default:
                                {
                                    Grid[x, y] = new Block(BlockType.Empty,x,y);
                                    break;
                                }
                        }
                        subline += 10;
                        x++;
                        if(x>=width)
                        {
                            x = 0;
                            y++;
                        }
                        subline = 0;
                    }
                    line++;
                    XmlNode objectGroup = doc.DocumentElement.SelectSingleNode("objectgroup[@name='Object Layer 1']");
                    XmlNodeList objects = objectGroup.SelectNodes("object");
                    line++;
                    for (int i = 0; i < objects.Count; i++)
                    {
                        subline = 10;
                        int xPos = int.Parse(objects[i].Attributes["x"].Value);
                        subline++;
                        int yPos = int.Parse(objects[i].Attributes["y"].Value);
                        Console.WriteLine(objects[i].Attributes["name"].Value);
                        Console.WriteLine("Coords = " + xPos + " | " + yPos);
                        subline++;
                        switch (objects[i].Attributes["name"].Value)
                        {
                            case "PlayerStartPos":
                                {
                                    Console.WriteLine("found");
                                    this.PlayerStartPosition = new Point(xPos / Resources.TextureSize, yPos / Resources.TextureSize);///////////////////////////////////////////////////////////////////////////
                                    Console.WriteLine("Coords = " + PlayerStartPosition.X + " | " + PlayerStartPosition.Y);
                                    break;
                                }
                            default:
                                {
                                    break;
                                }
                        }
                        subline++;
                    }
                    subline = 0;
                    line++;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Level.Level(string)---Faild!--" + e.Message);
                Console.WriteLine("Line = " + line);
                Console.WriteLine("Subline = " + subline);
                Console.WriteLine("File.Exists = " + File.Exists(Resources.ResourceFileLocation + filePath));
                int width = 20;
                int height = 20;
                Grid = new Block[width, height];
                FileName = "none";
                PlayerStartPosition = new Point(1, 1);
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        if (x == 0 || y == 0 || x == width - 1 || y == height - 1)
                        {
                            Grid[x, y] = new Block(BlockType.Solid, x, y);
                        }
                        else
                        {
                            Grid[x, y] = new Block(BlockType.Empty, x, y);
                        }
                    }
                }
            }
        }
    }
    */
}
