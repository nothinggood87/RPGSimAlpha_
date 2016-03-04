using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGSimAlpha
{
    class ShipLogistics
    {
        private delegate void Action(int x, int y);
        private delegate void Machine();
        public ShipLogistics(Cluster outside,Block[,] grid,Controller ctrl)
        {
            External = outside;
            CargoHold = new Inventory();
            TechGrid = new TechnicalBlock[outside.Width, outside.Height];
            DoToAll(new Action((int x,int y) => { SetMachine(ref TechGrid[x, y],grid[x,y]); }));
            Parent = ctrl;
        }
        public void Update(View view)
        {
            DoToAll((int x, int y) =>
            {
                TechGrid[x, y].Update();
            });
            if (Parent.Accelerate)
                AccelerateForward();
            Parent.Update(External, view);
        }
        public void RightClick(Block block)
        {
            Console.WriteLine("Active = " + TechGrid[block.X,block.Y].Activated + "\n Enter: t/f");
            char ret = Console.ReadKey().KeyChar;
            if (ret == 't' || ret == 'T')
                TechGrid[block.X, block.Y].Activated = true;
            else TechGrid[block.X, block.Y].Activated = false;

        }
        private TechnicalBlock[,] TechGrid { get; set; }
        private Controller Parent { get; }
        private Inventory CargoHold { get; }
        /// <summary>
        /// the actual ship
        /// </summary>
        private Cluster External { get; }
        public float Thrust { get; set; } = 0;
        private void DoToAll(Action action)
        {
            for (short i = 0; i < External.Width; i++)
            {
                for (short j = 0; j < External.Height; j++)
                {
                    action(i, j);
                }
            }
        }
        public void AccelerateForward()
        {
            External.Velocity.X += (float)((Thrust * Math.Sin(External.CurrentRotation)) / (External.Mass));
            External.Velocity.Y -= (float)((Thrust * Math.Cos(External.CurrentRotation)) / (External.Mass));
        }
        public void SetMachine(ref TechnicalBlock techBlock,Block childBlock,bool activated = true)
        {
            switch(childBlock.Type)
            {
                case Resources.BlockRegistry.BlockTypes.ThrusterIon:
                    {
                        techBlock = new TechnicalBlock(childBlock, new TechnicalBlock.Machine(Rocket), activated);
                        break;
                    }
                case Resources.BlockRegistry.BlockTypes.SolarPanel:
                    {
                        techBlock = new TechnicalBlock(childBlock, new TechnicalBlock.Machine(SolarPanel), activated);
                        break;
                    }
                case Resources.BlockRegistry.BlockTypes.Collector:
                    {
                        techBlock = new TechnicalBlock(childBlock, new TechnicalBlock.Machine(HydrogenCollecter), activated);
                        break;
                    }
                default:
                    {
                        techBlock = new TechnicalBlock(childBlock, new TechnicalBlock.Machine(Generic), false);
                        break;
                    }
            }
        }
        //blocks
        public void SolarPanel()
        {
            byte location = (byte)Inventory.HoldingTypes.ElectricCharge;
            if (CargoHold.CargoAmounts[location] < sbyte.MaxValue)
            {
                CargoHold.CargoAmounts[location]++;
            }
        }
        public void HydrogenCollecter()
        {
            byte location = (byte)Inventory.HoldingTypes.Fuel;
            if (CargoHold.CargoAmounts[location] < sbyte.MaxValue)
            {
                CargoHold.CargoAmounts[location]++;
            }
        }
        public void Rocket()
        {
            byte fuel = (byte)Inventory.HoldingTypes.Fuel;
            byte electricCharge = (byte)Inventory.HoldingTypes.ElectricCharge;
            if (Parent.Accelerate && CargoHold.CargoAmounts[fuel] > 0 
                && CargoHold.CargoAmounts[electricCharge] > 0)
            {
                CargoHold.CargoAmounts[fuel]-=1;
                CargoHold.CargoAmounts[electricCharge]-=1;
                Thrust += Resources.Physics.Thrust;
            }
        }
        private void Generic() { }
    }
}
