using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGSimAlpha
{
    class Inventory
    {
        public enum HoldingTypes
        {
            Fuel = 0,
            ElectricCharge,
        }
        public byte[] CargoAmounts { get; set; } = new byte[Enum.GetNames(typeof(HoldingTypes)).Length];
    }
}
