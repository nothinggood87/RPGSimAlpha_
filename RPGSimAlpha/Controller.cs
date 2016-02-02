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
            if (AutoPilot)//broken
            {
                float DesiredHeading = Resources.Helper.GetPolarCoords(cluster.Velocity).Y;
                DesiredHeading *= -1;
                Console.WriteLine("theta = " + DesiredHeading);
                Console.WriteLine("CurrentRotation = " + cluster.CurrentRotation);
                //SetHeadingTo(DesiredHeading);
                if(cluster.Velocity.LengthSquared == 0) { return; }
                if (Math.Round(ChildCluster.CurrentRotation, 1) == Math.Round(DesiredHeading,1))
                {
                    Console.WriteLine("Heading Correct");
                    cluster.ResetTorque();
                    cluster.AccelerateForward();
                }
                else
                {
                    SetHeadingTo(DesiredHeading);
                }
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
                        if(-input[1] > 0) { cluster.AccelerateForward(); }
                        cluster.AccelerateTorque(input[0]);
                        view.SetPosition((cluster.PositionTopLeft + (cluster.CenterOfMass/View.MaxTextureSize) + cluster.Velocity) * view.CurrentTextureSize);
                        if (Input.KeysDown.Contains(OpenTK.Input.Key.KeypadPlus)) { view.Zoom *= 1.0625f; }
                        if (Input.KeysDown.Contains(OpenTK.Input.Key.KeypadMinus)) { view.Zoom /= 1.0625f; }
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
        private void SetHeadingTo(float theta)
        {
            if (Resources.Helper.GetSign((int)theta) == Resources.Helper.GetSign((int)ChildCluster.Rotation))
                return;
            if (theta < -3.14159) //180' degrees
                ChildCluster.AccelerateTorque(1);
            else
                ChildCluster.AccelerateTorque(-1);
            if (Math.Round(ChildCluster.Rotation, 2) == Math.Round(theta, 2))
                return;
            return;
        }
    }
}
