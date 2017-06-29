-- 主要负责消息模块转发

LMsgCenter = { msgID = 0};

local  this = LMsgCenter;

function LMsgCenter:New(msgID)
    local o = {};

    self.__index = self;
    
    setmetatable(o,self);

    self.msgID=msgID;

    return o;
end

-- 单例
function LMsgCenter.GetInstance()
	return this;
end

function LMsgCenter.SendToMsg(msg)
	this.AnalysisMsg(msg);
end

function LMsgCenter.RecvMsg(msg)
	
end

-- 分析消息
function LMsgCenter.AnalysisMsg(msg)
	managerID = msg:GetManager();

	if managerID == LManagerID.LuaManager then

	elseif managerID == LManagerID.LNetManager then

	elseif managerID == LManagerID.LUIManager then

	elseif managerID == LManagerID.LNPCManager then

	elseif managerID == LManagerID.LCharactorManager then

	elseif managerID == LManagerID.LAssetMananger then

	elseif managerID == LManagerID.LDataManager then

	elseif managerID == LManagerID.LAudioManager then

	end
	
end