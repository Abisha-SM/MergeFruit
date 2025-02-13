using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GuestLogin : MonoBehaviour
{

    // private const string GuestDataFileName = "userData.json"; // JSON file name to store guest data
    public static GuestLogin instance;
    public bool guestLoginbool;
    public string guestname;
    public int currentlevel;
   // public PlayerDataSO playerdata;

    // Serializable class for saving guest data as JSON
    [System.Serializable]
    public class GuestData
    {
        public string guestId;
        public int gameProgress;   // Track game progress, could be used for level or score
        public string settings;
        public string guestname;
        public int currentLevel;   // Track the current level
    }

    private string localDataPath;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }

        //  localDataPath = Application.persistentDataPath + "/GuestData.json";
        LoadGuestDataFromFile();
    }

    public void OnGuestLoginButtonClick()
    {
        //if (!File.Exists(localDataPath))
        // {
        // Generate a new guest ID, game progress, and settings
        // GuestData guestData = new GuestData
        // {
        // guestId = System.Guid.NewGuid().ToString(),
        //  gameProgress = 0, // Initial progress
        // settings = "DefaultSettings", // Default settings
        guestname = Random.Range(1, 9).ToString();
        // currentLevel = playerdata.player.PlayerCurrentLevel // Start at level 1
        // };

        // SaveGuestData(guestData); // Save the new guest data to the JSON file
        // Debug.Log("Guest ID created: " + guestData.guestId);
        // guestname = guestData.guestname;
        // }
        /* else
         {
             GuestData guestData = LoadGuestDataFromFile();
             guestname = guestData.guestname;
             currentlevel = guestData.currentLevel;
             Debug.Log("Guest already logged in with ID: " + guestData.guestId);
         }*/

        SceneManager.LoadScene(1);
        guestLoginbool = true;
    }

    public void OnLogoutButtonClick()
    {
        //  if (File.Exists(localDataPath))
        //  {
        //    File.Delete(localDataPath);
        //   Debug.Log("Guest data file deleted.");
        // }

        Debug.Log("Guest data cleared");
        SceneManager.LoadScene(0);
        guestLoginbool = false;
       // playerdata.player.PlayerCurrentLevel = 1;
    }

    // Save guest data to JSON file
    private void SaveGuestData(GuestData guestData)
    {
        string jsonData = JsonUtility.ToJson(guestData, true); // Serialize guest data to JSON
        File.WriteAllText(localDataPath, jsonData); // Write to file
        Debug.Log("Guest data saved to file.");
    }

    // Load guest data from JSON file
    private GuestData LoadGuestDataFromFile()
    {
        if (File.Exists(localDataPath))
        {
            string jsonData = File.ReadAllText(localDataPath);
            return JsonUtility.FromJson<GuestData>(jsonData); // Deserialize JSON to GuestData object
        }
        else
        {
            Debug.LogError("Guest data file not found.");
            return null; // Return null if the file does not exist
        }
    }

    // Get the current level of the guest
    public int GetCurrentLevel()
    {
        GuestData guestData = LoadGuestDataFromFile();
        if (guestData != null)
        {
            return guestData.currentLevel;  // Return the saved current level
        }
        else
        {
            return 1; // Default to level 1 if no data found
        }
    }

    // Increment the level and save progress
    public void IncrementLevel()
    {
        GuestData guestData = LoadGuestDataFromFile();
        if (guestData != null)
        {
            guestData.currentLevel++;  // Increment level
            SaveGuestData(guestData);  // Save the updated guest data
            Debug.Log("Level incremented to: " + guestData.currentLevel);
        }
    }
}