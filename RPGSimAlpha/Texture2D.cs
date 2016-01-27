using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGSimAlpha
{
    struct Texture2D
    {
        public int ID { get; }
        public int Width { get; }
        public int Height { get; }
        public Texture2D(int id,int width,int height)
        {
            ID = id;
            Width = width;
            Height = height;
        }
    }
}
