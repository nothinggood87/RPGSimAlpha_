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
            game.ObjHandler.AddCluster(new Cluster(Resources.IO.LoadMultiBlock(@"\Shuttle.TMX"), new OpenTK.Vector2(-5, -5), new OpenTK.Vector2(-0.00f,-0.00f), 0.00f,Controller.ControlType.Human));
            game.ObjHandler.AddCluster(new Cluster(Cluster.Shape.Rectangle, Resources.BlockRegistry.BlockTypes.Solid, new OpenTK.Vector2(50, 50), new OpenTK.Vector2(0,0),0.01f, 128,128,Controller.ControlType.None));
            //game.ObjHandler.AddCluster(new Cluster(Cluster.Shape.Rectangle,Resources.BlockRegistry.BlockTypes.Solid,new OpenTK.Vector2(5,10),new OpenTK.Vector2(0,-0.01f),Resources.Helper.ConvertDegreesToRadians(1),8,4));
            new Task(() =>
            {
                while (1 == 1)
                {
                    System.Threading.Thread.Sleep(2000);
                    Console.WriteLine("FramesPerSec = " + game.TICK/2.0);
                    game.TICK = 0;
                }
            }).Start();
            game.Run();
            new Task(() =>
            {
                while (1 == 1)
                {
                    System.Threading.Thread.Sleep(1000);
                    Console.WriteLine("FramesPerSec = " + game.TICK);
                    game.TICK = 0;
                }
            }).Start();
            Console.ReadLine();
        }
    }
}
