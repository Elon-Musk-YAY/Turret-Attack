using UnityEngine;
using UnityEngine.UI;

public class UpdateChecker : MonoBehaviour
{
    
    public static string gameVersion = "1.5";
    public GameObject updateAvailableUI;
    public Text versionText;
    public Text uiVersionText;
    public static UpdateChecker instance;

    public static string remoteVersion;

    private void Awake()
    {
        instance = this;
    }

    public void GoToDownloadPage() {
        Application.OpenURL("https://akshar727.itch.io/Turret-Overload");
        Dismiss();
    }

    public void Dismiss() {
        updateAvailableUI.SetActive(false);
    }
    private void OpenUI() {
        updateAvailableUI.SetActive(true);
    }

    private void Start()
    {
        uiVersionText.text = $"v{gameVersion} (c) Akshar Desai {System.DateTime.Now.Year}";
    }

    public void CheckForUpdates(string remoteVersion) {
        if (remoteVersion != gameVersion) {
            versionText.text = $"Turret Overload v{remoteVersion} is now available!";
            OpenUI();
        }
    }

  
}