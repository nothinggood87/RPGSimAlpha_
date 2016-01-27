using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGSimAlpha
{
    namespace Resources
    {
        static class BlockRegistry
        {
            public const int TextureSize = 16;
            public enum BlockTypes
            {
                ThrusterIon = 0,
                WingBack,
                WingLeftEdge,
                WingMid,
                WingRightEdge,
                BadGuy,
                Bullet,
                Empty,
                Ladder,
                LadderPlatform,
                Solid,
            }
            public static BlockTemplate[] Registry { get; } = new BlockTemplate[]
            {
                new BlockTemplate(BlockTypes.ThrusterIon,2.25f),
                new BlockTemplate(BlockTypes.WingBack,0.25f),
                new BlockTemplate(BlockTypes.WingLeftEdge,0.5f),
                new BlockTemplate(BlockTypes.WingMid,1),
                new BlockTemplate(BlockTypes.WingRightEdge,0.5f),
                new BlockTemplate(BlockTypes.BadGuy,0.5f),
                new BlockTemplate(BlockTypes.Bullet,0.03125f),
                new BlockTemplate(BlockTypes.Empty,0,false),
                new BlockTemplate(BlockTypes.Ladder,0.125f,false,false,true),
                new BlockTemplate(BlockTypes.LadderPlatform,0.15f,false,true,true),
                new BlockTemplate(BlockTypes.Solid,2),
            };
        }
        struct BlockTemplate
        {
            public bool IsSolid { get; }
            public bool IsPlatform { get; }
            public bool IsLadder { get; }
            public float Mass { get; }
            public BlockRegistry.BlockTypes Type { get; }
            public BlockTemplate(BlockRegistry.BlockTypes type, float mass, bool isSolid = true, bool isPlatform = false, bool isLadder = false)
            {
                Type = type;
                IsSolid = isSolid;
                IsPlatform = isPlatform;
                IsLadder = isLadder;
                Mass = mass;
            }
        }
    }
}
