namespace OpenFramework.Editor
{
    using System.Collections.Generic;
    using UnityEngine;
    public class OpenFrameworkResources : ScriptableObject
    {
        public TextAsset repository;
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