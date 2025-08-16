using System;
using UnityEngine;

public static class TimeUtils
{
    public static int GetYearFromGameSeconds(float seconds)
    {
        int actualTimeInDays = Mathf.FloorToInt(seconds * Configuration.REAL_TIME_SCALE);
        int daysInAYear = Configuration.DAYS_IN_MONTH * Configuration.MONTHS_IN_YEAR;
        return actualTimeInDays / daysInAYear;
    }

    public static int GetMonthFromGameSeconds(float seconds)
    {
        int actualTimeInDays = Mathf.FloorToInt(seconds * Configuration.REAL_TIME_SCALE);
        int daysInAYear = Configuration.DAYS_IN_MONTH * Configuration.MONTHS_IN_YEAR;
        int remainingDaysFromYear = actualTimeInDays % daysInAYear;
        return remainingDaysFromYear / Configuration.DAYS_IN_MONTH + 1;

    }

    public static int GetDaysFromGameSeconds(float seconds)
    {
        int actualTimeInDays = Mathf.FloorToInt(seconds * Configuration.REAL_TIME_SCALE);
        int daysInAYear = Configuration.DAYS_IN_MONTH * Configuration.MONTHS_IN_YEAR;
        int remainingDaysFromYear = actualTimeInDays % daysInAYear;
        return remainingDaysFromYear % Configuration.DAYS_IN_MONTH + 1;
    }

    public static string GetFormatedDateFromSeconds(float seconds)
    {
        int actualTimeInDays = Mathf.FloorToInt(seconds * Configuration.REAL_TIME_SCALE);

        int daysInAYear = Configuration.DAYS_IN_MONTH * Configuration.MONTHS_IN_YEAR;
        int year = actualTimeInDays / daysInAYear;

        int remainingDaysFromYear = actualTimeInDays % daysInAYear;
        int month = remainingDaysFromYear / Configuration.DAYS_IN_MONTH;

        int day = remainingDaysFromYear % Configuration.DAYS_IN_MONTH;

        return $"{year}y {month}m {day}d";
    }
}