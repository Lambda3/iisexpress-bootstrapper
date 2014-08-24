using System;
using System.IO;
using System.Linq;

namespace IISExpressBootstrapper
{
    public class Locator
    {
        public static string GetFullPath(string webApplicationName)
        {
            var solutionFolder = GetSolutionFolderPath();
            var projectPath = FindSubFolderPath(solutionFolder, webApplicationName);

            return projectPath;
        }

        private static string GetSolutionFolderPath()
        {
            var directory = new DirectoryInfo(Environment.CurrentDirectory);

            if (IsTfsBuild(directory))
                return new DirectoryInfo(directory.FullName + "\\_PublishedWebsites").FullName;

            while (directory != null && directory.GetFiles("*.sln").Length == 0)
            {
                directory = directory.Parent;
            }

            if (directory == null)
                throw new DirectoryNotFoundException();

            return directory.FullName;
        }

        private static bool IsTfsBuild(FileSystemInfo currentDirectory)
        {
            if (currentDirectory == null || !currentDirectory.Name.ToLower().Equals("bin")) return false;
            
            return Directory.Exists(currentDirectory.FullName + "\\_PublishedWebsites");
        }

        private static string FindSubFolderPath(string rootFolderPath, string folderName)
        {
            var directory = new DirectoryInfo(rootFolderPath);

            directory = (directory.GetDirectories("*", SearchOption.AllDirectories)
                .Where(folder => String.Equals(folder.Name, folderName, StringComparison.CurrentCultureIgnoreCase)))
                .FirstOrDefault();

            if (directory == null)
            {
                throw new DirectoryNotFoundException("Could not infer the web application folder path.");
            }

            return directory.FullName;
        }
    }
}
