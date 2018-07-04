# OpenPackageManager

## How to use:
- clone this repository inside unity Asset folder. (or any subfolder)
 
 ``` git clone https://github.com/omid3098/OpenPackageManager.git ```
- choose Window > OpenPackageManager
- install any package you want.



## How to add my own package to the list?
- you need to have a github repository for your asset.
- your github repository should not be a Unity project. it sould only contain your package data.
you can compare [a valid repository](https://github.com/omid3098/OpenAudio) with [an invalid](https://github.com/omid3098/OpenWatcher) one

![image](https://user-images.githubusercontent.com/6388730/42286418-10a8bf78-7fc8-11e8-94e7-318a7afa3525.png)

- you nees to have at least one release in your repository. [(how to create releases)](https://help.github.com/articles/creating-releases)
- create new issue with these info:

issue title: [NewPackage] Package name
issue content: 

anything you want to say + repository github link + this Json:

```
        {
            "name": "Package display name",
            "description": "Package description",
            "packageName": "Github repository name",
            "version": "Latest released version in github",
            "author": "Your Github username"
        }
```
Example: 
```
        {
            "name": "Open Audio",
            "description": "An easy to use audio manager for unity",
            "packageName": "OpenAudio",
            "version": "1.0.0",
            "author": "omid3098"
        }
```

[Check sample issue](https://github.com/omid3098/OpenPackageManager/issues/1)