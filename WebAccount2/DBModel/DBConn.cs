using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public static class DBConn
{
    private static string m_DBAccount;

    public static string DBAccount
    {
        get
        {
            if (string.IsNullOrEmpty(m_DBAccount))
            {
                if ("ER01ZXNIQGFET10" == System.Net.Dns.GetHostName())
                {
                    m_DBAccount = "Data Source=.;Initial Catalog=DBAccount;User ID=sa;Password=123456";
                }
                else
                {
                    m_DBAccount = "Data Source=.;Initial Catalog=DBAccount;User ID=suzhen2;Password=123456";
                }
            }
            return m_DBAccount;
        }
    }
}