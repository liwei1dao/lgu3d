using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace lgu3d.Editor
{

  public enum FileType
  {
    Useless,                //无用文件
    Folder,                 //文件夹节点
    Audio,                  //音频文件
    Image,                  //图片文件
    UnityFile,              //unity3d资源文件
    DataFile,               //数据配置文件
    ScriptFile,             //脚本文件
  }
  public interface IDirFileInfo
  {
    string GetName();
    string GetPath();
    FileType GetFileType();
    int GetLayer();
    List<IDirFileInfo> GetChilds();
    IDirFileInfo CreataChail(IDirFileInfo parent, string path, string[] filter);
    void Refresh(IDirFileInfo parent);
    void RefreshChild();
    void OnGUI();
  }

  [System.Serializable]
  public class DirFileInfoBase : IDirFileInfo
  {
    static readonly string[] AudioFileSuffix = { ".mp3", ".alff", ".wav", ".ogg" };
    static readonly string[] ImageFileSuffix = { ".psd", ".tiff", ".jpg", ".tga", ".png", ".gif" };
    static readonly string[] UnityFileSuffix = { ".prefab", ".unity", ".fbx", ".mat", ".anim", ".controller", ".shader" };
    static readonly string[] DataFileSuffix = { ".xml", ".json", ".csv", ".xlsx", ".asset" };
    static readonly string[] ScriptFileSuffix = { ".cs", ".lua", ".txt" };

    public string Name;
    public string Path;
    public FileType FileType;
    public int Layer;
    public bool Foldout = false;
    public List<IDirFileInfo> Childs;

    public string GetName()
    {
      return Name;
    }
    public string GetPath()
    {
      return Path;
    }
    public FileType GetFileType()
    {
      return FileType;
    }
    public int GetLayer()
    {
      return Layer;
    }
    public List<IDirFileInfo> GetChilds()
    {
      return Childs;
    }
    public DirFileInfoBase(IDirFileInfo parent, string path, string[] filter)
    {
      Path = path.Replace("\\", "/");
      Name = PathTools.GetPathFolderName(Path);
      Layer = parent == null ? 0 : parent.GetLayer() + 1;
      Childs = new List<IDirFileInfo>();
      if (PathTools.IsDirectory(Path))
      {
        string[] fileList = Directory.GetFileSystemEntries(Path);
        foreach (string file in fileList)
        {
          IDirFileInfo Item = CreataChail(this, file, filter);
          if (Item.GetFileType() != FileType.Useless)
          {
            Childs.Add(Item);
          }
        }
        if (Childs.Count > 0)
        {
          FileType = FileType.Folder;
        }
        else
        {
          FileType = FileType.Useless;
        }
      }
      else
      {
        if (PathTools.CheckSuffix(Path, filter))
        {
          Name = Name.Substring(0, Name.IndexOf("."));
          FileType = getFileType(Path);
        }
        else
        {
          FileType = FileType.Useless;
        }
      }
    }

    public virtual IDirFileInfo CreataChail(IDirFileInfo parent, string path, string[] filter)
    {
      DirFileInfoBase dfinfo = new DirFileInfoBase(parent, path, filter);
      return dfinfo;
    }

    public FileType getFileType(string path)
    {
      FileType type = FileType.Useless;
      if (checkSuffix(path, AudioFileSuffix))
      {
        return FileType.Audio;
      }
      else if (checkSuffix(path, ImageFileSuffix))
      {
        return FileType.Image;
      }
      else if (checkSuffix(path, UnityFileSuffix))
      {
        return FileType.UnityFile;
      }
      else if (checkSuffix(path, DataFileSuffix))
      {
        return FileType.DataFile;
      }
      else if (checkSuffix(path, ScriptFileSuffix))
      {
        return FileType.ScriptFile;
      }
      return type;
    }
    public bool checkSuffix(string _FlieName, string[] Suffix)
    {
      for (int i = 0; i < Suffix.Length; i++)
      {
        if (_FlieName.EndsWith(Suffix[i]))
          return true;
      }
      return false;
    }

    public virtual void Refresh(IDirFileInfo parent) { }
    public virtual void RefreshChild() { }

    public virtual void OnGUI()
    {
#if UNITY_EDITOR
            EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
            EditorGUI.indentLevel = Layer;
            if (FileType == FileType.Folder)
            {
                Foldout = EditorGUILayout.Foldout(Foldout, Name);
                EditorGUILayout.Space();
            }
            else
            {
                EditorGUILayout.LabelField(Name);
                EditorGUILayout.Space();
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
