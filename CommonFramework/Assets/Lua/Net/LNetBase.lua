--region *.lua
--Date
--此文件由[BabeLua]插件自动生成



--endregion
LNetBase = {
    msgIDs = {},
}

LNetBase.__index = LNetBase;

function LNetBase:New()

    local self = {};
    
    setmetatable(self,LNetBase);

    return self;
end

function LNetBase:RegistSelf(script ,msgs)
    LUIManager.GetInstance():RegistMsgs(script,msgs);
end

function LNetBase:UnRegistSelf(script ,msgs)
    LUIManager.GetInstance():UnRegistMsgs(script,msgs);
end

function LNetBase:Destroy()
    self:UnRegistSelf(self,self.msgIDs);
end