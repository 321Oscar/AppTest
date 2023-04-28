using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppTest.View.UC
{
    public partial class TransparentLayerToolStripLabel : ToolStripLabel
    {
        private bool _isTransparentLayerVisible = false;

        public bool IsTransparentLayerVisible { get { return _isTransparentLayerVisible; } set { _isTransparentLayerVisible = value; this.Invalidate(); } }

        public TransparentLayerToolStripLabel()
        {
            InitializeComponent();
            this.Click += new EventHandler(OnLabelClick);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (_isTransparentLayerVisible)
            {
                using (Brush brush = new SolidBrush(Color.FromArgb(128, Color.White)))
                {
                    e.Graphics.FillRectangle(brush, this.ContentRectangle);
                }
            }
        }

        private void OnLabelClick(object sender, EventArgs e)
        {
            _isTransparentLayerVisible = !_isTransparentLayerVisible;
            this.Invalidate(); // 重绘
        }
    }
}
