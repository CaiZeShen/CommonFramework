-- 消息的存储和处理
LManagerBase = { 

eventTree = {},

};

local  this = LManagerBase;

function LManagerBase:New(msgID)
    local o = {};

    self.__index = self;
    
    setmetatable(o,self);

    self.msgID=msgID;

    return o;
end

-- 单例
function LManagerBase.GetInstance()
	return this;
end


function LManagerBase:ProcessEvent(msg)
   if this:FindKey(self.eventTree , msg.msgID) then
        local tmpNode =self.eventTree[msg.msgID];

        while tmpNode ~= nil do
            tmpNode.value:ProcessEvent(msg);

            if  tmpNode.next ~= nil then
                tmpNode = tmpNode.next;
            end
        end

   else
        print("Msg not contains msg = " .. msg.msgID);
   end

end

-- 注册单个msg
function LManagerBase:RegistMsg(id,eventNode)
    if this:FindKey(self.eventTree,id) then
        --  新建一个键值对
        self.eventTree[id] = eventNode;
    else
        -- 挂到链表结构最后
        tmpNode = self.eventTree[id];

        while tmpNode.next ~= nil do

            tmpNode=tmpNode.next;
        end

        tmpNode.next = eventNode;

    end

end

-- 注册多个msg
function LManagerBase:RegistMsgs(script,msgs)
    for k,v in pairs(msgs) do
        
        eventNode = LEventNode:New(script);

        self:RegistMsg(v,eventNode);
    end

end

-- 取消注册单个msg
function LManagerBase:UnRegistMsg(script,id)
    if this:FindKey(self.eventTree,id) then

        tmpNode = self.eventTree[id];
        if  tmpNode.value == scipt then
            -- 处理头部
            if tmpNode.next ==nil then
                self.eventTree[id] = nil;
            else
                self.eventTree[id] = tmpNode.next;
                tmpNode.next = nil;
            end

        else
            -- 遍历到末尾，或找到
            while tmpNode.next ~= nil and tmpNode.next.value ~= script do
                tmpNode=tmpNode.next;
            end

            -- 找到则处理，没找到则不管
            if tmpNode.next ~= nil then
                curNode = tmpNode.next;
                -- 判断是否是末尾
                if  curNode.next == nil then
                    tmpNode.next = nil;
                else
                    tmpNode.next = curNode.next;
                    curNode.next = nil;
                end
            end

        end
    end

end

-- 取消注册多个msg
function LManagerBase:UnRegistMsgs(script,...)
    if  arg == nil then
        return;
    end

    for k,v in pairs(arg) do
  		self:UnRegistMsg(script,v);
  	end
end

function LManagerBase:Destroy()
    keys = {};

    keyIndex = 1;

    for k,value in pairs(self.eventTree) do
        keys[keyIndex] = k;
        keyIndex = keyIndex +1;
    end

    for i=1,#keys do
        self.eventTree[keys[i]] = nil;
    end

end

-- 是否包含key
function LManagerBase:FindKey(dict,key)
    for k,v in pairs(dict) do
        if k == key then
            return true;
        end
    end

    return false;
end