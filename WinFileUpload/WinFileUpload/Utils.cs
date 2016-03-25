using System;
using System.Collections.Generic;
using System.IO;

namespace Shared
{
    class Utils
    {
        public string FileName { get; set; }
        public string TempFolder { get; set; }
        public int MaxFileSizeMB { get; set; }
        public List<String> FileParts { get; set; }

        public Utils()
        {
            FileParts = new List<string>();
        }

        /// <summary>
        /// Split = get number of files 
        /// .. Name = original name + ".part_N.X" (N = file part number, X = total files)
        /// </summary>
        /// <returns></returns>
        public bool SplitFile()
        {
            // improvement - make more robust
            bool rslt = false;
            string BaseFileName = Path.GetFileName(FileName);
            int BufferChunkSize = MaxFileSizeMB * (1024 * 1024);
            const int READBUFFER_SIZE = 1024;
            byte[] FSBuffer = new byte[READBUFFER_SIZE];
            // adapted from: http://stackoverflow.com/questions/3967541/how-to-split-large-files-efficiently
            using (FileStream FS = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                int TotalFileParts = 0;
                if (FS.Length < BufferChunkSize)
                {
                    TotalFileParts = 1;
                }
                else
                {
                    float PreciseFileParts = ((float)FS.Length / (float)BufferChunkSize);
                    TotalFileParts = (int)Math.Ceiling(PreciseFileParts);
                }

                int FilePartCount = 0;
                while (FS.Position < FS.Length)
                {
                    string FilePartName = String.Format("{0}.part_{1}.{2}", BaseFileName, (FilePartCount + 1).ToString(), TotalFileParts.ToString());
                    FilePartName = Path.Combine(TempFolder, FilePartName);
                    FileParts.Add(FilePartName);
                    using (FileStream FilePart = new FileStream(FilePartName, FileMode.Create))
                    {
                        int bytesRemaining = BufferChunkSize;
                        int bytesRead = 0;
                        while (bytesRemaining > 0 && (bytesRead = FS.Read(FSBuffer, 0, Math.Min(bytesRemaining, READBUFFER_SIZE))) > 0)
                        {
                            FilePart.Write(FSBuffer, 0, bytesRead);
                            bytesRemaining -= bytesRead;
                        }
                    }
                  FilePartCount++;
                }

            }
                return rslt;
        }

    }

    public struct SortedFile
    {
        public int FileOrder { get; set; }
        public String FileName { get; set; }
    }
}
