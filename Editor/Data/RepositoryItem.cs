namespace OpenPackageManager.Editor
{
    using System.Collections.Generic;

    [System.Serializable]
    public class RepositoryItem
    {
        public List<string> packages;
        public string server;
        public RepositoryItem()
        {
            packages = new List<string>();
        }
    }
}