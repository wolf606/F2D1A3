using System.Collections;
using System.Text;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using UnityEngine.UI;

namespace GameNetworking
{   
    public delegate void OnSuccess<T>(T result);
    public delegate void OnError(string error);

    public static class HttpRequest
    {
        public static IEnumerator Post<T>(string uri, string jsonBody, string token, OnSuccess<T> onSuccess, OnError onError)
        {
            // Create the request
            UnityWebRequest webRequest = new UnityWebRequest(uri, "POST");
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);
            webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.SetRequestHeader("Accept", "application/json");
            if (token != null)
            {
                webRequest.SetRequestHeader("Authorization", token);
            }

            // Send the request and wait for the response
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError || webRequest.isHttpError)
            {

                onError(webRequest.downloadHandler.text);
            }
            else
            {
                // Parse the JSON response
                T result = JsonUtility.FromJson<T>(webRequest.downloadHandler.text);
                onSuccess(result);
            }
        }

        public static IEnumerator Get<T>(string uri, string token, OnSuccess<T> onSuccess, OnError onError)
        {
            // Create the request
            UnityWebRequest webRequest = UnityWebRequest.Get(uri);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Accept", "application/json");
            if (token != null)
            {

                webRequest.SetRequestHeader("Authorization", token);
            }

            // Send the request and wait for the response
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
                onError(webRequest.error);
            }
            else
            {
                // Parse the JSON response
                //Debug.Log(webRequest.downloadHandler.text);
                T result = JsonUtility.FromJson<T>(webRequest.downloadHandler.text);
                onSuccess(result);
            }
        }

        public static IEnumerator GetImage(string uri, OnSuccess<Sprite> onSuccess, OnError onError)
        {
            UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(uri);

            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
                onError(webRequest.error);
            }
            else
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(webRequest);
                Rect rect = new Rect(0, 0, texture.width, texture.height);
                Sprite sprite = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f));
                onSuccess(sprite);
            }
        }
    }
}
