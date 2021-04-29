using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Data;
using Excel;

namespace lgu3d.Editor
{
    public enum DataFileType
    {
        Excel = 1,
        Xml = 2,
        Josn = 3,
        Asset = 4,
    }
    /// <summary>
    /// 导入文件类型
    /// </summary>
    public enum ImputFileType
    {
        Excel = 1,
        Xml = 2,
        Json = 3,
    }
    /// <summary>
    /// 导出文件类型
    /// </summary>
    public enum ExportFileType
    {
        Asset = 4,
    }

    public class ExeclImportTools : CompositeTools
    {
        public List<ExcelFileImportInfo> Files;
        public ImputFileType importDataType = ImputFileType.Excel;
        public ExportFileType exportDataType = ExportFileType.Asset;
        public string ScannPath = "";
        public string SavePath = "";
        public ExeclImportTools(EditorWindow _MyWindow)
            : base(_MyWindow)
        {
            Files = new List<ExcelFileImportInfo>();
            Tools = new Dictionary<string, BaseTools>
            {
                { "扫描Execl文件",new ExcelImportWindow(_MyWindow,this)}
            };
        }
        /// <summary>
        /// 检索模块资源目录
        /// </summary>
        /// <param name="_Path"></param>
        public void ScannPathDirectory()
        {
            Files.Clear();
            string[] fileList = Directory.GetFileSystemEntries(Application.dataPath + ScannPath);
            string[] filter;
            switch (importDataType) {
                case ImputFileType.Xml:
                    filter = new string[] { ".xml" };
                    break;
                case ImputFileType.Excel:
                    filter = new string[] { ".xlsx" };
                    break;
                case ImputFileType.Json:
                    filter = new string[] { ".json" };
                    break;
                default:
                    filter = new string[] { ".xml",".xlsx",".json" };
                    break;

            }
            foreach (string file in fileList)
            {
                ExcelFileImportInfo FilerConfig = new ExcelFileImportInfo(null, file, filter);
                if (FilerConfig.GetFileType() != FileType.Useless)
                {
                    Files.Add(FilerConfig);
                }
            }
        }

        public void ImportFile() {
            foreach (var item in Files)
            {
                DataShift(item);
            }
        }

        /// <summary>
        /// 数据转换
        /// </summary>
        private void DataShift(IDirFileInfo filetree)
        {
            ExcelFileImportInfo P = filetree as ExcelFileImportInfo;
            if (P.GetFileType() == FileType.Folder)
            {
                for (int i = 0; i < P.GetChilds().Count; i++)
                {
                    DataShift(filetree.GetChilds()[i]);
                }
            }
            else if (P.GetFileType() == FileType.DataFile && P.IsExport)
            {
                string savepath = string.Empty;
                switch (P.DataType)
                {
                    case DataFileType.Excel:
                        switch ((DataFileType)exportDataType)
                        {
                            case DataFileType.Xml:
                                savepath = SavePath + filetree.GetPath().Replace(".xlsx", ".xml");
                                ExcelToXml(filetree.GetPath(), savepath);
                                break;
                            case DataFileType.Josn:
                                savepath = SavePath + filetree.GetPath().Replace(".xlsx", ".json");
                                ExcelToJson(filetree.GetPath(), savepath);
                                break;
                            case DataFileType.Asset:
                                savepath = SavePath + filetree.GetPath().Replace(".xlsx", ".asset");
                                ExcelToAsset(filetree.GetPath(), savepath);
                                break;
                        }
                        break;
                    case DataFileType.Xml:
                        switch ((DataFileType)exportDataType)
                        {
                            case DataFileType.Asset:
                                savepath = SavePath + filetree.GetPath().Replace(".xml", ".asset");
                                XmlToAsset(filetree.GetPath(), savepath);
                                break;
                        }
                        break;
                    case DataFileType.Josn:
                        switch ((DataFileType)exportDataType)
                        {
                            case DataFileType.Asset:
                                savepath = SavePath + filetree.GetPath().Replace(".json", ".asset");
                                JsonToAsset(filetree.GetPath(), savepath);
                                break;
                        }
                        break;
                }
            }
        }


        #region Excel 数据文件转换
        private static void ExcelToXml(string ExcelFile, string XmlFile)
        {
            DataSet result = ExcelDataTools.ReadExcelFile(ExcelFile);
            string XmlStr = ExcelDataTools.ExcelToXmlStr(result);
            ExcelDataTools.WirteStrToFile(XmlFile, XmlStr);
        }

        private static void ExcelToJson(string ExcelFile, string JsonFile)
        {
            DataSet result = ExcelDataTools.ReadExcelFile(ExcelFile);
            string JonsStr = ExcelDataTools.ExcelToJsonStr(result);
            ExcelDataTools.WirteStrToFile(JsonFile, JonsStr);
        }

        private static void ExcelToAsset(string ExcelFile, string AssetFile)
        {
            string ClassName = ExcelFile.Substring(ExcelFile.LastIndexOf("/") + 1);
            ClassName = ClassName.Substring(0, ClassName.LastIndexOf("."));
            string _AssetFile = AssetFile.Substring(AssetFile.LastIndexOf("Assets"));
            DataSet result = ExcelDataTools.ReadExcelFile(ExcelFile);
            ScriptableObject ddata = ExcelDataTools.DataSetToAsset(result, ClassName);
            AssetDatabase.CreateAsset(ddata, _AssetFile);
        }
        #endregion

        #region Xml数据文件转换
        private static void XmlToAsset(string XmlFile, string AssetFile)
        {
            string ClassName = XmlFile.Substring(XmlFile.LastIndexOf("/") + 1);
            ClassName = ClassName.Substring(0, ClassName.LastIndexOf("."));
            string _AssetFile = AssetFile.Substring(AssetFile.LastIndexOf("Assets"));
            DataSet result = ExcelDataTools.ReadXmlFile(XmlFile);
            ScriptableObject ddata = ExcelDataTools.DataSetToAsset(result, ClassName);
            AssetDatabase.CreateAsset(ddata, _AssetFile);
        }
        #endregion

        #region Json数据文件转换
        private static void JsonToAsset(string JsonFile, string AssetFile)
        {
            string ClassName = JsonFile.Substring(JsonFile.LastIndexOf("/") + 1);
            ClassName = ClassName.Substring(0, ClassName.LastIndexOf("."));
            string _AssetFile = AssetFile.Substring(AssetFile.LastIndexOf("Assets"));
            DataSet result = ExcelDataTools.ReadJsonFile(JsonFile);
            ScriptableObject ddata = ExcelDataTools.DataSetToAsset(result, ClassName);
            AssetDatabase.CreateAsset(ddata, _AssetFile);
        }
        #endregion

    }

}
