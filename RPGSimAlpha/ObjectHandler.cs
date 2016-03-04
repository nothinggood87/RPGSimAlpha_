using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace RPGSimAlpha
{
    /// <summary>
    /// hasPlaceHolder
    /// </summary>
    /// <param name="level"></param>
    class ObjectHandler
    {
        private List<Cluster> Clusters { get; set; } = new List<Cluster>();
        public void AddCluster(Cluster cluster) => Clusters.Add(cluster);
        private List<ShipLogistics> Ships { get; set; } = new List<ShipLogistics>();
        public void AddShipLogistics(ShipLogistics ship) => Ships.Add(ship);
        /// <summary>
        /// hasPlaceHolder
        /// </summary>
        /// <param name="level"></param>
        public void Update(View view,System.Drawing.RectangleF windowFrame,Galaxy gal)
        {
            ShipLogistics[] ships = Ships.ToArray();
            for (int i = 0; i < ships.Length; i++)
            {
                ships[i].Update(view);//placeHolder
            }
            Cluster[] clusters = Clusters.ToArray();
            for (int i = 0; i < clusters.Length; i++)
            {
                clusters[i].Update(view,gal,ref windowFrame);//placeHolder
            }
            //Render();
        }
        public void Render(System.Drawing.RectangleF windowFrame,View view)
        {
            Cluster[] clusters = Clusters.ToArray();
            for (int i = 0; i < clusters.Length; i++)
            {
                clusters[i].Draw(view,ref windowFrame);
            }
        }
    }
}
