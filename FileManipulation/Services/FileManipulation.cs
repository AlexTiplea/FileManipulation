using System;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FileManipulation.Services
{
    public class FileManipulation : IFileManipulation
    {
        private readonly IConfiguration _configuration;

        public FileManipulation(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IEnumerable<string> GetAllFiles()
        {
            var folderPath = _configuration.GetValue<string>("InputFolder");
            var fileList = new List<string>();

            if (Directory.Exists(folderPath))
            {
                var files = Directory.GetFiles(folderPath);
                foreach (var file in files)
                {
                    fileList.Add(Path.GetFileName(file));
                }
            }

            return fileList;
        }

        public bool IsValid(string fileName)
        {
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);

            switch (fileNameWithoutExtension)
            {
                case "valid":
                    DeleteFile(fileName);
                    return true;
                case "invalid":
                    MoveFile(fileName);
                    return false;
            }

            return false;
        }

        private void DeleteFile(string fileName)
        {
            var files = GetAllFiles();
            var folderPath = _configuration.GetValue<string>("InputFolder");

            if (files.Contains(fileName))
            {
                try
                {
                    File.Delete(Path.Combine(folderPath, fileName));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }

        private void MoveFile(string fileName)
        {
            var sourcePath = _configuration.GetValue<string>("InputFolder");
            var destinationPath = _configuration.GetValue<string>("ErrorFolder");
            
            var files = GetAllFiles();

            if (!Directory.Exists(destinationPath))
            {
                Directory.CreateDirectory(destinationPath);
            }

            if (files.Contains(fileName))
            {
                var sourceFile = Path.Combine(sourcePath, fileName);
                var destinationFile = Path.Combine(destinationPath, fileName);

                try
                {
                    File.Move(sourceFile, destinationFile);
                    File.Delete(Path.Combine(sourcePath, fileName));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }
    }
}
