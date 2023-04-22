using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;

namespace Lunarsoft
{
    public class BaseApi : MonoBehaviour
    {
        public string baseUrl;
        public string contentType = "application/json";
        public string bearerToken;

        private void Awake()
        {
            // Load the bearer token from PlayerPrefs if it exists
            string storedToken = PlayerPrefs.GetString("bearerToken", string.Empty);
            if (!string.IsNullOrEmpty(storedToken))
            {
                SetBearerToken(storedToken);
            }
        }


        public void SetBearerToken(string token)
        {
            bearerToken = token;
        }

        protected void SetCommonHeaders(UnityWebRequest request)
        {
            request.SetRequestHeader("Content-Type", contentType);

            if (!string.IsNullOrEmpty(bearerToken))
            {
                request.SetRequestHeader("Authorization", $"Bearer {bearerToken}");
            }
        }

        public IEnumerator Get(string endpoint, Action<string> onSuccess, Action<string> onError)
        {
            using (UnityWebRequest request = UnityWebRequest.Get(baseUrl + endpoint))
            {
                SetCommonHeaders(request);

                yield return request.SendWebRequest();

                HandleRequest(request, onSuccess, onError);
            }
        }

        public IEnumerator Post(string endpoint, JObject jsonData, Action<string> onSuccess, Action<string> onError)
        {
            using (UnityWebRequest request = UnityWebRequest.Post(baseUrl + endpoint, "POST"))
            {
                string jsonString = jsonData.ToString();
                byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonString);
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                request.downloadHandler = new DownloadHandlerBuffer();
                SetCommonHeaders(request);

                yield return request.SendWebRequest();

                HandleRequest(request, onSuccess, onError);
            }
        }


        public IEnumerator Put(string endpoint, string jsonData, Action<string> onSuccess, Action<string> onError)
        {
            using (UnityWebRequest request = UnityWebRequest.Put(baseUrl + endpoint, jsonData))
            {
                SetCommonHeaders(request);

                yield return request.SendWebRequest();

                HandleRequest(request, onSuccess, onError);
            }
        }

        public IEnumerator Delete(string endpoint, Action<string> onSuccess, Action<string> onError)
        {
            using (UnityWebRequest request = UnityWebRequest.Delete(baseUrl + endpoint))
            {
                SetCommonHeaders(request);

                yield return request.SendWebRequest();

                HandleRequest(request, onSuccess, onError);
            }
        }

        private void HandleRequest(UnityWebRequest request, Action<string> onSuccess, Action<string> onError)
        {
            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(request.error);
                onError?.Invoke(request.error);
            }
            else
            {
                onSuccess?.Invoke(request.downloadHandler.text);
            }
        }
    }
}
