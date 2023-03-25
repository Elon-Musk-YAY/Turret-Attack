using UnityEngine;
using UnityEngine.UI;

public class UpdateChecker : MonoBehaviour
{
    
    public static string gameVersion = "1.3";
    public GameObject updateAvailableUI;
    public Text versionText;
    public static UpdateChecker instance;

    public static string remoteVersion;

    private void Awake()
    {
        instance = this;
    }

    public void GoToDownloadPage() {
        Application.OpenURL("https://akshar727.itch.io/Turret-Attack");
        Dismiss();
    }

    public void Dismiss() {
        updateAvailableUI.SetActive(false);
    }
    private void OpenUI() {
        updateAvailableUI.SetActive(true);
    }

    public void CheckForUpdates(string remoteVersion) {
        if (remoteVersion != gameVersion) {
            versionText.text = $"Turret Attack v{remoteVersion} is now available!";
            OpenUI();
        }
    }

  
}