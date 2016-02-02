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
            Game game = new Game(1000, 1080);
            game.ObjHandler.AddCluster(new Cluster(Resources.IO.LoadMultiBlock(@"\Shuttle.TMX"), new OpenTK.Vector2(0, 0), new OpenTK.Vector2(-0.00f,-0.00f), 0.00f,Controller.ControlType.Human));
            game.ObjHandler.AddCluster(new Cluster(Cluster.Shape.Rectangle, Resources.BlockRegistry.BlockTypes.WingMid, new OpenTK.Vector2(0, -1000), new OpenTK.Vector2(0,0),0.025f, 1000,250,Controller.ControlType.None));
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
