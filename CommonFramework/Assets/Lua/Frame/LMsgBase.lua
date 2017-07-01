-- 负责消息路由

LMsgBase = { msgID = 0}

LMsgBase.__index = LMsgBase;

function LMsgBase:New(msgID)

    local self = {};
    
    setmetatable(self,LMsgBase);

    self.msgID=msgID;

    return self;
end

function LMsgBase:GetManager()
    tmpID = math.floor(self.msgID / MsgSpan) * MsgSpan;

    return math.ceil(tmpID);
end