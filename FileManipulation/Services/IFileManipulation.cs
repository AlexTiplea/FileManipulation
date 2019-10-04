using System.Collections.Generic;

namespace FileManipulation.Services
{
    public interface IFileManipulation
    {
        IEnumerable<string> GetAllFiles();

        bool IsValid(string fileName);
    }
}
