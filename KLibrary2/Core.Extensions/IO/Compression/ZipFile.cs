using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ionic.Zip;
using ZipFileManager = Ionic.Zip.ZipFile;

namespace Keiho.IO.Compression
{
    // TODO: ファイルおよびフォルダーのフィルタリング。
    public static class ZipFile
    {
        public static void Compress(string zipFilePath, string sourceDirPath)
        {
            Compress(zipFilePath, sourceDirPath, null);
        }

        public static void Compress(string zipFilePath, string sourceDirPath, string password)
        {
            using (var zipFile = new ZipFileManager(SharedObjects.Encodings.ShiftJis))
            {
                if (password != null)
                {
                    zipFile.Password = password;
                }

                zipFile.AddDirectory(sourceDirPath);
                zipFile.Save(zipFilePath);
            }
        }

        public static void Extract(string zipFilePath, string targetDirPath)
        {
            Extract(zipFilePath, targetDirPath, null);
        }

        public static void Extract(string zipFilePath, string targetDirPath, string password)
        {
            using (var zipFile = ZipFileManager.Read(zipFilePath, new ReadOptions { Encoding = SharedObjects.Encodings.ShiftJis }))
            {
                if (password != null)
                {
                    zipFile.Password = password;
                }

                zipFile.ExtractAll(targetDirPath);
            }
        }
    }
}
