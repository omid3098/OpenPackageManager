using System;
using System.Collections;
using System.IO;
using OpenPackageManager.Editor;
using UnityEditor;
using UnityEngine;

namespace OpenConnectionTools
{
    public static class ConnectionTools
    {
        // private static int requestTimeout = 10;
        // public const string masterLink = "https://api.github.com/";

        // public static IEnumerator PostData(string masterLink, string apiLink, string data, Action<string> OnComplete, Action<string> OnError)
        // {
        //     UnityWebRequest request = new UnityWebRequest(masterLink + apiLink, UnityWebRequest.kHttpVerbPOST);
        //     request.SetRequestHeader("Content-Type", "application/json");
        //     if (!string.IsNullOrEmpty(data)) request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(data));
        //     request.downloadHandler = new DownloadHandlerBuffer();
        //     request.timeout = requestTimeout;
        //     yield return request.SendWebRequest();
        //     if (request.isNetworkError || request.isHttpError)
        //     {
        //         OnError(request.error);
        //     }
        //     else
        //     {
        //         OnComplete(request.downloadHandler.text);
        //     }
        // }

        // public static IEnumerator GetData(string masterLink, string apiLink, string data, Action<string> OnComplete, Action<string> OnError)
        // {
        //     UnityWebRequest request = new UnityWebRequest(masterLink + apiLink, UnityWebRequest.kHttpVerbGET);
        //     request.SetRequestHeader("Content-Type", "application/json");
        //     if (data != null) request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(data));
        //     request.downloadHandler = new DownloadHandlerBuffer();
        //     request.timeout = requestTimeout;
        //     yield return request.SendWebRequest();
        //     if (request.isNetworkError || request.isHttpError)
        //     {
        //         OnError(request.error);
        //     }
        //     else
        //     {
        //         OnComplete(request.downloadHandler.text);
        //     }
        // }

        // public static IEnumerator GetData(string masterLink, string apiLink, string data, Action<byte[]> OnComplete, Action<string> OnError)
        // {
        //     UnityWebRequest request = new UnityWebRequest(masterLink + apiLink, UnityWebRequest.kHttpVerbGET);
        //     // request.SetRequestHeader("Content-Type", "application/json");
        //     if (data != null) request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(data));
        //     request.downloadHandler = new DownloadHandlerBuffer();
        //     request.timeout = requestTimeout;
        //     yield return request.SendWebRequest();
        //     if (request.isNetworkError || request.isHttpError)
        //     {
        //         OnError(request.error);
        //     }
        //     else
        //     {
        //         OnComplete(request.downloadHandler.data);
        //     }
        // }

        private static string saveDirectory = Application.dataPath + OpenPackageManagerConfig.DownloadDirectory;

        public static void UpdateRepository(Action<string> OnComplete)
        {
            Debug.Log("Updating repository...");
            EditorCoroutine.start(GetData(OpenPackageManagerConfig.RepositoryLink, null, null,
             (string data) =>
             {
                 // On Success
                 if (!string.IsNullOrEmpty(data))
                 {
                     Debug.Log("Repository Updated: " + data);
                     if (OnComplete != null) OnComplete(data);
                 }
                 else
                 {
                     Debug.LogError("Faild to update repository. received data is null.");
                 }
             }, (error) =>
             {
                 Debug.LogError("Faild to update repository: " + error);
             }));
        }

        public static void DownloadPackage(RepositoryPackage package, Action<float> OnProgress, Action<byte[]> OnComplete, Action<string> OnError)
        {
            // https://api.github.com/repos/omid3098/OpenUi/zipball
            if (!Directory.Exists(saveDirectory))
            {
                Debug.Log("Download directory does not exist. Creating: " + saveDirectory);
                Directory.CreateDirectory(saveDirectory);
                AssetDatabase.Refresh();
            }

            var url = "https://api.github.com/repos/" + package.owner.login + "/" + package.name + "/zipball";
            Debug.Log(url);
            EditorCoroutine.start(GetData(url, "", OnProgress, (byteArrays) =>
            {
                if (OnComplete != null) OnComplete(byteArrays);
                string fullPath = saveDirectory + "/" + package.name + ".zip";
                Debug.Log(fullPath);
                File.WriteAllBytes(fullPath, byteArrays);
                ZipUtil.Unzip(fullPath, saveDirectory);
                AssetDatabase.Refresh();
            }, OnError));
        }


        public static IEnumerator GetData(string masterLink, string apiLink, Action<float> OnProgress, Action<byte[]> OnComplete, Action<string> OnError)
        {
            WWW www = new WWW(masterLink + apiLink);
            while (!www.isDone)
            {
                Debug.Log("downloading...");
                if (OnProgress != null) OnProgress(www.progress);
                yield return null;
                if (www.isDone)
                {
                    if (!string.IsNullOrEmpty(www.error))
                    {
                        if (OnError != null) OnError(www.error);
                    }
                    if (OnComplete != null) OnComplete(www.bytes);
                }
            }
        }

        public static IEnumerator GetData(string masterLink, string apiLink, Action<float> OnProgress, Action<string> OnComplete, Action<string> OnError)
        {
            yield return EditorCoroutine.start(GetData(masterLink, apiLink, OnProgress, (byte[] bytes) =>
            {
                if (OnComplete != null) OnComplete(System.Text.Encoding.UTF8.GetString(bytes));
            }, OnError));
        }
    }
}