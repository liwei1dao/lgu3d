using UnityEngine;
using UnityEditor;

namespace lgu3d.Editor
{
  [System.Serializable]
  public class ExcelFileImportInfo : DirFileInfoBase
  {
    public bool IsExport = false;
    public DataFileType DataType;
    public ExcelFileImportInfo(IDirFileInfo parent, string path, string[] filter)
        : base(parent, path, filter)
    {
      IsExport = parent == null ? true : (parent as ExcelFileImportInfo).IsExport;
      if (FileType == FileType.DataFile)
      {
        if (Path.EndsWith(".xml"))
        {
          DataType = DataFileType.Xml;
        }
        else if (Path.EndsWith(".json"))
        {
          DataType = DataFileType.Josn;
        }
        else if (Path.EndsWith(".csv"))
        {
          DataType = DataFileType.CSV;
        }
        else if (Path.EndsWith(".xlsx"))
        {
          DataType = DataFileType.Excel;
        }
        else if (Path.EndsWith(".asset"))
        {
          DataType = DataFileType.Asset;
        }
      }
    }

    public override IDirFileInfo CreataChail(IDirFileInfo parent, string path, string[] filter)
    {
      ExcelFileImportInfo dfinfo = new ExcelFileImportInfo(parent, path, filter);
      return dfinfo;
    }

    public override void Refresh(IDirFileInfo parent)
    {
      ExcelFileImportInfo P = parent as ExcelFileImportInfo;
      IsExport = P.IsExport;
      RefreshChild();
    }

    public override void RefreshChild()
    {
      foreach (var item in Childs)
      {
        item.Refresh(this);
      }
    }

    public override void OnGUI()
    {
#if UNITY_EDITOR
      EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
      EditorGUI.indentLevel = Layer;
      if (FileType == FileType.Folder)
      {
        Foldout = EditorGUILayout.Foldout(Foldout, Name);
        EditorGUILayout.Space();
        bool export = GUILayout.Toggle(IsExport, "Export");
        if (export != IsExport)
        {
          IsExport = export;
          RefreshChild();
        }
      }
      else
      {
        EditorGUILayout.LabelField(Name);
        EditorGUILayout.Space();
        IsExport = GUILayout.Toggle(IsExport, "Export");
      }
      EditorGUILayout.EndHorizontal();
      if (Foldout)
      {
        foreach (var item in Childs)
        {
          item.OnGUI();
        }
      }
#endif
    }
  }
}
