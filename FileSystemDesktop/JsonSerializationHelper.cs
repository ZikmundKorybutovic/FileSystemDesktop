using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace FileSystemAnalyzer
{
    public class JsonSerializationHelper : ISerializationHelper
    {
        #region Constructors
        public JsonSerializationHelper()
        {

        }
        #endregion

        #region Public methods
        public object Deserialize<T>(string fullPath)
        {
            using (StreamReader sr = new StreamReader(fullPath))
            {
                return JsonConvert.DeserializeObject<T>(sr.ReadToEnd());
            }
        }

        public void Serialize(object obj, string fullPath)
        {
            JsonSerializer jSerializer = new JsonSerializer();
            using (StreamWriter sw = new StreamWriter(fullPath))
            using (JsonWriter jwriter = new JsonTextWriter(sw))
            {
                jSerializer.Serialize(jwriter, obj);
            }
        }
        #endregion
    }
}