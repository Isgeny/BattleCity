using System;

namespace BattleCity
{
    public static class GameRandom
    {
        private static Random _rand;

        static GameRandom()
        {
            _rand = new Random();
        }

        public static int RandNumber(int maxValue)
        {
            return _rand.Next(maxValue + 1);
        }

        public static int RandNumber(int minValue, int maxValue)
        {
            return _rand.Next(minValue, maxValue + 1);
        }
    }
}