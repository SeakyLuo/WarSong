using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class Login : MonoBehaviour
{
    public InputField inputEmail, inputPassword;
    public GameObject createAccountPanel, emptyEmail, wrongPassword, emptyPassword;
    public GameObject settingsPanel, forgotPasswordPanel, networkError;

    //support phone number

    // Use this for initialization
    void Start () {
        // If already has an account saved
        string email = PlayerPrefs.GetString("email"),
                password = PlayerPrefs.GetString("password");
        if (email != "" && password != "")
            StartCoroutine(RequestLogin(email, password, false));
	}

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
            settingsPanel.SetActive(true);
    }

    public void ConfirmLogin()
    {
        bool emailIsEmpty = (inputEmail.text == ""), passwordIsEmpty = (inputPassword.text == "");
        if(emailIsEmpty || passwordIsEmpty)
        {
            emptyEmail.SetActive(emailIsEmpty);
            emptyPassword.SetActive(passwordIsEmpty);
            return;
        }
        emptyPassword.SetActive(false);
        StartCoroutine(RequestLogin(inputEmail.text, inputPassword.text));
    }

    //public void RequestLogin(string email, string password)
    //{
    //    // Connect to the server
    //    // if not work return and warn
    //    // else save email and password
    //    // download data
    //    // Info Loader

    //    if (email == "1@1.com" && password == "12345678") // match
    //    {
    //        PlayerPrefs.SetString("email", email);
    //        PlayerPrefs.SetString("password", password);
    //        if (emptyEmail.activeSelf) emptyEmail.SetActive(false);
    //        if (emptyPassword.activeSelf) emptyPassword.SetActive(false);
    //        if (wrongPassword.activeSelf) wrongPassword.SetActive(false);
    //        SceneManager.LoadScene("Main");
    //    }
    //    else
    //    {
    //        inputPassword.text = "";
    //        wrongPassword.SetActive(true);
    //    }        
    //}

    public IEnumerator RequestLogin(string email, string password, bool showError = true)  //connect with server, and VERIFY credentials
    {
        WWWForm infoToPhp = new WWWForm();
        infoToPhp.AddField("email", email);
        infoToPhp.AddField("password", password);

        WWW sendToPhp = new WWW("http://localhost:8888/action_login.php", infoToPhp);
        yield return sendToPhp;

        if (string.IsNullOrEmpty(sendToPhp.error)) //if no error connecting to server
        {
            if (sendToPhp.text.Contains("invalid creds"))  //if credentials don't exist 
            {
                inputPassword.text = "";
                wrongPassword.SetActive(true);
            }
            else                                           //connection and credentials success
            {
                PlayerPrefs.SetString("email", email);
                PlayerPrefs.SetString("password", password);
                if (emptyEmail.activeSelf) emptyEmail.SetActive(false);
                if (emptyPassword.activeSelf) emptyPassword.SetActive(false);
                if (wrongPassword.activeSelf) wrongPassword.SetActive(false);
                SceneManager.LoadScene("Main");
            }
        }
        else                                               //connection failure
        {
            if(showError) networkError.SetActive(true);
        }
    }
    
}
