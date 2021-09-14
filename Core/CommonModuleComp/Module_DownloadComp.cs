using System;
using System.IO;
using System.Net;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace lgu3d
{

  public delegate void DownloadProgress(DownloadTaskGroup TasksGroup, float Progress);
  public delegate void TaskCompleted(DownloadTaskGroup TasksGroup, DownloadTask Task);
  public delegate void TaskError(DownloadTaskGroup TasksGroup, DownloadTask Task);
  /// <summary>
  /// 单个下载任务
  /// </summary>
  public class DownloadTask
  {
    public string Id;                                                               //资源Id
    public string Url;                                                              //下载链接
    public string FileName;                                                         //文件名称
    public float Szie { get; set; }                                                 //文件大小 单位kb
    public float Progress { get; private set; }                                     //下载进度
    public bool IsComp { get; set; }                                                //是否下载完成

    public DownloadTask()
    {
    }

    public DownloadTask(string _Id, string _Url, string _SavePath, float _Szie)
    {
      Id = _Id;
      IsComp = false;
      Url = _Url;
      FileName = _SavePath;
      Szie = _Szie;
    }

    public void UpdateDownloadProgress(ulong DownloadBytes)
    {
      Progress = (DownloadBytes / 1204.0f) / Szie;
    }
  }
  /// <summary>
  /// 一组下载任务
  /// </summary>
  public class DownloadTaskGroup
  {
    private DownloadTask[] AllTask;                                                 //所有下载任务
    private Queue<DownloadTask> NoCompTasks;                                        //未完成任务
    private Queue<DownloadTask> CompTasks;                                          //完成任务
    private float Size = 0;
    private float CompSize = 0;
    private DownloadProgress DownloadBack;
    private TaskCompleted CompTaskBack;
    private TaskError ErrorTaskBack;
    public bool IsComp { get; private set; }                                        //是否下载完成
    public float Progress { get; private set; }                                     //下载进度
    public DownloadTask CurrTask { get { return NoCompTasks.Peek(); } }             //当前任务

    public DownloadTaskGroup(DownloadTask[] taskQueues, DownloadProgress downloadBack = null, TaskCompleted compTaskBack = null, TaskError errorTaskBack = null)
    {
      IsComp = false;
      AllTask = taskQueues;
      NoCompTasks = new Queue<DownloadTask>(taskQueues);
      CompTasks = new Queue<DownloadTask>();
      for (int i = 0; i < taskQueues.Length; i++)
      {
        Size += taskQueues[i].Szie;
      }
      CompSize = 0;
      DownloadBack = downloadBack;
      CompTaskBack = compTaskBack;
      ErrorTaskBack = errorTaskBack;
    }

    public DownloadTask NextTask()
    {
      if (NoCompTasks.Count > 0)
      {
        return NoCompTasks.Peek();
      }
      return null;
    }

    public virtual void UpdateDownloadProgress(ulong DownloadBytes)
    {
      if (!IsComp)
      {
        CurrTask.UpdateDownloadProgress(DownloadBytes);
        Progress = (CompSize + DownloadBytes / 1204.0f) / Size;
        DownloadBack?.Invoke(this, Progress);
      }
      else
      {
        DownloadBack(this, 1);
      }
    }

    /// <summary>
    /// 任务完成
    /// </summary>
    public virtual void TaskCompleted()
    {
      DownloadTask Task = CurrTask;
      Task.IsComp = true;
      CompSize += Task.Szie;
      CompTasks.Enqueue(NoCompTasks.Dequeue());
      if (NoCompTasks.Count == 0)
      {
        IsComp = true;
      }
      CompTaskBack?.Invoke(this, Task);
    }

    /// <summary>
    /// 任务完成
    /// </summary>
    public virtual void TaskError()
    {
      DownloadTask Task = CurrTask;
      Task.IsComp = false;
      ErrorTaskBack?.Invoke(this, Task);
    }
  }
  public class Module_DownloadComp<C> : ModelCompBase<C> where C : ModelBase, new()
  {
    private Queue<DownloadTaskGroup> TasksQueues;
    private DownloadTaskGroup CurrDownloadTask;
    private bool IsDownloading = false;
    #region 框架构造
    public override void Load(ModelBase _ModelContorl, params object[] _Agr)
    {
      IsDownloading = false;
      TasksQueues = new Queue<DownloadTaskGroup>();
      base.Load(_ModelContorl);
      base.LoadEnd();
    }
    #endregion

    /// <summary>
    /// 添加下载任务
    /// </summary>
    /// <param name="_DownloadTask"></param>
    public void AddTask(DownloadTaskGroup _DownloadTask)
    {
      TasksQueues.Enqueue(_DownloadTask);
      if (!IsDownloading)
        StartNextTask();
    }

    public void StartNextTask()
    {
      if (CurrDownloadTask != null)
      {
        if (CurrDownloadTask.IsComp)
        {
          if (TasksQueues.Count > 0)
          {
            IsDownloading = true;
            CurrDownloadTask = TasksQueues.Dequeue();
            StartTask(CurrDownloadTask.NextTask());
          }
          else
          {
            IsDownloading = false;
          }
        }
        else
        {
          StartTask(CurrDownloadTask.NextTask());
        }
      }
      else
      {
        if (TasksQueues.Count > 0)
        {
          IsDownloading = true;
          CurrDownloadTask = TasksQueues.Dequeue();
          StartTask(CurrDownloadTask.NextTask());
        }
        else
        {
          IsDownloading = false;
        }
      }
    }

    private void StartTask(DownloadTask _Task)
    {
      string filPath = _Task.FileName.Substring(0, _Task.FileName.LastIndexOf("/"));
      if (!Directory.Exists(filPath))
      {
        Directory.CreateDirectory(filPath);
      }
      MyModule.StartCoroutine(DownloadFile(_Task));
    }

    IEnumerator DownloadFile(DownloadTask task)
    {
      UnityWebRequest www = new UnityWebRequest(task.Url);
      DownloadHandlerFile dowle = new DownloadHandlerFile(task.FileName);
      www.downloadHandler = dowle;
      www.SendWebRequest();
      while (!www.isDone)
      {
        CurrDownloadTask.UpdateDownloadProgress(www.downloadedBytes);
        yield return 1;
      }
      if (www.result == UnityWebRequest.Result.Success)
      {
        CurrDownloadTask.UpdateDownloadProgress((ulong)task.Szie);
        CurrDownloadTask.TaskCompleted();
        StartNextTask();
      }
      else
      {
        CurrDownloadTask.TaskError();
      }
    }
  }
}
