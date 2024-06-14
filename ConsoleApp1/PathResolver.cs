namespace ConsoleApp1
{
    public static class PathResolver
    {
        public static string? GetGameIdFromPath(string path)
        {
            var segments = path.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            if (segments.Length == 2 && segments[0].Equals("game", StringComparison.OrdinalIgnoreCase))
            {
                return segments[1];
            }

            return null;
        }
    }
}