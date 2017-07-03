--region *.lua
--Date
--此文件由[BabeLua]插件自动生成



--endregion
LNetManager = LNetManager:New();

LNetManager.__index = LNetManager;

local this = LNetManager;

function LNetManager:New()

    local self = {};
    
    setmetatable(self,LNetManager);

    return self;
end

function LNetManager:GetInstance()
    return this;
end

function LNetManager:SendMsg(msg)
    if msg:GetManager() == LManagerID.LNetManager then
        self:ProcessEvent(msg);
    else
        LMsgCenter.SendToMsg(msg);
    end
end