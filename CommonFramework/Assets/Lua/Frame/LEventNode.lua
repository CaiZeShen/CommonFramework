--region *.lua
--Date
--此文件由[BabeLua]插件自动生成


-- 消息节点
--endregion

LEventNode = { next = nil};

function LEventNode:New(event)
    local o = {};

    self.__index = self;
    
    setmetatable(o,self);

    self.value=event;
    self.next=nil;

    return o;
end
