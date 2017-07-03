-- 主要负责消息模块转发
LMsgCenter = { msgID = 0};

LMsgCenter.__index = LMsgCenter;

local this = LMsgCenter;

function LMsgCenter:New(msgID)

    local self = {};
    
    setmetatable(self,LMsgCenter);

    self.msgID=msgID;

    return self;
end

-- 单例
function LMsgCenter.GetInstance()
	return this;
end

function LMsgCenter.Awake()
    -- 与C#关联
	LuaAndCMsgCenter.Instance:SettingLuaCallBack(this.OnRecvMsg);
end

this.Awake();

function LMsgCenter.SendToMsg(msg)
	this.AnalysisMsg(msg);
end

function LMsgCenter.OnRecvMsg(fromNet,arg0,arg1,arg2)
	if fromNet == true then
        local tmpMsg = LMsgBase:New(arg0);
        tmpMsg.state = arg1;
        tmpMsg.data = arg2;

        this.AnalysisMsg(tmpMsg);
    else
        this.AnalysisMsg(arg0);
    end
end

-- 分析消息
function LMsgCenter.AnalysisMsg(msg)
	managerID = msg:GetManager();

	if managerID == LManagerID.LuaManager then

	elseif managerID == LManagerID.LNetManager then

	elseif managerID == LManagerID.LUIManager then
            LUIManager.GetInstance():ProcessEvent(msg);
	elseif managerID == LManagerID.LNPCManager then

	elseif managerID == LManagerID.LCharactorManager then

	elseif managerID == LManagerID.LAssetMananger then

	elseif managerID == LManagerID.LDataManager then

	elseif managerID == LManagerID.LAudioManager then

    else
        -- 让c#的消息中心处理
        MsgCenter.Instance:ProcessEvent(msg);
	end
	
end