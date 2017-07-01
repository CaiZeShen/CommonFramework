--region *.lua
--Date
--此文件由[BabeLua]插件自动生成



--endregion
LAssetMsg = LMsgBase:New();

LAssetMsg.__index = LAssetMsg;

function LAssetMsg:New(msgID,scene,bundle,res,single,backFuc)

    local self = {};
    
    setmetatable(self,LAssetMsg);

    self.msgID = msgID;
    self.sceneName = scene;
    self.bundleName = bundle;
    self.resName = res;
    self.isSingle = single;
    self.callBackFunc = backFuc;

    return self;
end