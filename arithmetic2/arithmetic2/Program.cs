﻿using System;
//哈希表
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace arithmetic2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("请输入要生成的题数");
            int quantity = int.Parse(Console.ReadLine());
            Console.WriteLine("请选择难度（输入1为四年级，整数运算，输入2为五年级，带有限小数运算，输入3为六年级,带分数运算)\n输入4混合运算，生成时间较久！");
            int num = int.Parse(Console.ReadLine());
            Console.WriteLine("请输入运算范围");
            int scope = int.Parse(Console.ReadLine());
            //利用哈希表进行数据的存储与查重
            Hashtable fourOperations = new Hashtable();
            Console.WriteLine("正在生成题目,请稍等");
            switch (num)
            {
                case 1:
                    #region 四年级题目
                    for (int i = 0; i < quantity; i++)
                    {
                        string topic = (topicfour(scope));
                        string answer = (consequence(topic));
                        if (fourOperations.Contains(topic))
                        {
                            i--;
                            break;
                        }
                        if (Convert.ToDouble(answer) > 0)
                        {
                            fourOperations.Add(topic, answer);
                        }
                        else
                        {
                            i--;
                        }
                    }
                    break;
                #endregion
                case 2:
                    #region 五年级题目
                    for (int i = 0; i < quantity; i++)
                    {
                        string topic = (topicfive(scope));
                        string answer = (consequence(topic));
                        if (fourOperations.Contains(topic))
                        {
                            i--;
                            break;
                        }
                        if (Convert.ToDouble(answer) > 0)
                        {
                            fourOperations.Add(topic, answer);
                        }
                        else
                        {
                            i--;
                        }
                    }
                    break;
                #endregion
                case 3:
                    #region 六年级题目
                    for (int i = 0; i < quantity; i++)
                    {
                        string topic = (topicssix(scope));
                        string answer = (solution(topic));
                        answer = reductionOfFraction(answer);
                        if (fourOperations.Contains(topic))
                        {
                            i--;
                            break;
                        }
                        if (Convert.ToDouble(consequence(answer)) > 0)
                        {
                            fourOperations.Add(topic, answer);
                        }
                        else
                        {
                            i--;
                        }
                    }
                    break;
                #endregion
                case 4:
                    #region 混合运算题目
                    for (int i = 0; i < quantity; i++)
                    {
                        Console.WriteLine(mixture(scope));
                    }
                    #endregion
                    break;
            }
            #region 写入TXT
            //题目的TXT
            FileStream fs = new FileStream("D:\\四则运算.txt", FileMode.Create);
            //答案的TXT
            FileStream da = new FileStream("D:\\四则运算的答案.txt", FileMode.Create);
            int plus = 1;
            foreach (string a in fourOperations.Keys)
            {
                //获得字节数组
                byte[] data = System.Text.Encoding.Default.GetBytes("第" + plus + "题." + a + " =" + "\r\n");
                //开始写入
                fs.Write(data, 0, data.Length);
                plus++;
            }
            //清空缓冲区、关闭流
            fs.Flush();
            fs.Close();
            plus = 1;
            foreach (string b in fourOperations.Values)
            {
                //获得字节数组
                byte[] data = System.Text.Encoding.Default.GetBytes("第" + plus + "题:" + b + "\r\n");
                //开始写入
                da.Write(data, 0, data.Length);
                plus++;
            }
            //清空缓冲区、关闭流
            da.Flush();
            da.Close();
            #endregion
            Console.WriteLine("生成完毕");
            Console.ReadKey();
        }
        //小数转分数
        public static string FractionalConversion(string decimals)
        {
            //因为小数只生成一位小数，所以分母可以确定为2-10，公约数为2/3/5
            //    int indexes = decimals.IndexOf(".");
            //    string conv = decimals.Substring(indexes+1, decimals.Length);
            //    return reductionOfFraction(conv+"/10");
            //将数*10再进行约分
            return reductionOfFraction(decimals.Replace(".", "") + "/10");
        }
        //分数约分
        public static string reductionOfFraction(string grade)
        {
            int indexes = grade.IndexOf("/");
            int element = int.Parse(grade.Substring(0, indexes));
            int denominator = int.Parse(grade.Substring(indexes + 1, grade.Length - indexes - 1));
            int min = Math.Min(element, denominator);
            for (int i = 2; i < min; i++)
            {
                if (element % i == 0 && denominator % i == 0)
                {
                    element = element / i;
                    denominator = denominator / i;
                    min = Math.Min(element, denominator);
                    i = 2;
                }
            }
            return element.ToString() + "/" + denominator;
        }
        //无分数结果验算
        public static string consequence(string equation)
        {
            //小数与整数运算           
            string formula = equation.Replace("÷", "/");
            formula = formula.Replace("×", "*");
            formula = formula.Replace("＋", "+");
            formula = formula.Replace("－", "-");
            DataTable dt = new DataTable();
            string really_data = dt.Compute(formula, "false").ToString();
            return really_data;
        }
        //分数的验算
        public static string fractionalArithmetic(string symbol, string one, string two)
        {
            one = one.Replace(" ", "");
            two = two.Replace(" ", "");
            //取出第一个数的分子和分母
            int indexesOne = one.IndexOf("/");
            int elementOne = int.Parse(one.Substring(0, indexesOne));
            //int denominatorOne = int.Parse(one.Substring(indexesOne + 1, one.Length-1));
            int denominatorOne = int.Parse(one.Substring(indexesOne + 1, one.Length - indexesOne - 1));
            //记录第一个数的分母
            //int lalal = denominatorOne;
            //取出第二个数的分子和分母
            int indexesTwo = two.IndexOf("/");
            int elementTwo = int.Parse(two.Substring(0, indexesTwo));
            int denominatorTwo = int.Parse(two.Substring(indexesTwo + 1, two.Length - indexesTwo - 1));
            //假设第一个数第二个数之间的公约数都是对方,计算第一个数分子的倍数
            elementOne = elementOne * denominatorTwo;
            //第二个数分子的倍数
            elementTwo = elementTwo * denominatorOne;
            //取出分母
            denominatorTwo = denominatorTwo * denominatorOne;
            //如果是除法
            if (symbol == "÷")
            {
                return reductionOfFraction((elementOne.ToString() + "/" + elementTwo.ToString()));
            }
            if (symbol == "×")
            {
                return reductionOfFraction((elementOne * elementTwo).ToString() + "/" + denominatorTwo);
            }
            //第一个分子与第二个分子的换算
            string strin = consequence(elementOne + symbol + elementTwo);
            return reductionOfFraction(strin + "/" + denominatorTwo);

        }
        //六年级题目的解法
        public static string solution(string topic)
        {
            string[] array = new string[5];
            int i = 0;
            foreach (var a in topic)
            {
                if (a.ToString() == "＋" || a.ToString() == "－" || a.ToString() == "×" || a.ToString() == "÷")
                {
                    i++;
                    array[i] += a;
                    i++;
                }
                else
                {
                    array[i] += a;
                }
            }
            for (int l = 0; l < array.Length; l++)
            {
                if (array[l] == "×" || array[l] == "÷")
                {
                    if (l == 3)
                    {
                        return fractionalArithmetic(array[1], array[0], fractionalArithmetic(array[l], array[l - 1], array[l + 1]));
                    }
                }
            }
            return fractionalArithmetic(array[3], fractionalArithmetic(array[1], array[0], array[2]), array[4]);
        }
        #region 生成题目的逻辑
        //生成四年级题目
        public static string topicfour(int scope)
        {
            string ret;
            ret = integer(scope) + " " + operators() + " " + integer(scope) + " " + operators() + " " + integer(scope);
            return ret;
        }
        //生成五年级题目
        public static string topicfive(int scope)
        {
            string ret;
            ret = decimals(scope) + " " + operators() + " " + decimals(scope) + " " + operators() + " " + decimals(scope);
            return ret;
        }
        //生成六年级题目
        public static string topicssix(int scope)
        {
            string ret;
            ret = grade(scope) + " " + operators() + " " + grade(scope) + " " + operators() + " " + grade(scope);
            return ret;
        }
        //生成混合题目
        public static string mixture(int scope)
        {
            string ret = "";
            Random random = new Random();
            for (int i = 0; i < 3; i++)
            {
                switch (random.Next(1, 4))
                {
                    case 1:
                        ret += integer(scope);
                        break;
                    case 2:
                        ret += decimals(scope);
                        break;
                    case 3:
                        ret += grade(scope);
                        break;
                }
                if (i != 2)
                {
                    ret += operators();
                }
            }
            return ret;
        }
        #endregion
        #region 生成数与符号
        //随机整数
        public static string integer(int scope)
        {
            Random random = new Random();
            return random.Next(0, scope).ToString();
        }
        //随机小数
        public static string decimals(int scope)
        {
            Random random = new Random();
            return random.Next(0, scope).ToString() + "." + random.Next(1, 10);
        }
        //随机分数
        public static string grade(int scope)
        {
            Random random = new Random();
            return random.Next(1, scope).ToString() + "/" + random.Next(1, 10);
        }
        //随机运算符      
        public static string operators()
        {
            Random random = new Random();
            switch (random.Next(1, 5))
            {
                case 1:
                    return "＋";
                case 2:
                    return "－";
                case 3:
                    return "×";
                case 4:
                    return "÷";
            }
            return "";
        }
        //括号
        public static void bracket()
        {

        }
        #endregion
        //分数运算
        public static string operation(string symbol, string num1, string num2)
        {

            return "";
        }
    }
}
