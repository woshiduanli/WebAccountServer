using LitJson;
using Mmcoy.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebAccount.Controllers.api
{
    public class GameServerController : ApiController
    {
        // GET: api/GameServer
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/GameServer/5
        public List<RetGameServerEntity> Get(int id)
        {
            return null;
            //return GameServerCacheModel.Instance.GetGameServerList(id);
            //return GameServerCacheModel.Instance.GetGameServerPageList();
        }

        // POST: api/GameServer
        public object Post([FromBody]string jsonStr)
        {
            RetValue ret = new RetValue();

            JsonData jsonData = JsonMapper.ToObject(jsonStr);

            //时间戳
            long t = Convert.ToInt64(jsonData["t"].ToString());
            string deviceIdentifier = jsonData["deviceIdentifier"].ToString();
            string deviceModel = jsonData["deviceModel"].ToString();
            string sign = jsonData["sign"].ToString();

            ////1.判断时间戳 如果大于3秒 直接返回错误
            //if (MFDSAUtil.GetTimestamp() - t > 3)
            //{
            //    ret.HasError = true;
            //    ret.ErrorMsg = "请求无效";
            //    return ret;
            //}

            ////2.验证签名
            //string signServer = MFEncryptUtil.Md5(string.Format("{0}:{1}", t, deviceIdentifier));
            //if (!signServer.Equals(sign, StringComparison.CurrentCultureIgnoreCase))
            //{
            //    ret.HasError = true;
            //    ret.ErrorMsg = "请求无效";
            //    return ret;
            //}

            int type = Convert.ToInt32(jsonData["Type"].ToString());

            if (type == 0)
            {
                string channelId = jsonData["ChannelId"].ToString();
                string innerVersion = jsonData["InnerVersion"].ToString();

                //先获取渠道状态 根据渠道状态 来加载不同的区服
                ChannelEntity entity = ChannelCacheModel.Instance.GetEntity(string.Format("[ChannelId]={0} and [InnerVersion]={1}", channelId, innerVersion));
                if (entity == null)
                {
                    ret.HasError = true;
                    ret.ErrorMsg = "渠道号不存在";
                }

                //获取页签
                return GameServerCacheModel.Instance.GetGameServerPageList(string.Format("[ChannelStatus]={0}", entity.ChannelStatus));

            }
            else if (type == 1)
            {
                //string channelId = jsonData["ChannelId"].ToString();
                string channelId = "0"; 
                //string innerVersion = jsonData["InnerVersion"].ToString();
                string innerVersion = 1001.ToString ();


                //先获取渠道状态 根据渠道状态 来加载不同的区服
                ChannelEntity entity = ChannelCacheModel.Instance.GetEntity(string.Format("[ChannelId]={0} and [InnerVersion]={1}", channelId, innerVersion));
                if (entity == null)
                {
                    ret.HasError = true;
                    ret.ErrorMsg = "渠道号不存在";
                }
                // 这里写死
                //entity.ChannelStatus = 0;
                int pageIndex = int.Parse(jsonData["pageIndex"].ToString());
                //获取区服列表
                return GameServerCacheModel.Instance.GetGameServerList(pageIndex, string.Format("[ChannelStatus]={0}", entity.ChannelStatus));
            }
            else if (type == 2)
            {
                //更新最后登录信息
                int userId = int.Parse(jsonData["userId"].ToString());
                int lastServerId = int.Parse(jsonData["lastServerId"].ToString());
                string lastServerName = jsonData["lastServerName"].ToString();

                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic["Id"] = userId;
                dic["LastLogOnServerId"] = lastServerId;
                dic["LastLogOnServerName"] = lastServerName;
                dic["LastLogOnServerTime"] = DateTime.Now;

                AccountCacheModel.Instance.Update("LastLogOnServerId=@LastLogOnServerId, LastLogOnServerName=@LastLogOnServerName, LastLogOnServerTime=@LastLogOnServerTime", "Id=@Id", dic);
            }

            return ret;
        }

        // PUT: api/GameServer/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/GameServer/5
        public void Delete(int id)
        {
        }
    }
}
