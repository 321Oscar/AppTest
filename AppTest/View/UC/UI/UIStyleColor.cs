﻿/******************************************************************************
 * SunnyUI 开源控件库、工具类库、扩展类库、多页面开发框架。
 * CopyRight (C) 2012-2021 ShenYongHua(沈永华).
 * QQ群：56829229 QQ：17612584 EMail：SunnyUI@QQ.Com
 *
 * Blog:   https://www.cnblogs.com/yhuse
 * Gitee:  https://gitee.com/yhuse/SunnyUI
 * GitHub: https://github.com/yhuse/SunnyUI
 *
 * SunnyUI.dll can be used for free under the GPL-3.0 license.
 * If you use this code, please keep this note.
 * 如果您使用此代码，请保留此说明。
 ******************************************************************************
 * 文件名称: UIStyleColor.cs
 * 文件说明: 控件样式定义类
 * 当前版本: V3.0
 * 创建日期: 2020-01-01
 *
 * 2020-01-01: V2.2.0 增加文件说明
 * 2020-04-25: V2.2.4 更新主题配置类
******************************************************************************/

using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace AppTest.UI
{
    public class UIBaseStyle
    {
        public virtual UIStyle Name { get; protected set; }

        public virtual Color PrimaryColor { get; protected set; }
        public virtual Color RegularColor { get; protected set; }
        public virtual Color SecondaryColor { get; protected set; }
        public virtual Color PlainColor { get; protected set; }

        public virtual Color RectColor { get; protected set; }
        public virtual Color RectHoverColor { get; protected set; }
        public virtual Color RectPressColor { get; protected set; }
        public virtual Color RectSelectedColor { get; protected set; }

        public virtual Color ButtonFillColor { get; protected set; }
        public virtual Color ButtonFillHoverColor { get; protected set; }
        public virtual Color ButtonFillPressColor { get; protected set; }
        public virtual Color ButtonFillSelectedColor { get; protected set; }

        public virtual Color ButtonForeColor { get; protected set; }
        public virtual Color ButtonForeHoverColor { get; protected set; }
        public virtual Color ButtonForePressColor { get; protected set; }
        public virtual Color ButtonForeSelectedColor { get; protected set; }

        public virtual Color FillDisableColor => Color.FromArgb(244, 244, 244);
        public virtual Color RectDisableColor => Color.FromArgb(173, 178, 181);
        public virtual Color ForeDisableColor => Color.FromArgb(109, 109, 103);

        public virtual Color LabelForeColor => UIFontColor.Primary;

        public virtual Color AvatarFillColor => Color.Silver;
        public virtual Color AvatarForeColor => PrimaryColor;

        public virtual Color CheckBoxColor => PrimaryColor;
        public virtual Color CheckBoxForeColor => LabelForeColor;
        public virtual Color PanelForeColor => LabelForeColor;

        public virtual Color DropDownControlColor => PanelForeColor;

        public virtual Color TitleColor { get; protected set; }
        public virtual Color TitleForeColor { get; protected set; }

        public virtual Color MenuSelectedColor { get; protected set; } = UIColor.Blue;

        public virtual Color GridSelectedColor { get; protected set; } = Color.FromArgb(155, 200, 255);

        public virtual Color GridSelectedForeColor => UIFontColor.Primary;
        public virtual Color GridStripeEvenColor => Color.White;
        public virtual Color GridStripeOddColor => PlainColor;

        public virtual Color GridLineColor => Color.FromArgb(233, 236, 244);

        public virtual Color ListItemSelectBackColor => PrimaryColor;
        public virtual Color ListItemSelectForeColor => PlainColor;

        public virtual Color LineForeColor => UIFontColor.Primary;

        public virtual Color ContextMenuColor => PlainColor;

        public virtual Color ProgressIndicatorColor => PrimaryColor;

        public virtual Color ProcessBarFillColor => PlainColor;

        public virtual Color ProcessBarForeColor => PrimaryColor;

        public virtual Color ScrollBarForeColor => PrimaryColor;

        public virtual Color SwitchActiveColor => PrimaryColor;

        public virtual Color SwitchInActiveColor => Color.Gray;

        public virtual Color SwitchFillColor => Color.White;

        public virtual Color TrackBarFillColor => PlainColor;

        public virtual Color TrackBarForeColor => PrimaryColor;

        public virtual Color TrackBarRectColor => PrimaryColor;

        public virtual Color TrackDisableColor => Color.Silver;

        public virtual Color PageTitleFillColor => Color.FromArgb(76, 76, 76);

        public virtual Color PageTitleForeColor => Color.White;

        public virtual Color TreeViewSelectedColor => PrimaryColor;

        public virtual Color TreeViewHoverColor => GridSelectedColor;

        public virtual bool BuiltIn => true;

        public virtual void LoadFromFile()
        {
        }

        public override string ToString()
        {
            return Name.ToString();
        }

        protected virtual void Init(Color color, UIStyle style, Color foreColor)
        {
            Name = style;

            MenuSelectedColor = TitleColor = RectColor = PrimaryColor = color;
            Color[] colors = Color.White.GradientColors(PrimaryColor, 16);
            Color[] colors1 = PrimaryColor.GradientColors(Color.Black, 16);
            PlainColor = colors[1];
            SecondaryColor = colors[5];
            RegularColor = colors[10];

            ButtonFillColor = PrimaryColor;
            RectHoverColor = ButtonFillHoverColor = colors[12];
            RectSelectedColor = RectPressColor = ButtonFillSelectedColor = ButtonFillPressColor = colors1[3];
            GridSelectedColor = colors[3];

            ButtonForeColor = ButtonForeHoverColor = ButtonForePressColor = ButtonForeSelectedColor = TitleForeColor = foreColor;
        }

        protected virtual void InitPlain(Color color, UIStyle style, Color foreColor)
        {
            Name = style;
            MenuSelectedColor = RectColor = RectHoverColor = TitleColor = ButtonForeColor = ButtonFillHoverColor = PrimaryColor = color;
            Color[] colors = Color.White.GradientColors(PrimaryColor, 16);
            Color[] colors1 = PrimaryColor.GradientColors(Color.Black, 16);
            ButtonFillColor = PlainColor = colors[1];
            SecondaryColor = colors[5];
            RegularColor = colors[10];
            ButtonFillSelectedColor = RectPressColor = RectSelectedColor = ButtonFillPressColor = colors1[3];
            GridSelectedColor = colors[3];
            ButtonForeHoverColor = ButtonForePressColor = ButtonForeSelectedColor = TitleForeColor = foreColor;
        }
    }

    public class UIPurpleStyle : UIBaseStyle
    {
        public UIPurpleStyle()
        {
            Init(UIColor.Purple, UIStyle.Purple, Color.White);
        }
    }

    public class UILightPurpleStyle : UIBaseStyle
    {
        public UILightPurpleStyle()
        {
            InitPlain(UIColor.Purple, UIStyle.LightPurple, Color.White);
        }
    }

    public class UIColorfulStyle : UIBaseStyle
    {
        public UIColorfulStyle()
        {
            Init(Color.FromArgb(0, 190, 172), UIStyle.Colorful, Color.White);
        }

        public void Init(Color styleColor, Color foreColor)
        {
            Init(styleColor, UIStyle.Colorful, foreColor);
        }
    }

    public class UICustomStyle : UIBlueStyle
    {
        public override UIStyle Name => UIStyle.Custom;
    }

    public class UIOffice2010BlueStyle : UIBaseStyle
    {
        public override UIStyle Name => UIStyle.Office2010Blue;
        public override Color PrimaryColor => Color.FromArgb(120, 148, 182);
        public override Color RegularColor => Color.FromArgb(120, 148, 182);
        public override Color SecondaryColor => Color.FromArgb(120, 148, 182);
        public override Color PlainColor => Color.FromArgb(207, 221, 238);
        public override Color ButtonFillColor => Color.FromArgb(217, 230, 243);
        public override Color ButtonFillHoverColor => Color.FromArgb(249, 226, 137);
        public override Color ButtonFillPressColor => Color.FromArgb(255, 228, 137);
        public override Color ButtonForeColor => Color.FromArgb(30, 57, 91);
        public override Color ButtonForeHoverColor => Color.FromArgb(30, 57, 91);
        public override Color ButtonForePressColor => Color.FromArgb(30, 57, 91);
        public override Color RectColor => Color.FromArgb(180, 192, 211);
        public override Color RectHoverColor => Color.FromArgb(238, 201, 88);
        public override Color RectPressColor => Color.FromArgb(194, 118, 43);
        public override Color TitleColor => Color.FromArgb(191, 210, 233);
        public override Color TitleForeColor => Color.FromArgb(30, 57, 91);
        public override Color RectSelectedColor => RectPressColor;
        public override Color ButtonForeSelectedColor => ButtonForePressColor;
        public override Color ButtonFillSelectedColor => ButtonFillPressColor;
    }

    public class UIOffice2010SilverStyle : UIBaseStyle
    {
        public override UIStyle Name => UIStyle.Office2010Silver;
        public override Color PrimaryColor => Color.FromArgb(139, 144, 151);
        public override Color RegularColor => Color.FromArgb(120, 148, 182);
        public override Color SecondaryColor => Color.FromArgb(120, 148, 182);
        public override Color PlainColor => Color.FromArgb(224, 228, 233);
        public override Color ButtonFillColor => Color.FromArgb(247, 248, 249);
        public override Color ButtonFillHoverColor => Color.FromArgb(249, 226, 137);
        public override Color ButtonFillPressColor => Color.FromArgb(255, 228, 137);
        public override Color ButtonForeColor => Color.FromArgb(46, 46, 46);
        public override Color ButtonForeHoverColor => Color.FromArgb(46, 46, 46);
        public override Color ButtonForePressColor => Color.FromArgb(46, 46, 46);
        public override Color RectSelectedColor => RectPressColor;
        public override Color ButtonForeSelectedColor => ButtonForePressColor;
        public override Color ButtonFillSelectedColor => ButtonFillPressColor;
        public override Color RectColor => Color.FromArgb(139, 144, 151);
        public override Color RectHoverColor => Color.FromArgb(238, 201, 88);
        public override Color RectPressColor => Color.FromArgb(194, 118, 43);
        public override Color TitleColor => Color.Silver;
        public override Color TitleForeColor => Color.FromArgb(46, 46, 46);
    }

    public class UIOffice2010BlackStyle : UIBaseStyle
    {
        public override UIStyle Name => UIStyle.Office2010Black;
        public override Color PrimaryColor => Color.FromArgb(49, 49, 49);
        public override Color RegularColor => Color.FromArgb(120, 148, 182);
        public override Color SecondaryColor => Color.FromArgb(120, 148, 182);
        public override Color PlainColor => Color.FromArgb(211, 211, 211);
        public override Color ButtonFillColor => Color.FromArgb(211, 211, 211);
        public override Color ButtonFillHoverColor => Color.FromArgb(249, 226, 137);
        public override Color ButtonFillPressColor => Color.FromArgb(255, 228, 137);
        public override Color ButtonForeColor => Color.Black;
        public override Color ButtonForeHoverColor => Color.FromArgb(70, 70, 70);
        public override Color ButtonForePressColor => Color.FromArgb(70, 70, 70);
        public override Color RectSelectedColor => RectPressColor;
        public override Color ButtonForeSelectedColor => ButtonForePressColor;
        public override Color ButtonFillSelectedColor => ButtonFillPressColor;
        public override Color RectColor => Color.FromArgb(145, 145, 145);
        public override Color RectHoverColor => Color.FromArgb(238, 201, 88);
        public override Color RectPressColor => Color.FromArgb(194, 118, 43);
        public override Color AvatarFillColor => Color.FromArgb(148, 148, 148);
        public override Color TitleColor => Color.FromArgb(118, 118, 118);
        public override Color TitleForeColor => Color.Black;
    }

    public class UIBlueStyle : UIBaseStyle
    {
        public override UIStyle Name => UIStyle.Blue;
        public override Color PrimaryColor => UIColor.Blue;
        public override Color RegularColor => Color.FromArgb(120, 148, 182);
        public override Color SecondaryColor => Color.FromArgb(120, 148, 182);
        public override Color PlainColor => UIColor.LightBlue;
        public override Color ButtonFillColor => UIColor.Blue;
        public override Color ButtonFillHoverColor => Color.FromArgb(111, 168, 255);
        public override Color ButtonFillPressColor => Color.FromArgb(74, 131, 229);
        public override Color ButtonForeColor => Color.White;
        public override Color ButtonForeHoverColor => Color.White;
        public override Color ButtonForePressColor => Color.White;
        public override Color RectSelectedColor => RectPressColor;
        public override Color ButtonForeSelectedColor => ButtonForePressColor;
        public override Color ButtonFillSelectedColor => ButtonFillPressColor;
        public override Color RectColor => UIColor.Blue;
        public override Color RectHoverColor => Color.FromArgb(111, 168, 255);
        public override Color RectPressColor => Color.FromArgb(74, 131, 229);
        public override Color TitleColor => UIColor.Blue;
        public override Color TitleForeColor => Color.White;
    }

    public class UILightBlueStyle : UIBaseStyle
    {
        public override UIStyle Name => UIStyle.LightBlue;
        public override Color PrimaryColor => UIColor.Blue;
        public override Color RegularColor => Color.FromArgb(120, 148, 182);
        public override Color SecondaryColor => Color.FromArgb(120, 148, 182);
        public override Color PlainColor => UIColor.LightBlue;
        public override Color ButtonFillColor => UIColor.LightBlue;
        public override Color ButtonFillHoverColor => UIColor.Blue;
        public override Color ButtonFillPressColor => Color.FromArgb(74, 131, 229);
        public override Color ButtonForeColor => UIColor.Blue;
        public override Color ButtonForeHoverColor => Color.White;
        public override Color ButtonForePressColor => Color.White;
        public override Color RectSelectedColor => RectPressColor;
        public override Color ButtonForeSelectedColor => ButtonForePressColor;
        public override Color ButtonFillSelectedColor => ButtonFillPressColor;
        public override Color RectColor => UIColor.Blue;
        public override Color RectHoverColor => UIColor.Blue;
        public override Color RectPressColor => Color.FromArgb(74, 131, 229);
        public override Color TitleColor => UIColor.Blue;
        public override Color TitleForeColor => Color.White;
    }

    public class UIGreenStyle : UIBaseStyle
    {
        public override UIStyle Name => UIStyle.Green;
        public override Color PrimaryColor => UIColor.Green;
        public override Color RegularColor => Color.FromArgb(120, 148, 182);
        public override Color SecondaryColor => Color.FromArgb(120, 148, 182);
        public override Color PlainColor => UIColor.LightGreen;
        public override Color ButtonFillColor => UIColor.Green;
        public override Color ButtonFillHoverColor => Color.FromArgb(136, 202, 81);
        public override Color ButtonFillPressColor => Color.FromArgb(100, 168, 35);
        public override Color ButtonForeColor => Color.White;
        public override Color ButtonForeHoverColor => Color.White;
        public override Color ButtonForePressColor => Color.White;
        public override Color RectSelectedColor => RectPressColor;
        public override Color ButtonForeSelectedColor => ButtonForePressColor;
        public override Color ButtonFillSelectedColor => ButtonFillPressColor;
        public override Color RectColor => UIColor.Green;
        public override Color RectHoverColor => Color.FromArgb(136, 202, 81);
        public override Color RectPressColor => Color.FromArgb(100, 168, 35);
        public override Color TitleColor => UIColor.Green;
        public override Color TitleForeColor => Color.White;
        public override Color MenuSelectedColor => UIColor.Green;
        public override Color GridSelectedColor => Color.FromArgb(173, 227, 123);
    }

    public class UILightGreenStyle : UIBaseStyle
    {
        public override UIStyle Name => UIStyle.LightGreen;
        public override Color PrimaryColor => UIColor.Green;
        public override Color RegularColor => Color.FromArgb(120, 148, 182);
        public override Color SecondaryColor => Color.FromArgb(120, 148, 182);
        public override Color PlainColor => UIColor.LightGreen;
        public override Color ButtonFillColor => UIColor.LightGreen;
        public override Color ButtonFillHoverColor => UIColor.Green;
        public override Color ButtonFillPressColor => Color.FromArgb(100, 168, 35);
        public override Color ButtonForeColor => UIColor.Green;
        public override Color ButtonForeHoverColor => Color.White;
        public override Color ButtonForePressColor => Color.White;
        public override Color RectSelectedColor => RectPressColor;
        public override Color ButtonForeSelectedColor => ButtonForePressColor;
        public override Color ButtonFillSelectedColor => ButtonFillPressColor;
        public override Color RectColor => UIColor.Green;
        public override Color RectHoverColor => UIColor.Green;
        public override Color RectPressColor => Color.FromArgb(100, 168, 35);
        public override Color TitleColor => UIColor.Green;
        public override Color TitleForeColor => Color.White;
        public override Color MenuSelectedColor => UIColor.Green;
        public override Color GridSelectedColor => Color.FromArgb(173, 227, 123);
    }

    public class UIRedStyle : UIBaseStyle
    {
        public override UIStyle Name => UIStyle.Red;
        public override Color PrimaryColor => UIColor.Red;
        public override Color RegularColor => Color.FromArgb(120, 148, 182);
        public override Color SecondaryColor => Color.FromArgb(120, 148, 182);
        public override Color PlainColor => UIColor.LightRed;
        public override Color ButtonFillColor => UIColor.Red;
        public override Color ButtonFillHoverColor => Color.FromArgb(232, 127, 128);
        public override Color ButtonFillPressColor => Color.FromArgb(202, 87, 89);
        public override Color ButtonForeColor => Color.White;
        public override Color ButtonForeHoverColor => Color.White;
        public override Color ButtonForePressColor => Color.White;
        public override Color RectSelectedColor => RectPressColor;
        public override Color ButtonForeSelectedColor => ButtonForePressColor;
        public override Color ButtonFillSelectedColor => ButtonFillPressColor;
        public override Color RectColor => UIColor.Red;
        public override Color RectHoverColor => Color.FromArgb(232, 127, 128);
        public override Color RectPressColor => Color.FromArgb(202, 87, 89);
        public override Color TitleColor => UIColor.Red;
        public override Color TitleForeColor => Color.White;
        public override Color MenuSelectedColor => UIColor.Red;
        public override Color GridSelectedColor => Color.FromArgb(241, 160, 160);
    }

    public class UILightRedStyle : UIBaseStyle
    {
        public override UIStyle Name => UIStyle.LightRed;
        public override Color PrimaryColor => UIColor.Red;
        public override Color RegularColor => Color.FromArgb(120, 148, 182);
        public override Color SecondaryColor => Color.FromArgb(120, 148, 182);
        public override Color PlainColor => UIColor.LightRed;
        public override Color ButtonFillColor => UIColor.LightRed;
        public override Color ButtonFillHoverColor => UIColor.Red;
        public override Color ButtonFillPressColor => Color.FromArgb(202, 87, 89);
        public override Color ButtonForeColor => UIColor.Red;
        public override Color ButtonForeHoverColor => Color.White;
        public override Color ButtonForePressColor => Color.White;
        public override Color RectSelectedColor => RectPressColor;
        public override Color ButtonForeSelectedColor => ButtonForePressColor;
        public override Color ButtonFillSelectedColor => ButtonFillPressColor;
        public override Color RectColor => UIColor.Red;
        public override Color RectHoverColor => UIColor.Red;
        public override Color RectPressColor => Color.FromArgb(202, 87, 89);
        public override Color TitleColor => UIColor.Red;
        public override Color TitleForeColor => Color.White;
        public override Color MenuSelectedColor => UIColor.Red;
        public override Color GridSelectedColor => Color.FromArgb(241, 160, 160);
    }

    public class UIOrangeStyle : UIBaseStyle
    {
        public override UIStyle Name => UIStyle.Orange;
        public override Color PrimaryColor => UIColor.Orange;
        public override Color RegularColor => Color.FromArgb(120, 148, 182);
        public override Color SecondaryColor => Color.FromArgb(120, 148, 182);
        public override Color PlainColor => UIColor.LightOrange;
        public override Color ButtonFillColor => UIColor.Orange;
        public override Color ButtonFillHoverColor => Color.FromArgb(223, 174, 86);
        public override Color ButtonFillPressColor => Color.FromArgb(192, 137, 43);
        public override Color ButtonForeColor => Color.White;
        public override Color ButtonForeHoverColor => Color.White;
        public override Color ButtonForePressColor => Color.White;
        public override Color RectSelectedColor => RectPressColor;
        public override Color ButtonForeSelectedColor => ButtonForePressColor;
        public override Color ButtonFillSelectedColor => ButtonFillPressColor;
        public override Color RectColor => UIColor.Orange;
        public override Color RectHoverColor => Color.FromArgb(223, 174, 86);
        public override Color RectPressColor => Color.FromArgb(192, 137, 43);
        public override Color TitleColor => UIColor.Orange;
        public override Color TitleForeColor => Color.White;
        public override Color MenuSelectedColor => UIColor.Orange;
        public override Color GridSelectedColor => Color.FromArgb(238, 207, 151);
    }

    public class UILightOrangeStyle : UIBaseStyle
    {
        public override UIStyle Name => UIStyle.LightOrange;
        public override Color PrimaryColor => UIColor.Orange;
        public override Color RegularColor => Color.FromArgb(120, 148, 182);
        public override Color SecondaryColor => Color.FromArgb(120, 148, 182);
        public override Color PlainColor => UIColor.LightOrange;
        public override Color ButtonFillColor => UIColor.LightOrange;
        public override Color ButtonFillHoverColor => UIColor.Orange;
        public override Color ButtonFillPressColor => Color.FromArgb(192, 137, 43);
        public override Color ButtonForeColor => UIColor.Orange;
        public override Color ButtonForeHoverColor => Color.White;
        public override Color RectSelectedColor => RectPressColor;
        public override Color ButtonForeSelectedColor => ButtonForePressColor;
        public override Color ButtonFillSelectedColor => ButtonFillPressColor;
        public override Color ButtonForePressColor => Color.White;
        public override Color RectColor => UIColor.Orange;
        public override Color RectHoverColor => UIColor.Orange;
        public override Color RectPressColor => Color.FromArgb(192, 137, 43);
        public override Color TitleColor => UIColor.Orange;
        public override Color TitleForeColor => Color.White;
        public override Color MenuSelectedColor => UIColor.Orange;
        public override Color GridSelectedColor => Color.FromArgb(238, 207, 151);
    }

    public class UIGrayStyle : UIBaseStyle
    {
        public override UIStyle Name => UIStyle.Gray;
        public override Color PrimaryColor => UIColor.Gray;
        public override Color RegularColor => Color.FromArgb(120, 148, 182);
        public override Color SecondaryColor => Color.FromArgb(120, 148, 182);
        public override Color PlainColor => UIColor.LightGray;
        public override Color ButtonFillColor => UIColor.Gray;
        public override Color ButtonFillHoverColor => Color.FromArgb(158, 160, 165);
        public override Color ButtonFillPressColor => Color.FromArgb(121, 123, 129);
        public override Color ButtonForeColor => Color.White;
        public override Color ButtonForeHoverColor => Color.White;
        public override Color ButtonForePressColor => Color.White;
        public override Color RectSelectedColor => RectPressColor;
        public override Color ButtonForeSelectedColor => ButtonForePressColor;
        public override Color ButtonFillSelectedColor => ButtonFillPressColor;
        public override Color RectColor => UIColor.Gray;
        public override Color RectHoverColor => Color.FromArgb(158, 160, 165);
        public override Color RectPressColor => Color.FromArgb(121, 123, 129);
        public override Color TitleColor => UIColor.Gray;
        public override Color TitleForeColor => Color.White;
        public override Color GridSelectedColor => Color.Silver;
    }

    public class UILightGrayStyle : UIBaseStyle
    {
        public override UIStyle Name => UIStyle.LightGray;
        public override Color PrimaryColor => UIColor.Gray;
        public override Color RegularColor => Color.FromArgb(120, 148, 182);
        public override Color SecondaryColor => Color.FromArgb(120, 148, 182);
        public override Color PlainColor => UIColor.LightGray;
        public override Color ButtonFillColor => UIColor.LightGray;
        public override Color ButtonFillHoverColor => UIColor.Gray;
        public override Color ButtonFillPressColor => Color.FromArgb(121, 123, 129);
        public override Color ButtonForeColor => UIColor.Gray;
        public override Color ButtonForeHoverColor => Color.White;
        public override Color ButtonForePressColor => Color.White;
        public override Color RectSelectedColor => RectPressColor;
        public override Color ButtonForeSelectedColor => ButtonForePressColor;
        public override Color ButtonFillSelectedColor => ButtonFillPressColor;
        public override Color RectColor => UIColor.Gray;
        public override Color RectHoverColor => UIColor.Gray;
        public override Color RectPressColor => Color.FromArgb(121, 123, 129);
        public override Color TitleColor => UIColor.Gray;
        public override Color TitleForeColor => Color.White;
        public override Color GridSelectedColor => Color.Silver;
    }

    public class UIWhiteStyle : UIBaseStyle
    {
        public override UIStyle Name => UIStyle.White;
        public override Color PrimaryColor => Color.FromArgb(216, 219, 227);
        public override Color RegularColor => Color.FromArgb(120, 148, 182);
        public override Color SecondaryColor => Color.FromArgb(120, 148, 182);
        public override Color PlainColor => Color.White;
        public override Color ButtonFillColor => Color.White;
        public override Color ButtonFillHoverColor => UIColor.LightBlue;
        public override Color ButtonFillPressColor => UIColor.LightBlue;
        public override Color ButtonForeColor => Color.FromArgb(0x60, 0x62, 0x66);
        public override Color ButtonForeHoverColor => UIColor.Blue;
        public override Color ButtonForePressColor => Color.FromArgb(74, 131, 229);
        public override Color RectSelectedColor => RectPressColor;
        public override Color ButtonForeSelectedColor => ButtonForePressColor;
        public override Color ButtonFillSelectedColor => ButtonFillPressColor;
        public override Color RectColor => Color.FromArgb(216, 219, 227);
        public override Color RectHoverColor => Color.FromArgb(197, 222, 255);
        public override Color RectPressColor => Color.FromArgb(74, 131, 229);
        public override Color AvatarFillColor => Color.FromArgb(130, 130, 130);
        public override Color TitleColor => Color.FromArgb(216, 219, 227);
        public override Color TitleForeColor => Color.FromArgb(0x60, 0x62, 0x66);
    }

    public class UIDarkBlueStyle : UIBaseStyle
    {
        public override UIStyle Name => UIStyle.DarkBlue;
        public override Color PrimaryColor => UIColor.DarkBlue;
        public override Color RegularColor => Color.FromArgb(120, 148, 182);
        public override Color SecondaryColor => Color.FromArgb(120, 148, 182);
        public override Color PlainColor => UIColor.LightGray;
        public override Color ButtonFillColor => UIColor.DarkBlue;
        public override Color ButtonFillHoverColor => Color.FromArgb(190, 230, 253);
        public override Color ButtonFillPressColor => Color.FromArgb(169, 217, 242);
        public override Color ButtonForeColor => Color.FromArgb(130, 130, 130);
        public override Color ButtonForeHoverColor => Color.FromArgb(130, 130, 130);
        public override Color ButtonForePressColor => Color.FromArgb(130, 130, 130);
        public override Color RectSelectedColor => RectPressColor;
        public override Color ButtonForeSelectedColor => ButtonForePressColor;
        public override Color ButtonFillSelectedColor => ButtonFillPressColor;
        public override Color RectColor => Color.FromArgb(130, 130, 130);
        public override Color RectHoverColor => Color.FromArgb(130, 130, 130);
        public override Color RectPressColor => Color.FromArgb(130, 130, 130);
        public override Color TitleColor => Color.FromArgb(130, 130, 130);
        public override Color TitleForeColor => Color.White;
    }

    public class UIBlackStyle : UIBaseStyle
    {
        public override UIStyle Name => UIStyle.Black;
        public override Color PrimaryColor => Color.FromArgb(24, 24, 24);
        public override Color RegularColor => Color.FromArgb(120, 148, 182);
        public override Color SecondaryColor => Color.FromArgb(120, 148, 182);
        public override Color PlainColor => Color.FromArgb(24, 24, 24);
        public override Color ButtonFillColor => UIColor.DarkBlue;
        public override Color ButtonFillHoverColor => UIColor.RegularBlue;
        public override Color ButtonFillPressColor => UIColor.LightBlue;
        public override Color ButtonForeColor => Color.White;
        public override Color ButtonForeHoverColor => Color.FromArgb(130, 130, 130);
        public override Color ButtonForePressColor => Color.FromArgb(130, 130, 130);
        public override Color RectSelectedColor => RectPressColor;
        public override Color ButtonForeSelectedColor => ButtonForePressColor;
        public override Color ButtonFillSelectedColor => ButtonFillPressColor;
        public override Color RectColor => Color.FromArgb(130, 130, 130);
        public override Color RectHoverColor => Color.FromArgb(130, 130, 130);
        public override Color RectPressColor => Color.FromArgb(130, 130, 130);
        public override Color LabelForeColor => UIFontColor.Plain;
        public override Color DropDownControlColor => UIFontColor.Primary;
        public override Color CheckBoxColor => UIColor.Blue;

        public override Color TitleColor => Color.FromArgb(130, 130, 130);
        public override Color TitleForeColor => Color.White;
        public override Color LineForeColor => UIFontColor.Plain;
        public override Color ContextMenuColor => UIColor.RegularGray;

        public override Color GridStripeOddColor => UIColor.RegularGray;
        public override Color GridSelectedColor => UIFontColor.Plain;

        public override Color GridSelectedForeColor => UIColor.White;

        public override Color ListItemSelectBackColor => UIColor.Blue;
        public override Color ListItemSelectForeColor => UIColor.LightBlue;

        public override Color ProgressIndicatorColor => UIColor.Blue;

        public override Color ProcessBarFillColor => PlainColor;

        public override Color ProcessBarForeColor => UIColor.RegularGray;

        public override Color ScrollBarForeColor => UIColor.RegularGray;

        public override Color SwitchActiveColor => UIColor.DarkBlue;

        public override Color SwitchInActiveColor => UIFontColor.Plain;

        public override Color SwitchFillColor => Color.White;

        public override Color TrackBarForeColor => UIColor.Blue;

        public override Color TrackBarRectColor => UIColor.Blue;

        public override Color TrackDisableColor => Color.Silver;

        public override Color TreeViewSelectedColor => UIFontColor.Secondary;

        public override Color TreeViewHoverColor => UIFontColor.Plain;
    }

    public static class GDI {
        public static Color[] GradientColors(this Color startColor, Color endColor, int count)
        {
            count = Math.Max(count, 2);
            Bitmap image = new Bitmap(1024, 3);
            Graphics g = image.Graphics();
            Brush br = new LinearGradientBrush(image.Bounds(), startColor, endColor, 0.0F);
            g.FillRectangle(br, image.Bounds());
            br.Dispose();
            g.Dispose();

            Color[] colors = new Color[count];
            colors[0] = startColor;
            colors[count - 1] = endColor;

            if (count > 2)
            {
                FastBitmap fb = new FastBitmap(image);
                fb.Lock();
                for (int i = 1; i < count - 1; i++)
                {
                    colors[i] = fb.GetPixel(image.Width * i / (count - 1), 1);
                }

                fb.Unlock();
                fb.Dispose();
            }

            image.Dispose();
            return colors;
        }

        /// <summary>
        /// 设置GDI高质量模式抗锯齿
        /// </summary>
        /// <param name="g"></param>
        public static void SetHighQuality(this Graphics g)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;  //使绘图质量最高，即消除锯齿
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.CompositingQuality = CompositingQuality.HighQuality;
        }

        /// <summary>
        /// 设置GDI默认值
        /// </summary>
        /// <param name="g"></param>
        public static void SetDefaultQuality(this Graphics g)
        {
            g.SmoothingMode = SmoothingMode.Default;
            g.InterpolationMode = InterpolationMode.Default;
            g.CompositingQuality = CompositingQuality.Default;
        }
    }
}