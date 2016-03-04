using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGSimAlpha
{
    class Controller
    {
        public Controller(ControlType master)
        {
            Master = master;
        }
        public ControlType Master { get; }
        private bool AutoPilot { get; set; }
        public enum ControlType
        {
            None = 0,
            Human,
            Runner,
            Charger,
        }
        private OpenTK.Vector2 velocityLast = OpenTK.Vector2.Zero;
        public bool Accelerate { get; set; } = false;
        public void Update(Cluster cluster,View view)
        {
            if (AutoPilot)
            {
                Console.WriteLine("Coords = " + cluster.PositionTopLeft);
                if (SetHeadingTo(FixTheta(Math.Abs(Resources.Helper.GetPolarCoords(cluster.Velocity).Y)), cluster))
                    Accelerate = true;
                else Accelerate = false;
                if (cluster.Velocity.LengthSquared < 0.01)
                    AutoPilot = false;
            }
            switch(Master)
            {
                case ControlType.Human:
                    {
                        if(Input.KeyRelease(OpenTK.Input.Key.O))
                        {
                            if(AutoPilot)
                            {
                                AutoPilot = false;
                                Console.WriteLine("AutoPilot:Off");
                            }
                            else
                            {
                                AutoPilot = true;
                                Console.WriteLine("AutoPilot:On");
                            }
                        }
                        sbyte[] input = Input.GetKeyInputVector2();
                        if (-input[1] > 0)
                            Accelerate = true;
                        else if(!AutoPilot)
                            Accelerate = false;
                        
                        cluster.AccelerateTorque(input[0]);
                        view.SetPosition(
                            cluster.PositionTopLeft +
                            new OpenTK.Vector2(cluster.Width / 2, cluster.Height / 2) + 
                            cluster.Velocity);
                        if (Input.KeysDown.Contains(OpenTK.Input.Key.KeypadPlus)) { view.Zoom *= 1.0625f; }
                        if (Input.KeysDown.Contains(OpenTK.Input.Key.KeypadMinus)) { view.Zoom /= 1.0625f; }
                        OpenTK.Vector2 velocity = new OpenTK.Vector2((float)Math.Round(cluster.Velocity.X,2), (float)Math.Round(cluster.Velocity.Y, 2));
                        if (velocity != velocityLast)
                        {
                            //Console.WriteLine("Velocity = " + velocity);
                            Console.WriteLine("Kilometers Per Second = " + velocity.Length*60*60/1000);
                            velocityLast = velocity;
                        }
                        break;
                    }
                case ControlType.Runner:
                    {
                        break;
                            }
                case ControlType.Charger:
                    {
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }
        private const byte Precision = 2;
        private bool SetHeadingTo(float theta,Cluster cluster)
        {
            if (Math.Round(theta, Precision) == Math.Round(cluster.CurrentRotation, Precision))
                return true;
            theta -= cluster.CurrentRotation;
            if (theta > Math.PI)
                theta -= (float)(Math.PI*3);
            else if (theta < -Math.PI)
                theta += (float)(Math.PI);
            cluster.AccelerateTorque(Resources.Helper.GetSign(theta));
            return false;
        }
        private const float circle = (float)(Math.PI * 2);
        private float FixTheta(float theta)
        {
            if (theta > circle)
                theta -= circle;
            else if (theta < -circle)
                theta += circle;
            return theta;
        }
    }
}
