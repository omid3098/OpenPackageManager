namespace OpenPackageManager.Editor
{
    using UnityEngine;
    using UnityEditor;
    using OpenConnectionTools;
    using System;
    using System.Collections.Generic;
    using System.IO;

    class OpenPackageManagerWindow : EditorWindow
    {
        // private static OpenPackageManagerConfig openPackageManagerConfig;
        public static RepositoryItem repository { get; private set; }
        public static List<RepositoryPackage> allPackages { get; private set; }
        private static OpenPackageManagerDrawer drawer;
        private string repoLocalPath;
        private const string repositoryLocalFileName = "repository.json";

        [MenuItem("Window/Open Package Manager")]
        public static void ShowWindow()
        {
            Debug.Log("ShowWindow");
            Debug.Log(Application.temporaryCachePath);
            var window = EditorWindow.GetWindow(typeof(OpenPackageManagerWindow));
            window.titleContent = new GUIContent("Open Package Manager");
        }
        private void OnEnable()
        {
            repoLocalPath = Path.Combine(Application.temporaryCachePath, repositoryLocalFileName);
            if (File.Exists(repoLocalPath))
            {
                // load local repository
                var data = File.ReadAllText(repoLocalPath);
                ParseRepository(data);
            }
            else
            {
                // download repository from github
                ConnectionTools.UpdateRepository((data) =>
                {
                    // write file locally
                    File.WriteAllText(repoLocalPath, data);
                    ParseRepository(data);
                });
            }
        }

        private void ParseRepository(string data)
        {
            // parse repository
            repository = JsonUtility.FromJson<RepositoryItem>(data);
            allPackages = new List<RepositoryPackage>();
            // allPackages.AddRange(repositoryItem.packages);
            for (int i = 0; i < repository.standardPackages.Count; i++)
            {
                var packageInfo = repository.standardPackages[i].Split('/');
                string authorLocalPath = Application.temporaryCachePath + "/" + packageInfo[0];
                string packageLocalPath = authorLocalPath + "/" + packageInfo[1];
                if (!Directory.Exists(packageLocalPath)) Directory.CreateDirectory(packageLocalPath);

                string packageLocalJsonPath = packageLocalPath + "/" + packageInfo[1] + ".json";
                if (File.Exists(packageLocalJsonPath))
                {
                    string packageData = File.ReadAllText(packageLocalJsonPath);
                    ParsePackage(packageData);
                }
                else
                {
                    // https://api.github.com/repos/omid3098/OpenAudio
                    var url = "https://api.github.com/repos/" + repository.standardPackages[i];
                    Debug.Log(url);
                    EditorCoroutine.start(ConnectionTools.GetData(url, "", null, (_data) =>
                    {
                        File.WriteAllText(packageLocalJsonPath, _data);
                        ParsePackage(_data);
                    }, null));
                }
                drawer = new OpenPackageManagerDrawer();
            }
        }

        void ParsePackage(string packageData)
        {
            var _package = JsonUtility.FromJson<RepositoryPackage>(packageData);
            allPackages.Add(_package);
        }

        void OnGUI()
        {
            if (drawer != null) drawer.OnGUI();
            else EditorGUILayout.LabelField("Loading...", GUILayout.ExpandWidth(false));
        }

        // private void Update()
        // {
        //     Debug.Log("Update");
        // }

        // static IEnumerator DownloadRepository()
        // {
        //     updating = true;
        //     WWW downloadFile = new WWW(OpenPackageManagerConfig.RepositoryLink);
        //     //shows download progress
        //     while (!downloadFile.isDone)
        //     {
        //         Debug.Log("updating repository...");
        //         yield return null;
        //         if (downloadFile.isDone)
        //         {
        //             Debug.Log("updated...");
        //         }
        //     }
        // }
    }
}