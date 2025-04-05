using System.Collections;
using CapyScript.Assets;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Login : MonoBehaviour
{
    [SerializeField] bool isRegister;
    [Space]
    [SerializeField] TMP_InputField usernameField;
    [SerializeField] TMP_InputField passwordField;
    [SerializeField] TMP_InputField emailField;

    private void Awake()
    {
        AssetDatabase.LoadScriptableObjects();
        Screen.fullScreenMode = FullScreenMode.MaximizedWindow;
        Cursor.lockState = CursorLockMode.None;

        if (User.activeUser != null)
        {
            LoginSuccess();
        }
    }

    public async void Submit()
    {
        if (User.activeUser != null)
        {
            return;
        }

        if (isRegister)
        {
            await TryRegister();
        }
        else
        {
            await TryLogin();
        }
    }

    async Awaitable TryRegister()
    {
        if (string.IsNullOrWhiteSpace(usernameField.text) || string.IsNullOrWhiteSpace(passwordField.text) || string.IsNullOrWhiteSpace(emailField.text))
        {
            Debug.LogError("Please fill all fields.");
            return;
        }

        string username = usernameField.text.Trim();
        string password = passwordField.text.Trim();
        string email = emailField.text.Trim();

        string response = await APIReference.GetData("register.php?username=" + username + "&passwd=" + password + "&email=" + email);

        if (response.Contains("success"))
        {
            await TryLogin();
        }
        else
        {
            Error(response);
        }
    }

    async Awaitable TryLogin()
    {
        if (string.IsNullOrWhiteSpace(usernameField.text) || string.IsNullOrWhiteSpace(passwordField.text))
        {
            Debug.LogError("Please fill all fields.");
            return;
        }

        string username = usernameField.text.Trim();
        string password = passwordField.text.Trim();

        string response = await APIReference.GetData("login.php?username=" + username + "&passwd=" + password);

        if (response.Contains("success"))
        {
            User.SetActiveUser(username);
            Game.GenerateStore();
            LoginSuccess();
        }
        else
        {
            Error(response);
        }
    }

    void Error(string response)
    {
        Debug.LogError("An error occurred: " + response);
    }

    void LoginSuccess()
    {
        Debug.Log("Login success!");
        StartCoroutine(OnLogin());
    }

    IEnumerator OnLogin()
    {
        yield return new WaitWhile(() => User.activeUser.RefreshPending || Game.StoreRefreshPending);
        SceneManager.LoadScene("Main");
    }
}
