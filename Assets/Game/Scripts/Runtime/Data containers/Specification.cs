using UnityEngine;

public class Specification : DatabaseReference<int>
{
    public string OS;
    public string CPU;
    public string GPU;
    public string RAM;
    public string Storage;
    
    public Specification(int id) : base(id)
    {
        Get();
    }

    public override void Refresh()
    {
        Get();
    }

    async void Get()
    {
        RefreshPending = true;

        string result = await APIReference.GetData("specifications.php?id=" + key);
        var data = JSONTools.Convert(result);

        OS = data["system"];
        CPU = data["cpu"];
        GPU = data["gpu"];
        RAM = data["ram"];
        Storage = data["storage"];

        RefreshPending = false;
    }
}
