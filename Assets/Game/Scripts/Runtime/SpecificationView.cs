using TMPro;
using UnityEngine;

public class SpecificationView : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI OS;
    [SerializeField] TextMeshProUGUI CPU;
    [SerializeField] TextMeshProUGUI GPU;
    [SerializeField] TextMeshProUGUI RAM;
    [SerializeField] TextMeshProUGUI Storage;

    public void Set(Specification spec)
    {
        OS.text = spec.OS;
        CPU.text = spec.CPU;
        GPU.text = spec.GPU;
        RAM.text = spec.RAM;
        Storage.text = spec.Storage;
    }
}
