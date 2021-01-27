DownloadTask = Class.define("DownloadTask");

function DownloadTask:ctor(_Id,_Url,_SavePath,_Szie)
    self.Id = _Id
    self.Url = _Url
    self.FileName = _SavePath
    self.Szie = _Szie
    self.Progress = 0
    self.IsComp = false
end

function DownloadTask:getter_Progress()
    return self.Progress
end

function DownloadTask:UpdateDownloadProgress(DownloadBytes)
    self.Progress = DownloadBytes/ self.Szie;
end


DownloadTaskGroup = Class.define("DownloadTaskGroup");
function DownloadTaskGroup:ctor(TaskQueues,DownloadBack,CompTaskBack,OnErrorBack)
    print("创建下载组任务 "..#TaskQueues)
    self.IsComp = false;
    self.Progress = 0
    self.CompSize = 0
    self.AllTask = TaskQueues;
    self.NoCompTasks = Class.new(Queue);
    self.CompTasks = Class.new(Queue)
    self.Size = 0
    for i,v in ipairs(TaskQueues) do
        self.NoCompTasks:Enqueue(v)
        self.Size = self.Size + v.Szie
    end
    self.DownloadBack = DownloadBack;
    self.CompTaskBack = CompTaskBack;
    self.OnErrorBack = OnErrorBack;
end

-- 当前任务
function DownloadTaskGroup:getter_CurrTask() 
    return self.NoCompTasks:Peek()
end

function DownloadTaskGroup:NextTask()
    if self.NoCompTasks.Count > 0 then
        return self.NoCompTasks:Peek()
    end
    return nil;
end

function DownloadTaskGroup:UpdateDownloadProgress(DownloadBytes)
    if not self.IsComp then
        self.NoCompTasks:Peek():UpdateDownloadProgress(DownloadBytes);
        self.Progress = (self.CompSize + DownloadBytes) / self.Size;
        if self.DownloadBack then
           self.DownloadBack(self);
        end
    else
        self.Progres = 1
        if self.DownloadBack then
            self.DownloadBack(self);
        end
    end
end

--任务完成
function DownloadTaskGroup:TaskCompleted()
    local Task = self.CurrTask
    Task.IsComp = true
    self.CompSize = self.CompSize + Task.Szie
    self.CompTasks:Enqueue(self.NoCompTasks:Dequeue())
    if self.NoCompTasks.Count == 0 then
        self.IsComp = true
    end
    if self.CompTaskBack then
        self.CompTaskBack(self, Task)
    end
end

--任务错误
function DownloadTaskGroup:OnError(error)
    if self.OnErrorBack then
        self.OnErrorBack(self,error)
    end
end