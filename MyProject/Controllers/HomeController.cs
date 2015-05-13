using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MyProject.Controllers
{
    public class HomeController : Controller  
    {
        public ActionResult Fleck()
        {
            return View();    
        }

        public ActionResult Index()
        {
            //List<Person> personList = new List<Person>
            //{
            //    new Person(1, "crp"),
            //    new Person(2, "tm")296ca3
            //};

            //DataTable dt = new DataTable();
            //DataColumn dc = null;
            //dc = dt.Columns.Add("ID", Type.GetType("System.Int32")); 

            //DataRow row = dt.NewRow();
            //row["ID"] = 1;
            //dt.Rows.Add(row);

            //var dtList = dt.AsEnumerable();

            //var result = (from p in personList
            //             join k in dtList on p.ID equals k.Field<int>("ID")
            //             where k.Field<int>("ID") == p.ID                      
            //         select new Person  
            //         {  
            //            ID = p.ID,
            //            Name = p.Name
            //         }).ToList();  


            DataTable dt = new DataTable();
            DataColumn dc = null;
            dc = dt.Columns.Add("ID", Type.GetType("System.String"));
            dc = dt.Columns.Add("Name", Type.GetType("System.String"));
            dc = dt.Columns.Add("Type", Type.GetType("System.String"));


            dt.Rows.Add("0001", "三里屯", "红色");
            dt.Rows.Add("0002", "禄米仓", "黑色");
            dt.Rows.Add("0001", "三里屯", "蓝色");
            dt.Rows.Add("0002", "禄米仓", "黑色");
            dt.Rows.Add("0003", "华润", "红色");

            var count = dt.AsEnumerable().Count(d => d.Field<string>("ID") == "");

            StringBuilder build = new StringBuilder();

            dt.AsEnumerable().ToList().ForEach((o) =>
            {
                build.AppendFormat("'{0}',", o.Field<string>("ID"));
                build.AppendFormat("'{0}',", o.Field<string>("Name"));
                build.AppendFormat("'{0}',", o.Field<string>("Type"));
            });

            var l = dt.AsEnumerable().GroupBy(g => g.Field<string>("ID") + g.Field<string>("Name"));

            foreach (var item in l)
            {
                string key = item.Key;

                foreach (var a in item)
                {


                }
            }




            var dt1 = ConvertToTable(dt);

            return View();
        }




        DataTable ConvertToTable(DataTable source)
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("ID");
            dt.Columns.Add("Name");

            var columns = (from x in source.Rows.Cast<DataRow>() select x[2].ToString()).Distinct();

            foreach (var item in columns)
                dt.Columns.Add(item).DefaultValue = 0;

            var data = from x in source.Rows.Cast<DataRow>()
                       group x by x[1] into g
                       select new { Key = g.Key.ToString(), Items = g };

            data.ToList().ForEach(x =>
            {

                string[] array = new string[dt.Columns.Count];

                array[1] = x.Key;

                for (int i = 2; i < dt.Columns.Count; i++)
                {
                    if (array[0] == null)
                        array[0] = x.Items.ToList<DataRow>()[0]["ID"].ToString();

                    array[i] = (from y in x.Items
                                where y[2].ToString() == dt.Columns[i].ToString()
                                select y[2].ToString()
                                ).Count().ToString();
                }
                dt.Rows.Add(array);
            });
            return dt;
        }

        public ActionResult Test()
        {
            return View();
        }



        public ActionResult Egg()
        {
            return View();
        }

        [HttpPost]
        public decimal SmashEgg()
        {
            var prizeMoney = Portion(Level.One);

            return prizeMoney;
        }


        public decimal Portion(Level level)
        {
            decimal prizeMoney = 0; //中奖金额
            decimal reward = AwardAmount(level); //奖励金额

            //分别计算得到奖励金额的 20%、50%、100%
            Proportion proportion = new Proportion(reward);

            /*方法一*/

            Random random = new Random();
            var randomNum = random.Next(100);

            if (randomNum >= 1 && randomNum <= 90)
            {
                prizeMoney = proportion.TwentyPercent;
            }
            else if (randomNum >= 91 && randomNum <= 99)
            {
                prizeMoney = proportion.FiftyPercent;
            }
            else
            {
                prizeMoney = proportion.HundredPercent;
            }


            //方法二

            //中奖率 90%，9%，1%
            decimal[] probabilityArr = { 0.9m, 0.09m, 0.01m };
            int[] sequenceArr = new int[100];

            decimal probability = 0;
            int i = 0, j = 0, index = 0;

            //根据中奖概率产生随机序列
            for (; j < probabilityArr.Length; j++)
            {
                probability = probabilityArr[j] * 100;
                for (; i < probability; i++)
                {
                    sequenceArr[index] = j + 1;
                    index++;
                }
                i = 0;
            }

            //随机排序1
            int x = 0, y = 0, tmp = 0;
            for (y = sequenceArr.Length - 1; y > 0; y--)
            {
                x = random.Next(0, y + 1);
                tmp = sequenceArr[y];
                sequenceArr[y] = sequenceArr[x];
                sequenceArr[x] = tmp;
            }

            //随机排序2
            int[] keys = new int[sequenceArr.Length];
            Random random1 = new Random();
            //产生排序Key
            for (int k = 0; k < sequenceArr.Length; k++)
            {
                keys[k] = random1.Next();
            }
            //排序
            Array.Sort(keys, sequenceArr);

            return prizeMoney;
        }

        /// <summary>
        /// 奖励金额
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public decimal AwardAmount(Level level)
        {
            decimal reward = 0; //奖励金额

            switch (level)
            {
                case Level.One:
                    reward = 5;  //1-10万元，5块奖励
                    break;
                case Level.Two:
                    reward = 10;  //10-50万元，10块奖励
                    break;
                case Level.Three:
                    reward = 50; //50-100万元，50块奖励
                    break;
                case Level.Four:
                    reward = 100;//100万元以上，100块奖励
                    break;
            }

            return reward;
        }


        /// <summary>
        /// 抽奖级别
        /// </summary>
        public enum Level
        {
            One = 1,
            Two = 2,
            Three = 3,
            Four = 4
        }

        /// <summary>
        /// 奖励金额实体
        /// </summary>
        public class Proportion
        {

            /// <summary>
            /// 实例化奖励金额，计算奖励金额的 20%、50%、100%
            /// </summary>
            /// <param name="reward">奖励金额</param>
            public Proportion(decimal reward)
            {
                this.TwentyPercent = reward * 0.2m; //奖励20%
                this.FiftyPercent = reward * 0.5m;  //奖励50%
                this.HundredPercent = reward * 1m;  //奖励100%
            }

            /// <summary>
            /// 百分之二十奖励金额
            /// </summary>
            public decimal TwentyPercent { get; set; }

            /// <summary>
            /// 百分之五十奖励金额
            /// </summary>
            public decimal FiftyPercent { get; set; }

            /// <summary>
            /// 百分之百奖励金额
            /// </summary>

            public decimal HundredPercent { get; set; }
        }



        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }



    }


    public class Person
    {
        public Person() { }

        public Person(int id, string name)
        {
            this.ID = id;
            this.Name = name;
        }
        public int ID { get; set; }

        public string Name { get; set; }
    }
}