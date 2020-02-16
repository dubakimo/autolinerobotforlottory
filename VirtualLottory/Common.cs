using database;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlServerCe;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace VirtualLottory
{
    public class Common
    {
        /// <summary>
        /// 檢查回傳歷史數據最新期連續失敗期數
        /// </summary>
        /// <param name="dt">歷史數據資料容器</param>
        /// <returns>失敗次數</returns>
        public static int checkfailure(DataTable dt)
        {

            int f = 0;
            for (int j = 0; j < dt.Rows.Count; j++)
            {
                if (dt.Rows[j]["PlanResult"].ToString() == "-1" && j == 0)
                {
                    f++;
                }
                else if (j > 0 && f == 0)
                {
                    break;
                }
                else if (j > 0 && f > 0)
                {
                    if (dt.Rows[j]["PlanResult"].ToString() == "-1")
                    {
                        f++;
                    }
                    else
                    {
                        break;
                    }

                }
            }

            return f;
        }

        /// <summary>
        /// 取得押注倍數
        /// </summary>
        /// <param name="type">4B/4D/6B/6D</param>
        /// <param name="plan">xfyt/pk10</param>
        /// <returns></returns>
        public static int  getTime(string type, string plan)
        {
            DataTable dt = new DataTable();

            StringBuilder sbSQL = new StringBuilder();
            //取得目前正在闖關中的資料
            //sbSQL.AppendFormat("select max(id) as id,CarNo,times,result,status,Period from history where type=@type and plans=@plan and status = 0 group by id,CarNo,times,result,status,Period");
            sbSQL.AppendFormat("select Top 1 * from history where type=@type and plans=@plan order by id desc");

            using (DBCeObject db = new DBCeObject())
            {

                List<SqlCeParameter> param = new List<SqlCeParameter>();
                param.Add(new SqlCeParameter("@type", type));
                param.Add(new SqlCeParameter("@plan", plan));

                dt = db.Select(sbSQL.ToString(), param);
            }

            //如果無資料表示一局的開始,反回押注倍數1
            if (dt.Rows.Count == 0)
                return 1;

            //如果history最近一期結果是成功1==>回傳1(從1倍開始下注)
            //if (dt.Rows[0]["status"].ToString() == "1" || dt.Rows[0]["status"].ToString() == "0")  
            if (dt.Rows[0]["status"].ToString() == "1")
                return 1;

            DataTable dts = new DataTable();

            int times = int.Parse(dt.Rows[0]["times"].ToString());
            //times 累加 ==>在times table中查出相對應的value
            StringBuilder sbSQL2 = new StringBuilder();
            sbSQL2.AppendFormat("select * from time where type=@type ");

            using (DBCeObject db = new DBCeObject())
            {

                List<SqlCeParameter> param = new List<SqlCeParameter>();
                param.Add(new SqlCeParameter("@type", type));

                dts = db.Select(sbSQL2.ToString(), param);
            }

            //抓出原本的times
            DataRow[] dr = dts.Select("value='" + times.ToString() + "'");

            int sn = int.Parse(dr[0]["sn"].ToString());

            if (sn == 10)
                sn = 0;

            //取得最新的times(押注倍數)
            DataRow[] drs = dts.Select("sn='" + (sn + 1).ToString() + "'");

            int RetValue = int.Parse(drs[0]["value"].ToString());

            return RetValue;
        }

        /// <summary>
        /// 更改狀態status : 0 ==> 1
        /// </summary>
        /// <param name="type">4B/4D/6B/6D</param>
        /// <param name="plan">xfyt/pk10</param>
        /// <param name="planID">75122</param>
        /// <param name="StartPeriod">57</param>
        /// <returns></returns>
        public static int upDatetData(string type, string plan, string planID, int status)
        {
            StringBuilder sbSQL = new StringBuilder();
            //取得目前正在闖關中的資料
            //sbSQL.AppendFormat("select max(id) as id,CarNo,times,result,status,Period from history where type=@type and plans=@plan and status = 0 group by id,CarNo,times,result,status,Period");
            sbSQL.AppendFormat(" Update history set status = @status,modtime=@modtime where  type=@type and plans=@plan and PlanId=@planID");


            using (DBCeObject db = new DBCeObject())
            {

                List<SqlCeParameter> param = new List<SqlCeParameter>();
                param.Add(new SqlCeParameter("@type", type));
                param.Add(new SqlCeParameter("@plan", plan));
                param.Add(new SqlCeParameter("@planID", planID));
                param.Add(new SqlCeParameter("@status", status));
                param.Add(new SqlCeParameter("@modtime", DateTime.Now));
                int i = db.ExecuteNonQuery(sbSQL.ToString(), param);

                return i;
            }

        }

        /// <summary>
        /// 確認最新反押距離上次成功反押期數是否差距6期
        /// </summary>
        /// <param name="type"></param>
        /// <param name="plan"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        public static int check6Period(string type, string plan,int period)
        {
            DataTable dt = getBetData(type, plan, 1);

            int oldp = 0;
            if (dt.Rows.Count != 0 )
            {
                oldp =  int.Parse(dt.Rows[0]["Period"].ToString());
            }
               
            int total = period - oldp;

            if (total > 6)
                return 1;  //表示超過6期
            else
                return 0;  //表示未超過6期


        }

        //取得目前最新紀錄
        /// <summary>
        /// 取得資料庫中history正在闖關計畫
        /// </summary>
        /// <param name="type">4D/4B/6B/6D</param>
        /// <param name="plan">pk10/xfyt</param>
        /// <param name="planID">闖關計畫流水id</param>
        /// <param name="status">狀態:0:闖關中,1:成功,-1:失敗</param>
        /// <returns></returns>
        public static DataTable getBetData(string type, string plan, int status)
        {
            BetList btl = new BetList();
            DataTable dt;
            StringBuilder sbSQL = new StringBuilder();
            //取得目前正在闖關中的資料
            //sbSQL.AppendFormat("select max(id) as id,CarNo,times,result,status,Period from history where type=@type and plans=@plan and status = 0 group by id,CarNo,times,result,status,Period");
            //sbSQL.AppendFormat("select Top 1 * from history where type=@type and plans=@plan and status =@status order by id desc");
            sbSQL.AppendFormat("select * from history where type=@type and plans=@plan and status =@status order by id desc");
            using (DBCeObject db = new DBCeObject())
            {

                List<SqlCeParameter> param = new List<SqlCeParameter>();
                param.Add(new SqlCeParameter("@type", type));
                param.Add(new SqlCeParameter("@plan", plan));
                param.Add(new SqlCeParameter("@status", status));

                dt = db.Select(sbSQL.ToString(), param);
            }

            return dt;
        }

        //確認目前該期的闖關計畫成功或失敗
        /// <summary>
        /// 檢查闖關是否會過關(正押判斷) 
        /// </summary>
        /// <param name="dr1">最新一期資料容器</param>
        /// <param name="dr2">歷史最新一期資料容器</param>
        /// <param name="type">4B/4D/6B/6D...</param>
        /// <param name="plan">彩種:pk10,xfyt</param>
        /// <param name="status">闖關狀態:0:闖關中,1:成功,-1:失敗</param>
        /// <returns>反押,正押判斷</returns>
        public static bool checkBetData(DataRow dr1, DataRow dr2, string type, string plan, int status, ref List<BetList> lstBet, ref List<BetList> ndBet)
        {
            //lstBet = new List<BetList>();
            //ndBet = new List<BetList>();
            string planID = dr1["PlanID"].ToString();
            //取得目前history中最新闖關中的資料
            DataTable dtHist = getBetData(type, plan, status);
            //history押注結果
            if (dtHist.Rows.Count == 0)
                return false;

            //比對回傳最新期數的Plan id 若與資料庫中相同==>return false
            //foreach (DataRow dr in dtHist.Rows)
            //{
            //    if (dr["PlanId"].ToString() == planID)
            //        return false;
            //}

            string result = dtHist.Rows[0]["result"].ToString();
            string times = dtHist.Rows[0]["times"].ToString();     //取得倍數
            //最新闖關押注
            string[] PlanDataText = dr1["PlanDataText"].ToString().Split(",".ToCharArray());
            //判斷是多少顆球
            int ball = PlanDataText.Length;
            //目前開出的大小單雙
            string few = dr1["PlanResultFew"].ToString();
            //回傳目前下注值(大,小,單,雙)  ==>最新一期few從0開始,歷史few從1開始
            string r = string.Empty;
            //if ((type == "4B" || type == "4D") && int.Parse(few) == 4)
            //if (int.Parse(few) == 0 && dtHist.Rows[0]["BetType"].ToString() =="0")
                //反押
                //r = PlanDataText[int.Parse(few)-1];
            //else
                //正押
                //r = PlanDataText[int.Parse(few)];
            //else
            //    r = PlanDataText[int.Parse(few)];

            if (dtHist.Rows.Count == 0)
            {
                //如果查不到回傳false
                return false;
            }
            else
            {
                //比對是否為同一個期數,如果不同期數=>離開
                if (planID != dtHist.Rows[0]["PlanId"].ToString())
                    return false;
                int i = 0;
                //如果有查到=>確認是否有過關
                if (dr1["PlanResult"].ToString() == "1")
                {    //如果過關==>Update history status
                    i = upDatetData(type, plan, planID, 1);
                }
                else if (dr1["PlanResult"].ToString() == "-1")
                {
                    i = upDatetData(type, plan, planID, -1);
                    //time如果為1023=>回報牌爆
                    if (times == "1023")
                    {
                        ////加入爆關訊息
                        BetList blst = new BetList();
                        blst.PlanId = planID;                     //計畫流水號
                        blst.StartPeriod = int.Parse(dtHist.Rows[0]["StartPeriod"].ToString());  //起始期數
                        blst.carNo = dtHist.Rows[0]["CarNo"].ToString();      //車號
                        string results = dtHist.Rows[0]["result"].ToString();
                        if (type == "4B" || type == "6B" || type == "8B")
                            blst.bigsmall = results;
                        else
                            blst.oddEven = results;
                        blst.times = int.Parse(dtHist.Rows[0]["times"].ToString());
                        blst.Period = int.Parse(dtHist.Rows[0]["Period"].ToString());
                        blst.type = type;   //4B/4D/6B/6D/8D/8B
                        ndBet.Add(blst);

                    }


                }
                else if (dr1["PlanResult"].ToString() == "0" && int.Parse(dr1["PlanResultFew"].ToString()) > 0)
                {
                    ////反押成功,Update 資料庫,並返回過關訊息
                    //i = upDatetData(type, plan, planID, 1);    //update資料庫
                    ////回傳過關訊息
                    int carNo = int.Parse(dr1["PlanPosition"].ToString()) + 1;
                    ////加入過關訊息
                    BetList blst = new BetList();


                    //該計畫闖關中 , 但進入第二顆 ==>要判斷歷史最新牌為失敗或成功來判斷是正押,還是反押
                    if (dr2["PlanResult"].ToString() == "-1" && dtHist.Rows[0]["betType"].ToString() == "1")
                    {
                        //判斷正押成功或失敗,必須判斷顆粒數是否有增加,且押注是否與資料庫中的投注相同
                        if (int.Parse(dtHist.Rows[0]["few"].ToString()) < int.Parse(few) && dtHist.Rows[0]["result"].ToString() == r)
                        {
                            i = upDatetData(type, plan, planID, -1);  //如果相同=>正押失敗

                            //blst.PlanId = planID;                     //計畫流水號
                            //blst.StartPeriod = int.Parse(dtHist.Rows[0]["StartPeriod"].ToString());  //起始期數
                            //blst.carNo = dtHist.Rows[0]["CarNo"].ToString();      //車號
                            //string results = dtHist.Rows[0]["result"].ToString();
                            //if (type == "4B" || type == "6B" || type == "8B")
                            //    blst.bigsmall = results;
                            //else
                            //    blst.oddEven = results;
                            //blst.times = int.Parse(dtHist.Rows[0]["times"].ToString());
                            //blst.Period = int.Parse(dtHist.Rows[0]["Period"].ToString());
                            //blst.type = type;   //4B/4D/6B/6D/8D/8B
                            //blst.status = "1";
                            //ndBet.Add(blst);
                        }
                        else
                        {
                            //blst.PlanId = planID;                     //計畫流水號
                            //blst.StartPeriod = int.Parse(dtHist.Rows[0]["StartPeriod"].ToString());  //起始期數
                            //blst.carNo = dtHist.Rows[0]["CarNo"].ToString();      //車號
                            //string results = dtHist.Rows[0]["result"].ToString();
                            //if (type == "4B" || type == "6B" || type == "8B")
                            //    blst.bigsmall = results;
                            //else
                            //    blst.oddEven = results;
                            //blst.times = int.Parse(dtHist.Rows[0]["times"].ToString());
                            //blst.Period = int.Parse(dtHist.Rows[0]["Period"].ToString());
                            //blst.type = type;   //4B/4D/6B/6D/8D/8B
                            //blst.status = "1";
                            ////加入是否已經有正押判斷
                            //if (Common.chkIntData2(blst, plan) == 1)
                            //{
                              
                            //    ndBet.Add(blst);
                         
                            //    i = upDatetData(type, plan, planID, 1);  //如果相同=>正押成功

                            //}
                         
                           
                        }
                        //time如果為1023=>回報牌爆
                        //if (times == "1023")
                        //{
                        //    ////加入爆關訊息
                        //    //BetList blst = new BetList();
                        //    blst.PlanId = planID;                     //計畫流水號
                        //    blst.StartPeriod = int.Parse(dtHist.Rows[0]["StartPeriod"].ToString());  //起始期數
                        //    blst.carNo = dtHist.Rows[0]["CarNo"].ToString();      //車號
                        //    string results = dtHist.Rows[0]["result"].ToString();
                        //    if (type == "4B" || type == "6B" || type == "8B")
                        //        blst.bigsmall = results;
                        //    else
                        //        blst.oddEven = results;
                        //    blst.times = int.Parse(dtHist.Rows[0]["times"].ToString());
                        //    blst.Period = int.Parse(dtHist.Rows[0]["Period"].ToString());
                        //    blst.type = type;   //4B/4D/6B/6D/8D/8B
                        //    blst.status = "1";
                        //    ndBet.Add(blst);
                        //}

                    }
                    else if (dr2["PlanResult"].ToString() == "1" && dtHist.Rows[0]["betType"].ToString()== "0")
                    {
                        //如果歷史第一期過關==>表示目前反壓中,且最新一期第一顆反押過關
                        blst.PlanId = planID;                     //計畫流水號
                        blst.StartPeriod = int.Parse(dtHist.Rows[0]["StartPeriod"].ToString());  //起始期數
                        blst.carNo = carNo.ToString();
                        string results = dtHist.Rows[0]["result"].ToString();
                        if (type == "4B" || type == "6B" || type == "8B")
                            blst.bigsmall = results;
                        else
                            blst.oddEven = results;
                        blst.times = int.Parse(dtHist.Rows[0]["times"].ToString());
                        blst.Period = int.Parse(dtHist.Rows[0]["Period"].ToString());
                        blst.type = type;   //4B/4D/6B/6D/8D/8B
                        blst.status = "1";
                        ndBet.Add(blst);
                        //更新資料庫
                        i = upDatetData(type, plan, planID, 1);

                    }
                    //else
                    //{

                    //    //如果歷史第一期過關==>表示目前反壓中,且最新一期第一顆反壓過關
                    //    blst.PlanId = planID;                     //計畫流水號
                    //    blst.StartPeriod = int.Parse(dtHist.Rows[0]["StartPeriod"].ToString());  //起始期數
                    //    blst.carNo = carNo.ToString();
                    //    string results = dtHist.Rows[0]["result"].ToString();
                    //    if (type == "4B" || type == "6B" || type == "8B")
                    //        blst.bigsmall = results;
                    //    else
                    //        blst.oddEven = results;
                    //    blst.times = int.Parse(dtHist.Rows[0]["times"].ToString());
                    //    blst.Period = int.Parse(dtHist.Rows[0]["Period"].ToString());
                    //    blst.type = type;   //4B/4D/6B/6D/8D/8B
                    //    ndBet.Add(blst);
                    //    //更新資料庫
                    //    i = upDatetData(type, plan, planID, 1);
                    //}
                    //如果闖關中,押注相同==>正押,表示不過關(本次押注失敗)
                    //string result = dr[""].ToString();
                    //如果闖關中,押注相反==>反押,表示過關(成功)

                    //i = upDatetData(type, plan, planID, 1);
                }

                if (i == 1)
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// 歷史數據比對
        /// </summary>
        /// <param name="dt">歷史闖關資料</param>
        /// <param name="dr2">最新一期闖關資料</param>
        /// <param name="type"></param>
        /// <param name="plan"></param>
        /// <param name="StartPeriod"></param>
        /// <returns></returns>
        public static DataRow[] CheckOldData(DataTable dt, DataRow dr2, string type, string plan, int StartPeriod, ref List<BetList> lstBet, ref List<BetList> ndBet)
        {
            DataRow[] dtr = null;
            //lstBet = new List<BetList>();
            //ndBet = new List<BetList>();
            //取得目前history資料表中最新闖關中失敗的資料
            DataTable dtHist = getBetData(type, plan, 0);

            if (dtHist.Rows.Count == 0)
                return dtr;
            //string planID = dtHist.Rows[0]["PlanID"].ToString();



            //如果比對期數資料為有資料==>update資料庫把status改成-1,否則改為1
            int status = 0;
            int few = 0;  //目前該計畫出到的顆數;
            string times = string.Empty;  //倍數

            //if (dtr.Length != 0 )
            foreach (DataRow dr in dtHist.Rows)
            {
                //判斷是正押還是反押 ,如果是正押=>與最新紀錄比對
                //if (dr["betType"].ToString() == "0") //反押
                //{
                //    //比對期數是否一致
                //    if (dr["PlanId"].ToString() == dr2["PlanId"].ToString())
                //    {
                //        few = int.Parse(dr["few"].ToString());
                //          string[] dxs = dr2["PlanDataText"].ToString().Split(",".ToCharArray());
                //        if (dr["result"].ToString() != dxs[few].ToString())
                //        {
                //            //表示反壓成功
                //            status = 1;
                //            upDatetData(type, plan, dr["PlanId"].ToString(), status);
                //            goto ShowMess;
                //        }
                //    }
                //}
                

                //與歷史關卡紀錄比對
                dtr = dt.Select("PlanId='" + dr["PlanId"].ToString() + "'");

                if (dtr.Length == 0)
                    continue;
                //正押判斷==>history中的大小單雙,如果與歷史的已過關第n期一致,表示過關
                status = int.Parse(dtr[0]["PlanResult"].ToString());   //歷史闖關成功=>等於反押失敗:=1
                few = int.Parse(dtr[0]["PlanResultFew"].ToString());   //歷史闖關顆數
                times = dr["times"].ToString();                        //倍數
                //取得前一期是否失敗(失敗表示為正押)
                DataRow dtr2 = null;

                //取得歷史數據前一期
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dr["PlanId"].ToString() == dt.Rows[i]["PlanID"].ToString())
                    {
                        dtr2 = dt.Rows[i + 1];
                        break;
                    }

                }

                string[] dx = dtr[0]["PlanDataText"].ToString().Split(",".ToCharArray());

                if (dtr2["PlanResult"].ToString() == "-1")
                {
                    //前一期失敗,表示正押
                  
                    //如果比對是同一個few .status = 1==>加入過關訊息
                    if (few -1 == int.Parse(dr["few"].ToString()))
                        upDatetData(type, plan, dr["PlanId"].ToString(), status);
                    else
                    {
                        status = -1;
                        upDatetData(type, plan, dr["PlanId"].ToString(), -1);
                    }
                       

                }
                else
                {
                    //否則前一期成功可能為反押
                    if (status == 1 || status == 0)
                    {  //該計畫成功, 投注(result)不同==>反押失敗
                        //if (dr["result"].ToString() != dx[few-1].ToString())    
                        if (dr["result"].ToString() != dx[0].ToString() && few >= 1 && dr["BetType"].ToString() == "0")    //當兩邊result不同,且result=1 or 0 ,表示為反壓成功(該次計畫過關,反押成功)
                        {
                            //前一關成功表示反壓失敗
                            upDatetData(type, plan, dr["PlanId"].ToString(), -1); //更新該關狀態
                            status = -1;
                            //反押失敗
                        }
                        else if (dr["result"].ToString() == dx[few-1].ToString() && few > 1 && dr["BetType"].ToString() == "1" )
                        {
                            //前一關成功表示正押成功
                            upDatetData(type, plan, dr["PlanId"].ToString(), 1); //更新該關狀態
                            status = 1;
                            
                        }
                        else if (dr["result"].ToString() == dx[0].ToString() && few >= 1 && dr["BetType"].ToString() == "0")
                        {
                            //表示本期過關,反押失敗
                            upDatetData(type, plan, dr["PlanId"].ToString(), -1); //更新該關狀態
                            status = -1;

                        }
                      
                    }
                    else
                    {
                        //若前一期失敗, 投注(result)相同==>正押
                        //if (dr["result"].ToString() == dx[few - 1].ToString())    //當兩邊result相同時表示違正押
                        if (dr["result"].ToString() == dx[few-1].ToString())    //當兩邊result相同時表示違正押
                        {
                            upDatetData(type, plan, dr["PlanId"].ToString(), status); //更新該關狀態
                            //status = 1;
                        }
                        else
                        {
                            //若前一期失敗,投注相反==>反押成功
                            upDatetData(type, plan, dr["PlanId"].ToString(), 1); //更新該關狀態
                            status = 1;
                        }
                    }

                }
            ShowMess:
                if (status == 1)
                {
                    //添加過關訊息
                    string r = dr["result"].ToString();
                    BetList blst = new BetList();
                    blst.PlanId = dr["PlanId"].ToString();                     //計畫流水號
                    blst.StartPeriod = int.Parse(dr["StartPeriod"].ToString());  //起始期數
                    blst.carNo = dr["CarNo"].ToString();

                    string results = dr["result"].ToString();  //投注單
                    if (type == "4B" || type == "6B" || type == "8B")
                        blst.bigsmall = results;
                    else
                        blst.oddEven = results;
                    blst.times = int.Parse(dr["times"].ToString());
                    blst.Period = int.Parse(dr["Period"].ToString());
                    blst.type = type;   //4B/4D/6B/6D/8D/8B
                    blst.status = "1";  //過關
                    ndBet.Add(blst);
                }
                else
                {
                    //time如果為1023=>回報牌爆
                    if (times == "1023")
                    {
                        ////加入爆關訊息
                        BetList blst = new BetList();
                        blst.PlanId = dr["PlanId"].ToString();                     //計畫流水號
                        blst.StartPeriod = int.Parse(dr["StartPeriod"].ToString());  //起始期數
                        blst.carNo = dr["CarNo"].ToString();      //車號
                        string results = dr["result"].ToString();
                        if (type == "4B" || type == "6B" || type == "8B")
                            blst.bigsmall = results;
                        else
                            blst.oddEven = results;
                        blst.times = int.Parse(dr["times"].ToString());
                        blst.Period = int.Parse(dr["Period"].ToString());
                        blst.status = "-1";
                        blst.type = type;   //4B/4D/6B/6D/8D/8B
                        ndBet.Add(blst);
                    }
                }


            }

            return dtr;

        }

    

        public static DataTable LINQToDataTable<T>(IEnumerable<T> varlist)
        {
            DataTable dtReturn = new DataTable();

            // column names 
            PropertyInfo[] oProps = null;

            if (varlist == null) return dtReturn;

            foreach (T rec in varlist)
            {
                // Use reflection to get property names, to create table, Only first time, others 
                // will follow 
                if (oProps == null)
                {
                    oProps = ((Type)rec.GetType()).GetProperties();
                    foreach (PropertyInfo pi in oProps)
                    {
                        Type colType = pi.PropertyType;

                        if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition()
                        == typeof(Nullable<>)))
                        {
                            colType = colType.GetGenericArguments()[0];
                        }

                        dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                    }
                }

                DataRow dr = dtReturn.NewRow();

                foreach (PropertyInfo pi in oProps)
                {
                    dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue
                    (rec, null);
                }

                dtReturn.Rows.Add(dr);
            }
            return dtReturn;
        }


        //匯出資料到Excel表
        public static  void ExportToWorkSheet(DataTable dt, ref Microsoft.Office.Interop.Excel.Worksheet ws)
        {
            //標頭處理
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                ws.Cells[1, i + 1] = dt.Columns[i].ColumnName;

            }

            //資料處理
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (dt.Rows[i][j].GetType() == typeof(string))
                    {
                        ws.Cells[i + 2, j + 1] = "'" + dt.Rows[i][j].ToString();
                    }
                    else
                    {
                        ws.Cells[i + 2, j + 1] = dt.Rows[i][j].ToString();
                    }
                }
            }

        }
        //匯出資料到cvs檔案
        public static void ExportToExcel(DataTable dt, ref  Microsoft.Office.Interop.Excel.Application excel)
        {
            excel.Workbooks.Add(true);
            //標頭處理
            for (int i = 0; i < dt.Columns.Count - 1; i++)
            {
                if (dt.Columns[i].ColumnName != "Patient_ID")
                {
                    excel.Cells[1, i + 1] = dt.Columns[i].ColumnName;
                }

            }

            //資料處理
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 0; j < dt.Columns.Count - 1; j++)
                {
                    if (dt.Columns[j].ColumnName != "Patient_ID")
                    {
                        if (dt.Rows[i][j].GetType() == typeof(string))
                        {
                            excel.Cells[i + 2, j + 1] = "'" + dt.Rows[i][j].ToString();
                        }
                        else
                        {
                            excel.Cells[i + 2, j + 1] = dt.Rows[i][j].ToString();
                        }
                    }
                }
            }

            excel.DisplayAlerts = false;
            excel.AlertBeforeOverwriting = false;
        }

        public static void SaveToCSV(DataTable oTable, string FilePath)
        {
            string data = "";
            string s = string.Empty;
            string r = string.Empty;
            StreamWriter wr = new StreamWriter(FilePath, false, System.Text.Encoding.UTF8);

            string[] HeaderStr = new string[] {"彩種","日期","期數","車號","大小單雙","倍數","輸贏", "彩金","累積總獎金"};

            //設定標頭對應文字
            for (int i = 0; i < HeaderStr.Length; i++)
            {
                 s = string.Format("{0}", (HeaderStr[i]).ToString());

                data += s + ",";

            }
            data += "\n";
            wr.Write(data);
            data = "";

            foreach (DataRow row in oTable.Rows)
            {
                int j = 0;
                foreach (DataColumn column in oTable.Columns)
                {
                    if (!column.Caption.Equals("Patient_ID") && !column.Caption.Equals("StartDT") && !column.Caption.Equals("EndDT") && !column.Caption.Equals("DevTypeID") && !column.Caption.Equals("FWversion"))
                    {
                        if (j == 9)   //將德國 8,5 轉成 8.5
                        {
                            data += row[column].ToString().Replace(",", ".").Trim() + ",";
                        }
                        else
                        {
                            data += row[column].ToString().Trim() + ",";
                        }
                    }
                    j++;
                }
                data += "\n";
                wr.Write(data);
                data = "";
            }
            data += "\n";

            wr.Dispose();
            wr.Close();
        }

        /// <summary>
        /// 取得歷史投注紀錄報表
        /// </summary>
        /// <returns></returns>
        public static DataTable getHistory()
        {
            StringBuilder sbSQL = new StringBuilder();
            DataTable dthis = new DataTable();
            sbSQL.AppendFormat("select plans,postime,Period,CarNo,result,times,status,status*2*times as Cash from history");

            using (DBCeObject db = new DBCeObject())
            {

                //List<SqlCeParameter> param = new List<SqlCeParameter>();
                //param.Add(new SqlCeParameter("@type", type));
                //param.Add(new SqlCeParameter("@plan", plan));

                dthis = db.Select(sbSQL.ToString());
            }

            return dthis;
        }

        public static bool AddHistData(DataTable dt, string type, string plan)
        {
            //dt資料加入type & plan欄位以及欄位值
            dt.Columns.Add("type");
            dt.Columns.Add("plans"); dt.AcceptChanges();
            foreach (DataRow dr in dt.Rows)
            {
                dr["type"] = type;
                dr["plans"] = plan;
            }
            dt.AcceptChanges();

            //刪除資料庫中舊有的舊歷史資料
            StringBuilder sbSQL = new StringBuilder();
            sbSQL.AppendFormat("delete hisData where type=@type and plans=@plan ");

            using (DBCeObject db = new DBCeObject())
            {
                List<SqlCeParameter> param = new List<SqlCeParameter>();
                param.Add(new SqlCeParameter("@type", type));
                param.Add(new SqlCeParameter("@plan", plan));
                //刪除原有的歷史資料
                db.ExecuteNonQuery(sbSQL.ToString(), param);
            }
            //將最新的歷史資料一次性寫入資料庫
            IList<string> maplist = new List<string>();

            foreach (DataColumn col in dt.Columns)
            {
                maplist.Add(col.ColumnName);
            }

            //SQLce bulk  ==>將最新的資料一次性寫入資料庫
            bool flags = DBClass.BulkCopyImport(maplist, "hisData", dt);

            return flags;
        }

        /// <summary>
        /// 檢查回傳歷史資料是否與資料庫闖關資料一致
        /// </summary>
        /// <param name="dt">最新抓回的歷史數據</param>
        /// <param name="type">4B/4D/6B/6D/8B/8D</param>
        /// <param name="plan">pk10/xfyt彩種</param>
        /// <returns>一致傳回true </returns>
        public static int chkhisData(DataTable dt, string type, string plan)
        {

            StringBuilder sbSQL = new StringBuilder();
            DataTable dthis = new DataTable();
            sbSQL.AppendFormat("select * from hisData where type=@type and plans=@plan ");

            using (DBCeObject db = new DBCeObject())
            {

                List<SqlCeParameter> param = new List<SqlCeParameter>();
                param.Add(new SqlCeParameter("@type", type));
                param.Add(new SqlCeParameter("@plan", plan));

                dthis = db.Select(sbSQL.ToString(), param);
            }

            //計算取得累加數
            int total = 0;
            //將跑回圈相互做數據比對
            for (int i = 1; i < 6; i++)
            {
                DataRow[] dtr = dthis.Select("PlanId='" + dt.Rows[i]["PlanId"].ToString() + "'"); //對目前回傳的最新數據最比對查詢
                if (dtr.Length == 1)
                    total++;

            }
            if (total == 5)
            {
                //刪除舊有的闖關記錄後,新增10筆闖關紀錄
                AddHistData(dt, type, plan);
                return 1;    //如果五筆都存在則更新資料庫歷史資料,並回傳1 
            }
            else
                return -1;   //如果不足5筆==>回傳失敗
        }


        //確認新增反押投注是否已經存在於資料庫
        public static int chkIntData(BetList btl, string plan)
        {
            StringBuilder sbSQL = new StringBuilder();
            DataTable dt = new DataTable();
            sbSQL.AppendFormat("select * from history where type=@type and plans=@plan and PlanId=@planID  and BetType = 0 order by id desc");
            //反押不可重複下注單判斷
            using (DBCeObject db = new DBCeObject())
            {

                List<SqlCeParameter> param = new List<SqlCeParameter>();
                param.Add(new SqlCeParameter("@type", btl.type));
                param.Add(new SqlCeParameter("@plan", plan));
                param.Add(new SqlCeParameter("@planID", btl.PlanId));
                //param.Add(new SqlCeParameter("@status", btl.status));

                dt = db.Select(sbSQL.ToString(), param);
            }

            if (dt == null)
                return 0;
            else
            {
                if (dt.Rows.Count == 1)
                    return 1;
                else
                    return 0;
            }

        }


        //確認新正押投注是否已經存在於資料庫
        public static int chkIntData2(BetList btl, string plan)
        {
            StringBuilder sbSQL = new StringBuilder();
            DataTable dt = new DataTable();
            sbSQL.AppendFormat("select * from history where type=@type and plans=@plan and Period=@Period  and BetType = 1 order by id desc");
            //正押不可重複下注單判斷
            using (DBCeObject db = new DBCeObject())
            {

                List<SqlCeParameter> param = new List<SqlCeParameter>();
                param.Add(new SqlCeParameter("@type", btl.type));
                param.Add(new SqlCeParameter("@plan", plan));
                param.Add(new SqlCeParameter("@Period", btl.Period ));
                //param.Add(new SqlCeParameter("@status", btl.status));

                dt = db.Select(sbSQL.ToString(), param);
            }

            if (dt == null)
                return 0;
            else
            {
                if (dt.Rows.Count == 1)
                    return 1;
                else
                    return 0;
            }

        }




        //新增最新下注資料
        /// <summary>
        /// 新增投注資料
        /// </summary>
        /// <param name="btl">下注物件</param>
        /// <param name="plan">彩種,pk10/xfyt</param>
        public static void InsertBetData(BetList btl, string plan)
        {
            using (DBCeObject db = new DBCeObject())
            {
                db.ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
                
                bool bSuccess = true;
                string betString = string.Empty;
                if (string.IsNullOrEmpty(btl.oddEven))
                    betString = btl.bigsmall;
                else
                    betString = btl.oddEven;
                //db.Clear();
                db.SetValue("PlanId", btl.PlanId, SqlDbType.NVarChar, 20);       //計畫流水號
                db.SetValue("StartPeriod", btl.StartPeriod, SqlDbType.Int, 0);    //起始期號
                db.SetValue("CarNo", int.Parse(btl.carNo), SqlDbType.Int, 0);       //名次/車號
                db.SetValue("Period", btl.Period.ToString(), SqlDbType.NVarChar, 20);  //期數
                db.SetValue("times", btl.times, SqlDbType.Int, 0);         //倍數
                db.SetValue("type", btl.type, SqlDbType.NVarChar, 20);      //種類:4B,6D
                db.SetValue("result", betString, SqlDbType.NVarChar, 25);       //大小單雙
                db.SetValue("postime", DateTime.Now, SqlDbType.DateTime, 0);  //建立時間
                db.SetValue("modtime", DateTime.Now, SqlDbType.DateTime, 0);   //修改時間
                db.SetValue("plans", plan, SqlDbType.NVarChar, 25);              //彩種
                db.SetValue("few", btl.few, SqlDbType.Int, 0);                    //目前該計畫第幾顆
                db.SetValue("BetType", btl.BetType, SqlDbType.Int, 0);          //目前該注單是正押還是反押
                db.SetValue("status", 0, SqlDbType.Int, 0);                    //狀態0:闖關中,1:成功,-1:失敗

                db.Insert("history");
                if (db.IsSuccess)
                {

                }
                db.Clear();
            }
        }
    }

    public class BetList
    {
        public string PlanId { get; set; }    //計畫流水號
        public string carNo { get; set; }    //車次名號
        public string bigsmall { get; set; }  //大小
        public string oddEven { get; set; }   //單雙
        //public int record  { get; set; }   //期數
        public int Period { get; set; }      //期數
        public int few { get; set; }         //目前下注在該計畫的第幾顆,1為起始值
        public int StartPeriod { get; set; }   //起始期數
        public int times { get; set; }       //注金/倍數
        public string type { get; set; }   //那一種關,大小單雙,ex 6B(六期大小) , 4D(4期單雙)
        public string status { get; set; }   //狀況,0:闖關中,1:成功,-1:失敗
        public int BetType { get; set; }    //正押或是反壓, 1:正押,0:反押
    }
}
