using System.IO;
using System.Text;
using System;
using UnityEngine;

namespace lgu3d
{
    public static class RegularTools
    {
        //检测是否是手机号
        public static bool IsTelephone(string str)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str, @"^[1]+[3,4,5,6,7,8]+\d{9}");
        }

        public static bool CheckIDCard(string Id)
        {
            if (Id.Length == 18) 
            {
                long n = 0;
                if (long.TryParse(Id.Remove(17), out n) == false || n < Math.Pow(10, 16) || long.TryParse(Id.Replace('x', '0').Replace('X', '0'), out n) == false)
                {
                    return false;//数字验证
                }
                string[] arrVarifyCode = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');
                string[] Wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');
                char[] Ai = Id.Remove(17).ToCharArray();
                int sum = 0;
                for (int i = 0; i < 17; i++)
                {
                    sum += int.Parse(Wi[i]) * int.Parse(Ai[i].ToString());
                }
                int y = -1;
                Math.DivRem(sum, 11, out y);
                if (arrVarifyCode[y] != Id.Substring(17, 1).ToLower())
                {
                    return false;//校验码验证
                }
                return true;//符合GB11643-1999标准
            }
            else if(Id.Length==15)
            {
                long n = 0;
                if (long.TryParse(Id, out n) == false || n < Math.Pow(10, 14))
                {
                    return false;//数字验证
                }
                string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
                if (address.IndexOf(Id.Remove(2)) == -1)
                {
                    return false;//省份验证
                }
                string birth = Id.Substring(6, 6).Insert(4, "-").Insert(2, "-");
                DateTime time = new DateTime();
                if (DateTime.TryParse(birth, out time) == false)
                {
                    return false;//生日验证
                }
                return true;//符合15位身份证标准
            }
            else
            {
                return false;
            }
        }

        public static string GetShortNick(string nick,bool filter=false)
        {
            if(!string.IsNullOrEmpty(nick))
            {
                char[] nickArray = nick.ToCharArray();
                int length = 0;
                string snick = "";
                for (int i = 0; i < nickArray.Length; i++)
                {
                    try
                    {
                        ushort bb = nickArray[i];
                        if ((bb > 126 && bb < 19968)
                            || (bb > 40869)
                            || bb < 33) continue;

                        if (nickArray[i] > 122)
                        {
                            length += 2;
                        }
                        else
                        {
                            length += 1;
                        }
                        if (length > 8 && !filter)
                        {
                            break;
                        }
                        snick += nickArray[i];
                    }
                    catch (Exception)
                    {

                    }
                }    
                if(length > 8 && !filter)
                {
                    snick = snick + "..";
                }
                return snick;          
            }
            return nick;
        }

    }
}