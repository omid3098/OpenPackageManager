namespace OpenPackageManager.Editor
{
    using System.Collections.Generic;
    using UnityEngine;

    public class OpenPackageManagerConfig
    {
        public static string DownloadDirectory = @"/Scripts/OpenPackageManager/packages/downloads";
        public static string RepositoryLink = "https://raw.githubusercontent.com/omid3098/OpenPackageManager/master/packages/repository.json";
    }

    [System.Serializable]
    public class RepositoryItem
    {
        public List<RepositoryPackage> packages;
        public string server;
        public RepositoryItem()
        {
            packages = new List<RepositoryPackage>();
        }
    }

    [System.Serializable]
    public class RepositoryPackage
    {
        public string name;
        public string description;
        public string packageName;
        public string version;
        public string author;
    }
}