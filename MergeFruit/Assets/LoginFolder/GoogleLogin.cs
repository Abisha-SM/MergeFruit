// <copyright file="SigninSampleScript.cs" company="Google Inc.">
// Copyright (C) 2017 Google Inc. All Rights Reserved.
//
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
//
//  http://www.apache.org/licenses/LICENSE-2.0
//
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations


using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Google;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.IO;
using UnityEngine.Networking;

public class GoogleLogin : MonoBehaviour
{

    public Text statusText;
    public string webClientId = "911369172066-3jmp1mqjif188j30usr2v9pvv2t4p5ie.apps.googleusercontent.com";
    private GoogleSignInConfiguration configuration;
    private string localDataPath;
    public static GoogleLogin instance;
    public bool googleLoginbool;
    public string username;
    public Sprite _profilePic;
    public string imageURL;
    public int currentlevel;
    //  public PlayerDataSO playerdata;



    [System.Serializable]
    public class UserData
    {
        public string displayName;
        public string email;
        public string userId;
        public int currentlevel;
    }

    private void Awake()
    {

        configuration = new GoogleSignInConfiguration
        {
            WebClientId = webClientId,
            RequestIdToken = true
        };


        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }

        // Set the path for saving user data
        // localDataPath = Application.persistentDataPath + "/GoogleData.json";
        LoadUserData();  // Attempt to load user data when the app starts
    }

    private void Start()
    {
        // LoadUserData(); // Attempt to load user data when the app starts
    }

    public void OnSignIn()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;

        GoogleSignIn.DefaultInstance.SignOut(); // Sign out if already signed in
        StartCoroutine(SignInCoroutine());
        // googleLoginbool = true;
    }

    IEnumerator SignInCoroutine()
    {
        googleLoginbool = true;
        yield return new WaitForSeconds(0.05f);  // Slight delay for smoother sign-in
        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(
            OnAuthenticationFinished, TaskScheduler.Default);
        // GoogleSignIn.DefaultInstance.SignIn().ContinueWith(
        //   OnDetails, TaskScheduler.Default);
        // yield return new WaitForSeconds(0.3f);
        // SceneManager.LoadScene(1);
        // StartCoroutine(GetTexture(imageURL));
        // Load a new scene after successful sign-in
    }

    public void OnSignOut()
    {
        Debug.Log("Signing out...");
        GoogleSignIn.DefaultInstance.SignOut();
        PlayerPrefs.DeleteKey("USERNAME");
        PlayerPrefs.DeleteKey("LOGIN");

        // Delete saved data file
        //  if (File.Exists(localDataPath))
        //  {
        //     File.Delete(localDataPath);
        // }

        currentlevel = 1;
        googleLoginbool = false;
        SceneManager.LoadScene(0);  // Load the initial scene

    }

    public void OnDisconnect()
    {
        AddStatusText("Calling Disconnect");
        GoogleSignIn.DefaultInstance.Disconnect();
    }

    internal void OnAuthenticationFinished(Task<GoogleSignInUser> task)
    {
        if (task.IsFaulted)
        {
            Debug.LogError("Authentication Failed: " + task.Exception.Message);
        }
        else if (task.IsCanceled)
        {
            Debug.LogError("Authentication Canceled");
        }
        else
        {
            Debug.Log("Authentication successful.");
            googleLoginbool = true;
            SceneManager.LoadScene(1);
            GoogleSignIn.DefaultInstance.SignIn().ContinueWith(
           OnDetails, TaskScheduler.Default);
            StartCoroutine(GetTexture(imageURL));
        }
    }

    internal void OnDetails(Task<GoogleSignInUser> task)
    {

        username = task.Result.DisplayName;
        imageURL = task.Result.ImageUrl?.ToString();
        //  SaveUserData(task.Result);  // Save user data after successful sign-in
        Debug.Log("Profile Image URL: " + task.Result.ImageUrl.OriginalString);
        StartCoroutine(GetTexture(task.Result.ImageUrl.ToString()));  // Retrieve the profile image

    }
    internal void OnImage(Task<GoogleSignInUser> task)
    {
        imageURL = task.Result.ImageUrl.OriginalString;
        StartCoroutine(GetTexture(imageURL));
    }

    IEnumerator GetTexture(string imageUrl)
    {
        WWW www = new WWW(imageUrl);
        yield return www;

        _profilePic = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));

    }

    // Save user data locally in a JSON file
    private void SaveUserData(GoogleSignInUser user)
    {
        UserData userData = new UserData
        {
            displayName = user.DisplayName,
            email = user.Email,
            userId = user.UserId,
            // currentlevel = playerdata.player.PlayerCurrentLevel
        };

        // Serialize to JSON and save to file
        string jsonData = JsonUtility.ToJson(userData, true);
        Debug.Log("Saving User Data: " + jsonData);  // Log the JSON data to ensure it's correct

        try
        {
            File.WriteAllText(localDataPath, jsonData);
            Debug.Log("User data saved to file.");
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error saving user data: " + ex.Message);
        }

        // Optionally store basic info in PlayerPrefs
        PlayerPrefs.SetString("USERNAME", user.DisplayName);
        PlayerPrefs.SetString("EMAIL", user.Email);
        PlayerPrefs.SetString("USER_ID", user.UserId);
        PlayerPrefs.Save();
    }

    // Load user data from the JSON file
    private void LoadUserData()
    {
        if (File.Exists(localDataPath))
        {
            string jsonData = File.ReadAllText(localDataPath);
            Debug.Log("Loaded JSON Data: " + jsonData);  // Log the loaded JSON
            UserData loadedData = JsonUtility.FromJson<UserData>(jsonData);
            AddStatusText("Welcome back: " + loadedData.displayName);
        }
        else
        {
            Debug.Log("No user data found. Please log in.");
        }
    }

    // Helper method to add status text to the UI
    void AddStatusText(string text)
    {
        statusText.text += text + "\n";
        Debug.Log(statusText.text);
    }

}

