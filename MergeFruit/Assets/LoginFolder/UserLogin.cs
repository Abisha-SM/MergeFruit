using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UserLogin : MonoBehaviour
{
    [SerializeField] private Button googleLoginbtn;
    [SerializeField] private Button fbLoginbtn;
    [SerializeField] private Button guestLoginbtn;

    private void OnEnable()
    {
        googleLoginbtn.onClick.AddListener(GLogin);
        fbLoginbtn.onClick.AddListener(FBLogin);
        guestLoginbtn.onClick.AddListener(guestLogin);
    }


    void GLogin()
    {
       // GoogleLogin.instance.OnSignIn();
    }

    void FBLogin()
    {
        FaceBookLogin.instance.Facebook_LogIn();
    }

    void guestLogin()
    {
        GuestLogin.instance.OnGuestLoginButtonClick();
    }



}
