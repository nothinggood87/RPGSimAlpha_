using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGSimAlpha
{
    namespace Resources
    {
        static class Helper
        {
            public static sbyte GetSign(int number)
            {
                if (number == 0) { return 0; }
                if (number > 0) { return 1; }
                return -1;
            }
            public static int GetPositive(int number)
            {
                if (number > 0)
                    return number;
                return number * -1;
            }
            public static float GetPositive(float number)
            {
                if (number > 0)
                    return number;
                return number * -1;
            }
            public static double GetPositive(double number)
            {
                if (number > 0)
                    return number;
                return number * -1;
            }
            public static int[] GetDisplacement(int[] baseValue, int[] otherValue) => new int[]
            {
            otherValue[0]-baseValue[0],
            otherValue[1]-baseValue[1],
            };
            public static short[] GetDisplacement(short[] baseValue, short[] otherValue) => new short[]
            {
            (short)(otherValue[0]-baseValue[0]),
            (short)(otherValue[1]-baseValue[1]),
            };
            public static int[] Round(float[] value) => new int[]
            {
            (int)Math.Round(value[0]),
            (int)Math.Round(value[1]),
            };
            public static float[] Convert(OpenTK.Vector2 value) => new float[]
            {
            value.X,
            value.Y,
            };
            public static sbyte[] GetFlashVector(float[] baseValue, float[] otherValue) => GetFlashVector(Round(baseValue), Round(otherValue));
            public static sbyte[] GetFlashVector(int[] baseValue, int[] otherValue) => GetFlashVector(GetDisplacement(baseValue, otherValue));
            public static sbyte[] GetFlashVector(int[] displacement)
            {
                sbyte[] value = new sbyte[2]
                {
                GetSign(displacement[0]),
                GetSign(displacement[1]),
                };

                if (GetPositive(displacement[0]) > GetPositive(displacement[1]))
                {
                    value[1] = 0;
                }
                else
                {
                    value[0] = 0;
                }
                return value;
            }
            public static double GetVector(short[] value1, short[] value2) => GetVector(GetDisplacement(value1, value2));
            public static double GetVector(int[] value1, int[] value2) => GetVector(GetDisplacement(value1, value2));
            public static double GetVector(int[] value2) => Math.Sqrt((value2[0] * value2[0]) * (value2[1] * value2[1]));
            public static double GetVector(short[] value2) => Math.Sqrt((value2[0] * value2[0]) * (value2[1] * value2[1]));
            public static float ConvertDegreesToRadians(float degrees) => (float)((degrees * Math.PI) / 180);
            public static OpenTK.Vector2 GetCoordinates(float theta,float distance) => new OpenTK.Vector2(
                distance* (float)Math.Cos(theta),
                distance* (float)Math.Sin(theta)
                );
        }
    }
}
