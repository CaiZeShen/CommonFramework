--region *.lua
--Date
--此文件由[BabeLua]插件自动生成



--endregion
LAssetBase = {
    msgIDs = {},
}

LAssetBase.__index = LAssetBase;

function LAssetBase:New()

    local self = {};
    
    setmetatable(self,LAssetBase);

    return self;
end

function LAssetBase:RegistSelf(script ,msgs)
    LAssetManager.GetInstance():RegistMsgs(script,msgs);
end

function LAssetBase:UnRegistSelf(script ,msgs)
    LAssetManager.GetInstance():UnRegistMsgs(script,msgs);
end

function LAssetBase:Destroy()
    self:UnRegistSelf(self,self.msgIDs);
end