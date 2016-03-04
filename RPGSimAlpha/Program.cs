using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGSimAlpha
{
    class Program
    {
        static void Main(string[] args)
        {
            Galaxy gal = new Galaxy(20000,15000);
            Game game = new Game(1000, 1000,gal);
            //Cluster shuttle = new Cluster(Resources.IO.LoadMultiBlock(@"\Shuttle.TMX"), new OpenTK.Vector2(0, 0), new OpenTK.Vector2(-0.00f, -0.00f), 0.00f);
            Block[,] grid = new Block[1, 3];
            grid[0, 0] = new Block(Resources.BlockRegistry.BlockTypes.Collector, grid, 0, 0);
            grid[0, 1] = new Block(Resources.BlockRegistry.BlockTypes.SolarPanel, grid, 0, 1);
            grid[0, 2] = new Block(Resources.BlockRegistry.BlockTypes.ThrusterIon, grid, 0, 2);
            float value = 0;
            Cluster Probe = new Cluster(grid, new OpenTK.Vector2(value,value),OpenTK.Vector2.Zero,0,new ushort[] {2,2 },gal);
            Controller player = new Controller(Controller.ControlType.Human);
            ShipLogistics ship = new ShipLogistics(Probe,grid, player);
            game.ObjHandler.AddCluster(Probe);
            game.ObjHandler.AddShipLogistics(ship);
            //game.ObjHandler.AddCluster(new Cluster(Cluster.Shape.Rectangle, Resources.BlockRegistry.BlockTypes.WingMid, new OpenTK.Vector2(0, -1000), new OpenTK.Vector2(0,0),(float)Math.PI/(60*10), 50,250));
            new Task(() =>
            {
                while (1 == 1 )
                {
                    System.Threading.Thread.Sleep(2000);
                    Console.WriteLine("FramesPerSec = " + game.TICK/2.0);
                    game.TICK = 0;
                }
            }).Start();
            game.Run();
            Console.ReadLine();
        }
    }
}
