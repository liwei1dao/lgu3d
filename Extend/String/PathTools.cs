using System.IO;

namespace lgu3d
{
    public static class PathTools
    {
        public static bool IsKeepFile(string _Path)
        {
            return File.Exists(_Path);
        }

        public static bool IsDirectory(string _Path)
        {
            return Directory.Exists(_Path);
        }

        public static string GetDirectory(string _Path)
        {
            return _Path.Substring(0, _Path.LastIndexOf("/"));
        }

        public static string GetPathFolderName(string _Path)
        {
            return Path.GetFileName(_Path);
        }

        /// <summary>
        /// 检测文件是否是指定后缀文件
        /// </summary>
        /// <param name="_FlieName"></param>
        /// <param name="Suffix"></param>
        /// <returns></returns>
        public static bool CheckSuffix(string _FlieName, string[] Suffix)
        {
            for (int i = 0; i < Suffix.Length; i++)
            {
                if (_FlieName.EndsWith(Suffix[i]))
                    return true;
            }
            return false;
        }
    }
}
