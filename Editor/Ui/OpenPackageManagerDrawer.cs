namespace OpenPackageManager.Editor
{
    using UnityEngine;
    using UnityEditor;
    using System.Collections.Generic;
    using System;

    public class OpenPackageManagerDrawer
    {
        private static Texture2D lineTexture;
        private static GUIStyle linkButtonStyle;
        private string filter;
        private static List<GithubPackage> showingPackages;
        private static GUILayoutOption buttonWidth;
        private static GUILayoutOption buttonHeight;
        private List<string> downloadingPackages;
        private float downloadProgress;
        private OpenPackageManagerController controller;

        public OpenPackageManagerDrawer(OpenPackageManagerController openPackageManagerWindow)
        {
            this.controller = openPackageManagerWindow;
            downloadingPackages = new List<string>();

            buttonWidth = GUILayout.Width(100);
            buttonHeight = GUILayout.Height(30);
            lineTexture = new Texture2D(1, 1);

            linkButtonStyle = new GUIStyle();
            linkButtonStyle.normal.textColor = Color.blue;
            linkButtonStyle.stretchWidth = true;
        }

        public void OnGUI()
        {
            GUILayout.Label("Packages", EditorStyles.boldLabel);
            SearchBar();
            if (string.IsNullOrEmpty(filter)) showingPackages = controller.allPackages;
            else showingPackages = controller.allPackages.FindAll(x => x.name.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0);

            foreach (var package in showingPackages)
            {
                DrawPackage(package);
            }
        }

        private void DrawLine()
        {
            GUILayout.Box(lineTexture, GUILayout.ExpandWidth(true), GUILayout.Height(1));
        }

        private void DrawPackage(GithubPackage package)
        {
            { // Draw package name, version, author and description 
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.BeginVertical();
                    {
                        EditorGUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.LabelField(package.name, EditorStyles.boldLabel);
                            if (GUILayout.Button("Author: " + package.owner.login, linkButtonStyle)) Application.OpenURL(controller.repository.server + package.owner.login);
                            if (GUILayout.Button("Source", linkButtonStyle)) Application.OpenURL(controller.repository.server + package.full_name);
                            EditorGUILayout.LabelField("Size: " + StringTools.GetBytesReadable(package.size * 1000));
                        }
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.LabelField("Description: " + package.description);
                    }
                    EditorGUILayout.EndVertical();
                }
                // Draw Download Button
                string downloadLable = "download";
                if (downloadingPackages.Contains(package.name)) downloadLable = downloadProgress + "%";
                if (GUILayout.Button(downloadLable, buttonWidth, buttonHeight))
                {
                    downloadingPackages.Add(package.name);
                    ConnectionTools.DownloadPackage(package, DownloadingProgress, (byteArray) =>
                    {
                        downloadingPackages.Remove(package.name);
                    }, (error) =>
                    {
                        downloadingPackages.Remove(package.name);
                    });
                }
                EditorGUILayout.EndHorizontal();
            }
            DrawLine();
        }

        public void DownloadingProgress(float value)
        {
            downloadProgress = value;
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
    }
}