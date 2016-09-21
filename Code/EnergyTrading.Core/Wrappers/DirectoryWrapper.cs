﻿using System.Collections.Generic;

namespace EnergyTrading.Wrappers
{
    using System.IO;

    using EnergyTrading.Attributes;

    [ExcludeFromCoverage]
    public class DirectoryWrapper : IDirectory
    {
        public bool Exists(string path)
        {
            return Directory.Exists(path);
        }

        public DirectoryInfo CreateDirectory(string path)
        {
            return Directory.CreateDirectory(path);
        }

        public void Delete(string path)
        {
            Directory.Delete(path);
        }

        public IEnumerable<string> EnumerateFiles(string path)
        {
            return Directory.EnumerateFiles(path);
        }

        public IEnumerable<string> EnumerateDirectories(string path)
        {
            return Directory.EnumerateDirectories(path);
        }
    }
}