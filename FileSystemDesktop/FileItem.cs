using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace FileSystemAnalyzer
{
    public class FileItem : Item
    {
        #region Fields
        /// <summary>
        /// Simple counter - incremented every time the file is changed
        /// </summary>
        private uint version = 1;

        /// <summary>
        /// Hashed file content to detect changes
        /// </summary>
        private byte[] controlHash;

        /// <summary>
        /// File property LastWriteTime
        /// </summary>
        private DateTime lastChanged;

        #endregion

        #region Constructors
        /// <summary>
        /// Basic constructor for new file - no need to supply version
        /// </summary>
        /// <param name="name"></param>
        /// <param name="lastChanged"></param>
        public FileItem(string name, byte[] hash, DateTime lastChanged) : base(name)
        {
            controlHash = hash;
            this.lastChanged = lastChanged;
        }

        [JsonConstructor]
        public FileItem(string name, byte[] hash, uint version, DateTime lastChanged) : this(name, hash, lastChanged)
        {
            this.version = version;
        }
        #endregion

        #region Public properties
        public uint Version
        {
            get => version;
            set => version = value;
        }

        public byte[] ControlHash
        {
            get => controlHash;
            set => controlHash = value;
        }

        public DateTime LastChanged
        {
            get => lastChanged;
            set => lastChanged = value;
        }

        #endregion

        #region Overriden operators
        public override bool Equals(object obj)
        {
            FileItem inFile = obj as FileItem;
            for (int i = 0; i < inFile.ControlHash.Length; i++)
            {
                if(inFile.ControlHash[i] != this.controlHash[i])
                {
                    return false;
                }
            }
            return inFile.LastChanged == this.lastChanged;
        }

        public override int GetHashCode()
        {
            return controlHash.GetHashCode();
        }

        #endregion
    }
}