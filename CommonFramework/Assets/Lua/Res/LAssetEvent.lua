--region *.lua
--Date
--此文件由[BabeLua]插件自动生成



--endregion

local lAssetBegin = LManagerID.LAssetMananger;

LAssetEvent = {
    CheckLoadAssetsFinish = lAssetBegin + 1,

    ReleaseSingleObj = lAssetBegin + 2,
    ReleaseBundleObjs = lAssetBegin + 3,
    ReleaseBundle = lAssetBegin + 4,
    ReleaseBundleAndObjs = lAssetBegin + 5,
    ReleaseSceneBundle = lAssetBegin + 6,
    ReleaseAll = lAssetBegin + 7,

    HunkRes = lAssetBegin + 8,

    MaxValue  = lAssetBegin + 9,
}