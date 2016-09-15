namespace EnergyTrading.FileProcessing.Verification
{
    using System.IO;

    using EnergyTrading.Configuration;

    public static class FileInfoExtensions
    {
        public static bool IsTestFile(this FileInfo fileInfo, IConfigurationManager configurationManager)
        {
            return fileInfo != null && fileInfo.Name.IsTestFile(configurationManager);
        }

        public static bool IsTestFile(this string fileName, IConfigurationManager configurationManager)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return false;
            }

            var prefix = configurationManager.GetVerificationPrefix();
            return fileName.StartsWith(prefix);
        }

        public static string GetTestIdFromFileName(this FileInfo fileInfo)
        {
            return fileInfo?.Name.GetTestIdFromFileName();
        }

        public static string GetTestIdFromFileName(this string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return null;
            }
            var parts = Path.GetFileNameWithoutExtension(fileName).Split('_');
            return parts.Length > 1 ? parts[1] : null;
        }
    }
}