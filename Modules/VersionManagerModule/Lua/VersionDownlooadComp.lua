local VersionDownlooadComp = Class.define("VersionDownlooadComp",BaseModelComp)

function VersionDownlooadComp:Load(MyModel,...)
    self:super(VersionDownlooadComp,"Load",MyModel,...);
    self.TasksQueues = Class.new(Queue)
    self.IsDownloading = false
    self.CurrDownloadTask = nil
    self:LoadEnd();
end

function VersionDownlooadComp:StartDownload(tasks,downloadBack,compTaskBack)
    print("添加下载任务 "..#tasks)
    local Task = Class.new(DownloadTaskGroup,tasks,downloadBack,compTaskBack)
    self:AddTask(Task);
end

function VersionDownlooadComp:AddTask(_DownloadTask)
    self.TasksQueues:Enqueue(_DownloadTask);
    if not self.IsDownloading then
        self:StartNextTask()
    end
end

function VersionDownlooadComp:StartNextTask()
    if self.CurrDownloadTask then
        if self.CurrDownloadTask.IsComp then
            if self.TasksQueues.Count > 0 then
                self.IsDownloading = true;
                self.CurrDownloadTask = self.TasksQueues:Dequeue()
                self:StartTask(self.CurrDownloadTask:NextTask())
            else
                self.IsDownloading = false
            end
        else
            self:StartTask(self.CurrDownloadTask:NextTask())
        end
    else
        if self.TasksQueues.Count > 0 then
            self.IsDownloading = true
            self.CurrDownloadTask = self.TasksQueues:Dequeue()
            local task = self.CurrDownloadTask:NextTask()
            self:StartTask(task)
        else
            self.IsDownloading = false
        end
    end
end

function VersionDownlooadComp:StartTask(task)
    FilesTools.CreateDirectoryByFile(task.FileName)
    StartCoroutine(self.Download,self,task)
end

function VersionDownlooadComp:Download(task)
    Debug.Log("开始执行下载任务 Url:"..task.Url)
    local www = UnityEngine.Networking.UnityWebRequest.New(task.Url)
    local dowle = UnityEngine.Networking.DownloadHandlerFile.New(task.FileName)
    www.downloadHandler = dowle
    www:SendWebRequest()
    while (not www.isDone)
    do
        local downloadedBytes = www.downloadProgress * task.Szie
        self.CurrDownloadTask:UpdateDownloadProgress(downloadedBytes)
        Yield(1)
    end
    Debug.Log("下载任务结束 Url:"..task.Url.." IsDone"..tostring(www.isDone))
    if www.isDone then
        self.CurrDownloadTask:UpdateDownloadProgress(task.Szie)
        self.CurrDownloadTask:TaskCompleted()
        self:StartNextTask();
    else
        self.CurrDownloadTask:OnError(www.error)
    end
end


return VersionDownlooadComp