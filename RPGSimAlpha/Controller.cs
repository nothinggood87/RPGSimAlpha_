using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGSimAlpha
{
    class Controller
    {
        public Controller(ControlType master,Cluster childCluster)
        {
            ChildCluster = childCluster;
            Master = master;
        }
        public ControlType Master { get; }
        private bool AutoPilot { get; set; }
        private Cluster ChildCluster { get; set; }
        public enum ControlType
        {
            None = 0,
            Human,
            Runner,
            Charger,
        }
        private OpenTK.Vector2 velocityLast = OpenTK.Vector2.Zero;
        public void Update(Cluster cluster,View view)
        {
            if (AutoPilot)
            {
                if (Input.KeysDown.Contains(OpenTK.Input.Key.BackSpace))
                    AutoPilot = false;
                else
                {
                    float DesiredHeading = Resources.Helper.GetPolarCoords(cluster.Velocity * -1).Y;
                    SetHeadingTo(DesiredHeading);
                    if (Math.Round(ChildCluster.Rotation, 3) == Math.Round(DesiredHeading,3))
                    {
                        cluster.AccelerateForward();
                    }
                }
            }
            switch(Master)
            {
                case ControlType.Human:
                    {
                        if(Input.KeyDown(OpenTK.Input.Key.O))
                        {

                        }
                        sbyte[] input = Input.GetKeyInputVector2();
                        if(-input[1] > 0) { cluster.AccelerateForward(); }
                        cluster.AccelerateTorque(input[0]);
                        view.SetPosition(((cluster.PositionTopLeft + (cluster.CenterOfMass*0.0625f)) * view.CurrentTextureSize));
                        if (Input.KeysDown.Contains(OpenTK.Input.Key.KeypadPlus)) { view.Zoom *= 1.0625; }
                        if (Input.KeysDown.Contains(OpenTK.Input.Key.KeypadMinus)) { view.Zoom /= 1.0625; }
                        OpenTK.Vector2 velocity = new OpenTK.Vector2((float)Math.Round(cluster.Velocity.X,2), (float)Math.Round(cluster.Velocity.Y, 2));
                        if (velocity != velocityLast)
                        {
                            Console.WriteLine("Velocity = " + velocity.X + " | " + velocity.Y);
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
        /// <summary>
        /// 3 decimle points precision
        /// </summary>
        /// <param name="theta"></param>
        /// <returns></returns>
        private bool SetHeadingTo(float theta)
        {
            if (theta > 3.14159) //180' degrees
                ChildCluster.AccelerateTorque(1);
            else
                ChildCluster.AccelerateTorque(-1);
            if (Math.Round(ChildCluster.Rotation, 3) == Math.Round(theta, 3))
                return true;
            return false;
        }
    }
}
