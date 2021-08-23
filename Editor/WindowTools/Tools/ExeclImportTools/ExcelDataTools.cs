using System;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEngine;
using Excel;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace lgu3d.Editor
{

  public static class ExcelDataTools
  {
    #region 字符串处理
    /// <summary>
    /// 过滤特殊字符
    /// </summary>
    /// <param name="s">字符串</param>
    /// <returns>json字符串</returns>
    private static string String2Json(String s)
    {
      StringBuilder sb = new StringBuilder();
      for (int i = 0; i < s.Length; i++)
      {
        char c = s.ToCharArray()[i];
        switch (c)
        {
          case '\"':
            sb.Append("\\\""); break;
          case '\\':
            sb.Append("\\\\"); break;
          case '/':
            sb.Append("\\/"); break;
          case '\b':
            sb.Append("\\b"); break;
          case '\f':
            sb.Append("\\f"); break;
          case '\n':
            sb.Append("\\n"); break;
          case '\r':
            sb.Append("\\r"); break;
          case '\t':
            sb.Append("\\t"); break;
          default:
            sb.Append(c); break;
        }
      }
      return sb.ToString();
    }

    /// <summary>
    /// 格式化字符型、日期型、布尔型
    /// </summary>
    /// <param name="str"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    private static string StringFormat(string str, Type type)
    {
      if (type == typeof(string))
      {
        str = String2Json(str);
        str = "\"" + str + "\"";
      }
      else if (type == typeof(DateTime))
      {
        str = "\"" + str + "\"";
      }
      else if (type == typeof(bool))
      {
        str = str.ToLower();
      }
      else if (type != typeof(string) && string.IsNullOrEmpty(str))
      {
        str = "\"" + str + "\"";
      }
      return str;
    }
    #endregion

    /// <summary>
    /// 写入字符串到文件
    /// </summary>
    /// <param name="FilePath"></param>
    /// <param name="WStr"></param>
    public static void WirteStrToFile(string FilePath, string WStr)
    {
      FileStream wstream = new FileStream(FilePath, FileMode.Create, FileAccess.Write);
      StreamWriter sw = new StreamWriter(wstream);
      sw.WriteLine(WStr);
      sw.Close();
      wstream.Close();
    }

    /// <summary>
    ///  Excel转换到Asset
    /// </summary>
    public static ScriptableObject DataSetToAsset(DataSet mData, string ClassName)
    {
      ScriptableObject ddata = ScriptableObject.CreateInstance(ClassName);
      FieldInfo DatasField = ddata.GetType().GetField("Datas");
      Type DataType = DatasField.FieldType.GetGenericArguments()[0];
      MethodInfo AddDataMethodInfo = ddata.GetType().GetMethod("AddData");
      DataRowCollection drc = mData.Tables[0].Rows;
      FieldInfo[] DataFields = new FieldInfo[mData.Tables[0].Columns.Count];
      for (int m = 0; m < mData.Tables[0].Columns.Count; m++)
      {
        DataFields[m] = DataType.GetField(mData.Tables[0].Columns[m].ColumnName);
      }
      for (int n = 0; n < drc.Count; n++)
      {
        object dataitem = DataType.GetConstructor(Type.EmptyTypes).Invoke(null);
        for (int m = 0; m < mData.Tables[0].Columns.Count; m++)
        {
          string value = drc[n][m].ToString();
          if (value != "" && DataFields[m] != null)
          {
            var obj = DataSerialization.GetValue(value, DataFields[m].FieldType);
            DataFields[m].SetValue(dataitem, obj);
          }
        }
        AddDataMethodInfo.Invoke(ddata, new object[] { dataitem });
      }
      return ddata;
    }

    /// <summary>
    /// 保存数据到excel
    /// </summary>
    /// <param name="tables"></param>
    /// <param name="file"></param>
    public static void SaveDataSetToCsv(DataSet tables, string file)
    {
      if (tables == null)
      {
        Debug.Log("表格出错了！！！！" + file);
        return;
      }
      DataTable table = tables.Tables[0];
      if (table == null)
      {
        Debug.Log("表格出错了！！！！" + file);
        return;
      }

      Debug.Log("转Excel...");
      string title = "";
      FileStream fs = new FileStream(file, FileMode.OpenOrCreate);

      StreamWriter sw = new StreamWriter(new BufferedStream(fs), System.Text.Encoding.Default);
      for (int i = 0; i < table.Columns.Count; i++)
      {

        title += table.Columns[i].ColumnName + "\t"; //栏位：自动跳到下一单元格
      }

      title = title.Substring(0, title.Length - 1) + "\n";

      sw.Write(title);

      foreach (DataRow row in table.Rows)
      {
        string line = "";
        for (int i = 0; i < table.Columns.Count; i++)
        {
          line += row[i].ToString().Trim() + "\t"; //内容：自动跳到下一单元格
        }
        line = line.Substring(0, line.Length - 1) + "\n";
        sw.Write(line);
      }
      sw.Close();
      fs.Close();
      Debug.Log("转Excel完成...");
    }



    #region Excel文件处理接口
    /// <summary>
    /// 读取Excel文件
    /// </summary>
    /// <param name="ExcelFile"></param>
    /// <returns></returns>
    public static DataSet ReadExcelFile(string ExcelFile)
    {
      FileStream stream = File.Open(ExcelFile, FileMode.Open, FileAccess.Read);
      IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
      DataSet result = excelReader.AsDataSet();
      return result;
    }

    /// <summary>
    /// 读取excel到xml字符串
    /// </summary>
    /// <param name="mData"></param>
    /// <returns></returns>
    public static string ExcelToXmlStr(DataSet mData)
    {
      StringBuilder XmlStr = new StringBuilder();
      DataRowCollection drc = mData.Tables[0].Rows;
      XmlStr.Append("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?><Root>");
      string lineInfo = "";
      for (int i = 2; i < drc.Count; i++)
      {
        lineInfo = "<Row>";
        XmlStr.Append(lineInfo);
        for (int j = 0; j < mData.Tables[0].Columns.Count; j++)
        {
          string value = drc[i][j].ToString();
          if (value != "")
          {
            lineInfo = string.Format("<{0}>{1}</{0}>", drc[0][j].ToString(), value);
            XmlStr.Append(lineInfo);
          }
        }
        lineInfo = "</Row>";
        XmlStr.Append(lineInfo);
      }
      XmlStr.Append("</Root>");
      return XmlStr.ToString();
    }

    /// <summary>
    /// Excel转换到Json
    /// </summary>
    /// <param name="mData"></param>
    /// <returns></returns>
    public static string ExcelToJsonStr(DataSet mData)
    {
      StringBuilder jsonString = new StringBuilder();
      jsonString.Append("[");
      DataRowCollection drc = mData.Tables[0].Rows;
      for (int i = 2; i < drc.Count; i++)
      {
        jsonString.Append("{");
        for (int j = 0; j < mData.Tables[0].Columns.Count; j++)
        {
          string strKey = drc[0][j].ToString();
          string strValue = drc[i][j].ToString();
          jsonString.Append("\"" + strKey + "\":");
          Type type = mData.Tables[0].Columns[j].DataType;
          strValue = StringFormat(strValue, type);
          if (j < mData.Tables[0].Columns.Count - 1)
          {
            jsonString.Append(strValue + ",");
          }
          else
          {
            jsonString.Append(strValue);
          }
        }
        jsonString.Append("},");
      }
      jsonString.Remove(jsonString.Length - 1, 1);
      jsonString.Append("]");
      return jsonString.ToString();
    }
    #endregion

    #region Xml文件处理接口
    public static DataSet ReadXmlFile(string XmlFile)
    {
      try
      {
        DataSet ds = new DataSet();
        //读取XML到DataSet 
        StreamReader sr = new StreamReader(XmlFile, Encoding.Default);
        ds.ReadXml(sr);
        sr.Close();
        if (ds.Tables.Count > 0)
          return ds;
        return null;
      }
      catch (Exception)
      {
        return null;
      }
    }
    #endregion

    #region Json文件处理接口  
    /// <summary>
    /// 读取Json 文件到 
    /// </summary>
    /// <param name="jsonFile"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static DataSet ReadJsonFile(string jsonFile)
    {
      try
      {
        DataSet ds = new DataSet();
        DataTable tb = null;
        //提取json的数据项JsonConvert
        string strJson = File.ReadAllText(jsonFile, Encoding.UTF8);
        Debug.Log("读取 Json 文件:" + jsonFile);
        JArray arrays = (JArray)JsonConvert.DeserializeObject(strJson);
        for (int i = 0; i < arrays.Count; i++)
        {
          string strData = arrays[i].ToString();
          strData = strData.Replace("  ", "").Replace(" ", "").Replace("\r\n", "").Replace(",\"", "*\"").Replace("\":", "\"#").ToString();
          strData = strData.Remove(0, 1);
          strData = strData.Remove(strData.Length - 1);
          string[] strRows = strData.Split('*');
          //创建表   
          if (tb == null)
          {
            tb = new DataTable();
            tb.TableName = "table1";
            foreach (string str in strRows)
            {
              var dc = new DataColumn();
              string[] strCell = str.Split('#');
              if (strCell[0].Substring(0, 1) == "\"")
              {
                int a = strCell[0].Length;
                dc.ColumnName = strCell[0].Substring(1, a - 2);
              }
              else
              {
                dc.ColumnName = strCell[0];
              }
              tb.Columns.Add(dc);
            }
            tb.AcceptChanges();
          }
          //增加内容   
          DataRow dr = tb.NewRow();
          for (int r = 0; r < strRows.Length; r++)
          {
            if (strRows[r].Length > 1)
            {
              dr[r] = strRows[r].Split('#')[1].Trim().Replace("，", ",").Replace("：", ":").Replace("\"", "");
            }
          }
          tb.Rows.Add(dr);
          tb.AcceptChanges();
        }
        ds.Tables.Add(tb);
        return ds;
      }
      catch (System.Exception e)
      {
        Debug.LogException(e);
      }
      return null;
    }

    #endregion
  }
}
