namespace OpenPackageManager.Editor
{
    using System;
    using System.Collections.Generic;

    [System.Serializable]
    public class RepositoryItem
    {
        public List<string> standardPackages;
        public List<RepositoryPackage> packages;
        public string server;
        public RepositoryItem()
        {
            packages = new List<RepositoryPackage>();
            standardPackages = new List<string>();
        }
    }

    [System.Serializable]
    public class Owner
    {
        public string login;
        public int id;
        public string node_id;
        public string avatar_url;
        public string gravatar_id;
        public string url;
        public string html_url;
        public string followers_url;
        public string following_url;
        public string gists_url;
        public string starred_url;
        public string subscriptions_url;
        public string organizations_url;
        public string repos_url;
        public string events_url;
        public string received_events_url;
        public string type;
        public bool site_admin;
    }

    [System.Serializable]
    public class RepositoryPackage
    {
        public int id;
        public string node_id;
        public string name;
        public string full_name;
        public Owner owner;
        public bool @private;
        public string html_url;
        public string description;
        public bool fork;
        public string url;
        public string forks_url;
        public string keys_url;
        public string collaborators_url;
        public string teams_url;
        public string hooks_url;
        public string issue_events_url;
        public string events_url;
        public string assignees_url;
        public string branches_url;
        public string tags_url;
        public string blobs_url;
        public string git_tags_url;
        public string git_refs_url;
        public string trees_url;
        public string statuses_url;
        public string languages_url;
        public string stargazers_url;
        public string contributors_url;
        public string subscribers_url;
        public string subscription_url;
        public string commits_url;
        public string git_commits_url;
        public string comments_url;
        public string issue_comment_url;
        public string contents_url;
        public string compare_url;
        public string merges_url;
        public string archive_url;
        public string downloads_url;
        public string issues_url;
        public string pulls_url;
        public string milestones_url;
        public string notifications_url;
        public string labels_url;
        public string releases_url;
        public string deployments_url;
        public DateTime created_at;
        public DateTime updated_at;
        public DateTime pushed_at;
        public string git_url;
        public string ssh_url;
        public string clone_url;
        public string svn_url;
        public object homepage;
        public int size;
        public int stargazers_count;
        public int watchers_count;
        public string language;
        public bool has_issues;
        public bool has_projects;
        public bool has_downloads;
        public bool has_wiki;
        public bool has_pages;
        public int forks_count;
        public object mirror_url;
        public bool archived;
        public int open_issues_count;
        public object license;
        public int forks;
        public int open_issues;
        public int watchers;
        public string default_branch;
        public int network_count;
        public int subscribers_count;
    }
}