using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemAnalyzer
{
    public interface ISerializationHelper
    {
        object Deserialize<T>(string fullPath);
        void Serialize(object obj, string fullPath);
    }
}
