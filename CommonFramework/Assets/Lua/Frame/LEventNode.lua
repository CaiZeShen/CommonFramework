--region *.lua
--Date
--此文件由[BabeLua]插件自动生成


-- 消息节点
--endregion

LEventNode = { next = nil};

LEventNode.__index = LEventNode;

function LEventNode:New(event)
    local self = {};

    setmetatable(self,LEventNode);

    self.value=event;
    self.next=nil;

    return self;
end

   

    