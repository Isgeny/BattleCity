namespace BattleCity
{
    public static class Options
    {
        private static bool friendlyFire;
        private static bool aiUseBonus;
        private static string p1Name;
        private static string p2Name;

        static Options()
        {
            throw new System.NotImplementedException();
        }

        public static bool AIUseBonus
        {
            get => default(bool);
            set
            {
            }
        }

        public static bool FriendlyFire
        {
            get => default(bool);
            set
            {
            }
        }

        public static string P1Name
        {
            get => default(string);
            set
            {
            }
        }

        public static string P2Name
        {
            get => default(string);
            set
            {
            }
        }

        private static void LoadOptionsFromFile()
        {
            throw new System.NotImplementedException();
        }

        public static void WriteOptionsToFile()
        {
            throw new System.NotImplementedException();
        }
    }
}