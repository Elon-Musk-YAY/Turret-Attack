using UnityEngine;
using UnityEngine.UI;

public class UpdateChecker : MonoBehaviour
{
    
    public static string gameVersion;
    public GameObject updateAvailableUI;
    public Text versionText;
    public Text updateUiVersionText;
    public static UpdateChecker Instance;

    public static string remoteVersion;

    private void Awake()
    {
        gameVersion = Application.version;
        Instance = this;
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
        updateUiVersionText.text = $"v{gameVersion} (c) Akshar Desai {System.DateTime.Now.Year}";
    }

    public void CheckForUpdates(string remoteVersion) {
        if (remoteVersion != gameVersion && !Application.isEditor) {
            versionText.text = $"Turret Overload v{remoteVersion} is now available!";
            OpenUI();
        }
    }

  
}