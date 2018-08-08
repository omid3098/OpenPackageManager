namespace OpenPackageManager.Editor
{
    using UnityEngine;
    using System.Collections.Generic;
    using System.IO;

    public class OpenPackageManagerController
    {
        public RepositoryItem repository { get; private set; }
        public List<GithubPackage> allPackages { get; private set; }

        public void ParseRepository(string data)
        {
            // parse repository
            repository = JsonUtility.FromJson<RepositoryItem>(data);
            allPackages = new List<GithubPackage>();
            // allPackages.AddRange(repositoryItem.packages);
            for (int i = 0; i < repository.packages.Count; i++)
            {
                var packageInfo = repository.packages[i].Split('/');
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
                    var url = "https://api.github.com/repos/" + repository.packages[i];
                    Debug.Log(url);
                    EditorCoroutine.start(ConnectionTools.GetData(url, "", null, (_data) =>
                    {
                        File.WriteAllText(packageLocalJsonPath, _data);
                        ParsePackage(_data);
                    }, null));
                }
            }
        }

        void ParsePackage(string packageData)
        {
            var _package = JsonUtility.FromJson<GithubPackage>(packageData);
            allPackages.Add(_package);
        }
    }
}