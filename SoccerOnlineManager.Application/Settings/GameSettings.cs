namespace SoccerOnlineManager.Application.Settings
{
    public class GameSettings
    {
        public decimal TeamInitialTransferBudget { get; set; }

        public decimal PlayerInitialValue { get; set; }

        public int TeamGoalkeepersCount { get; set; }

        public int TeamDefendersCount { get; set; }

        public int TeamMidfieldersCount { get; set; }

        public int TeamAttackersCount { get; set; }

        public int PlayerMinAge { get; set; }

        public int PlayerMaxAge { get; set; }

        public int MinPriceIncrease { get; set; }

        public int MaxPriceIncrease { get; set; }
    }
}
