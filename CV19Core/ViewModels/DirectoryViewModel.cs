using CV19Core.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CV19Core.ViewModels
{
    internal class DirectoryViewModel
    {
        private readonly DirectoryInfo _directoryInfo;

        public IEnumerable<DirectoryViewModel> SubDirectories => _directoryInfo
            .EnumerateDirectories()
            .Select(dirInfo => new DirectoryViewModel(dirInfo.FullName));

        public IEnumerable<FileViewModel> Files => _directoryInfo
           .EnumerateDirectories()
           .Select(file => new FileViewModel(file.FullName));

        public IEnumerable<object> DirectoryItems => 
            SubDirectories.Cast<object>().Concat(Files);

        public string Name  => _directoryInfo.Name;
        public string Path => _directoryInfo.FullName;
        public DateTime CreationDate => _directoryInfo.CreationTime;

        public DirectoryViewModel(string path) => _directoryInfo = new DirectoryInfo(path);
    }

    internal class FileViewModel : ViewModel
    {
        private readonly FileInfo _fileInfo;

        public string Name => _fileInfo.Name;
        public string Path => _fileInfo.FullName;
        public DateTime CreationDate => _fileInfo.CreationTime;


        public FileViewModel(string path) => _fileInfo = new FileInfo(path);
    }
}
