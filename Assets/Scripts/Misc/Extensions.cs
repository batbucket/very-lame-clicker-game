public static class Extensions {

    /// <summary>
    /// Modified to accept ulongs from
    /// https://stackoverflow.com/questions/2134161/format-number-like-stack-overflow-rounded-to-thousands-with-k-suffix
    /// </summary>
    /// <param name="num">This number to transform</param>
    /// <returns></returns>
    public static string KiloFormat(this ulong num) {
        if (num >= 100000000)
            return (num / 1000000).ToString("#,0M");

        if (num >= 10000000)
            return (num / 1000000).ToString("0.#") + "M";

        if (num >= 100000)
            return (num / 1000).ToString("#,0K");

        if (num >= 10000)
            return (num / 1000).ToString("0.#") + "K";

        return num.ToString("#,0");
    }
}