using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace RPGSimAlpha
{
    namespace Resources
    {
        static class Physics
        {
            public static Texture2D[] Textures { get; private set; }
            /// <summary>
            /// placeHolder
            /// </summary>
            public const float MaxTorque = 0.1f;
            /// <summary>
            /// placeHolder
            /// </summary>
            public const float Thrust = 0.0625f;
            /// <summary>
            /// Land,Air (x)
            /// </summary>
        }
    }
}
