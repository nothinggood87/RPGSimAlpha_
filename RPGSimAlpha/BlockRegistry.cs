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
            public enum BlockTypes
            {
                ThrusterIon = 0,
                WingBack,
                WingLeftEdge,
                WingMid,
                WingRightEdge,
                SolarPanel,
                Collector,
                BadGuy,
                Bullet,
                Empty,
                Ladder,
                LadderPlatform,
                Solid,
            }
            public static BlockTemplate[] Registry { get; } = new BlockTemplate[]
            {
                new BlockTemplate(BlockTypes.ThrusterIon,500),
                new BlockTemplate(BlockTypes.WingBack,270),
                new BlockTemplate(BlockTypes.WingLeftEdge,2700/5),
                new BlockTemplate(BlockTypes.WingMid,2700/4),
                new BlockTemplate(BlockTypes.WingRightEdge,2700/5),
                new BlockTemplate(BlockTypes.SolarPanel,150),
                new BlockTemplate(BlockTypes.Collector,300f),
                new BlockTemplate(BlockTypes.BadGuy,220),
                new BlockTemplate(BlockTypes.Bullet,0.03125f),
                new BlockTemplate(BlockTypes.Empty,0,false),
                new BlockTemplate(BlockTypes.Ladder,17.6f,false,false,true),
                new BlockTemplate(BlockTypes.LadderPlatform,20,false,true,true),
                new BlockTemplate(BlockTypes.Solid,2700),
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
