using System.Collections;
using System.Collections.Generic;
public static class StatsConfiguration
{
    public static List<StatConfig> Configs { get; private set; }

    public static void Init(GameStats gameStats)
    {
        Configs = new List<StatConfig>()
        {
            new StatConfig(
                StatType.Population,
                "population",
                "Population",
                () => gameStats.GetPopulation().ToString()
            ),
            new StatConfig(
                StatType.MaxAge,
                "maxAge",
                "Max Age",
                () => TimeUtils.GetFormatedDateFromSeconds(gameStats.maxAge)
            ),
            new StatConfig(
                StatType.MaxPopulation,
                "maxPopulation",
                "Max Population",
                () => gameStats.maxPopulation.ToString()
            ),
            new StatConfig(
                StatType.DeathCount,
                "deathCount",
                "Deaths",
                () => gameStats.deaths.ToString()
            ),
            new StatConfig(
                StatType.AverageAgeAtDeath,
                "averageAge",
                "Average Age",
                () => TimeUtils.GetFormatedDateFromSeconds(gameStats.GetAverageAgeAtDeath())
            ),
        };
    }
}

public enum StatType
{
    Population,
    MaxAge,
    AverageAgeAtDeath,
    MaxPopulation,
    DeathCount
}