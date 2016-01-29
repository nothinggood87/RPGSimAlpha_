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
        /// <summary>
        /// hasPlaceHolder
        /// </summary>
        /// <param name="level"></param>
        public void Update(View view)
        {
            Cluster[] clusters = Clusters.ToArray();
            for (int i = 0; i < clusters.Length; i++)
            {
                clusters[i].Update(view);//placeHolder
            }
            //Render();
        }
        public void Render(System.Drawing.RectangleF windowFrame, byte textureSize)
        {
            Cluster[] clusters = Clusters.ToArray();
            for (int i = 0; i < clusters.Length; i++)
            {
                clusters[i].Draw(windowFrame,textureSize);
            }
        }
    }
}
