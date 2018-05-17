/*----------------------------------------------------------------
 *  Copyright (C) 2018 天下商机（txooo.com）版权所有
 * 
 *  文 件 名：Sales
 *  所属项目：
 *  创建用户：马发才
 *  创建时间：2018/5/4 11:10:07
 *  
 *  功能描述：
 *          1、
 *          2、
 * 
 *  修改标识：
 *  修改描述：
 *  待 完 善：
 *          1、
----------------------------------------------------------------*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Txooo.Data.Entity;

namespace ConsoleApplication1
{
    public class Sales
    {
        public static DataTable GetUserDb(int pageIndex, int pageSize, string userId)
        {
            var _hash = new Hashtable();
            _hash["@user_id"] = userId;

            int totalCount = 0;
            var _dt = DataEntityHelper.GetTable(
              "Agent93390Test",
             " sales_user WITH(NOLOCK) ",
              _hash,
              " user_id asc",
              out totalCount,
              " and [user_id]>@user_id ",
              "*",
              pageIndex * pageSize,
              pageSize);
            return _dt;
        }
    }
}
