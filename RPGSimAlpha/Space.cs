using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace RPGSimAlpha
{
    class Galaxy
    {
        public Galaxy(ushort width, ushort height)
        {
            area = new SectorBlock[width, height];
        }
        public static long[] GetGalacticCoords(Cluster cluster) => new long[2]
            {
                (long)cluster.PositionTopLeft.X + (cluster.CurrentSectorBlock[0]*SectorBlock.SectorBlockSize),
                (long)cluster.PositionTopLeft.Y + (cluster.CurrentSectorBlock[1]*SectorBlock.SectorBlockSize),
            };
        private SectorBlock[,] area;
        /// <summary>
        /// get
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public SectorBlock GetSectorBlock(ushort[] location) => area[location[0], location[1]];
        /// <summary>
        /// set
        /// </summary>
        /// <param name="location"></param>
        /// <param name="cluster"></param>
        public void SetSectorBlock(Cluster cluster)
        {
            if (GetSectorBlock(cluster.CurrentSectorBlock) == null)
                area[cluster.CurrentSectorBlock[0], cluster.CurrentSectorBlock[1]] = new SectorBlock();
            GetSectorBlock(cluster.CurrentSectorBlock).Sector(cluster.CurrentSector);
        }
        public void RefreshCurrentSector(Cluster cluster,ref Vector2 positionTopLeft)
        {
            if (!SectorBlock.OutOfSectorBounds(ref cluster.CurrentSector[0], ref cluster.CurrentSector[1], ref positionTopLeft))
                return;
            List<Cluster> sector = GetSectorBlock(cluster.CurrentSectorBlock).Sector(cluster.CurrentSector);
            sector.Remove(cluster);
            if (sector.Count == 0)
                sector = null;
            for (byte i = 0; i < 2; i++)
            {
                if (cluster.PositionTopLeft[i] < 0)
                {
                    cluster.CurrentSectorBlock[i]--;
                    positionTopLeft[i] += SectorBlock.SectorBlockSize;
                }
                else if(cluster.PositionTopLeft[i] >= SectorBlock.SectorBlockSize)
                {
                    cluster.CurrentSectorBlock[i]++;
                    positionTopLeft[i] -= SectorBlock.SectorBlockSize;
                }
            }
            SetSectorBlock(cluster);
        }
        /*
        private void RefreshSectorBlock(ref Vector2 positionTopLeft,Cluster cluster)
        {
            try {
                GetSectorBlock(cluster.CurrentSectorBlock).Sectors[cluster.CurrentSector[0], cluster.CurrentSector[1]].Remove(cluster);
            }
            catch { }
            if (positionTopLeft.X < 0)
            {
                positionTopLeft.X += SectorBlock.SectorBlockSize;
                cluster.CurrentSectorBlock[0]--;
            }
            else if(positionTopLeft.X >= SectorBlock.SectorBlockSize)
            {
                positionTopLeft.X -= SectorBlock.SectorBlockSize;
                cluster.CurrentSectorBlock[0]++;
            }
            if (positionTopLeft.Y < 0)
            {
                positionTopLeft.Y += SectorBlock.SectorBlockSize;
                cluster.CurrentSectorBlock[1]--;
            }
            else if (positionTopLeft.Y >= SectorBlock.SectorBlockSize)
            {
                positionTopLeft.Y -= SectorBlock.SectorBlockSize;
                cluster.CurrentSectorBlock[1]++;
            }
            ushort[] xy = SectorBlock.GetNearestSectorCoords(ref positionTopLeft);
            GetSectorBlock(cluster.CurrentSectorBlock).Sectors[xy[0], xy[1]].Add(cluster);
        }
        */
    }
    class SectorBlock
    {
        public SectorBlock()
        {
            Sectors = new List<Cluster>[100, 100];
            for (short i = 0; i < 100; i++)
            {
                for (short j = 0; j < 100; j++)
                {
                    Sectors[i, j] = new List<Cluster>();
                }
            }
        }
        public List<Cluster> Sector(ushort[] location) => Sectors[location[0], location[1]];
        /// <summary>
        /// *1000 for special coordinates
        /// </summary>
        public List<Cluster>[,] Sectors { get; }
        public static ushort[] GetNearestSectorCoords(ref Vector2 positionTopLeft)
        {
            return new ushort[] 
            {
                (ushort)Math.Floor(positionTopLeft.X / SectorSize),
                (ushort)Math.Floor(positionTopLeft.Y / SectorSize)
            };
        }
        public static bool OutOfSectorBounds(ref ushort sectorX,ref ushort sectorY,ref Vector2 coords)
        {
            ushort[] xy = GetNearestSectorCoords(ref coords);
            if (sectorX == xy[0] && sectorY == xy[1])
                return false;
            return true;
        }
        internal const uint SectorBlockSize = SectorSize * SectorSize;//1,000,000
        private const ushort SectorSize = 10000;
        public static bool OutOfSectorBlockBounds(ref Vector2 coords)
        {
            if (coords.X >= 0 && coords.X <= SectorBlockSize)
                return false;
            if (coords.Y >= 0 && coords.Y <= SectorBlockSize)
                return false;
            return true;
        }
    }
}
