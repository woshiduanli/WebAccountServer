using Mmcoy.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public partial class GameServerCacheModel
{

    public List<RetGameServerPageEntity> GetGameServerPageList(string condition)
    {
        List<RetGameServerPageEntity> lst = new List<RetGameServerPageEntity>();

        List<GameServerEntity> gameServerLst = GetList(condition: condition, isDesc: false);

        RetGameServerPageEntity entity = null;
        int pageIndex = 1;
        for (int i = 0; i < gameServerLst.Count; i++)
        {
            //每10个服务器一组
            if (i % 10 == 0)
            {
                //10个一组的第一个
                entity = new RetGameServerPageEntity();
                entity.PageIndex = pageIndex;
                pageIndex++;

                entity.Name = gameServerLst[i].Id.ToString();
                lst.Add(entity);
            }

            if ((i + 1) % 10 == 0 || i == gameServerLst.Count - 1)
            {
                //10个一组的最后一个
                if (entity != null)
                {
                    entity.Name += " - " + gameServerLst[i].Id.ToString() + "服"; ;
                }
            }
        }

        return lst.OrderByDescending(p => p.PageIndex).ToList();
    }

    public List<RetGameServerEntity> GetGameServerList(int pageIndex, string condition)
    {
        List<RetGameServerEntity> retList = new List<RetGameServerEntity>();

        if (pageIndex == 0)
        {
            //推荐服务器
            //新区 玩家有帐号的区

            //临时的方案 返回最新前三个
            MFReturnValue<List<GameServerEntity>> retValue = this.GetPageList(condition: condition, pageSize: 3, pageIndex: 1);

            if (!retValue.HasError)
            {
                List<GameServerEntity> lst = retValue.Value;

                for (int i = 0; i < lst.Count; i++)
                {
                    retList.Add(new RetGameServerEntity()
                    {
                        Id = lst[i].Id.Value,
                        RunStatus = lst[i].RunStatus,
                        IsCommand = lst[i].IsCommand,
                        IsNew = lst[i].IsNew,
                        Name = lst[i].Name,
                        Ip = lst[i].Ip,
                        Port = lst[i].Port
                    });
                }
            }

        }
        else
        {
            MFReturnValue<List<GameServerEntity>> retValue = this.GetPageList(condition: condition, pageSize: 10, pageIndex: pageIndex, isDesc: false);

            if (!retValue.HasError)
            {
                List<GameServerEntity> lst = retValue.Value;

                for (int i = 0; i < lst.Count; i++)
                {
                    retList.Add(new RetGameServerEntity()
                    {
                        Id = lst[i].Id.Value,
                        RunStatus = lst[i].RunStatus,
                        IsCommand = lst[i].IsCommand,
                        IsNew = lst[i].IsNew,
                        Name = lst[i].Name,
                        Ip = lst[i].Ip,
                        Port = lst[i].Port
                    });
                }
            }
        }
        return retList.OrderByDescending(p => p.Id).ToList();
    }
}