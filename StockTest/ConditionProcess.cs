using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockTest
{
    public class ConditionProcess
    {
        Form1 main;
        public string accnt_no;
        public List<Condition> conditions = new List<Condition>();
        public Condition[] onPlayConditions = new Condition[5];

        //자동매수 기본값들
        public float sell_start = 1;           //매도하기 위한 최소 수익(시작가)
        public float sell_end = 10;           //매도하기 위한 최대 수익(종)
        public float buy_start = 1;            //매수하기 위한 최소 가격(시작가)
        public float buy_end = 10;            //매수하기 위한 최대 가격(종)

        public float sell_per = 100;
        public float buy_per = 100;
        public bool is_highbuy = false;
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
        public bool is_playing = false;
        public bool is_selltime = false;
        public bool is_buytime = false;
        public bool is_highmedo = false;
        public bool is_highmedo_per = false;
        public int highMedo_price;
        public float highMedo_perval;
        public int targetPrice;
        public int remainCountVal;
        public float remainCount;
        public bool is_rebuy;                   //재매수
        public bool haveEffect;                 //유효매매
        public bool end_clear;                  //종료매매
        public bool is_endLimit;                //상풀림매매
        public bool is_price_change_sound = false;//변동알림

        public ConditionProcess(Form1 _main)
        {
            main = _main;
            main.GetAPI().OnReceiveTrCondition += OnReceiveTrCondition;
            main.GetAPI().OnReceiveRealCondition += OnReceiveRealCondition;
            main.GetAPI().OnReceiveRealData += OnReceiveRealData;
            main.GetAPI().OnReceiveTrData += OnReceiveTrData;
        }

        public int AddCondition(string name, int index)
        {
            for (int i = 0; i < conditions.Count; i++)
            {
                if (conditions[i].name == name)
                    return -1;
            }
            Condition newcon = new Condition(main, name, index);
            conditions.Add(newcon);

            if(main.xmlData.Find("Condition" + name + ".isPlay") != null)
            {
                try
                {
                    //newcon.isPlay = bool.Parse(main.xmlData.Find("Condition" + name + ".isPlay").value);
                    newcon.startTime.hour = int.Parse(main.xmlData.Find("Condition" + name + ".startTime.hour").value);
                    newcon.startTime.min = int.Parse(main.xmlData.Find("Condition" + name + ".startTime.min").value);
                    newcon.startTime.sec = int.Parse(main.xmlData.Find("Condition" + name + ".startTime.sec").value);
                    newcon.endTime.hour = int.Parse(main.xmlData.Find("Condition" + name + ".endTime.hour").value);
                    newcon.endTime.min = int.Parse(main.xmlData.Find("Condition" + name + ".endTime.min").value);
                    newcon.endTime.sec = int.Parse(main.xmlData.Find("Condition" + name + ".endTime.sec").value);
                    newcon.buyMoney = int.Parse(main.xmlData.Find("Condition" + name + ".buyMoney").value);
                    newcon.maxBuyCount = int.Parse(main.xmlData.Find("Condition" + name + ".maxBuyCount").value);
                    for (int i = 0; i < 5; i++)
                    {
                        if(main.xmlData.Find("ConditiononPlayConditions" + i).value == name)
                        {
                            onPlayConditions[i] = newcon;
                            
                            newcon.SendCondition();
                            break;
                        }
                    }
                    newcon.buyType = int.Parse(main.xmlData.Find("Condition" + name + ".buyType").value);
                    newcon.minValue = int.Parse(main.xmlData.Find("Condition" + name + ".minValue").value);
                    newcon.addConditionIndex = int.Parse(main.xmlData.Find("Condition" + name + ".addConditionIndex").value);
                }
                catch
                {

                }
            }


            return 0;
        }

        public Condition FindCondition(int index)
        {
            return conditions.Find(a => a.index == index);
        }

        public Condition FindCondition(string name)
        {
            return conditions.Find(a => a.name == name);
        }

        public int FindConditionIndex(int index)
        {
            return conditions.FindIndex(a => a.index == index);
        }

        public void SetDefault(string _code, string _accnt_no)
        {
            StockChecker stockChecker = main.stockCheckers.Find(a => a.isThis(_code, _accnt_no));
            if (stockChecker != null)
            { 
                stockChecker.sell_start = sell_start;               //매도하기 위한 최소 수익(시작가)
                stockChecker.sell_end = sell_end;                   //매도하기 위한 최대 수익(종)
                stockChecker.buy_start = buy_start;                         //매수하기 위한 최소 가격(시작가)
                stockChecker.buy_end = buy_end;                          //매수하기 위한 최대 가격(종)
                stockChecker.sell_per = sell_per;
                stockChecker.buy_per = buy_per;
                stockChecker.highbuy_start = highbuy_start;
                stockChecker.highbuy_end = highbuy_end;
                stockChecker.highbuy_per = highbuy_per;
                for(int i = 0; i < sell_conditions.Length; i++)
                {
                    stockChecker.sell_conditions[i] = sell_conditions[i];
                    stockChecker.buy_conditions[i] = buy_conditions[i];
                    stockChecker.sell_kinds[i] = sell_kinds[i];
                    stockChecker.buy_kinds[i] = buy_kinds[i];
                }
                stockChecker.is_sell = is_sell;                        //매도를 할 것인가
                stockChecker.is_buy = is_buy;                         //매수를 할 것인가
                stockChecker.is_cut = is_cut;
                stockChecker.cut_price = cut_price;                         //손절가( % )
                stockChecker.is_playing = is_playing;
                stockChecker.is_selltime = is_selltime;
                stockChecker.is_buytime = is_buytime;
                stockChecker.targetPrice = targetPrice;
                stockChecker.remainCountVal = remainCountVal;
                stockChecker.highMedo_perval = highMedo_perval;
                stockChecker.is_rebuy = is_rebuy;                   //재매수
                stockChecker.is_highmedo = is_highmedo;
                stockChecker.is_highmedo_per = is_highmedo_per;
                stockChecker.highMedo_price = highMedo_price;
                stockChecker.haveEffect = is_rebuy;                 //유효매매
                stockChecker.end_clear = end_clear;                 //종료매매
                stockChecker.is_endLimit = is_endLimit;             //상풀림매매
                stockChecker.is_price_change_sound = is_price_change_sound;         //변동알림
                stockChecker.is_highmesu = is_highbuy;         //변동알림
            }
        }

        void OnReceiveTrCondition(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveTrConditionEvent e)
        {
            string codelist = e.strCodeList.Trim();
            if (codelist.Length > 0)
                codelist = codelist.Remove(codelist.Length - 1);
            string[] codes = codelist.Trim().Split(';');
            Condition findCondition = FindCondition(e.nIndex);
            if (findCondition == null)
                return;
            if (codes.Length > 100)
            {
                string[] codes100 = new string[100];
                Array.Copy(codes, codes100, 100);
                codes = codes100;
            }
            for (int j = 0; j < codes.Length; j++)
            {
                for (int i = 0; i < onPlayConditions.Length; i++)
                {
                    if (onPlayConditions[i] == null)
                        continue;
                    StockInfo findstock = onPlayConditions[i].stocks.Find(a => a.code == main.RealCode(codes[j]));
                    if (findstock != null)
                    {
                        FindCondition(e.nIndex).AddStock(findstock);
                    }
                }
            }
            findCondition.AddStock(codes);
            findCondition.SendStock();
        }

        private void OnReceiveRealData(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveRealDataEvent e)
        {
            if (e.sRealType.Trim() == "주식체결")
            {
                string realcode = main.RealCode(e.sRealKey.Trim());
                for (int i = 0; i < onPlayConditions.Length; i++)
                {
                    if (onPlayConditions[i] == null)
                        continue;
                }
            }
        }

        private void OnReceiveTrData(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveTrDataEvent e)
        {
            string[] str = e.sRQName.Split(';');
            if(str.Length == 2)
            {
                if(str[0] == "조건검색목록")
                {
                    int index = int.Parse(str[1]);
                    int cnt = main.GetAPI().GetRepeatCnt(e.sTrCode, e.sRQName);
                    Condition condition = FindCondition(index);
                    for (int i = 0; i < cnt; i++)
                    {
                        condition.AddData(main.RealCode(main.GetAPI().GetCommData(e.sTrCode, e.sRQName, i, "종목코드").Trim()), Math.Abs(int.Parse(main.GetAPI().GetCommData(e.sTrCode, e.sRQName, i, "현재가").Trim())), float.Parse(main.GetAPI().GetCommData(e.sTrCode, e.sRQName, i, "등락율").Trim()), int.Parse(main.GetAPI().GetCommData(e.sTrCode, e.sRQName, i, "전일대비").Trim()), float.Parse(main.GetAPI().GetCommData(e.sTrCode, e.sRQName, i, "전일거래량대비").Trim()));
                    }
                }
            }
        }

        public void OnReceiveRealCondition(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveRealConditionEvent e)
        {
            if (e.strType == "I")//종목 편입
            {
                string stockName = main.GetAPI().GetMasterCodeName(e.sTrCode);
                for (int i = 0; i < onPlayConditions.Length; i++)
                {
                    if (onPlayConditions[i] == null)
                        continue;
                    StockInfo findstock = onPlayConditions[i].stocks.Find(a => a.code == main.RealCode(e.sTrCode));
                    if (findstock != null)
                    {
                        FindCondition(int.Parse(e.strConditionIndex)).AddStock(findstock);
                        return;
                    }
                }
                StockInfo stock = FindCondition(int.Parse(e.strConditionIndex)).AddStock(main.RealCode(e.sTrCode));
                if(stock != null)
                {
                    stock.scrnum = main.get_scr_no();
                    main.GetAPI().SetRealReg(stock.scrnum, stock.code, "20;41;10;951;", "1");
                }
            }
            else if (e.strType == "D")//종목 이탈
            {
                string stockName = main.GetAPI().GetMasterCodeName(e.sTrCode);
                StockInfo stock = FindCondition(int.Parse(e.strConditionIndex)).RemoveStock(main.RealCode(e.sTrCode));
                for(int i = 0; i < onPlayConditions.Length; i++)
                {
                    if (onPlayConditions[i] == null)
                        continue;
                    if (onPlayConditions[i].stocks.Find(a => a.code == stock.code) != null)
                        return;
                }
                main.GetAPI().DisconnectRealData(stock.scrnum);
            }
        }

    }
}
