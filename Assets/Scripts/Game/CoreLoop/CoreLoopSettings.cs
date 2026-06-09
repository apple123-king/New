namespace Game.CoreLoop
{
    /// <summary>
    /// CoreLoop 的规则常量配置。
    /// </summary>
    public sealed class CoreLoopSettings
    {
        public CoreLoopSettings(
            int maxMajorRounds,
            int scoreToWin,
            int bulletsPerAttack,
            float deploymentDurationSeconds,
            float stalemateWarningSeconds,
            float stalemateForceSwitchSeconds)
        {
            MaxMajorRounds = maxMajorRounds;
            ScoreToWin = scoreToWin;
            BulletsPerAttack = bulletsPerAttack;
            DeploymentDurationSeconds = deploymentDurationSeconds;
            StalemateWarningSeconds = stalemateWarningSeconds;
            StalemateForceSwitchSeconds = stalemateForceSwitchSeconds;
        }

        public int MaxMajorRounds { get; }

        public int ScoreToWin { get; }

        public int BulletsPerAttack { get; }

        public float DeploymentDurationSeconds { get; }

        public float StalemateWarningSeconds { get; }

        public float StalemateForceSwitchSeconds { get; }

        /// <summary>
        /// 创建迭代 1 垂直切片使用的默认规则。
        /// </summary>
        public static CoreLoopSettings CreateDefault()
        {
            return new CoreLoopSettings(
                maxMajorRounds: 9,
                scoreToWin: 5,
                bulletsPerAttack: 3,
                deploymentDurationSeconds: 4f,
                stalemateWarningSeconds: 8f,
                stalemateForceSwitchSeconds: 11f);
        }
    }
}
