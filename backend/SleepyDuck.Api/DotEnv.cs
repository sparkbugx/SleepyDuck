namespace SleepyDuck.Api;

/// <summary>
/// Loads KEY=VALUE pairs from the nearest .env file into the process environment.
/// Variables that are already set win, so a real deployment can skip the file.
/// </summary>
public static class DotEnv
{
    public static void Load()
    {
        var path = Find(Directory.GetCurrentDirectory()) ?? Find(AppContext.BaseDirectory);
        if (path is null)
        {
            return;
        }

        foreach (var raw in File.ReadAllLines(path))
        {
            var line = raw.Trim();
            var separator = line.IndexOf('=');
            if (line.Length == 0 || line[0] == '#' || separator <= 0)
            {
                continue;
            }

            var key = line[..separator].Trim();
            var value = line[(separator + 1)..].Trim();
            if (value.Length >= 2 && value[0] == value[^1] && value[0] is '"' or '\'')
            {
                value = value[1..^1];
            }

            if (Environment.GetEnvironmentVariable(key) is null)
            {
                Environment.SetEnvironmentVariable(key, value);
            }
        }
    }

    private static string? Find(string start)
    {
        for (var dir = new DirectoryInfo(start); dir is not null; dir = dir.Parent)
        {
            var candidate = Path.Combine(dir.FullName, ".env");
            if (File.Exists(candidate))
            {
                return candidate;
            }
        }

        return null;
    }
}
