using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Configuration;
using ThreadingTimer = System.Threading.Timer;
using TimersTimer = System.Timers.Timer;
using System.Configuration;


namespace VirtualLottory
{
    public partial class Form1 : Form
    {

        //程式開始
        ThreadingTimer _ThreadTimer = null;
        TimersTimer _TimersTimer = null;

       
        public string strPk10new4;
        public string strPk10his4;
        public string strPK10new6;
        public string strPK10his6;
        public string strPK10new8;
        public string strPK10his8;
        public string strxyftnew4;
        public string strxyfthis4;
        public string strxyftnew6;
        public string strxyfthis6;
        public string strxyftnew8;
        public string strxyfthis8;




        public Form1()
        {
            InitializeComponent();
            //108/3/26 : Arthur Timer Thread
            Thread t = Thread.CurrentThread;
            bool IsThreadPool = t.IsThreadPoolThread;
            bool IsBackground = t.IsBackground;
            t.Name = "Main Thread";
            string msg = string.Format("Thread[{0}]:{1},Is ThreadPool=[{2}],Is Background=[{3}]", t.ManagedThreadId, t.ThreadState, IsThreadPool, IsBackground);
            this.Text = msg;
        }
    
        //// <summary> 
        /// 產生不重複的亂數 
        /// </summary> 
        /// <param name="intLower"></param>產生亂數的範圍下限 
        /// <param name="intUpper"></param>產生亂數的範圍上限 
        /// <param name="intNum"></param>產生亂數的數量 
        /// <returns></returns> 
        private System.Collections.ArrayList MakeRand(int intLower, int intUpper, int intNum)
        {
            System.Collections.ArrayList arrayRand = new System.Collections.ArrayList();
            Random random = new Random((int)DateTime.Now.Ticks);
            int intRnd;
            while (arrayRand.Count < intNum)
            {
                intRnd = random.Next(intLower, intUpper + 1);
                if (!arrayRand.Contains(intRnd))
                {
                    arrayRand.Add(intRnd);
                }
            }
            return arrayRand;
        }
        //取得投注獲利報表
        private void button2_Click(object sender, EventArgs e)
        {
            DataTable dt = Common.getHistory();

            //2015/01/21 原本用Excel 物件列印法
            string strTemplatePath = ConfigurationManager.AppSettings["csv_temp"].ToString();
            string strDirectory = "C:\\pk10data";
            string fileName = "pk10_" + DateTime.Now.Ticks + ".csv";
            string fileNameXls = "pk10_" + DateTime.Now.Ticks + ".xlsx";
            string strzTargetPath = strDirectory + "\\" + fileName;
            string strzXlsTargetPath = strDirectory + "\\" + fileNameXls;
            //StreamWriter sw = new StreamWriter(MainWindow.LsInfo.SavePath + "\\LongTerm_" + MainWindow.LsInfo.PatientID + ".csv",false,Encoding.Unicode);
            //StreamWriter sw = new StreamWriter(MainWindow.LsInfo.SavePath + "\\LongTerm_" + MainWindow.LsInfo.PatientID + ".csv", false);
            //DataTable dt = Get_PDF_Data();

            int iColCount = dt.Columns.Count;
            string s = string.Empty;
          
            Common.SaveToCSV(dt, strzTargetPath);

            FileInfo fileinfo = new FileInfo(strzTargetPath);
            if (!fileinfo.Exists)
            {
                string mess = strzTargetPath + " " + "msg_NOTFOUND";
                MessageBox.Show(mess);
                return;
            }

            Microsoft.Win32.SaveFileDialog sd = new Microsoft.Win32.SaveFileDialog();
            sd.FileName = fileinfo.Name;
            sd.Filter = "csc|*.CSV";
            sd.DefaultExt = ".csv";
            if (sd.ShowDialog().GetValueOrDefault())
            {

                fileinfo.CopyTo(sd.FileName);

            }

            MessageBox.Show("Completed");
           
        }
        /// <summary>
        /// 呼叫api,重新載入最新數據
        /// </summary>
        private void reload()
        {

           

            //sw.Start();//碼表開始計時
            //北京賽車4期最新一期
            strPk10new4 = Run(@"https://csj.1396j.com/pk10/GetNewsPassPlan?fewOff=4");
            //北京賽車歷史4期
            strPk10his4 = Run(@"http://csj.1396j.com/pk10/GetPassPlanAjax?fewOff=4&totalcount=30");

            //txtTime.Text = sw.Elapsed.TotalMilliseconds.ToString();
            //sw.Stop();

            //sw.Reset();//碼表歸零

            //sw.Start();//碼表開始計時
            //北京賽車6期最新一期
            strPK10new6 = Run(@"https://csj.1396j.com/pk10/GetNewsPassPlan?fewOff=6");
            //北京賽車歷史6期
            strPK10his6 = Run(@"http://csj.1396j.com/pk10/GetPassPlanAjax?fewOff=6&totalcount=30");

            //lblpk10_6.Text = sw.Elapsed.TotalMilliseconds.ToString();
            //sw.Stop();

            //sw.Reset();//碼表歸零

            //sw.Start();//碼表開始計時
            //北京賽車8期最新一期
            strPK10new8 = Run(@"https://csj.1396j.com/pk10/GetNewsPassPlan?fewOff=8");
            //北京賽車歷史8期
            strPK10his8 = Run(@"http://csj.1396j.com/pk10/GetPassPlanAjax?fewOff=8&totalcount=30");

            //lblpk10_8.Text = sw.Elapsed.TotalMilliseconds.ToString();
            //sw.Stop();


            //sw.Reset();//碼表歸零

            //sw.Start();//碼表開始計時

            //飛艇4期最新一期
            strxyftnew4 = Run(@"https://csj.1396j.com/xyft/GetNewsPassPlan?fewOff=4");
            //飛艇歷史4期
            strxyfthis4 = Run(@"http://csj.1396j.com/xyft/GetPassPlanAjax?fewOff=4&totalcount=30");

            //lblft_4.Text = sw.Elapsed.TotalMilliseconds.ToString();
            //sw.Stop();

            //sw.Reset();//碼表歸零

            //sw.Start();//碼表開始計時

            //飛艇6期最新一期
            strxyftnew6 = Run(@"https://csj.1396j.com/xyft/GetNewsPassPlan?fewOff=6");
            //飛艇歷史6期
            strxyfthis6 = Run(@"http://csj.1396j.com/xyft/GetPassPlanAjax?fewOff=6&totalcount=30");

            //lblft_6.Text = sw.Elapsed.TotalMilliseconds.ToString();
            //sw.Stop();

            //sw.Reset();//碼表歸零

            //sw.Start();//碼表開始計時

            //飛艇8期最新一期
            strxyftnew8  = Run(@"https://csj.1396j.com/xyft/GetNewsPassPlan?fewOff=8");
            //飛艇歷史8期
            strxyfthis8 = Run(@"http://csj.1396j.com/xyft/GetPassPlanAjax?fewOff=8&totalcount=80");

            //lblft_8.Text = sw.Elapsed.TotalMilliseconds.ToString();
            //sw.Stop();
        }

        /// <summary>
        /// 运行
        /// </summary>
        /// <returns></returns>
        public string Run(string arg)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo("c:\\curl\\curl.exe");
            startInfo.WindowStyle = ProcessWindowStyle.Minimized;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardInput = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = false;
            startInfo.StandardOutputEncoding = Encoding.UTF8;
            //startInfo.StandardErrorEncoding = Encoding.UTF8;
            startInfo.CreateNoWindow = true;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;

            startInfo.Arguments = string.Format("-L -b curl.cookies --location --request GET " + "\"{0}\"",arg);

            Process process = null;
            try
            {
                process = Process.Start(startInfo);
            }
            catch
            {
                return process.StandardError.ReadToEnd();
            }

            StreamReader str = process.StandardOutput;

            string resaultValue = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            process.Close();
            return resaultValue;

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        //飛艇闖關分析
        public void getxfyt(string html, string html2, string html3, string html4, string html5, string html6)
        {
            List<BetList> lstBet = new List<BetList>();   //下注資料List

            List<BetList> ndBet = new List<BetList>();   //過關資料

            int carNo = 0;      //車次名號
            string bigsmall = string.Empty;   //大小
            string oddEven = string.Empty;
            string record = string.Empty;     //期數

            #region "測試用"
            StreamReader srnews = new StreamReader("C:\\pk10data\\4xyftnew.txt");
            StreamReader srhis = new StreamReader("C:\\pk10data\\4xyfthis.txt");

            html = srnews.ReadToEnd();  //六期最新一期
            html2 = srhis.ReadToEnd(); //六期歷史
            html3 = html;   //八期最新一期
            html4 = html2;  //八期歷史
            html5 = html;   //4期最新一期
            html6 = html2;  //4期歷史
            srnews.Close();
            srhis.Close();
            #endregion


            // 使用預設編碼讀入 HTML 
            HtmlAgilityPack.HtmlDocument htDoc = new HtmlAgilityPack.HtmlDocument();
            HtmlAgilityPack.HtmlDocument htDoc2 = new HtmlAgilityPack.HtmlDocument();
            HtmlAgilityPack.HtmlDocument htDoc3 = new HtmlAgilityPack.HtmlDocument();
            HtmlAgilityPack.HtmlDocument htDoc4 = new HtmlAgilityPack.HtmlDocument();
            HtmlAgilityPack.HtmlDocument htDoc5 = new HtmlAgilityPack.HtmlDocument();
            HtmlAgilityPack.HtmlDocument htDoc6 = new HtmlAgilityPack.HtmlDocument();


            try
            {
                //byte[] dbytes = IndexAsync(uri.ToString()).Result;
                //MemoryStream ms = new MemoryStream(url.DownloadData("https://www.1396j.com/pk10/passplan?utm=52kj"));
                //string responseStr = System.Text.Encoding.Unicode.GetString(dbytes);
                htDoc.LoadHtml(html);     //飛艇闖關計畫6關最新一期
                htDoc2.LoadHtml(html2);   //飛艇闖關計畫6關歷史期數
                htDoc3.LoadHtml(html3);    //飛艇闖關計畫8關最新一期
                htDoc4.LoadHtml(html4);   //飛艇闖關計畫8關歷史期數
                htDoc5.LoadHtml(html5);    //飛艇闖關計畫8關最新一期
                htDoc6.LoadHtml(html6);   //飛艇闖關計畫8關歷史期數

            }
            catch (Exception ex)
            {
                throw ex;
            }

            //回傳json格式資料
            string jsonfile1 = htDoc.DocumentNode.InnerText;   //六期最新一期闖關計畫
            string jsonfile2 = htDoc2.DocumentNode.InnerText;  //六期歷史闖關計畫
            string jsonfile3 = htDoc3.DocumentNode.InnerText;  //八期最新一期闖關計畫
            string jsonfile4 = htDoc4.DocumentNode.InnerText;  //八期歷史闖關計畫
            string jsonfile5 = htDoc5.DocumentNode.InnerText;  //四期最新一期闖關計畫
            string jsonfile6 = htDoc6.DocumentNode.InnerText;  //四期歷史闖關計畫

            if (jsonfile1.IndexOf("html") != -1) return;
            if (jsonfile2.IndexOf("html") != -1) return;
            if (jsonfile3.IndexOf("html") != -1) return;
            if (jsonfile4.IndexOf("html") != -1) return;
            if (jsonfile5.IndexOf("html") != -1) return;
            if (jsonfile6.IndexOf("html") != -1) return;

            DataSet myDataSet1 = JsonConvert.DeserializeObject<DataSet>(jsonfile1);
            DataSet myDataSet2 = JsonConvert.DeserializeObject<DataSet>(jsonfile2);
            DataSet myDataSet3 = JsonConvert.DeserializeObject<DataSet>(jsonfile3);
            DataSet myDataSet4 = JsonConvert.DeserializeObject<DataSet>(jsonfile4);
            DataSet myDataSet5 = JsonConvert.DeserializeObject<DataSet>(jsonfile5);
            DataSet myDataSet6 = JsonConvert.DeserializeObject<DataSet>(jsonfile6);


            DataTable dt1 = myDataSet1.Tables[0];   //六期最新報號 0:bigSmall  
            DataTable dt2 = myDataSet2.Tables[0];   //六期歷史數據
            DataTable dt3 = myDataSet1.Tables[1];   //六期最新報號 1:oddEven 
            DataTable dt4 = myDataSet2.Tables[1];   //六期歷史數據
            DataTable dt5 = myDataSet3.Tables[0];   //八期最新報號 0:bigSmall 
            DataTable dt6 = myDataSet4.Tables[0];   //八期歷史數據
            DataTable dt7 = myDataSet3.Tables[1];   //八期最新報號 1:oddEven
            DataTable dt8 = myDataSet4.Tables[1];   //八期歷史數據
            DataTable dt9 = myDataSet5.Tables[0];   //四期最新報號 0:bigSmall 
            DataTable dt10 = myDataSet6.Tables[0];  //四期歷史數據
            DataTable dt11 = myDataSet5.Tables[1];  //四期最新報號 1:oddEven 
            DataTable dt12 = myDataSet6.Tables[1];  //四期歷史數據

            bool flag = false;

            string few = string.Empty;

            int i = 0;

            #region "飛艇6期"

            //六期目前開出的大小
            few = dt1.Rows[i]["PlanResultFew"].ToString();
            DataRow dr1 = dt1.Rows[i];   //最新一期
            DataRow dr2 = dt2.Rows[i];  //歷史計畫最新一期



            //最新歷史計畫與資料庫已存在歷史資料比對
            //if (Common.chkhisData(dt2, "6B", "xfyt") != 1)
            //    goto Plan6D;


            //先檢查history中4B闖關是否過關
            bool oddFLag = Common.checkBetData(dr1, dr2, "6B", "xfyt", 0, ref lstBet, ref ndBet);
            Common.CheckOldData(dt2, dr1, "6B", "xfyt", 0, ref lstBet, ref ndBet);

            //DataRow[] dr = CheckOldData(dt2,dr1, "6B", "xfyt", -1);   //回傳歷史第一期資料

            //計算歷史數據連續失敗的次數
            int failure = 0;
            failure = Common.checkfailure(dt2);

            //正押判斷
            if (dt1.Rows[i]["PlanResult"].ToString() == "0" && int.Parse(few) >= 0 && dt2.Rows[0]["PlanResult"].ToString() == "-1")
            {     //最新期數闖關中,大於1期==>抓出歷史數據第一期為失敗 

                //回傳車道名次
                carNo = int.Parse(dt1.Rows[i]["PlanPosition"].ToString()) + 1;
                //闖關中,開出關數少於五關,且前一期失敗=>回傳車號與下注值
                //計畫大小列表
                string[] PlanDataText = dt1.Rows[i]["PlanDataText"].ToString().Split(",".ToCharArray());
                //回傳下注值(大or 小)
                bigsmall = PlanDataText[int.Parse(dt1.Rows[i]["PlanResultFew"].ToString())];

                BetList blst = new BetList();
                blst.PlanId = dt1.Rows[i]["PlanId"].ToString();                     //計畫流水號
                blst.StartPeriod = int.Parse(dt1.Rows[i]["StartPeriod"].ToString());  //起始期數
                blst.carNo = carNo.ToString();
                blst.bigsmall = bigsmall;
                blst.few = int.Parse(few);        //下注該計畫第幾顆數位置
                blst.Period = int.Parse(dt1.Rows[i]["StartPeriod"].ToString()) + int.Parse(dt1.Rows[i]["PlanResultFew"].ToString());
                blst.times = Common.getTime("6B", "xfyt");  //倍數
                blst.type = "6B";   //飛艇六期大小
                blst.BetType = 1;   //正押
                DateTime time = DateTime.Now;
                if (dt5.Rows[i]["StartPeriod"].ToString() == "1")
                {
                    //當期號為第一期時候,限制必須是下午13:00後
                    if (time.Hour >= 13)
                    {
                        lstBet.Add(blst);
                    }
                }
                else
                {
                    lstBet.Add(blst);
                }

            }
            else if (dt1.Rows[i]["PlanResult"].ToString() == "0" && failure > 2)
            {
                //回傳車道名次
                carNo = int.Parse(dt1.Rows[i]["PlanPosition"].ToString()) + 1;
                //闖關中,開出關數少於五關,且前一期失敗=>回傳車號與下注值
                //計畫大小列表
                string[] PlanDataText = dt1.Rows[i]["PlanDataText"].ToString().Split(",".ToCharArray());
                //回傳下注值(大or 小)
                bigsmall = PlanDataText[int.Parse(dt1.Rows[i]["PlanResultFew"].ToString())];

                BetList blst = new BetList();
                blst.PlanId = dt1.Rows[i]["PlanId"].ToString();                     //計畫流水號
                blst.StartPeriod = int.Parse(dt1.Rows[i]["StartPeriod"].ToString());  //起始期數
                blst.carNo = carNo.ToString();
                blst.bigsmall = bigsmall;
                blst.few = int.Parse(few);        //下注該計畫第幾顆數位置
                blst.Period = int.Parse(dt1.Rows[i]["StartPeriod"].ToString()) + int.Parse(dt1.Rows[i]["PlanResultFew"].ToString());
                blst.times = Common.getTime("6B", "xfyt");  //倍數
                blst.type = "6B";   //飛艇六期大小
                blst.BetType = 1;   //正押
                DateTime time = DateTime.Now;
                if (dt1.Rows[i]["StartPeriod"].ToString() == "1")
                {
                    //當期號為第一期時候,限制必須是下午13:00後
                    if (time.Hour >= 13)
                    {
                        lstBet.Add(blst);
                    }
                }
                else
                {
                    lstBet.Add(blst);
                }

            }

            //到推五關全過判斷
            //如果是在前1關就闖關成功,則看前一關
            if (dt2.Rows[i]["PlanResult"].ToString() == "1" && dt2.Rows[i]["PlanResultFew"].ToString() == "1")
            {
                //如果前2關也是成功==>抓出最新一期的闖關號投頭
                if (dt2.Rows[i + 1]["PlanResult"].ToString() == "1" && dt2.Rows[i + 1]["PlanResultFew"].ToString() == "1")
                {
                    //如果前3關也是成功==>抓出最新一期的闖關號投頭
                    if (dt2.Rows[i + 2]["PlanResult"].ToString() == "1" && dt2.Rows[i + 2]["PlanResultFew"].ToString() == "1")
                    {
                        //如果前4關也是成功==>抓出最新一期的闖關號投頭
                        if (dt2.Rows[i + 3]["PlanResult"].ToString() == "1" && dt2.Rows[i + 3]["PlanResultFew"].ToString() == "1")
                        {
                             //如果前5關也是成功==>抓出最新一期的闖關號投頭
                            if (dt2.Rows[i + 4]["PlanResult"].ToString() == "1" && dt2.Rows[i + 4]["PlanResultFew"].ToString() == "1")
                            {
                                int panidNew = int.Parse(dt1.Rows[i]["PlanId"].ToString()); //最新一期計畫期數
                                int panidhis = int.Parse(dt2.Rows[i]["PlanId"].ToString()); //歷史第一期計畫期數
                                //如果前6關也是成功==>抓出最新一期的闖關號投頭
                                if (dt2.Rows[i + 5]["PlanResult"].ToString() == "1" && panidNew > panidhis)
                                {
                                    int period = int.Parse(dt1.Rows[i]["StartPeriod"].ToString()) + int.Parse(dt1.Rows[i]["PlanResultFew"].ToString());
                                    //最新一期必須是尚未開出
                                    if (int.Parse(dt1.Rows[i]["PlanResult"].ToString()) == 0 && int.Parse(dt1.Rows[i]["PlanResultFew"].ToString()) == 0 && Common.check6Period("6B", "xfyt", period) == 1)
                                    {
                                        //回傳車道名次
                                        carNo = int.Parse(dt1.Rows[i]["PlanPosition"].ToString()) + 1;
                                        //計畫大小列表
                                        string[] PlanDataText = dt1.Rows[i]["PlanDataText"].ToString().Split(",".ToCharArray());
                                        //回傳下注值(大or 小)
                                        bigsmall = PlanDataText[int.Parse(dt1.Rows[i]["PlanResultFew"].ToString())];

                                        //反押判斷
                                        if (bigsmall == "大")
                                            bigsmall = "小";
                                        else
                                            bigsmall = "大";

                                        BetList blst = new BetList();
                                        blst.PlanId = dt1.Rows[i]["PlanId"].ToString();                     //計畫流水號
                                        blst.StartPeriod = int.Parse(dt1.Rows[i]["StartPeriod"].ToString());  //起始期數
                                        blst.carNo = carNo.ToString();
                                        blst.bigsmall = bigsmall;
                                        blst.few = int.Parse(dt1.Rows[i]["PlanResultFew"].ToString());        //下注該計畫第幾顆數位置
                                        blst.times = Common.getTime("6B", "xfyt");
                                        blst.Period = int.Parse(dt1.Rows[i]["StartPeriod"].ToString()) + int.Parse(dt1.Rows[i]["PlanResultFew"].ToString());
                                        blst.type = "6B";   //飛艇六期大小
                                        blst.BetType = 0;   //反押
                                        DateTime time = new DateTime();
                                        if (dt1.Rows[i]["StartPeriod"].ToString() == "1")
                                        {
                                            //當期號為第一期時候,限制必須是下午13:00後
                                            if (time.Hour >= 13)
                                            {
                                                lstBet.Add(blst);
                                            }
                                        }
                                        else
                                        {
                                            lstBet.Add(blst);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        //六期單雙
        Plan6D:

            //目前開出的大小單雙
            few = dt3.Rows[i]["PlanResultFew"].ToString();

            DataRow dr3 = dt3.Rows[i];   //最新一期
            DataRow dr4 = dt4.Rows[i];  //歷史計畫最新一期

            //最新歷史計畫與資料庫已存在歷史資料比對
            //if (Common.chkhisData(dt4, "6D", "xfyt") != 1)
            //    goto Plan8B;

            //先檢查history中4B闖關是否過關
            bool oddFLag3 = Common.checkBetData(dr3, dr4, "6D", "xfyt", 0, ref lstBet, ref ndBet);
            Common.CheckOldData(dt4, dr3, "6D", "xfyt", 0, ref lstBet, ref ndBet);

            //計算歷史數據連續失敗的次數
            failure = 0;
            failure = Common.checkfailure(dt4);

            //飛艇六期單雙正押
            if (dt3.Rows[i]["PlanResult"].ToString() == "0" && int.Parse(few) >= 0 && dt4.Rows[0]["PlanResult"].ToString() == "-1")
            {//最新期數闖關中,且大於1期==>抓出歷史數據第一期為失敗 
                //回傳車道名次
                carNo = int.Parse(dt3.Rows[i]["PlanPosition"].ToString()) + 1;
                //闖關中,開出關數少於五關,且前一期失敗=>回傳車號與下注值
                //計畫大小列表
                string[] PlanDataText = dt3.Rows[i]["PlanDataText"].ToString().Split(",".ToCharArray());
                //回傳下注值(大or 小)
                oddEven = PlanDataText[int.Parse(dt3.Rows[i]["PlanResultFew"].ToString())];

                BetList blst = new BetList();
                blst.PlanId = dt3.Rows[i]["PlanId"].ToString();                     //計畫流水號
                blst.StartPeriod = int.Parse(dt3.Rows[i]["StartPeriod"].ToString());  //起始期數
                blst.carNo = carNo.ToString();
                blst.oddEven = oddEven;
                blst.times = Common.getTime("6D", "xfyt");
                blst.few = int.Parse(dt3.Rows[i]["PlanResultFew"].ToString());        //下注該計畫第幾顆數位置
                blst.Period = int.Parse(dt3.Rows[i]["StartPeriod"].ToString()) + int.Parse(dt3.Rows[i]["PlanResultFew"].ToString());
                blst.type = "6D";   //飛艇六期單雙
                blst.BetType = 1;   //正押
                DateTime time = DateTime.Now;
                if (dt3.Rows[i]["StartPeriod"].ToString() == "1")
                {
                    //當期號為第一期時候,限制必須是下午13:00後
                    if (time.Hour >= 13)
                    {
                        lstBet.Add(blst);
                    }
                }
                else
                {
                    lstBet.Add(blst);

                }

            }
            else if (dt3.Rows[i]["PlanResult"].ToString() == "0" && failure > 2)
            {
                //回傳車道名次
                carNo = int.Parse(dt3.Rows[i]["PlanPosition"].ToString()) + 1;
                //闖關中,開出關數少於五關,且前一期失敗=>回傳車號與下注值
                //計畫大小列表
                string[] PlanDataText = dt3.Rows[i]["PlanDataText"].ToString().Split(",".ToCharArray());
                //回傳下注值(大or 小)
                oddEven = PlanDataText[int.Parse(dt3.Rows[i]["PlanResultFew"].ToString())];

                BetList blst = new BetList();
                blst.PlanId = dt3.Rows[i]["PlanId"].ToString();                     //計畫流水號
                blst.StartPeriod = int.Parse(dt3.Rows[i]["StartPeriod"].ToString());  //起始期數
                blst.carNo = carNo.ToString();
                blst.oddEven = oddEven;
                blst.times = Common.getTime("6D", "xfyt");
                blst.few = int.Parse(few);        //下注該計畫第幾顆數位置
                blst.Period = int.Parse(dt3.Rows[i]["StartPeriod"].ToString()) + int.Parse(dt3.Rows[i]["PlanResultFew"].ToString());
                blst.type = "6D";   //飛艇六期單雙
                blst.BetType = 1;   //正押
                DateTime time = DateTime.Now;
                if (dt3.Rows[i]["StartPeriod"].ToString() == "1")
                {
                    //當期號為第一期時候,限制必須是下午13:00後
                    if (time.Hour >= 13)
                    {
                        lstBet.Add(blst);
                    }
                }
                else
                {
                    lstBet.Add(blst);
                }

            }


            //到推五關全過判斷
            //如果是在前1關就闖關成功,則看前一關
            if (dt4.Rows[i]["PlanResult"].ToString() == "1" && dt4.Rows[i]["PlanResultFew"].ToString() == "1")
            {
                //如果前2關也是成功==>抓出最新一期的闖關號投頭
                if (dt4.Rows[i + 1]["PlanResult"].ToString() == "1" && dt4.Rows[i + 1]["PlanResultFew"].ToString() == "1")
                {
                    //如果前3關也是成功==>抓出最新一期的闖關號投頭
                    if (dt4.Rows[i + 2]["PlanResult"].ToString() == "1" && dt4.Rows[i + 2]["PlanResultFew"].ToString() == "1")
                    {
                        //如果前4關也是成功==>抓出最新一期的闖關號投頭
                        if (dt4.Rows[i + 3]["PlanResult"].ToString() == "1" && dt4.Rows[i + 3]["PlanResultFew"].ToString() == "1")
                        {
                               //如果前5關也是成功==>抓出最新一期的闖關號投頭
                            if (dt4.Rows[i + 4]["PlanResult"].ToString() == "1" && dt4.Rows[i + 4]["PlanResultFew"].ToString() == "1")
                            {
                                int panidNew = int.Parse(dt3.Rows[i]["PlanId"].ToString()); //最新一期計畫期數
                                int panidhis = int.Parse(dt4.Rows[i]["PlanId"].ToString()); //歷史第一期計畫期數

                                //如果前6關也是成功==>抓出最新一期的闖關號投頭
                                if (dt4.Rows[i + 5]["PlanResult"].ToString() == "1" && panidNew > panidhis)
                                {
                                    int period = int.Parse(dt3.Rows[i]["StartPeriod"].ToString()) + int.Parse(dt3.Rows[i]["PlanResultFew"].ToString());
                                    //最新一期必須是尚未開出
                                    if (int.Parse(dt3.Rows[i]["PlanResult"].ToString()) == 0 && int.Parse(dt3.Rows[i]["PlanResultFew"].ToString()) == 0 && Common.check6Period("6D", "xfyt", period) == 1)
                                    {
                                        //回傳車道名次
                                        carNo = int.Parse(dt3.Rows[i]["PlanPosition"].ToString()) + 1;
                                        //計畫大小列表
                                        string[] PlanDataText = dt3.Rows[i]["PlanDataText"].ToString().Split(",".ToCharArray());
                                        //回傳下注值(大or 小)
                                        oddEven = PlanDataText[int.Parse(dt3.Rows[i]["PlanResultFew"].ToString())];

                                        //反押
                                        if (oddEven == "单")
                                            oddEven = "双";
                                        else
                                            oddEven = "单";

                                        BetList blst = new BetList();
                                        blst.PlanId = dt3.Rows[i]["PlanId"].ToString();                     //計畫流水號
                                        blst.StartPeriod = int.Parse(dt3.Rows[i]["StartPeriod"].ToString());  //起始期數
                                        blst.carNo = carNo.ToString();
                                        blst.oddEven = oddEven;
                                        blst.times = Common.getTime("6D", "xfyt");
                                        blst.few = int.Parse(dt3.Rows[i]["PlanResultFew"].ToString());        //下注該計畫第幾顆數位置
                                        blst.Period = int.Parse(dt3.Rows[i]["StartPeriod"].ToString()) + int.Parse(dt3.Rows[i]["PlanResultFew"].ToString());
                                        blst.type = "6D";   //飛艇六期單雙
                                        blst.BetType = 0;   //反押
                                        DateTime time = new DateTime();
                                        if (dt3.Rows[i]["StartPeriod"].ToString() == "1")
                                        {
                                            //當期號為第一期時候,限制必須是下午13:00後
                                            if (time.Hour >= 13)
                                            {
                                                lstBet.Add(blst);
                                            }
                                        }
                                        else
                                        {
                                            lstBet.Add(blst);
                                        }
                                    }

                                }
                            }
                        }
                    }
                }
            }
            #endregion

            #region "八期飛艇"
            //飛艇八期判斷
            few = string.Empty;

            i = 0;
        Plan8B:

            //bigSmall正押判斷八期期目前開出的大小
            few = dt5.Rows[i]["PlanResultFew"].ToString();

            DataRow dr5 = dt5.Rows[i];   //最新一期
            DataRow dr6 = dt6.Rows[i];  //歷史計畫最新一期

            //最新歷史計畫與資料庫已存在歷史資料比對
            //if (Common.chkhisData(dt6, "8B", "xfyt") != 1)
            //    goto Plan8D;

            //先檢查history中4B闖關是否過關
            bool oddFLag5 = Common.checkBetData(dr5, dr6, "8B", "xfyt", 0, ref lstBet, ref ndBet);

            Common.CheckOldData(dt6, dr5, "8B", "xfyt", 0, ref lstBet, ref ndBet);


            //計算歷史數據連續失敗的次數
            failure = 0;
            failure = Common.checkfailure(dt6);

            //闖關中,已經開了7關不中==>抓出該車次
            if (dt5.Rows[i]["PlanResult"].ToString() == "0" && int.Parse(few) > 6)
            {
                //回傳車道名次
                carNo = int.Parse(dt5.Rows[i]["PlanPosition"].ToString()) + 1;
                //計畫大小列表
                string[] PlanDataText = dt5.Rows[i]["PlanDataText"].ToString().Split(",".ToCharArray());
                //回傳下注值(大or 小)
                bigsmall = PlanDataText[int.Parse(few)];

                BetList blst = new BetList();
                blst.PlanId = dt5.Rows[i]["PlanId"].ToString();                     //計畫流水號
                blst.StartPeriod = int.Parse(dt5.Rows[i]["StartPeriod"].ToString());  //起始期數
                blst.carNo = carNo.ToString();
                blst.bigsmall = bigsmall;
                blst.times = Common.getTime("8B", "xfyt");
                blst.Period = int.Parse(dt5.Rows[i]["StartPeriod"].ToString()) + int.Parse(dt5.Rows[i]["PlanResultFew"].ToString());
                blst.type = "8B";   //飛艇八期大小
                blst.BetType = 1;   //正押
                blst.few = int.Parse(dt5.Rows[i]["PlanResultFew"].ToString());        //下注該計畫第幾顆數位置
                DateTime time = DateTime.Now;
                if (dt5.Rows[i]["StartPeriod"].ToString() == "1")
                {
                    //當期號為第一期時候,限制必須是下午13:00後
                    if (time.Hour >= 13)
                    {
                        lstBet.Add(blst);
                    }
                }
                else
                {
                    lstBet.Add(blst);
                }
            }
            else if (dt5.Rows[i]["PlanResult"].ToString() == "0" && dt6.Rows[0]["PlanResult"].ToString() == "-1")
            {     //抓出歷史數據第一期為失敗 , 且最數闖關牌小於7期

                //回傳車道名次
                carNo = int.Parse(dt5.Rows[i]["PlanPosition"].ToString()) + 1;
                //闖關中,開出關數少於五關,且前一期失敗=>回傳車號與下注值
                //計畫大小列表
                string[] PlanDataText = dt5.Rows[i]["PlanDataText"].ToString().Split(",".ToCharArray());
                //回傳下注值(大or 小)
                bigsmall = PlanDataText[int.Parse(few)];

                BetList blst = new BetList();
                blst.PlanId = dt5.Rows[i]["PlanId"].ToString();                     //計畫流水號
                blst.StartPeriod = int.Parse(dt5.Rows[i]["StartPeriod"].ToString());  //起始期數
                blst.carNo = carNo.ToString();
                blst.bigsmall = bigsmall;
                blst.times = Common.getTime("8B", "xfyt");
                blst.Period = int.Parse(dt5.Rows[i]["StartPeriod"].ToString()) + int.Parse(dt5.Rows[i]["PlanResultFew"].ToString());
                blst.type = "8B";   //飛艇八期大小
                blst.BetType = 1;   //正押
                blst.few = int.Parse(dt5.Rows[i]["PlanResultFew"].ToString());        //下注該計畫第幾顆數位置
                DateTime time = DateTime.Now;
                if (dt5.Rows[i]["StartPeriod"].ToString() == "1")
                {
                    //當期號為第一期時候,限制必須是下午13:00後
                    if (time.Hour >= 13)
                    {
                        lstBet.Add(blst);
                    }
                }
                else
                {
                    lstBet.Add(blst);
                }
            }
            else if (dt5.Rows[i]["PlanResult"].ToString() == "0" && failure > 2)
            {
                //回傳車道名次
                carNo = int.Parse(dt5.Rows[i]["PlanPosition"].ToString()) + 1;
                //闖關中,開出關數少於五關,且前一期失敗=>回傳車號與下注值
                //計畫大小列表
                string[] PlanDataText = dt5.Rows[i]["PlanDataText"].ToString().Split(",".ToCharArray());
                //回傳下注值(大or 小)
                bigsmall = PlanDataText[int.Parse(dt5.Rows[i]["PlanResultFew"].ToString())];

                BetList blst = new BetList();
                blst.PlanId = dt5.Rows[i]["PlanId"].ToString();                     //計畫流水號
                blst.StartPeriod = int.Parse(dt5.Rows[i]["StartPeriod"].ToString());  //起始期數
                blst.carNo = carNo.ToString();
                blst.bigsmall = bigsmall;
                blst.times = Common.getTime("8B", "xfyt");
                blst.Period = int.Parse(dt5.Rows[i]["StartPeriod"].ToString()) + int.Parse(dt5.Rows[i]["PlanResultFew"].ToString());
                blst.type = "8B";   //飛艇八期大小
                blst.few = int.Parse(dt5.Rows[i]["PlanResultFew"].ToString());        //下注該計畫第幾顆數位置
                DateTime time = DateTime.Now;
                blst.BetType = 1;   //正押
                if (dt5.Rows[i]["StartPeriod"].ToString() == "1")
                {
                    //當期號為第一期時候,限制必須是下午13:00後
                    if (time.Hour >= 13)
                    {
                        lstBet.Add(blst);
                    }
                }
                else
                {
                    lstBet.Add(blst);
                }
            }


            //到推五關全過判斷
            //如果是在前1關就闖關成功,則看前一關
            if (dt6.Rows[i]["PlanResult"].ToString() == "1" && dt6.Rows[i]["PlanResultFew"].ToString() == "1")
            {
                //如果前2關也是成功==>抓出最新一期的闖關號投頭
                if (dt6.Rows[i + 1]["PlanResult"].ToString() == "1" && dt6.Rows[i + 1]["PlanResultFew"].ToString() == "1")
                {
                    //如果前3關也是成功==>抓出最新一期的闖關號投頭
                    if (dt6.Rows[i + 2]["PlanResult"].ToString() == "1" && dt6.Rows[i + 2]["PlanResultFew"].ToString() == "1")
                    {
                        //如果前4關也是成功==>抓出最新一期的闖關號投頭
                        if (dt6.Rows[i + 3]["PlanResult"].ToString() == "1" && dt6.Rows[i + 3]["PlanResultFew"].ToString() == "1")
                        {
                            //如果前5關也是成功==>抓出最新一期的闖關號投頭
                            if (dt6.Rows[i + 4]["PlanResult"].ToString() == "1" && dt6.Rows[i + 4]["PlanResultFew"].ToString() == "1")
                            {
                                int panidNew = int.Parse(dt5.Rows[i]["PlanId"].ToString()); //最新一期計畫期數
                                int panidhis = int.Parse(dt6.Rows[i]["PlanId"].ToString()); //歷史第一期計畫期數
                                //如果前6關也是成功==>抓出最新一期的闖關號投頭
                                if (dt6.Rows[i + 5]["PlanResult"].ToString() == "1" && panidNew > panidhis)
                                {
                                    int period = int.Parse(dt5.Rows[i]["StartPeriod"].ToString()) + int.Parse(dt5.Rows[i]["PlanResultFew"].ToString());
                                    //最新一期必須是尚未開出
                                    if (int.Parse(dt5.Rows[i]["PlanResult"].ToString()) == 0 && int.Parse(dt5.Rows[i]["PlanResultFew"].ToString()) == 0 && Common.check6Period("8B", "xfyt", period) == 1)
                                    {
                                        //回傳車道名次
                                        carNo = int.Parse(dt5.Rows[i]["PlanPosition"].ToString()) + 1;
                                        //計畫大小列表
                                        string[] PlanDataText = dt5.Rows[i]["PlanDataText"].ToString().Split(",".ToCharArray());
                                        //回傳下注值(大or 小)
                                        bigsmall = PlanDataText[int.Parse(dt5.Rows[i]["PlanResultFew"].ToString())];

                                        //反押判斷
                                        if (bigsmall == "大")
                                            bigsmall = "小";
                                        else
                                            bigsmall = "大";

                                        BetList blst = new BetList();
                                        blst.PlanId = dt5.Rows[i]["PlanId"].ToString();                     //計畫流水號
                                        blst.StartPeriod = int.Parse(dt5.Rows[i]["StartPeriod"].ToString());  //起始期數
                                        blst.carNo = carNo.ToString();
                                        blst.bigsmall = bigsmall;
                                        blst.times = Common.getTime("8B", "xfyt");
                                        blst.Period = int.Parse(dt5.Rows[i]["StartPeriod"].ToString()) + int.Parse(dt5.Rows[i]["PlanResultFew"].ToString());
                                        blst.type = "8B";   //飛艇八期大小
                                        blst.BetType = 0;   //反押
                                        blst.few = int.Parse(dt5.Rows[i]["PlanResultFew"].ToString());        //下注該計畫第幾顆數位置
                                        DateTime time = DateTime.Now;
                                        if (dt5.Rows[i]["StartPeriod"].ToString() == "1")
                                        {
                                            //當期號為第一期時候,限制必須是下午13:00後
                                            if (time.Hour >= 13)
                                            {
                                                lstBet.Add(blst);
                                            }
                                        }
                                        else
                                        {
                                            lstBet.Add(blst);
                                        }

                                    }
                                }
                            }
                        }
                    }
                }
            }

        //八期單雙
        Plan8D:

            oddEven = string.Empty;

            ///判斷八期單雙目前開出的大小單雙
            few = dt7.Rows[i]["PlanResultFew"].ToString();

            DataRow dr7 = dt7.Rows[i];   //最新一期
            DataRow dr8 = dt8.Rows[i];  //歷史計畫最新一期

            //最新歷史計畫與資料庫已存在歷史資料比對
            //if (Common.chkhisData(dt8, "8D", "xfyt") != 1)
            //    goto Plan4B;

            //先檢查history中4B闖關是否過關
            bool oddFLags = Common.checkBetData(dr7, dr8, "8D", "xfyt", 0, ref lstBet, ref ndBet);
            Common.CheckOldData(dt8, dr7, "8D", "xfyt", 0, ref lstBet, ref ndBet);

            //計算歷史數據連續失敗的次數
            failure = 0;
            failure = Common.checkfailure(dt8);

            //oddEven正押判斷
            //闖關中,已經開了七關不中==>抓出該車次,開始正押
            if (dt7.Rows[i]["PlanResult"].ToString() == "0" && int.Parse(few) > 6)
            {
                //回傳車道名次
                carNo = int.Parse(dt7.Rows[i]["PlanPosition"].ToString()) + 1;
                //計畫大小列表
                string[] PlanDataText = dt7.Rows[i]["PlanDataText"].ToString().Split(",".ToCharArray());
                //回傳下注值(大or 小)
                oddEven = PlanDataText[int.Parse(few)];

                BetList blst = new BetList();
                blst.PlanId = dt7.Rows[i]["PlanId"].ToString();                     //計畫流水號
                blst.StartPeriod = int.Parse(dt7.Rows[i]["StartPeriod"].ToString());  //起始期數
                blst.carNo = carNo.ToString();
                blst.oddEven = oddEven;
                blst.times = Common.getTime("8D", "xfyt");
                blst.Period = int.Parse(dt7.Rows[i]["StartPeriod"].ToString()) + int.Parse(dt7.Rows[i]["PlanResultFew"].ToString());
                blst.type = "8D";   //飛艇八期單雙
                blst.BetType = 1;   //正押
                blst.few = int.Parse(dt7.Rows[i]["PlanResultFew"].ToString());        //下注該計畫第幾顆數位置
                DateTime time = DateTime.Now;
             
                if (dt7.Rows[i]["StartPeriod"].ToString() == "1")
                {
                    //當期號為第一期時候,限制必須是下午13:00後
                    if (time.Hour >= 13)
                    {
                        lstBet.Add(blst);
                    }
                }
                else
                {
                    lstBet.Add(blst);
                }
            }
            else if (dt7.Rows[i]["PlanResult"].ToString() == "0" && dt8.Rows[0]["PlanResult"].ToString() == "-1")
            {//最新期數闖關中,且小於5期==>抓出歷史數據第一期為失敗 
                //回傳車道名次
                carNo = int.Parse(dt7.Rows[i]["PlanPosition"].ToString()) + 1;
                //闖關中,開出關數少於五關,且前一期失敗=>回傳車號與下注值
                //計畫大小列表
                string[] PlanDataText = dt7.Rows[i]["PlanDataText"].ToString().Split(",".ToCharArray());
                //回傳下注值(大or 小)
                oddEven = PlanDataText[int.Parse(dt7.Rows[i]["PlanResultFew"].ToString())];


                BetList blst = new BetList();
                blst.PlanId = dt7.Rows[i]["PlanId"].ToString();                     //計畫流水號
                blst.StartPeriod = int.Parse(dt7.Rows[i]["StartPeriod"].ToString());  //起始期數
                blst.carNo = carNo.ToString();
                blst.oddEven = oddEven;
                blst.times = Common.getTime("8D", "xfyt");
                blst.Period = int.Parse(dt7.Rows[i]["StartPeriod"].ToString()) + int.Parse(dt7.Rows[i]["PlanResultFew"].ToString());
                blst.type = "8D";   //飛艇八期單雙
                blst.BetType = 1;   //正押
                blst.few = int.Parse(dt7.Rows[i]["PlanResultFew"].ToString());        //下注該計畫第幾顆數位置
                DateTime time = DateTime.Now;
                if (int.Parse(dt7.Rows[i]["StartPeriod"].ToString()) == 1 )
                {
                    //當期號為第一期時候,限制必須是下午13:00後
                    if (time.Hour >= 13)
                    {
                        lstBet.Add(blst);
                    }
                }
                else
                {
                    lstBet.Add(blst);
                }
            }
            else if (dt7.Rows[i]["PlanResult"].ToString() == "0" && failure > 2)
            {
                //回傳車道名次
                carNo = int.Parse(dt7.Rows[i]["PlanPosition"].ToString()) + 1;
                //闖關中,開出關數少於五關,且前一期失敗=>回傳車號與下注值
                //計畫大小列表
                string[] PlanDataText = dt7.Rows[i]["PlanDataText"].ToString().Split(",".ToCharArray());
                //回傳下注值(大or 小)
                oddEven = PlanDataText[int.Parse(few)];


                BetList blst = new BetList();
                blst.PlanId = dt7.Rows[i]["PlanId"].ToString();                     //計畫流水號
                blst.StartPeriod = int.Parse(dt7.Rows[i]["StartPeriod"].ToString());  //起始期數
                blst.carNo = carNo.ToString();
                blst.oddEven = oddEven;
                blst.times = Common.getTime("8D", "xfyt");
                blst.Period = int.Parse(dt7.Rows[i]["StartPeriod"].ToString()) + int.Parse(dt7.Rows[i]["PlanResultFew"].ToString());
                blst.type = "8D";   //飛艇八期單雙
                blst.BetType = 1;   //正押
                blst.few = int.Parse(dt7.Rows[i]["PlanResultFew"].ToString());        //下注該計畫第幾顆數位置
                DateTime time = DateTime.Now;
                if (dt7.Rows[i]["StartPeriod"].ToString() == "1")
                {
                    //當期號為第一期時候,限制必須是下午13:00後
                    if (time.Hour >= 13)
                    {
                        lstBet.Add(blst);
                    }
                }
                else
                {
                    lstBet.Add(blst);
                }

            }

            //反押判斷
            //到推五關全過判斷
            //如果是在前1關就闖關成功,則看前一關
            if (dt8.Rows[i]["PlanResult"].ToString() == "1" && dt8.Rows[i]["PlanResultFew"].ToString() == "1")
            {
                //如果前2關也是成功==>抓出最新一期的闖關號投頭
                if (dt8.Rows[i + 1]["PlanResult"].ToString() == "1" && dt8.Rows[i + 1]["PlanResultFew"].ToString() == "1")
                {
                    //如果前3關也是成功==>抓出最新一期的闖關號投頭
                    if (dt8.Rows[i + 2]["PlanResult"].ToString() == "1" && dt8.Rows[i + 2]["PlanResultFew"].ToString() == "1")
                    {
                         //如果前4關也是成功==>抓出最新一期的闖關號投頭
                        if (dt8.Rows[i + 3]["PlanResult"].ToString() == "1" && dt8.Rows[i + 3]["PlanResultFew"].ToString() == "1")
                        {
                           //如果前5關也是成功==>抓出最新一期的闖關號投頭
                            if (dt8.Rows[i + 4]["PlanResult"].ToString() == "1" && dt8.Rows[i + 4]["PlanResultFew"].ToString() == "1")
                            {
                                int panidNew = int.Parse(dt7.Rows[i]["PlanId"].ToString()); //最新一期計畫期數
                                int panidhis = int.Parse(dt8.Rows[i]["PlanId"].ToString()); //歷史第一期計畫期數
                                //如果前6關也是成功==>抓出最新一期的闖關號投頭
                                if (dt8.Rows[i + 5]["PlanResult"].ToString() == "1" && panidNew > panidhis)
                                {

                                    int period = int.Parse(dt7.Rows[i]["StartPeriod"].ToString()) + int.Parse(dt7.Rows[i]["PlanResultFew"].ToString());
                                    //最新一期必須是尚未開出
                                    if (int.Parse(dt7.Rows[i]["PlanResult"].ToString()) == 0 && int.Parse(dt7.Rows[i]["PlanResultFew"].ToString()) == 0 && Common.check6Period("8D", "xfyt", period) == 1)
                                    {
                                        //回傳車道名次
                                        carNo = int.Parse(dt7.Rows[i]["PlanPosition"].ToString()) + 1;
                                        //計畫大小列表
                                        string[] PlanDataText = dt7.Rows[i]["PlanDataText"].ToString().Split(",".ToCharArray());
                                        //回傳下注值(大or 小)
                                        oddEven = PlanDataText[int.Parse(dt7.Rows[i]["PlanResultFew"].ToString())];

                                        //反押
                                        if (oddEven == "单")
                                            oddEven = "双";
                                        else
                                            oddEven = "单";


                                        BetList blst = new BetList();
                                        blst.PlanId = dt7.Rows[i]["PlanId"].ToString();                     //計畫流水號
                                        blst.StartPeriod = int.Parse(dt7.Rows[i]["StartPeriod"].ToString());  //起始期數
                                        blst.carNo = carNo.ToString();
                                        blst.oddEven = oddEven;
                                        blst.times = Common.getTime("8D", "xfyt");
                                        blst.Period = int.Parse(dt7.Rows[i]["StartPeriod"].ToString()) + int.Parse(dt7.Rows[i]["PlanResultFew"].ToString());
                                        blst.type = "8D";   //飛艇八期單雙
                                        blst.few = int.Parse(dt7.Rows[i]["PlanResultFew"].ToString());        //下注該計畫第幾顆數位置
                                        blst.BetType = 0;   //反押
                                        DateTime time = DateTime.Now;
                                        if (dt7.Rows[i]["StartPeriod"].ToString() == "1")
                                        {
                                            //當期號為第一期時候,限制必須是下午13:00後
                                            if (time.Hour >= 13)
                                            {
                                                lstBet.Add(blst);
                                            }
                                        }
                                        else
                                        {
                                            lstBet.Add(blst);
                                        }


                                    }

                                }
                            }

                        }

                    }

                }
            }

            #endregion

            #region "四期飛艇"
        //飛艇4期大小判斷
        Plan4B:
            few = string.Empty;

            i = 0;
            //bigSmall正押判斷
            few = dt9.Rows[i]["PlanResultFew"].ToString();

            DataRow dr9 = dt9.Rows[i];   //最新一期
            DataRow dr10 = dt10.Rows[i];  //歷史計畫最新一期

            //最新歷史計畫與資料庫已存在歷史資料比對
            //if (Common.chkhisData(dt10, "4B", "xfyt") != 1)
            //    goto Plan4D;

            //先檢查history中4B闖關是否過關
            bool flags = Common.checkBetData(dr9, dr10, "4B", "xfyt", 0, ref lstBet, ref ndBet);
            Common.CheckOldData(dt10, dr9, "4B", "xfyt", 0, ref lstBet, ref ndBet);

            //計算歷史數據連續失敗的次數
            failure = 0;
            failure = Common.checkfailure(dt10);



            if (dt9.Rows[i]["PlanResult"].ToString() == "0" && dt10.Rows[0]["PlanResult"].ToString() == "-1" && int.Parse(dt9.Rows[i]["PlanResultFew"].ToString()) > 2)
            {     //最新期數闖關中,few開出三顆==>抓出歷史數據第一期為失敗==>開始正押 

                //回傳車道名次
                carNo = int.Parse(dt9.Rows[i]["PlanPosition"].ToString()) + 1;
                //闖關中,開出關數少於五關,且前一期失敗=>回傳車號與下注值
                //計畫大小列表
                string[] PlanDataText = dt9.Rows[i]["PlanDataText"].ToString().Split(",".ToCharArray());
                //回傳下注值(大or 小)
                bigsmall = PlanDataText[int.Parse(dt9.Rows[i]["PlanResultFew"].ToString())];


                BetList blst = new BetList();
                blst.PlanId = dt9.Rows[i]["PlanId"].ToString();                     //計畫流水號
                blst.StartPeriod = int.Parse(dt9.Rows[i]["StartPeriod"].ToString());  //起始期數
                blst.carNo = carNo.ToString();
                blst.bigsmall = bigsmall;
                blst.times = Common.getTime("4B", "xfyt");
                blst.Period = int.Parse(dt9.Rows[i]["StartPeriod"].ToString()) + int.Parse(dt9.Rows[i]["PlanResultFew"].ToString());
                blst.type = "4B";   //飛艇四期大小
                blst.BetType = 1;   //正押
                blst.few = int.Parse(dt9.Rows[i]["PlanResultFew"].ToString());        //下注該計畫第幾顆數位置
                DateTime time = DateTime.Now;
                if (dt9.Rows[i]["StartPeriod"].ToString() == "1")
                {
                    //當期號為第一期時候,限制必須是下午13:00後
                    if (time.Hour >= 13)
                    {
                        lstBet.Add(blst);
                    }
                }
                else
                {
                    lstBet.Add(blst);
                }
            }
            else if (dt9.Rows[i]["PlanResult"].ToString() == "0" && failure >= 2)
            {
                //回傳車道名次
                carNo = int.Parse(dt9.Rows[i]["PlanPosition"].ToString()) + 1;
                //闖關中,開出關數少於五關,且前一期失敗=>回傳車號與下注值
                //計畫大小列表
                string[] PlanDataText = dt9.Rows[i]["PlanDataText"].ToString().Split(",".ToCharArray());
                //回傳下注值(大or 小)
                bigsmall = PlanDataText[int.Parse(few)];


                BetList blst = new BetList();
                blst.PlanId = dt9.Rows[i]["PlanId"].ToString();                     //計畫流水號
                blst.StartPeriod = int.Parse(dt9.Rows[i]["StartPeriod"].ToString());  //起始期數
                blst.carNo = carNo.ToString();
                blst.bigsmall = bigsmall;
                blst.times = Common.getTime("4B", "xfyt");
                blst.Period = int.Parse(dt9.Rows[i]["StartPeriod"].ToString()) + int.Parse(dt9.Rows[i]["PlanResultFew"].ToString());
                blst.type = "4B";   //飛艇四期大小
                blst.BetType = 1;   //正押
                blst.few = int.Parse(dt9.Rows[i]["PlanResultFew"].ToString());        //下注該計畫第幾顆數位置
                DateTime time = DateTime.Now;
                if (dt9.Rows[i]["StartPeriod"].ToString() == "1")
                {
                    //當期號為第一期時候,限制必須是下午13:00後
                    if (time.Hour >= 13)
                    {
                        lstBet.Add(blst);
                    }
                }
                else
                {
                    lstBet.Add(blst);
                }
            }



            //反押推五關全過判斷
            //如果是在前1關就闖關成功,則看前一關
            if (dt10.Rows[i]["PlanResult"].ToString() == "1" && dt10.Rows[i]["PlanResultFew"].ToString() == "1")
            {
                //如果前2關也是成功==>抓出最新一期的闖關號投頭
                if (dt10.Rows[i + 1]["PlanResult"].ToString() == "1" && dt10.Rows[i + 1]["PlanResultFew"].ToString() == "1")
                {
                    //如果前3關也是成功==>抓出最新一期的闖關號投頭
                    if (dt10.Rows[i + 2]["PlanResult"].ToString() == "1" && dt10.Rows[i + 2]["PlanResultFew"].ToString() == "1")
                    {
                        //如果前4關也是成功==>抓出最新一期的闖關號投頭
                        if (dt10.Rows[i + 3]["PlanResult"].ToString() == "1" && dt10.Rows[i + 3]["PlanResultFew"].ToString() == "1")
                        {
                             //如果前5關也是成功==>抓出最新一期的闖關號投頭
                            if (dt10.Rows[i + 4]["PlanResult"].ToString() == "1" && dt10.Rows[i + 4]["PlanResultFew"].ToString() == "1")
                            {
                                //如果前6關也是成功==>抓出最新一期的闖關號投頭
                                if (dt10.Rows[i + 5]["PlanResult"].ToString() == "1")
                                {
                                    int panidNew = int.Parse(dt9.Rows[i]["PlanId"].ToString()); //最新一期計畫期數
                                    int panidhis = int.Parse(dt10.Rows[i]["PlanId"].ToString()); //歷史第一期計畫期數

                                    int period = int.Parse(dt9.Rows[i]["StartPeriod"].ToString()) + int.Parse(dt9.Rows[i]["PlanResultFew"].ToString());

                                    //最新一期必須是尚未開出
                                    if (int.Parse(dt9.Rows[i]["PlanResult"].ToString()) == 0 && panidNew > panidhis && int.Parse(dt9.Rows[i]["PlanResultFew"].ToString()) == 0 && Common.check6Period("4B", "xfyt", period) == 1)
                                    {
                                        //回傳車道名次
                                        carNo = int.Parse(dt9.Rows[i]["PlanPosition"].ToString()) + 1;
                                        //計畫大小列表
                                        string[] PlanDataText = dt9.Rows[i]["PlanDataText"].ToString().Split(",".ToCharArray());
                                        //回傳下注值(大or 小)
                                        bigsmall = PlanDataText[int.Parse(dt9.Rows[i]["PlanResultFew"].ToString())];

                                        //反押判斷
                                        if (bigsmall == "大")
                                            bigsmall = "小";
                                        else
                                            bigsmall = "大";


                                        BetList blst = new BetList();
                                        blst.PlanId = dt9.Rows[i]["PlanId"].ToString();                     //計畫流水號
                                        blst.StartPeriod = int.Parse(dt9.Rows[i]["StartPeriod"].ToString());  //起始期數
                                        blst.carNo = carNo.ToString();
                                        blst.bigsmall = bigsmall;
                                        blst.times = Common.getTime("4B", "xfyt");
                                        blst.Period = int.Parse(dt9.Rows[i]["StartPeriod"].ToString()) + int.Parse(dt9.Rows[i]["PlanResultFew"].ToString());
                                        blst.type = "4B";   //飛艇四期大小
                                        blst.BetType = 0;   //反押
                                        blst.few = int.Parse(dt9.Rows[i]["PlanResultFew"].ToString());        //下注該計畫第幾顆數位置

                                        DateTime time = DateTime.Now;
                                        if (dt9.Rows[i]["StartPeriod"].ToString() == "1")
                                        {
                                            //當期號為第一期時候,限制必須是下午13:00後
                                            if (time.Hour >= 13)
                                            {
                                                lstBet.Add(blst);
                                            }
                                        }
                                        else
                                        {
                                            lstBet.Add(blst);
                                        }
                                    }

                                }
                            }
                        }

                    }

                }
            }

            //四期單雙判斷
        Plan4D:
            oddEven = string.Empty;
            i = 0;
            //目前開出的大小單雙
            few = dt11.Rows[i]["PlanResultFew"].ToString();

            DataRow dr11 = dt11.Rows[i];   //最新一期
            DataRow dr12 = dt12.Rows[i];  //歷史計畫最新一期

            //最新歷史計畫與資料庫已存在歷史資料比對==>如果比對無吻合歷史資料=>跳入下一個工作
            //if (Common.chkhisData(dt12, "4D", "xfyt") != 1)
            //    goto ShowMess;

            //先檢查history中4B闖關是否過關
            bool oddFLag6 = Common.checkBetData(dr11, dr12, "4D", "xfyt", 0, ref lstBet, ref ndBet);
            Common.CheckOldData(dt12, dr11, "4D", "xfyt", 0, ref lstBet, ref ndBet);

            //計算歷史數據連續失敗的次數
            failure = 0;
            failure = Common.checkfailure(dt12);
            //闖關中,已經開了五關不中==>抓出該車次

            if (dt11.Rows[i]["PlanResult"].ToString() == "0" && dt12.Rows[0]["PlanResult"].ToString() == "-1" && int.Parse(dt11.Rows[i]["PlanResultFew"].ToString()) > 2)
            {//最新期數闖關中,且小於5期==>抓出歷史數據第一期為失敗 
                //回傳車道名次
                carNo = int.Parse(dt11.Rows[i]["PlanPosition"].ToString()) + 1;
                //闖關中,開出關數少於五關,且前一期失敗=>回傳車號與下注值
                //計畫大小列表
                string[] PlanDataText = dt11.Rows[i]["PlanDataText"].ToString().Split(",".ToCharArray());
                //回傳下注值(大or 小)
                oddEven = PlanDataText[int.Parse(dt11.Rows[i]["PlanResultFew"].ToString())];


                BetList blst = new BetList();
                blst.PlanId = dt11.Rows[i]["PlanId"].ToString();                     //計畫流水號
                blst.StartPeriod = int.Parse(dt11.Rows[i]["StartPeriod"].ToString());  //起始期數
                blst.carNo = carNo.ToString();
                blst.oddEven = oddEven;
                blst.times = Common.getTime("4D", "xfyt");
                blst.Period = int.Parse(dt11.Rows[i]["StartPeriod"].ToString()) + int.Parse(dt11.Rows[i]["PlanResultFew"].ToString());
                blst.type = "4D";   //飛艇四期單雙
                blst.BetType = 1;   //正押
                blst.few = int.Parse(dt11.Rows[i]["PlanResultFew"].ToString());        //下注該計畫第幾顆數位置
                DateTime time = DateTime.Now;
                if (dt11.Rows[i]["StartPeriod"].ToString() == "1")
                {
                    //當期號為第一期時候,限制必須是下午13:00後
                    if (time.Hour >= 13)
                    {
                        lstBet.Add(blst);
                    }
                }
                else
                {
                    lstBet.Add(blst);
                }
            }
            else if (dt11.Rows[i]["PlanResult"].ToString() == "0" && failure >= 2)
            {
                //回傳車道名次
                carNo = int.Parse(dt11.Rows[i]["PlanPosition"].ToString()) + 1;
                //闖關中,開出關數少於五關,且前一期失敗=>回傳車號與下注值
                //計畫大小列表
                string[] PlanDataText = dt11.Rows[i]["PlanDataText"].ToString().Split(",".ToCharArray());
                //回傳下注值(大or 小)
                oddEven = PlanDataText[int.Parse(dt11.Rows[i]["PlanResultFew"].ToString())];


                BetList blst = new BetList();
                blst.PlanId = dt11.Rows[i]["PlanId"].ToString();                     //計畫流水號
                blst.StartPeriod = int.Parse(dt11.Rows[i]["StartPeriod"].ToString());  //起始期數
                blst.carNo = carNo.ToString();
                blst.oddEven = oddEven;
                blst.times = Common.getTime("4D", "xfyt");
                blst.Period = int.Parse(dt11.Rows[i]["StartPeriod"].ToString()) + int.Parse(dt11.Rows[i]["PlanResultFew"].ToString());
                blst.type = "4D";   //飛艇四期單雙
                blst.BetType = 1;   //正押
                blst.few = int.Parse(dt11.Rows[i]["PlanResultFew"].ToString());        //下注該計畫第幾顆數位置
                DateTime time = DateTime.Now;
                if (dt11.Rows[i]["StartPeriod"].ToString() == "1")
                {
                    //當期號為第一期時候,限制必須是下午13:00後
                    if (time.Hour >= 13)
                    {
                        lstBet.Add(blst);
                    }
                }
                else
                {
                    lstBet.Add(blst);
                }
            }

            //反押判斷
            //抓出前五關全過,開始反押,如果前五關全不中=>開始正押
            //如果是在前1關就闖關成功,則看前一關
            if (dt12.Rows[i]["PlanResult"].ToString() == "1" && dt12.Rows[i]["PlanResultFew"].ToString() == "1")
            {
                //如果前2關也是成功==>抓出最新一期的闖關號投頭
                if (dt12.Rows[i + 1]["PlanResult"].ToString() == "1" && dt12.Rows[i + 1]["PlanResultFew"].ToString() == "1")
                {
                    //如果前3關也是成功==>抓出最新一期的闖關號投頭
                    if (dt12.Rows[i + 2]["PlanResult"].ToString() == "1" && dt12.Rows[i + 2]["PlanResultFew"].ToString() == "1")
                    {
                        //如果前4關也是成功==>抓出最新一期的闖關號投頭
                        if (dt12.Rows[i + 3]["PlanResult"].ToString() == "1" && dt12.Rows[i + 3]["PlanResultFew"].ToString() == "1")
                        {
                            //如果前5關也是成功==>抓出最新一期的闖關號投頭
                            if (dt12.Rows[i + 4]["PlanResult"].ToString() == "1" && dt12.Rows[i + 4]["PlanResultFew"].ToString() == "1")
                            {
                                int panidNew = int.Parse(dt11.Rows[i]["PlanId"].ToString()); //最新一期計畫期數
                                int panidhis = int.Parse(dt12.Rows[i]["PlanId"].ToString()); //歷史第一期計畫期數
                                //如果前6關也是成功==>抓出最新一期的闖關號投頭
                                if (dt12.Rows[i + 5]["PlanResult"].ToString() == "1" && panidNew > panidhis)
                                {
                                    int period = int.Parse(dt11.Rows[i]["StartPeriod"].ToString()) + int.Parse(dt11.Rows[i]["PlanResultFew"].ToString());
                                    //最新一期必須是尚未開出
                                    if (int.Parse(dt11.Rows[i]["PlanResult"].ToString()) == 0 && int.Parse(dt11.Rows[i]["PlanResultFew"].ToString()) == 0 && Common.check6Period("4D", "xfyt", period) == 1)
                                    {
                                        //回傳車道名次
                                        carNo = int.Parse(dt11.Rows[i]["PlanPosition"].ToString()) + 1;
                                        //計畫大小列表
                                        string[] PlanDataText = dt11.Rows[i]["PlanDataText"].ToString().Split(",".ToCharArray());
                                        //回傳下注值(大or 小)
                                        oddEven = PlanDataText[int.Parse(dt11.Rows[i]["PlanResultFew"].ToString())];

                                        //反押
                                        if (oddEven == "单")
                                            oddEven = "双";
                                        else
                                            oddEven = "单";


                                        BetList blst = new BetList();
                                        blst.PlanId = dt11.Rows[i]["PlanId"].ToString();                     //計畫流水號
                                        blst.StartPeriod = int.Parse(dt11.Rows[i]["StartPeriod"].ToString());  //起始期數
                                        blst.carNo = carNo.ToString();
                                        blst.oddEven = oddEven;
                                        blst.status = "0";   //闖關中
                                        blst.times = Common.getTime("4D", "xfyt");
                                        blst.Period = int.Parse(dt11.Rows[i]["StartPeriod"].ToString()) + int.Parse(dt11.Rows[i]["PlanResultFew"].ToString());
                                        blst.type = "4D";   //飛艇四期單雙
                                        blst.BetType = 0;   //反押
                                        blst.few = int.Parse(dt11.Rows[i]["PlanResultFew"].ToString());        //下注該計畫第幾顆數位置
                                        DateTime time = DateTime.Now;
                                        if (dt11.Rows[i]["StartPeriod"].ToString() == "1")
                                        {
                                            //當期號為第一期時候,限制必須是下午13:00後
                                            if (time.Hour >= 13)
                                            {
                                                lstBet.Add(blst);
                                            }
                                        }
                                        else
                                        {
                                            lstBet.Add(blst);
                                        }

                                    }

                                }
                            }
                        }
                    }
                }
            }

            #endregion
        ShowMess:
            //已過關的牌注
            string Notes = string.Empty;

            if (ndBet.Count != 0)
            {
                // Notes = "幸運飛艇" + Environment.NewLine;
                foreach (BetList bl in ndBet)
                {
                    if (Notes.IndexOf("期數:") == -1)
                        Notes += "期數:" + bl.Period + Environment.NewLine;
                    Notes += "  " + bl.carNo;
                    if (!string.IsNullOrEmpty(bl.bigsmall))
                        Notes += "  " + bl.bigsmall;
                    if (!string.IsNullOrEmpty(bl.oddEven))
                        Notes += "  " + bl.oddEven;

                    Notes += " " + bl.times + "倍";

                    if (bl.times <= 1023 && bl.status == "1")
                        Notes += " 過關" + Environment.NewLine;
                    else
                        Notes += " 爆關" + Environment.NewLine;

                }

                if (!string.IsNullOrEmpty(Notes))
                    Notes = "幸運飛艇" + Environment.NewLine + Notes;

                if (!string.IsNullOrEmpty(Notes))
                    //isRock.LineBot.Utility.PushMessage("Cbaa0920c644078cd1f3f98322b11b91c", Notes, "gpkB663L+H+xduic75ygNa/FYFeGjzfyzvcZSzj1X8KqzyTnldKHceqOlGnIDhMmiU4eienvd49L25fqNsqi13qurEEFxZJnQTxBI9HlQmaULztlmx5nQO4eB7iSgQPQWoPZvRATzHmO9dWWkw5gtwdB04t89/1O/w1cDnyilFU=");
                    //isRock.LineBot.Utility.PushMessage("Uafd73c8615edbee77275b4bbc583c61c", Notes, "gpkB663L+H+xduic75ygNa/FYFeGjzfyzvcZSzj1X8KqzyTnldKHceqOlGnIDhMmiU4eienvd49L25fqNsqi13qurEEFxZJnQTxBI9HlQmaULztlmx5nQO4eB7iSgQPQWoPZvRATzHmO9dWWkw5gtwdB04t89/1O/w1cDnyilFU=");  //Arrhur個人Line uid   
                    //isRock.LineBot.Utility.PushMessage("Cbaa0920c644078cd1f3f98322b11b91c", Notes, "rZSLxGnpfniRYY2U0VYpOTwFikfGiiHwYtWGi8XSyK5jCTzu3m0RhDE+R10awbV6yok+4+zvznDcbplUcr/u7FD3c+6r0saGYUBDLH3iFnQddOaPsZXoeM49nfDIDS932CU7r6/cmqInaPNgAWBi0QdB04t89/1O/w1cDnyilFU=");   //Yvonne Line
                    isRock.LineBot.Utility.PushMessage("Uafd73c8615edbee77275b4bbc583c61c", Notes, "rZSLxGnpfniRYY2U0VYpOTwFikfGiiHwYtWGi8XSyK5jCTzu3m0RhDE+R10awbV6yok+4+zvznDcbplUcr/u7FD3c+6r0saGYUBDLH3iFnQddOaPsZXoeM49nfDIDS932CU7r6/cmqInaPNgAWBi0QdB04t89/1O/w1cDnyilFU="); //Arrhur個人Line uid 
            }

            Notes = string.Empty;

            if (lstBet.Count != 0)
            {

                foreach (BetList bl in lstBet)
                {
                    //新增注單資料
                    if (Common.chkIntData(bl, "xfyt") == 0 && Common.chkIntData2(bl, "xfyt") == 0)
                    {
                        Common.InsertBetData(bl, "xfyt");
                        if (Notes.IndexOf("期數:") == -1)
                            Notes += "期數:" + bl.Period + Environment.NewLine;
                        Notes += "  " + bl.carNo;
                        if (!string.IsNullOrEmpty(bl.bigsmall))
                            Notes += "  " + bl.bigsmall;
                        if (!string.IsNullOrEmpty(bl.oddEven))
                            Notes += "  " + bl.oddEven;
                        Notes += " " + bl.times + "倍";
                    }
                }

                if (!string.IsNullOrEmpty(Notes))
                    Notes = "幸運飛艇" + Environment.NewLine + Notes;

                if (!string.IsNullOrEmpty(Notes))
                    //isRock.LineBot.Utility.PushMessage("Cbaa0920c644078cd1f3f98322b11b91c", Notes, "gpkB663L+H+xduic75ygNa/FYFeGjzfyzvcZSzj1X8KqzyTnldKHceqOlGnIDhMmiU4eienvd49L25fqNsqi13qurEEFxZJnQTxBI9HlQmaULztlmx5nQO4eB7iSgQPQWoPZvRATzHmO9dWWkw5gtwdB04t89/1O/w1cDnyilFU=");
                    //isRock.LineBot.Utility.PushMessage("Uafd73c8615edbee77275b4bbc583c61c", Notes, "gpkB663L+H+xduic75ygNa/FYFeGjzfyzvcZSzj1X8KqzyTnldKHceqOlGnIDhMmiU4eienvd49L25fqNsqi13qurEEFxZJnQTxBI9HlQmaULztlmx5nQO4eB7iSgQPQWoPZvRATzHmO9dWWkw5gtwdB04t89/1O/w1cDnyilFU=");  //Arrhur個人Line uid
                    //isRock.LineBot.Utility.PushMessage("Cbaa0920c644078cd1f3f98322b11b91c", Notes, "rZSLxGnpfniRYY2U0VYpOTwFikfGiiHwYtWGi8XSyK5jCTzu3m0RhDE+R10awbV6yok+4+zvznDcbplUcr/u7FD3c+6r0saGYUBDLH3iFnQddOaPsZXoeM49nfDIDS932CU7r6/cmqInaPNgAWBi0QdB04t89/1O/w1cDnyilFU=");   //Yvonne Line
                    isRock.LineBot.Utility.PushMessage("Uafd73c8615edbee77275b4bbc583c61c", Notes, "rZSLxGnpfniRYY2U0VYpOTwFikfGiiHwYtWGi8XSyK5jCTzu3m0RhDE+R10awbV6yok+4+zvznDcbplUcr/u7FD3c+6r0saGYUBDLH3iFnQddOaPsZXoeM49nfDIDS932CU7r6/cmqInaPNgAWBi0QdB04t89/1O/w1cDnyilFU="); //Arrhur個人Line uid 
            }
            lstBet.Clear();
            ndBet.Clear();
        }

        //北京賽車闖關分析
        //抓出北京賽車計畫吻合注單
        public void getpk10(string html, string html2, string html3, string html4, string html5, string html6)
        {
            List<BetList> lstBetpk10 = new List<BetList>();   //下注資料List

            List<BetList> ndBetpk10 = new List<BetList>();   //過關資料
            int carNo = 0;      //車次名號
            string bigsmall = string.Empty;   //大小
            string oddEven = string.Empty;
            string record = string.Empty;     //期數



            #region "數據測試區塊"
            //StreamReader srnews = new StreamReader("C:\\pk10data\\p8Bnew.txt");
            //StreamReader srhis = new StreamReader("C:\\pk10data\\p8Bhis.txt");

            //html = srnews.ReadToEnd();  //六期最新一期
            //html2 = srhis.ReadToEnd(); //六期歷史
            //html3 = html;   //八期最新一期
            //html4 = html2;  //八期歷史
            //html5 = html;   //4期最新一期
            //html6 = html2;  //4期歷史
            //srnews.Close();
            //srhis.Close();
            #endregion




            // 使用預設編碼讀入 HTML 
            HtmlAgilityPack.HtmlDocument htDoc = new HtmlAgilityPack.HtmlDocument();
            HtmlAgilityPack.HtmlDocument htDoc2 = new HtmlAgilityPack.HtmlDocument();
            HtmlAgilityPack.HtmlDocument htDoc3 = new HtmlAgilityPack.HtmlDocument();
            HtmlAgilityPack.HtmlDocument htDoc4 = new HtmlAgilityPack.HtmlDocument();
            HtmlAgilityPack.HtmlDocument htDoc5 = new HtmlAgilityPack.HtmlDocument();
            HtmlAgilityPack.HtmlDocument htDoc6 = new HtmlAgilityPack.HtmlDocument();

            try
            {
                htDoc.LoadHtml(html);     //北京闖關計畫6關最新一期
                htDoc2.LoadHtml(html2);   //北京闖關計畫6關歷史期數
                htDoc3.LoadHtml(html3);    //北京闖關計畫8關最新一期
                htDoc4.LoadHtml(html4);   //北京闖關計畫8關歷史期數
                htDoc5.LoadHtml(html5);    //北京闖關計畫8關最新一期
                htDoc6.LoadHtml(html6);   //北京闖關計畫8關歷史期數

            }
            catch (Exception ex)
            {
                throw ex;
            }

            //回傳json格式資料
            string jsonfile1 = htDoc.DocumentNode.InnerText;   //六期最新一期闖關計畫
            string jsonfile2 = htDoc2.DocumentNode.InnerText;  //六期歷史闖關計畫
            string jsonfile3 = htDoc3.DocumentNode.InnerText;  //八期最新一期闖關計畫
            string jsonfile4 = htDoc4.DocumentNode.InnerText;  //八期歷史闖關計畫
            string jsonfile5 = htDoc5.DocumentNode.InnerText;  //四期最新一期闖關計畫
            string jsonfile6 = htDoc6.DocumentNode.InnerText;  //四期歷史闖關計畫

            if (jsonfile1.IndexOf("html") != -1) return;
            if (jsonfile2.IndexOf("html") != -1) return;
            if (jsonfile3.IndexOf("html") != -1) return;
            if (jsonfile4.IndexOf("html") != -1) return;
            if (jsonfile5.IndexOf("html") != -1) return;
            if (jsonfile6.IndexOf("html") != -1) return;

            DataSet myDataSet1 = JsonConvert.DeserializeObject<DataSet>(jsonfile1);
            DataSet myDataSet2 = JsonConvert.DeserializeObject<DataSet>(jsonfile2);
            DataSet myDataSet3 = JsonConvert.DeserializeObject<DataSet>(jsonfile3);
            DataSet myDataSet4 = JsonConvert.DeserializeObject<DataSet>(jsonfile4);
            DataSet myDataSet5 = JsonConvert.DeserializeObject<DataSet>(jsonfile5);
            DataSet myDataSet6 = JsonConvert.DeserializeObject<DataSet>(jsonfile6);


            DataTable dt1 = myDataSet1.Tables[0];   //六期最新報號 0:bigSmall  
            DataTable dt2 = myDataSet2.Tables[0];   //六期歷史數據
            DataTable dt3 = myDataSet1.Tables[1];   //六期最新報號 1:oddEven 
            DataTable dt4 = myDataSet2.Tables[1];   //六期歷史數據
            DataTable dt5 = myDataSet3.Tables[0];   //八期最新報號 0:bigSmall 
            DataTable dt6 = myDataSet4.Tables[0];   //八期歷史數據
            DataTable dt7 = myDataSet3.Tables[1];   //八期最新報號 1:oddEven
            DataTable dt8 = myDataSet4.Tables[1];   //八期歷史數據
            DataTable dt9 = myDataSet5.Tables[0];   //四期最新報號 0:bigSmall 
            DataTable dt10 = myDataSet6.Tables[0];  //四期歷史數據
            DataTable dt11 = myDataSet5.Tables[1];  //四期最新報號 1:oddEven 
            DataTable dt12 = myDataSet6.Tables[1];  //四期歷史數據

            bool flag = false;

            string few = string.Empty;

            int i = 0;

            #region "北京6期"

            //六期目前大小判斷
            few = dt1.Rows[i]["PlanResultFew"].ToString();

            DataRow dr1 = dt1.Rows[i];   //最新一期
            DataRow dr2 = dt2.Rows[i];  //歷史計畫最新一期

            //最新歷史計畫與資料庫已存在歷史資料比對
            //if (Common.chkhisData(dt2, "6B", "pk10") != 1)
            //    goto Plan6D;

            //先檢查history中4B闖關是否過關
            bool oddFLag = Common.checkBetData(dr1, dr2, "6B", "pk10", 0, ref lstBetpk10, ref ndBetpk10);
            Common.CheckOldData(dt2, dr1, "6B", "pk10", 0, ref lstBetpk10, ref ndBetpk10);

            //計算歷史數據連續失敗的次數
            int failure = 0;
            failure = Common.checkfailure(dt2);

            //正押判斷
            //if (dt1.Rows[i]["PlanResult"].ToString() == "0" && int.Parse(few) > 0 && dt2.Rows[0]["PlanResult"].ToString() == "-1")
            if (dt1.Rows[i]["PlanResult"].ToString() == "0" && int.Parse(few) >= 0 && dt2.Rows[0]["PlanResult"].ToString() == "-1")
            {     //最新期數闖關中,大於1期==>抓出歷史數據第一期為失敗 

                //回傳車道名次
                carNo = int.Parse(dt1.Rows[i]["PlanPosition"].ToString()) + 1;
                //闖關中,開出關數少於五關,且前一期失敗=>回傳車號與下注值
                //計畫大小列表
                string[] PlanDataText = dt1.Rows[i]["PlanDataText"].ToString().Split(",".ToCharArray());
                //回傳下注值(大or 小)
                bigsmall = PlanDataText[int.Parse(dt1.Rows[i]["PlanResultFew"].ToString())];

                BetList blst = new BetList();
                blst.PlanId = dt1.Rows[i]["PlanId"].ToString();                     //計畫流水號
                blst.StartPeriod = int.Parse(dt1.Rows[i]["StartPeriod"].ToString());  //起始期數
                blst.carNo = carNo.ToString();
                blst.bigsmall = bigsmall;

                blst.Period = int.Parse(dt1.Rows[i]["StartPeriod"].ToString()) + int.Parse(dt1.Rows[i]["PlanResultFew"].ToString());
                blst.times = Common.getTime("6B", "pk10");  //倍數
                blst.type = "6B";   //飛艇六期大小
                blst.BetType = 1;   //正押
                blst.few = int.Parse(dt1.Rows[i]["PlanResultFew"].ToString());        //下注該計畫第幾顆數位置
                lstBetpk10.Add(blst);
            }
            else if (dt1.Rows[i]["PlanResult"].ToString() == "0" && failure > 2)
            {
                //回傳車道名次
                carNo = int.Parse(dt1.Rows[i]["PlanPosition"].ToString()) + 1;
                //闖關中,開出關數少於五關,且前一期失敗=>回傳車號與下注值
                //計畫大小列表
                string[] PlanDataText = dt1.Rows[i]["PlanDataText"].ToString().Split(",".ToCharArray());
                //回傳下注值(大or 小)
                bigsmall = PlanDataText[int.Parse(few)];

                BetList blst = new BetList();
                blst.PlanId = dt1.Rows[i]["PlanId"].ToString();                     //計畫流水號
                blst.StartPeriod = int.Parse(dt1.Rows[i]["StartPeriod"].ToString());  //起始期數
                blst.carNo = carNo.ToString();
                blst.bigsmall = bigsmall;

                blst.Period = int.Parse(dt1.Rows[i]["StartPeriod"].ToString()) + int.Parse(dt1.Rows[i]["PlanResultFew"].ToString());
                blst.times = Common.getTime("6B", "pk10");  //倍數
                blst.type = "6B";   //飛艇六期大小
                blst.BetType = 1;   //正押
                blst.few = int.Parse(dt1.Rows[i]["PlanResultFew"].ToString());        //下注該計畫第幾顆數位置
                lstBetpk10.Add(blst);

            }

            //到推五關全過判斷
            //如果是在前1關就闖關成功,則看前一關
            if (dt2.Rows[i]["PlanResult"].ToString() == "1" && dt2.Rows[i]["PlanResultFew"].ToString() == "1")
            {
                //如果前2關也是成功==>抓出最新一期的闖關號投頭
                if (dt2.Rows[i + 1]["PlanResult"].ToString() == "1" && dt2.Rows[i + 1]["PlanResultFew"].ToString() == "1")
                {
                    //如果前3關也是成功==>抓出最新一期的闖關號投頭
                    if (dt2.Rows[i + 2]["PlanResult"].ToString() == "1" && dt2.Rows[i + 2]["PlanResultFew"].ToString() == "1")
                    {
                        //如果前4關也是成功==>抓出最新一期的闖關號投頭
                        if (dt2.Rows[i + 3]["PlanResult"].ToString() == "1" && dt2.Rows[i + 3]["PlanResultFew"].ToString() == "1")
                        {
                              //如果前5關也是成功==>抓出最新一期的闖關號投頭
                            if (dt2.Rows[i + 4]["PlanResult"].ToString() == "1" && dt2.Rows[i + 4]["PlanResultFew"].ToString() == "1")
                            {
                                int panidNew = int.Parse(dt1.Rows[i]["PlanId"].ToString()); //最新一期計畫期數
                                int panidhis = int.Parse(dt2.Rows[i]["PlanId"].ToString()); //歷史第一期計畫期數
                                //如果前5關也是成功==>抓出最新一期的闖關號投頭
                                if (dt2.Rows[i + 5]["PlanResult"].ToString() == "1" && panidNew > panidhis)
                                {
                                    //期數
                                    int period = int.Parse(dt1.Rows[i]["StartPeriod"].ToString()) + int.Parse(dt1.Rows[i]["PlanResultFew"].ToString());
                                    //最新一期必須是尚未開出,且是最新一期的第一顆,且距離上一次過關期數Period差距6期
                                    if (int.Parse(dt1.Rows[i]["PlanResult"].ToString()) == 0 && int.Parse(dt1.Rows[i]["PlanResultFew"].ToString()) == 0 &&  Common.check6Period("6B","pk10",period) ==1)
                                    {
                                        //回傳車道名次
                                        carNo = int.Parse(dt1.Rows[i]["PlanPosition"].ToString()) + 1;
                                        //計畫大小列表
                                        string[] PlanDataText = dt1.Rows[i]["PlanDataText"].ToString().Split(",".ToCharArray());
                                        //回傳下注值(大or 小)
                                        bigsmall = PlanDataText[int.Parse(dt1.Rows[i]["PlanResultFew"].ToString())];

                                        //反押判斷
                                        if (bigsmall == "大")
                                            bigsmall = "小";
                                        else
                                            bigsmall = "大";

                                        BetList blst = new BetList();
                                        blst.PlanId = dt1.Rows[i]["PlanId"].ToString();                     //計畫流水號
                                        blst.StartPeriod = int.Parse(dt1.Rows[i]["StartPeriod"].ToString());  //起始期數
                                        blst.carNo = carNo.ToString();
                                        blst.bigsmall = bigsmall;
                                        blst.times = Common.getTime("6B", "pk10");
                                        blst.Period = int.Parse(dt1.Rows[i]["StartPeriod"].ToString()) + int.Parse(dt1.Rows[i]["PlanResultFew"].ToString());
                                        blst.type = "6B";   //pk10 六期大小
                                        blst.BetType = 0;   //反押
                                        blst.few = int.Parse(dt1.Rows[i]["PlanResultFew"].ToString());        //下注該計畫第幾顆數位置
                                        lstBetpk10.Add(blst);

                                    }
                                }
                            }
                        }
                    }
                }
            }

            //六期單雙
        Plan6D:
            //pk10六期目前單雙判斷
            few = dt3.Rows[i]["PlanResultFew"].ToString();


            DataRow dr3 = dt3.Rows[i];   //最新一期
            DataRow dr4 = dt4.Rows[i];  //歷史計畫最新一期

            //最新歷史計畫與資料庫已存在歷史資料比對
            //if (Common.chkhisData(dt4, "6D", "pk10") != 1)
            //    goto Plan8B;

            //先檢查history中4B闖關是否過關
            bool oddFLag3 = Common.checkBetData(dr3, dr4, "6D", "pk10", 0, ref lstBetpk10, ref ndBetpk10);
            Common.CheckOldData(dt4, dr3, "6D", "pk10", 0, ref lstBetpk10, ref ndBetpk10);

            //計算歷史數據連續失敗的次數
            failure = 0;
            failure = Common.checkfailure(dt4);

            //pk10 六期單雙正押
            if (dt3.Rows[i]["PlanResult"].ToString() == "0" && int.Parse(few) >= 0 && dt4.Rows[0]["PlanResult"].ToString() == "-1")
            {//最新期數闖關中,且大於1期==>抓出歷史數據第一期為失敗 
                //回傳車道名次
                carNo = int.Parse(dt3.Rows[i]["PlanPosition"].ToString()) + 1;
                //闖關中,開出關數少於五關,且前一期失敗=>回傳車號與下注值
                //計畫大小列表
                string[] PlanDataText = dt3.Rows[i]["PlanDataText"].ToString().Split(",".ToCharArray());
                //回傳下注值(大or 小)
                oddEven = PlanDataText[int.Parse(dt3.Rows[i]["PlanResultFew"].ToString())];


                BetList blst = new BetList();
                blst.PlanId = dt3.Rows[i]["PlanId"].ToString();                     //計畫流水號
                blst.StartPeriod = int.Parse(dt3.Rows[i]["StartPeriod"].ToString());  //起始期數
                blst.carNo = carNo.ToString();
                blst.oddEven = oddEven;
                blst.times = Common.getTime("6D", "pk10");
                blst.Period = int.Parse(dt3.Rows[i]["StartPeriod"].ToString()) + int.Parse(dt3.Rows[i]["PlanResultFew"].ToString());
                blst.type = "6D";   //pk10 六期單雙
                blst.BetType = 1;   //正押
                blst.few = int.Parse(few);        //下注該計畫第幾顆數位置
                lstBetpk10.Add(blst);

            }
            else if (dt3.Rows[i]["PlanResult"].ToString() == "0" && failure > 2)
            {
                //回傳車道名次
                carNo = int.Parse(dt3.Rows[i]["PlanPosition"].ToString()) + 1;
                //闖關中,開出關數少於五關,且前一期失敗=>回傳車號與下注值
                //計畫大小列表
                string[] PlanDataText = dt3.Rows[i]["PlanDataText"].ToString().Split(",".ToCharArray());
                //回傳下注值(大or 小)
                oddEven = PlanDataText[int.Parse(few)];


                BetList blst = new BetList();
                blst.PlanId = dt3.Rows[i]["PlanId"].ToString();                     //計畫流水號
                blst.StartPeriod = int.Parse(dt3.Rows[i]["StartPeriod"].ToString());  //起始期數
                blst.carNo = carNo.ToString();
                blst.oddEven = oddEven;
                blst.times = Common.getTime("6D", "pk10");
                blst.Period = int.Parse(dt3.Rows[i]["StartPeriod"].ToString()) + int.Parse(dt3.Rows[i]["PlanResultFew"].ToString());
                blst.type = "6D";   //pk10 六期單雙
                blst.BetType = 1;   //正押
                blst.few = int.Parse(dt3.Rows[i]["PlanResultFew"].ToString());        //下注該計畫第幾顆數位置
                lstBetpk10.Add(blst);

            }


            //到推五關全過判斷
            //如果是在前1關就闖關成功,則看前一關
            if (dt4.Rows[i]["PlanResult"].ToString() == "1" && dt4.Rows[i]["PlanResultFew"].ToString() == "1")
            {
                //如果前2關也是成功==>抓出最新一期的闖關號投頭
                if (dt4.Rows[i + 1]["PlanResult"].ToString() == "1" && dt4.Rows[i + 1]["PlanResultFew"].ToString() == "1")
                {
                    //如果前3關也是成功==>抓出最新一期的闖關號投頭
                    if (dt4.Rows[i + 2]["PlanResult"].ToString() == "1" && dt4.Rows[i + 2]["PlanResultFew"].ToString() == "1")
                    {
                        //如果前4關也是成功==>抓出最新一期的闖關號投頭
                        if (dt4.Rows[i + 3]["PlanResult"].ToString() == "1" && dt4.Rows[i + 3]["PlanResultFew"].ToString() == "1")
                        {
                             //如果前5關也是成功==>抓出最新一期的闖關號投頭
                            if (dt4.Rows[i + 4]["PlanResult"].ToString() == "1" && dt4.Rows[i + 4]["PlanResultFew"].ToString() == "1")
                            {
                                //如果前6關也是成功==>抓出最新一期的闖關號投頭
                                if (dt4.Rows[i + 5]["PlanResult"].ToString() == "1")
                                {
                                    int panidNew = int.Parse(dt3.Rows[i]["PlanId"].ToString()); //最新一期計畫期數
                                    int panidhis = int.Parse(dt4.Rows[i]["PlanId"].ToString()); //歷史第一期計畫期數

                                    //期數
                                    int period = int.Parse(dt3.Rows[i]["StartPeriod"].ToString()) + int.Parse(dt3.Rows[i]["PlanResultFew"].ToString());
                                    //最新一期必須是尚未開出
                                    if (int.Parse(dt3.Rows[i]["PlanResult"].ToString()) == 0 && panidNew > panidhis && int.Parse(dt3.Rows[i]["PlanResult"].ToString()) == 0 && Common.check6Period("6D","pk10",period) ==1)
                                    {
                                        //回傳車道名次
                                        carNo = int.Parse(dt3.Rows[i]["PlanPosition"].ToString()) + 1;
                                        //計畫大小列表
                                        string[] PlanDataText = dt3.Rows[i]["PlanDataText"].ToString().Split(",".ToCharArray());
                                        //回傳下注值(大or 小)
                                        oddEven = PlanDataText[int.Parse(dt3.Rows[i]["PlanResultFew"].ToString())];

                                        //反押
                                        if (oddEven == "单")
                                            oddEven = "双";
                                        else
                                            oddEven = "单";


                                        BetList blst = new BetList();
                                        blst.PlanId = dt3.Rows[i]["PlanId"].ToString();                     //計畫流水號
                                        blst.StartPeriod = int.Parse(dt3.Rows[i]["StartPeriod"].ToString());  //起始期數
                                        blst.carNo = carNo.ToString();
                                        blst.oddEven = oddEven;
                                        blst.times = Common.getTime("6D", "pk10");
                                        blst.Period = int.Parse(dt3.Rows[i]["StartPeriod"].ToString()) + int.Parse(dt3.Rows[i]["PlanResultFew"].ToString());
                                        blst.type = "6D";   //飛艇六期單雙
                                        blst.BetType = 0;   //反押
                                        blst.few = int.Parse(dt3.Rows[i]["PlanResultFew"].ToString());        //下注該計畫第幾顆數位置
                                        lstBetpk10.Add(blst);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            #endregion

            #region "北京八期"
            //飛艇八期大小判斷
            few = string.Empty;

            i = 0;

        Plan8B:
            //北京八期期目前大小判斷
            few = dt5.Rows[i]["PlanResultFew"].ToString();

            DataRow dr5 = dt5.Rows[i];   //最新一期
            DataRow dr6 = dt6.Rows[i];  //歷史計畫最新一期

            //最新歷史計畫與資料庫已存在歷史資料比對
            //if (Common.chkhisData(dt6, "8B", "pk10") != 1)
            //    goto Plan8D;


            //先檢查history中4B闖關是否過關
            bool oddFLag5 = Common.checkBetData(dr5, dr6, "8B", "pk10", 0, ref lstBetpk10, ref  ndBetpk10);
            Common.CheckOldData(dt6, dr5, "8B", "pk10", 0, ref lstBetpk10, ref  ndBetpk10);

            //計算歷史數據連續失敗的次數
            failure = 0;
            failure = Common.checkfailure(dt6);


            //闖關中,已經開了7關不中==>抓出該車次
            if (dt5.Rows[i]["PlanResult"].ToString() == "0" && int.Parse(dt5.Rows[i]["PlanResultFew"].ToString()) > 6)
            {
                //回傳車道名次
                carNo = int.Parse(dt5.Rows[i]["PlanPosition"].ToString()) + 1;
                //計畫大小列表
                string[] PlanDataText = dt5.Rows[i]["PlanDataText"].ToString().Split(",".ToCharArray());
                //回傳下注值(大or 小)
                bigsmall = PlanDataText[int.Parse(few)];

                BetList blst = new BetList();
                blst.PlanId = dt5.Rows[i]["PlanId"].ToString();                     //計畫流水號
                blst.StartPeriod = int.Parse(dt5.Rows[i]["StartPeriod"].ToString());  //起始期數
                blst.carNo = carNo.ToString();
                blst.bigsmall = bigsmall;
                blst.times = Common.getTime("8B", "pk10"); ;  //倍數
                blst.type = "8B";
                blst.BetType = 1;   //正押
                blst.Period = int.Parse(dt5.Rows[i]["StartPeriod"].ToString()) + int.Parse(dt5.Rows[i]["PlanResultFew"].ToString());
                blst.few = int.Parse(few);        //下注該計畫第幾顆數位置
                lstBetpk10.Add(blst);
            }
            else if (dt5.Rows[i]["PlanResult"].ToString() == "0" && dt6.Rows[0]["PlanResult"].ToString() == "-1")
            {     //抓出歷史數據第一期為失敗 , 且最數闖關牌小於7期

                //回傳車道名次
                carNo = int.Parse(dt5.Rows[i]["PlanPosition"].ToString()) + 1;
                //闖關中,開出關數少於五關,且前一期失敗=>回傳車號與下注值
                //計畫大小列表
                string[] PlanDataText = dt5.Rows[i]["PlanDataText"].ToString().Split(",".ToCharArray());
                //回傳下注值(大or 小)
                bigsmall = PlanDataText[int.Parse(few)];

                BetList blst = new BetList();
                blst.PlanId = dt5.Rows[i]["PlanId"].ToString();                     //計畫流水號
                blst.StartPeriod = int.Parse(dt5.Rows[i]["StartPeriod"].ToString());  //起始期數
                blst.carNo = carNo.ToString();
                blst.bigsmall = bigsmall;
                blst.times = Common.getTime("8B", "pk10"); ;  //倍數
                blst.type = "8B";
                blst.BetType = 1;   //正押
                blst.Period = int.Parse(dt5.Rows[i]["StartPeriod"].ToString()) + int.Parse(dt5.Rows[i]["PlanResultFew"].ToString());
                blst.few = int.Parse(few);        //下注該計畫第幾顆數位置
                lstBetpk10.Add(blst);

            }
            else if (dt5.Rows[i]["PlanResult"].ToString() == "0" && failure > 2)
            {
                //回傳車道名次
                carNo = int.Parse(dt5.Rows[i]["PlanPosition"].ToString()) + 1;
                //闖關中,開出關數少於五關,且前一期失敗=>回傳車號與下注值
                //計畫大小列表
                string[] PlanDataText = dt5.Rows[i]["PlanDataText"].ToString().Split(",".ToCharArray());
                //回傳下注值(大or 小)
                bigsmall = PlanDataText[int.Parse(dt5.Rows[i]["PlanResultFew"].ToString())];

                BetList blst = new BetList();
                blst.PlanId = dt5.Rows[i]["PlanId"].ToString();                     //計畫流水號
                blst.StartPeriod = int.Parse(dt5.Rows[i]["StartPeriod"].ToString());  //起始期數
                blst.carNo = carNo.ToString();
                blst.bigsmall = bigsmall;
                blst.times = Common.getTime("8B", "pk10"); ;  //倍數
                blst.type = "8B";
                blst.BetType = 1;   //正押
                blst.Period = int.Parse(dt5.Rows[i]["StartPeriod"].ToString()) + int.Parse(dt5.Rows[i]["PlanResultFew"].ToString());
                blst.few = int.Parse(dt5.Rows[i]["PlanResultFew"].ToString());        //下注該計畫第幾顆數位置
                lstBetpk10.Add(blst);
            }


            //到推五關全過判斷
            //如果是在前1關就闖關成功,則看前一關
            if (dt6.Rows[i]["PlanResult"].ToString() == "1" && dt6.Rows[i]["PlanResultFew"].ToString() == "1")
            {
                //如果前2關也是成功==>抓出最新一期的闖關號投頭
                if (dt6.Rows[i + 1]["PlanResult"].ToString() == "1" && dt6.Rows[i + 1]["PlanResultFew"].ToString() == "1")
                {
                    //如果前3關也是成功==>抓出最新一期的闖關號投頭
                    if (dt6.Rows[i + 2]["PlanResult"].ToString() == "1" && dt6.Rows[i + 2]["PlanResultFew"].ToString() == "1")
                    {
                        //如果前4關也是成功==>抓出最新一期的闖關號投頭
                        if (dt6.Rows[i + 3]["PlanResult"].ToString() == "1" && dt6.Rows[i + 3]["PlanResultFew"].ToString() == "1")
                        {
                             //如果前5關也是成功==>抓出最新一期的闖關號投頭
                        if (dt6.Rows[i + 4]["PlanResult"].ToString() == "1" && dt6.Rows[i + 4]["PlanResultFew"].ToString() == "1")
                        {
                            int panidNew = int.Parse(dt5.Rows[i]["PlanId"].ToString()); //最新一期計畫期數
                            int panidhis = int.Parse(dt6.Rows[i]["PlanId"].ToString()); //歷史第一期計畫期數
                            //如果前6關也是成功==>抓出最新一期的闖關號投頭
                            if (dt6.Rows[i + 5]["PlanResult"].ToString() == "1" && panidNew > panidhis)
                            {
                                int period = int.Parse(dt5.Rows[i]["StartPeriod"].ToString()) + int.Parse(dt5.Rows[i]["PlanResultFew"].ToString());
                                //最新一期必須是尚未開出,反押在第一顆未開出
                                if (int.Parse(dt5.Rows[i]["PlanResult"].ToString()) == 0 && int.Parse(dt5.Rows[i]["PlanResultFew"].ToString()) == 0 && Common.check6Period("8B", "pk10", period) == 1)
                                {
                                    //回傳車道名次
                                    carNo = int.Parse(dt5.Rows[i]["PlanPosition"].ToString()) + 1;
                                    //計畫大小列表
                                    string[] PlanDataText = dt5.Rows[i]["PlanDataText"].ToString().Split(",".ToCharArray());
                                    //回傳下注值(大or 小)
                                    bigsmall = PlanDataText[int.Parse(dt5.Rows[i]["PlanResultFew"].ToString())];

                                    //反押判斷
                                    if (bigsmall == "大")
                                        bigsmall = "小";
                                    else
                                        bigsmall = "大";

                                    BetList blst = new BetList();
                                    blst.PlanId = dt5.Rows[i]["PlanId"].ToString();                     //計畫流水號
                                    blst.StartPeriod = int.Parse(dt5.Rows[i]["StartPeriod"].ToString());  //起始期數
                                    blst.carNo = carNo.ToString();
                                    blst.bigsmall = bigsmall;
                                    blst.times = Common.getTime("8B", "pk10"); ;  //倍數
                                    blst.type = "8B";
                                    blst.BetType = 0;   //反押
                                    blst.Period = int.Parse(dt5.Rows[i]["StartPeriod"].ToString()) + int.Parse(dt5.Rows[i]["PlanResultFew"].ToString());
                                    blst.few = int.Parse(dt5.Rows[i]["PlanResultFew"].ToString());        //下注該計畫第幾顆數位置
                                    lstBetpk10.Add(blst);

                                }
                            }
                            }
                        }
                    }
                }
            }
        Plan8D:

            oddEven = string.Empty;

            ///北京八期目前單雙判斷
            few = dt7.Rows[i]["PlanResultFew"].ToString();
            DataRow dr7 = dt7.Rows[i];   //最新一期
            DataRow dr8 = dt8.Rows[i];  //歷史計畫最新一期

            //如果最新 plan id < 歷史計畫最新一期 plan id
            if (int.Parse(dr7["PlanId"].ToString()) < int.Parse(dr8["PlanId"].ToString()))
                goto Plan4B;

            //先檢查history中4B闖關是否過關
            bool oddFLags = Common.checkBetData(dr7, dr8, "8D", "pk10", 0, ref  lstBetpk10, ref ndBetpk10);
            Common.CheckOldData(dt8, dr7, "8D", "pk10", 0, ref lstBetpk10, ref ndBetpk10);

            //計算歷史數據連續失敗的次數
            failure = 0;
            failure = Common.checkfailure(dt8);

            //oddEven正押判斷
            //闖關中,已經開了七關不中==>抓出該車次,開始正押
            if (dt7.Rows[i]["PlanResult"].ToString() == "0" && int.Parse(dt7.Rows[i]["PlanResultFew"].ToString()) > 6)
            {
                //回傳車道名次
                carNo = int.Parse(dt7.Rows[i]["PlanPosition"].ToString()) + 1;
                //計畫大小列表
                string[] PlanDataText = dt7.Rows[i]["PlanDataText"].ToString().Split(",".ToCharArray());
                //回傳下注值(大or 小)
                oddEven = PlanDataText[int.Parse(few)];

                BetList blst = new BetList();
                blst.PlanId = dt7.Rows[i]["PlanId"].ToString();                     //計畫流水號
                blst.StartPeriod = int.Parse(dt7.Rows[i]["StartPeriod"].ToString());  //起始期數
                blst.carNo = carNo.ToString();
                blst.oddEven = oddEven;
                blst.times = Common.getTime("8D", "pk10"); ;  //倍數
                blst.type = "8D";
                blst.BetType = 1;   //正押
                blst.Period = int.Parse(dt7.Rows[i]["StartPeriod"].ToString()) + int.Parse(dt7.Rows[i]["PlanResultFew"].ToString());
                blst.few = int.Parse(dt7.Rows[i]["PlanResultFew"].ToString());        //下注該計畫第幾顆數位置
                lstBetpk10.Add(blst);
            }
            else if (dt7.Rows[i]["PlanResult"].ToString() == "0" && dt8.Rows[0]["PlanResult"].ToString() == "-1")
            {//最新期數闖關中,且小於5期==>抓出歷史數據第一期為失敗 
                //回傳車道名次
                carNo = int.Parse(dt7.Rows[i]["PlanPosition"].ToString()) + 1;
                //闖關中,開出關數少於五關,且前一期失敗=>回傳車號與下注值
                //計畫大小列表
                string[] PlanDataText = dt7.Rows[i]["PlanDataText"].ToString().Split(",".ToCharArray());
                //回傳下注值(大or 小)
                oddEven = PlanDataText[int.Parse(dt7.Rows[i]["PlanResultFew"].ToString())];


                BetList blst = new BetList();
                blst.PlanId = dt7.Rows[i]["PlanId"].ToString();                     //計畫流水號
                blst.StartPeriod = int.Parse(dt7.Rows[i]["StartPeriod"].ToString());  //起始期數
                blst.carNo = carNo.ToString();
                blst.oddEven = oddEven;
                blst.times = Common.getTime("8D", "pk10"); ;  //倍數
                blst.type = "8D";
                blst.BetType = 1;   //正押
                blst.Period = int.Parse(dt7.Rows[i]["StartPeriod"].ToString()) + int.Parse(dt7.Rows[i]["PlanResultFew"].ToString());
                blst.few = int.Parse(dt7.Rows[i]["PlanResultFew"].ToString());        //下注該計畫第幾顆數位置
                lstBetpk10.Add(blst);

            }
            else if (dt7.Rows[i]["PlanResult"].ToString() == "0" && failure > 2)
            {
                //回傳車道名次
                carNo = int.Parse(dt7.Rows[i]["PlanPosition"].ToString()) + 1;
                //闖關中,開出關數少於五關,且前一期失敗=>回傳車號與下注值
                //計畫大小列表
                string[] PlanDataText = dt7.Rows[i]["PlanDataText"].ToString().Split(",".ToCharArray());
                //回傳下注值(大or 小)
                oddEven = PlanDataText[int.Parse(dt7.Rows[i]["PlanResultFew"].ToString())];


                BetList blst = new BetList();
                blst.PlanId = dt7.Rows[i]["PlanId"].ToString();                     //計畫流水號
                blst.StartPeriod = int.Parse(dt7.Rows[i]["StartPeriod"].ToString());  //起始期數
                blst.carNo = carNo.ToString();
                blst.oddEven = oddEven;
                blst.times = Common.getTime("8D", "pk10"); ;  //倍數
                blst.type = "8D";
                blst.BetType = 1;   //正押
                blst.Period = int.Parse(dt7.Rows[i]["StartPeriod"].ToString()) + int.Parse(dt7.Rows[i]["PlanResultFew"].ToString());
                blst.few = int.Parse(dt7.Rows[i]["PlanResultFew"].ToString());        //下注該計畫第幾顆數位置
                lstBetpk10.Add(blst);

            }

            //反押判斷
            //抓出前七關全過,開始反押,如果前七關全不中=>開始正押

            //到推七關全過判斷
            //如果是在前1關就闖關成功,則看前一關
            if (dt8.Rows[i]["PlanResult"].ToString() == "1" && dt8.Rows[i]["PlanResultFew"].ToString() == "1")
            {
                //如果前2關也是成功==>抓出最新一期的闖關號投頭
                if (dt8.Rows[i + 1]["PlanResult"].ToString() == "1" && dt8.Rows[i + 1]["PlanResultFew"].ToString() == "1")
                {
                    //如果前3關也是成功==>抓出最新一期的闖關號投頭
                    if (dt8.Rows[i + 2]["PlanResult"].ToString() == "1" && dt8.Rows[i + 2]["PlanResultFew"].ToString() == "1")
                    {
                        //如果前4關也是成功==>抓出最新一期的闖關號投頭
                        if (dt8.Rows[i + 3]["PlanResult"].ToString() == "1" && dt8.Rows[i + 3]["PlanResultFew"].ToString() == "1")
                        {
                             //如果前5關也是成功==>抓出最新一期的闖關號投頭
                            if (dt8.Rows[i + 4]["PlanResult"].ToString() == "1" && dt8.Rows[i + 4]["PlanResultFew"].ToString() == "1")
                            {
                                //如果前6關也是成功==>抓出最新一期的闖關號投頭
                                if (dt8.Rows[i + 5]["PlanResult"].ToString() == "1")
                                {
                                    int panidNew = int.Parse(dt7.Rows[i]["PlanId"].ToString()); //最新一期計畫期數
                                    int panidhis = int.Parse(dt8.Rows[i]["PlanId"].ToString()); //歷史第一期計畫期數

                                    int period = int.Parse(dt7.Rows[i]["StartPeriod"].ToString()) + int.Parse(dt7.Rows[i]["PlanResultFew"].ToString());
                                    //最新一期必須是尚未開出
                                    if (int.Parse(dt7.Rows[i]["PlanResult"].ToString()) == 0 && panidNew > panidhis && int.Parse(dt7.Rows[i]["PlanResultFew"].ToString()) == 0 && Common.check6Period("8D","pk10",period)==1)
                                    {
                                        //回傳車道名次
                                        carNo = int.Parse(dt7.Rows[i]["PlanPosition"].ToString()) + 1;
                                        //計畫大小列表
                                        string[] PlanDataText = dt7.Rows[i]["PlanDataText"].ToString().Split(",".ToCharArray());
                                        //回傳下注值(大or 小)
                                        oddEven = PlanDataText[int.Parse(dt7.Rows[i]["PlanResultFew"].ToString())];

                                        //反押
                                        if (oddEven == "单")
                                            oddEven = "双";
                                        else
                                            oddEven = "单";

                                        BetList blst = new BetList();
                                        blst.PlanId = dt7.Rows[i]["PlanId"].ToString();                     //計畫流水號
                                        blst.StartPeriod = int.Parse(dt7.Rows[i]["StartPeriod"].ToString());  //起始期數
                                        blst.carNo = carNo.ToString();
                                        blst.oddEven = oddEven;
                                        blst.times = Common.getTime("8D", "pk10"); ;  //倍數
                                        blst.type = "8D";
                                        blst.BetType = 0;   //反押
                                        blst.Period = int.Parse(dt7.Rows[i]["StartPeriod"].ToString()) + int.Parse(dt7.Rows[i]["PlanResultFew"].ToString());
                                        blst.few = int.Parse(dt7.Rows[i]["PlanResultFew"].ToString());        //下注該計畫第幾顆數位置
                                        lstBetpk10.Add(blst);


                                    }

                                }
                            }
                        }

                    }

                }
            }

            #endregion

            #region "四期北京"
            //北京四期大小判斷
            few = string.Empty;
        Plan4B:
            i = 0;
            //北京四期大小判斷
            few = dt9.Rows[i]["PlanResultFew"].ToString();

            DataRow dr9 = dt9.Rows[i];   //最新一期
            DataRow dr10 = dt10.Rows[i];  //歷史計畫最新一期

            //最新歷史計畫與資料庫已存在歷史資料比對
            //if (Common.chkhisData(dt10, "4B", "pk10") != 1)
            //    goto Plan4D;

            //先檢查history中4B闖關是否過關
            bool flags = Common.checkBetData(dr9, dr10, "4B", "pk10", 0, ref lstBetpk10, ref ndBetpk10);
            Common.CheckOldData(dt10, dr9, "4B", "pk10", 0, ref lstBetpk10, ref ndBetpk10);

            //計算歷史數據連續失敗的次數
            failure = 0;
            failure = Common.checkfailure(dt10);

            int panidNews = int.Parse(dt9.Rows[i]["PlanId"].ToString()); //最新一期計畫期數
            int panidhiss = int.Parse(dt10.Rows[i]["PlanId"].ToString()); //歷史第一期計畫期數

            if (panidNews < panidhiss)
                goto Plan4D;

            if (dt9.Rows[i]["PlanResult"].ToString() == "0" && dt10.Rows[0]["PlanResult"].ToString() == "-1" && int.Parse(dt9.Rows[i]["PlanResultFew"].ToString()) > 2)
            {     //最新期數闖關中,且小於5期==>抓出歷史數據第一期為失敗 

                //回傳車道名次
                carNo = int.Parse(dt9.Rows[i]["PlanPosition"].ToString()) + 1;
                //闖關中,開出關數少於五關,且前一期失敗=>回傳車號與下注值
                //計畫大小列表
                string[] PlanDataText = dt9.Rows[i]["PlanDataText"].ToString().Split(",".ToCharArray());
                //回傳下注值(大or 小)
                bigsmall = PlanDataText[int.Parse(few)];


                BetList blst = new BetList();
                blst.PlanId = dt9.Rows[i]["PlanId"].ToString();                     //計畫流水號
                blst.StartPeriod = int.Parse(dt9.Rows[i]["StartPeriod"].ToString());  //起始期數
                blst.carNo = carNo.ToString();
                blst.bigsmall = bigsmall;
                blst.times = Common.getTime("4B", "pk10"); ;  //倍數
                blst.type = "4B";
                blst.BetType = 1;   //正押
                blst.Period = int.Parse(dt9.Rows[i]["StartPeriod"].ToString()) + int.Parse(dt9.Rows[i]["PlanResultFew"].ToString());
                blst.few = int.Parse(dt9.Rows[i]["PlanResultFew"].ToString());        //下注該計畫第幾顆數位置
                lstBetpk10.Add(blst);

            }
            else if (dt9.Rows[i]["PlanResult"].ToString() == "0" && failure >= 2)
            {
                //回傳車道名次
                carNo = int.Parse(dt9.Rows[i]["PlanPosition"].ToString()) + 1;
                //闖關中,開出關數少於五關,且前一期失敗=>回傳車號與下注值
                //計畫大小列表
                string[] PlanDataText = dt9.Rows[i]["PlanDataText"].ToString().Split(",".ToCharArray());
                //回傳下注值(大or 小)
                bigsmall = PlanDataText[int.Parse(few)];


                BetList blst = new BetList();
                blst.PlanId = dt9.Rows[i]["PlanId"].ToString();                     //計畫流水號
                blst.StartPeriod = int.Parse(dt9.Rows[i]["StartPeriod"].ToString());  //起始期數
                blst.carNo = carNo.ToString();
                blst.bigsmall = bigsmall;
                blst.times = Common.getTime("4B", "pk10"); ;  //倍數
                blst.type = "4B";
                blst.BetType = 1;   //正押
                blst.Period = int.Parse(dt9.Rows[i]["StartPeriod"].ToString()) + int.Parse(dt9.Rows[i]["PlanResultFew"].ToString());
                blst.few = int.Parse(dt9.Rows[i]["PlanResultFew"].ToString());        //下注該計畫第幾顆數位置
                lstBetpk10.Add(blst);
            }



            //反押推五關全過判斷
            //如果是在前1關就闖關成功,則看前一關
            if (dt10.Rows[i]["PlanResult"].ToString() == "1" && dt10.Rows[i]["PlanResultFew"].ToString() == "1")
            {
                //如果前2關也是成功==>抓出最新一期的闖關號投頭
                if (dt10.Rows[i + 1]["PlanResult"].ToString() == "1" && dt10.Rows[i + 1]["PlanResultFew"].ToString() == "1")
                {
                    //如果前3關也是成功==>抓出最新一期的闖關號投頭
                    if (dt10.Rows[i + 2]["PlanResult"].ToString() == "1" && dt10.Rows[i + 2]["PlanResultFew"].ToString() == "1")
                    {
                        //如果前4關也是成功==>抓出最新一期的闖關號投頭
                        if (dt10.Rows[i + 3]["PlanResult"].ToString() == "1" && dt10.Rows[i + 3]["PlanResultFew"].ToString() == "1")
                        {
                             //如果前5關也是成功==>抓出最新一期的闖關號投頭
                            if (dt10.Rows[i + 4]["PlanResult"].ToString() == "1" && dt10.Rows[i + 4]["PlanResultFew"].ToString() == "1")
                            {
                                //如果前6關也是成功==>抓出最新一期的闖關號投頭
                                if (dt10.Rows[i + 5]["PlanResult"].ToString() == "1")
                                {
                                    int panidNew = int.Parse(dt9.Rows[i]["PlanId"].ToString()); //最新一期計畫期數
                                    int panidhis = int.Parse(dt10.Rows[i]["PlanId"].ToString()); //歷史第一期計畫期數

                                    int period = int.Parse(dt9.Rows[i]["StartPeriod"].ToString()) + int.Parse(dt9.Rows[i]["PlanResultFew"].ToString());
                                    //最新一期必須是尚未開出==>為開除PlanResult , few皆為0 , 若已開出 且成功第一顆過關 ==>PlanResult , few皆為1
                                    if (int.Parse(dt9.Rows[i]["PlanResult"].ToString()) == 0 && panidNew > panidhis && int.Parse(dt9.Rows[i]["PlanResultFew"].ToString()) == 0 && Common.check6Period("4B", "pk10", period) == 1)
                                    {
                                        //回傳車道名次
                                        carNo = int.Parse(dt9.Rows[i]["PlanPosition"].ToString()) + 1;
                                        //計畫大小列表
                                        string[] PlanDataText = dt9.Rows[i]["PlanDataText"].ToString().Split(",".ToCharArray());
                                        //回傳下注值(大or 小)
                                        bigsmall = PlanDataText[int.Parse(dt9.Rows[i]["PlanResultFew"].ToString())];

                                        //反押判斷
                                        if (bigsmall == "大")
                                            bigsmall = "小";
                                        else
                                            bigsmall = "大";


                                        BetList blst = new BetList();
                                        blst.PlanId = dt9.Rows[i]["PlanId"].ToString();                     //計畫流水號
                                        blst.StartPeriod = int.Parse(dt9.Rows[i]["StartPeriod"].ToString());  //起始期數
                                        blst.carNo = carNo.ToString();
                                        blst.bigsmall = bigsmall;
                                        blst.times = Common.getTime("4B", "pk10"); ;  //倍數
                                        blst.type = "4B";
                                        blst.BetType = 0;   //反押
                                        blst.Period = int.Parse(dt9.Rows[i]["StartPeriod"].ToString()) + int.Parse(dt9.Rows[i]["PlanResultFew"].ToString());
                                        blst.few = int.Parse(dt9.Rows[i]["PlanResultFew"].ToString());        //下注該計畫第幾顆數位置
                                        lstBetpk10.Add(blst);


                                    }

                                }
                            }
                        }

                    }

                }
            }

            //北京四期單雙判斷
        Plan4D:
            oddEven = string.Empty;

            //目前開出的大小單雙
            few = dt11.Rows[i]["PlanResultFew"].ToString();

            DataRow dr11 = dt11.Rows[i];   //最新一期
            DataRow dr12 = dt12.Rows[i];  //歷史計畫最新一期
            //最新歷史計畫與資料庫已存在歷史資料比對
            //if (Common.chkhisData(dt12, "4D", "pk10") != 1)
            //    goto ShowMess;

            //先檢查history中4B闖關是否過關
            bool flagss = Common.checkBetData(dr11, dr12, "4D", "pk10", 0, ref lstBetpk10, ref ndBetpk10);
            Common.CheckOldData(dt12, dr11, "4D", "pk10", 0, ref lstBetpk10, ref ndBetpk10);

            //計算歷史數據連續失敗的次數
            failure = 0;
            failure = Common.checkfailure(dt12);

            //闖關中,已經開了五關不中==>抓出該車次

            if (dt11.Rows[i]["PlanResult"].ToString() == "0" && dt12.Rows[0]["PlanResult"].ToString() == "-1" && int.Parse(dt11.Rows[i]["PlanResultFew"].ToString()) > 2)
            {//最新期數闖關中,且小於5期==>抓出歷史數據第一期為失敗 
                //回傳車道名次
                carNo = int.Parse(dt11.Rows[i]["PlanPosition"].ToString()) + 1;
                //闖關中,開出關數少於五關,且前一期失敗=>回傳車號與下注值
                //計畫大小列表
                string[] PlanDataText = dt11.Rows[i]["PlanDataText"].ToString().Split(",".ToCharArray());
                //回傳下注值(大or 小)
                oddEven = PlanDataText[int.Parse(dt11.Rows[i]["PlanResultFew"].ToString())];


                BetList blst = new BetList();
                blst.PlanId = dt11.Rows[i]["PlanId"].ToString();                     //計畫流水號
                blst.StartPeriod = int.Parse(dt11.Rows[i]["StartPeriod"].ToString());  //起始期數
                blst.carNo = carNo.ToString();
                blst.oddEven = oddEven;
                blst.times = Common.getTime("4D", "pk10"); ;  //倍數
                blst.type = "4D";
                blst.BetType = 1;   //正押
                blst.Period = int.Parse(dt11.Rows[i]["StartPeriod"].ToString()) + int.Parse(dt11.Rows[i]["PlanResultFew"].ToString());
                blst.few = int.Parse(dt11.Rows[i]["PlanResultFew"].ToString());        //下注該計畫第幾顆數位置
                lstBetpk10.Add(blst);

            }
            else if (dt11.Rows[i]["PlanResult"].ToString() == "0" && failure >= 2)
            {
                //回傳車道名次
                carNo = int.Parse(dt11.Rows[i]["PlanPosition"].ToString()) + 1;
                //闖關中,開出關數少於五關,且前一期失敗=>回傳車號與下注值
                //計畫大小列表
                string[] PlanDataText = dt11.Rows[i]["PlanDataText"].ToString().Split(",".ToCharArray());
                //回傳下注值(大or 小)
                oddEven = PlanDataText[int.Parse(dt11.Rows[i]["PlanResultFew"].ToString())];


                BetList blst = new BetList();
                blst.PlanId = dt11.Rows[i]["PlanId"].ToString();                     //計畫流水號
                blst.StartPeriod = int.Parse(dt11.Rows[i]["StartPeriod"].ToString());  //起始期數
                blst.carNo = carNo.ToString();
                blst.oddEven = oddEven;
                blst.times = Common.getTime("4D", "pk10"); ;  //倍數
                blst.type = "4D";
                blst.BetType = 1;   //正押
                blst.Period = int.Parse(dt11.Rows[i]["StartPeriod"].ToString()) + int.Parse(dt11.Rows[i]["PlanResultFew"].ToString());
                blst.few = int.Parse(dt11.Rows[i]["PlanResultFew"].ToString());        //下注該計畫第幾顆數位置
                lstBetpk10.Add(blst);

            }

            //反押判斷
            //抓出前五關全過,開始反押,如果前五關全不中=>開始正押
            //for (int i = 0; i < dt4.Rows.Count; i++)
            //{
            //到推五關全過判斷
            //如果是在前1關就闖關成功,則看前一關
            if (dt12.Rows[i]["PlanResult"].ToString() == "1" && dt12.Rows[i]["PlanResultFew"].ToString() == "1")
            {
                //如果前2關也是成功==>抓出最新一期的闖關號投頭
                if (dt12.Rows[i + 1]["PlanResult"].ToString() == "1" && dt12.Rows[i + 1]["PlanResultFew"].ToString() == "1")
                {
                    //如果前3關也是成功==>抓出最新一期的闖關號投頭
                    if (dt12.Rows[i + 2]["PlanResult"].ToString() == "1" && dt12.Rows[i + 2]["PlanResultFew"].ToString() == "1")
                    {
                         //如果前4關也是成功==>抓出最新一期的闖關號投頭
                        if (dt12.Rows[i + 3]["PlanResult"].ToString() == "1" && dt12.Rows[i + 3]["PlanResultFew"].ToString() == "1")
                        {
                          
                        //如果前5關也是成功==>抓出最新一期的闖關號投頭
                        if (dt12.Rows[i + 4]["PlanResult"].ToString() == "1" && dt12.Rows[i + 4]["PlanResultFew"].ToString() == "1")
                        {
                            int panidNew = int.Parse(dt11.Rows[i]["PlanId"].ToString()); //最新一期計畫期數
                            int panidhis = int.Parse(dt12.Rows[i]["PlanId"].ToString()); //歷史第一期計畫期數
                            //如果前6關也是成功==>抓出最新一期的闖關號投頭
                            if (dt12.Rows[i + 5]["PlanResult"].ToString() == "1" && panidNew > panidhis)
                            {
                                int period = int.Parse(dt11.Rows[i]["StartPeriod"].ToString()) + int.Parse(dt11.Rows[i]["PlanResultFew"].ToString());
                                //最新一期必須是尚未開出
                                if (int.Parse(dt11.Rows[i]["PlanResult"].ToString()) == 0 && int.Parse(dt11.Rows[i]["PlanResultFew"].ToString()) == 0 && Common.check6Period("4D", "pk10", period) == 1)
                                {
                                    //回傳車道名次
                                    carNo = int.Parse(dt11.Rows[i]["PlanPosition"].ToString()) + 1;
                                    //計畫大小列表
                                    string[] PlanDataText = dt11.Rows[i]["PlanDataText"].ToString().Split(",".ToCharArray());
                                    //回傳下注值(大or 小)
                                    oddEven = PlanDataText[int.Parse(dt11.Rows[i]["PlanResultFew"].ToString())];


                                    //反押
                                    if (oddEven == "单")
                                        oddEven = "双";
                                    else
                                        oddEven = "单";


                                    BetList blst = new BetList();
                                    blst.PlanId = dt11.Rows[i]["PlanId"].ToString();                     //計畫流水號
                                    blst.StartPeriod = int.Parse(dt11.Rows[i]["StartPeriod"].ToString());  //起始期數
                                    blst.carNo = carNo.ToString();     //名次
                                    blst.oddEven = oddEven;            //單雙
                                    blst.Period = int.Parse(dt11.Rows[i]["StartPeriod"].ToString()) + int.Parse(dt11.Rows[i]["PlanResultFew"].ToString());
                                    blst.times = Common.getTime("4D", "pk10"); ;  //倍數
                                    blst.type = "4D";
                                    blst.BetType = 0;   //反押
                                    blst.few = int.Parse(dt11.Rows[i]["PlanResultFew"].ToString());        //下注該計畫第幾顆數位置
                                    lstBetpk10.Add(blst);

                                }
                            }
                            }
                        }
                    }
                }
            }
            #endregion
        ShowMess:
            //已過關的牌注
            string Notes = string.Empty;

            if (ndBetpk10.Count != 0)
            {


                foreach (BetList bl in ndBetpk10)
                {
                    if (Notes.IndexOf("期數:") == -1)
                        Notes += "期數:" + bl.Period + Environment.NewLine;
                    Notes += "  " + bl.carNo;
                    if (!string.IsNullOrEmpty(bl.bigsmall))
                        Notes += "  " + bl.bigsmall;
                    if (!string.IsNullOrEmpty(bl.oddEven))
                        Notes += "  " + bl.oddEven;
                    Notes += " " + bl.times + "倍";
                    if (bl.times <= 1023 && bl.status =="1")
                        Notes += " 過關" + Environment.NewLine;
                    else
                        Notes += " 爆關" + Environment.NewLine;
                }
                if (!string.IsNullOrEmpty(Notes))
                    Notes = "北京賽車" + Environment.NewLine + Notes;

                if (!string.IsNullOrEmpty(Notes))
                    //isRock.LineBot.Utility.PushMessage("Cbaa0920c644078cd1f3f98322b11b91c", Notes, "gpkB663L+H+xduic75ygNa/FYFeGjzfyzvcZSzj1X8KqzyTnldKHceqOlGnIDhMmiU4eienvd49L25fqNsqi13qurEEFxZJnQTxBI9HlQmaULztlmx5nQO4eB7iSgQPQWoPZvRATzHmO9dWWkw5gtwdB04t89/1O/w1cDnyilFU=");
                    //isRock.LineBot.Utility.PushMessage("Uafd73c8615edbee77275b4bbc583c61c", Notes, "gpkB663L+H+xduic75ygNa/FYFeGjzfyzvcZSzj1X8KqzyTnldKHceqOlGnIDhMmiU4eienvd49L25fqNsqi13qurEEFxZJnQTxBI9HlQmaULztlmx5nQO4eB7iSgQPQWoPZvRATzHmO9dWWkw5gtwdB04t89/1O/w1cDnyilFU=");  //Arrhur個人Line uid
                    //isRock.LineBot.Utility.PushMessage("Cbaa0920c644078cd1f3f98322b11b91c", Notes, "rZSLxGnpfniRYY2U0VYpOTwFikfGiiHwYtWGi8XSyK5jCTzu3m0RhDE+R10awbV6yok+4+zvznDcbplUcr/u7FD3c+6r0saGYUBDLH3iFnQddOaPsZXoeM49nfDIDS932CU7r6/cmqInaPNgAWBi0QdB04t89/1O/w1cDnyilFU=");   //Yvonne Line
                    isRock.LineBot.Utility.PushMessage("Uafd73c8615edbee77275b4bbc583c61c", Notes, "rZSLxGnpfniRYY2U0VYpOTwFikfGiiHwYtWGi8XSyK5jCTzu3m0RhDE+R10awbV6yok+4+zvznDcbplUcr/u7FD3c+6r0saGYUBDLH3iFnQddOaPsZXoeM49nfDIDS932CU7r6/cmqInaPNgAWBi0QdB04t89/1O/w1cDnyilFU="); //Arrhur個人Line uid 
            }

            Notes = string.Empty;

            if (lstBetpk10.Count != 0)
            {
                foreach (BetList bl in lstBetpk10)
                {
                    //新增注單資料
                    if (Common.chkIntData(bl, "pk10") == 0 && Common.chkIntData2(bl, "pk10") == 0)
                    {
                        Common.InsertBetData(bl, "pk10");
                        if (Notes.IndexOf("期數:") == -1)
                            Notes += "期數:" + bl.Period + Environment.NewLine;
                        Notes += "  " + bl.carNo;
                        if (!string.IsNullOrEmpty(bl.bigsmall))
                            Notes += " " + bl.bigsmall;
                        if (!string.IsNullOrEmpty(bl.oddEven))
                            Notes += " " + bl.oddEven;
                        Notes += " " + bl.times + "倍";
                    }
                }
                if (!string.IsNullOrEmpty(Notes))
                    Notes = "北京賽車" + Environment.NewLine + Notes;

                if (!string.IsNullOrEmpty(Notes))
                    //isRock.LineBot.Utility.PushMessage("Cbaa0920c644078cd1f3f98322b11b91c", Notes, "gpkB663L+H+xduic75ygNa/FYFeGjzfyzvcZSzj1X8KqzyTnldKHceqOlGnIDhMmiU4eienvd49L25fqNsqi13qurEEFxZJnQTxBI9HlQmaULztlmx5nQO4eB7iSgQPQWoPZvRATzHmO9dWWkw5gtwdB04t89/1O/w1cDnyilFU=");  
                    //isRock.LineBot.Utility.PushMessage("Uafd73c8615edbee77275b4bbc583c61c", Notes, "gpkB663L+H+xduic75ygNa/FYFeGjzfyzvcZSzj1X8KqzyTnldKHceqOlGnIDhMmiU4eienvd49L25fqNsqi13qurEEFxZJnQTxBI9HlQmaULztlmx5nQO4eB7iSgQPQWoPZvRATzHmO9dWWkw5gtwdB04t89/1O/w1cDnyilFU=");  //Arrhur個人Line uid
                    //isRock.LineBot.Utility.PushMessage("Cbaa0920c644078cd1f3f98322b11b91c", Notes, "rZSLxGnpfniRYY2U0VYpOTwFikfGiiHwYtWGi8XSyK5jCTzu3m0RhDE+R10awbV6yok+4+zvznDcbplUcr/u7FD3c+6r0saGYUBDLH3iFnQddOaPsZXoeM49nfDIDS932CU7r6/cmqInaPNgAWBi0QdB04t89/1O/w1cDnyilFU=");   //Yvonne Line
                    isRock.LineBot.Utility.PushMessage("Uafd73c8615edbee77275b4bbc583c61c", Notes, "rZSLxGnpfniRYY2U0VYpOTwFikfGiiHwYtWGi8XSyK5jCTzu3m0RhDE+R10awbV6yok+4+zvznDcbplUcr/u7FD3c+6r0saGYUBDLH3iFnQddOaPsZXoeM49nfDIDS932CU7r6/cmqInaPNgAWBi0QdB04t89/1O/w1cDnyilFU="); //Arrhur個人Line uid 
            }
            lstBetpk10.Clear();
            ndBetpk10.Clear();
        }
        //定時執行程序
        private void btnTimer_Click(object sender, EventArgs e)
        {
            this._TimersTimer = new TimersTimer();
            this._TimersTimer.Interval = 1000 * 30;
            this._TimersTimer.Elapsed += new System.Timers.ElapsedEventHandler(_TimersTimer_Elapsed);
            this._TimersTimer.Start();
        }

        void _TimersTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Thread t = Thread.CurrentThread;
            bool IsThreadPool = t.IsThreadPoolThread;
            bool IsBackground = t.IsBackground;
            string msg = string.Format("Thread[{0}]:{1},Is ThreadPool=[{2}],Is Background=[{3}]", t.ManagedThreadId, t.ThreadState, IsThreadPool, IsBackground);

            //檢查目前時間
            DateTime time = e.SignalTime;

            if ((time.Minute == 33 || time.Minute == 53 || time.Minute == 13))
            {
                //重新載入所有tab資料
                reload();
                //跑xfyt
                this.getxfyt(strxyftnew6.Trim(), strxyfthis6.Trim(), strxyftnew8.Trim(), strxyfthis8.Trim(), strxyftnew4.Trim(), strxyfthis4.Trim());

                //跑pk10
                getpk10(strPK10new6.Trim(), strPK10his6.Trim(), strPK10new8.Trim(), strPK10his8.Trim(), strPk10new4.Trim(), strPk10his4.Trim());
            }



            if ((time.Minute == 4 || time.Minute == 8 || time.Minute == 12 || time.Minute == 16 || time.Minute == 20 || time.Minute == 24 || time.Minute == 28 || time.Minute == 32 || time.Minute == 36 || time.Minute == 40 || time.Minute == 44 || time.Minute == 48 || time.Minute == 52 || time.Minute == 56 || time.Minute == 58))
            {
                //重新載入所有tab資料
                reload();

                //跑xfyt
                this.getxfyt(strxyftnew6.Trim(), strxyfthis6.Trim(), strxyftnew8.Trim(), strxyfthis8.Trim(), strxyftnew4.Trim(), strxyfthis4.Trim());

                //跑pk10
                getpk10(strPK10new6.Trim(), strPK10his6.Trim(), strPK10new8.Trim(), strPK10his8.Trim(), strPk10new4.Trim(), strPk10his4.Trim());

            }

            if (time.Minute == 5 || time.Minute == 10 || time.Minute == 15 || time.Minute == 20 || time.Minute == 25 || time.Minute == 30 || time.Minute == 3  || time.Minute == 44 || time.Minute == 50 || time.Minute == 55 || time.Minute == 00)
            {
                //重新載入所有tab資料
                reload();
                //跑xfyt
                this.getxfyt(strxyftnew6.Trim(), strxyfthis6.Trim(), strxyftnew8.Trim(), strxyfthis8.Trim(), strxyftnew4.Trim(), strxyfthis4.Trim());

                //跑pk10
                getpk10(strPK10new6.Trim(), strPK10his6.Trim(), strPK10new8.Trim(), strPK10his8.Trim(), strPk10new4.Trim(), strPk10his4.Trim());
            }

            //this.listBox2.Items.Add(msg);
        }
        /// <summary>
        /// 重新呼叫api載入所有最新資料
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            reload();
        }

        /// <summary>
        /// 依據數據資料送出吻合的注單
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            //跑xfyt
            this.getxfyt(strxyftnew6.Trim(), strxyfthis6.Trim(), strxyftnew8.Trim(), strxyfthis8.Trim(), strxyftnew4.Trim(), strxyfthis4.Trim());

            //跑pk10
            getpk10(strPK10new6.Trim(), strPK10his6.Trim(), strPK10new8.Trim(), strPK10his8.Trim(), strPk10new4.Trim(), strPk10his4.Trim());
        }

        private void btngetHisData_Click(object sender, EventArgs e)
        {
            //刪除資料庫中舊有的舊歷史資料
            StringBuilder sbSQL = new StringBuilder();
            sbSQL.AppendFormat("delete hisData ");

            DBClass.ExecuteNonQuery(sbSQL.ToString());
            //刪除原有的歷史資料


            //xfyt歷史資料
            string html1 = strxyfthis6.Trim(); //六期歷史
            string html2 = strxyfthis8.Trim(); //八期歷史
            string html3 = strxyfthis4.Trim(); //4期歷史

            //pk10歷史資料
            string html4 = strPK10his6.Trim(); //六期歷史
            string html5 = strPK10his8.Trim(); //八期歷史
            string html6 = this.strPk10his4.Trim(); //4期歷史

            // 使用預設編碼讀入 HTML 
            HtmlAgilityPack.HtmlDocument htDoc = new HtmlAgilityPack.HtmlDocument();
            HtmlAgilityPack.HtmlDocument htDoc2 = new HtmlAgilityPack.HtmlDocument();
            HtmlAgilityPack.HtmlDocument htDoc3 = new HtmlAgilityPack.HtmlDocument();
            HtmlAgilityPack.HtmlDocument htDoc4 = new HtmlAgilityPack.HtmlDocument();
            HtmlAgilityPack.HtmlDocument htDoc5 = new HtmlAgilityPack.HtmlDocument();
            HtmlAgilityPack.HtmlDocument htDoc6 = new HtmlAgilityPack.HtmlDocument();


            htDoc.LoadHtml(html1);     //飛艇闖關計畫6關歷史期數
            htDoc2.LoadHtml(html2);    //飛艇闖關計畫8關歷史期數
            htDoc3.LoadHtml(html3);    //飛艇闖關計畫4關歷史期數
            htDoc4.LoadHtml(html4);    //北京闖關計畫6關歷史期數
            htDoc5.LoadHtml(html5);    //北京闖關計畫8關歷史期數
            htDoc6.LoadHtml(html6);    //北京闖關計畫4關歷史期數

            //回傳json格式資料
            string jsonfile1 = htDoc.DocumentNode.InnerText;
            string jsonfile2 = htDoc2.DocumentNode.InnerText;
            string jsonfile3 = htDoc3.DocumentNode.InnerText;
            string jsonfile4 = htDoc4.DocumentNode.InnerText;
            string jsonfile5 = htDoc5.DocumentNode.InnerText;
            string jsonfile6 = htDoc6.DocumentNode.InnerText;

            DataSet myDataSet1 = JsonConvert.DeserializeObject<DataSet>(jsonfile1);
            DataSet myDataSet2 = JsonConvert.DeserializeObject<DataSet>(jsonfile2);
            DataSet myDataSet3 = JsonConvert.DeserializeObject<DataSet>(jsonfile3);
            DataSet myDataSet4 = JsonConvert.DeserializeObject<DataSet>(jsonfile4);
            DataSet myDataSet5 = JsonConvert.DeserializeObject<DataSet>(jsonfile5);
            DataSet myDataSet6 = JsonConvert.DeserializeObject<DataSet>(jsonfile6);


            DataTable dt1 = myDataSet1.Tables[0];   //飛艇六期歷史數據 0:bigSmall  
            DataTable dt2 = myDataSet1.Tables[1];   //飛艇六期歷史數據 1:oddEven 
            DataTable dt3 = myDataSet2.Tables[0];   //飛艇八期歷史數據 0:bigSmal
            DataTable dt4 = myDataSet2.Tables[1];   //飛艇八期歷史數據 1:oddEven
            DataTable dt5 = myDataSet3.Tables[0];   //飛艇四期歷史數據 0:bigSmall 
            DataTable dt6 = myDataSet3.Tables[1];   //飛艇四期歷史數據 1:oddEven 
            DataTable dt7 = myDataSet4.Tables[0];   //pk10六期歷史數據 0:bigSmall  
            DataTable dt8 = myDataSet4.Tables[1];   //pk10六期歷史數據 1:oddEven
            DataTable dt9 = myDataSet5.Tables[0];   //pk10八期歷史數據 0:bigSmall  
            DataTable dt10 = myDataSet5.Tables[1];  //pk10八期歷史數據 1:oddEven 
            DataTable dt11 = myDataSet6.Tables[0];  //pk10四期歷史數據 0:bigSmall  
            DataTable dt12 = myDataSet6.Tables[1];  //pk10四期歷史數據 1:oddEven 

            Common.AddHistData(dt1, "6B", "xfyt");
            Common.AddHistData(dt2, "6D", "xfyt");
            Common.AddHistData(dt3, "8B", "xfyt");
            Common.AddHistData(dt4, "8D", "xfyt");
            Common.AddHistData(dt5, "4B", "xfyt");
            Common.AddHistData(dt6, "4D", "xfyt");

            Common.AddHistData(dt7, "6B", "pk10");
            Common.AddHistData(dt8, "6D", "pk10");
            Common.AddHistData(dt9, "8B", "pk10");
            Common.AddHistData(dt10, "8D", "pk10");
            Common.AddHistData(dt11, "4B", "pk10");
            Common.AddHistData(dt12, "4D", "pk10");
        }

    }
}
