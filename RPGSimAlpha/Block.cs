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
    struct Block
    {
        public Resources.BlockRegistry.BlockTypes Type { get; }
        /// <summary>
        /// position on the x(Right-Left) axis
        /// </summary>
        public short X { get; }
        /// <summary>
        /// position on the Y(Up-Down) axis
        /// </summary>
        public short Y { get; }
        public bool IsSolid => Resources.BlockRegistry.Registry[(short)Type].IsSolid;
        public bool IsPlatform => Resources.BlockRegistry.Registry[(short)Type].IsPlatform;
        public bool IsLadder => Resources.BlockRegistry.Registry[(short)Type].IsLadder;
        public float Mass => Resources.BlockRegistry.Registry[(short)Type].Mass;
        public Block(Resources.BlockRegistry.BlockTypes type,short x,short y)
        {
            this.X = x;
            this.Y = y;
            this.Type = type;
        }
    }
}
