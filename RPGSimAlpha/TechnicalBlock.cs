using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGSimAlpha
{
    class TechnicalBlock
    {
        public TechnicalBlock(Block childBlock,Machine action,bool activated = true)
        {
            ChildBlock = childBlock;
            Action = action;
            Activated = activated;
        }
        public Block ChildBlock { get; }
        public bool Activated { get; set; }
        public delegate void Machine();
        public Machine Action { get; }
        public void Update()
        {
            if (Activated)
                Action();
        }
    }
}
