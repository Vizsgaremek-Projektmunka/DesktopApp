using CapyScript.Assets;
using UnityEngine;

public class APIConnectionTest : MonoBehaviour
{
    private async void Awake()
    {
        return;
        
        AssetDatabase.LoadScriptableObjects();
        string result = await APIReference.GetData("companies.php");

        Debug.Log(result);
        foreach (var item in JSONTools.ConvertAll(result))
        {
            string line = "(";

            foreach (var element in item)
            {
                line += element.Key + ": " + element.Value + " | ";
            }

            line = line.Trim().Trim('|').Trim() + ")";
            Debug.Log(line);
        }
    }
}
