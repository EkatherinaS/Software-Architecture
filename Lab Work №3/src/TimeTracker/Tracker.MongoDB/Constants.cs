namespace Tracker.MongoDB
{
    public class Constants
    {
        public static string ConnectionString = "mongodb://localhost:27017";
        public static string DatabaseName = "PlanFixStage";
        private static bool isProd = false;

        public static void SetUp()
        {
            if (isProd) DatabaseName = "PlanFix";
            else DatabaseName = "PlanFixStage";
        }
    }
}
