using System;
using System.Collections.Generic;

public static class CreatureNameGenerator
{
    static Random random = new Random();

    private static readonly string[] prefixes =
    {
        "Bon", "Xar", "Vel", "Tor", "Lum", "Zor", "Nar", "Kel", "Vor", "Tan",
        "Mon", "Ser", "Ral", "Dus", "Fen", "Gro", "Hir", "Pax", "Lur", "Kri"
    };

    private static readonly string[] middles =
    {
        "shi", "zor", "nax", "lum", "gor", "rin", "vel", "tar", "pon", "xis",
        "mur", "lok", "sin", "dar", "vor", "tur", "zan", "lun", "pek", "kro"
    };

    private static readonly string[] suffixes =
    {
        "is", "ar", "on", "us", "in", "as", "or", "es", "um", "ek",
        "ix", "an", "ir", "en", "ul", "os", "it", "er", "ak", "et"
    };
    
    public static string GenerateName()
    {
        string prefix = prefixes[random.Next(prefixes.Length)];
        string middle = random.NextDouble() < 0.7 ? middles[random.Next(middles.Length)] : "";
        string suffix = random.NextDouble() < 0.8 ? suffixes[random.Next(suffixes.Length)] : "";

        return prefix + middle + suffix;
    }
}