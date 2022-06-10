using System;
using System.IO;
using System.Linq;
using System.Reflection;

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
            var directory = new DirectoryInfo(GetAssemblyDirectory());

            if (IsTfsBuild(directory))
                return new DirectoryInfo(directory.FullName + "\\_PublishedWebsites").FullName;

            while (directory != null && directory.GetFiles("*.sln").Length == 0)
                directory = directory.Parent;

            return directory == null ? throw new DirectoryNotFoundException() : directory.FullName;
        }

        public static string GetAssemblyDirectory()
        {
            var codeBase = Assembly.GetExecutingAssembly().CodeBase;
            var uri = new UriBuilder(codeBase);
            var path = Uri.UnescapeDataString(uri.Path);
            return Path.GetDirectoryName(path);
        }

        private static bool IsTfsBuild(FileSystemInfo currentDirectory) =>
            currentDirectory != null && currentDirectory.Name.ToLower().Equals("bin") && Directory.Exists(currentDirectory.FullName + "\\_PublishedWebsites");

        private static string FindSubFolderPath(string rootFolderPath, string folderName)
        {
            var directory = new DirectoryInfo(rootFolderPath);

            directory = directory.GetDirectories("*", SearchOption.AllDirectories)
                .Where(folder => string.Equals(folder.Name, folderName, StringComparison.CurrentCultureIgnoreCase))
                .FirstOrDefault();

            return directory == null
                ? throw new DirectoryNotFoundException("Could not infer the web application folder path.")
                : directory.FullName;
        }
    }
}
