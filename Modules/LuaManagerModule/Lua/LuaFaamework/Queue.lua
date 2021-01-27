Queue = Class.define("Queue")

function Queue:_ctor()
    self.tab = {}
end

function Queue:Enqueue(value)
    table.insert(self.tab, value)
end

function Queue:Dequeue()
    if #self.tab > 0 then
        local value = self.tab[1]
        table.remove(self.tab,1)
        return value
    else
        return nil
    end
end

function Queue:Peek()
    if #self.tab > 0 then
        return self.tab[1]
    else
        return nil
    end
end

function Queue:getter_Count()
    return #self.tab
end

function Queue:Clear()
    table.ClearArray(self.tab)
end