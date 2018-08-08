namespace OpenPackageManager.Editor
{
    using System.Collections.Generic;

    [System.Serializable]
    public class OpenPackageFileFormat
    {
        public string AssetPath;
        public List<string> Dependencies;
        public List<string> Tags;

        public OpenPackageFileFormat()
        {
            Dependencies = new List<string>();
            Tags = new List<string>();
        }
    }
}