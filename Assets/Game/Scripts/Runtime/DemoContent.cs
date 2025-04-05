using CapyScript;
using TMPro;
using UnityEngine;

public class DemoContent : MonoBehaviour
{
    void Start()
    {
        GetComponent<TextMeshProUGUI>().text = LoremIpsum.GenerateParagraphs(100);
    }
}
