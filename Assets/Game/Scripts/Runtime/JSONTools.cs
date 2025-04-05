using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class JSONTools
{
    public static List<Dictionary<string, string>> ConvertAll(string json)
    {
        string trimmed = json.TrimJSONArray();

        int start = trimmed.IndexOf('[');
        int end = trimmed.IndexOf(']');

        string substring = trimmed.Substring(start + 1, end - start - 1);
        string[] elements = substring.Split("},{");

        List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();

        foreach (string element in elements)
        {
            string elementJSON = element;

            if (!elementJSON.StartsWith("{"))
            {
                elementJSON = "{" + elementJSON;
            }

            if (!elementJSON.EndsWith("}"))
            {
                elementJSON = elementJSON + "}";
            }

            result.Add(Convert(elementJSON));
        }

        return result;
    }

    public static Dictionary<string, string> Convert(string json)
    {
        string trimmed = json.TrimJSON();

        string[] pairs = trimmed.Split(',');

        Dictionary<string, string> result = new Dictionary<string, string>();

        foreach (string pair in pairs)
        {
            int separatorIndex = pair.IndexOf(':');
            if (separatorIndex >= 0)
            {
                string key = pair.Substring(0, separatorIndex).TrimJSON();
                string value = pair.Substring(separatorIndex + 1).TrimJSON();

                result.Add(key, value);
            }
        }

        return result;
    }

    public static string TrimJSON(this string str)
    {
        return str.Trim(' ', '\t', '\r', '\n', '"', '{', '}', '[', ']');
    }

    public static string TrimJSONArray(this string str)
    {
        return str.Trim(' ', '\t', '\r', '\n', '"', '{', '}');
    }
}
