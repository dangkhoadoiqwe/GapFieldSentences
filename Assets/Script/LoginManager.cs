using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    [SerializeField]
    public TMP_InputField usernameInputField;
    [SerializeField]
    public TMP_InputField passwordInputField;

    public void OnSubmitLogin()
    {
        string username = usernameInputField.text;
        string password = passwordInputField.text;

        string loginCheckMessage = CheckLoginInfo(username, password);

        if (string.IsNullOrEmpty(loginCheckMessage))
        {
            Debug.Log("LOGIN");
            SceneManager.LoadScene(1);
        }
        else
        {
            Debug.LogError("ERROR: " +  loginCheckMessage);
        }
    }

    private string CheckLoginInfo(string username, string password)
    {
        string returnString = "";

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            returnString = "Both username and password are empty";
        }
        else if (string.IsNullOrEmpty(username))
        {
            returnString = "Username is empty";
        }
        else if (string.IsNullOrEmpty(password))
        {
            returnString = "Password is empty";
        }
        else if (password.Length < 8)
        {
            returnString = "Password should be at least 8 characters long";
        }
        else if (!HasNumber(password))
        {
            returnString = "Password should contain at least one number";
        }
        else
        {
            returnString = "";
        }

        return returnString;
    }

    private bool HasNumber(string input)
    {
        foreach (char c in input)
        {
            if (char.IsDigit(c))
            {
                return true;
            }
        }
        return false;
    }
}

