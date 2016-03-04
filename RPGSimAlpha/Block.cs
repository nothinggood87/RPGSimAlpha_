using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using System.Drawing;
using System.IO;

namespace RPGSimAlpha
{
    class Block
    {
        private Block[,] Grid { get; }
        public Resources.BlockRegistry.BlockTypes Type { get; }
        public bool IsSolid => Resources.BlockRegistry.Registry[(short)Type].IsSolid;
        public bool IsPlatform => Resources.BlockRegistry.Registry[(short)Type].IsPlatform;
        public bool IsLadder => Resources.BlockRegistry.Registry[(short)Type].IsLadder;
        public float Mass => Resources.BlockRegistry.Registry[(short)Type].Mass;
        public Texture2D Texture(byte textureSize) => Resources.IO.GetTexture(Type, textureSize);
        public short X { get; }
        public short Y { get; }
        public Block(Resources.BlockRegistry.BlockTypes type,Block[,] grid,short x,short y)
        {
            this.Type = type;
            Grid = grid;
            X = x;
            Y = y;
        }
        public Vector2 PreciseLocation { get; private set; }
        public void SetPreciseLocation(Vector2 value) => PreciseLocation = value;
    }
}
