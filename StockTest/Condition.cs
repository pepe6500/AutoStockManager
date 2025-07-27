using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StockTest
{
    public class Condition
    {
        Form1 main;

        public bool isPlay = false;
        public Time startTime = new Time();
        public Time endTime = new Time();
        public int index;
        public string name;
        public int buyMoney;
        public int maxBuyCount;
        public int minValue = 0;
        public int addConditionIndex = 0;
        public string scrnum;
        public int buyType = 0;


        public List<StockInfo> stocks = new List<StockInfo>();
        public List<StockInfo> buyStocks = new List<StockInfo>();
        

        public Condition(Form1 _main, string _name, int _index)
        {
            main = _main;
            name = _name;
            index = _index;
        }

        public void AddStock(StockInfo stockInfo)
        {
            if (stocks.Find(a => a.code == stockInfo.code) != null)
                return;
            stocks.Add(stockInfo);
        }

        public StockInfo AddStock(string _code)
        {
            if (stocks.Find(a => a.code == _code) != null)
                return null;
            StockInfo stock = new StockInfo
            {
                name = main.GetAPI().GetMasterCodeName(_code),
                code = _code
            };
            stocks.Add(stock);
            return stock;
        }

        public StockInfo RemoveStock(string _code)
        {
            StockInfo stock = stocks.Find(a => a.code == _code);
            if (stock == null)
                return null;
            stocks.Remove(stock);
            return stock;
        }

        public void AddStock(string[] _codes)
        {
            for (int i = 0; i < _codes.Length; i++)
            {
                string code = main.RealCode(_codes[i]);
                if (code == "")
                    return;
                if (stocks.Find(a => a.code == code) != null)
                    continue;
                stocks.Add(new StockInfo
                {
                    name = main.GetAPI().GetMasterCodeName(code),
                    code = code
                });
            }
        }

        public bool AddData(string code, int price, float fluctuation, int netChange, float netChangeTrading)
        {
            StockInfo stock = stocks.Find(a => a.code == code);
            stock.price = price;
            stock.fluctuation = fluctuation;
            stock.netChange = netChange;
            stock.netChangeTrading = netChangeTrading;
            stocks.Sort(delegate (StockInfo A, StockInfo B)
            {
                if (A.netChangeTrading < B.netChangeTrading) return 1;
                else if (A.netChangeTrading > B.netChangeTrading) return -1;
                else return 0;
            });
            int index = stocks.FindIndex(a => a == stock);
            switch(addConditionIndex)
            {
                case 1:
                    if (netChangeTrading < minValue)
                        return false;
                    break;
            }

            for (int i = 0; i < main.stockCheckers.Count; i++)
            {
                if (main.stockCheckers[i].code == stock.code)
                    return false;
            }

            if (isPlay)
            {
                if (maxBuyCount > buyStocks.Count)
                {
                    if (Form1.CompareTime(DateTime.Now, startTime.hour, startTime.min, startTime.sec) == 1 && Form1.CompareTime(DateTime.Now, endTime.hour, endTime.min, endTime.sec) == -1)
                    {
                        if (buyStocks.Find(a => a == stock) == null)
                        {
                            int buycount = buyMoney / price;
                            if (buycount > 0)
                            {
                                switch (buyType)
                                {
                                    case 0:
                                        main.Mesu(stock.name, main.GetMainAccountNum(), main.get_scr_no(), stock.code, buycount, price);
                                        main.Send_Log(name + " " + stock.name + " " + String.Format("{0:#,###}", price) + "원 " + buycount + "주 현재가 매수 전일비 : " + netChangeTrading);
                                        break;
                                    case 1:
                                        main.Mesu(stock.name, main.GetMainAccountNum(), main.get_scr_no(), stock.code, buycount, Form1.PriceMoveTick(stock.code, stock.name, price, 1));
                                        main.Send_Log(name + " " + stock.name + " " + String.Format("{0:#,###}", Form1.PriceMoveTick(stock.code, stock.name, price, 1)) + "원 " + buycount + "주 1호가위 매수 전일비 : " + netChangeTrading);
                                        break;
                                    case 2:
                                        main.Mesu(stock.name, main.GetMainAccountNum(), main.get_scr_no(), stock.code, buycount, Form1.PriceMoveTick(stock.code, stock.name, price, 2));
                                        main.Send_Log(name + " " + stock.name + " " + String.Format("{0:#,###}", Form1.PriceMoveTick(stock.code, stock.name, price, 2)) + "원 " + buycount + "주 2호가위 매수 전일비 : " + netChangeTrading);
                                        break;
                                    case 3:
                                        main.Mesu(stock.name, main.GetMainAccountNum(), main.get_scr_no(), stock.code, buycount);
                                        main.Send_Log(name + " " + stock.name + " " + String.Format("{0:#,###}", price) + "원 " + buycount + "주 시장가 매수 전일비 : " + netChangeTrading);
                                        break;
                                }

                                main.insert_tb_accnt_info(stock.code, stock.name, main.GetMainAccountNum(), stock.price, 0, stock.price);
                                buyStocks.Add(stock);
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }


        public void SendCondition()
        {
            scrnum = main.get_scr_no();
            int result = main.GetAPI().SendCondition(scrnum, name, index, 1);
            if (result > 0)
            {
                main.Send_Log_Debug(name + " 조건검색 성공");
            }
            else
            {
                main.Send_Log_Debug(name + " 조건검색 실패 : " + result);
            }
        }

        public void StopCondition()
        {
            main.Send_Log_Debug(name + " 조건검색 삭제");
            main.GetAPI().SendConditionStop(scrnum, name, index);
        }

        public void SendStock()
        {
            int codeCount = stocks.Count;
            if (codeCount <= 0)
                return;
            int i;
            for (i = 0; i < stocks.Count; i++)
            {
                if (stocks[i].scrnum == null)
                {
                    stocks[i].scrnum = main.get_scr_no();
                    main.GetAPI().SetRealReg(stocks[i].scrnum, stocks[i].code, "20;41;10;951;", "1");
                }
            }
            string temp = "";
            for (i = 0; i < stocks.Count; i++)
            {
                temp += stocks[i].code + ";";
                if ((i + 1) % 100 == 0)
                {
                    temp = temp.Remove(temp.Length - 1);
                    int comres = main.GetAPI().CommKwRqData(temp, 0, 100, 0, "조건검색목록;" + index, main.get_scr_no());
                    if (comres < 0)
                        main.Send_Log("조건검색목록;" + index + " 종목 검색실패 : " + comres);
                    temp = "";
                }
                if (i + 1 >= 200)
                    break;
            }
            if ((i + 1) % 100 != 0)
            {
                int comres = main.GetAPI().CommKwRqData(temp, 0, codeCount % 100, 0, "조건검색목록;" + index, main.get_scr_no());
                if (comres < 0)
                    main.Send_Log("조건검색목록;" + index + " 종목 검색실패 : " + comres);
            }
        }

        public void CheckTime()
        {
        }
    }

    public class StockInfo
    {
        public string name;
        public string code;
        public int price;
        public float fluctuation;
        public int netChange;
        public float netChangeTrading;

        public string scrnum = null;
    }
}
