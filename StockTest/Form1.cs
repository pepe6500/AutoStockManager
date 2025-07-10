using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.ExceptionServices;
using System.Security;
using System.Xml;
using System.IO;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Speech;
using System.Speech.Synthesis;
using static StockTest.StockChecker;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;
using static StockTest.Form1;
using static System.Runtime.CompilerServices.RuntimeHelpers;


namespace StockTest
{

    public partial class Form1 : Form
    {


        /// <summary>
        /// 장 시간 외 테스트를 위한 변수( 실사용시 false )
        /// </summary>
        bool test = false;
        

        public string user_id = null;
        bool err = false;
        int g_is_thread = 0; // 0이면 스레드 미생성, 1이면 스레드 생성
        Task thread1 = null; // 생성된 스레드 객체를 담을 변수
        Task threadtest = null; // 생성된 스레드 객체를 담을 변수
        Task timer = null; // 생성된 스레드 객체를 담을 변수
        bool flag = false;
        Object getinfo = null;
        public string g_user_id = null;
        string g_accnt_no = null; // 증권계좌번호
        string l_accno_cnt = "0"; // 소유한 증권계좌번호의 수
        string[] l_accno_arr = null; // N개의 증권계좌번호를 저장할 배열
        bool[] enabled_accno_arr = null;

        int g_cur_price = 0; // 현재가
        int g_is_next = 0; // 다음 조회 데이터가 있는지 확인
        int g_flag_1 = 0; // 1이면 요청에 대한 응답 완료
        int g_flag_2 = 0;
        int g_flag_3 = 0;
        int g_flag_4 = 0;
        int g_flag_5 = 0;
        int g_flag_6 = 0;// 현재가 조회 플래그
        int g_flag_7 = 0;
        int g_scr_no = 1000; // Open API 요청번호
        int g_ord_amt_possible = 0;
        int nowStockNum = 0;

        bool autoScrollLog = true;

        bool first = true;
        bool firstget = true;

        bool nowAuto = false;

        public string mesuSound = "";
        public string medoSound = "";
        public string pyunSound = "";
        public string cutSound = "";
        public string jumunerrorSound = "";

        public bool mesuSoundOn = false;
        public bool medoSoundOn = false;
        public bool pyunSoundOn = false;
        public bool cutSoundOn = false;
        public bool jumunerrorSoundOn = false;

        public bool mesuWinOn = false;
        public bool medoWinOn = false;
        public bool pyunWinOn = false;
        public bool cutWinOn = false;
        public bool jumunerrorWinOn = false;
        /// <summary>
        /// 주문 접수 정보(보유수량 계산용)
        /// </summary>
        List<JumunData> Jumuns = new List<JumunData>();

        /// <summary>
        /// 전체 감시 중
        /// </summary>
        public bool play = false;
        public bool allPlay = false;
        public bool allPlaySound = false;
        public bool allPlayMedo = true;
        public bool allPlayMesu = true;

        //기본값들
        public float sell_start = 1;           //매도하기 위한 최소 수익(시작가)
        public float sell_end = 10;           //매도하기 위한 최대 수익(종)
        public int sell_condition = 3;       //수익이 생긴 후 매도할 조건(떨어진 틱)
        public float buy_start = 1;            //매수하기 위한 최소 가격(시작가)
        public float buy_end = 10;            //매수하기 위한 최대 가격(종)
        public int buy_condition = 3;        //가격이 떨어진 후 매수할 조건(올라간 틱)

        public float sell_per = 100;
        public float buy_per = 100;
        public float highbuy_start = 1;
        public float highbuy_end = 10;
        public float highbuy_per = 100;
        public float highbuy_possible = 3;
        public int[] sell_conditions = new int[7];
        public int[] buy_conditions = new int[7];
        public int[] sell_kinds = new int[7];
        public int[] buy_kinds = new int[7];

        public bool is_sell = true;               //매도를 할 것인가
        public bool is_buy = true;                //매수를 할 것인가
        public bool is_cut = false;
        public float cut_price = 5;            //손절가( % )
        public int cut_count = 1;              //손절개수
        public float timedelay = 10f;
        public bool is_selltime = false;
        public bool is_buytime = false;
        public int targetPrice;
        public bool is_rebuy;                   //재매수
        public bool haveEffect;                 //유효매매
        public bool end_clear;                  //종료매매
        public bool end_clearCheck = false;             //종료매매확인
        public bool is_endLimit;                //상풀림매매
        public bool is_price_change_sound = false;//변동알림
        public int remainCountVal = 0;              //손절개수

        public bool end = false;                //자동실행용 종료확인
        public bool SaveEnd = false;            //자동실행용 종료확인
        public int lastDay;

        public int[] possible_Amount = null;

        /// <summary>
        /// 고매도가 실행중인지
        /// </summary>
        public bool is_highmedo;
        /// <summary>
        /// 고매도가 비중으로 실행중인지
        /// </summary>
        public bool is_highmedo_per;
        /// <summary>
        /// 고매도 퍼센트 값
        /// </summary>
        public float highMedo_perval;
        /// <summary>
        /// 고매도 가격 값
        /// </summary>
        public int highMedo_price;

        string logpath = "";
        public bool saveLog = true;
        public bool autoRestart = false;
        public bool autoBuy = false;

        public static string[] Kosdaq;
        public static string[] ETF;
        public static string[] ELW;
        public static string[] muchual;
        public static string[] sinjuinsu;
        public static string[] rich;
        public static string[] highld;
        public static string[] no3;

        WMPLib.WindowsMediaPlayer player;

        public List<StockChecker> stockCheckers = new List<StockChecker>();
        public ConditionProcess conditionProcess;
        public List<string> fixedList = new List<string>();

        public XmlData xmlData = new XmlData();

        public List<SoundSetting> soundSettings = new List<SoundSetting>();

        public Form12 conditionWindow = null;

        [DllImport("kernel32.dll")]
        public static extern bool Beep(int n, int m);    // n은 주파수, m은 소리내는 시간(단위: 1/1000초)

        public Form1()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            this.UpdateStyles();
            this.FormClosing += Form1_Closing;
            this.Text = "주식 프로그램 / " + CommUtil.BuildDate.Year + "." + CommUtil.BuildDate.Month + "." + CommUtil.BuildDate.Day;
        }

        private void SaveSettings()
        {
            try
            {
                Send_Log("설정 저장중..");


                xmlData.SetData("TEMP", "TEMP");
                xmlData.SetData("mesuSound", mesuSound);
                xmlData.SetData("medoSound", medoSound);
                xmlData.SetData("pyunSound", pyunSound);
                xmlData.SetData("cutSound", cutSound);
                xmlData.SetData("jumunerrorSound", jumunerrorSound);
                xmlData.SetData("mesuSoundOn", mesuSoundOn.ToString());
                xmlData.SetData("medoSoundOn", medoSoundOn.ToString());
                xmlData.SetData("pyunSoundOn", pyunSoundOn.ToString());
                xmlData.SetData("jumunerrorSoundOn", jumunerrorSoundOn.ToString());
                xmlData.SetData("cutSoundOn", cutSoundOn.ToString());
                xmlData.SetData("mesuWinOn", mesuWinOn.ToString());
                xmlData.SetData("medoWinOn", medoWinOn.ToString());
                xmlData.SetData("pyunWinOn", pyunWinOn.ToString());
                xmlData.SetData("cutWinOn", cutWinOn.ToString());
                xmlData.SetData("jumunerrorWinOn", jumunerrorWinOn.ToString());

                xmlData.SetData("sell_start", sell_start.ToString());
                xmlData.SetData("sell_end", sell_end.ToString());
                xmlData.SetData("sell_per", sell_per.ToString());
                xmlData.SetData("buy_start", buy_start.ToString());
                xmlData.SetData("buy_end", buy_end.ToString());
                xmlData.SetData("buy_per", buy_per.ToString());
                xmlData.SetData("highbuy_start", highbuy_start.ToString());
                xmlData.SetData("highbuy_end", highbuy_end.ToString());
                xmlData.SetData("highbuy_per", highbuy_per.ToString());

                xmlData.SetData("is_cut", is_cut.ToString());
                xmlData.SetData("cut_price", cut_price.ToString());
                xmlData.SetData("cut_count", cut_count.ToString());
                xmlData.SetData("is_sell", is_sell.ToString());
                xmlData.SetData("is_buy", is_buy.ToString());
                xmlData.SetData("is_highmedo", is_highmedo.ToString());
                xmlData.SetData("is_highmedo_per", is_highmedo_per.ToString());
                xmlData.SetData("highMedo_price", highMedo_price.ToString());
                xmlData.SetData("highMedo_perval", highMedo_perval.ToString());
                xmlData.SetData("end_clear", end_clear.ToString());
                xmlData.SetData("is_endLimit", is_endLimit.ToString());
                xmlData.SetData("haveEffect", haveEffect.ToString());
                xmlData.SetData("is_rebuy", is_rebuy.ToString());
                xmlData.SetData("is_selltime", is_selltime.ToString());
                xmlData.SetData("is_buytime", is_buytime.ToString());
                xmlData.SetData("autoRestart", autoRestart.ToString());
                xmlData.SetData("allPlaySound", allPlaySound.ToString());
                xmlData.SetData("saveLog", saveLog.ToString());
                xmlData.SetData("play", play.ToString());
                xmlData.SetData("targetPrice", targetPrice.ToString());
                xmlData.SetData("remainCountVal", remainCountVal.ToString());
                xmlData.SetData("is_price_change_sound", is_price_change_sound.ToString());



                for (int j = 0; j < sell_conditions.Length; j++)
                    xmlData.SetData("sell_conditions" + j, sell_conditions[j].ToString());
                for (int j = 0; j < buy_conditions.Length; j++)
                    xmlData.SetData("buy_conditions" + j, buy_conditions[j].ToString());



                for (int j = 0; j < enabled_accno_arr.Length; j++)
                    xmlData.SetData("enabled_accno_arr" + l_accno_arr[j], enabled_accno_arr[j].ToString());


                int fixednum = 0;
                int removecount = 0;
                while (true)
                {
                    if (xmlData.Remove("FIXED" + removecount) == -1)
                    {
                        break;
                    }
                    removecount++;
                }
                for (int i = 0; i < stockCheckers.Count; i++)
                {
                    //if (stockCheckers[i].state == StockChecker.StockState.selling || stockCheckers[i].state == StockChecker.StockState.sellingstart || stockCheckers[i].state == StockChecker.StockState.sellwait)
                    //{
                    //    xmlData.SetData(stockCheckers[i].name + ".state", ((int)StockChecker.StockState.sellwait).ToString());
                    //}
                    //else if (stockCheckers[i].state == StockChecker.StockState.buying || stockCheckers[i].state == StockChecker.StockState.buyingstart || stockCheckers[i].state == StockChecker.StockState.buywait)
                    //{
                    //    xmlData.SetData(stockCheckers[i].name + ".state", ((int)StockChecker.StockState.buywait).ToString());
                    //}
                    //else
                    //{
                    //    xmlData.SetData(stockCheckers[i].name + ".state", ((int)StockChecker.StockState.none).ToString());
                    //}

                    string keyString = "C" + stockCheckers[i].code + stockCheckers[i].accnt_no;

                    xmlData.SetData(keyString + ".targetPrice", stockCheckers[i].targetPrice.ToString());
                    xmlData.SetData(keyString + ".is_cut", stockCheckers[i].is_cut.ToString());
                    xmlData.SetData(keyString + ".cut_price", stockCheckers[i].cut_price.ToString());
                    for (int j = 0; j < stockCheckers[i].sell_conditions.Length; j++)
                        xmlData.SetData(keyString + ".sell_conditions" + j, stockCheckers[i].sell_conditions[j].ToString());
                    for (int j = 0; j < stockCheckers[i].buy_conditions.Length; j++)
                        xmlData.SetData(keyString + ".buy_conditions" + j, stockCheckers[i].buy_conditions[j].ToString());
                    //xmlData.SetData(stockCheckers[i].name + ".cut_price", stockCheckers[i].cut_price.ToString());
                    xmlData.SetData(keyString + ".is_sell", stockCheckers[i].is_sell.ToString());
                    xmlData.SetData(keyString + ".is_buy", stockCheckers[i].is_buy.ToString());
                    xmlData.SetData(keyString + ".end_clear", stockCheckers[i].end_clear.ToString());
                    xmlData.SetData(keyString + ".is_playing", stockCheckers[i].is_playing.ToString());
                    xmlData.SetData(keyString + ".is_highmedo", stockCheckers[i].is_highmedo.ToString());
                    xmlData.SetData(keyString + ".is_highmedo_per", stockCheckers[i].is_highmedo_per.ToString());
                    xmlData.SetData(keyString + ".highMedo_per", stockCheckers[i].highMedo_per.ToString());
                    xmlData.SetData(keyString + ".highMedo_price", stockCheckers[i].highMedo_price.ToString());
                    xmlData.SetData(keyString + ".haveEffect", stockCheckers[i].haveEffect.ToString());
                    xmlData.SetData(keyString + ".highMedo_perval", stockCheckers[i].highMedo_perval.ToString());
                    if (stockCheckers[i].is_fixed)
                    {
                        xmlData.SetData("FIXED" + fixednum, keyString);
                        fixednum++;
                    }
                    xmlData.SetData(keyString + ".is_endLimit", stockCheckers[i].is_endLimit.ToString());
                    xmlData.SetData(keyString + ".remainCountVal", stockCheckers[i].remainCountVal.ToString());
                    xmlData.SetData(keyString + ".sell_start", stockCheckers[i].sell_start.ToString());
                    xmlData.SetData(keyString + ".sell_end", stockCheckers[i].sell_end.ToString());
                    xmlData.SetData(keyString + ".sell_per", stockCheckers[i].sell_per.ToString());
                    xmlData.SetData(keyString + ".buy_start", stockCheckers[i].buy_start.ToString());
                    xmlData.SetData(keyString + ".buy_end", stockCheckers[i].buy_end.ToString());
                    xmlData.SetData(keyString + ".buy_per", stockCheckers[i].buy_per.ToString());
                    xmlData.SetData(keyString + ".highbuy_start", stockCheckers[i].highbuy_start.ToString());
                    xmlData.SetData(keyString + ".highbuy_end", stockCheckers[i].highbuy_end.ToString());
                    xmlData.SetData(keyString + ".highbuy_per", stockCheckers[i].highbuy_per.ToString());

                    xmlData.SetData(keyString + ".is_price_change_sound", stockCheckers[i].is_price_change_sound.ToString());
                    xmlData.SetData(keyString + ".targetCount", stockCheckers[i].targetCount.ToString());
                    xmlData.SetData(keyString + ".targetisprice", stockCheckers[i].targetisprice.ToString());
                    //xmlData.SetData(stockCheckers[i].name + ".is_repeat", stockCheckers[i].is_repeat.ToString());
                    //xmlData.SetData(stockCheckers[i].name + ".low_price", stockCheckers[i].low_price.ToString());
                    //xmlData.SetData(stockCheckers[i].name + ".high_price", stockCheckers[i].high_price.ToString());
                }

                for (int i = 0; i < soundSettings.Count; i++)
                {
                    xmlData.SetData("Sound" + soundSettings[i].name + ".path", soundSettings[i].path);
                    xmlData.SetData("Sound" + soundSettings[i].name + ".sound", soundSettings[i].sound.ToString());
                    xmlData.SetData("Sound" + soundSettings[i].name + ".window", soundSettings[i].window.ToString());
                    xmlData.SetData("Sound" + soundSettings[i].name + ".highLight", soundSettings[i].highLight.ToString());
                }

                for (int i = 0; i < conditionProcess.onPlayConditions.Length; i++)
                {
                    if (conditionProcess.onPlayConditions[i] == null)
                    {
                        xmlData.SetData("ConditiononPlayConditions" + i, "none");
                        continue;
                    }
                    xmlData.SetData("ConditiononPlayConditions" + i, conditionProcess.onPlayConditions[i].name);
                    xmlData.SetData("Condition" + conditionProcess.onPlayConditions[i].name + ".isPlay", conditionProcess.onPlayConditions[i].isPlay.ToString());
                    xmlData.SetData("Condition" + conditionProcess.onPlayConditions[i].name + ".startTime.hour", conditionProcess.onPlayConditions[i].startTime.hour.ToString());
                    xmlData.SetData("Condition" + conditionProcess.onPlayConditions[i].name + ".startTime.min", conditionProcess.onPlayConditions[i].startTime.min.ToString());
                    xmlData.SetData("Condition" + conditionProcess.onPlayConditions[i].name + ".startTime.sec", conditionProcess.onPlayConditions[i].startTime.sec.ToString());
                    xmlData.SetData("Condition" + conditionProcess.onPlayConditions[i].name + ".endTime.hour", conditionProcess.onPlayConditions[i].endTime.hour.ToString());
                    xmlData.SetData("Condition" + conditionProcess.onPlayConditions[i].name + ".endTime.min", conditionProcess.onPlayConditions[i].endTime.min.ToString());
                    xmlData.SetData("Condition" + conditionProcess.onPlayConditions[i].name + ".endTime.sec", conditionProcess.onPlayConditions[i].endTime.sec.ToString());
                    xmlData.SetData("Condition" + conditionProcess.onPlayConditions[i].name + ".maxBuyCount", conditionProcess.onPlayConditions[i].maxBuyCount.ToString());
                    xmlData.SetData("Condition" + conditionProcess.onPlayConditions[i].name + ".buyMoney", conditionProcess.onPlayConditions[i].buyMoney.ToString());
                    xmlData.SetData("Condition" + conditionProcess.onPlayConditions[i].name + ".buyType", conditionProcess.onPlayConditions[i].buyType.ToString());
                    xmlData.SetData("Condition" + conditionProcess.onPlayConditions[i].name + ".minValue", conditionProcess.onPlayConditions[i].minValue.ToString());
                    xmlData.SetData("Condition" + conditionProcess.onPlayConditions[i].name + ".addConditionIndex", conditionProcess.onPlayConditions[i].addConditionIndex.ToString());
                    xmlData.SetData("Condition" + conditionProcess.onPlayConditions[i].name + ".linked", conditionProcess.onPlayConditions[i].linked);
                }


                xmlData.SetData("Condition_is_playing", conditionProcess.is_playing.ToString());
                xmlData.SetData("Condition_is_cut", conditionProcess.is_cut.ToString());
                xmlData.SetData("Condition_cut_price", conditionProcess.cut_price.ToString());
                xmlData.SetData("Condition_cut_count", conditionProcess.cut_count.ToString());
                xmlData.SetData("Condition_is_sell", conditionProcess.is_sell.ToString());
                xmlData.SetData("Condition_is_buy", conditionProcess.is_buy.ToString());
                xmlData.SetData("Condition_is_highmedo", conditionProcess.is_highmedo.ToString());
                xmlData.SetData("Condition_is_highmedo_per", conditionProcess.is_highmedo_per.ToString());
                xmlData.SetData("Condition_highMedo_price", conditionProcess.highMedo_price.ToString());
                xmlData.SetData("Condition_highMedo_perval", conditionProcess.highMedo_perval.ToString());
                xmlData.SetData("Condition_end_clear", conditionProcess.end_clear.ToString());
                xmlData.SetData("Condition_is_endLimit", conditionProcess.is_endLimit.ToString());
                xmlData.SetData("Condition_haveEffect", conditionProcess.haveEffect.ToString());
                xmlData.SetData("Condition_is_rebuy", conditionProcess.is_rebuy.ToString());
                xmlData.SetData("Condition_is_selltime", conditionProcess.is_selltime.ToString());
                xmlData.SetData("Condition_is_buytime", conditionProcess.is_buytime.ToString());
                xmlData.SetData("Condition_sell_start", conditionProcess.sell_start.ToString());
                xmlData.SetData("Condition_sell_end", conditionProcess.sell_end.ToString());
                xmlData.SetData("Condition_sell_per", conditionProcess.sell_per.ToString());
                xmlData.SetData("Condition_buy_start", conditionProcess.buy_start.ToString());
                xmlData.SetData("Condition_buy_end", conditionProcess.buy_end.ToString());
                xmlData.SetData("Condition_buy_per", conditionProcess.buy_per.ToString());
                xmlData.SetData("Condition_highbuy_start", conditionProcess.highbuy_start.ToString());
                xmlData.SetData("Condition_highbuy_end", conditionProcess.highbuy_end.ToString());
                xmlData.SetData("Condition_highbuy_per", conditionProcess.highbuy_per.ToString());
                xmlData.SetData("Condition_is_price_change_sound", conditionProcess.is_price_change_sound.ToString());
                xmlData.SetData("Condition_targetPrice", conditionProcess.targetPrice.ToString());
                xmlData.SetData("Condition_remainCountVal", conditionProcess.remainCountVal.ToString());
                xmlData.SetData("Condition_is_highbuy", conditionProcess.is_highbuy.ToString());

                for (int i = 0; i < conditionProcess.sell_conditions.Length; i++)
                {
                    xmlData.SetData("Condition_sell_conditions" + i, conditionProcess.sell_conditions[i].ToString());
                    xmlData.SetData("Condition_buy_conditions" + i, conditionProcess.buy_conditions[i].ToString());
                    xmlData.SetData("Condition_sell_kinds" + i, conditionProcess.sell_kinds[i].ToString());
                    xmlData.SetData("Condition_buy_kinds" + i, conditionProcess.buy_kinds[i].ToString());
                }


                if (xmlData != null)
                    xmlData.bSaveXML();

                if (thread1 != null)
                    thread1.Dispose();
                if (threadtest != null)
                    threadtest.Dispose();
                axKHOpenAPI1.Dispose();
            }
            catch (Exception ex)
            {
                Send_Log_Debug("설정 저장 오류 : " + ex.Message + ex.StackTrace);
            }

            xmlData.bSaveXML();
            Send_Log("설정 저장 완료");
        }

        private void Form1_Closing(object sender, EventArgs e)
        {
            SaveSettings();
        }

        private void Form1_Load_1_Thread()
        {

        }

        public void LoadSoundSetting(string name)
        {
            for (int i = 0; i < soundSettings.Count; i++)
            {
                if (soundSettings[i].name == name)
                    return;
            }

            SoundSetting soundSetting = new SoundSetting();
            soundSetting.name = name;

            if (xmlData.Find("Sound" + name + ".path") != null)
            {
                soundSetting.path = xmlData.Find("Sound" + name + ".path").value;
                soundSetting.sound = bool.Parse(xmlData.Find("Sound" + name + ".sound").value);
                soundSetting.window = bool.Parse(xmlData.Find("Sound" + name + ".window").value);
                soundSetting.highLight = bool.Parse(xmlData.Find("Sound" + name + ".highLight").value);
            }
            else
            {
                soundSetting.path = "";
                soundSetting.sound = false;
                soundSetting.window = false;
                soundSetting.highLight = false;
            }

            soundSettings.Add(soundSetting);
            return;
        }

        void TextBoxKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button13_Click(null, null);
            }
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            //((Label)(panel1.Controls[0])).Text = "aaaa";
            //프로그램 시작시 로그인 창 호출
            axKHOpenAPI1.CommConnect();
            conditionProcess = new ConditionProcess(this);
            if (Directory.Exists(Path.GetDirectoryName(Application.ExecutablePath) + "\\LOGS") == false)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(Application.ExecutablePath) + "\\LOGS");
            }

            logpath = Path.GetDirectoryName(Application.ExecutablePath) + "\\LOGS\\LOG_" + DateTime.Now.Year + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second + ".txt";
            System.IO.File.WriteAllText(logpath, "", Encoding.Default);

            axKHOpenAPI1.OnEventConnect += this.axKHOpenAPI1_OnEventConnect;
            axKHOpenAPI1.OnReceiveTrData += this.axKHOpenAPI1_OnReceiveTrData;
            axKHOpenAPI1.OnReceiveMsg += this.axKHOpenAPI1_OnReceiveMsg;
            axKHOpenAPI1.OnReceiveChejanData += this.axKHOpenAPI1_OnReceiveChejanData;
            axKHOpenAPI1.OnReceiveRealData += this.axKHOpenAPI1_OnReceiveRealData;
            axKHOpenAPI1.OnReceiveConditionVer += OnReceiveConditionVer;

            button14_Click(null, null);
            textBox2.KeyDown += TextBoxKeyDown;

            g_is_thread = 1; // 스레드 생성으로 값 설정

            if (thread1 == null)
                thread1 = Task.Factory.StartNew(new Action(m_thread1));
            if (timer == null)
                timer = Task.Factory.StartNew(new Action(Timer));
            log_msg.Text = "로그인 대기";

            button8.Text = "매도 실행중";
            button8.BackColor = Color.FromArgb(255, 210, 210);
            button9.Text = "매수 실행중";
            button9.BackColor = Color.FromArgb(255, 210, 210);
            button5.BackColor = Color.FromArgb(210, 210, 255);
            //this.FormBorderStyle = FormBorderStyle.FixedSingle;

            for (int i = 0; i < sell_conditions.Length; i++)
            {
                sell_conditions[i] = 3;
                buy_conditions[i] = 3;
                conditionProcess.sell_conditions[i] = 3;
                conditionProcess.buy_conditions[i] = 3;
            }
            dataGridView1.Rows.Add(20);
        }

        public void Logined()
        {
            Send_Log("로그인 완료");
            SetLogmsg("로그인 완료");
            g_user_id = axKHOpenAPI1.GetLoginInfo("USER_ID").Trim();    // 사용자 아이디를 가져와서 클래스 변수에 저장
            textBox1.Text = g_user_id;                                  // 클래스 변수에 저장한 아이디를 텍스트박스에 출력
            l_accno_cnt = axKHOpenAPI1.GetLoginInfo("ACCOUNT_CNT").Trim(); // 사용자의 증권계좌번호 수를 가져옴
            int accno_cnt = int.Parse(l_accno_cnt);
            l_accno_arr = new string[accno_cnt];
            enabled_accno_arr = new bool[accno_cnt];
            possible_Amount = new int[accno_cnt];

            string accnt_nos = axKHOpenAPI1.GetLoginInfo("ACCNO").Trim();   // 증권계좌번호 가져옴
            string[] accnt_array = accnt_nos.Split(';');
            for (int i = 0; i < l_accno_arr.Length; i++)
            {
                l_accno_arr[i] = accnt_array[i].Trim();
            }

            g_accnt_no = l_accno_arr[0].Trim();

            xmlData.main = this;
            xmlData.bLoadXML();

            //SelectAutoBuy selectAutoBuy = new SelectAutoBuy(this);
            //selectAutoBuy.Show();

            if (xmlData.Find("mesuSound") != null)
            {
                try
                {
                    mesuSound = xmlData.Find("mesuSound").value;
                    medoSound = xmlData.Find("medoSound").value;
                    pyunSound = xmlData.Find("pyunSound").value;
                    cutSound = xmlData.Find("cutSound").value;
                    jumunerrorSound = xmlData.Find("jumunerrorSound").value;
                    mesuSoundOn = bool.Parse(xmlData.Find("mesuSoundOn").value);
                    medoSoundOn = bool.Parse(xmlData.Find("medoSoundOn").value);
                    pyunSoundOn = bool.Parse(xmlData.Find("pyunSoundOn").value);
                    cutSoundOn = bool.Parse(xmlData.Find("cutSoundOn").value);
                    jumunerrorSoundOn = bool.Parse(xmlData.Find("jumunerrorSoundOn").value);
                    mesuWinOn = bool.Parse(xmlData.Find("mesuWinOn").value);
                    medoWinOn = bool.Parse(xmlData.Find("medoWinOn").value);
                    pyunWinOn = bool.Parse(xmlData.Find("pyunWinOn").value);
                    cutWinOn = bool.Parse(xmlData.Find("cutWinOn").value);
                    jumunerrorWinOn = bool.Parse(xmlData.Find("jumunerrorWinOn").value);
                    is_cut = bool.Parse(xmlData.Find("is_cut").value);
                    cut_price = float.Parse(xmlData.Find("cut_price").value);
                    cut_count = int.Parse(xmlData.Find("cut_count").value);
                    is_sell = bool.Parse(xmlData.Find("is_sell").value);
                    is_buy = bool.Parse(xmlData.Find("is_buy").value);
                    is_highmedo = bool.Parse(xmlData.Find("is_highmedo").value);
                    is_highmedo_per = bool.Parse(xmlData.Find("is_highmedo_per").value);
                    highMedo_price = int.Parse(xmlData.Find("highMedo_price").value);
                    highMedo_perval = float.Parse(xmlData.Find("highMedo_perval").value);
                    end_clear = bool.Parse(xmlData.Find("end_clear").value);
                    is_endLimit = bool.Parse(xmlData.Find("is_endLimit").value);
                    haveEffect = bool.Parse(xmlData.Find("haveEffect").value);
                    is_rebuy = bool.Parse(xmlData.Find("is_rebuy").value);
                    is_selltime = bool.Parse(xmlData.Find("is_selltime").value);
                    is_buytime = bool.Parse(xmlData.Find("is_buytime").value);
                    autoRestart = bool.Parse(xmlData.Find("autoRestart").value);
                    saveLog = bool.Parse(xmlData.Find("saveLog").value);

                    sell_start = float.Parse(xmlData.Find("sell_start").value);
                    sell_end = float.Parse(xmlData.Find("sell_end").value);
                    sell_per = float.Parse(xmlData.Find("sell_per").value);
                    buy_start = float.Parse(xmlData.Find("buy_start").value);
                    buy_end = float.Parse(xmlData.Find("buy_end").value);
                    buy_per = float.Parse(xmlData.Find("buy_per").value);
                    highbuy_start = float.Parse(xmlData.Find("highbuy_start").value);
                    highbuy_end = float.Parse(xmlData.Find("highbuy_end").value);
                    highbuy_per = float.Parse(xmlData.Find("highbuy_per").value);
                    allPlaySound = bool.Parse(xmlData.Find("allPlaySound").value);
                    is_price_change_sound = bool.Parse(xmlData.Find("is_price_change_sound").value);
                    if (allPlaySound)
                    {
                        AllStartSound();
                    }
                    else
                    {
                        AllStopSound();
                    }

                    for (int ii = 0; ii < buy_conditions.Length; ii++)
                    {
                        buy_conditions[ii] = int.Parse(xmlData.Find("buy_conditions" + ii).value.Trim());
                    }
                    for (int i = 0; i < sell_conditions.Length; i++)
                    {
                        sell_conditions[i] = int.Parse(xmlData.Find("sell_conditions" + i).value.Trim());
                    }

                    targetPrice = int.Parse(xmlData.Find("targetPrice").value);

                    play = bool.Parse(xmlData.Find("play").value);

                    for (int i = 0; i < enabled_accno_arr.Length; i++)
                        enabled_accno_arr[i] = bool.Parse(xmlData.Find("enabled_accno_arr" + l_accno_arr[i]).value);

                    remainCountVal = int.Parse(xmlData.Find("remainCountVal").value);
                }
                catch (Exception e)
                {

                    Send_Log("XML: " + e.Message + e.StackTrace);
                }
            }
            if (xmlData.Find("AutoStart") != null)
                nowAuto = bool.Parse(xmlData.Find("AutoStart").value);
            if (xmlData.Find("Condition_sell_start") != null)
            {
                try
                {
                    conditionProcess.is_playing = bool.Parse(xmlData.Find("Condition_is_playing").value);
                    conditionProcess.is_cut = bool.Parse(xmlData.Find("Condition_is_cut").value);
                    conditionProcess.cut_price = float.Parse(xmlData.Find("Condition_cut_price").value);
                    conditionProcess.cut_count = int.Parse(xmlData.Find("Condition_cut_count").value);
                    conditionProcess.is_sell = bool.Parse(xmlData.Find("Condition_is_sell").value);
                    conditionProcess.is_buy = bool.Parse(xmlData.Find("Condition_is_buy").value);
                    conditionProcess.is_highmedo = bool.Parse(xmlData.Find("Condition_is_highmedo").value);
                    conditionProcess.is_highmedo_per = bool.Parse(xmlData.Find("Condition_is_highmedo_per").value);
                    conditionProcess.highMedo_price = int.Parse(xmlData.Find("Condition_highMedo_price").value);
                    conditionProcess.highMedo_perval = float.Parse(xmlData.Find("Condition_highMedo_perval").value);
                    conditionProcess.end_clear = bool.Parse(xmlData.Find("Condition_end_clear").value);
                    conditionProcess.is_endLimit = bool.Parse(xmlData.Find("Condition_is_endLimit").value);
                    conditionProcess.haveEffect = bool.Parse(xmlData.Find("Condition_haveEffect").value);
                    conditionProcess.is_rebuy = bool.Parse(xmlData.Find("Condition_is_rebuy").value);
                    conditionProcess.is_selltime = bool.Parse(xmlData.Find("Condition_is_selltime").value);
                    conditionProcess.is_buytime = bool.Parse(xmlData.Find("Condition_is_buytime").value);

                    conditionProcess.sell_start = float.Parse(xmlData.Find("Condition_sell_start").value);
                    conditionProcess.sell_end = float.Parse(xmlData.Find("Condition_sell_end").value);
                    conditionProcess.sell_per = float.Parse(xmlData.Find("Condition_sell_per").value);
                    conditionProcess.buy_start = float.Parse(xmlData.Find("Condition_buy_start").value);
                    conditionProcess.buy_end = float.Parse(xmlData.Find("Condition_buy_end").value);
                    conditionProcess.buy_per = float.Parse(xmlData.Find("Condition_buy_per").value);
                    conditionProcess.highbuy_start = float.Parse(xmlData.Find("Condition_highbuy_start").value);
                    conditionProcess.highbuy_end = float.Parse(xmlData.Find("Condition_highbuy_end").value);
                    conditionProcess.highbuy_per = float.Parse(xmlData.Find("Condition_highbuy_per").value);
                    conditionProcess.is_price_change_sound = bool.Parse(xmlData.Find("Condition_is_price_change_sound").value);
                    conditionProcess.targetPrice = int.Parse(xmlData.Find("Condition_targetPrice").value);
                    conditionProcess.remainCountVal = int.Parse(xmlData.Find("Condition_remainCountVal").value);
                    conditionProcess.is_highbuy = bool.Parse(xmlData.Find("Condition_is_highbuy").value);

                    for (int i = 0; i < conditionProcess.sell_conditions.Length; i++)
                    {
                        conditionProcess.sell_conditions[i] = int.Parse(xmlData.Find("Condition_sell_conditions" + i).value.Trim());
                        conditionProcess.buy_conditions[i] = int.Parse(xmlData.Find("Condition_buy_conditions" + i).value.Trim());
                        conditionProcess.sell_kinds[i] = int.Parse(xmlData.Find("Condition_sell_kinds" + i).value.Trim());
                        conditionProcess.buy_kinds[i] = int.Parse(xmlData.Find("Condition_buy_kinds" + i).value.Trim());
                    }
                }
                catch (Exception ex)
                {
                    Send_Log_Debug("설정 저장 오류 : " + ex.Message + ex.StackTrace);
                }
            }

            try
            {
                xmlData.SetData("AutoStart", "false");

                LoadSoundSetting("매도");
                LoadSoundSetting("상승매수");
                LoadSoundSetting("매수");
                LoadSoundSetting("손절");
                LoadSoundSetting("고가매도");
                LoadSoundSetting("종료매매");
                LoadSoundSetting("편입");
                LoadSoundSetting("예수금부족");
                LoadSoundSetting("VI발동");
                LoadSoundSetting("상한가");
                LoadSoundSetting("하한가");
                LoadSoundSetting("매수못함");
                LoadSoundSetting("매도못함");
                LoadSoundSetting("목표채움");
                LoadSoundSetting("목표초과");
                LoadSoundSetting("상승매수풀림");
                LoadSoundSetting("잔고제로");
                LoadSoundSetting("1퍼상승때마다");
                LoadSoundSetting("1퍼하락때마다");
                LoadSoundSetting("수익전환");
                LoadSoundSetting("손실전환");
                LoadSoundSetting("수익5퍼");
                LoadSoundSetting("손실5퍼");
                LoadSoundSetting("수익10퍼");
                LoadSoundSetting("손실10퍼");
                LoadSoundSetting("오류");
            }
            catch (Exception e)
            {
                Send_Log_Debug("알림 설정 불러오기 실패 : " + e.Message + e.StackTrace);
            }

            int j = 0;
            while (true)
            {
                if (xmlData.Find("FIXED" + j) == null)
                {
                    break;
                }
                fixedList.Add(xmlData.Find("FIXED" + j).value);
                j++;
            }
            int conres = axKHOpenAPI1.GetConditionLoad();
            //조건검색 목록 요청
            if (conres > 0)
            {
                Send_Log_Debug("조건검색 목록 요청 성공");
            }
            else
            {
                Send_Log_Debug("조건검색 목록 요청 실패 : " + conres);
            }

            Updateg_accnt_no();
            buttonAccountNumber.Text = g_accnt_no;

            string enabledAccountNumbersString = "";
            for (int i = 0; i < l_accno_arr.Length; i++)
            {
                if (enabled_accno_arr[i] == true)
                {
                    enabledAccountNumbersString += l_accno_arr[i];
                    if (i + 1 < l_accno_arr.Length)
                        enabledAccountNumbersString += ", ";
                }
            }
            Send_Log("사용할 증권계좌번호는 : [" + enabledAccountNumbersString + "] 입니다. \n");

        }


        public async void Timer()
        {
            while (true)
            {
                if (allPlay && !end_clearCheck)
                {
                    if (DateTime.Now.Hour == 15 && DateTime.Now.Minute == 19)
                    {
                        end_clearCheck = true;
                        for (int i = 0; i < stockCheckers.Count; i++)
                        {
                            if (stockCheckers[i].end_clear && stockCheckers[i].is_playing && stockCheckers[i].PriceMoveTick(stockCheckers[i].price.price, 1) <= stockCheckers[i].limitprice)
                            {
                                Send_Log(stockCheckers[i].name + " 종료매매");
                                if (stockCheckers[i].state == StockChecker.StockState.buywait)
                                {
                                    stockCheckers[i].Mesu("");
                                }
                                else if (stockCheckers[i].state == StockChecker.StockState.sellwait)
                                {
                                    stockCheckers[i].Medo("");
                                }
                            }
                        }
                    }
                }
                if (autoRestart)
                {
                    if ((DateTime.Now.Hour >= 16 || DateTime.Now.Hour <= 7) && !end && axKHOpenAPI1.GetConnectState() == 1)
                    {
                        end = true;
                    }
                    if (end)
                    {
                        if (DateTime.Now.Hour == 8)
                        {
                            xmlData.SetData("AutoStart", "true");

                            SaveEnd = false;
                            Form1_Closing(this, null);

                            while (true)
                            {
                                if (SaveEnd)
                                {
                                    break;
                                }
                                else
                                {
                                    await Task.Delay(1000);
                                }
                            }

                            System.Diagnostics.Process.Start(Application.ExecutablePath); // to start new instance of application
                            System.Diagnostics.Process.GetCurrentProcess().Kill();
                            return;
                        }
                    }
                }

                try
                {
                    this.BeginInvoke(new Action(() =>
                    {
                        Send_Log(logbox.Text.Length.ToString());
                        this.Text = "주식 프로그램 / " + CommUtil.BuildDate.Year + "." + CommUtil.BuildDate.Month + "." + CommUtil.BuildDate.Day + " /      " + DateTime.Now + ":" + DateTime.Now.Millisecond / 100;
                    }
                    ));
                    await Task.Delay(100);
                }
                catch
                {
                    await Task.Delay(100);
                    continue;
                }
            }
        }

        /// <summary>
        /// 테스트 임시 가격
        /// </summary>
        public async void m_threadtest()
        {
            while (true)
            {
                for (int i = 0; i < stockCheckers.Count; i++)
                {
                    if (stockCheckers[i].code == "035720")
                    {
                        this.BeginInvoke(new Action(() =>
                        {
                            int pri = (int)(20000 + Math.Sin((DateTime.Now.Ticks % 10000000000) / 250000000f) * 2000);
                            pri -= pri % 50;
                            for (int j = 0; j < 10; j++)
                            {
                                stockCheckers[i].gridView.Rows[j].Cells[0].Value = string.Format("{0:#,###}", stockCheckers[i].PriceMoveTick(pri, 10 - j));
                                stockCheckers[i].prices[j] = stockCheckers[i].PriceMoveTick(pri, 10 - j);
                                stockCheckers[i].gridView.Rows[10 + j].Cells[0].Value = string.Format("{0:#,###}", stockCheckers[i].PriceMoveTick(pri, -j));
                                stockCheckers[i].prices[10 + j] = stockCheckers[i].PriceMoveTick(pri, -j);
                                stockCheckers[i].AddPriceData(pri, DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second);
                            }
                        }
                        ));
                        break;
                    }
                }

                await Task.Delay(100);
            }
        }

        /// <summary>
        /// 실시간 등록 테스트 
        /// </summary>
        public void m_threadrealdata(object code)
        {
            string scr = get_scr_no();
            int num = axKHOpenAPI1.SetRealReg(scr, (string)code, "20;41;10;951;", "1");
            List<StockChecker> stocks = stockCheckers.FindAll(a => a.code == (string)code);

            foreach (StockChecker stock in stocks)
            {
                if (stock != null)
                {
                    if (stock.realtime != "")
                    {
                        axKHOpenAPI1.DisconnectRealData(stock.realtime);
                        stock.realtime = "";
                    }

                    if (num == 0)
                    {
                        stock.realtime = scr;
                        break;
                    }
                    else
                    {
                        Send_Log(code + " 등록실패: " + num);
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
        }


        public async void m_thread1()
        {
            try
            {
                if (g_is_thread == 0) // 최초 스레드 생성
                {
                    g_is_thread = 1; // 중복 스레드 생성 방지
                }

                //로그인 대기
                SetLogmsg("로그인 대기 중");
                int ret2 = 0;
                while (!err)
                {
                    ret2 = axKHOpenAPI1.GetConnectState();
                    if (ret2 == 1 && g_flag_1 == 1)
                    {
                        break;
                    }
                    else
                    {
                        await Task.Delay(500);
                        //delay(1000);
                    }
                }

                string codeList = axKHOpenAPI1.GetCodeListByMarket("10");
                Kosdaq = codeList.Split(';');
                codeList = axKHOpenAPI1.GetCodeListByMarket("8");
                ETF = codeList.Split(';');
                codeList = axKHOpenAPI1.GetCodeListByMarket("3");
                ELW = codeList.Split(';');
                codeList = axKHOpenAPI1.GetCodeListByMarket("4");
                muchual = codeList.Split(';');
                codeList = axKHOpenAPI1.GetCodeListByMarket("5");
                sinjuinsu = codeList.Split(';');
                codeList = axKHOpenAPI1.GetCodeListByMarket("6");
                rich = codeList.Split(';');
                codeList = axKHOpenAPI1.GetCodeListByMarket("9");
                highld = codeList.Split(';');
                codeList = axKHOpenAPI1.GetCodeListByMarket("30");
                no3 = codeList.Split(';');

                if (test)
                {
                    if (threadtest == null)
                        threadtest = Task.Factory.StartNew(new Action(Timer));
                }

                axKHOpenAPI1.SetInputValue("계좌번호", g_accnt_no);
                axKHOpenAPI1.SetInputValue("비밀번호", "");
                axKHOpenAPI1.SetInputValue("상장폐지조회구분", "1");
                axKHOpenAPI1.SetInputValue("비밀번호입력매체구분", "00");
                string stockCodeList = "";
                for (int i = 0; i < fixedList.Count; i++)
                {
                    stockCodeList += fixedList[i];
                    if (i + 1 < fixedList.Count)
                    {
                        stockCodeList += ";";
                    }
                }
                if (stockCodeList != "")
                    axKHOpenAPI1.CommKwRqData(stockCodeList, 0, fixedList.Count, 0, "관심종목정보요청", get_scr_no());


                //if (nowAuto == true)
                if (true) // 기본 자동실행
                {
                    Send_Log("자동실행");
                    AllStart();
                }

                for (; ; ) // 첫 번째 무한루프 시작
                {
                    int l_for_cnt = 0;
                    int l_for_flag = 0;
                    g_is_next = 0;

                    for (; ; )
                    {
                        l_for_flag = 0;
                        for (; ; )
                        {
                            try
                            {
                                g_flag_2 = 0;
                                // 보유 중인 모든 계좌 정보 요청
                                for (int i = 0; i < l_accno_arr.Length; i++)
                                {
                                    // 활성화되지 않은 계좌 제외
                                    if (enabled_accno_arr[i] == false)
                                        continue;

                                    string current_accnt_no = l_accno_arr[i];

                                    // 잘못된 계좌번호에 대한 요청을 보내지 않도록 막음
                                    if (current_accnt_no.Length != 10)
                                    {
                                        continue;
                                    }

                                    axKHOpenAPI1.SetInputValue("계좌번호", current_accnt_no);
                                    axKHOpenAPI1.SetInputValue("비밀번호", "");
                                    axKHOpenAPI1.SetInputValue("상장폐지조회구분", "1");
                                    axKHOpenAPI1.SetInputValue("비밀번호입력매체구분", "00");

                                    // 계좌정보 데이터 수신 요청
                                    Send_Log_Debug("계좌정보 수신 요청 중: " + current_accnt_no);
                                    string sRQName = "계좌평가현황요청," + current_accnt_no.ToString();
                                    axKHOpenAPI1.CommRqData(sRQName, "OPW00004", g_is_next, get_scr_no());

                                    await Task.Delay(500);
                                }
                                l_for_cnt = 0;
                            }
                            catch (Exception e)
                            {
                                PlayEffect(25);
                                Send_Log("m_thread1():axKHOpenAPI1.SetInputValue [" + e.Message + "]");
                            }
                            for (; ; )
                            {
                                if (g_flag_2 == 1)
                                {
                                    await Task.Delay(1000);
                                    l_for_flag = 1;
                                    break;
                                }
                                else
                                {
                                    l_for_cnt++;
                                    if (l_for_cnt == 5)
                                    {
                                        l_for_flag = 0;
                                        break;
                                    }
                                    else
                                    {
                                        await Task.Delay(1000);
                                        //delay(2000);
                                        continue;
                                    }
                                }
                            }
                            if (l_for_flag == 1)
                            {
                                break;
                            }
                            else if (l_for_flag == 0)
                            {
                                await Task.Delay(2000);
                                continue;
                            }

                            CutLogBoxByLength();
                        }
                        if (g_is_next == 0)
                        {
                            break;
                        }
                        //delay(4000);
                    }
                    l_for_flag = 0;
                    g_cur_price = 0;



                    await Task.Delay(60000);
                    //delay(200); // 첫 번째 무한루프 지연
                }
            }
            catch (System.Exception e)
            {
                try
                {
                    PlayEffect(25);
                    Send_Log("m_thread1() " + e.HResult + " [" + e.Message + "]");
                }
                catch
                {

                }
            }
        }

        public AxKHOpenAPILib.AxKHOpenAPI GetAPI()
        {
            return axKHOpenAPI1;
        }

        public void Restart()
        {
            allPlay = true;
        }


        // 이미 불러온 정보라면 수정만 한 후 false 반환
        // 새로 받는 정보라면 생성 후 true 반환
        public int insert_tb_accnt_info(string i_jongmok_cd, string i_jongmok_nm, string i_accnt_no, int i_buy_price, int i_own_stock_cnt, int now_price)
        {
            try
            {
                if (i_own_stock_cnt < 0 || now_price <= 0 || i_buy_price < 0)
                {
                    return -1;
                }
                i_jongmok_cd = RealCode(i_jongmok_cd);
                for (int i = 0; i < stockCheckers.Count; i++)
                {
                    if (stockCheckers[i].isThis(i_jongmok_cd, i_accnt_no))
                    {
                        stockCheckers[i].realprice = i_buy_price;
                        stockCheckers[i].count = i_own_stock_cnt;
                        stockCheckers[i].SetCountColor();
                        if (!test)
                            stockCheckers[i].AddPriceData(now_price, "");

                        ((Label)stockCheckers[i].pan.Controls["label8"]).Text = "보유수량: " + i_own_stock_cnt;

                        return i;
                    }
                }
                StockChecker newStockChecker = new StockChecker(i_jongmok_cd, i_jongmok_nm, i_accnt_no, i_own_stock_cnt, this);
                newStockChecker.index = stockCheckers.Count;
                newStockChecker.realprice = i_buy_price;
                newStockChecker.all_high_price = now_price;
                newStockChecker.price.price = now_price;
                axKHOpenAPI1.SetInputValue("종목코드", i_jongmok_cd);
                if (newStockChecker.index == 0)
                {
                    newStockChecker.pan = (Panel)this.Controls["flowLayoutPanel1"].Controls["panel1"];
                    newStockChecker.gridView = dataGridView1;
                    ((Label)this.Controls["flowLayoutPanel1"].Controls["panel1"].Controls["name"]).Text = (GetAccntIndex(i_accnt_no) + 1) + " " + newStockChecker.name;


                    ((CheckBox)this.Controls["flowLayoutPanel1"].Controls["panel1"].Controls["playing_cb"]).Click += newStockChecker.EVT_ChangePlaying;
                    ((CheckBox)this.Controls["flowLayoutPanel1"].Controls["panel1"].Controls["checkBox2"]).Click += newStockChecker.EVT_ChangeMedo;
                    ((CheckBox)this.Controls["flowLayoutPanel1"].Controls["panel1"].Controls["checkBox4"]).Click += newStockChecker.EVT_ChangeMesu;
                    ((CheckBox)this.Controls["flowLayoutPanel1"].Controls["panel1"].Controls["checkBox6"]).Click += newStockChecker.EVT_ChangeEndClear;
                    ((Label)this.Controls["flowLayoutPanel1"].Controls["panel1"].Controls["targetprice_text"]).Click += newStockChecker.EVT_ChangeTargetPrice;
                    ((Label)this.Controls["flowLayoutPanel1"].Controls["panel1"].Controls["targetcount"]).Click += newStockChecker.EVT_ChangeTargetCount;
                    ((Label)this.Controls["flowLayoutPanel1"].Controls["panel1"].Controls["label7"]).Click += newStockChecker.EVT_ChangeRemainCount;
                    ((CheckBox)this.Controls["flowLayoutPanel1"].Controls["panel1"].Controls["checkBox7"]).Click += newStockChecker.EVT_ChangeHighMedo;
                    ((CheckBox)this.Controls["flowLayoutPanel1"].Controls["panel1"].Controls["checkBox8"]).Click += newStockChecker.EVT_ChangeCut;
                    ((CheckBox)this.Controls["flowLayoutPanel1"].Controls["panel1"].Controls["checkBox9"]).Click += newStockChecker.EVT_ChangeRebuy;
                    ((Label)this.Controls["flowLayoutPanel1"].Controls["panel1"].Controls["panel3"].Controls["label19"]).Click += newStockChecker.EVT_ChangeMedoTick;
                    ((Label)this.Controls["flowLayoutPanel1"].Controls["panel1"].Controls["panel3"].Controls["label10"]).Click += newStockChecker.EVT_ChangeMesuTick;
                    ((Button)this.Controls["flowLayoutPanel1"].Controls["panel1"].Controls["panel3"].Controls["button4"]).Click += newStockChecker.EVT_ChangePer;
                    ((Button)this.Controls["flowLayoutPanel1"].Controls["panel1"].Controls["panel3"].Controls["button11"]).Click += newStockChecker.EVT_SetFirst;
                    ((Label)this.Controls["flowLayoutPanel1"].Controls["panel1"].Controls["label6"]).Click += newStockChecker.EVT_ChangeHighMedoPrice;
                    ((Label)this.Controls["flowLayoutPanel1"].Controls["panel1"].Controls["label11"]).Click += newStockChecker.EVT_ChangeCutPrice;
                    ((CheckBox)this.Controls["flowLayoutPanel1"].Controls["panel1"].Controls["checkbox1"]).Click += newStockChecker.EVT_ChangeHaveEffect;
                    ((CheckBox)this.Controls["flowLayoutPanel1"].Controls["panel1"].Controls["checkbox10"]).Click += newStockChecker.EVT_ChangeEndlimit;
                    ((Button)this.Controls["flowLayoutPanel1"].Controls["panel1"].Controls["button6"]).Click += newStockChecker.EVT_Remove;
                    ((CheckBox)this.Controls["flowLayoutPanel1"].Controls["panel1"].Controls["highmesu"]).Click += newStockChecker.EVT_ChangeHighMesu;
                    ((CheckBox)this.Controls["flowLayoutPanel1"].Controls["panel1"].Controls["pricechangesound"]).Click += newStockChecker.EVT_ChangePriceChangeSound;
                    ((Button)this.Controls["flowLayoutPanel1"].Controls["panel1"].Controls["panel3"].Controls["button3"]).Click += newStockChecker.AllMedo;
                    ((Button)this.Controls["flowLayoutPanel1"].Controls["panel1"].Controls["panel3"].Controls["button2"]).Click += newStockChecker.halfMedo;
                    ((Label)this.Controls["flowLayoutPanel1"].Controls["panel1"].Controls["panel3"].Controls["label12"]).Click += newStockChecker.EVT_ChangeMarkPrice;
                }
                else
                {
                    Panel newpan = ((Panel)this.Controls["flowLayoutPanel1"].Controls["panel1"]).Clone();
                    newStockChecker.pan = newpan;



                    newpan.Controls.Add(((Label)this.Controls["flowLayoutPanel1"].Controls["panel1"].Controls["targetprice_text"]).Clone());
                    newpan.Controls.Add(((Label)this.Controls["flowLayoutPanel1"].Controls["panel1"].Controls["label11"]).Clone());
                    newpan.Controls.Add(((CheckBox)this.Controls["flowLayoutPanel1"].Controls["panel1"].Controls["checkBox9"]).Clone());
                    newpan.Controls.Add(((CheckBox)this.Controls["flowLayoutPanel1"].Controls["panel1"].Controls["checkBox8"]).Clone());
                    newpan.Controls.Add(((Label)this.Controls["flowLayoutPanel1"].Controls["panel1"].Controls["label6"]).Clone());
                    newpan.Controls.Add(((Label)this.Controls["flowLayoutPanel1"].Controls["panel1"].Controls["targetcount"]).Clone());
                    newpan.Controls.Add(((CheckBox)this.Controls["flowLayoutPanel1"].Controls["panel1"].Controls["checkBox7"]).Clone());
                    newpan.Controls.Add(((CheckBox)this.Controls["flowLayoutPanel1"].Controls["panel1"].Controls["checkBox6"]).Clone());
                    newpan.Controls.Add(((Label)this.Controls["flowLayoutPanel1"].Controls["panel1"].Controls["label9"]).Clone());
                    newpan.Controls.Add(((Label)this.Controls["flowLayoutPanel1"].Controls["panel1"].Controls["label8"]).Clone());
                    newpan.Controls.Add(((Label)this.Controls["flowLayoutPanel1"].Controls["panel1"].Controls["label7"]).Clone());
                    newpan.Controls.Add(((CheckBox)this.Controls["flowLayoutPanel1"].Controls["panel1"].Controls["checkBox4"]).Clone());
                    newpan.Controls.Add(((CheckBox)this.Controls["flowLayoutPanel1"].Controls["panel1"].Controls["checkBox2"]).Clone());
                    newpan.Controls.Add(((CheckBox)this.Controls["flowLayoutPanel1"].Controls["panel1"].Controls["playing_cb"]).Clone());
                    newpan.Controls.Add(((Label)this.Controls["flowLayoutPanel1"].Controls["panel1"].Controls["label13"]).Clone());
                    newpan.Controls.Add(((CheckBox)this.Controls["flowLayoutPanel1"].Controls["panel1"].Controls["checkBox10"]).Clone());
                    newpan.Controls.Add(((Button)this.Controls["flowLayoutPanel1"].Controls["panel1"].Controls["button6"]).Clone());
                    newpan.Controls.Add(((CheckBox)this.Controls["flowLayoutPanel1"].Controls["panel1"].Controls["checkBox1"]).Clone());
                    newpan.Controls.Add(((Label)this.Controls["flowLayoutPanel1"].Controls["panel1"].Controls["label3"]).Clone());
                    //newpan.Controls.Add(((Label)this.Controls["flowLayoutPanel1"].Controls["panel1"].Controls["label5"]).Clone());
                    //newpan.Controls.Add(((Label)this.Controls["flowLayoutPanel1"].Controls["panel1"].Controls["label4"]).Clone());
                    newpan.Controls.Add(((Label)this.Controls["flowLayoutPanel1"].Controls["panel1"].Controls["name"]).Clone());
                    newpan.Controls.Add(((CheckBox)this.Controls["flowLayoutPanel1"].Controls["panel1"].Controls["highmesu"]).Clone());
                    newpan.Controls.Add(((CheckBox)this.Controls["flowLayoutPanel1"].Controls["panel1"].Controls["pricechangesound"]).Clone());
                    newpan.Controls.Add(((Label)this.Controls["flowLayoutPanel1"].Controls["panel1"].Controls["label15"]).Clone());


                    ((CheckBox)newpan.Controls["playing_cb"]).Click += newStockChecker.EVT_ChangePlaying;
                    ((CheckBox)newpan.Controls["checkBox2"]).Click += newStockChecker.EVT_ChangeMedo;
                    ((CheckBox)newpan.Controls["checkBox4"]).Click += newStockChecker.EVT_ChangeMesu;
                    ((CheckBox)newpan.Controls["checkBox6"]).Click += newStockChecker.EVT_ChangeEndClear;
                    ((Label)newpan.Controls["targetprice_text"]).Click += newStockChecker.EVT_ChangeTargetPrice;
                    ((Label)newpan.Controls["targetcount"]).Click += newStockChecker.EVT_ChangeTargetCount;
                    ((Label)newpan.Controls["label7"]).Click += newStockChecker.EVT_ChangeRemainCount;
                    ((CheckBox)newpan.Controls["checkBox7"]).Click += newStockChecker.EVT_ChangeHighMedo;
                    ((CheckBox)newpan.Controls["checkBox8"]).Click += newStockChecker.EVT_ChangeCut;
                    ((CheckBox)newpan.Controls["checkBox9"]).Click += newStockChecker.EVT_ChangeRebuy;
                    ((Label)newpan.Controls["label6"]).Click += newStockChecker.EVT_ChangeHighMedoPrice;
                    ((Label)newpan.Controls["label11"]).Click += newStockChecker.EVT_ChangeCutPrice;
                    ((CheckBox)newpan.Controls["checkbox1"]).Click += newStockChecker.EVT_ChangeHaveEffect;
                    ((CheckBox)newpan.Controls["checkbox10"]).Click += newStockChecker.EVT_ChangeEndlimit;
                    ((Button)newpan.Controls["button6"]).Click += newStockChecker.EVT_Remove;
                    ((CheckBox)newpan.Controls["highmesu"]).Click += newStockChecker.EVT_ChangeHighMesu;
                    ((CheckBox)newpan.Controls["pricechangesound"]).Click += newStockChecker.EVT_ChangePriceChangeSound;


                    flowLayoutPanel1.Controls.Add(newpan);
                    newpan.Name = "panel" + (newStockChecker.index + 1);
                    /////////////////////////////
                    ////////panel 복제 복사//////
                    /////////////////////////////
                    int index = 0;


                    DataGridView newdg = new DataGridView();
                    newdg.Name = this.Controls["flowLayoutPanel1"].Controls["panel1"].Controls["dataGridView1"].Name;
                    newdg.Location = this.Controls["flowLayoutPanel1"].Controls["panel1"].Controls["dataGridView1"].Location;
                    newdg.Size = this.Controls["flowLayoutPanel1"].Controls["panel1"].Controls["dataGridView1"].Size;
                    newdg.ColumnCount = (((DataGridView)this.Controls["flowLayoutPanel1"].Controls["panel1"].Controls["dataGridView1"]).ColumnCount);
                    newdg.RowHeadersVisible = false;
                    newdg.RowTemplate = (((DataGridView)this.Controls["flowLayoutPanel1"].Controls["panel1"].Controls["dataGridView1"]).RowTemplate);
                    newdg.MultiSelect = (((DataGridView)this.Controls["flowLayoutPanel1"].Controls["panel1"].Controls["dataGridView1"]).MultiSelect);
                    newdg.ColumnHeadersVisible = (((DataGridView)this.Controls["flowLayoutPanel1"].Controls["panel1"].Controls["dataGridView1"]).ColumnHeadersVisible);
                    newdg.AllowUserToResizeColumns = (((DataGridView)this.Controls["flowLayoutPanel1"].Controls["panel1"].Controls["dataGridView1"]).AllowUserToResizeColumns);
                    newdg.AllowUserToResizeRows = (((DataGridView)this.Controls["flowLayoutPanel1"].Controls["panel1"].Controls["dataGridView1"]).AllowUserToResizeRows);
                    newdg.RowHeadersDefaultCellStyle = (((DataGridView)this.Controls["flowLayoutPanel1"].Controls["panel1"].Controls["dataGridView1"]).RowHeadersDefaultCellStyle);
                    newdg.DefaultCellStyle = (((DataGridView)this.Controls["flowLayoutPanel1"].Controls["panel1"].Controls["dataGridView1"]).DefaultCellStyle);
                    newdg.ReadOnly = (((DataGridView)this.Controls["flowLayoutPanel1"].Controls["panel1"].Controls["dataGridView1"]).ReadOnly);
                    newdg.ScrollBars = (((DataGridView)this.Controls["flowLayoutPanel1"].Controls["panel1"].Controls["dataGridView1"]).ScrollBars);
                    newdg.DefaultCellStyle.Alignment = (((DataGridView)this.Controls["flowLayoutPanel1"].Controls["panel1"].Controls["dataGridView1"]).DefaultCellStyle.Alignment);
                    for (int i = 0; i < ((DataGridView)this.Controls["flowLayoutPanel1"].Controls["panel1"].Controls["dataGridView1"]).Columns.Count; i++)
                    {
                        newdg.Columns[i].Name = (((DataGridView)this.Controls["flowLayoutPanel1"].Controls["panel1"].Controls["dataGridView1"]).Columns[i].Name);
                        newdg.Columns[i].HeaderText = (((DataGridView)this.Controls["flowLayoutPanel1"].Controls["panel1"].Controls["dataGridView1"]).Columns[i].HeaderText);
                        newdg.Columns[i].Width = (((DataGridView)this.Controls["flowLayoutPanel1"].Controls["panel1"].Controls["dataGridView1"]).Columns[i].Width);
                    }
                    newdg.Rows.Clear();
                    newdg.Rows.Add(19);
                    newpan.Controls.Add(newdg);
                    newStockChecker.gridView = newdg;

                    Panel newpan1_1 = ((Panel)this.Controls["flowLayoutPanel1"].Controls["panel1"].Controls["panel3"]).Clone();
                    index = 0;

                    newpan1_1.Controls.Add(((Label)this.Controls["flowLayoutPanel1"].Controls["panel1"].Controls["panel3"].Controls[index++]).Clone());
                    newpan1_1.Controls.Add(((Label)this.Controls["flowLayoutPanel1"].Controls["panel1"].Controls["panel3"].Controls[index++]).Clone());
                    newpan1_1.Controls.Add(((Label)this.Controls["flowLayoutPanel1"].Controls["panel1"].Controls["panel3"].Controls[index++]).Clone());
                    newpan1_1.Controls.Add(((Label)this.Controls["flowLayoutPanel1"].Controls["panel1"].Controls["panel3"].Controls[index++]).Clone());
                    newpan1_1.Controls.Add(((Button)this.Controls["flowLayoutPanel1"].Controls["panel1"].Controls["panel3"].Controls[index++]).Clone());
                    newpan1_1.Controls.Add(((Label)this.Controls["flowLayoutPanel1"].Controls["panel1"].Controls["panel3"].Controls[index++]).Clone());
                    newpan1_1.Controls.Add(((Button)this.Controls["flowLayoutPanel1"].Controls["panel1"].Controls["panel3"].Controls[index++]).Clone());
                    newpan1_1.Controls.Add(((Button)this.Controls["flowLayoutPanel1"].Controls["panel1"].Controls["panel3"].Controls[index++]).Clone());
                    newpan1_1.Controls.Add(((Button)this.Controls["flowLayoutPanel1"].Controls["panel1"].Controls["panel3"].Controls[index++]).Clone());
                    newpan1_1.Controls.Add(((Label)this.Controls["flowLayoutPanel1"].Controls["panel1"].Controls["panel3"].Controls[index++]).Clone());
                    newpan1_1.Controls.Add(((Label)this.Controls["flowLayoutPanel1"].Controls["panel1"].Controls["panel3"].Controls[index++]).Clone());

                    ((Label)newpan1_1.Controls["label19"]).Click += newStockChecker.EVT_ChangeMedoTick;
                    ((Label)newpan1_1.Controls["label17"]).Click += newStockChecker.EVT_ChangeMedoTick;
                    ((Label)newpan1_1.Controls["label18"]).Click += newStockChecker.EVT_ChangeMedoTick;
                    ((Label)newpan1_1.Controls["label10"]).Click += newStockChecker.EVT_ChangeMesuTick;
                    ((Label)newpan1_1.Controls["label20"]).Click += newStockChecker.EVT_ChangeMesuTick;
                    ((Label)newpan1_1.Controls["label21"]).Click += newStockChecker.EVT_ChangeMesuTick;
                    ((Button)newpan1_1.Controls["button4"]).Click += newStockChecker.EVT_ChangePer;
                    ((Button)newpan1_1.Controls["button11"]).Click += newStockChecker.EVT_SetFirst;
                    ((Button)newpan1_1.Controls["button3"]).Click += newStockChecker.AllMedo;
                    ((Button)newpan1_1.Controls["button2"]).Click += newStockChecker.halfMedo;
                    ((Label)newpan1_1.Controls["label12"]).Click += newStockChecker.EVT_ChangeMarkPrice;

                    for (int j = 0; j < newpan1_1.Controls.Count; j++)
                    {
                        newpan1_1.Controls[j].Parent = newpan1_1;
                        //newpan1_1.Controls[j].BackColor = SystemColors.Control;
                    }

                    newpan.Controls.Add(newpan1_1);


                    /////////////////////////////
                    //////panel 복제 복사 끝/////
                    /////////////////////////////


                    newpan.Controls["name"].Text = (GetAccntIndex(i_accnt_no) + 1) + " " + newStockChecker.name;

                }
                m_threadrealdata(i_jongmok_cd);
                //Task.Factory.StartNew(new Action<object>(m_threadrealdata), i_jongmok_cd); ; // 스레드 생성
                newStockChecker.ReloadPanel();
                newStockChecker.isKosdaq();
                if (!firstget)
                {
                    PlayEffect(6);
                }
                //axKHOpenAPI1.SetRealReg(get_scr_no(), i_jongmok_cd, "51;10;12;930;931;933;9019;21;41;", "1");
                Send_Log(i_jongmok_cd);
                if (!test)
                {
                    newStockChecker.AddPriceData(now_price, "");
                    newStockChecker.realprice = i_buy_price;
                }
                newStockChecker.realprice = i_buy_price;
                stockCheckers.Add(newStockChecker);


                string currentAccountkeyString = "C" + i_jongmok_cd + i_accnt_no;
                string mainAccountKeyString = "C" + g_accnt_no + i_accnt_no;
                bool isSettingsExist = xmlData.Find(currentAccountkeyString + ".is_cut") != null;
                bool isSettingsExistOnMainAccount = xmlData.Find(mainAccountKeyString + ".is_cut") != null;

                //저장된 값 확인 후 불러오기
                if (isSettingsExist || isSettingsExistOnMainAccount)
                {
                    string keyString = isSettingsExist ? currentAccountkeyString : mainAccountKeyString;

                    try
                    {
                        newStockChecker.targetPrice = int.Parse(xmlData.Find(keyString + ".targetPrice").value.Trim());
                        if (newStockChecker.targetPrice != 0)
                            ((Label)newStockChecker.pan.Controls["targetprice_text"]).Text = "목표가: " + string.Format("{0:#,###}", newStockChecker.targetPrice / 10000) + "만원";
                        else
                            ((Label)newStockChecker.pan.Controls["targetprice_text"]).Text = "목표가: 0원";

                        for (int i = 0; i < newStockChecker.sell_conditions.Length; i++)
                            newStockChecker.sell_conditions[i] = int.Parse(xmlData.Find(keyString + ".sell_conditions" + i).value.Trim());
                        for (int i = 0; i < newStockChecker.buy_conditions.Length; i++)
                            newStockChecker.buy_conditions[i] = int.Parse(xmlData.Find(keyString + ".buy_conditions" + i).value.Trim());

                        newStockChecker.is_cut = bool.Parse(xmlData.Find(keyString + ".is_cut").value.Trim());
                        newStockChecker.cut_price = float.Parse(xmlData.Find(keyString + ".cut_price").value.Trim());
                        if (newStockChecker.cut_price > 100) newStockChecker.cut_per = false;
                        newStockChecker.is_sell = bool.Parse(xmlData.Find(keyString + ".is_sell").value.Trim());
                        newStockChecker.is_buy = bool.Parse(xmlData.Find(keyString + ".is_buy").value.Trim());
                        newStockChecker.end_clear = bool.Parse(xmlData.Find(keyString + ".end_clear").value.Trim());
                        newStockChecker.is_playing = bool.Parse(xmlData.Find(keyString + ".is_playing").value.Trim());
                        newStockChecker.is_highmedo = bool.Parse(xmlData.Find(keyString + ".is_highmedo").value.Trim());
                        newStockChecker.is_highmedo_per = bool.Parse(xmlData.Find(keyString + ".is_highmedo_per").value.Trim());
                        newStockChecker.highMedo_per = int.Parse(xmlData.Find(keyString + ".highMedo_per").value.Trim());
                        newStockChecker.highMedo_price = int.Parse(xmlData.Find(keyString + ".highMedo_price").value.Trim());
                        newStockChecker.haveEffect = bool.Parse(xmlData.Find(keyString + ".haveEffect").value.Trim());
                        newStockChecker.highMedo_perval = int.Parse(xmlData.Find(keyString + ".highMedo_perval").value.Trim());
                        newStockChecker.is_endLimit = bool.Parse(xmlData.Find(keyString + ".is_endLimit").value.Trim());
                        newStockChecker.remainCountVal = int.Parse(xmlData.Find(keyString + ".remainCountVal").value.Trim());
                        newStockChecker.sell_start = float.Parse(xmlData.Find(keyString + ".sell_start").value.Trim());
                        newStockChecker.sell_end = int.Parse(xmlData.Find(keyString + ".sell_end").value.Trim());
                        newStockChecker.sell_per = float.Parse(xmlData.Find(keyString + ".sell_per").value.Trim());
                        newStockChecker.buy_start = float.Parse(xmlData.Find(keyString + ".buy_start").value.Trim());
                        newStockChecker.buy_end = int.Parse(xmlData.Find(keyString + ".buy_end").value.Trim());
                        newStockChecker.buy_per = float.Parse(xmlData.Find(keyString + ".buy_per").value.Trim());
                        newStockChecker.highbuy_start = float.Parse(xmlData.Find(keyString + ".highbuy_start").value.Trim());
                        newStockChecker.highbuy_end = float.Parse(xmlData.Find(keyString + ".highbuy_end").value.Trim());
                        newStockChecker.highbuy_per = float.Parse(xmlData.Find(keyString + ".highbuy_per").value.Trim());
                        newStockChecker.is_price_change_sound = bool.Parse(xmlData.Find(keyString + ".is_price_change_sound").value.Trim());
                        newStockChecker.targetCount = int.Parse(xmlData.Find(keyString + ".targetCount").value.Trim());
                        newStockChecker.targetisprice = bool.Parse(xmlData.Find(keyString + ".targetisprice").value.Trim());
                        //newStockChecker.is_repeat = bool.Parse(xmlData.Find(i_jongmok_nm + ".is_repeat").value.Trim());
                        //newStockChecker.low_price = int.Parse(xmlData.Find(i_jongmok_nm + ".low_price").value.Trim());
                        //newStockChecker.high_price = int.Parse(xmlData.Find(i_jongmok_nm + ".high_price").value.Trim());
                        //newStockChecker.state = (StockChecker.StockState)int.Parse(xmlData.Find(i_jongmok_nm + ".state").value.Trim());
                    }
                    catch (Exception e)
                    {
                        PlayEffect(25);
                        Send_Log("XML: " + e.Message + e.StackTrace);
                    }
                }
                else
                {
                    newStockChecker.is_cut = is_cut;
                    newStockChecker.cut_price = cut_price;
                    newStockChecker.is_sell = is_sell;
                    newStockChecker.is_buy = is_buy;


                    newStockChecker.targetPrice = targetPrice;
                    if (newStockChecker.targetPrice != 0)
                        ((Label)newStockChecker.pan.Controls["targetprice_text"]).Text = "목표가: " + string.Format("{0:#,###}", newStockChecker.targetPrice / 10000) + "만원";
                    else
                        ((Label)newStockChecker.pan.Controls["targetprice_text"]).Text = "목표가: 0원";


                    newStockChecker.is_playing = play;
                    newStockChecker.is_cut = is_cut;
                    newStockChecker.cut_price = cut_price;
                    newStockChecker.is_sell = is_sell;
                    newStockChecker.is_buy = is_buy;
                    newStockChecker.end_clear = end_clear;
                    newStockChecker.is_highmedo = is_highmedo;
                    newStockChecker.is_highmedo_per = is_highmedo_per;
                    newStockChecker.highMedo_perval = highMedo_perval;
                    newStockChecker.highMedo_price = highMedo_price;
                    newStockChecker.haveEffect = haveEffect;
                    newStockChecker.is_endLimit = is_endLimit;
                    newStockChecker.is_price_change_sound = is_price_change_sound;
                    newStockChecker.remainCountVal = remainCountVal;
                }
                if (allPlay && newStockChecker.is_playing)
                {
                    newStockChecker.Play();
                }
                newStockChecker.CheckPriceKind();
                newStockChecker.SetCounts();
                newStockChecker.ReloadHoga();
                newStockChecker.ReloadPanel();

                return stockCheckers.Count - 1;
            }
            catch (Exception e)
            {
                Send_Log("insert_tb_accnt_info() [" + e.Message + "]");
                Send_Log("insert_tb_accnt_info() [" + e.StackTrace + "]");
                return -1;
            }

        }

        public void Stopreal(string screennum)
        {
            axKHOpenAPI1.DisconnectRealData(screennum);
        }

        public int StartRealCondition(string screennum, string conName, int index, int nSh)
        {
            return axKHOpenAPI1.SendCondition(screennum, conName, index, nSh);
        }
        public void StopRealCondition(string screennum, string conName, int index)
        {
            axKHOpenAPI1.SendConditionStop(screennum, conName, index);
        }

        public void SetLogmsg(string text)
        {
            if (this.log_msg.InvokeRequired)
            {
                logbox.BeginInvoke(new Action(() => this.log_msg.Text = text));
            }
            else
            {
                this.log_msg.Text = text;
            }
        }

        public void Send_Log_Debug(string msg, bool time)
        {
            if (checkBox3.Checked == true)
            {
                Send_Log(msg, time);
            }
            else
            {
                if (saveLog)
                {
                    string res = "";
                    if (time)
                    {
                        DateTime l_cur_time;
                        string l_cur_tm;
                        string l_cur_dtm;
                        l_cur_tm = "";
                        l_cur_time = DateTime.Now;
                        l_cur_tm = l_cur_time.ToString("HH:mm:ss");
                        l_cur_dtm = l_cur_tm + " ";
                        res += l_cur_dtm;
                    }
                    res += msg;

                    System.IO.File.AppendAllText(logpath, Environment.NewLine + res, Encoding.Default);
                }
            }
        }

        public void Send_Log_Debug(string msg)
        {
            if (checkBox3.Checked == true)
            {
                Send_Log(msg);
            }
            else
            {
                if (saveLog)
                {
                    string res = "";
                    DateTime l_cur_time;
                    string l_cur_tm;
                    string l_cur_dtm;
                    l_cur_tm = "";
                    l_cur_time = DateTime.Now;
                    l_cur_tm = l_cur_time.ToString("HH:mm:ss");
                    l_cur_dtm = l_cur_tm + " ";
                    res += l_cur_dtm;
                    res += msg;

                    System.IO.File.AppendAllText(logpath, Environment.NewLine + res, Encoding.Default);
                }
            }
        }

        //로그 출력
        public void Send_Log(string msg, bool time)
        {
            try
            {
                if (logbox != null)
                {
                    string res = "";
                    if (time)
                    {
                        DateTime l_cur_time;
                        string l_cur_tm;
                        string l_cur_dtm;
                        l_cur_tm = "";
                        l_cur_time = DateTime.Now;
                        l_cur_tm = l_cur_time.ToString("HH:mm:ss");
                        l_cur_dtm = l_cur_tm + " ";
                        res += l_cur_dtm;
                    }
                    res += msg;

                    if (saveLog)
                        System.IO.File.AppendAllText(logpath, Environment.NewLine + res, Encoding.Default);

                    logbox.BeginInvoke(new Action(() =>
                    {
                        logbox.AppendText(res + Environment.NewLine);
                    }
                    ));
                }
            }
            catch (Exception e)
            {
                PlayEffect(25);
                // Send_Log("Send_Log() [" + e.Message + "]");
            }
        }
        public void Send_Log(string msg)
        {
            try
            {
                if (logbox != null)
                {
                    DateTime l_cur_time;
                    string l_cur_tm;
                    string l_cur_dtm;
                    l_cur_tm = "";
                    l_cur_time = DateTime.Now;
                    l_cur_tm = l_cur_time.ToString("HH:mm:ss");
                    l_cur_dtm = l_cur_tm + " ";

                    if (saveLog)
                        System.IO.File.AppendAllText(logpath, Environment.NewLine + l_cur_dtm + msg, Encoding.Default);

                    logbox.BeginInvoke(new Action(() =>
                    {
                        logbox.AppendText(l_cur_dtm + msg + Environment.NewLine);
                    }
                    ));
                }
            }
            catch (Exception e)
            {
                PlayEffect(25);
                // Send_Log("Send_Log() [" + e.Message + "]");
            }
        }

        /// <summary>
        /// Logbox의 텍스트가 너무 길어져 메모리를 낭비하는 것을 방지하는 함수
        /// </summary>
        public void CutLogBoxByLength()
        {
            if (logbox.Text.Length > 5000)
            {
                logbox.BeginInvoke(new Action(() =>
                {
                    logbox.Text = logbox.Text.Substring(logbox.Text.Length - 2000);
                }
                ));
            }
        }


        //실수 입력창 호출
        public void CallValueWindowFloat(string name, Action<float, bool> action)
        {
            Form2 form2 = new Form2(name, action);
            form2.Name = name;
            form2.ShowDialog();
        }
        //정수 입력창 호출
        public void CallValueWindowInt(string name, Action<int> action)
        {
            Form3 form3 = new Form3(name, action);
            form3.Name = name;
            form3.ShowDialog();
        }
        /// <summary>
        /// 틱설정창 호출
        /// </summary>
        /// <param name="name">창 이름설정</param>
        /// <param name="action">창 종료시 행동</param>
        public void CallValueWindowTick(string name, StockChecker _stockChecker)
        {
            Form6 form6 = new Form6(this, _stockChecker);
            form6.Text = name;
            form6.ShowDialog();
        }

        //로그인 이벤트 함수
        private void axKHOpenAPI1_OnEventConnect(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnEventConnectEvent e)
        {
            //에러가 난다면 에러코드 출력 후 종료
            if (e.nErrCode < 0)
            {
                err = true;
                MessageBox.Show("Error : " + e.nErrCode);
                this.Dispose();
                Application.ExitThread();
                Environment.Exit(0);
            }
            else
            {

                //delay(1000);
                if (axKHOpenAPI1.GetLoginInfo("ACCOUNT_CNT").Trim() == "")
                {
                    Application.Restart();
                    return;
                }

                this.Invoke(new MethodInvoker(delegate ()
                {
                    Logined();
                }));

                g_flag_1 = 1;
                loginButton.Dispose();
            }
        }

        //조건검색식 수신 이벤트
        void OnReceiveConditionVer(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveConditionVerEvent e)
        {
            string conditionNameList = axKHOpenAPI1.GetConditionNameList();
            string[] conditionNameArray = conditionNameList.Split(';');

            for(int i = 0; i < conditionNameArray.Length; i++)
            {
                string[] conditionInfo = conditionNameArray[i].Split('^');
                if(conditionInfo.Length == 2)
                {
                    Thread.Sleep(250);
                    System.Windows.Forms.Application.DoEvents();
                    conditionProcess.AddCondition(conditionInfo[1].Trim(), int.Parse(conditionInfo[0].Trim()));
                }
            }
            for (int i = 0; i < conditionProcess.conditions.Count; i++)
            {
                if (conditionProcess.conditions.Find(a => a.name == conditionProcess.conditions[i].linked) == null)
                {
                    conditionProcess.conditions[i].linked = null;
                }
            }
        }

        //데이터수신 이벤트
        private void axKHOpenAPI1_OnReceiveTrData(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveTrDataEvent e)
        {
            ShowMemory();
            if (saveLog)
            {
                System.IO.File.AppendAllText(logpath.Replace(".txt", "_RealTime.txt"), Environment.NewLine + DateTime.Now);
                System.IO.File.AppendAllText(logpath.Replace(".txt", "_RealTime.txt"), Environment.NewLine + "OnReceiveTrData " + axKHOpenAPI1.CommGetData(e.sTrCode, "", e.sRQName, 0, "종목코드").Trim());
                System.IO.File.AppendAllText(logpath.Replace(".txt", "_RealTime.txt"), Environment.NewLine + e.sRQName);
            }
            //Send_Log(e.sRQName);
            if (e.sRQName == "증거금세부내역조회요청") // 응답받은 요청명이 '증거금세부내역조회요청'이라면
            {
                g_ord_amt_possible = int.Parse(axKHOpenAPI1.CommGetData(e.sTrCode, "", e.sRQName, 0, "100주문가능금액").Trim()); // 주문가능금액을 저장
                g_flag_1 = 1;
            }
            else if (e.sRQName == "주식기본정보") // 응답받은 요청명이 '주식기본정보'이라면
            {
                try
                {
                    axKHOpenAPI1.GetRepeatCnt(e.sTrCode, e.sRQName);
                    if (axKHOpenAPI1.CommGetData(e.sTrCode, "", e.sRQName, 0, "종목코드").Trim() == "" || axKHOpenAPI1.CommGetData(e.sTrCode, "", e.sRQName, 0, "종목명").Trim() == "")
                    {
                        return;
                    }
                    Send_Log(axKHOpenAPI1.CommGetData(e.sTrCode, "", e.sRQName, 0, "종목명").Trim() + " 종목추가");
                    int index = insert_tb_accnt_info(RealCode(axKHOpenAPI1.CommGetData(e.sTrCode, "", e.sRQName, 0, "종목코드").Trim()), axKHOpenAPI1.CommGetData(e.sTrCode, "", e.sRQName, 0, "종목명").Trim(), g_accnt_no, 0, 0, Math.Abs(int.Parse(axKHOpenAPI1.CommGetData(e.sTrCode, "", e.sRQName, 0, "현재가").Trim())));
                    if (index >= 0)
                    {
                        stockCheckers[index].limitprice = (int)(int.Parse(axKHOpenAPI1.CommGetData(e.sTrCode, "", e.sRQName, 0, "전일종가").Trim()) * 1.3f);
                        stockCheckers[index].check = false;
                    }
                }
                catch (Exception ex)
                {
                    Send_Log("종목추가 오류 [" + ex.Message + "]");
                }
                return;

            }
            else if (e.sRQName.Contains("계좌평가현황요청")) // 응답받은 요청명이 '계좌평가현황요청'이라면
            {
                string accnt_no = e.sRQName.Split(',')[1];
                SetLogmsg("계좌 정보 수신 완료");
                g_flag_2 = 1;
                int repeat_cnt = 0;
                int ii = 0;
                string user_id = null;
                string jongmok_cd = null;
                string jongmok_nm = null;
                int own_stock_cnt = 0;
                int buy_price = 0;
                int own_amt = 0;
                int now_price = 0;
                int lastpri = 0;

                int possible_Amount = int.Parse(axKHOpenAPI1.CommGetData(e.sTrCode, "", e.sRQName, 0, "D+2추정예수금").Trim());
                ChangePossibleAmount(accnt_no, possible_Amount);

                repeat_cnt = axKHOpenAPI1.GetRepeatCnt(e.sTrCode, e.sRQName); // 보유종목수 가져오기
                //Send_Log("보유종목수 : " + repeat_cnt.ToString() + "\n");
                for (ii = 0; ii < repeat_cnt; ii++)
                {
                    user_id = "";
                    jongmok_cd = "";
                    own_stock_cnt = 0;
                    buy_price = 0;
                    own_amt = 0;
                    user_id = g_user_id;
                    try
                    {
                        jongmok_cd = RealCode(axKHOpenAPI1.CommGetData(e.sTrCode, "", e.sRQName, ii, "종목코드").Trim());
                        if (jongmok_cd == "")
                        {
                            continue;
                        }
                        now_price = int.Parse(axKHOpenAPI1.CommGetData(e.sTrCode, "", e.sRQName, ii, "현재가").Trim());
                        jongmok_nm = axKHOpenAPI1.CommGetData(e.sTrCode, "", e.sRQName, ii, "종목명").Trim();

                        own_stock_cnt = (int)int.Parse(axKHOpenAPI1.CommGetData(e.sTrCode, "", e.sRQName, ii, "보유수량").Trim());
                        buy_price = (int)int.Parse(axKHOpenAPI1.CommGetData(e.sTrCode, "", e.sRQName, ii, "평균단가").Trim());
                        own_amt = (int)int.Parse(axKHOpenAPI1.CommGetData(e.sTrCode, "", e.sRQName, ii, "매입금액").Trim());
                        lastpri = (int)int.Parse(axKHOpenAPI1.GetMasterLastPrice(jongmok_cd));
                    }
                    catch (Exception)
                    {
                        Send_Log("API 잘못된 값 넘어옴 : " + axKHOpenAPI1.CommGetData(e.sTrCode, "", e.sRQName, ii, "종목코드").Trim());
                        //Send_Log("<종목코드 : " + axKHOpenAPI1.CommGetData(e.sTrCode, "", e.sRQName, ii, "종목코드").Trim() + ">");
                        //Send_Log("<종목명 : " + axKHOpenAPI1.CommGetData(e.sTrCode, "", e.sRQName, ii, "종목명").Trim() + ">");
                        //Send_Log("<보유수량 : " + axKHOpenAPI1.CommGetData(e.sTrCode, "", e.sRQName, ii, "보유수량").Trim() + ">");
                        //Send_Log("<현재가 : " + axKHOpenAPI1.CommGetData(e.sTrCode, "", e.sRQName, ii, "현재가").Trim() + ">");
                        //Send_Log("<평균단가 : " + axKHOpenAPI1.CommGetData(e.sTrCode, "", e.sRQName, ii, "평균단가").Trim() + ">");
                        //Send_Log("<매입금액 : " + axKHOpenAPI1.CommGetData(e.sTrCode, "", e.sRQName, ii, "매입금액").Trim() + ">");
                    }
                    //Send_Log("종목코드 : " + jongmok_cd + "\n");
                    //Send_Log("종목명 : " + jongmok_nm + "\n");
                    //Send_Log("보유주식수 : " + own_stock_cnt.ToString() + "\n");
                    if (own_stock_cnt == 0) // 보유주식수가 0이라면 저장하지 않음
                    {
                        continue;
                    }
                    int index = insert_tb_accnt_info(jongmok_cd, jongmok_nm, accnt_no, buy_price, own_stock_cnt, now_price);

                    if (index >= 0)
                    {
                        stockCheckers[index].limitprice = (int)(lastpri * 1.3f);
                        stockCheckers[index].check = false;
                    }
                }
                //Send_Log("TB_ACCNT_INFO 테이블 설정 완료");
                if (e.sPrevNext.Length == 0)
                {
                    g_is_next = 0;
                }
                else
                {
                    g_is_next = int.Parse(e.sPrevNext);
                }
                if (firstget)
                {
                    firstget = false;
                    // 아래 필요없는 요청 같아서 주석 처리 함. - 김동완 2025.06.01
                    //axKHOpenAPI1.SetInputValue("계좌번호", g_accnt_no);
                    //axKHOpenAPI1.SetInputValue("비밀번호", "");
                    //axKHOpenAPI1.SetInputValue("상장폐지조회구분", "1");
                    //axKHOpenAPI1.SetInputValue("비밀번호입력매체구분", "00");
                    //axKHOpenAPI1.SetInputValue("종목코드", "");
                    //int temp = axKHOpenAPI1.CommRqData("주식테스트", "otp10007", 0, get_scr_no());
                }

                for (int i = 0; i < stockCheckers.Count; i++)
                {
                    if (stockCheckers[i].check)
                    {
                        stockCheckers[i].count = 0;
                        stockCheckers[i].SetCountColor();
                        stockCheckers[i].realprice = 0;
                        break;
                    }
                    else
                    {
                        stockCheckers[i].check = true;
                    }
                }

            }
            else if (e.sRQName == "관심종목정보요청")
            {
                int repeat_cnt = axKHOpenAPI1.GetRepeatCnt(e.sTrCode, e.sRQName); // 보유종목수 가져오기
                //Send_Log("보유종목수 : " + repeat_cnt.ToString() + "\n");
                for (int ii = 0; ii < repeat_cnt; ii++)
                {
                    if (axKHOpenAPI1.CommGetData(e.sTrCode, "", e.sRQName, ii, "종목코드").Trim() != "")
                    {
                        int index = insert_tb_accnt_info(RealCode(axKHOpenAPI1.CommGetData(e.sTrCode, "", e.sRQName, ii, "종목코드").Trim()), axKHOpenAPI1.CommGetData(e.sTrCode, "", e.sRQName, ii, "종목명").Trim(), g_accnt_no, 0, 0, int.Parse(axKHOpenAPI1.CommGetData(e.sTrCode, "", e.sRQName, ii, "현재가").Trim().Replace("-", "")));
                        if (index >= 0)
                        {
                            stockCheckers[index].is_fixed = true;
                            ((Label)stockCheckers[index].pan.Controls["label13"]).Visible = true;
                            //stockCheckers[index].ReloadPanel();
                            Send_Log(axKHOpenAPI1.CommGetData(e.sTrCode, "", e.sRQName, ii, "종목명").Trim() + " 고정종목 불러옴");
                        }
                        else
                        {
                            Send_Log(axKHOpenAPI1.CommGetData(e.sTrCode, "", e.sRQName, ii, "종목명").Trim() + " 고정종목 불러오기 실패");
                        }
                    }
                }
            }
            else if (e.sRQName == "주식주문")
            {
                if (axKHOpenAPI1.CommGetData(e.sTrCode, "", e.sRQName, 0, "주문번호").Trim() == "")
                {
                    for (int i = 0; i < stockCheckers.Count; i++)
                    {
                        if (stockCheckers[i].junum == e.sScrNo)
                        {
                            Send_Log(stockCheckers[i].name + " 주문 실패:" + e.sErrorCode);
                            stockCheckers[i].Stop();
                            return;
                        }
                    }
                }
            }
        } //axKHOpenAPI1_OnReceiveTrData 메서드 종료

        private void ChangePossibleAmount(string accno, int possible_Amount)
        {
            this.possible_Amount[GetAccntIndex(accno)] = possible_Amount;
            if (accno == l_accno_arr[0])
            {
                // 0번 계좌의 예수금만 표시
                if (accno == l_accno_arr[0])
                {
                    ChangePossibleAmountText(possible_Amount);
                }
            }
        }

        private void ChangePossibleAmountText(int possible_Amount)
        {
            label14.Text = "예수금: " + string.Format("{0:#,###}", possible_Amount);
        }

        private void axKHOpenAPI1_OnReceiveMsg(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveMsgEvent e)
        {
        }

        private void axKHOpenAPI1_OnReceiveChejanData(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveChejanDataEvent e)
        {
            try
            {
                string accno = axKHOpenAPI1.GetChejanData(9201).Trim();
                if (e.sGubun == "0")
                {
                    StockChecker temp = stockCheckers.Find(a => a.isThis(RealCode(axKHOpenAPI1.GetChejanData(9001).Trim()), axKHOpenAPI1.GetChejanData(9201).Trim()));
                    //Send_Log(temp.name + " 주문 체결");
                    if (temp == null)
                    {
                        if (axKHOpenAPI1.GetChejanData(913).Trim() == "체결")
                        {
                            if (axKHOpenAPI1.GetChejanData(907).Trim() == "2")
                            {
                                BackgroundWorker backgroundWorker = new BackgroundWorker();
                                backgroundWorker.DoWork += (_, args) =>
                                {
                                    for (int i = 0; i <= 100; i++)
                                    {
                                        insert_tb_accnt_info(RealCode(axKHOpenAPI1.GetChejanData(9001).Trim()), axKHOpenAPI1.GetChejanData(302).Trim(), axKHOpenAPI1.GetChejanData(9201).Trim(), int.Parse(axKHOpenAPI1.GetChejanData(910).Trim()), int.Parse(axKHOpenAPI1.GetChejanData(911).Trim()), int.Parse(axKHOpenAPI1.GetChejanData(10).Trim()));
                                    }
                                };
                            }
                        }
                        return;
                    }
                    if (axKHOpenAPI1.GetChejanData(913).Trim() == "체결")
                    {
                        for (int i = 0; i < Jumuns.Count; i++)
                        {
                            if (Jumuns[i].jumunnumber == axKHOpenAPI1.GetChejanData(9203).Trim())
                            {

                                int count = int.Parse(axKHOpenAPI1.GetChejanData(911));
                                if (axKHOpenAPI1.GetChejanData(907).Trim() == "2")
                                {
                                    int totalcount = temp.count + (count - Jumuns[i].chegyulcount);
                                    int totalprice = temp.count * temp.realprice;
                                    totalprice += (count - Jumuns[i].chegyulcount) * int.Parse(axKHOpenAPI1.GetChejanData(910));
                                    temp.realprice = totalprice / totalcount;
                                    temp.count += (count - Jumuns[i].chegyulcount);
                                    int possible_Amount = this.possible_Amount[GetAccntIndex(accno)] - (count - Jumuns[i].chegyulcount) * int.Parse(axKHOpenAPI1.GetChejanData(910));
                                    ChangePossibleAmount(accno, possible_Amount);
                                    Jumuns[i].chegyulcount = count;
                                }
                                else if (axKHOpenAPI1.GetChejanData(907).Trim() == "1")
                                {
                                    temp.count -= (count - Jumuns[i].chegyulcount);
                                    int possible_Amount = this.possible_Amount[GetAccntIndex(accno)] + (count - Jumuns[i].chegyulcount) * int.Parse(axKHOpenAPI1.GetChejanData(910));
                                    ChangePossibleAmount(accno, possible_Amount);
                                    Jumuns[i].chegyulcount = count;
                                }
                                if (int.Parse(axKHOpenAPI1.GetChejanData(902).Trim()) == 0)
                                {
                                    Jumuns.Remove(Jumuns[i]);
                                }
                                //temp.ReloadPanel();
                                ((Label)temp.pan.Controls["label8"]).Text = "보유수량: " + temp.count;
                                return;
                            }
                        }
                    }
                    else if (axKHOpenAPI1.GetChejanData(913) == "접수")
                    {
                        Jumuns.Add(new JumunData(axKHOpenAPI1.GetChejanData(9203).Trim(), int.Parse(axKHOpenAPI1.GetChejanData(900).Trim())));
                    }
                    else if (axKHOpenAPI1.GetChejanData(913) == "취소")
                    {
                        for (int i = 0; i < Jumuns.Count; i++)
                        {
                            if (Jumuns[i].jumunnumber == axKHOpenAPI1.GetChejanData(9203).Trim())
                            {
                                Jumuns.Remove(Jumuns[i]);
                            }
                        }
                    }

                }
                else if (e.sGubun == "1")
                {
                }
            }
            catch (Exception ex)
            {
                PlayEffect(25);
                Send_Log("axKHOpenAPI1_OnReceiveChejanData() [" + ex.Message + "]");
            }
        }

        private void axKHOpenAPI1_OnReceiveRealData(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveRealDataEvent e)
        {
            try
            {
                if (e.sRealType.Trim() == "주식체결")
                {
                    string realcode = RealCode(e.sRealKey.Trim());
                    List<StockChecker> stocks = stockCheckers.FindAll(a => a.code == realcode);

                    foreach (StockChecker stock in stocks)
                    {
                        if (stock == null)
                            return;
                        if (Math.Abs(int.Parse(axKHOpenAPI1.GetCommRealData(e.sRealType, 10).Trim())) != stock.price.price)
                        {
                            stock.AddPriceData(Math.Abs(int.Parse(axKHOpenAPI1.GetCommRealData(e.sRealType, 10).Trim())), axKHOpenAPI1.GetCommRealData(e.sRealType, 20).Trim());
                            stock.fluctuation = float.Parse(axKHOpenAPI1.GetCommRealData(e.sRealType, 12).Trim());
                        }
                    }
                }
                //if (e.sRealType.Trim() == "주식시세")
                //{
                //    for (int i = 0; i < stockCheckers.Count; i++)
                //    {
                //        if (stockCheckers[i].code == e.sRealKey.Trim())
                //        {
                //            stockCheckers[i].AddPriceData(Math.Abs(int.Parse(axKHOpenAPI1.GetCommRealData(e.sRealType, 10).Trim())));
                //            stockCheckers[i].fluctuation = float.Parse(axKHOpenAPI1.GetCommRealData(e.sRealType, 12).Trim());
                //            break;
                //        }
                //    }
                //}
                else if (e.sRealType.Trim() == "주식호가잔량")
                {
                    string realcode = RealCode(e.sRealKey.Trim());
                    List<StockChecker> stocks = stockCheckers.FindAll(a => a.code == realcode);
                    foreach (StockChecker stock in stocks)
                    {
                        if (stock == null)
                            return;
                        for (int j = 0; j < 10; j++)
                        {
                            stock.gridView.Rows[j].Cells[0].Value = string.Format("{0:#,###}", Math.Abs(int.Parse(axKHOpenAPI1.GetCommRealData(e.sRealType, 50 - j).Trim())));
                            stock.prices[j] = Math.Abs(int.Parse(axKHOpenAPI1.GetCommRealData(e.sRealType, 50 - j).Trim()));
                            stock.gridView.Rows[19 - j].Cells[0].Value = string.Format("{0:#,###}", Math.Abs(int.Parse(axKHOpenAPI1.GetCommRealData(e.sRealType, 60 - j).Trim())));
                            stock.prices[19 - j] = Math.Abs(int.Parse(axKHOpenAPI1.GetCommRealData(e.sRealType, 60 - j).Trim()));
                        }
                        if (stock.is_playing)
                            stock.ReloadHoga();
                    }
                }
            }
            catch (Exception ex)
            {
                Send_Log("axKHOpenAPI1_OnReceiveRealData() [" + ex.Message + "]");
            }
            ShowMemory();
        }

        public void ShowMemory()
        {
            // 1. Obtain the current application process
            Process currentProcess = Process.GetCurrentProcess();
            // 2. Obtain the used memory by the process
            long usedMemory = currentProcess.PrivateMemorySize64;
            label16.Text = "메모리 : " + string.Format("{0:#,###}", usedMemory);
        }


        //시간 비교 함수(시간,분 만 비교) 1이면 a가 더 큼, -1이면 b가 더 큼, 0이면 두개 같음.
        static public int CompareTime(DateTime a, DateTime b)
        {
            if (a.Hour - b.Hour > 0)
            {
                return 1;
            }
            else if (a.Hour - b.Hour < 0)
            {
                return -1;
            }
            else
            {
                if (a.Minute - b.Minute > 0)
                {
                    return 1;
                }
                else if (a.Minute - b.Minute < 0)
                {
                    return -1;
                }
                else
                {
                    if (a.Second - b.Second > 0)
                    {
                        return 1;
                    }
                    else if (a.Second - b.Second < 0)
                    {
                        return -1;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
        }

        public void AddList(string code)
        {
            axKHOpenAPI1.SetInputValue("계좌번호", g_accnt_no);
            axKHOpenAPI1.SetInputValue("비밀번호", "");
            axKHOpenAPI1.SetInputValue("상장폐지조회구분", "1");
            axKHOpenAPI1.SetInputValue("비밀번호입력매체구분", "00");
            axKHOpenAPI1.SetInputValue("종목코드", code);
            int temp = axKHOpenAPI1.CommRqData("주식기본정보", "opt10007", 0, "0101");
            if (temp != 0)
            {
                Send_Log(g_accnt_no + " 종목추가 오류: " + temp);
            }
            else
            {
                Send_Log(g_accnt_no + " 종목추가 정상 접수: " + code);
            }
        }

        static public int CompareTime(DateTime a, int h, int m, int s)
        {
            if (a.Hour - h > 0)
            {
                return 1;
            }
            else if (a.Hour - h < 0)
            {
                return -1;
            }
            else
            {
                if (a.Minute - m > 0)
                {
                    return 1;
                }
                else if (a.Minute - m < 0)
                {
                    return -1;
                }
                else
                {
                    if (a.Second - s > 0)
                    {
                        return 1;
                    }
                    else if (a.Second - s < 0)
                    {
                        return -1;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
        }

        public string get_scr_no() //Open API 화면번호 가져오기 메서드
        {
            if (g_scr_no < 1180)    //화면번호를 200개 이상 사용하면 프로그램 오작동
                g_scr_no++;
            else
                g_scr_no = 1000;
            return g_scr_no.ToString();
        }

        //로그인 버튼
        private void button1_Click(object sender, EventArgs e)
        {
            axKHOpenAPI1.CommConnect();
        }

        public int Medo(string name, string current_accnt_no, string scrnum, string code, int count, int price = -1)
        {
            if (name == "테스트")
                return -1;
            if (count > 0)
            {
                int errcode;
                if (price > 0)
                {
                    errcode = axKHOpenAPI1.SendOrder("주식주문", scrnum, current_accnt_no, 2, code, count, price, "00", "");
                }
                else
                {
                    errcode = axKHOpenAPI1.SendOrder("주식주문", scrnum, current_accnt_no, 2, code, count, 0, "03", "");
                }
                
                if (errcode == 0)
                {
                    if (medoSoundOn && medoSound != null)
                    {
                        PlayEffect(0);
                    }
                    //Send_Log(name + " : " + count + " 개 " + string.Format("{0:#,###}", price) + "원 지정가 매도");
                }
                else
                {
                    Send_Log(current_accnt_no + " " + name + " : 매도주문실패 code: " + errcode);
                }
                return errcode;
            }
            return -1;
        }

        public int Mesu(string name, string current_accnt_no, string scrnum, string code, int count, int price = -1)
        {
            if (name == "테스트")
                return -1;
            if (count > 0)
            {
                int errcode;
                if (price > 0)
                {
                    errcode = axKHOpenAPI1.SendOrder("주식주문", scrnum, g_accnt_no, 1, code, count, price, "00", "");
                }
                else
                {
                    errcode = axKHOpenAPI1.SendOrder("주식주문", scrnum, g_accnt_no, 1, code, count, 0, "03", "");
                }

                if (errcode == 0)
                {
                    PlayEffect(2);
                    //Send_Log(name + " : " + count + " 개 " + string.Format("{0:#,###}", price) + "원 지정가 매수");
                }
                else
                {
                    Send_Log(current_accnt_no + " " + name + " : 매수주문실패 code: " + errcode);
                    PlayEffect(25);
                }
                return errcode;
            }
            return -1;
        }

        public void PlayEffect(int code)
        {
            if(code < 0)
            {
                Send_Log_Debug("PlayEffect : code < 0");
                return;
            }

            if (soundSettings[code].sound)
            {
                if(soundSettings[code].path != "")
                    PlayFile(soundSettings[code].path);
            }
            if(soundSettings[code].window)
            {
                TopMost = true;
                TopMost = false;
            }
            if (soundSettings[code].highLight)
            {
                //반짝임 구현
            }
        }

        public void PlayFile(string path)
        {
            try
            {
                player = new WMPLib.WindowsMediaPlayer();
                player.URL = path;
                player.controls.play();
            }
            catch (Exception e)
            {
                Send_Log("PlayFile() [" + e.Message + "]");
            }
        }

        public string RealCode(string code)
        {
            while(true)
            {
                if (code[0] < '0' || code[0] > '9')
                    code = code.Remove(0, 1);
                else
                    break;
            }
            return code;
        }

        protected override Point ScrollToControl(Control activeControl)
        {
            Point pt = this.AutoScrollPosition;
            return pt;
        }

        protected override void AdjustFormScrollbars(bool displayScrollbars)
        {
            return;
        }


        private void button5_Click(object sender, EventArgs e)
        {
            if (allPlay)
            {
                AllStop();
            }
            else
            {
                AllStart();
            }
        }

        public void AllStartSound()
        {
            allPlaySound = true;

            if (this.button10.InvokeRequired)
            {
                button10.BeginInvoke(new Action(() =>
                {
                    button10.Text = "전체변동알림";
                    button10.BackColor = Color.FromArgb(255, 210, 210);
                }
                ));
            }
            else
            {
                button10.Text = "전체변동알림";
                button10.BackColor = Color.FromArgb(255, 210, 210);
            }
        }

        public void AllStopSound()
        {
            allPlaySound = false;
            button10.Text = "개별변동알림";
            button10.BackColor = Color.FromArgb(210, 210, 255);
        }

        public void AllStart()
        {
            allPlay = true;
            for (int i = 0; i < stockCheckers.Count; i++)
            {
                stockCheckers[i].SetPanelColor();
                if (stockCheckers[i].is_playing)
                    stockCheckers[i].Play();
                stockCheckers[i].ReloadPanel();
            }

            if (this.button5.InvokeRequired)
            {
                button5.BeginInvoke(new Action(() =>
                {
                    button5.Text = "전체 감시중";
                    button5.BackColor = Color.FromArgb(255, 210, 210);
                }
                ));
            }
            else
            {
                button5.Text = "전체 감시중";
                button5.BackColor = Color.FromArgb(255, 210, 210);
            }
        }

        public void AllStop()
        {
            allPlay = false;
            for (int i = 0; i < stockCheckers.Count; i++)
            {
                stockCheckers[i].SetPanelColor();
                if (!stockCheckers[i].is_playing)
                    stockCheckers[i].Stop();
                stockCheckers[i].ReloadPanel();
            }
            button5.Text = "전체 정지중";
            button5.BackColor = Color.FromArgb(210, 210, 255);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Form9 form9 = new Form9(this, g_accnt_no);
            form9.ShowDialog();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Form10 form10 = new Form10(this);
            form10.ShowDialog();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (mesuSoundOn && mesuSound != null)
            {
                PlayEffect(2);
            }
        }

        private void button8_Click_1(object sender, EventArgs e)
        {
            allPlayMedo = !allPlayMedo;
            if(allPlayMedo)
            {
                button8.Text = "매도 실행중";
                button8.BackColor = Color.FromArgb(255, 210, 210);
            }
            else
            {
                button8.Text = "매도 정지중";
                button8.BackColor = Color.FromArgb(210, 210, 255);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            allPlayMesu = !allPlayMesu;
            if (allPlayMesu)
            {
                button9.Text = "매수 실행중";
                button9.BackColor = Color.FromArgb(255, 210, 210);
            }
            else
            {
                button9.Text = "매수 정지중";
                button9.BackColor = Color.FromArgb(210, 210, 255);
            }
        }

        private void logbox_TextChanged(object sender, EventArgs e)
        {
            autoScrollLog = false;
        }

        private void label16_Click(object sender, EventArgs e)
        {
            ShowMemory();
            Send_Log(label16.Text);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if(allPlaySound)
            {
                AllStopSound();
            }
            else
            {
                AllStartSound();
            }
        }

        public void SetFirst(int ind, StockChecker st)
        {
            if (ind == 0)
                return;

            st.index = 0;
            flowLayoutPanel1.Controls.SetChildIndex(st.pan, 0);
            st.pan.Name = "panel" + (st.index + 1);
            for (int i = 0; i < ind; i++)
            {
                stockCheckers[i].index += 1;
                stockCheckers[i].pan.Name = "panel" + (stockCheckers[i].index + 1);
                stockCheckers[i].ReloadPanel();

                stockCheckers[i].pan.Controls["name"].Text = stockCheckers[i].name;
            }
            st.ReloadPanel();
            st.pan.Controls["name"].Text = st.name;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            SaveSettings();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            int pricedata;
            try
            {
                pricedata = int.Parse(textBox2.Text);
                int index = insert_tb_accnt_info("000000", "테스트", g_accnt_no, 10000, 100, pricedata);
            }
            catch (Exception)
            {
                Send_Log("정수값 입력 오류");
            }

        }

        private void button14_Click(object sender, EventArgs e)
        {
            if(logbox.Enabled == true)
            {
                logbox.Enabled = false;
                logbox.Visible = false;
                button14.Text = "보이기";
            }
            else
            {
                logbox.Enabled = true;
                logbox.Visible = true;
                button14.Text = "숨기기";
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            if (conditionWindow == null)
            {
                Form12 form12 = new Form12(this);
                form12.Show();
            }
            else
            {
                conditionWindow.Select();
            }
        }

        public static int PriceMoveTick(string code, string name, int price, int tick)
        {
            ConditionTransactionType type = GetConditionTransactionTypeByCode(code, name);

            if (tick > 0)
            {
                while (true)
                {
                    if (tick == 0)
                        break;

                    price += GetPriceUnit(price, type, false);
                    tick -= 1;
                }
            }
            else if (tick < 0)
            {
                while (true)
                {
                    if (tick == 0)
                        break;
                    price -= GetPriceUnit(price, type, true);
                    tick += 1;
                }
            }

            return price;
        }

        public enum ConditionTransactionType
        {
            ELW = 3,
            muchual = 4,
            sinjuinsu = 5,
            rich = 6,
            ETF = 8,
            highld = 9,
            Kosdaq = 10,
            no3 = 30,
            ETN = 60,
            Other = 0
        }


        public static ConditionTransactionType GetConditionTransactionTypeByCode(string code, string name)
        {
            bool IsTargetInArray(string[] array, string target)
            {
                for (int i = 0; i < array.Length; i++)
                {
                    if (array[i] == target)
                        return true;
                }
                return false;
            }

            // 키움 API에서 목록을 지원해주지 않아 종목 명에 ETN이 포함되는지 확인
            if (name.Contains("ETN"))
                return ConditionTransactionType.ETN;

            if (IsTargetInArray(Kosdaq, code))
                return ConditionTransactionType.Kosdaq;
            else if (IsTargetInArray(ETF, code))
                return ConditionTransactionType.ETF;
            else if (IsTargetInArray(ELW, code))
                return ConditionTransactionType.ELW;
            else if (IsTargetInArray(muchual, code))
                return ConditionTransactionType.muchual;
            else if (IsTargetInArray(sinjuinsu, code))
                return ConditionTransactionType.sinjuinsu;
            else if (IsTargetInArray(rich, code))
                return ConditionTransactionType.rich;
            else if (IsTargetInArray(highld, code))
                return ConditionTransactionType.highld;
            else if (IsTargetInArray(no3, code))
                return ConditionTransactionType.no3;
            else
                return ConditionTransactionType.Other;
        }

        public static int GetPriceUnit(int price, ConditionTransactionType type, bool isDownward = false)
        {
            /* 아래 내용을 참고하여 호가를 업데이트 함 - 김동완 2025.05.14
            가격대	        단위
            ~ 2천원	        1원
            2천 ~ 5천원	    5원
            5천 ~ 2만원	    10원
            2만 ~ 5만원	    50원
            5만 ~ 20만원    100원
            20만 ~ 50만원	500원
            50만원 ~	    1,000원


            https://samsungpop.com/ux/kor/customer/notice/notice/noticeViewContent.do?MenuSeqNo=19236
            */

            // 경계선에 있는 가격이 아래로 내려갈 때의 호가간격을 알아내기 위해 -1하여 아래 단위를 보여주게 함.
            // 예: 2000원의 호가 간격은 아래로는 1원 위로는 5원임. - 김동완 2025.05.14
            if (isDownward == true)
                price--;

            int result = 0;
            if (type == ConditionTransactionType.ETF || type == ConditionTransactionType.ETN)
            {
                if (price < 2000)
                {
                    result = 1;
                }
                else
                {
                    result = 5;
                }
            }
            else
            {
                if (price < 2000)
                {
                    result = 1;
                }
                else if (price < 5000)
                {
                    result = 5;
                }
                else if (price < 20000)
                {
                    result = 10;
                }
                else if (price < 50000)
                {
                    result = 50;
                }
                else if (price < 200000)
                {
                    result = 100;
                }
                else if (price < 500000)
                {
                    result = 500;
                }
                else
                {
                    result = 1000;
                }
            }
            return result;
        }

        public string GetMainAccountNum()
        {
            return g_accnt_no;
        }

        public int GetAccntIndex(string _accno)
        {
            for (int i = 0;  i < l_accno_arr.Length; i++)
                if (l_accno_arr[i] == _accno)
                    return i;
            return -1;
        }

        private void buttonAccountNumber_Click(object sender, EventArgs e)
        {
            AccountNumbersForm accountNumbersForm = new AccountNumbersForm(l_accno_arr, enabled_accno_arr, (result) =>
            {
                this.enabled_accno_arr = result;
                Updateg_accnt_no();

                string enabledAccountNumbersString = "";
                for (int i = 0; i < l_accno_arr.Length; i++)
                {
                    if (enabled_accno_arr[i] == true)
                    {
                        enabledAccountNumbersString += l_accno_arr[i];
                        if (i + 1 < l_accno_arr.Length)
                            enabledAccountNumbersString += ", ";
                    }
                }
                Send_Log("사용할 증권계좌번호는 : [" + enabledAccountNumbersString + "] 입니다. \n");
                return;
            });

            accountNumbersForm.Show();
        }

        public void Updateg_accnt_no()
        {
            for (int i = 0; i < l_accno_arr.Length; i++)
            {
                if (enabled_accno_arr[i] == true)
                {
                    g_accnt_no = l_accno_arr[i];
                    return;
                }
            }
            g_accnt_no = l_accno_arr[0];
        }
    }

    public class SoundSetting
    {
        public string name;
        public string path;
        public bool sound;
        public bool window;
        public bool highLight;
    }

    //실제 알고리즘을 실행하는 클래스
    public class StockChecker
    {
        public enum StockState
        {
            none = 0,
            selling = 1,
            sellingstart = 2,
            buying = 3,
            buyingstart = 4,
            sellwait = 5,
            buywait = 6,
            pros = 7
        }

        public class PriceData
        {
            public int price;
            public Time time = new Time();
        }

        private readonly SpeechSynthesizer _speechSynthesizer = new SpeechSynthesizer();

        public string name = null;              //종목명
        public string accnt_no;
        public string code = null;              //종목코드
        public ConditionTransactionType kind = 0;
        public StockState state = StockState.none;       //현재상태
        public int realprice;                   //매입가
        public PriceData markPrice = new PriceData();
        public float sell_start = 1;            //매도하기 위한 수익(시작가)
        public float sell_end = 10;             //매도하기 위한 수익(종료가)
        public int sell_kind = 1;               //매도 종류
        public float buy_start = 1;             //매수하기 위한 가격(시작가)
        public float buy_end = 10;              //매수하기 위한 가격(종료가)
        public int buy_kind = 1;                //매수 종류
        public float buy_count = 100;           //매수시 주문할 개수 (총 비중)
        public float buy_per = 100;             //매수시 주문할 개수 (변동수량)
        public float sell_count = 100;          //매도시 주문할 개수 (총 비중)
        public float sell_per = 100;          //매도시 주문할 개수 (변동수량)
        public int count;                       //보유 개수
        public bool is_sell = false;             //매도를 할 것인가
        public bool is_buy = false;              //매수를 할 것인가
        public bool is_selltime = false;
        public bool is_buytime = false;
        public PriceData price = new PriceData(); //종목 가격 데이터
        public bool is_cut = false;              //손절여부
        public float cut_price = 5;             //손절가( % )
        public bool cut_per = true;
        public float fluctuation;               //등락율
        public float benefit;                   //수익율

        public int low_price;                   //계산용 최저가
        public int high_price;                  //계산용 최고가
        public int sell_price;                  //계산용 매도기준
        public int buy_price;                   //계산용 매수기준
        public int real_sell_price;             //실제 매도가
        public int real_buy_price;              //실제 매도가
        public float now_sell_count = 0;
        public float now_buy_count = 0;
        public int remainCountVal = 0;              //남길개수
        
        /// 고매도가 실행중인지
        public bool is_highmedo;
        /// 고매도가 비중으로 실행중인지
        public bool is_highmedo_per;
        /// 상풀림매도가 실행중인지
        public bool is_endLimit;
        /// 고매도 퍼센트 값
        public int highMedo_per;
        public float highMedo_perval;
        /// 고매도 가격 값
        public int highMedo_price;
        public int all_high_price = 0;

        /// 상승매수가 실행중인지
        public bool is_highmesu;
        public int highmesu_markprice = 0;
        public int highmesu_highprice = 0;
        public float highbuy_start = 1;
        public float highbuy_end = 10;
        public float highbuy_count = 100;           //상승매수시 주문할 개수 (총 비중)
        public float highbuy_per = 100;             //상승매수시 주문할 개수 (변동수량)
        public float now_highbuy_count = 0;       


        public bool end_clear;                  //종료매매
        public int targetPrice;                 //목표가격
        public int targetCount;                 //목표가격
        public bool targetisprice = true;
        public int priceKind;                   //가격대

        public int MarketKind;                  //거래소구분

        public bool is_rebuy;                   //재매수
        public bool haveEffect;                 //유효매매
        public bool is_fixed = false;           //고정종목

        public int limitprice = 0;              //상한가
        public bool limitprice_on = false;      //상한가를 도달 했었는가


        public int[] sell_conditions = new int[7];
        public int[] buy_conditions = new int[7];
        public int[] sell_kinds = new int[7];
        public int[] buy_kinds = new int[7];

        public int[] prices = new int[20];

        public bool is_playing = false;
        public int index;
        public bool auto_sell_price = true;
        public bool auto_buy_price = true;
        public int poscount = 0;
        public bool check = true;
        public string junum;
        public string realtime = "";
        Form1 main;

        public DataGridView gridView;

        public int purchase_price;
        public float soundmark = -10000;
        public bool is_price_change_sound = false;//변동알림

        public Panel pan;

        public bool canbuy = true;

        public StockChecker(string _code, string _name, string _accnt_no, int _count, Form1 form1)
        {
            code = _code;
            name = _name;
            accnt_no = _accnt_no;
            count = _count;
            main = form1;
            for (int i = 0; i < sell_conditions.Length; i++)
            {
                sell_conditions[i] = main.sell_conditions[i];
                buy_conditions[i] = main.buy_conditions[i];
            }
            sell_end = main.sell_end;
            sell_start = main.sell_start;
            sell_per = main.sell_per;
            buy_start = main.buy_start;
            buy_end = main.buy_end;
            buy_per = main.buy_per;
            highbuy_start = main.highbuy_start;
            highbuy_end = main.highbuy_end;
            highbuy_per = main.highbuy_per;
        }

        public bool isThis(string _code, string _accnt_no)
        {
            return code == _code && accnt_no == _accnt_no;
        }

        public void Play()
        {
            main.Send_Log(name + " 실행");
            state = StockState.none;
            markPrice.price = price.price;
            if (targetisprice)
                targetCount = (targetPrice / markPrice.price);
            else
                targetPrice = targetCount * price.price;
            SetCountColor();
            ((Label)pan.Controls["targetcount"]).Text = "목표수량: " + targetCount;
            ((Label)pan.Controls["targetprice_text"]).Text = "목표가: " + targetPrice / 10000 + "만원";
            ((Label)pan.Controls["panel3"].Controls["label12"]).Text = "기준가: " + string.Format("{0:#,###}", markPrice.price);
            markPrice.time.hour = price.time.hour;
            markPrice.time.min = price.time.min;
            markPrice.time.sec = price.time.sec;
            buy_count = targetCount / (buy_end - buy_start) * buy_per / 100;
            sell_count = targetCount / (sell_end - sell_start) * sell_per / 100;
            highmesu_markprice = price.price;
            highmesu_highprice = price.price;
            ((Label)pan.Controls["label15"]).Text = "상승기준: " + string.Format("{0:#,###}", highmesu_markprice);
            if (is_highmedo)
            {
                if (is_highmedo_per)
                {
                    highMedo_per = (int)(price.price * highMedo_perval / 100f);
                    ((Label)pan.Controls["label6"]).Text = "고매:" + highMedo_perval.ToString("##0.0") + ":" + string.Format("{0:#,###}", all_high_price - highMedo_per);
                }
            }
            SetCounts();
            low_price = price.price;
            high_price = price.price;
            ((Label)pan.Controls["label9"]).Text = "대기중";
            ReloadPanel();
            SetPanelColor();
            ReloadHoga();
        }

        public void Stop()
        {
            main.Send_Log(accnt_no + " " + name + " 정지");
            state = StockState.none;
            ((Label)pan.Controls["label9"]).Text = "대기중";
            SetPanelColor();
            ReloadHoga();
        }

        public void EVT_ChangePlaying(object sender, EventArgs e)
        {
            if (is_playing == false)
            {
                is_playing = true;
                if (main.allPlay)
                {
                    Play();
                }
            }
            else
            {
                is_playing = false;
                Stop();
            }
            main.xmlData.SetData("C" + code + accnt_no + ".is_playing", is_playing.ToString());
            SetPanelColor();
            ReloadHoga();
        }

        public void EVT_ChangeHighMesu(object sender, EventArgs e)
        {
            is_highmesu = ((CheckBox)sender).Checked;
            main.Send_Log(accnt_no + " " + name + " 상승매수 변경");
            highmesu_markprice = price.price;
            highmesu_highprice = price.price;
            ((Label)pan.Controls["label15"]).Visible = (is_sell && is_highmesu);
            ((Label)pan.Controls["label15"]).Text = "상승기준: " + string.Format("{0:#,###}", highmesu_markprice);
        }

        public void EVT_ChangeMesu(object sender, EventArgs e)
        {
            is_buy = ((CheckBox)sender).Checked;
            main.Send_Log(accnt_no + " " + name + " 매수 변경");
            SetPanelColor();
        }
        

        public void EVT_ChangeMedo(object sender, EventArgs e)
        {
            is_sell = ((CheckBox)sender).Checked;
            main.Send_Log(accnt_no + " " + name + " 매도 변경");
            highmesu_markprice = price.price;
            highmesu_highprice = price.price;
            ((Label)pan.Controls["label15"]).Visible = (is_sell && is_highmesu);
            ((Label)pan.Controls["label15"]).Text = "상승기준: " + string.Format("{0:#,###}", highmesu_markprice);
            SetPanelColor();
        }
        

        public void EVT_ChangeEndClear(object sender, EventArgs e)
        {
            end_clear = ((CheckBox)sender).Checked;
        }

        public void EVT_ChangeRebuy(object sender, EventArgs e)
        {
            is_rebuy = ((CheckBox)sender).Checked;
        }

        public void EVT_ChangeTargetPrice(object sender, EventArgs e)
        {
            Action<int> action = (a) =>
            {
                this.targetPrice = a * 10000;
                this.targetisprice = true;
                ((Label)pan.Controls["targetprice_text"]).Text = "목표가: " + string.Format("{0:#,###}", a) + "만원";
                main.Send_Log(accnt_no + " " + name + " 목표가" + a + "만 변경");
                SetCounts();
                ReloadHoga();
            };
            main.CallValueWindowInt(name + " : 목표가 설정", action);
        }

        public void EVT_ChangeTargetCount(object sender, EventArgs e)
        {
            Action<int> action = (a) =>
            {
                this.targetCount = a;
                this.targetisprice = false;
                this.targetPrice = 0;
                ((Label)pan.Controls["targetprice_text"]).Text = "목표가: "+ (targetCount * price.price) / 10000 + "만원";
                ((Label)pan.Controls["targetcount"]).Text = "목표개수: " + string.Format("{0:#,###}", a) + "개";
                main.Send_Log(accnt_no + " " + name + " 목표개수" + a + "개 변경");
                SetCounts();
                ReloadHoga();
            };
            main.CallValueWindowInt(name + " : 목표개수 설정", action);
        }

        public void EVT_ChangeMarkPrice(object sender, EventArgs e)
        {
            Action<int> action = (a) =>
            {
                markPrice.price = a;
                highmesu_markprice = a;
                ((Label)pan.Controls["label15"]).Visible = (is_sell && is_highmesu);
                ((Label)pan.Controls["label15"]).Text = "상승기준: " + string.Format("{0:#,###}", highmesu_markprice);
                main.Send_Log(accnt_no + " " + name + " 기준가" + a + "원 변경");
                ReloadPanel();
                SetCounts();
                ReloadHoga();
            };
            main.CallValueWindowInt(name + " : 기준가 설정", action);
        }

        public void EVT_ChangeRemainCount(object sender, EventArgs e)
        {
            Action<int> action = (a) =>
            {
                if (a >= 0)
                {
                    this.remainCountVal = a;
                    ((Label)pan.Controls["label7"]).Text = "남김: " + remainCountVal + "개";
                    main.Send_Log(accnt_no + " " + name + " 남길수" + remainCountVal + "개 변경");
                    ReloadHoga();
                }
            };
            main.CallValueWindowInt(name + " : 남길수 설정", action);
        }

        public void EVT_ChangeHighMedo(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked == true)
            {
                this.is_highmedo = true;
            }
            else
            {
                this.is_highmedo = false;
            }
        }

        public void EVT_ChangeCutPrice(object sender, EventArgs e)
        {
            Action<float, bool> action = (a, b) =>
            {
                if (b)
                {
                    this.cut_price = a;
                    if (a < 100)
                    {
                        cut_per = true;
                        ((Label)pan.Controls["label11"]).Text = "손절: " + cut_price.ToString("##0.0") + "%" + Environment.NewLine + string.Format("{0:#,###}", realprice * ((100 - cut_price) / 100f));
                    }
                    else
                    {
                        cut_per = false;
                        ((Label)pan.Controls["label11"]).Text = "손절: " + string.Format("{0:#,###}", cut_price) + "원";
                    }

                }
                else
                {
                }
            };
            main.CallValueWindowFloat(name + " : 손절 설정", action);
        }

        public void EVT_ChangeCut(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked == true)
            {
                this.is_cut = true;
            }
            else
            {
                this.is_cut = false;
            }
        }

        public void EVT_ChangeMedoTick(object sender, EventArgs e)
        {
            main.CallValueWindowTick(name + " : 매매틱 설정", this);
        }

        public void EVT_ChangeMesuTick(object sender, EventArgs e)
        {
            main.CallValueWindowTick(name + " : 매매틱 설정", this);
        }

        public void EVT_ChangePer(object sender, EventArgs e)
        {
            Form6 form6 = new Form6(main, this);
            form6.Name = name + " 비중설정";
            form6.ShowDialog();
        }

        public void EVT_ChangeHaveEffect(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked == true)
            {
                this.haveEffect = true;
            }
            else
            {
                this.haveEffect = false;
            }
        }

        public void EVT_Remove(object sender, EventArgs e)
        {
            main.Send_Log(accnt_no + " " + name + " 목록제거");
            is_playing = false;
            main.xmlData.SetData("C" + code + accnt_no + ".is_playing", is_playing.ToString());
            main.stockCheckers.Remove(this);
            pan.Dispose();
            for (int i = 0; i < main.stockCheckers.Count; i++)
            {
                if (main.stockCheckers[i].index > index)
                {
                    main.stockCheckers[i].index -= 1;
                }
                main.Controls["flowLayoutPanel1"].Controls[i].Name = "panel" + (main.stockCheckers[i].index + 1);
            }

            if (realtime != "")
            {
                main.Stopreal(realtime);
            }
            return;
        }

        public void EVT_ChangeEndlimit(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked == true)
            {
                this.is_endLimit = true;
            }
            else
            {
                this.is_endLimit = false;
            }
        }

        public void EVT_ChangePriceChangeSound(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked == true)
            {
                this.is_price_change_sound = true;
                soundmark = fluctuation;
            }
            else
            {
                this.is_price_change_sound = false;
            }
        }
        public void EVT_SetFirst(object sender, EventArgs e)
        {
            main.SetFirst(index, this);
        }

        /// <summary>
        /// 목표개수, 매도 / 매수 개수 재설정
        /// </summary>
        public void SetCounts()
        {
            if (is_playing && main.allPlay)
            {
                if (targetisprice)
                    targetCount = (targetPrice / markPrice.price);
                else
                    targetPrice = targetCount * price.price;
                SetCountColor();
                ((Label)pan.Controls["targetcount"]).Text = "목표수량: " + targetCount;
                ((Label)pan.Controls["targetprice_text"]).Text = "목표가: " + targetPrice / 10000 + "만원";
                buy_count = targetCount / (buy_end - buy_start) * buy_per / 100;
                highbuy_count = targetCount / (highbuy_end - highbuy_start) * highbuy_per / 100;
                sell_count = targetCount / (sell_end - sell_start) * sell_per / 100;
            }
            else
            {
                if (targetisprice)
                    targetCount = (targetPrice / price.price);
                else
                    targetPrice = targetCount * price.price;
                SetCountColor();
                ((Label)pan.Controls["targetcount"]).Text = "목표수량: " + (targetPrice / price.price);
                ((Label)pan.Controls["targetprice_text"]).Text = "목표가: " + targetPrice / 10000 + "만원";
            }
            ((Label)pan.Controls["label7"]).Text = "남김: " + remainCountVal + "개";
        }

        /// <summary>
        /// 고매도점 가격 설정
        /// </summary>
        public void EVT_ChangeHighMedoPrice(object sender, EventArgs e)
        {
            Action<float, bool> action = (a, b) =>
            {
                if (b)
                {
                    if (a <= 100)
                    {
                        highMedo_perval = a;
                        highMedo_per = (int)(price.price * highMedo_perval / 100f);
                        is_highmedo_per = true;
                    }
                    else
                    {
                        if((int)a > price.price)
                        {
                            main.Send_Log("고점매도 : 현재 금액보다 높은 입력 값 입니다.");
                            return;
                        }
                        highMedo_price = (int)a;
                        is_highmedo_per = false;
                    }
                    all_high_price = price.price;
                    ReloadPanel();
                }
                else
                {
                }
            };
            main.CallValueWindowFloat(name + " : 고점매도 설정", action);

        }

        /// <summary>
        /// 가격대 확인 함수
        /// </summary>
        public void CheckPriceKind()
        {
            if (price.price >= 500000)
            {
                priceKind = 0;
            }
            else if (price.price >= 200000)
            {
                priceKind = 1;
            }
            else if (price.price >= 50000)
            {
                priceKind = 2;
            }
            else if (price.price >= 20000)
            {
                priceKind = 3;
            }
            else if (price.price >= 5000)
            {
                priceKind = 4;
            }
            else if (price.price >= 2000)
            {
                priceKind = 5;
            }
            else
            {
                priceKind = 6;
            }
            ((Label)pan.Controls["panel3"].Controls["label17"]).Text = sell_conditions[priceKind] + " " + (int)sell_start + "~" + (int)sell_end;
            ((Label)pan.Controls["panel3"].Controls["label18"]).Text = sell_per.ToString();

            ((Label)pan.Controls["panel3"].Controls["label21"]).Text = buy_conditions[priceKind] + " " + (int)buy_start + "~" + (int)buy_end;
            ((Label)pan.Controls["panel3"].Controls["label20"]).Text = buy_per.ToString();
        }

        public void Medo(string time, int sellcount = -1)
        {
            if (sellcount <= 0 && sellcount != -1)
                return;
            if ((Math.Min(price.price, (sell_end + 100) * markPrice.price / 100f) - Math.Max(markPrice.price * (100 + sell_start) / 100f, real_sell_price)) / markPrice.price * 100f * sell_count <= 0)
                return;
            try
            {
                string total;
                float totalcount;
                if (time != "")
                    total = ("" + time[0] + time[1] + ":" + time[2] + time[3] + ":" + time[4] + time[5] + " ");
                else
                    total = DateTime.Now.ToString("HH:mm:ss") + " ";
                //now_sell_count += (Math.Min(markPrice.price + (markPrice.price * sell_end / 100), high_price) - last_sell_price) / markPrice.price * sell_count;
                if(sellcount == -1)
                    totalcount = now_sell_count + (Math.Min(price.price, (sell_end + 100) * markPrice.price / 100f) - Math.Max(markPrice.price * (100 + sell_start) / 100f, real_sell_price)) / markPrice.price * 100f * sell_count;
                else
                    totalcount = sellcount;

                if (totalcount < 0 || totalcount > (sell_end - sell_start) * sell_count)
                {
                    main.Send_Log_Debug("accnt_no : " + accnt_no);
                    main.Send_Log_Debug("name : " + name);
                    main.Send_Log_Debug("now_sell_count : " + now_sell_count);
                    main.Send_Log_Debug("totalcount : " + totalcount);
                    main.Send_Log_Debug("price.price : " + price.price);
                    main.Send_Log_Debug("markPrice.price : " + markPrice.price);
                    main.Send_Log_Debug("sell_start : " + sell_start);
                    main.Send_Log_Debug("sell_end : " + sell_end);
                    main.Send_Log_Debug("real_sell_price : " + real_sell_price);
                    main.Send_Log_Debug("sell_count : " + sell_count);
                }



                ((Label)pan.Controls["label9"]).Text = "대기중";
                ((Label)pan.Controls["label9"]).ForeColor = SystemColors.ControlText;

                if (totalcount > 0)
                {

                    if (totalcount > (int)(count - remainCountVal))
                    {
                        if ((int)(count - remainCountVal) > 0)
                        {
                            main.Send_Log(accnt_no + " " + total + name + " " + (int)(count - remainCountVal) + "개 " + string.Format("{0:#,###}", price.price) + "원 매도_부족 매도시 기준가: " + string.Format("{0:#,###}", markPrice.price) + " 최고가: " + string.Format("{0:#,###}", high_price), false);
                            state = StockState.selling;
                            real_sell_price = price.price;
                            if (sell_kinds[priceKind] == 0)
                            {
                                if (main.allPlayMedo)
                                {
                                    main.Medo(name, accnt_no, main.get_scr_no(), code, (int)(count - remainCountVal), price.price);
                                }
                            }
                            else if (sell_kinds[priceKind] == 1)
                            {
                                if (main.allPlayMedo)
                                {
                                    main.Medo(name, accnt_no, main.get_scr_no(), code, (int)(count - remainCountVal));
                                }
                            }
                            totalcount -= (float)Math.Ceiling(totalcount);
                            now_sell_count = totalcount;
                        }
                        else
                        {
                            if (!haveEffect)
                            {
                                main.Send_Log_Debug(accnt_no + " " + total + name + " 0개 " + string.Format("{0:#,###}", price.price) + "원 매도_부족 매도시 기준가: " + string.Format("{0:#,###}", markPrice.price) + " 최고가: " + string.Format("{0:#,###}", high_price), false);
                                state = StockState.selling;
                                real_sell_price = price.price;
                                totalcount -= (float)Math.Ceiling(totalcount);
                                now_sell_count = totalcount;
                            }
                            else
                            {
                                state = StockState.sellwait;
                            }
                        }
                    }
                    else
                    {
                        main.Send_Log(accnt_no + " " + total + name + " " + (int)Math.Ceiling(totalcount) + "개 " + string.Format("{0:#,###}", price.price) + "원 매도 매도시 기준가: " + string.Format("{0:#,###}", markPrice.price) + " 최고가: " + string.Format("{0:#,###}", high_price), false);
                        state = StockState.selling;
                        real_sell_price = price.price;
                        if (sell_kinds[priceKind] == 0)
                        {
                            if (main.allPlayMedo)
                            {
                                main.Medo(name, accnt_no, main.get_scr_no(), code, (int)Math.Ceiling(totalcount), price.price);
                            }
                        }
                        else if (sell_kinds[priceKind] == 1)
                        {
                            if (main.allPlayMedo)
                            {
                                main.Medo(name, accnt_no, main.get_scr_no(), code, (int)Math.Ceiling(totalcount));
                            }
                        }
                        totalcount -= (float)Math.Ceiling(totalcount);
                        now_sell_count = totalcount;
                    }

                    int hg = 0;
                    int nw = 0;
                    for (int i = 0; i < 20; i++)
                    {
                        if (prices[i] == high_price)
                        {
                            hg = i;
                        }
                        else if (prices[i] == price.price)
                        {
                            nw = i;
                        }
                    }
                }
                else
                {
                    if (!haveEffect)
                    {
                        main.Send_Log_Debug(accnt_no + " " + total + name + " 0 개 " + string.Format("{0:#,###}", price.price) + "원 매도 처리 매도시 기준가: " + string.Format("{0:#,###}", markPrice.price) + " 최고가: " + string.Format("{0:#,###}", high_price), false);
                        state = StockState.selling;
                        real_sell_price = price.price;
                        totalcount -= (float)Math.Ceiling(totalcount);
                        now_sell_count = totalcount;
                    }
                    else
                    {
                        state = StockState.sellwait;
                    }
                }
            }
            catch (Exception ex)
            {
                main.PlayEffect(25);
                main.Send_Log(accnt_no + " " + "Medo()" + ex.HResult + " [" + ex.Message + "]");
            }
        }

        public void Mesu(string time)
        {
            if (price.price >= real_buy_price)
                return;
            if ((Math.Min(markPrice.price * (100 - buy_start) / 100f, real_buy_price) - Math.Max(price.price, (100 - buy_end) * markPrice.price / 100f)) / markPrice.price * 100f * buy_count < 0)
                return;
            try
            {
                float totalcount;
                string total;
                if (time != "")
                    total = ("" + time[0] + time[1] + ":" + time[2] + time[3] + ":" + time[4] + time[5] + " ");
                else
                    total = DateTime.Now.ToString("HH:mm:ss") + " ";
                //now_buy_count += (last_buy_price - Math.Max(markPrice.price - (markPrice.price * buy_end / 100), low_price)) / markPrice.price * buy_count;
                totalcount = now_buy_count + (Math.Min(markPrice.price * (100 - buy_start) / 100f, real_buy_price) - Math.Max(price.price, (100 - buy_end) * markPrice.price / 100f)) / markPrice.price * 100f * buy_count;
                if (totalcount < 0)
                {
                    main.Send_Log_Debug("accnt_no : " + accnt_no);
                    main.Send_Log_Debug("name : " + name);
                    main.Send_Log_Debug("now_buy_count : " + now_buy_count);
                    main.Send_Log_Debug("totalcount : " + totalcount);
                    main.Send_Log_Debug("price.price : " + price.price);
                    main.Send_Log_Debug("markPrice.price : " + markPrice.price);
                    main.Send_Log_Debug("buy_start : " + buy_start);
                    main.Send_Log_Debug("buy_end : " + buy_end);
                    main.Send_Log_Debug("real_buy_price : " + real_buy_price);
                    main.Send_Log_Debug("buy_count : " + buy_count);
                }
                state = StockState.buying;

                ((Label)pan.Controls["label9"]).Text = "대기중";
                ((Label)pan.Controls["label9"]).ForeColor = SystemColors.ControlText;
                if (totalcount * price.price > main.possible_Amount[main.GetAccntIndex(accnt_no)])
                {
                    if (canbuy)
                    {
                        main.Send_Log(accnt_no + " " + name + " 매수 예수금 부족: " + main.possible_Amount, false);
                        canbuy = false;
                    }
                    int realcount = main.possible_Amount[main.GetAccntIndex(accnt_no)] / price.price;
                    totalcount = realcount;
                }
                else if(canbuy == false)
                {
                    canbuy = true;
                }
                if (totalcount > 0)
                {
                    if (targetCount > count + (int)Math.Ceiling(totalcount))
                    {
                        main.Send_Log(accnt_no + " " + total + name + " " + (int)Math.Ceiling(totalcount) + "개 " + string.Format("{0:#,###}", price.price) + "원 매수 매수시 기준가: " + string.Format("{0:#,###}", markPrice.price) + " 최저가: " + string.Format("{0:#,###}", low_price), false);
                        real_buy_price = price.price;
                        if (buy_kinds[priceKind] == 0)
                        {
                            if (main.allPlayMesu)
                                main.Mesu(name, accnt_no, main.get_scr_no(), code, (int)Math.Ceiling(totalcount), price.price);
                        }
                        else if (buy_kinds[priceKind] == 1)
                        {
                            if (main.allPlayMesu)
                                main.Mesu(name, accnt_no, main.get_scr_no(), code, (int)Math.Ceiling(totalcount));
                        }
                        totalcount -= (float)Math.Ceiling(totalcount);
                        now_buy_count = totalcount;
                    }
                    else
                    {
                        int nowcount = targetCount - count;
                        if (nowcount > 0)
                        {
                            main.Send_Log(accnt_no + " " + total + name + " " + nowcount + "개 " + string.Format("{0:#,###}", price.price) + "원 매수_도달 매수시 기준가: " + string.Format("{0:#,###}", markPrice.price) + " 최저가: " + string.Format("{0:#,###}", low_price), false);
                            real_buy_price = price.price;
                            if (buy_kinds[priceKind] == 0)
                            {
                                if (main.allPlayMesu)
                                    main.Mesu(name, accnt_no, main.get_scr_no(), code, nowcount, price.price);
                            }
                            else if (buy_kinds[priceKind] == 1)
                            {
                                if (main.allPlayMesu)
                                    main.Mesu(name, accnt_no, main.get_scr_no(), code, nowcount);
                            }
                            totalcount -= (float)Math.Ceiling(totalcount);
                            now_buy_count = totalcount;
                        }
                        else
                        {
                            if (!haveEffect)
                            {
                                main.Send_Log_Debug(accnt_no + " " + total + name + " 0개 " + string.Format("{0:#,###}", price.price) + "원 매수_도달 매수시 기준가: " + string.Format("{0:#,###}", markPrice.price) + " 최저가: " + string.Format("{0:#,###}", low_price), false);
                                real_buy_price = price.price;
                                totalcount -= (float)Math.Ceiling(totalcount);
                                now_buy_count = totalcount;
                            }
                            else
                            {
                                state = StockState.buywait;
                            }
                        }
                    }




                    int lw = 0;
                    int nw = 0;
                    for (int i = 0; i < 20; i++)
                    {
                        if (prices[i] == low_price)
                        {
                            lw = i;
                        }
                        else if (prices[i] == price.price)
                        {
                            nw = i;
                        }
                    }
                }
                else
                {
                    if (!haveEffect)
                    {
                        main.Send_Log_Debug(accnt_no + " " + total + name + " 0개 " + string.Format("{0:#,###}", price.price) + "원 매수 처리", false);
                        main.Send_Log_Debug(accnt_no + " " + "매수시 기준가: " + string.Format("{0:#,###}", markPrice.price) + " 최저가: " + string.Format("{0:#,###}", low_price), false);
                        real_buy_price = price.price;
                    }
                    else
                    {
                        state = StockState.buywait;
                    }
                }
            }
            catch (Exception ex)
            {
                main.PlayEffect(25);
                main.Send_Log(accnt_no + " " + "Mesu()" + ex.HResult + " [" + ex.Message + "]");
            }
        }


        public void AllMedo(object sender, EventArgs e)
        {
            Form11 form11 = new Form11(main,0,this);
            form11.Text = accnt_no + " " + name + "전량매도";
            form11.ShowDialog();
        }

        public void halfMedo(object sender, EventArgs e)
        {
            Form11 form11 = new Form11(main, 1, this);
            form11.Text = accnt_no + " " + name + "반매도";
            form11.ShowDialog();
        }

        /// <summary>
        /// 가격데이터 입력
        /// </summary>
        /// <param name="data">현재가격</param>
        public void AddPriceData(int data, string time)
        {
            if(data == price.price)
            {
                return;
            }
            if(is_price_change_sound || main.allPlaySound)
            {
                int dis = (int)(fluctuation - soundmark);

                if (soundmark > 100 || soundmark < -100)
                {
                    soundmark = fluctuation;
                }
                else if (dis >= 1)
                {
                    soundmark += dis;

                    _speechSynthesizer.SpeakAsyncCancelAll();
                    _speechSynthesizer.SpeakAsync(dis + "퍼 오름");
                }
                else if(dis <= -1)
                {
                    soundmark += dis;

                    _speechSynthesizer.SpeakAsyncCancelAll();
                    _speechSynthesizer.SpeakAsync((-dis) + "퍼 내림");
                }
            }
            //main.Send_Log(name + "정보입력 " + data);
            price.price = data;
            price.time.hour = DateTime.Now.Hour;
            price.time.min = DateTime.Now.Minute;
            price.time.sec = DateTime.Now.Second;

            CheckPriceKind();

            int susu = 0;
            if ((int)(realprice * count * 0.00015) >= 10)
                susu += ((int)(realprice * count * 0.00015) % 10) * 10;
            if ((int)(price.price * count * 0.00015) >= 10)
                susu += ((int)(price.price * count * 0.00015) % 10) * 10;
            if (kind == ConditionTransactionType.Kosdaq)
            {
                susu += (int)(price.price * count * 0.0025);
            }
            else
            {
                susu += (int)(price.price * count * 0.001);
                susu += (int)(price.price * count * 0.0015);
            }
            benefit = ((((price.price * count) - susu) / (float)(realprice * count) * 100f) - 100);
            //((Label)pan.Controls["label4"]).Text = "수: " + benefit.ToString("##0.00") + "%";
            //((Label)pan.Controls["label5"]).Text = "등: " + fluctuation.ToString("##0.00") + "%";

            //작동알고리즘
            if (is_playing && main.allPlay)
            {
                if (is_endLimit && limitprice > 0)
                {
                    if (limitprice_on)
                    {
                        if (PriceMoveTick(price.price, 1) < limitprice)
                        {
                            main.Send_Log(accnt_no + " " + name + " 상풀림매도");
                            Medo(time, count);
                            limitprice_on = false;
                            if (!is_rebuy)
                                is_playing = false;
                            else
                            {
                                state = StockState.selling;
                                real_sell_price = price.price;
                            }
                        }
                    }
                    else if (PriceMoveTick(price.price, 1) > limitprice)
                    {
                        limitprice_on = true;
                    }
                }

                if ((all_high_price < price.price) && is_highmedo)
                {
                    all_high_price = price.price;
                    if (is_highmedo_per)
                        ((Label)pan.Controls["label6"]).Text = "고매:" + highMedo_perval.ToString("##0.0") + ":" + string.Format("{0:#,###}", all_high_price - highMedo_per);
                }


                if (is_highmedo)
                {
                    if (is_highmedo_per)
                    {
                        if (all_high_price - highMedo_per > price.price)
                        {
                            main.Send_Log(name + " 고점매도 도달", false);
                            Medo(time, (int)(count - remainCountVal));
                            is_highmedo = false;
                            ((CheckBox)pan.Controls["checkBox7"]).Checked = false;
                            return;
                        }
                    }
                    else
                    {
                        if (highMedo_price > price.price)
                        {
                            main.Send_Log(name + " 고점매도 도달", false);
                            Medo(time, (int)(count - remainCountVal));
                            is_highmedo = false;
                            ((CheckBox)pan.Controls["checkBox7"]).Checked = false;
                            return;
                        }
                    }
                }

                if(is_highmesu && is_sell)
                {
                    try
                    {
                        if(highmesu_markprice > price.price)
                        {
                            highmesu_markprice = price.price;
                            highmesu_highprice = price.price;
                            ((Label)pan.Controls["label15"]).Text = "상승기준: " + string.Format("{0:#,###}", highmesu_markprice);
                            highbuy_count = targetCount / (highbuy_end - highbuy_start) * highbuy_per / 100;
                        }
                        if (highmesu_highprice < price.price)
                        {
                            if(price.price - highmesu_highprice > highmesu_markprice * (main.highbuy_possible / 100f))
                            {
                                is_highmesu = false;
                                now_sell_count = 0;
                                main.Send_Log(accnt_no + " " + name + " 상승매수 급상승");
                                ((CheckBox)pan.Controls["highmesu"]).Checked = is_highmesu;
                                ShowState();
                                return;
                            }
                            string total;
                            if (time != "")
                                total = ("" + time[0] + time[1] + ":" + time[2] + time[3] + ":" + time[4] + time[5] + " ");
                            else
                                total = DateTime.Now.ToString("HH:mm:ss") + " ";
                            now_highbuy_count += (Math.Min(price.price, (highbuy_end + 100) * highmesu_markprice / 100f) - Math.Max(highmesu_markprice * (100 + highbuy_start) / 100f, highmesu_highprice)) / highmesu_markprice * 100f * highbuy_count;
                            highmesu_highprice = price.price;

                            state = StockState.selling;
                            real_sell_price = price.price;

                            if (now_highbuy_count * price.price > main.possible_Amount[main.GetAccntIndex(accnt_no)])
                            {
                                main.Send_Log(accnt_no + " " + name + " 상승매수 예수금 부족: " + main.possible_Amount[main.GetAccntIndex(accnt_no)], false); main.Send_Log("now_highbuy_count: " + now_highbuy_count, false);
                                int realcount = main.possible_Amount[main.GetAccntIndex(accnt_no)] / price.price;
                                now_highbuy_count = realcount;
                            }
                            if (now_highbuy_count > 0)
                            {

                                if (targetCount > count + (int)Math.Ceiling(now_highbuy_count))
                                {
                                    main.Send_Log(accnt_no + " " + total + name + " " + (int)Math.Ceiling(now_highbuy_count) + "개 " + string.Format("{0:#,###}", price.price) + "원 상승매수", false);
                                    highmesu_highprice = price.price;
                                    if (buy_kinds[priceKind] == 0)
                                    {
                                        if (main.allPlayMesu)
                                            main.Mesu(name, accnt_no, main.get_scr_no(), code, (int)Math.Ceiling(now_highbuy_count), price.price);
                                    }
                                    else if (buy_kinds[priceKind] == 1)
                                    {
                                        if (main.allPlayMesu)
                                            main.Mesu(name, accnt_no, main.get_scr_no(), code, (int)Math.Ceiling(now_highbuy_count));
                                    }
                                }
                                else
                                {
                                    int nowcount = targetCount - count;
                                    if (nowcount > 0)
                                    {
                                        main.Send_Log(accnt_no + " " + total + name + " " + nowcount + "개 " + string.Format("{0:#,###}", price.price) + "원 상승매수_도달", false);
                                        is_highmesu = false;
                                        now_sell_count = 0;
                                        main.Send_Log(accnt_no + " " + name + " 상승매수 변경");
                                        ((CheckBox)pan.Controls["highmesu"]).Checked = is_highmesu;
                                        highmesu_markprice = price.price;
                                        highmesu_highprice = price.price;
                                        ((Label)pan.Controls["label15"]).Text = "상승기준: " + string.Format("{0:#,###}", highmesu_markprice);
                                        markPrice.price = price.price;
                                        if (buy_kinds[priceKind] == 0)
                                        {
                                            if (main.allPlayMesu)
                                                main.Mesu(name, accnt_no, main.get_scr_no(), code, nowcount, price.price);
                                        }
                                        else if (buy_kinds[priceKind] == 1)
                                        {
                                            if (main.allPlayMesu)
                                                main.Mesu(name, accnt_no, main.get_scr_no(), code, nowcount);
                                        }
                                    }
                                    else
                                    {
                                        if (!haveEffect)
                                        {
                                            main.Send_Log_Debug(accnt_no + " " + total + name + " 0개 " + string.Format("{0:#,###}", price.price) + "원 상승매수_도달", false);
                                            is_highmesu = false;
                                            now_sell_count = 0;
                                            main.Send_Log(accnt_no + " " + name + " 상승매수 변경");
                                            ((CheckBox)pan.Controls["pricechangesound"]).Checked = is_price_change_sound;
                                            highmesu_markprice = price.price;
                                            highmesu_highprice = price.price;
                                            markPrice.price = price.price;
                                            ((Label)pan.Controls["label15"]).Text = "상승기준: " + string.Format("{0:#,###}", highmesu_markprice);
                                        }
                                    }
                                }
                                now_highbuy_count -= (float)Math.Ceiling(now_highbuy_count);

                            }
                        }
                        if((highbuy_end + 100) * highmesu_markprice < price.price)
                        {
                            is_highmesu = false;
                            now_sell_count = 0;
                            main.Send_Log(accnt_no + " " + name + " 상승매수 목표가 도달");
                            ((CheckBox)pan.Controls["highmesu"]).Checked = is_highmesu;
                        }
                    }
                    catch (Exception ex)
                    {
                        main.PlayEffect(25);
                        main.Send_Log(accnt_no + " " + "HighMesu" + ex.HResult + " [" + ex.Message + "]");
                    }
                }

                if (markPrice.price == 0)
                {
                    markPrice.price = price.price;
                    if (targetisprice)
                        targetCount = (targetPrice / markPrice.price);
                    else
                        targetPrice = targetCount * price.price;
                    SetCountColor();
                    ((Label)pan.Controls["targetcount"]).Text = "목표수량: " + targetCount;
                    ((Label)pan.Controls["targetprice_text"]).Text = "목표가: " + targetPrice / 10000 + "만원";
                    ((Label)pan.Controls["panel3"].Controls["label12"]).Text = "기준가: " + string.Format("{0:#,###}", markPrice.price);
                    markPrice.time.hour = price.time.hour;
                    markPrice.time.min = price.time.min;
                    markPrice.time.sec = price.time.sec;
                }
                else
                {
                    if (targetisprice)
                        targetCount = (targetPrice / markPrice.price);
                    else
                        targetPrice = targetCount * price.price;
                    SetCountColor();
                }

                if (is_cut && realprice != 0 && count > 0)
                {
                    if ((-cut_price > benefit && cut_price < 100) || (cut_price >= price.price && cut_price >= 100))
                    {
                        if (!is_rebuy)
                            is_playing = false;

                        main.Send_Log(accnt_no + " " + name + " 손절가 도달");
                        state = StockState.none;
                        main.Medo(name, accnt_no, main.get_scr_no(), code, count);
                        ReloadPanel();
                        ReloadHoga();
                        SetPanelColor();
                        ShowState();
                        return;
                    }
                }

                if (is_playing && main.allPlay)
                {
                    if (state == StockState.none)
                    {
                        if (price.price < low_price)
                        {
                            low_price = price.price;
                        }

                        if (price.price > high_price)
                        {
                            high_price = price.price;
                        }


                        if (PriceMoveTick(price.price, -sell_conditions[priceKind]) > (100 + sell_start) * markPrice.price / 100f && !is_highmesu && is_sell)
                        {
                            high_price = price.price;
                            sell_price = GetSellPrice();
                            real_sell_price = -1;
                            ((Label)pan.Controls["targetcount"]).Text = "목표수량: " + targetCount;
                            ((Label)pan.Controls["targetprice_text"]).Text = "목표가: " + targetPrice / 10000 + "만원";
                            ((Label)pan.Controls["panel3"].Controls["label12"]).Text = "기준가: " + string.Format("{0:#,###}", markPrice.price);
                            //state = StockState.sellwait;
                            SetNowState(StockState.sellwait);

                            ((Label)pan.Controls["label9"]).Text = "매도될수량: " + (now_sell_count + (Math.Min(PriceMoveTick(high_price, -sell_conditions[priceKind]), (sell_end + 100) * markPrice.price / 100f) - Math.Max(markPrice.price * (100 + sell_start) / 100f, real_sell_price)) / markPrice.price * 100f * sell_count).ToString("##0.0");
                            ((Label)pan.Controls["label9"]).ForeColor = Color.CornflowerBlue;
                        }

                        if (PriceMoveTick(price.price, buy_conditions[priceKind]) < (100 - buy_start) * markPrice.price / 100f && is_buy)
                        {
                            low_price = price.price;
                            buy_price = GetBuyPrice();
                            real_buy_price = int.MaxValue;
                            ((Label)pan.Controls["targetcount"]).Text = "목표수량: " + targetCount;
                            ((Label)pan.Controls["targetprice_text"]).Text = "목표가: " + targetPrice / 10000 + "만원";
                            ((Label)pan.Controls["panel3"].Controls["label12"]).Text = "기준가: " + string.Format("{0:#,###}", markPrice.price);
                            //state = StockState.buywait;
                            SetNowState(StockState.buywait);

                            ((Label)pan.Controls["label9"]).Text = "매수될수량: " + (now_buy_count + (Math.Min(markPrice.price * (100 - buy_start) / 100f, real_buy_price) - Math.Max(PriceMoveTick(low_price, buy_conditions[priceKind]), (100 - buy_end) * markPrice.price / 100f)) / markPrice.price * 100f * buy_count).ToString("##0.0");
                            ((Label)pan.Controls["label9"]).ForeColor = Color.OrangeRed;
                        }
                    }
                    else if (state == StockState.sellwait)
                    {
                        if (!is_sell || is_highmesu)
                        {
                            low_price = price.price;
                            high_price = price.price;
                            markPrice.price = price.price;
                            ((Label)pan.Controls["targetcount"]).Text = "목표수량: " + targetCount;
                            ((Label)pan.Controls["targetprice_text"]).Text = "목표가: " + targetPrice / 10000 + "만원";
                            ((Label)pan.Controls["panel3"].Controls["label12"]).Text = "기준가: " + string.Format("{0:#,###}", markPrice.price);
                            markPrice.time.hour = price.time.hour;
                            markPrice.time.min = price.time.min;
                            markPrice.time.sec = price.time.sec;
                            state = StockState.none;

                            ((Label)pan.Controls["label9"]).Text = "대기중";
                            ((Label)pan.Controls["label9"]).ForeColor = SystemColors.ControlText;
                            ReloadPanel();
                            SetHogacolor();
                            ShowState();
                            return;
                        }

                        if (price.price <= (100 + sell_start) * markPrice.price / 100f)
                        {
                            state = StockState.none;

                            ((Label)pan.Controls["label9"]).Text = "대기중";
                            ((Label)pan.Controls["label9"]).ForeColor = SystemColors.ControlText;
                            ShowState();
                            return;
                        }
                        if (price.price > high_price)
                        {
                            high_price = price.price;
                            sell_price = GetSellPrice();

                            ((Label)pan.Controls["label9"]).Text = "매도될수량: " + (now_sell_count + (Math.Min(PriceMoveTick(high_price, -sell_conditions[priceKind]), (sell_end + 100) * markPrice.price / 100f) - Math.Max(markPrice.price * (100 + sell_start) / 100f, real_sell_price)) / markPrice.price * 100f * sell_count).ToString("##0.0");
                            ((Label)pan.Controls["label9"]).ForeColor = Color.CornflowerBlue;
                        }
                        if (price.price <= sell_price)
                        {
                            if (price.price <= real_sell_price)
                            {
                                state = StockState.selling;
                                ((Label)pan.Controls["label9"]).Text = "대기중";
                                ShowState();
                                return;
                            }
                            else
                                Medo(time);
                        }
                    }
                    else if (state == StockState.buywait)
                    {
                        if (!is_buy)
                        {
                            low_price = price.price;
                            high_price = price.price;
                            markPrice.price = price.price;
                            ((Label)pan.Controls["targetcount"]).Text = "목표수량: " + targetCount;
                            ((Label)pan.Controls["targetprice_text"]).Text = "목표가: " + targetPrice / 10000 + "만원";
                            ((Label)pan.Controls["panel3"].Controls["label12"]).Text = "기준가: " + string.Format("{0:#,###}", markPrice.price);
                            markPrice.time.hour = price.time.hour;
                            markPrice.time.min = price.time.min;
                            markPrice.time.sec = price.time.sec;
                            state = StockState.none;
                            ReloadPanel();
                            SetHogacolor();
                            ShowState();
                            return;
                        }

                        if (price.price >= (100 - buy_start) * markPrice.price / 100f && is_buy)
                        {
                            state = StockState.none;
                            ((Label)pan.Controls["label9"]).Text = "대기중";
                            ((Label)pan.Controls["label9"]).ForeColor = SystemColors.ControlText;
                            ShowState();
                            return;
                        }
                        if (price.price < low_price)
                        {
                            low_price = price.price;
                            buy_price = GetBuyPrice();

                            ((Label)pan.Controls["label9"]).Text = "매수될수량: " + (now_buy_count + (Math.Min(markPrice.price * (100 - buy_start) / 100f, real_buy_price) - Math.Max(PriceMoveTick(low_price, buy_conditions[priceKind]), (100 - buy_end) * markPrice.price / 100f)) / markPrice.price * 100f * buy_count).ToString("##0.0");
                            ((Label)pan.Controls["label9"]).ForeColor = Color.OrangeRed;
                        }
                        if (price.price >= buy_price)
                        {
                            if (price.price >= real_buy_price)
                            {
                                state = StockState.buying;
                                ((Label)pan.Controls["label9"]).Text = "대기중";
                                ShowState();
                                return;
                            }
                            else
                            Mesu(time);
                        }

                    }
                    else if (state == StockState.selling)
                    {
                        if (!is_sell || is_highmesu)
                        {
                            low_price = price.price;
                            high_price = price.price;
                            markPrice.price = price.price;
                            ((Label)pan.Controls["targetcount"]).Text = "목표수량: " + targetCount;
                            ((Label)pan.Controls["targetprice_text"]).Text = "목표가: " + targetPrice / 10000 + "만원";
                            ((Label)pan.Controls["panel3"].Controls["label12"]).Text = "기준가: " + string.Format("{0:#,###}", markPrice.price);
                            markPrice.time.hour = price.time.hour;
                            markPrice.time.min = price.time.min;
                            markPrice.time.sec = price.time.sec;
                            state = StockState.none;
                            ((Label)pan.Controls["label9"]).Text = "대기중";
                            ((Label)pan.Controls["label9"]).ForeColor = SystemColors.ControlText;
                            SetHogacolor();
                            ShowState();
                            return;
                        }
                        else if (PriceMoveTick(price.price, buy_conditions[priceKind]) < real_sell_price - (real_sell_price * buy_start / 100f))
                        {
                            now_buy_count = 0;
                            real_buy_price = int.MaxValue;
                            low_price = price.price;
                            buy_price = GetBuyPrice();
                            //state = StockState.buywait;
                            SetNowState(StockState.buywait);

                            markPrice.price = real_sell_price;
                            ((Label)pan.Controls["targetcount"]).Text = "목표수량: " + targetCount;
                            ((Label)pan.Controls["targetprice_text"]).Text = "목표가: " + targetPrice / 10000 + "만원";
                            ((Label)pan.Controls["panel3"].Controls["label12"]).Text = "기준가: " + string.Format("{0:#,###}", markPrice.price);
                            markPrice.time.hour = price.time.hour;
                            markPrice.time.min = price.time.min;
                            markPrice.time.sec = price.time.sec;
                            ((Label)pan.Controls["label9"]).Text = "매수될수량: " + (now_buy_count + (Math.Min(markPrice.price * (100 - buy_start) / 100f, real_buy_price) - Math.Max(PriceMoveTick(low_price, buy_conditions[priceKind]), (100 - buy_end) * markPrice.price / 100f)) / markPrice.price * 100f * buy_count).ToString("##0.0");
                            ((Label)pan.Controls["label9"]).ForeColor = Color.OrangeRed;
                            SetCounts();
                        }
                        else if (PriceMoveTick(price.price, -sell_conditions[priceKind]) > real_sell_price)
                        {
                            high_price = price.price;
                            sell_price = GetSellPrice();
                            ((Label)pan.Controls["label9"]).Text = "매도될수량: " + (now_sell_count + (Math.Min(PriceMoveTick(high_price, -sell_conditions[priceKind]), (sell_end + 100) * markPrice.price / 100f) - Math.Max(markPrice.price * (100 + sell_start) / 100f, real_sell_price)) / markPrice.price * 100f * sell_count).ToString("##0.0");
                            ((Label)pan.Controls["label9"]).ForeColor = Color.CornflowerBlue;
                            //state = StockState.sellwait;
                            SetNowState(StockState.sellwait);
                        }
                    }
                    else if (state == StockState.buying)
                    {
                        if (!is_buy)
                        {
                            low_price = price.price;
                            high_price = price.price;
                            markPrice.price = price.price;
                            ((Label)pan.Controls["targetcount"]).Text = "목표수량: " + targetCount;
                            ((Label)pan.Controls["targetprice_text"]).Text = "목표가: " + targetPrice / 10000 + "만원";
                            ((Label)pan.Controls["panel3"].Controls["label12"]).Text = "기준가: " + string.Format("{0:#,###}", markPrice.price);
                            markPrice.time.hour = price.time.hour;
                            markPrice.time.min = price.time.min;
                            markPrice.time.sec = price.time.sec;
                            state = StockState.none;
                            ((Label)pan.Controls["label9"]).Text = "대기중";
                            ((Label)pan.Controls["label9"]).ForeColor = SystemColors.ControlText;
                            SetHogacolor();
                            ShowState();
                            return;
                        }
                        else if (PriceMoveTick(price.price, -sell_conditions[priceKind]) > real_buy_price + (real_buy_price * sell_start / 100f))
                        {
                            now_sell_count = 0;
                            real_sell_price = -1;
                            high_price = price.price;
                            sell_price = GetSellPrice();
                            sell_count = targetCount / (sell_end - sell_start) * sell_per / 100;
                            //state = StockState.sellwait;
                            SetNowState(StockState.sellwait);
                            markPrice.price = real_buy_price;
                            ((Label)pan.Controls["targetcount"]).Text = "목표수량: " + targetCount;
                            ((Label)pan.Controls["targetprice_text"]).Text = "목표가: " + targetPrice / 10000 + "만원";
                            ((Label)pan.Controls["panel3"].Controls["label12"]).Text = "기준가: " + string.Format("{0:#,###}", markPrice.price);
                            markPrice.time.hour = price.time.hour;
                            markPrice.time.min = price.time.min;
                            markPrice.time.sec = price.time.sec;

                            ((Label)pan.Controls["label9"]).Text = "매도될수량: " + (now_sell_count + (Math.Min(PriceMoveTick(high_price, -sell_conditions[priceKind]), (sell_end + 100) * markPrice.price / 100f) - Math.Max(markPrice.price * (100 + sell_start) / 100f, real_sell_price)) / markPrice.price * 100f * sell_count).ToString("##0.0");
                            ((Label)pan.Controls["label9"]).ForeColor = Color.CornflowerBlue;
                        }
                        else if (PriceMoveTick(price.price, buy_conditions[priceKind]) < real_buy_price)
                        {
                            low_price = price.price;
                            buy_price = GetBuyPrice();
                            ((Label)pan.Controls["label9"]).Text = "매수될수량: " + (now_buy_count + (Math.Min(markPrice.price * (100 - buy_start) / 100f, real_buy_price) - Math.Max(PriceMoveTick(low_price, buy_conditions[priceKind]), (100 - buy_end) * markPrice.price / 100f)) / markPrice.price * 100f * buy_count).ToString("##0.0");
                            ((Label)pan.Controls["label9"]).ForeColor = Color.OrangeRed;
                            //state = StockState.buywait;
                            SetNowState(StockState.buywait);
                        }
                    }

                    SetHogacolor();
                }
            }
            else
            {
                SetCounts();
                if (is_highmedo)
                {
                    if (is_highmedo_per)
                    {
                        highMedo_per = (int)(price.price * highMedo_perval / 100f);
                        ((Label)pan.Controls["label6"]).Text = "고매:" + highMedo_perval.ToString("##0.0") + ":" + string.Format("{0:#,###}", all_high_price - highMedo_per);
                    }
                }
                for (int i = 0; i < 20; i++)
                {
                    if (prices[i] == price.price)
                    {
                        gridView.Rows[i].Cells[0].Style.BackColor = Color.FromArgb(230, 255, 230);
                        gridView.Rows[i].Cells[0].Style.SelectionBackColor = Color.FromArgb(230, 255, 230);
                        gridView.Rows[i].Cells[1].Style.BackColor = Color.FromArgb(230, 255, 230);
                        gridView.Rows[i].Cells[1].Style.SelectionBackColor = Color.FromArgb(230, 255, 230);
                    }
                    else
                    {
                        gridView.Rows[i].Cells[0].Style.BackColor = Color.FromArgb(255, 255, 255);
                        gridView.Rows[i].Cells[0].Style.SelectionBackColor = Color.FromArgb(255, 255, 255);
                        gridView.Rows[i].Cells[1].Style.BackColor = Color.FromArgb(255, 255, 255);
                        gridView.Rows[i].Cells[1].Style.SelectionBackColor = Color.FromArgb(255, 255, 255);
                    }
                }
            }
            ShowState();
        }

        public void ShowState()
        {
            string msg = "";
            msg += "현재가: " + string.Format("{0:#,###}", price.price);
            if (state == StockState.buywait)
            {
                msg += " 하락중 최저가:" + string.Format("{0:#,###}", low_price) + " 매수가: " + string.Format("{0:#,###}", buy_price);
            }
            else if (state == StockState.sellwait)
            {
                msg += " 상승중 최고가:" + string.Format("{0:#,###}", high_price) + " 매도가: " + string.Format("{0:#,###}", sell_price);
            }
            else if (state == StockState.buying)
            {
                msg += " 매수후 최저가:" + string.Format("{0:#,###}", low_price) + " 전매수가: " + string.Format("{0:#,###}", real_buy_price);
            }
            else if (state == StockState.selling)
            {
                msg += " 매도후 최고가:" + string.Format("{0:#,###}", high_price) + " 전매도가: " + string.Format("{0:#,###}", real_sell_price);
            }
            else if (state == StockState.none)
            {
                if (is_playing && main.allPlay)
                    msg += " 대기중";
            }
            SetMsg(msg);
        }

        public void SetNowState(StockState _state)
        {
            if (_state == state)
                return;

            if(_state == StockState.buywait)
            {
                now_buy_count = 0;
                state = _state;
            }
            else if(_state == StockState.sellwait)
            {
                now_sell_count = 0;
                now_highbuy_count = 0;
                state = _state;
            }
        }

        public int GetSellPrice()
        {
            return PriceMoveTick(price.price, -sell_conditions[priceKind]);
        }

        public int GetBuyPrice()
        {
            return PriceMoveTick(price.price, buy_conditions[priceKind]);
        }

        public void SetHogacolor()
        {
            
            //표 색상 입력

            for (int i = 0; i < 20; i++)
            {
                if (markPrice.price < price.price)
                {
                    if (prices[i] >= markPrice.price && prices[i] < price.price)
                    {
                        gridView.Rows[i].Cells[0].Style.BackColor = Color.FromArgb(255, 230, 230);
                        gridView.Rows[i].Cells[0].Style.SelectionBackColor = Color.FromArgb(255, 230, 230);
                        gridView.Rows[i].Cells[1].Style.BackColor = Color.FromArgb(255, 230, 230);
                        gridView.Rows[i].Cells[1].Style.SelectionBackColor = Color.FromArgb(255, 230, 230);
                    }
                    else
                    {
                        gridView.Rows[i].Cells[0].Style.BackColor = Color.FromArgb(255, 255, 255);
                        gridView.Rows[i].Cells[0].Style.SelectionBackColor = Color.FromArgb(255, 255, 255);
                        gridView.Rows[i].Cells[1].Style.BackColor = Color.FromArgb(255, 255, 255);
                        gridView.Rows[i].Cells[1].Style.SelectionBackColor = Color.FromArgb(255, 255, 255);
                    }
                }
                else if (markPrice.price > price.price)
                {
                    if (prices[i] <= markPrice.price && prices[i] > price.price)
                    {
                        gridView.Rows[i].Cells[0].Style.BackColor = Color.FromArgb(230, 230, 255);
                        gridView.Rows[i].Cells[0].Style.SelectionBackColor = Color.FromArgb(230, 230, 255);
                        gridView.Rows[i].Cells[1].Style.BackColor = Color.FromArgb(230, 230, 255);
                        gridView.Rows[i].Cells[1].Style.SelectionBackColor = Color.FromArgb(230, 230, 255);
                    }
                    else
                    {
                        gridView.Rows[i].Cells[0].Style.BackColor = Color.FromArgb(255, 255, 255);
                        gridView.Rows[i].Cells[0].Style.SelectionBackColor = Color.FromArgb(255, 255, 255);
                        gridView.Rows[i].Cells[1].Style.BackColor = Color.FromArgb(255, 255, 255);
                        gridView.Rows[i].Cells[1].Style.SelectionBackColor = Color.FromArgb(255, 255, 255);
                    }
                }
                else
                {
                    gridView.Rows[i].Cells[0].Style.BackColor = Color.FromArgb(255, 255, 255);
                    gridView.Rows[i].Cells[0].Style.SelectionBackColor = Color.FromArgb(255, 255, 255);
                    gridView.Rows[i].Cells[1].Style.BackColor = Color.FromArgb(255, 255, 255);
                    gridView.Rows[i].Cells[1].Style.SelectionBackColor = Color.FromArgb(255, 255, 255);
                }
                if (prices[i] == price.price)
                {
                    gridView.Rows[i].Cells[0].Style.BackColor = Color.FromArgb(230, 255, 230);
                    gridView.Rows[i].Cells[0].Style.SelectionBackColor = Color.FromArgb(230, 255, 230);
                    gridView.Rows[i].Cells[1].Style.BackColor = Color.FromArgb(230, 255, 230);
                    gridView.Rows[i].Cells[1].Style.SelectionBackColor = Color.FromArgb(230, 255, 230);
                }
            }
        }

        public void ReloadHoga()
        {
            for (int i = 0; i < 20; i++)
            {
                if ((prices[i] > (100 + highbuy_start) * highmesu_markprice / 100f && prices[i] < (100 + highbuy_end) * highmesu_markprice / 100f) && is_highmesu)
                {
                    gridView.Rows[i].Cells[2].Value = (int)Math.Ceiling(((prices[i] - (100 + highbuy_start) * highmesu_markprice / 100f) / highmesu_markprice * 100f * highbuy_count));
                    gridView.Rows[i].Cells[2].Style.BackColor = Color.LightCoral;
                    gridView.Rows[i].Cells[2].Style.SelectionBackColor = SystemColors.Window;
                }
                else if (prices[i] < (100 - buy_start) * markPrice.price / 100f && prices[i] > (100 - buy_end) * markPrice.price / 100f)
                {
                    gridView.Rows[i].Cells[2].Value = (int)Math.Ceiling((((100 - buy_start) * markPrice.price / 100f - prices[i]) / markPrice.price * 100f * buy_count));
                    gridView.Rows[i].Cells[2].Style.BackColor = SystemColors.Window;
                    gridView.Rows[i].Cells[2].Style.SelectionBackColor = SystemColors.Window;
                }
                else if ((prices[i] > (100 + sell_start) * markPrice.price / 100f && prices[i] < (100 + sell_end) * markPrice.price / 100f) && !is_highmesu)
                {
                    gridView.Rows[i].Cells[2].Value = (int)Math.Ceiling(((prices[i] - (100 + sell_start) * markPrice.price / 100f) / markPrice.price * 100f * sell_count));
                    gridView.Rows[i].Cells[2].Style.BackColor = SystemColors.Window;
                    gridView.Rows[i].Cells[2].Style.SelectionBackColor = SystemColors.Window;
                }
                else
                {
                    gridView.Rows[i].Cells[2].Value = "";
                    gridView.Rows[i].Cells[2].Style.BackColor = Color.FromArgb(200, 200, 200);
                    gridView.Rows[i].Cells[2].Style.SelectionBackColor = Color.FromArgb(200, 200, 200);
                }
                if (markPrice.price != 0)
                {
                    if (prices[i] != 0)
                        gridView.Rows[i].Cells[1].Value = ((prices[i] - markPrice.price) / (float)markPrice.price * 100f).ToString("##0.00");
                    else
                        gridView.Rows[i].Cells[1].Value = "";
                }
            }
        }

        public void SetCountColor()
        {
            if (count >= targetCount)
            {
                ((Label)pan.Controls["targetprice_text"]).ForeColor = Color.OrangeRed;
                ((Label)pan.Controls["targetcount"]).ForeColor = Color.OrangeRed;
                ((Label)pan.Controls["label8"]).ForeColor = Color.OrangeRed;
            }
            else
            {
                ((Label)pan.Controls["targetprice_text"]).ForeColor = SystemColors.ControlText;
                ((Label)pan.Controls["targetcount"]).ForeColor = SystemColors.ControlText;
                ((Label)pan.Controls["label8"]).ForeColor = SystemColors.ControlText;
            }
        }

        public void SetPurchasePrice(int pri)
        {
            purchase_price = pri;
            main.Send_Log(name + "손절 변경 " + pri + "%");
        }

        public ConditionTransactionType isKosdaq()
        {
            kind = GetConditionTransactionTypeByCode(code, name);
            return kind;
        }

        public int PriceMoveTick(int price, int tick)
        {

            price = Form1.PriceMoveTick(code, name, price, tick);

            CheckPriceKind();

            return price;
        }

        public void SetMsg(string msg)
        {

            if (this.pan.Controls["label3"].InvokeRequired)
            {
                pan.Controls["label3"].BeginInvoke(new Action(() =>
                {

                    ((Label)pan.Controls["label3"]).Text = msg;
                }
                ));
            }
            else
            {
                ((Label)pan.Controls["label3"]).Text = msg;
            }
        }

        public void SetPanelColor()
        {
            if (is_playing && main.allPlay)
            {
                if (is_sell && is_buy)
                {
                    ((Panel)pan).BackColor = Color.FromArgb(255, 255, 255);
                    ((Panel)pan.Controls["panel3"]).BackColor = Color.FromArgb(255, 255, 255);
                }
                else if (is_sell)
                {
                    ((Panel)pan).BackColor = Color.FromArgb(240, 210, 210);
                    ((Panel)pan.Controls["panel3"]).BackColor = Color.FromArgb(240, 210, 210);
                }
                else if (is_buy)
                {
                    ((Panel)pan).BackColor = Color.FromArgb(210, 210, 240);
                    ((Panel)pan.Controls["panel3"]).BackColor = Color.FromArgb(210, 210, 240);
                }
                else
                {
                    ((Panel)pan).BackColor = Color.FromArgb(180, 180, 180);
                    ((Panel)pan.Controls["panel3"]).BackColor = Color.FromArgb(180, 180, 180);
                }
            }
            else
            {
                if (main.allPlay)
                {
                    ((Panel)pan).BackColor = Color.FromArgb(180, 180, 180);
                    ((Panel)pan.Controls["panel3"]).BackColor = Color.FromArgb(180, 180, 180);
                }
                else
                {
                    ((Panel)pan).BackColor = Color.FromArgb(255, 255, 255);
                    ((Panel)pan.Controls["panel3"]).BackColor = Color.FromArgb(255, 255, 255);
                }
            }
        }

        public int GetNowPrice()
        {
            return price.price;
        }

        public void ReloadPanel()
        {
            ((CheckBox)pan.Controls["playing_cb"]).Checked = is_playing;
            ((Label)pan.Controls["label13"]).Visible = is_fixed;
            ((Label)pan.Controls["label15"]).Visible = (is_sell && is_highmesu);
            ((Label)pan.Controls["label15"]).Text = "상승기준: " + string.Format("{0:#,###}", highmesu_markprice);
            if (is_fixed)
            {
                ((Button)pan.Controls["button6"]).BackColor = Color.RoyalBlue;
            }
            else
            {
                ((Button)pan.Controls["button6"]).BackColor = Color.Firebrick;
            }
            ((CheckBox)pan.Controls["checkbox2"]).Checked = is_sell;
            ((CheckBox)pan.Controls["checkbox4"]).Checked = is_buy;
            if (targetPrice != 0)
                ((Label)pan.Controls["targetprice_text"]).Text = "목표가: " + string.Format("{0:#,###}", targetPrice / 10000) + "만원";
            else
                ((Label)pan.Controls["targetprice_text"]).Text = "목표가: 0원";

            if (markPrice.price != 0)
            {
                ((Label)pan.Controls["targetcount"]).Text = "목표수량: " + targetCount;
                ((Label)pan.Controls["targetprice_text"]).Text = "목표가: " + targetPrice / 10000 + "만원";
                if (state == StockState.sellwait)
                {
                    ((Label)pan.Controls["label9"]).Text = "매도될수량: " + (now_sell_count + (Math.Min(PriceMoveTick(high_price, -sell_conditions[priceKind]), (sell_end + 100) * markPrice.price / 100f) - Math.Max(markPrice.price * (100 + sell_start) / 100f, real_sell_price)) / markPrice.price * 100f * sell_count).ToString("##0.0");
                    ((Label)pan.Controls["label9"]).ForeColor = Color.CornflowerBlue;
                }
                else if (state == StockState.buywait)
                {
                    ((Label)pan.Controls["label9"]).Text = "매수될수량: " + (now_buy_count + (Math.Min(markPrice.price * (100 - buy_start) / 100f, real_buy_price) - Math.Max(PriceMoveTick(low_price, buy_conditions[priceKind]), (100 - buy_end) * markPrice.price / 100f)) / markPrice.price * 100f * buy_count).ToString("##0.0");
                    ((Label)pan.Controls["label9"]).ForeColor = Color.OrangeRed;
                }
                else
                {
                    ((Label)pan.Controls["label9"]).Text = "대기중";
                    ((Label)pan.Controls["label9"]).ForeColor = SystemColors.ControlText;
                }
            }
            else
            {
                ((Label)pan.Controls["targetcount"]).Text = "목표수량: " + targetCount;
                ((Label)pan.Controls["label9"]).Text = "대기중";
                ((Label)pan.Controls["label9"]).ForeColor = SystemColors.ControlText;
            }
            ((Label)pan.Controls["label8"]).Text = "보유수량: " + count;
            SetCountColor();
            ((Label)pan.Controls["label7"]).Text = "남김: " + remainCountVal + "개";
            ((CheckBox)pan.Controls["checkbox6"]).Checked = end_clear;
            if (is_highmedo_per)
                ((Label)pan.Controls["label6"]).Text = "고매:" + highMedo_perval.ToString("##0.0") + ":" + string.Format("{0:#,###}", all_high_price - highMedo_per);
            else
                ((Label)pan.Controls["label6"]).Text = "고매:" + string.Format("{0:#,###}", highMedo_price) + "원";
            ((CheckBox)pan.Controls["checkBox7"]).Checked = is_highmedo;

            if(cut_price < 100)
                ((Label)pan.Controls["label11"]).Text = "손절: " + cut_price.ToString("##0.0") + "%" + Environment.NewLine + string.Format("{0:#,###}", realprice * ((100 - cut_price) / 100f));
            else
                ((Label)pan.Controls["label11"]).Text = "손절: " + string.Format("{0:#,###}", cut_price) + "원";

            ((CheckBox)pan.Controls["checkBox8"]).Checked = is_cut;
            ((CheckBox)pan.Controls["checkBox9"]).Checked = is_rebuy;
            ((Label)pan.Controls["panel3"].Controls["label12"]).Text = "기준가: " + string.Format("{0:#,###}", markPrice.price);
            ((CheckBox)pan.Controls["checkbox1"]).Checked = haveEffect;
            ((CheckBox)pan.Controls["checkbox10"]).Checked = is_endLimit;
            ((CheckBox)pan.Controls["pricechangesound"]).Checked = is_price_change_sound;
            //if(count > 0)
            //{
            //    ((Button)main.Controls["flowLayoutPanel1"].Controls["panel" + (index + 1)].Controls["button6"]).Visible = false;
            //    ((Button)main.Controls["flowLayoutPanel1"].Controls["panel" + (index + 1)].Controls["button6"]).Enabled = false;
            //}
            //else
            //{
            //    ((Button)main.Controls["flowLayoutPanel1"].Controls["panel" + (index + 1)].Controls["button6"]).Visible = true;
            //    ((Button)main.Controls["flowLayoutPanel1"].Controls["panel" + (index + 1)].Controls["button6"]).Enabled = true;
            //}

            int susu = 0;
            if ((int)(realprice * count * 0.00015) >= 10)
                susu += (int)(realprice * count * 0.00015);
            if ((int)(price.price * count * 0.00015) >= 10)
                susu += (int)(price.price * count * 0.00015);
            if (kind == ConditionTransactionType.Kosdaq)
            {
                susu += (int)(price.price * count * 0.0025);
            }
            else
            {
                susu += (int)(price.price * count * 0.001);
                susu += (int)(price.price * count * 0.0015);
            }
            benefit = ((((price.price * count) - susu) / ((float)realprice * count) * 100f) - 100);
            //((Label)pan.Controls["label4"]).Text = "수: " + benefit.ToString("##0.00") + "%";
            SetPanelColor();
        }



        [HandleProcessCorruptedStateExceptions]
        [SecurityCritical]
        public DateTime delay(int MS)
        {
            DateTime ThisMoment = DateTime.Now;
            TimeSpan duration = new TimeSpan(0, 0, 0, 0, MS);
            DateTime AfterWards = ThisMoment.Add(duration);

            while (AfterWards >= ThisMoment)
            {
                try
                {
                    unsafe
                    {
                        System.Windows.Forms.Application.DoEvents();
                    }
                }
                catch (Exception ex)
                {
                    main.PlayEffect(25);
                    main.Send_Log("delay() : [" + ex.Message + "]\n");
                }
                ThisMoment = DateTime.Now;
            }
            return DateTime.Now;
        }
    }

    public class Time
    {
        public int hour = 0;
        public int min = 0;
        public int sec = 0;

        public Time()
        {
        }
        public Time(int h, int m, int s)
        {
            hour = h;
            min = m;
            sec = s;
            CheckTime();
        }

        public void CheckTime()
        {
            if (sec > 60)
            {
                sec -= 60;
                min += 1;
            }
            if (min > 60)
            {
                min -= 60;
                hour += 1;
            }
        }

        static public int Compare(Time a, Time b)
        {
            a.CheckTime();
            b.CheckTime();
            if (a.hour - b.hour > 0)
            {
                return 1;
            }
            else if (a.hour - b.hour < 0)
            {
                return -1;
            }
            else
            {
                if (a.min - b.min > 0)
                {
                    return 1;
                }
                else if (a.min - b.min < 0)
                {
                    return -1;
                }
                else
                {
                    if (a.sec - b.sec > 0)
                    {
                        return 1;
                    }
                    else if (a.sec - b.sec < 0)
                    {
                        return -1;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
        }
        static public int Compare(Time a, int h, int m, int s)
        {
            a.CheckTime();

            if (s > 60)
            {
                s -= 60;
                m += 1;
            }
            if (m > 60)
            {
                m -= 60;
                h += 1;
            }

            if (a.hour - h > 0)
            {
                return 1;
            }
            else if (a.hour - h < 0)
            {
                return -1;
            }
            else
            {
                if (a.min - m > 0)
                {
                    return 1;
                }
                else if (a.min - m < 0)
                {
                    return -1;
                }
                else
                {
                    if (a.sec - s > 0)
                    {
                        return 1;
                    }
                    else if (a.sec - s < 0)
                    {
                        return -1;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
        }

        static public int Distance(Time a, Time b)
        {
            int atotal = 0;
            int btotal = 0;

            atotal += a.hour * 360;
            atotal += a.min * 60;
            atotal += a.sec;
            btotal += b.hour * 360;
            btotal += b.min * 60;
            btotal += b.sec;

            return Math.Abs(atotal - btotal);
        }
    }

    public class JumunData
    {
        public int jumuncount;
        public int chegyulcount = 0;
        public string jumunnumber;

        public JumunData(string junum, int jucount)
        {
            jumuncount = jucount;
            jumunnumber = junum;
        }
    }


    public class XmlData
    {

        public class Data
        {
            public Data(string _name, string _value)
            {
                name = _name;
                value = _value;
            }

            public string name;
            public string value;
        }

        List<Data> datas = new List<Data>();
        public Form1 main;

        public bool bSaveXML()
        {
            string sFilePath = Path.GetDirectoryName(Application.ExecutablePath);

            try
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.NewLineOnAttributes = true;
                XmlWriter xmlWriter = XmlWriter.Create(@sFilePath + "\\Settings_" + main.g_user_id + ".xml"); // 저장할 xml 파일명
                xmlWriter.WriteStartDocument();
                xmlWriter.WriteStartElement("root");
                for (int i = 0; i < datas.Count; i++)
                {
                    xmlWriter.WriteElementString(datas[i].name, datas[i].value); // 앞이 node, 뒤는 value
                }
                xmlWriter.WriteEndDocument();

                xmlWriter.Flush();
                xmlWriter.Close();

                main.SaveEnd = true;
                return true;
            }
            catch (Exception ex)
            {
                if (main != null)
                {
                    main.PlayEffect(25);
                    main.Send_Log("bSaveXML() [" + ex.Message + "]");
                }
                return false;
            }
        }

        public bool bLoadXML()
        {
            string sFilePath = Path.GetDirectoryName(Application.ExecutablePath);

            try
            {
                if (File.Exists(@sFilePath + "\\Settings_" + main.g_user_id + ".xml"))   //  xml 파일 존재 유무 검사
                {
                    XmlTextReader xmlReadData = new XmlTextReader(@sFilePath + "\\Settings_" + main.g_user_id + ".xml");    //  xml 파일 열기

                    while (xmlReadData.Read())
                    {
                        if (xmlReadData.NodeType == XmlNodeType.Element)
                        {
                            if (Find(xmlReadData.Name.Trim()) == null)
                                datas.Add(new Data(xmlReadData.Name.Trim(), xmlReadData.ReadString().Trim()));
                            //switch (xmlReadData.Name.ToUpper().Trim())
                            //{
                            //    case "PRODUCT": sProductNm = xmlReadData.ReadString().ToString().Trim(); break;
                            //    case "WORKER": sWorkerNm = xmlReadData.ReadString().ToString().Trim(); break;
                            //}
                        }
                    }
                    xmlReadData.Close();
                }
                else // xml 파일이 존재 하지 않을 때
                {
                    // 디폴트값으로 xml 파일을 기록해야하므로 저장 함수로 보낸다.
                    bSaveXML();
                }

                return true;
            }
            catch (Exception ex)
            {
                main.PlayEffect(25);
                main.Send_Log("bLoadXML() [" + ex.Message + "]");
                return false;
            }
        }

        public Data Find(string _name)
        {
            for (int i = 0; i < datas.Count; i++)
            {
                if (datas[i].name == _name)
                {
                    return datas[i];
                }
            }
            return null;
        }

        public int Remove(string _name)
        {
            for (int i = 0; i < datas.Count; i++)
            {
                if (datas[i].name == _name)
                {
                    datas.Remove(datas[i]);
                    return 0;
                }
            }
            return -1;
        }

        public void SetData(string _name, string _value)
        {
            Data temp = Find(_name);
            if (temp == null)
            {
                temp = new Data(_name, _value);
                datas.Add(temp);
            }
            else
            {
                temp.value = _value;
            }
        }
    }


    public static class ControlExtensions
    {
        public static T Clone<T>(this T controlToClone)
            where T : Control
        {
            PropertyInfo[] controlProperties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            T instance = Activator.CreateInstance<T>();

            foreach (PropertyInfo propInfo in controlProperties)
            {
                if (propInfo.CanWrite)
                {
                    if (propInfo.Name != "WindowTarget")
                        propInfo.SetValue(instance, propInfo.GetValue(controlToClone, null), null);
                }
            }

            return instance;
        }

        public static string ControlsToString<T>(this T controlToClone, string res, int stack)
            where T : Control
        {
            res += "\n";
            for (int i = 0; i < stack; i++)
            {
                res += "    ";
            }
            res += controlToClone.Name;
            if (controlToClone.Controls.Count == 0)
            {
                return res;
            }
            else
            {
                for (int i = 0; i < controlToClone.Controls.Count; i++)
                {
                    res = controlToClone.Controls[i].ControlsToString(res, stack + 1);
                }
            }
            return res;
        }
    }

    static class CommUtil
    {
        /// <summary>
        /// 파일의 (제품)버전을 구한다.
        /// </summary>
        /// <returns>파일 (제품)버전</returns>
        static public string AppVersion
        {
            get
            {
                return System.Windows.Forms.Application.ProductVersion;
            }
        }

        /// <summary>
        /// 컴파일한 날짜를 구한다.
        ///   단, AssemblyInfo.cs 파일에서 AssemblyVersion는 다음 형식으로 되어있어야만한다.
        ///   [assembly: AssemblyVersion("1.0.*")]
        /// </summary>
        /// <returns>컴파일한 날짜</returns>
        static public DateTime BuildDate
        {
            get
            {
                System.Version assemblyVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                // assemblyVersion.Build = days after 2000-01-01
                // assemblyVersion.Revision*2 = seconds after 0-hour  (NEVER daylight saving time) 
                DateTime buildDate = new DateTime(2000, 1, 1).AddDays(assemblyVersion.Build).AddSeconds(assemblyVersion.Revision * 2);

                return buildDate;
            }
        }
    }
}
