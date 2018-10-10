using LitJson;
using Mmcoy.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Web.Http;

namespace WebAccount.Controllers.api
{
    public class AppleRechargeController : ApiController
    {
        public string CreatePostHttpResponse(string datas, bool isSandbox = false)
        {
            //正式购买地址 沙盒购买地址  
            string url_buy = "https://buy.itunes.apple.com/verifyReceipt";
            string url_sandbox = "https://sandbox.itunes.apple.com/verifyReceipt";
            string url = isSandbox ? url_sandbox : url_buy;

            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.ProtocolVersion = HttpVersion.Version10;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            byte[] data = Encoding.GetEncoding("UTF-8").GetBytes(datas.ToString());
            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            Stream responseStream = response.GetResponseStream();   //获取响应的字符串流  
            StreamReader sr = new StreamReader(responseStream); //创建一个stream读取流  
            var str = sr.ReadToEnd();
            sr.Close();
            responseStream.Close();
            return str.ToString();
        }

        public string Get()
        {
            string receipt = "ewoJInNpZ25hdHVyZSIgPSAiQXhralNXbHJDNEdCWXErbVkwVHYrOFdmSUQ0RXpzZnNyOHYvQ3dPUnVEMzZtNkk3R0NwbktzYUhGWVMxN3oxSDR3VEYyU1g0VHpOVWhNUS9EZkhkNnErWmJRWkxydHlOU0k1YkRxQWNSWTZubmc3c2dLeitla2MyeVIzRVJEcUxlQm51QldFaGhoNGhnSDRLdjdpZm1BZVNHZ3I0NmxZNHRYTk53SGlQTG5qMzNsL0pkR2xXb2phSzRJRW4rdXA3OHQwdWloK3dZc1hLYXdsUkwxdlBnd2JiTDRMYnBXRDQxdnRUek1RcS9tQmJMdnVCSUJxNzBlUWNnNDNCZkhXWktXUU1EK2liQnYrYkpxZFFualdLVndGTUxEMGV1ZDRnTFBIVnpBMy83R0R4UndSM2cyMEE4MC9iTGx5dnFMRzJ6Z3hHczV3RjZuZXNyQWh1ZE5kbjF3a0FBQVdBTUlJRmZEQ0NCR1NnQXdJQkFnSUlEdXRYaCtlZUNZMHdEUVlKS29aSWh2Y05BUUVGQlFBd2daWXhDekFKQmdOVkJBWVRBbFZUTVJNd0VRWURWUVFLREFwQmNIQnNaU0JKYm1NdU1Td3dLZ1lEVlFRTERDTkJjSEJzWlNCWGIzSnNaSGRwWkdVZ1JHVjJaV3h2Y0dWeUlGSmxiR0YwYVc5dWN6RkVNRUlHQTFVRUF3dzdRWEJ3YkdVZ1YyOXliR1IzYVdSbElFUmxkbVZzYjNCbGNpQlNaV3hoZEdsdmJuTWdRMlZ5ZEdsbWFXTmhkR2x2YmlCQmRYUm9iM0pwZEhrd0hoY05NVFV4TVRFek1ESXhOVEE1V2hjTk1qTXdNakEzTWpFME9EUTNXakNCaVRFM01EVUdBMVVFQXd3dVRXRmpJRUZ3Y0NCVGRHOXlaU0JoYm1RZ2FWUjFibVZ6SUZOMGIzSmxJRkpsWTJWcGNIUWdVMmxuYm1sdVp6RXNNQ29HQTFVRUN3d2pRWEJ3YkdVZ1YyOXliR1IzYVdSbElFUmxkbVZzYjNCbGNpQlNaV3hoZEdsdmJuTXhFekFSQmdOVkJBb01Da0Z3Y0d4bElFbHVZeTR4Q3pBSkJnTlZCQVlUQWxWVE1JSUJJakFOQmdrcWhraUc5dzBCQVFFRkFBT0NBUThBTUlJQkNnS0NBUUVBcGMrQi9TV2lnVnZXaCswajJqTWNqdUlqd0tYRUpzczl4cC9zU2cxVmh2K2tBdGVYeWpsVWJYMS9zbFFZbmNRc1VuR09aSHVDem9tNlNkWUk1YlNJY2M4L1cwWXV4c1FkdUFPcFdLSUVQaUY0MWR1MzBJNFNqWU5NV3lwb041UEM4cjBleE5LaERFcFlVcXNTNCszZEg1Z1ZrRFV0d3N3U3lvMUlnZmRZZUZScjZJd3hOaDlLQmd4SFZQTTNrTGl5a29sOVg2U0ZTdUhBbk9DNnBMdUNsMlAwSzVQQi9UNXZ5c0gxUEttUFVockFKUXAyRHQ3K21mNy93bXYxVzE2c2MxRkpDRmFKekVPUXpJNkJBdENnbDdaY3NhRnBhWWVRRUdnbUpqbTRIUkJ6c0FwZHhYUFEzM1k3MkMzWmlCN2o3QWZQNG83UTAvb21WWUh2NGdOSkl3SURBUUFCbzRJQjF6Q0NBZE13UHdZSUt3WUJCUVVIQVFFRU16QXhNQzhHQ0NzR0FRVUZCekFCaGlOb2RIUndPaTh2YjJOemNDNWhjSEJzWlM1amIyMHZiMk56Y0RBekxYZDNaSEl3TkRBZEJnTlZIUTRFRmdRVWthU2MvTVIydDUrZ2l2Uk45WTgyWGUwckJJVXdEQVlEVlIwVEFRSC9CQUl3QURBZkJnTlZIU01FR0RBV2dCU0lKeGNKcWJZWVlJdnM2N3IyUjFuRlVsU2p0ekNDQVI0R0ExVWRJQVNDQVJVd2dnRVJNSUlCRFFZS0tvWklodmRqWkFVR0FUQ0IvakNCd3dZSUt3WUJCUVVIQWdJd2diWU1nYk5TWld4cFlXNWpaU0J2YmlCMGFHbHpJR05sY25ScFptbGpZWFJsSUdKNUlHRnVlU0J3WVhKMGVTQmhjM04xYldWeklHRmpZMlZ3ZEdGdVkyVWdiMllnZEdobElIUm9aVzRnWVhCd2JHbGpZV0pzWlNCemRHRnVaR0Z5WkNCMFpYSnRjeUJoYm1RZ1kyOXVaR2wwYVc5dWN5QnZaaUIxYzJVc0lHTmxjblJwWm1sallYUmxJSEJ2YkdsamVTQmhibVFnWTJWeWRHbG1hV05oZEdsdmJpQndjbUZqZEdsalpTQnpkR0YwWlcxbGJuUnpMakEyQmdnckJnRUZCUWNDQVJZcWFIUjBjRG92TDNkM2R5NWhjSEJzWlM1amIyMHZZMlZ5ZEdsbWFXTmhkR1ZoZFhSb2IzSnBkSGt2TUE0R0ExVWREd0VCL3dRRUF3SUhnREFRQmdvcWhraUc5Mk5rQmdzQkJBSUZBREFOQmdrcWhraUc5dzBCQVFVRkFBT0NBUUVBRGFZYjB5NDk0MXNyQjI1Q2xtelQ2SXhETUlKZjRGelJqYjY5RDcwYS9DV1MyNHlGdzRCWjMrUGkxeTRGRkt3TjI3YTQvdncxTG56THJSZHJqbjhmNUhlNXNXZVZ0Qk5lcGhtR2R2aGFJSlhuWTR3UGMvem83Y1lmcnBuNFpVaGNvT0FvT3NBUU55MjVvQVE1SDNPNXlBWDk4dDUvR2lvcWJpc0IvS0FnWE5ucmZTZW1NL2oxbU9DK1JOdXhUR2Y4YmdwUHllSUdxTktYODZlT2ExR2lXb1IxWmRFV0JHTGp3Vi8xQ0tuUGFObVNBTW5CakxQNGpRQmt1bGhnd0h5dmozWEthYmxiS3RZZGFHNllRdlZNcHpjWm04dzdISG9aUS9PamJiOUlZQVlNTnBJcjdONFl0UkhhTFNQUWp2eWdhWndYRzU2QWV6bEhSVEJoTDhjVHFBPT0iOwoJInB1cmNoYXNlLWluZm8iID0gImV3b0pJbTl5YVdkcGJtRnNMWEIxY21Ob1lYTmxMV1JoZEdVdGNITjBJaUE5SUNJeU1ERTRMVEEwTFRFM0lEQTRPakV5T2pFeElFRnRaWEpwWTJFdlRHOXpYMEZ1WjJWc1pYTWlPd29KSW5WdWFYRjFaUzFwWkdWdWRHbG1hV1Z5SWlBOUlDSXpNelEwWmpBM01Ea3pNRE5oTlRBMU9HSXpZVFpsT1RRMk5qY3paak5sTTJJeU9UUTFZamN6SWpzS0NTSnZjbWxuYVc1aGJDMTBjbUZ1YzJGamRHbHZiaTFwWkNJZ1BTQWlNVEF3TURBd01ETTVNVFV5TWpFeU55STdDZ2tpWW5aeWN5SWdQU0FpTUNJN0Nna2lkSEpoYm5OaFkzUnBiMjR0YVdRaUlEMGdJakV3TURBd01EQXpPVEUxTWpJeE1qY2lPd29KSW5GMVlXNTBhWFI1SWlBOUlDSXhJanNLQ1NKdmNtbG5hVzVoYkMxd2RYSmphR0Z6WlMxa1lYUmxMVzF6SWlBOUlDSXhOVEl6T1RjM09UTXhNREF3SWpzS0NTSjFibWx4ZFdVdGRtVnVaRzl5TFdsa1pXNTBhV1pwWlhJaUlEMGdJamcyTnpBNE1EQTRMVFpCTWpZdE5FRkRNUzA0TlVRd0xUVTBRa1F6UmtWQ09UVkZRU0k3Q2draWNISnZaSFZqZEMxcFpDSWdQU0FpTXpBd01pSTdDZ2tpYVhSbGJTMXBaQ0lnUFNBaU1UTTNNREF3TkRrek5DSTdDZ2tpZG1WeWMybHZiaTFsZUhSbGNtNWhiQzFwWkdWdWRHbG1hV1Z5SWlBOUlDSXdJanNLQ1NKcGN5MXBiaTFwYm5SeWJ5MXZabVpsY2kxd1pYSnBiMlFpSUQwZ0ltWmhiSE5sSWpzS0NTSndkWEpqYUdGelpTMWtZWFJsTFcxeklpQTlJQ0l4TlRJek9UYzNPVE14TURBd0lqc0tDU0p3ZFhKamFHRnpaUzFrWVhSbElpQTlJQ0l5TURFNExUQTBMVEUzSURFMU9qRXlPakV4SUVWMFl5OUhUVlFpT3dvSkltbHpMWFJ5YVdGc0xYQmxjbWx2WkNJZ1BTQWlabUZzYzJVaU93b0pJbTl5YVdkcGJtRnNMWEIxY21Ob1lYTmxMV1JoZEdVaUlEMGdJakl3TVRndE1EUXRNVGNnTVRVNk1USTZNVEVnUlhSakwwZE5WQ0k3Q2draVltbGtJaUE5SUNKamIyMHVlVzkxZVc5MUxtMXRieUk3Q2draWNIVnlZMmhoYzJVdFpHRjBaUzF3YzNRaUlEMGdJakl3TVRndE1EUXRNVGNnTURnNk1USTZNVEVnUVcxbGNtbGpZUzlNYjNOZlFXNW5aV3hsY3lJN0NuMD0iOwoJImVudmlyb25tZW50IiA9ICJTYW5kYm94IjsKCSJwb2QiID0gIjEwMCI7Cgkic2lnbmluZy1zdGF0dXMiID0gIjAiOwp9";


            string strJosn = string.Format("{{\"receipt-data\":\"{0}\"}}", receipt);
            // 请求验证  
            string strResult = CreatePostHttpResponse(strJosn, true);
            return strResult;
        }

        public RetValue Post([FromBody]string jsonStr)
        {
            RetValue ret = new RetValue();

            JsonData jsonData = JsonMapper.ToObject(jsonStr);

            //时间戳
            long t = Convert.ToInt64(jsonData["t"].ToString());
            string deviceIdentifier = jsonData["deviceIdentifier"].ToString();
            string deviceModel = jsonData["deviceModel"].ToString();
            string sign = jsonData["sign"].ToString();

            //1.判断时间戳 如果大于3秒 直接返回错误
            if (MFDSAUtil.GetTimestamp() - t > 3)
            {
                ret.HasError = true;
                ret.ErrorMsg = "请求无效";
                return ret;
            }

            //2.验证签名
            string signServer = MFEncryptUtil.Md5(string.Format("{0}:{1}", t, deviceIdentifier));
            if (!signServer.Equals(sign, StringComparison.CurrentCultureIgnoreCase))
            {
                ret.HasError = true;
                ret.ErrorMsg = "请求无效";
                return ret;
            }

            //苹果回执
            string receipt = jsonData["receipt"].ToString();

            string strJosn = string.Format("{{\"receipt-data\":\"{0}\"}}", receipt);
            // 请求验证  
            string strResult = CreatePostHttpResponse(strJosn, true);

            JsonData retJson = LitJson.JsonMapper.ToObject(strResult);

            int status = int.Parse(retJson["status"].ToString());
            if (status == 0)
            {
                //成功
                string retreceipt = retJson["receipt"].ToJson();
                JsonData retReceiptJson = JsonMapper.ToObject(retreceipt);
                string rechargeProductId = retReceiptJson["product_id"].ToString();
                ret.Value = rechargeProductId; //把充值产品编号传递给客户端
                //订单号 付费服务器识别码_玩家账号_要充值到哪个GameServerId_角色ID_充值的产品Id_时间
                string orderId = jsonData["orderId"].ToString();

                string[] arr = orderId.Split('_');
                if (arr.Length == 6)
                {
                    //1.记录充值日志
                    RechargeLogEntity rechargeLogEntity = new RechargeLogEntity();
                    rechargeLogEntity.AccountId = arr[1].ToInt();
                    short channelId = AccountCacheModel.Instance.GetEntity(rechargeLogEntity.AccountId).ChannelId;

                    rechargeLogEntity.ChannelId = channelId;
                    rechargeLogEntity.GameServerId = arr[2].ToInt();
                    rechargeLogEntity.RoldId = arr[3].ToInt();
                    rechargeLogEntity.RechargeProductId = short.Parse(rechargeProductId);
                    rechargeLogEntity.OrderId = orderId;
                    rechargeLogEntity.CreateTime = DateTime.Now;

                    RechargeLogCacheModel.Instance.Create(rechargeLogEntity);


                    //2.找到对应的游戏服
                    int gameServerId = arr[2].ToInt();

                    GameServerEntity entity = GameServerCacheModel.Instance.GetEntity(gameServerId);
                    if (entity != null)
                    {
                        //发送socket请求 给游戏服
                        Socket rechargeServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        rechargeServer.Connect(new IPEndPoint(IPAddress.Parse(entity.Ip), entity.Port));
                        string str = string.Format("{0}_{1}_{2}", channelId, arr[3], arr[4]);
                        rechargeServer.Send(System.Text.UTF8Encoding.UTF8.GetBytes(str));
                    }
                    else
                    {
                        ret.HasError = true;
                        ret.ErrorMsg = "充值失败";
                    }
                }
                else
                {
                    ret.HasError = true;
                    ret.ErrorMsg = "充值失败";
                }


            }
            else
            {
                ret.HasError = true;
                ret.ErrorMsg = "充值失败";
            }


            return ret;
        }
    }
}