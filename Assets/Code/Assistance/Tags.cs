namespace Code.Assistance
{
    public static class Tags
    {
        public static string Player = "Player";
        public static string Wood = "Wood";
        public static string Stone = "Stone";
        public static string Food = "Food";
        public static string Specialist = "Specialist";
        public static string EveryMan = "EveryMan";
        
        
        internal enum Role
        {
            None = 0,
            Player = 1,
            Resource = 2,
            Specialist = 3,
            EveryMan = 4
        }
    }
}
