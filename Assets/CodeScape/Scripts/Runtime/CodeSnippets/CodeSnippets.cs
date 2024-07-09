using UnityEngine;

public static class CodeSnippets
{
    static string[] positiveSnippets = { "Enties.Sleep()", "Entities.Speed/2" };
    public static string GetRandomPositiveSnippet()
    {
        return positiveSnippets[Random.Range(0, positiveSnippets.Length)];
    }

    static string[] negativeSnippets = { "Player.InvertControls()", "Player.TimeMultiplier = 0.5f", "Player.CanJump = false" };
    public static string GetRandomBugBombSnippet()
    {
        return negativeSnippets[Random.Range(0, negativeSnippets.Length)];
    }
}
