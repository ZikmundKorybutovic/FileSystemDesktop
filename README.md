The FileSystem Analyzer

Simple desktop application developed to analyze files and subfolders in a local directory.

In the first run the application maps all files and subfolders and saves the current state into two json files, then it 
displays changes made since the last run (the jsons are not accounted for).

For each file a version number is saved starting at 1 that is raised with each modification. 
The application uses combination of SHA1 hash and file property LastWriteTime to detect file content changes.
File name consists of the full file path trimmed of the root folder path - this allows the application to differentiate
among files with the same names stored in different subfolders.

The key to reading reports is following:
[A] added files
[M] modified files (incl. version #)
[D] deleted files or subfolders (as per subfolders - only deleted ones are reported)

Possible limitations
1) The app has to have a write access to the target directory, since the json files are saved there.

2) Computing and comparing the hashes can slow down the analysis process in case of extensive file count or size.
