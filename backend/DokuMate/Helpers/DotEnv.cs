using System.Collections;
using System.Net;

namespace DokuMate.Helpers;

public static class DotEnv
{
    public static void Load(string filePath)
    {
        if (!File.Exists(filePath))
            throw new ArgumentException($"No .env file found at: '{filePath}'");

        Dictionary<string, string> variables = File.ReadAllLines(filePath)
            .Where(line => line.Contains('='))
            .Select(line =>
            {
                var split = line.Split('=');
                return (split[0].Trim(), split[1].Trim());
            })
            .ToDictionary(entry => entry.Item1, entry => entry.Item2);

        foreach (var entry in variables)
        {
            Environment.SetEnvironmentVariable(entry.Key, entry.Value);
        }
    }

    public static string GetVar(string variableName)
    {
        string? found = Environment.GetEnvironmentVariable(variableName);

        if (found == null)
            throw new KeyNotFoundException($"No Environment Variable found for: '{variableName}'");

        return found;
    }
}