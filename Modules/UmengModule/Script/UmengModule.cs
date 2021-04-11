using UnityEngine;
using Umeng;
using System.Collections.Generic;
using static Umeng.GA;

namespace lgu3d
{
    /// <summary>
    /// 友盟SDK模块
    /// </summary>
    public class UmengModule : ManagerContorBase<UmengModule>
    {
        public override void Load(params object[] _Agr)
        {
        #if !UNITY_EDITOR
            GA.StartWithAppKeyAndChannelId("60724542de41b946ab460d3b", "umeng");
            GA.SetLogEnabled(Debug.isDebugBuild);
        #endif
            base.Load(_Agr);
        }

        #region Event
        /// <summary>
        /// 基本事件
        /// </summary>
        /// <param name="eventId">友盟后台设定的事件Id</param>
        public void Event(string eventId){
        #if !UNITY_EDITOR
            GA.Event(eventId);
        #endif
        }
        /// <summary>
        /// 基本事件
        /// </summary>
        /// <param name="eventId">友盟后台设定的事件Id</param>
        /// <param name="label">分类标签</param>
        public static void Event(string eventId, string label){
        #if !UNITY_EDITOR
            GA.Event(eventId,label);
        #endif
        }
        /// <summary>
        /// 属性事件
        /// </summary>
        /// <param name="eventId">友盟后台设定的事件Id</param>
        /// <param name="attributes"> 属性中的Key-Vaule Pair不能超过10个</param>
        public static void Event(string eventId, Dictionary<string, string> attributes){
            #if !UNITY_EDITOR
            GA.Event(eventId,attributes);
            #endif
        }
        /// <summary>
		/// 自定义事件 — 计算事件数
		/// </summary>
		public static void Event(string eventId, Dictionary<string, string> attributes, int value){
            #if !UNITY_EDITOR
            GA.Event(eventId,attributes,value);
            #endif
        }
        #endregion
   
        
        #region Pag
        /// <summary>
	    /// 页面时长统计,记录某个页面被打开多长时间
	    /// </summary>
	    /// <param name="pageName">被统计view名称</param>
        public static void PageBegin(string pageName){
            #if !UNITY_EDITOR
            GA.PageBegin(pageName);
            #endif
        }
        /// <summary>
        /// 页面时长统计,记录某个页面被打开多长时间
        /// 与PageBegin配对使用
        /// </summary>
        /// <param name="pageName">被统计view名称</param>
        /// 
        public static void PageEnd(string pageName){
            #if !UNITY_EDITOR
            GA.PageEnd(pageName);
            #endif
        }
        #endregion
        
        
        #region User
        /// <summary>
		/// 设置玩家等级
		/// </summary>
		/// <param name="level">玩家等级</param>
		public static void SetUserLevel(int level){
            #if !UNITY_EDITOR
            GA.SetUserLevel(level);
            #endif
        }
        /// <summary>
        /// 玩家进入关卡
        /// </summary>
        /// <param name="level">关卡</param>
        public static void StartLevel(string level){
            #if !UNITY_EDITOR
            GA.StartLevel(level);
            #endif
        }
        /// <summary>
        /// 玩家通过关卡
        /// </summary>
        /// <param name="level">如果level设置为null 则为当前关卡</param>
        public static void FinishLevel(string level){
            #if !UNITY_EDITOR
            GA.FinishLevel(level);
            #endif
        }
        /// <summary>
        /// 玩家未通过关卡
        /// </summary>
        /// <param name="level">如果level设置为null 则为当前关卡</param>
        public static void FailLevel(string level){
            #if !UNITY_EDITOR
            GA.FailLevel(level);
            #endif
        }
        #endregion
   
        
        #region Pay
        /// <summary>
        /// 游戏中真实消费（充值）的时候调用此方法
        /// </summary>
        /// <param name="cash">本次消费金额</param>
		/// <param name="source">来源</param>
        /// <param name="coin">本次消费等值的虚拟币</param>
        public static void Pay(double cash, PaySource source, double coin){
            #if !UNITY_EDITOR
            GA.Pay(cash,source,coin);
            #endif
        }
        /// <summary>
		/// 游戏中真实消费（充值）的时候调用此方法
		/// </summary>
		/// <param name="cash">本次消费金额</param>
		/// <param name="source">来源:AppStore = 1,支付宝 = 2,网银 = 3,财付通 = 4,移动 = 5,联通 = 6,电信 = 7,Paypal = 8,
		/// 9~100对应渠道请到友盟后台设置本次消费的途径，网银，支付宝 等</param>
		/// <param name="coin">本次消费等值的虚拟币</param>
		public static void Pay(double cash, int source, double coin){
            #if !UNITY_EDITOR
            GA.Pay(cash,source,coin);
            #endif
        }
        /// <summary>
        /// 玩家支付货币购买道具
        /// </summary>
        /// <param name="cash">真实货币数量</param>
        /// <param name="source">支付渠道</param>
        /// <param name="item">道具名称</param>
        /// <param name="amount">道具数量</param>
        /// <param name="price">道具单价</param>
        public static void Pay(double cash, PaySource source, string item, int amount, double price){
            #if !UNITY_EDITOR
            GA.Pay(cash,source,item,amount,price);
            #endif
        }

        /// <summary>
        /// 玩家使用虚拟币购买道具
        /// </summary>
        /// <param name="item">道具名称</param>
        /// <param name="amount">道具数量</param>
        /// <param name="price">道具单价</param>
        public static void Buy(string item, int amount, double price){
            #if !UNITY_EDITOR
            GA.Buy(item,amount,price);
            #endif
        }
        /// <summary>
        /// 玩家使用虚拟币购买道具
        /// </summary>
        /// <param name="item">道具名称</param>
        /// <param name="amount">道具数量</param>
        /// <param name="price">道具单价</param>
        public static void Use(string item, int amount, double price){
            #if !UNITY_EDITOR
            GA.Use(item,amount,price);
            #endif
        }
        /// <summary>
        /// 玩家获虚拟币奖励
        /// </summary>
        /// <param name="coin">虚拟币数量</param>
        /// <param name="source">奖励方式</param>
        public static void Bonus(double coin, BonusSource source){
            #if !UNITY_EDITOR
            GA.Bonus(coin,source);
            #endif
        }
        /// <summary>
        /// 玩家获道具奖励
        /// </summary>
        /// <param name="item">道具名称</param>
        /// <param name="amount">道具数量</param>
        /// <param name="price">道具单价</param>
        /// <param name="source">奖励方式</param>
        ///         
        public static void Bonus(string item, int amount, double price, BonusSource source){
            #if !UNITY_EDITOR
            GA.Bonus(item,amount,price,source);
            #endif
        }
        /// <summary>
        /// 使用sign-In函数后，如果结束该userId的统计，需要调用ProfileSignOfff函数provider : 不能以下划线"_"开头，使用大写字母和数字标识; 如果是上市公司，建议使用股票代码。
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="provider"></param>
		public static void ProfileSignIn(string userId,string provider){
            #if !UNITY_EDITOR
            GA.ProfileSignIn(userId,provider);
            #endif
        }
        /// <summary>
        /// 该结束该userId的统计
        /// </summary>
		public static  void ProfileSignOff(){
            #if !UNITY_EDITOR
            GA.ProfileSignOff();
            #endif
        }
        #endregion
    }


}
