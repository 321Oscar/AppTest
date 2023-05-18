using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppTest.Utils.LogUtils
{
    public class TextBoxBaseAppender : AppenderSkeleton
    {
        public TextBoxBase TextBox { get; set; }

        protected override void Append(LoggingEvent loggingEvent)
        {
            if (this.TextBox == null)
                return;

            if (!this.TextBox.IsHandleCreated)
            {
                return;
            }

            if (this.TextBox.IsDisposed)
            {
                return;
            }


            var str = string.Empty;
            if (this.Layout is PatternLayout patternLayout)
            {
                str = patternLayout.Format(loggingEvent);

                if(loggingEvent.ExceptionObject != null)
                {
                    str += loggingEvent.ExceptionObject.ToString() + Environment.NewLine;
                }
            }
            else
            {
                str = loggingEvent.LoggerName + "-" + loggingEvent.RenderedMessage + Environment.NewLine;
            }

            if (!TextBox.InvokeRequired)
            {
                printf(str);
            }
            else
            {
                this.TextBox.BeginInvoke((MethodInvoker)delegate {
                    if (!this.TextBox.IsHandleCreated)
                        return;
                    if (!this.TextBox.IsDisposed)
                        return;
                    printf(str);
                });
            }
        }

        private void printf(string str)
        {
            if (TextBox.Lines.Length > 100)
            {
                TextBox.Clear();
            }
            TextBox.AppendText(str);
        }
    }
}
