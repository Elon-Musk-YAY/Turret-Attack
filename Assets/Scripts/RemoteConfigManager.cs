
using System.Threading.Tasks;
using Unity.Services.RemoteConfig;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using Unity.Services.Core.Environments;
using UnityEngine.SceneManagement;

public class RemoteConfigManager : MonoBehaviour
{
    public struct userAttributes { }
    public struct appAttributes { }


    async Task InitializeRemoteConfigAsync()
    {
        // initialize handlers for unity game services
        var options = new InitializationOptions();

        options.SetEnvironmentName("development");
        await UnityServices.InitializeAsync(options);

        // remote config requires authentication for managing environment information
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
    }

    async Task Start()
    {
        // initialize Unity's authentication and core services, however check for internet connection
        // in order to fail gracefully without throwing exception if connection does not exist
        if (Utilities.CheckForInternetConnection())
        {
            await InitializeRemoteConfigAsync();
        }
        RemoteConfigService.Instance.FetchCompleted += ApplyRemoteSettings;
        RemoteConfigService.Instance.FetchConfigs(new userAttributes(), new appAttributes());
    }

    void ApplyRemoteSettings(ConfigResponse configResponse)
    {
        if (SceneManager.GetActiveScene().name == "TowerDefenseMenu" && configResponse.requestOrigin == ConfigOrigin.Remote)
        {
            UpdateChecker.instance.CheckForUpdates(RemoteConfigService.Instance.appConfig.GetString("remoteVersion"));
            SeasonalEventsManager.instance.SetSeasonalEvents(RemoteConfigService.Instance.appConfig.GetBool("halloweenSeason"), RemoteConfigService.Instance.appConfig.GetBool("christmasSeason"));
        }
    }
}

