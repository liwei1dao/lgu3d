using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace lgu3d
{
  public enum AssetCheckMode               //资源校验方式
  {
    AppStartCheck,                       //APP启动校验
    ModelStartCheck,                     //模块启动校验
    UserCheck,                           //使用时校验
  }

  [Serializable]
  [JsonObject(MemberSerialization.OptOut)]
  public class ResBuileInfo
  {
    public string Id;                  //模块名
    public string Model;               //所属模块
    public float Size;                 //文件大小 单位kb
    public string Md5;                 //文件md5值
    public string[] Dependencies;      //资源的依赖关系
    [JsonIgnore]
    public List<string> Assets;        //Buile 序列化忽略的值
  }
  [JsonObject(MemberSerialization.OptIn)]
  public class AppModuleVersionInfo : ScriptableObject
  {
    public float ProVersion;               //程序版本 大版本
    // public float ResVersion;               //资源版本 小版本
  }

  [JsonObject(MemberSerialization.OptIn)]
  public class AppModuleAssetInfo : ScriptableObject
  {
    [JsonProperty("AppResInfo")]
    public Dictionary<string, ResBuileInfo> AppResInfo;

    public AppModuleAssetInfo()
    {
      AppResInfo = new Dictionary<string, ResBuileInfo>();
    }

    public void ClearNoKeepAB()
    {
      List<string> noab = new List<string>();
      foreach (var item in AppResInfo)
      {
        if (item.Value.Assets.Count == 0)
        {
          noab.Add(item.Key);
        }
      }
      foreach (var item in noab)
      {
        AppResInfo.Remove(item);
      }
    }
  }
}
