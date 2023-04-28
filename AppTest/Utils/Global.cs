using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppTest.Helper
{
    /// <summary>
    /// 全局常量
    /// </summary>
    public class Global
    {
        /// <summary>
        /// 数据库文件
        /// </summary>
        public const string DBPATH = "\\Data\\MeasureInfo.db";
        public const string CONFIGPATH = "\\Config\\Config.db";
        public const string DATETIMEFORMAT = "yyyy-MM-dd HH:mm:ss fff";

        public enum ByteOrder
        {
            /// <summary>
            /// 解析的顺序是Byte0 Byte1:01 23
            /// </summary>
            Motorola,
            /// <summary>
            /// 解析的顺序是Byte1 Byte0：23 01
            /// </summary>
            Intel,
        }
        public static string ProjectPath = Application.StartupPath + "\\Config\\Project.json";
        /*
         * v0.001.beta
         * 
         * v0.001.01-beta
         * 1.dbc协议 发送小数数据时，报文组建错误导致影响其他信号值
         * 2.【set】增加步长
         * 
         * v0.001.02-beta
         * 1.CAN2EU 设置波特率失败
         * 2.调用SetRefrence失败
         * 3.【Get】读不到数据时，textbox设为0
         * 
         * v0.001.03-beta
         * 1.取消【Get】读不到数据时，textbox设为0
         * 
         * V0.001.04-beta
         * # 修改增加方案的布局
         * ## CAN通道支持删除
         * ## CAN卡类型修改寄存器同步修改
         * # 修改增加窗口的布局
         * # Get、Measure、Set窗口修改布局
         * # Set窗口获取信号的最值，修改时判断最值
         * 
         * v0.001.05-beta
         * 修改SignalUC控件
         * set界面调整，采用flowlayout，根据窗口宽度调整信号控件的排列
         * form可增加信号，并添加查询信号
         * 信号窗口增加缩放
         * textbox点击为最后一位
         * 曲线图取消点
         * 
         * # MainFormV2
         * 主界面为project信息，打开project单独创建个窗口，在其中进行信号的读写。
         * 
         * # 增加历史曲线图
         * # dbc协议增加读取信号周期
         * # 窗口缩放导致界面卡顿，取消缩放
         * 
         * # 增加RollingCounter界面，按照信号的周期自动发送数据
         * ## rollingcounter和checksum自动计算
         * 
         * v0.001.06-beta
         * GetForm 注释Debug代码
         * 
         * v0.001.07-beta
         * 软件界面增加新的模式，软件启动时选择模式
         * 测量变量排列方式优化
         * 【Set】界面增加回车发送，调整步长精度0->0.1，步长可增减
         * 调整测量变量在窗口大小改变时显示不完全的问题
         * 历史数据支持导出，并增加将导出的历史数据还原为曲线图的功能
         * 增加字体修改
         * 
         * v0.001.08-beta
         * RollingCounter发送问题，定时器改为Thread，UI控件不实时更新RollingCounter的值
         * Measure界面接收数据和曲线描点分为两个定时器
         * SignalInfoUS控件，添加SignalValue属性，减少UI控件的更改
         * 2E-U通道号没有保存 获取不到CAN1的数据
         * 
         * Measure曲线和图例不对应问题修复
         * 获取数据定时器改为用Thread，Task死循环读取
         * 
         * v.001.09-beta
         * 2E-U发送通道问题
         * 
         * v0.001.10-beta
         * SignalEntity结构调整，历史数据查询方式修改
         * DBC窗口增加手动解析
         * 新增信号支持搜索ID号
         * 新增自定义信号，点击信号可查看信号详细信息
         * 
         * Measure改为Scope
         * 曲线的点数开放
         * 曲线多坐标实现（待）
         * 选择的信号按照Message ID，StartBit排序
         * 信号手动调整显示顺序（待）
         * 
         * 单通道支持多个协议文档
         * 协议文档更新后，Form中的信号自动更新
         * dbc算法错误 少了一个tmp
         * 
         * v0.001.11-beta
         * CAN通道数量根据CAN类型固定
         * Scope界面多Y轴
         * CheckColor显示信号值
         * 
         * v0.001.11.1-beta
         * CAN通道数量bug
         * 
         * SignalControl 输入负数
         * rollingcounter 支持步长，用输入框修改的话90->100会先90->0->10->100
         * 界面关闭后dispose
         * 
         * 优化日志记录模块，方便查找问题
         * 
         * Project打开CAN失败后，删除内存中存储的USB-CAN
         * 双击打开窗口
         * 
         * v0.001.12-beta
         * 退出软件时 提示删除日志，db数据库
         * 增加存储数据功能开启，关闭
         * 开启多个【Get】/【scope】，之前会抢占receive
         *  可能会导致【Get】将【Receive】/【Get】需要的数据接收
         *  
         * 长时间运行会报内存不足
         * 内存不足还在排查
         * 
         * v0.002.01-beta
         * 重设计Get/Set界面，
         * 主界面增加状态
         * 界面增加双缓冲
         * USBCAN2eu 双通道启用优化 
         * 修改提示框
         * 增加记录打开窗口的大小和位置，下次自动打开
         * 
         * v0.002.02-beta
         * Scope改为zedGraph控件
         * 删除Set中的【乘】【除】
         * CANReceive改为事件委托
         * rollingcounter启动时检测CAN盒连接是否正常
         * 
         * v0.002.03-beta
         * 子窗口恢复最小化窗口
         * 子窗口添加切换MDI模式
         * 修改mdi子界面的样式
         * 信号增加自定义名称
         * RollingCounter使用datagridview会有发送延时的问题，界面回滚。
         * 增加ASC文件解析窗口
         * 
         * v0.002.04-beta
         * 增加对zlgcan.dll的调用，支持CANFD-200U的数据收发
         * 修改代码结构
         * 修复dbc'文件读取失败的bug
         * 修复Scope界面在获取数据时修改信号会导致程序卡死的bug
         * 
         * 修复修改信号界面，删除信号再重新添加相同的信号失败的bug--2022/10/26
         * 
         * v0.002.05-beta
         * 增加XCP模块（polling）
         * 增加A2L文件解析
         * 修复RollingCounter界面启动发送后，Get界面无法获取数据的Bug（zlgcan 使用自收自发后，can收不到新数据）
         * Signal结构修改,projectItem结构修改
         * 
         * v0.003.01-beta
         * 增加XCPDAQForm，XCPDAQScopeForm
         * XCPSignal结构修改
         * 
         * v0.003.02-beta
         * XCP CANIndex=1时 连接失败的bug
         * addnewform 添加信号失败的bug
         * Form CANchannel变化bug
         * 
         * v0.003.03-beta
         * 解析a2l转换成XCPSignal时，对变量的类型解析方式修改
         * xcp连接，导入elf 图标更改
         * addxcpnewForm 增加rollingcounter验证
         * 增加数据长度>8的download
         * 
         * 增加数据保存，波形回放（内置数据）
         * 数据库init
         * 
         * v0.003.04-beta
         * 设备为2EU时，只启用CAN1时，打开通道失败
         * 修复Set信号最值为0的bug
         * XCP信号调整顺序
         * 修复新建XCP窗口，信号名显示不全
         * 修复Set下拉框信号名显示不全
         * 
         * 【DAQGet】窗口增加最值判断，超限变红
         * 修复修改XCP信号时，当窗口类型为【Set】时，初始化信号错误的bug
         * 
         * v0.003.05-beta
         * 1.修改XCP数据解析，地址转换等方法
         * 2.存储数据按钮放到左下角
         * 3.DAQGetForm修改颜色
         * 4.历史数据界面增加复制信号名
         * 
         * v0.003.06-beta
         * 1.修复导入elf文件更新地址 固定更新can0的a2l文件的bug
         * 2.删除excel格式解析数据
         * 2023/04/20
         * 1.增加软件校验：读取DID判断次数
         * 2.XCP增加sendNkey
         * 3.XCPDAQ保存信号增加报文时间戳
         * 4.增加UDS库
         * 5.修改历史记录界面，增加分页
         * 
         * v0.003.07-beta
         * 1.增加日志ListView
         * 2.校验软件次数 DID的factory
         * 2.1 校验软件 取消waiting cursor
         * 2.2 修改次数超限后的 窗口使能
         * 3.优化解析dbc
         */

        /// <summary>
        /// 版本号
        /// </summary>
        public const string SoftVersion = "v10.003.07-beta";

        /// <summary>
        /// Protect = 1时，版本号为10.xxx.xx
        /// </summary>
        public const int Protected = 1;

        #region 图标
        public static System.IntPtr IconHandle_ProjectCenter = (global::AppTest.Properties.Resources.ProjectCenter).GetHicon();
        public static System.IntPtr IconHandle_Project = (global::AppTest.Properties.Resources.Project_Color).GetHicon();
        public static System.IntPtr IconHandle_Measure = (global::AppTest.Properties.Resources.Measure_color).GetHicon();
        public static System.IntPtr IconHandle_Set = (global::AppTest.Properties.Resources.Set).GetHicon();
        public static System.IntPtr IconHandle_Get = (global::AppTest.Properties.Resources.Get).GetHicon();
        public static System.IntPtr IconHandle_RL = (global::AppTest.Properties.Resources.RL).GetHicon();
        #endregion

        /// <summary>
        /// 主界面显示类型
        /// </summary>
        public static int ShowType = 1;

        public static Font CurrentFont = new Font("Microsoft YaHei UI", 9);

    }
}
