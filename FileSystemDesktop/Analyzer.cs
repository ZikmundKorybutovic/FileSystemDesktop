using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System;
using System.Security.Cryptography;

namespace FileSystemAnalyzer
{
    public class Analyzer
    {
        /// <summary>
        /// Data container for analysis results
        /// </summary>
        public struct AnalysisResult
        {
            public List<FolderItem> DeletedFolders;
            public List<FileItem> AddedFiles;
            public List<FileItem> ModifiedFiles;
            public List<FileItem> DeletedFiles;
            public string Comment;

            public AnalysisResult(
                string comment,
                List<FolderItem> deletedFolders,
                List<FileItem> addedFiles,
                List<FileItem> modifiedFiles,
                List<FileItem> deletedFiles)
            {
                Comment = comment;
                DeletedFolders = deletedFolders;
                AddedFiles = addedFiles;
                ModifiedFiles = modifiedFiles;
                DeletedFiles = deletedFiles;
            }

            public AnalysisResult(string comment)
            {
                Comment = comment;
                DeletedFolders = null;
                AddedFiles = null;
                ModifiedFiles = null;
                DeletedFiles = null;
            }
        }

        #region Constants
        /// <summary>
        /// Name of the json file with data from previous analyses
        /// </summary>
        private const string JSONFILENAME = "FileSystemAnalyzer.json";

        /// <summary>
        /// Name of the json file containing subfolders' data
        /// </summary>
        private const string JSONFOLDERSNAME = "FileSystemAnalyzerFolders.json";
        #endregion

        #region Fields
        /// <summary>
        /// Path to the folder that should be analyzed
        /// </summary>
        private string mainPath;

        /// <summary>
        /// Determines if any changes were detected during the analysis
        /// </summary>
        private bool isDirty;
        #endregion

        #region Constructors
        public Analyzer(string path)
        {
            mainPath = path;
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Extract path to the file/subdirectory excluding the main directory path
        /// </summary>
        /// <param name="inPath">Full path to the file/subdirectory</param>
        /// <returns>Trimmed string</returns>
        private string trimPath(string inPath)
        {
            return inPath.Substring(mainPath.Length + 1, inPath.Length - (mainPath.Length + 1));
        }

        /// <summary>
        /// Helper method to provide the hash of the file
        /// </summary>
        /// <param name="fs">File stream to the file to be hashed</param>
        /// <returns></returns>
        private byte[] computeHash(FileStream fs)
        {
            var hashfunction = new SHA1CryptoServiceProvider();
            var hash = hashfunction.ComputeHash(fs);
            fs.Close();

            return hash;
        }

        /// <summary>
        /// Get current folders from the base directory for comparison
        /// </summary>
        /// <param name="baseFolder">DirectoryInfo object of mainPath</param>
        /// <returns>List of FolderItems</returns>
        private List<FolderItem> getCurrentFolders(DirectoryInfo baseFolder)
        {
            var allFolders = baseFolder.GetDirectories("*", SearchOption.AllDirectories);
            var resultFolders = new List<FolderItem>();

            foreach (var folder in allFolders)
            {
                resultFolders.Add(new FolderItem(trimPath(folder.FullName)));
            }

            return resultFolders;
        }

        /// <summary>
        /// Get current files from the base directory for comparison
        /// </summary>
        /// <param name="baseFolder">DirectoryInfo object of mainPath</param>
        /// <returns>List of FileItems</returns>
        private List<FileItem> getCurrentFiles(DirectoryInfo baseFolder)
        {
            var files = baseFolder.GetFiles("*", SearchOption.AllDirectories).ToList();
            files = files.Where(f => f.Name != JSONFILENAME && f.Name != JSONFOLDERSNAME).ToList();

            var resultFiles = new List<FileItem>();

            foreach (var file in files)
            {
                resultFiles.Add(new FileItem(trimPath(file.FullName), computeHash(new FileStream(file.FullName, FileMode.Open)), file.LastWriteTime));
            }

            return resultFiles;
        }

        #region Serialization
        private enum ItemType
        {
            File,
            Folder
        }

        /// <summary>
        /// Deserialize files using helper class
        /// </summary>
        /// <param name="path">Path to the serialized file</param>
        private object deserializeItems(string path, ItemType itemType)
        {
            ISerializationHelper serialization = new JsonSerializationHelper();
            
            switch (itemType)
            {
                case ItemType.File:
                    return serialization.Deserialize<List<FileItem>>(Path.Combine(path));
                case ItemType.Folder:
                    return serialization.Deserialize<List<FolderItem>>(Path.Combine(path));
                default:
                    return null;
            }
        }

        /// <summary>
        /// Serialize items using helper class
        /// </summary>
        /// <param name="path">Path to the file to be created</param>
        private void serializeItems(string path, List<FileItem> currentFiles, List<FolderItem> currentFolders)
        {
            ISerializationHelper serialization = new JsonSerializationHelper();
            serialization.Serialize(currentFiles, Path.Combine(path, JSONFILENAME));
            serialization.Serialize(currentFolders, Path.Combine(path, JSONFOLDERSNAME));
        }
        #endregion

        #region Analysis methods
        /// <summary>
        /// Analyzes differences between the original and current state of file items
        /// </summary>
        /// <param name="originalFiles">Original state</param>
        /// <param name="currentFiles">Current state</param>
        /// <returns>Lists of added, modified and deleted files</returns>
        private (List<FileItem> addedFiles, List<FileItem> modifiedFiles, List<FileItem> deletedFiles) analyzeFiles(List<FileItem> originalFiles, List<FileItem> currentFiles)
        {
            var addedFiles = new List<FileItem>();
            var modifiedFiles = new List<FileItem>();
            var deletedFiles = new List<FileItem>();

            foreach (var currentFile in currentFiles)
            {
                var originalFile = originalFiles.FirstOrDefault(f => f.Name == currentFile.Name);

                if (originalFile == null)
                {
                    addedFiles.Add(currentFile);
                    isDirty = true;
                }
                else if (!originalFile.Equals(currentFile))
                {
                    currentFile.Version += originalFile.Version;
                    modifiedFiles.Add(currentFile);
                    isDirty = true;
                }

                originalFiles.Remove(originalFile);
            }

            // if some files remain in the original list, they must have been deleted
            if (originalFiles.Count > 0)
            {
                deletedFiles = originalFiles;
                isDirty = true;
            }

            return (addedFiles, modifiedFiles, deletedFiles);
        }

        /// <summary>
        /// Analyzes differences between the original and current state of folder items
        /// </summary>
        /// <param name="originalFolders">Original state</param>
        /// <param name="currentFolders">Current state</param>
        /// <returns>List of deleted folders</returns>
        private List<FolderItem> analyzeFolders(List<FolderItem> originalFolders, List<FolderItem> currentFolders)
        {
            var deletedFolders = new List<FolderItem>();
            // we just keep track of deleted subfolders
            foreach (var originalFolder in originalFolders)
            {
                var folder = currentFolders.FirstOrDefault(f => f.Name == originalFolder.Name);
                if (folder == null)
                {
                    deletedFolders.Add(originalFolder);
                    isDirty = true;
                }
            }

            return deletedFolders;
        }
        #endregion

        #endregion

        #region Public methods

        public AnalysisResult DoAnalysis()
        {
            DirectoryInfo baseFolder = new DirectoryInfo(mainPath);

            if (!baseFolder.Exists)
            {
                return new AnalysisResult("The requested directory does not exist.");
            }

            var currentFiles = getCurrentFiles(baseFolder);
            var currentFolders = getCurrentFolders(baseFolder);

            var jsonFile = new FileInfo(Path.Combine(mainPath, JSONFILENAME));
            if (!jsonFile.Exists)
            {
                // no file means first run
                serializeItems(mainPath, currentFiles, currentFolders);
                return new AnalysisResult("New directory, no changes.");
            }

            isDirty = false;

            var originalFiles = deserializeItems(Path.Combine(mainPath, JSONFILENAME), ItemType.File) as List<FileItem>;
            var (addedFiles, modifiedFiles, deletedFiles) = analyzeFiles(originalFiles, currentFiles);

            var originalFolders = deserializeItems(Path.Combine(mainPath, JSONFOLDERSNAME), ItemType.Folder) as List<FolderItem>;
            var folderDifferences = analyzeFolders(originalFolders, currentFolders);

            if (isDirty)
            {
                serializeItems(mainPath, currentFiles, currentFolders);
                return new AnalysisResult("", folderDifferences, addedFiles, modifiedFiles, deletedFiles);
            }
            else
            {
                return new AnalysisResult("Analysis finished. No changes detected.");
            }
        }       

        #endregion

    }
}