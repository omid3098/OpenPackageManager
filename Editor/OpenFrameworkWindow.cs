namespace OpenFramework.Editor
{
    using UnityEngine;
    using UnityEditor;
    using System.Collections;
    using System;
    using System.Net;
    using System.IO;
    using System.Collections.Generic;

    class OpenFrameworkWindow : EditorWindow
    {
        private static OpenFrameworkResources repositoryResource;
        private static RepositoryItem repositoryItem;
        private static string saveDirectory;
        private static GUILayoutOption buttonWidth;
        private static GUILayoutOption buttonHeight;
        private static Texture2D lineTexture;
        private static GUIStyle linkButtonStyle;
        private string filter;
        private List<RepositoryPackage> showingPackages;

        [MenuItem("Window/OpenPackageManager")]
        public static void ShowWindow()
        {
            repositoryResource = Resources.Load<OpenFrameworkResources>("repository");
            repositoryItem = JsonUtility.FromJson<RepositoryItem>(repositoryResource.repository.text);
            saveDirectory = Application.dataPath + "/OpenFrameworkPackages/";
            buttonWidth = GUILayout.Width(100);
            buttonHeight = GUILayout.Height(30);
            lineTexture = new Texture2D(1, 1);

            linkButtonStyle = new GUIStyle();
            linkButtonStyle.normal.textColor = Color.blue;
            linkButtonStyle.stretchWidth = true;

            var window = EditorWindow.GetWindow(typeof(OpenFrameworkWindow));
            window.titleContent = new GUIContent("Open Package Manager");
        }
        void OnGUI()
        {
            GUILayout.Label("Packages", EditorStyles.boldLabel);
            SearchBar();

            if (string.IsNullOrEmpty(filter)) showingPackages = repositoryItem.packages;
            else showingPackages = repositoryItem.packages.FindAll(x => x.name.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0);

            foreach (var package in showingPackages)
            {
                DrawPackage(package);
            }
            // myString = EditorGUILayout.TextField("Text Field", myString);

            // groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);
            // myBool = EditorGUILayout.Toggle("Toggle", myBool);
            // myFloat = EditorGUILayout.Slider("Slider", myFloat, -3, 3);
            // EditorGUILayout.EndToggleGroup();
        }

        private void SearchBar()
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(10);
            EditorGUILayout.LabelField("Filter", GUILayout.ExpandWidth(false));
            filter = GUILayout.TextField(filter);
            GUILayout.Space(10);
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(10);
            DrawLine();

        }

        private static void DrawLine()
        {
            GUILayout.Box(lineTexture, GUILayout.ExpandWidth(true), GUILayout.Height(1));
        }

        private void DrawPackage(RepositoryPackage package)
        {
            { // Draw package name, version, author and description 
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.BeginVertical();
                    {
                        EditorGUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.LabelField(package.name + " - " + "Version: " + package.version, EditorStyles.boldLabel);
                            if (GUILayout.Button("Author: " + package.author, linkButtonStyle)) Application.OpenURL(repositoryItem.server + package.author);
                            if (GUILayout.Button(" - Source", linkButtonStyle)) Application.OpenURL(repositoryItem.server + package.author + "/" + package.packageName);
                        }
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.LabelField("Description: " + package.description);
                    }
                    EditorGUILayout.EndVertical();
                }
                if (GUILayout.Button("download", buttonWidth, buttonHeight))
                {
                    EditorCoroutine.start(DownloadPackage(package));
                }
                EditorGUILayout.EndHorizontal();
            }
            DrawLine();
        }

        IEnumerator DownloadPackage(RepositoryPackage package)
        {
            var url = repositoryItem.server + "/" + package.author + "/" + package.packageName + "/archive/" + package.version + ".zip";
            Debug.Log(url);
            WWW downloadFile = new WWW(url);
            //shows download progress
            while (!downloadFile.isDone)
            {
                Debug.Log("Downloading...");
                yield return null;
                if (downloadFile.isDone)
                {
                    Debug.Log("Finished...");
                }
            }
            if (!Directory.Exists(saveDirectory)) Directory.CreateDirectory(saveDirectory);
            string fullPath = saveDirectory + package.packageName + ".zip";
            Debug.Log(fullPath);
            File.WriteAllBytes(fullPath, downloadFile.bytes);
            ZipUtil.Unzip(fullPath, saveDirectory);
            AssetDatabase.Refresh();
        }
    }
}