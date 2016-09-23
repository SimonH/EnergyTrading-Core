using System;
using System.IO;
using EnergyTrading.Configuration;
using EnergyTrading.FileProcessing.Verification;
using EnergyTrading.Wrappers;

namespace EnergyTrading.FileProcessing
{
    public class DeleteTestFilePostProcessor : IFilePostProcessor
    {
        private readonly IFile _fileSystem;
        private readonly IConfigurationManager _configurationManager;
        public DeleteTestFilePostProcessor(IFile fileSystem, IConfigurationManager configurationManager)
        {
            if (fileSystem == null)
            {
                throw new ArgumentNullException(nameof(fileSystem));
            }
            if (configurationManager == null)
            {
                throw new ArgumentNullException(nameof(configurationManager));
            }
            _configurationManager = configurationManager;
            _fileSystem = fileSystem;
        }

        public void PostProcess(string outputFile, bool successful)
        {
            if (_fileSystem.Exists(outputFile) && Path.GetFileName(outputFile).IsTestFile(_configurationManager))
            {
                _fileSystem.Delete(outputFile);
            }
        }
    }
}