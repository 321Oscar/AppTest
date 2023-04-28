﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTest.UI
{
    public static class UILocalize
    {
        /// <summary>
        /// 提示
        /// </summary>
        public static string InfoTitle = "提示";

        /// <summary>
        /// 正确
        /// </summary>
        public static string SuccessTitle = "正确";

        /// <summary>
        /// 警告
        /// </summary>
        public static string WarningTitle = "警告";

        /// <summary>
        /// 错误
        /// </summary>
        public static string ErrorTitle = "错误";

        /// <summary>
        /// 提示
        /// </summary>
        public static string AskTitle = "提示";

        /// <summary>
        /// 输入
        /// </summary>
        public static string InputTitle = "输入";

        /// <summary>
        /// 选择
        /// </summary>
        public static string SelectTitle = "选择";

        /// <summary>
        /// 全部关闭
        /// </summary>
        public static string CloseAll = "全部关闭";

        /// <summary>
        /// 确定
        /// </summary>
        public static string OK = "确定";

        /// <summary>
        /// 取消
        /// </summary>
        public static string Cancel = "取消";

        /// <summary>
        /// [ 无数据 ]
        /// </summary>
        public static string GridNoData = "[ 无数据 ]";

        /// <summary>
        /// 数据加载中，请稍候...
        /// </summary>
        public static string GridDataLoading = "数据加载中，请稍候...";

        /// <summary>
        /// 数据源必须为DataTable或者List
        /// </summary>
        public static string GridDataSourceException = "数据源必须为DataTable或者List";

        /// <summary>
        /// "系统正在处理中，请稍候..."
        /// </summary>
        public static string SystemProcessing = "系统正在处理中，请稍候...";

        public static string Monday = "一";
        public static string Tuesday = "二";
        public static string Wednesday = "三";
        public static string Thursday = "四";
        public static string Friday = "五";
        public static string Saturday = "六";
        public static string Sunday = "日";

        public static string Prev = "上一页";
        public static string Next = "下一页";
        public static string SelectPageLeft = "第";
        public static string SelectPageRight = "页";

        public static string January = "一月";
        public static string February = "二月";
        public static string March = "三月";
        public static string April = "四月";
        public static string May = "五月";
        public static string June = "六月";
        public static string July = "七月";
        public static string August = "八月";
        public static string September = "九月";
        public static string October = "十月";
        public static string November = "十一月";
        public static string December = "十二月";

        public static string Today = "今天";
    }

    public static class UILocalizeHelper
    {
        public static void SetEN()
        {
            UILocalize.InfoTitle = "Info";
            UILocalize.SuccessTitle = "Success";
            UILocalize.WarningTitle = "Warning";
            UILocalize.ErrorTitle = "Error";
            UILocalize.AskTitle = "Query";
            UILocalize.InputTitle = "Input";
            UILocalize.SelectTitle = "Select";
            UILocalize.CloseAll = "Close all";
            UILocalize.OK = "OK";
            UILocalize.Cancel = "Cancel";
            UILocalize.GridNoData = "[ No data ]";
            UILocalize.GridDataLoading = "Data loading, please wait...";
            UILocalize.GridDataSourceException = "The data source must be DataTable or List";
            UILocalize.SystemProcessing = "The system is processing, please wait...";

            UILocalize.Monday = "Mon.";
            UILocalize.Tuesday = "Tue.";
            UILocalize.Wednesday = "Wed.";
            UILocalize.Thursday = "Thur.";
            UILocalize.Friday = "Fri.";
            UILocalize.Saturday = "Sat.";
            UILocalize.Sunday = "Sun.";

            UILocalize.Prev = "Previous";
            UILocalize.Next = "Next";
            UILocalize.SelectPageLeft = "Page";
            UILocalize.SelectPageRight = "";

            UILocalize.January = "Jan.";
            UILocalize.February = "Feb.";
            UILocalize.March = "Mar.";
            UILocalize.April = "Apr.";
            UILocalize.May = "May";
            UILocalize.June = "Jun.";
            UILocalize.July = "Jul.";
            UILocalize.August = "Aug.";
            UILocalize.September = "Sep.";
            UILocalize.October = "Oct.";
            UILocalize.November = "Nov.";
            UILocalize.December = "Dec.";

            UILocalize.Today = "Today";

            UIStyles.Translate();
        }

        public static void SetCH()
        {
            UILocalize.InfoTitle = "提示";
            UILocalize.SuccessTitle = "正确";
            UILocalize.WarningTitle = "警告";
            UILocalize.ErrorTitle = "错误";
            UILocalize.AskTitle = "提示";
            UILocalize.InputTitle = "输入";
            UILocalize.SelectTitle = "选择";
            UILocalize.CloseAll = "全部关闭";
            UILocalize.OK = "确定";
            UILocalize.Cancel = "取消";
            UILocalize.GridNoData = "[ 无数据 ]";
            UILocalize.GridDataLoading = "数据加载中，请稍候...";
            UILocalize.GridDataSourceException = "数据源必须为DataTable或者List";
            UILocalize.SystemProcessing = "系统正在处理中，请稍候...";

            UILocalize.Monday = "一";
            UILocalize.Tuesday = "二";
            UILocalize.Wednesday = "三";
            UILocalize.Thursday = "四";
            UILocalize.Friday = "五";
            UILocalize.Saturday = "六";
            UILocalize.Sunday = "日";

            UILocalize.Prev = "上一页";
            UILocalize.Next = "下一页";

            UILocalize.SelectPageLeft = "第";
            UILocalize.SelectPageRight = "页";

            UILocalize.January = "一月";
            UILocalize.February = "二月";
            UILocalize.March = "三月";
            UILocalize.April = "四月";
            UILocalize.May = "五月";
            UILocalize.June = "六月";
            UILocalize.July = "七月";
            UILocalize.August = "八月";
            UILocalize.September = "九月";
            UILocalize.October = "十月";
            UILocalize.November = "十一月";
            UILocalize.December = "十二月";

            UILocalize.Today = "今天";

            UIStyles.Translate();
        }
    }
}
