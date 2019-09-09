using System;
using System.Drawing;
using System.Windows.Forms;

namespace Snowy
{
    public partial class ListViewEx : System.Windows.Forms.ListView
    {
        public ListViewEx()
        {
            InitializeComponent();
        }
        //C# listview进度条显示
        private Color mProgressColor = Color.Red;
        public Color ProgressColor
        {
            get
            {
                return this.mProgressColor;
            }
            set
            {
                this.mProgressColor = value;
            }
        }
        private Color mProgressTextColor = Color.Black;
        public Color ProgressTextColor
        {
            get
            {
                return mProgressTextColor;
            }
            set
            {
                mProgressTextColor = value;
            }
        }
        public int ProgressColumIndex
        {
            set
            {
                progressIndex = value;
            }
            get
            {
                return progressIndex;
            }
        }
        int progressIndex = -1;
        const string numberstring = "0123456789.";
        private bool CheckIsFloat(String s)
        {
            //C# listview进度条显示
            foreach (char c in s)
            {
                if (numberstring.IndexOf(c) > -1)
                { continue; }
                else return false;
            }
            return true;
        }
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
        //C# listview进度条显示
        private void InitializeComponent()
        {
            this.OwnerDraw = true;
            this.View = View.Details;
        }
        protected override void OnDrawColumnHeader(DrawListViewColumnHeaderEventArgs e)
        {
            e.DrawDefault = true;
            base.OnDrawColumnHeader(e);
        }
        protected override void OnDrawSubItem(DrawListViewSubItemEventArgs e)
        {
            if (e.ColumnIndex != this.progressIndex)
            {
                e.DrawDefault = true; base.OnDrawSubItem(e);
            }
            else
            {
                if (CheckIsFloat(e.Item.SubItems[e.ColumnIndex].Text))
                //判断当前subitem文本是否可以转为浮点数
                {
                    float per = float.Parse(e.Item.SubItems[e.ColumnIndex].Text);
                    if (per >= 1.0f) { per = per / 100.0f; }
                    Rectangle rect = new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
                    DrawProgress(rect, per, e.Graphics);
                }
            }
        }
        //C# listview进度条显示 ///绘制进度条列的subitem 
        private void DrawProgress(Rectangle rect, float percent, Graphics g)
        {
            if (rect.Height > 2 && rect.Width > 2)
            {
                if ((rect.Top > 0 && rect.Top < this.Height) && (rect.Left > this.Left && rect.Left < this.Width))
                {
                    //绘制进度 
                    int width = (int)(rect.Width * percent);
                    Rectangle newRect = new Rectangle(rect.Left + 1, rect.Top + 1, width - 2, rect.Height - 2);
                    using (Brush tmpb = new SolidBrush(this.mProgressColor))
                    { g.FillRectangle(tmpb, newRect); }
                    newRect = new Rectangle(rect.Left + 1, rect.Top + 1, rect.Width - 2, rect.Height - 2);
                    g.DrawRectangle(Pens.RoyalBlue, newRect);
                    StringFormat sf = new StringFormat();
                    sf.Alignment = StringAlignment.Center;
                    sf.LineAlignment = StringAlignment.Center;
                    sf.Trimming = StringTrimming.EllipsisCharacter;
                    newRect = new Rectangle(rect.Left + 1, rect.Top + 1, rect.Width - 2, rect.Height - 2);
                    using (Brush b = new SolidBrush(mProgressTextColor))
                    {
                        g.DrawString(percent.ToString("p1"), this.Font, b, newRect, sf);
                    }
                }
            }
            //C# listview进度条显示
            else
            {
                return;
            }
        }
    }
}
