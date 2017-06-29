-- 负责消息路由

LMsgBase = { msgID = 0}

function LMsgBase:New(msgID)
    local o = {};

    self.__index = self;
    
    setmetatable(o,self);

    self.msgID=msgID;

    return o;
end

function LMsgBase:GetManager()
    tmpID = math.floor(self.msgID / MsgSpan) * MsgSpan;

    return math.ceil(tmpID);
end