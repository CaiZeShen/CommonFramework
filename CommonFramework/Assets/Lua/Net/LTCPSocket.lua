--region *.lua
--Date
--此文件由[BabeLua]插件自动生成


-- 把socket数据直接穿插给C#
--endregion

LTCPSocket = LNetBase:New();

LTCPSocket.__index = LTCPSocket;

local this = LTCPSocket;

function LTCPSocket:New()

    local self = {};
    
    setmetatable(self,LTCPSocket);

    return self;
end

function LTCPSocket:Awake()
    this.msgIDs = {
        LTCPEvent.Connect,
        LTCPEvent.SendMsg,
    };

    self.RegistSelf(this,this.msgIDs);
end

function LTCPSocket:SendMsgs(msg)
   tmpMsg = TCPSocketMsg.New();

    tmpMsg:ChangLuaMsg(TCPEvent.SendMsg,msg.data,msg.netID);
    
    this:SendMsg(tmpMsg);
end

function LTCPSocket:ConnectSocket(msg)
   local connect = TCPConnectMsg.New(TCPEvent.Connect,GloableConfig.ipAddress,GloableConfig.port);
   self:SendMsgs(connect);
end

function LTCPSocket:ProcessEvent(msg)
    if (msg.msgID == LTCPEvent.SendMsg) then
        this.SendMsgs(msg);
    elseif (msg.msgID == LTCPEvent.Connect) then
        this.ConnectSocket();
    end
end