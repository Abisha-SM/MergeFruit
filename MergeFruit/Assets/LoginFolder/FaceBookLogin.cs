using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Facebook.Unity;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.Networking; // For fetching the profile picture from URL

public class FaceBookLogin : MonoBehaviour
{
    public string Name;
    public static FaceBookLogin instance;
    public Texture fbProfilepicTexture;
    public bool FBLoginbool = false;
    public int currentlevel;
    private string localDataPath;
    //public PlayerDataSO playerdata;

    [System.Serializable]
    public class UserData
    {
        public string userName;
        public string userId;
        public string profilePicUrl;
        public int currentlevel;

    }

    #region Initialize

    private void Awake()
    {
        // Set up local data file path
        // localDataPath = Application.persistentDataPath + "/facebook_user_data.json";

        if (!FB.IsInitialized)
        {
            FB.Init(() =>
            {
                if (FB.IsInitialized)
                    FB.ActivateApp();
                else
                    Debug.Log("Couldn't initialize");
            });
        }
        else
        {
            FB.ActivateApp();
        }

        // Initialize singleton and prevent duplicate objects
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }

        // Load the data from local storage if available
        LoadLocalData();
    }

    #endregion

    void SetInit()
    {
        if (FB.IsLoggedIn)
        {
            Debug.Log("Facebook is logged in!");
            string s = "client token: " + FB.ClientToken + " User Id: " + AccessToken.CurrentAccessToken.UserId;
        }
        else
        {
            Debug.Log("Facebook is not logged in!");
        }
        DealWithFbMenus(FB.IsLoggedIn);
    }

    void DealWithFbMenus(bool isLoggedIn)
    {
        if (isLoggedIn)
        {
            FB.API("/me?fields=first_name", HttpMethod.GET, DisplayUsername);
            FB.API("/me/picture?type=square&height=128&width=128", HttpMethod.GET, DisplayProfilePic);
            SceneManager.LoadScene(1);
            FBLoginbool = true;
        }
        else
        {
            Debug.Log("Not logged in to Facebook");
        }
    }

    void DisplayUsername(IResult result)
    {
        if (result.Error == null)
        {
            string name = result.ResultDictionary["first_name"].ToString();
            Name = name;
            Debug.Log("Username: " + name);
            // currentlevel = playerdata.player.PlayerCurrentLevel;
            // Save user data locally
            string userId = AccessToken.CurrentAccessToken.UserId;
            SaveLocalData(name, userId, "", currentlevel); // Save name and userId, profilePicUrl is optional here
        }
        else
        {
            Debug.Log(result.Error);
        }
    }

    void DisplayProfilePic(IGraphResult result)
    {
        if (result.Texture != null)
        {
            Debug.Log("Profile picture loaded.");
            fbProfilepicTexture = result.Texture;

            // Save the profile picture URL (or texture)
            string profilePicUrl = result.RawResult; // Or extract the URL from result if needed
            SaveLocalData(Name, AccessToken.CurrentAccessToken.UserId, profilePicUrl, currentlevel);
        }
        else
        {
            Debug.Log(result.Error);
        }
    }



    // Login
    public void Facebook_LogIn()
    {
        List<string> permissions = new List<string>();
        permissions.Add("public_profile");
        FB.LogInWithReadPermissions(permissions, AuthCallBack);
    }

    void AuthCallBack(IResult result)
    {
        if (FB.IsLoggedIn)
        {
            SetInit();
            var aToken = AccessToken.CurrentAccessToken;
            foreach (string perm in aToken.Permissions)
            {
                Debug.Log(perm);
            }
        }
        else
        {
            Debug.Log("Failed to log in");
        }
    }

    // Logout
    public void Facebook_LogOut()
    {
        LogOut();
    }

    private void LogOut()
    {
        FBLoginbool = false;
        FB.LogOut();
        Name = "";
        fbProfilepicTexture = null;
        // playerdata.player.PlayerCurrentLevel = 1;
        // Delete the local file
        /* if (System.IO.File.Exists(localDataPath))
         {
             System.IO.File.Delete(localDataPath);
             Debug.Log("Local data deleted.");
         }*/

        SceneManager.LoadScene(0); // Go back to the login scene
    }

    #region Local Data Methods

    private void SaveLocalData(string userName, string userId, string profilePicUrl, int level)
    {
        UserData userData = new UserData
        {
            userName = userName,
            userId = userId,
            profilePicUrl = profilePicUrl,
            currentlevel = level
        };

        string jsonData = JsonUtility.ToJson(userData);
        System.IO.File.WriteAllText(localDataPath, jsonData);
        Debug.Log("User data saved locally.");
    }

    private void LoadLocalData()
    {
        if (System.IO.File.Exists(localDataPath))
        {
            string jsonData = System.IO.File.ReadAllText(localDataPath);
            UserData userData = JsonUtility.FromJson<UserData>(jsonData);

            Name = userData.userName;
            Debug.Log("Loaded user data: " + Name);
            // Optionally, load profile picture URL or texture here
            if (!string.IsNullOrEmpty(userData.profilePicUrl))
            {
                StartCoroutine(LoadProfilePicture(userData.profilePicUrl));
            }
        }
        else
        {
            Debug.Log("No local data found.");
        }
    }

    private IEnumerator LoadProfilePicture(string profilePicUrl)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(profilePicUrl);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            fbProfilepicTexture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            Debug.Log("Profile picture loaded.");
        }
        else
        {
            Debug.Log("Failed to load profile picture: " + request.error);
        }
    }

    #endregion

    #region Other

    public void FacebookSharefeed()
    {
        string url = "https:developers.facebook.com/docs/unity/reference/current/FB.ShareLink";
        FB.ShareLink(
            new Uri(url),
            "Checkout COCO 3D channel",
            "I just watched " + "22" + " times of this channel",
            null,
            ShareCallback);
    }

    private static void ShareCallback(IShareResult result)
    {
        Debug.Log("ShareCallback");
        SpentCoins(2, "sharelink");
        if (result.Error != null)
        {
            Debug.LogError(result.Error);
            return;
        }
        Debug.Log(result.RawResult);
    }

    public static void SpentCoins(int coins, string item)
    {
        var param = new Dictionary<string, object>();
        param[AppEventParameterName.ContentID] = item;
        FB.LogAppEvent(AppEventName.SpentCredits, (float)coins, param);
    }

    #endregion
}
