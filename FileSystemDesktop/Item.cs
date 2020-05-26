using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FileSystemAnalyzer
{
    /// <summary>
    /// Base class for File and Folder
    /// </summary>
    public abstract class Item
    {
        #region Fields

        /// <summary>
        /// Name as per File.Name or Directory.Name property
        /// </summary>
        private string name;

        #endregion

        #region Constructors

        /// <summary>
        /// Basic constructor for the class
        /// </summary>
        /// <param name="name"></param>
        public Item(string name)
        {
            this.name = name;
        }

        #endregion

        #region Public properties

        public string Name
        {
            get => name;
            set => name = value;
        }

        #endregion
    }
}