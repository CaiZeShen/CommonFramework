--region *.lua
--Date
--此文件由[BabeLua]插件自动生成



--endregion
LAssetBundleLoader = LAssetBase:New();

LAssetBundleLoader.__index = LAssetBundleLoader;

local this = LAssetBundleLoader;

function LAssetBundleLoader:New()

    local self = {};
    
    setmetatable(self,LAssetBundleLoader);

    return self;
end

function LAssetBundleLoader:Awake()
    self.msgIDs[1]=LAssetEvent.CheckLoadAssetsFinish;

    self.msgIDs[2]=LAssetEvent.ReleaseSingleObj;
    self.msgIDs[3]=LAssetEvent.ReleaseBundleObjs;
    self.msgIDs[4]=LAssetEvent.ReleaseBundle;
    self.msgIDs[5]=LAssetEvent.ReleaseBundleAndObjs;
    self.msgIDs[6]=LAssetEvent.ReleaseSceneBundle;
    self.msgIDs[7]=LAssetEvent.ReleaseAll;

    self.msgIDs[8]=LAssetEvent.HunkRes;

    this:RegistSelf(this, self.msgIDs);
end

function LAssetBundleLoader:SendMsg()

end

-- lua 层传递的 res 请求
function LAssetBundleLoader:ProcessEvent(msg)
    -- 加载资源
    if(msg.msgID == LAssetEvent.HunkRes) then 
        MLuaResLoader.Instance:GetResoures(msg.sceneName,msg.bundleName,msg.resName,msg.isSingle,msg.callBackFunc);

     -- 释放指定包中的指定资源
    elseif (msg.msgID == LAssetEvent.ReleaseSingleObj) then
        MLuaResLoader.Instance:UnLoadResObj(msg.sceneName,msg.bundleName,msg.resName);

    -- 释放指定包中全部资源
    elseif (msg.msgID == LAssetEvent.ReleaseBundleObjs) then
        MLuaResLoader.Instance:UnLoadBundleObjs(msg.sceneName,msg.bundleName);

    -- 释放指定包
    elseif (msg.msgID == LAssetEvent.ReleaseBundle) then
        MLuaResLoader.Instance:UnLoadSingleBundle(msg.sceneName,msg.bundleName);

    -- 释放指定包和包中资源
    elseif (msg.msgID == LAssetEvent.ReleaseBundleAndObjs) then
        MLuaResLoader.Instance:UnLoadBundleAndObjs(msg.sceneName,msg.bundleName);

    -- 释放所有包
    elseif (msg.msgID == LAssetEvent.ReleaseSceneBundle) then
        MLuaResLoader.Instance:UnLoadAllBundle(msg.sceneName);

    -- 释放所有包和资源
    elseif (msg.msgID == LAssetEvent.ReleaseAll) then
        MLuaResLoader.Instance:UnLoadAllBundleAndObjs(msg.sceneName);

    end
end