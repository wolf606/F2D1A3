using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

using Mono.Data.Sqlite;
using System.Data;
using System.Collections;

using ApiHandler;
using SQLiteHandler;

public class AccessTokenManager : MonoBehaviour
{
    private static AccessTokenManager instance;
    private string accessToken;
    private string admissionId;

    public static AccessTokenManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject obj = new GameObject("AccessTokenManager");
                instance = obj.AddComponent<AccessTokenManager>();
                DontDestroyOnLoad(obj);
            }
            return instance;
        }
    }

    public string AccessToken
    {
        get => accessToken;
        set => accessToken = value;
    }

    public string AdmissionId
    {
        get => admissionId;
        set => admissionId = value;
    }

    // Optional: You can use Awake to ensure the instance is created before any other script's Start method
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

public class LoginManager : MonoBehaviour
{
    public TMP_InputField usernameField;
    public TMP_InputField passwordField;

    private string connectionPath;

    void Awake()
    {
        connectionPath = "URI=file:" + Application.persistentDataPath + "/";
    }

    public void OnLoginButtonClicked()
    {
        // Verificar que los campos de texto no estén vacíos.
        if (string.IsNullOrEmpty(usernameField.text) || string.IsNullOrEmpty(passwordField.text))
        {
            Debug.Log("Por favor, completa todos los campos antes de continuar.");
            return; // Termina la función aquí para que no se cargue la nueva escena.
        }

        StartCoroutine(loadUserInfo());
    }

    public IEnumerator loadUserInfo()
    {
        yield return ApiRequest.Login(usernameField.text, passwordField.text, (AccessToken result) => {
            Debug.Log("Successfully logged in.");
            Debug.Log("Status: " + result.status);
            Debug.Log("Access token: " + result.accessToken);
            AccessTokenManager.Instance.AccessToken = result.accessToken;
            Debug.Log("Access token from AccessTokenManager: " + AccessTokenManager.Instance.AccessToken);

            // StartCoroutine(ApiRequest.GetLoggedInUser(AccessTokenManager.Instance.AccessToken, (User result2) => {
            //     Debug.Log("Successfully retrieved logged in user.");

            //     IDbConnection dbConnection = DbHandler.OpenDatabase(connectionPath);
            //     DbHandler.InsertUser(dbConnection, result2.data.id, result2.data.profile.pro_nombre, result2.data.profile.pro_apelli, result2.data.profile.pro_avatar, result2.data.email, result2.data.active ? 1 : 0, result2.data.role[0]);
            //     dbConnection.Close();
            //     dbConnection = null;

            // }, (string error) => {
            //     Debug.Log("Error: " + error);
            // }));

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
        }, (string error) => {
            Debug.Log("Error: " + error);
        });
    }
}
