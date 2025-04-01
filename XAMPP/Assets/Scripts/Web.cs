using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using static System.Net.WebRequestMethods;

public class Web : MonoBehaviour
{
    public string url = "http://localhost/unitybackendtutorial/";

    void Start()
    {
        // A correct website page.
        StartCoroutine(GetRequest($"{url}GetDate.php"));
        //StartCoroutine(GetRequest("http://jacobjeffcoat.iceiy.com/GetDate.php"));
        //StartCoroutine(GetUsers());
        //StartCoroutine(Login("testUser", "123456"));
        //StartCoroutine(Login("testUser2", "123456"));
        //StartCoroutine(Login("testUser3", "123456"));
        //StartCoroutine(RegisterUser("Bob", "BobsPassword"));

        // A non-existing page.
        //StartCoroutine(GetRequest("https://error.html"));
    }

    public IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Trick host into thinking unity is a browser
            AddHeader(webRequest);
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    break;
            }
        }
    }

    public IEnumerator GetUsers()
    {
        string uri = url + "GetUsers.php";
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Trick host into thinking unity is a browser
            AddHeader(webRequest);

            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    break;
            }
        }
    }

    public IEnumerator Login(string username, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("loginUser", username);
        form.AddField("loginPass", password);
        
        using (UnityWebRequest www = UnityWebRequest.Post(url + "Login.php", form))
        {
            // Trick host into thinking unity is a browser
            AddHeader(www);

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
                Main.instance.userInfo.SetCredentials(username, password);
                Main.instance.userInfo.SetID(www.downloadHandler.text);

                if (www.downloadHandler.text.Contains("Wrong Credentials") || www.downloadHandler.text.Contains("Username does not exist."))
                {
                    Debug.Log("Try again");
                }
                else
                {
                    Main.instance.userProfile.SetActive(true);
                    Main.instance.login.gameObject.SetActive(false);
                }
            }
        }
    }

    public IEnumerator RegisterUser(string username, string password)
    {
        WWWForm form = new();
        form.AddField("loginUser", username);
        form.AddField("loginPass", password);

        using (UnityWebRequest www = UnityWebRequest.Post(url + "RegisterUser.php", form))
        {
            // Trick host into thinking unity is a browser
            AddHeader(www);

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
            }
        }
    }

    public IEnumerator GetItemsIDs(string userID, System.Action<string> callback)
    {
        WWWForm form = new();
        form.AddField("userID", userID);

        string uri = url + "GetItemIDs.php";
        using (UnityWebRequest webRequest = UnityWebRequest.Post(uri, form))
        {
            // Trick host into thinking unity is a browser
            AddHeader(webRequest);

            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);

                    string jsonArray = webRequest.downloadHandler.text; 

                    //TODO Call callback function to pass the results
                    callback.Invoke(jsonArray);
                    break;
            }
        }
    }

    public IEnumerator GetItem(string itemID, System.Action<string> callback)
    {
        WWWForm form = new();
        form.AddField("itemID", itemID);

        string uri = url + "GetItem.php";
        using (UnityWebRequest webRequest = UnityWebRequest.Post(uri, form))
        {
            // Trick host into thinking unity is a browser
            AddHeader(webRequest);

            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);

                    string jsonArray = webRequest.downloadHandler.text;

                    callback.Invoke(jsonArray);
                    break;
            }
        }
    }

    public IEnumerator GetItemIcon(string itemID, System.Action<byte[]> callback)
    {
        WWWForm form = new();
        form.AddField("itemID", itemID);

        string uri = url + "GetItemIcon.php";
        using (UnityWebRequest webRequest = UnityWebRequest.Post(uri, form))
        {
            // Trick host into thinking unity is a browser
            AddHeader(webRequest);

            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    Debug.Log("DOWNLOADING: Icon " + itemID);

                    byte[] bytes = webRequest.downloadHandler.data;

                    if(webRequest.downloadHandler.data == null)
                    {
                        Debug.LogError("No data returned");
                    }

                    callback.Invoke(bytes);

                    break;
            }
        }
    }

    public IEnumerator SellItem(string userItemID, string itemID, string userID)
    {
        WWWForm form = new();

        form.AddField("id", userItemID);
        form.AddField("userID", userID);
        form.AddField("itemID", userID);

        string uri = url + "sellItem.php";
        using (UnityWebRequest webRequest = UnityWebRequest.Post(uri, form))
        {
            // Trick host into thinking unity is a browser
            AddHeader(webRequest);

            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    break;
            }
        }
    }

    public void AddHeader(UnityWebRequest webRequest)
    {
        //webRequest.SetRequestHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7");
        //webRequest.SetRequestHeader("Accept-Encoding", "gzip, deflate");
        //webRequest.SetRequestHeader("Accept-Language", "en");
        //webRequest.SetRequestHeader("Cache-Control", "max-age=0");
        //webRequest.SetRequestHeader("Cookie", "__test=d4f16507ae75e677830d2f5a3f570eca");
        //webRequest.SetRequestHeader("Upgrade-Insecure-Requests", "1");
        //webRequest.SetRequestHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/133.0.0.0 Safari/537.36");
    }
}
