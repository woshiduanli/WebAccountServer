using LitJson;
using Mmcoy.Framework;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;



//using WebAccount.Entity;
using System.Data.SqlClient;


namespace WebAccount.Controllers.api
{
    public class AccountController : ApiController
    {
        // GET: api/Account
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Account/5
        string conStr = "Data Source=.;Initial Catalog=DBAccount;User ID=suzhen2;Password=123456";
        // GET: api/Account/5
        // GET: api/Account/5
        public int Get(int id)
        {
            return 9; 
            //Console.w("dfsfddddddddddddddddds");

            //AccountEntity cccountEntity = new AccountEntity();
            //using (SqlConnection conn = new SqlConnection(conStr))
            //{
            //    conn.Open();
            //    SqlCommand cmd = new SqlCommand("Account_GetEntity", conn);
            //    cmd.CommandType = System.Data.CommandType.StoredProcedure;
            //    cmd.Parameters.Add(new SqlParameter("@Id", id));
            //    using (SqlDataReader dr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection))
            //    {
            //        if (dr.HasRows)
            //        {
            //            while (dr.Read())
            //            {
            //                cccountEntity.Id = dr["Id"] is DBNull ? 0 : (int)dr["Id"];
            //                cccountEntity.UserName = dr["UserName"] is DBNull ? "" : (string)dr["UserName"];

            //                return cccountEntity;
            //            }
            //        }
            //    }
            //}
            //return null;
        }

        // POST: api/Account
        public RetValue Post([FromBody]string jsonStr)
        {
            RetValue ret = new RetValue();

            JsonData jsonData = JsonMapper.ToObject(jsonStr);

            //时间戳
            long t = Convert.ToInt64(jsonData["t"].ToString());
            string deviceIdentifier = jsonData["deviceIdentifier"].ToString();
            string deviceModel = jsonData["deviceModel"].ToString();
            string sign = jsonData["sign"].ToString();

            //////   1.判断时间戳 如果大于3秒 直接返回错误
            ////if (MFDSAUtil.GetTimestamp() - t > 3)
            ////{
            ////    ret.HasError = true;
            ////    ret.ErrorMsg = "请求无效";
            ////    return ret;
            ////}

            ////  2.验证签名
            //string signServer = MFEncryptUtil.Md5(string.Format("{0}:{1}", t, deviceIdentifier));
            //if (!signServer.Equals(sign, StringComparison.CurrentCultureIgnoreCase))
            //{
            //    ret.HasError = true;
            //    ret.ErrorMsg = "请求无效";
            //    return ret;
            //}


            int type = Convert.ToInt32(jsonData["Type"].ToString());
            string userName = jsonData["UserName"].ToString();
            string pwd = jsonData["Pwd"].ToString();

            if (type == 0)
            {
                string channelId = jsonData["ChannelId"].ToString();
                ret.Type = "0"; 
                //注册
                MFReturnValue<int> retValue = AccountCacheModel.Instance.Register(userName, pwd, channelId, deviceIdentifier, deviceModel);
                ret.HasError = retValue.HasError;
                ret.ErrorMsg = retValue.Message;

                int userID = retValue.Value;
                AccountEntity entity = AccountCacheModel.Instance.GetEntity(userID);
                ret.Value = JsonMapper.ToJson(new RetAccountEntity(entity));

            }
            else
            {
                ret.Type = "1";  
                //登录
                AccountEntity entity = AccountCacheModel.Instance.LogOn(userName, pwd, deviceIdentifier, deviceModel);
                if (entity != null)
                {
                    ret.Value = JsonMapper.ToJson(new RetAccountEntity(entity));
                }
                else
                {
                    // 登录失败
                    ret.HasError = true;
                    ret.ErrorMsg = "帐户不存在";
                }
            }

            return ret;
        }

        // PUT: api/Account/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Account/5
        public void Delete(int id)
        {
        }
    }
}
