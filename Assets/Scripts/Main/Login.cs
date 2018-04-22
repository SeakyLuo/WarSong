using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Login : MonoBehaviour
{
    public InputField inputEmail, inputPassword;
    public GameObject createAccountPanel, emptyEmail, wrongPassword, emptyPassword;

    //support phone number

	// Use this for initialization
	void Start () {
        // If already has an account saved
        string email = PlayerPrefs.GetString("email"),
                password = PlayerPrefs.GetString("password");
        if (email != "" && password != "")
            login(email, password);
	}
	
	public void ConfirmLogin()
    {
        if(inputEmail.text == "")
        {
            emptyEmail.SetActive(true);
            return;
        }
        if(inputPassword.text == "")
        {
            emptyPassword.SetActive(true);
            return;
        }
        login(inputEmail.text, inputPassword.text);
    }

    public void login(string email, string password)
    {
        // Connect to the server
        // if not work return and warn
        // else save email and password
        // download data
        // Info Loader
        if (email == "1@1.com" && password == "12345678") // match
        {
            PlayerPrefs.SetString("email", email);
            PlayerPrefs.SetString("password", password);
            if (emptyEmail.activeSelf) emptyEmail.SetActive(false);
            if (emptyPassword.activeSelf) emptyPassword.SetActive(false);
            if (wrongPassword.activeSelf) wrongPassword.SetActive(false);
            SceneManager.LoadScene("Main");
        }
        else
        {
            Debug.Log(email+" "+password);
            inputPassword.text = "";
            wrongPassword.SetActive(true);
        }        
    }

    public void ForgotPassword()
    {

    }

    public void CreateNewAccount()
    {
        createAccountPanel.SetActive(true);
    }
}
