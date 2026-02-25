using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public static class TokenSequenceUtils
{
    private static readonly Regex NumberRegex = new Regex(@"-?\d+", RegexOptions.Compiled);

    public static bool TryGetTokenNumber(GameObject source, out int number)
    {
        number = 0;
        if (source == null)
        {
            return false;
        }

        if (TryParseNumber(source.name, out number))
        {
            return true;
        }

        TMP_Text tmp = source.GetComponentInChildren<TMP_Text>();
        if (tmp != null && TryParseNumber(tmp.text, out number))
        {
            return true;
        }

        TextMesh textMesh = source.GetComponentInChildren<TextMesh>();
        if (textMesh != null && TryParseNumber(textMesh.text, out number))
        {
            return true;
        }

        return false;
    }

    public static void PaintAllTokens(Color color)
    {
        Transform[] allTransforms = Object.FindObjectsOfType<Transform>(true);
        foreach (Transform transform in allTransforms)
        {
            if (!IsTokenName(transform.name))
            {
                continue;
            }

            Renderer[] renderers = transform.GetComponentsInChildren<Renderer>(true);
            foreach (Renderer renderer in renderers)
            {
                foreach (Material material in renderer.materials)
                {
                    material.color = color;
                }
            }
        }
    }

    public static bool SequenceMatchesTail(IReadOnlyList<int> values, IReadOnlyList<int> expected)
    {
        if (values == null || expected == null || expected.Count == 0 || values.Count < expected.Count)
        {
            return false;
        }

        int offset = values.Count - expected.Count;
        for (int i = 0; i < expected.Count; i++)
        {
            if (values[offset + i] != expected[i])
            {
                return false;
            }
        }

        return true;
    }

    private static bool IsTokenName(string name)
    {
        return name.StartsWith("token", System.StringComparison.OrdinalIgnoreCase);
    }

    private static bool TryParseNumber(string value, out int number)
    {
        number = 0;
        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        Match match = NumberRegex.Match(value);
        return match.Success && int.TryParse(match.Value, out number);
    }
}
