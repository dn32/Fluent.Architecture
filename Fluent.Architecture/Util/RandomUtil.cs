using System;

namespace Fluent.Architecture.Core.Util
{
    public static class RandomUtil
    {
        private static readonly Random Random = new Random();

        public static int NextRandom()
        {
            return Random.Next(1, int.MaxValue);
        }

        public static int NextRandom(int max)
        {
            return Random.Next(0, max);
        }

        public static int NextRandom(int min, double max)
        {
            int internalMax = max > int.MaxValue ? int.MaxValue : (int)max;
            return Random.Next(min, internalMax);
        }

        public static string NextRandomString(double size)
        {
            int internalSize = size > int.MaxValue ? int.MaxValue : (int)size;
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[internalSize];

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[NextRandom(chars.Length - 1)];
            }

            return new string(stringChars);
        }
    }
}
