using UnityEngine;

public class Company : DatabaseReference<int>
{
    public string Name;
    
    public Company(int id) : base(id)
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

        string result = await APIReference.GetData("companies.php?id=" + key);
        var data = JSONTools.Convert(result);

        Name = data["name"];

        RefreshPending = false;
    }
}
