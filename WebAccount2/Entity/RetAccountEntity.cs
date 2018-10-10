//===================================================

using Mmcoy.Framework;
using System;
using System.Collections.Generic;

/// <summary>
/// 账户实体
/// </summary>
public class RetAccountEntity 
{
    public RetAccountEntity()
    {

    }

    public RetAccountEntity(AccountEntity entity)
    {
        Id = entity.Id.Value;
        UserName = entity.UserName;
        YuanBao = entity.Money;
        LastServerId = entity.LastLogOnServerId;
        LastServerName = entity.LastLogOnServerName;

        if (LastServerId == 0)
        {
            MFReturnValue<List<GameServerEntity>> ret = GameServerCacheModel.Instance.GetPageList(isDesc: true, pageSize: 1);
            if (!ret.HasError)
            {
                List<GameServerEntity> lst = ret.Value;
                if (lst != null && lst.Count > 0)
                {
                    LastServerId = lst[0].Id.Value;
                    LastServerName = lst[0].Name;
                    LastServerIP = lst[0].Ip;
                    LastServerPort = lst[0].Port;
                    LastServerRunStatus = lst[0].RunStatus;
                }
            }
        }
        else
        {
            GameServerEntity entityGameServer = GameServerCacheModel.Instance.GetEntity(LastServerId);
            if (entityGameServer != null)
            {
                LastServerIP = entityGameServer.Ip;
                LastServerPort = entityGameServer.Port;
                LastServerRunStatus = entityGameServer.RunStatus;
            }
        }
    }

    public int Id { get; set; }

    public string UserName { get; set; }

    public string Pwd { get; set; }

    public int YuanBao { get; set; }

    public int LastServerId { get; set; }

    public string LastServerName { get; set; }

    public string LastServerIP { get; set; }

    public int LastServerPort { get; set; }

    public byte LastServerRunStatus { get; set; }

    public DateTime CreateTime { get; set; }

    public DateTime UpdateTime { get; set; }
}