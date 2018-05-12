using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class AuthenticationManager : MonoBehaviour
{

    public GameObject create_email, create_password, create_confirmPassword, create_username, login_email,
                      login_password;
    public Text create_textEmail, create_textPassword, create_textConfirmPassword, create_textUsername,
                      login_textEmail, login_textPassword, overall_textFeedback;
    public Button buttonRegister, cancelRegister;
    public GameObject createAccountPanel;

    public WWWForm infoToPhp;

    private void Start()
    {
        overall_textFeedback.text = "";
        displayLogin();
    }

    public void displayLogin()
    {
        createAccountPanel.SetActive(false);
    }

    public void displayCreateAccount()
    {
        overall_textFeedback.text = "";
        login_textEmail.text = "";
        login_textPassword.text = "";
        createAccountPanel.SetActive(true);
    }

    public void registerButton()
    {
        overall_textFeedback.text = "Processing registration";
        StartCoroutine("RequestReg");
    }

    public void cancelButton() //from CreateNewAccount to Login
    {
        displayLogin();
        overall_textFeedback.text = "";
    }

    public void loginButton()
    {
        overall_textFeedback.text = "Processing login";
        StartCoroutine("RequestLogin");
    }
    public IEnumerator RequestLogin()  //connect with server, and VERIFY credentials
    {
        string email = login_textEmail.text;
        string password = login_textPassword.text;

        infoToPhp = new WWWForm();
        infoToPhp.AddField("email", email);
        infoToPhp.AddField("password", password);

        WWW sendToPhp = new WWW("http://localhost:8888/action_login.php", infoToPhp);
        yield return sendToPhp;

        if (string.IsNullOrEmpty(sendToPhp.error)) //if no error connecting to server
        {
            if (sendToPhp.text.Contains("invalid creds"))  //if credentials don't exist 
            {
                overall_textFeedback.text = "Invalid credentials.";
            }
            else                                           //connection and credentials success
            {
                overall_textFeedback.text = "Success";
                SceneManager.LoadScene("Main");

            }


        }
        else                                               //connection failure
        {
            overall_textFeedback.text = "Error";

        }
    }

    public IEnumerator RequestReg()
    {
        string email = create_textEmail.text;
        string password = create_textPassword.text;
        string confPassword = create_textConfirmPassword.text;
        string userName = create_textUsername.text;

        if (password != confPassword)
        {
            overall_textFeedback.text = "Error - Passwords do not match!";
            yield break;
        }
        if (password.Length < 8)
        {
            overall_textFeedback.text = "Error - Password must be 8 characters!";
            yield break;
        }

        infoToPhp = new WWWForm();
        infoToPhp.AddField("email", email);
        infoToPhp.AddField("password", password);
        infoToPhp.AddField("userName", userName);

        WWW sendToPhp = new WWW("http://localhost:8888/action_reg.php", infoToPhp);
        yield return sendToPhp;

        if (string.IsNullOrEmpty(sendToPhp.error))
        {
            if (sendToPhp.text.Contains("Error Could not create."))
            {
                overall_textFeedback.text = "Error - Could not create...";
            }
            else if(sendToPhp.text.Contains("User Exists")){
                overall_textFeedback.text = "Error - Account with selected username or email " +
                    "already exists...";
            }
            else
            {
                overall_textFeedback.text = "Registration Successful - Enjoy!";
                displayLogin();
            }

        }
    }
}