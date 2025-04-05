using TMPro;
using UnityEngine;

public class Profile : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI name;
    [SerializeField] TextMeshProUGUI description;
    
    private void Awake()
    {
        if (User.activeUser == null)
        {
            return;
        }
        
        name.text = User.activeUser.Username;
        description.text = User.activeUser.ProfileDescription;
    }
}
