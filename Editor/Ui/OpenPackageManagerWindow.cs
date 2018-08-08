namespace OpenPackageManager.Editor
{
    using UnityEngine;
    using UnityEditor;
    using System;
    using System.IO;
    using System.Collections;

    class OpenPackageManagerWindow : EditorWindow
    {
        private OpenPackageManagerController controller;
        private OpenPackageManagerDrawer drawer;
        private string repoLocalPath;
        private static string cachePath;
        private const string repositoryLocalFileName = "repository.json";

        [MenuItem("Window/Open Package Manager")]
        public static void ShowWindow()
        {
            cachePath = Application.temporaryCachePath;
            Debug.Log("Show Window");
            var window = EditorWindow.GetWindow(typeof(OpenPackageManagerWindow));
            window.titleContent = new GUIContent("Open Package Manager");
        }

        private void OnEnable()
        {
            Debug.Log("OnEnable");
            if (controller == null) controller = new OpenPackageManagerController();
            repoLocalPath = Path.Combine(cachePath, repositoryLocalFileName);
            if (File.Exists(repoLocalPath))
            {
                // load local repository
                Debug.Log("local repository exists.");
                var data = File.ReadAllText(repoLocalPath);
                controller.ParseRepository(data);
                drawer = new OpenPackageManagerDrawer(controller);
            }
            else
            {
                // download repository from github
                foreach (var link in OpenPackageManagerConfig.RepositoryLinks)
                {
                    Debug.Log("local repository does not exist. downloading: " + link);
                    EditorCoroutine.start(ConnectionTools.GetData(link, null, (prog) => { Debug.Log("downloading " + prog); }, (data) =>
                         {
                             Debug.Log(("Download complete!"));
                             // write file locally
                             File.WriteAllText(repoLocalPath, data);
                             controller.ParseRepository(data);
                             drawer = new OpenPackageManagerDrawer(controller);
                         }, (data) => { Debug.Log("download faild: " + data); }));
                }
            }
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
    }
}