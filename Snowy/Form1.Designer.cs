namespace Snowy
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.tmrDrag = new System.Windows.Forms.Timer(this.components);
            this.rightClickMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.退出ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.衣柜ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.圣诞帽子ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.樱花帽子ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.水手帽ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.风车帽子ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.圣诞衣服ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.和服ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.动作ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.捶打ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.眨眼ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.操作ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.文件系统ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.查找ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.重新整理ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.邮件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.添加邮箱配置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.修改邮箱配置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.下载ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tmrBlink = new System.Windows.Forms.Timer(this.components);
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.tmrHit = new System.Windows.Forms.Timer(this.components);
            this.tmrSong = new System.Windows.Forms.Timer(this.components);
            this.tmrEat = new System.Windows.Forms.Timer(this.components);
            this.端口监听ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rightClickMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // tmrDrag
            // 
            this.tmrDrag.Tick += new System.EventHandler(this.tmrDrag_Tick);
            // 
            // rightClickMenu
            // 
            this.rightClickMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.退出ToolStripMenuItem,
            this.设置ToolStripMenuItem,
            this.衣柜ToolStripMenuItem,
            this.动作ToolStripMenuItem,
            this.操作ToolStripMenuItem});
            this.rightClickMenu.Name = "rightClickMenu";
            this.rightClickMenu.Size = new System.Drawing.Size(181, 136);
            // 
            // 退出ToolStripMenuItem
            // 
            this.退出ToolStripMenuItem.Name = "退出ToolStripMenuItem";
            this.退出ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.退出ToolStripMenuItem.Text = "退出";
            this.退出ToolStripMenuItem.Click += new System.EventHandler(this.退出ToolStripMenuItem_Click);
            // 
            // 设置ToolStripMenuItem
            // 
            this.设置ToolStripMenuItem.Name = "设置ToolStripMenuItem";
            this.设置ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.设置ToolStripMenuItem.Text = "设置";
            this.设置ToolStripMenuItem.Click += new System.EventHandler(this.设置ToolStripMenuItem_Click);
            // 
            // 衣柜ToolStripMenuItem
            // 
            this.衣柜ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.圣诞帽子ToolStripMenuItem,
            this.樱花帽子ToolStripMenuItem,
            this.水手帽ToolStripMenuItem,
            this.风车帽子ToolStripMenuItem,
            this.圣诞衣服ToolStripMenuItem,
            this.和服ToolStripMenuItem});
            this.衣柜ToolStripMenuItem.Name = "衣柜ToolStripMenuItem";
            this.衣柜ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.衣柜ToolStripMenuItem.Text = "衣柜";
            // 
            // 圣诞帽子ToolStripMenuItem
            // 
            this.圣诞帽子ToolStripMenuItem.Name = "圣诞帽子ToolStripMenuItem";
            this.圣诞帽子ToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.圣诞帽子ToolStripMenuItem.Text = "帽子 — 圣诞";
            this.圣诞帽子ToolStripMenuItem.Click += new System.EventHandler(this.帽子ToolStripMenuItem_Click);
            // 
            // 樱花帽子ToolStripMenuItem
            // 
            this.樱花帽子ToolStripMenuItem.Name = "樱花帽子ToolStripMenuItem";
            this.樱花帽子ToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.樱花帽子ToolStripMenuItem.Text = "帽子 — 樱花";
            this.樱花帽子ToolStripMenuItem.Click += new System.EventHandler(this.帽子ToolStripMenuItem_Click);
            // 
            // 水手帽ToolStripMenuItem
            // 
            this.水手帽ToolStripMenuItem.Name = "水手帽ToolStripMenuItem";
            this.水手帽ToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.水手帽ToolStripMenuItem.Text = "帽子 — 水手帽";
            this.水手帽ToolStripMenuItem.Click += new System.EventHandler(this.帽子ToolStripMenuItem_Click);
            // 
            // 风车帽子ToolStripMenuItem
            // 
            this.风车帽子ToolStripMenuItem.Name = "风车帽子ToolStripMenuItem";
            this.风车帽子ToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.风车帽子ToolStripMenuItem.Text = "帽子 — 风车";
            this.风车帽子ToolStripMenuItem.Click += new System.EventHandler(this.帽子ToolStripMenuItem_Click);
            // 
            // 圣诞衣服ToolStripMenuItem
            // 
            this.圣诞衣服ToolStripMenuItem.Name = "圣诞衣服ToolStripMenuItem";
            this.圣诞衣服ToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.圣诞衣服ToolStripMenuItem.Text = "衣服 — 圣诞";
            this.圣诞衣服ToolStripMenuItem.Click += new System.EventHandler(this.衣服ToolStripMenuItem_Click);
            // 
            // 和服ToolStripMenuItem
            // 
            this.和服ToolStripMenuItem.Name = "和服ToolStripMenuItem";
            this.和服ToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.和服ToolStripMenuItem.Text = "衣服 — 和服";
            this.和服ToolStripMenuItem.Click += new System.EventHandler(this.衣服ToolStripMenuItem_Click);
            // 
            // 动作ToolStripMenuItem
            // 
            this.动作ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.捶打ToolStripMenuItem1,
            this.眨眼ToolStripMenuItem1});
            this.动作ToolStripMenuItem.Name = "动作ToolStripMenuItem";
            this.动作ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.动作ToolStripMenuItem.Text = "动作";
            // 
            // 捶打ToolStripMenuItem1
            // 
            this.捶打ToolStripMenuItem1.Name = "捶打ToolStripMenuItem1";
            this.捶打ToolStripMenuItem1.Size = new System.Drawing.Size(100, 22);
            this.捶打ToolStripMenuItem1.Text = "捶打";
            this.捶打ToolStripMenuItem1.Click += new System.EventHandler(this.捶打ToolStripMenuItem1_Click);
            // 
            // 眨眼ToolStripMenuItem1
            // 
            this.眨眼ToolStripMenuItem1.Name = "眨眼ToolStripMenuItem1";
            this.眨眼ToolStripMenuItem1.Size = new System.Drawing.Size(100, 22);
            this.眨眼ToolStripMenuItem1.Text = "眨眼";
            this.眨眼ToolStripMenuItem1.Click += new System.EventHandler(this.眨眼ToolStripMenuItem1_Click);
            // 
            // 操作ToolStripMenuItem
            // 
            this.操作ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.文件系统ToolStripMenuItem,
            this.邮件ToolStripMenuItem,
            this.下载ToolStripMenuItem,
            this.端口监听ToolStripMenuItem});
            this.操作ToolStripMenuItem.Name = "操作ToolStripMenuItem";
            this.操作ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.操作ToolStripMenuItem.Text = "操作";
            // 
            // 文件系统ToolStripMenuItem
            // 
            this.文件系统ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.查找ToolStripMenuItem,
            this.重新整理ToolStripMenuItem});
            this.文件系统ToolStripMenuItem.Name = "文件系统ToolStripMenuItem";
            this.文件系统ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.文件系统ToolStripMenuItem.Text = "文件系统";
            // 
            // 查找ToolStripMenuItem
            // 
            this.查找ToolStripMenuItem.Name = "查找ToolStripMenuItem";
            this.查找ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.查找ToolStripMenuItem.Text = "查找";
            this.查找ToolStripMenuItem.Click += new System.EventHandler(this.查找ToolStripMenuItem_Click);
            // 
            // 重新整理ToolStripMenuItem
            // 
            this.重新整理ToolStripMenuItem.Name = "重新整理ToolStripMenuItem";
            this.重新整理ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.重新整理ToolStripMenuItem.Text = "重新整理";
            this.重新整理ToolStripMenuItem.Click += new System.EventHandler(this.重新整理ToolStripMenuItem_Click);
            // 
            // 邮件ToolStripMenuItem
            // 
            this.邮件ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.添加邮箱配置ToolStripMenuItem,
            this.修改邮箱配置ToolStripMenuItem});
            this.邮件ToolStripMenuItem.Name = "邮件ToolStripMenuItem";
            this.邮件ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.邮件ToolStripMenuItem.Text = "邮件";
            // 
            // 添加邮箱配置ToolStripMenuItem
            // 
            this.添加邮箱配置ToolStripMenuItem.Name = "添加邮箱配置ToolStripMenuItem";
            this.添加邮箱配置ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.添加邮箱配置ToolStripMenuItem.Text = "添加邮箱配置";
            this.添加邮箱配置ToolStripMenuItem.Click += new System.EventHandler(this.添加邮箱配置ToolStripMenuItem_Click);
            // 
            // 修改邮箱配置ToolStripMenuItem
            // 
            this.修改邮箱配置ToolStripMenuItem.Name = "修改邮箱配置ToolStripMenuItem";
            this.修改邮箱配置ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.修改邮箱配置ToolStripMenuItem.Text = "修改邮箱配置";
            this.修改邮箱配置ToolStripMenuItem.Click += new System.EventHandler(this.修改邮箱配置ToolStripMenuItem_Click);
            // 
            // 下载ToolStripMenuItem
            // 
            this.下载ToolStripMenuItem.Name = "下载ToolStripMenuItem";
            this.下载ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.下载ToolStripMenuItem.Text = "下载管理器";
            this.下载ToolStripMenuItem.Click += new System.EventHandler(this.下载ToolStripMenuItem_Click);
            // 
            // tmrBlink
            // 
            this.tmrBlink.Tick += new System.EventHandler(this.tmrBlink_Tick);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.rightClickMenu;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "notifyIcon1";
            this.notifyIcon1.Visible = true;
            // 
            // tmrHit
            // 
            this.tmrHit.Tick += new System.EventHandler(this.TmrHit_Tick);
            // 
            // tmrSong
            // 
            this.tmrSong.Tick += new System.EventHandler(this.TmrSong_Tick);
            // 
            // tmrEat
            // 
            this.tmrEat.Tick += new System.EventHandler(this.TmrEat_Tick);
            // 
            // 端口监听ToolStripMenuItem
            // 
            this.端口监听ToolStripMenuItem.Name = "端口监听ToolStripMenuItem";
            this.端口监听ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.端口监听ToolStripMenuItem.Text = "端口监听";
            this.端口监听ToolStripMenuItem.Click += new System.EventHandler(this.端口监听ToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(266, 242);
            this.ContextMenuStrip = this.rightClickMenu;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.Form1_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.Form1_DragEnter);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseUp);
            this.rightClickMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer tmrDrag;
        private System.Windows.Forms.ContextMenuStrip rightClickMenu;
        private System.Windows.Forms.ToolStripMenuItem 退出ToolStripMenuItem;
        private System.Windows.Forms.Timer tmrBlink;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ToolStripMenuItem 衣柜ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 圣诞帽子ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 圣诞衣服ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 樱花帽子ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 水手帽ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 和服ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 风车帽子ToolStripMenuItem;
        private System.Windows.Forms.Timer tmrHit;
        private System.Windows.Forms.Timer tmrSong;
        private System.Windows.Forms.ToolStripMenuItem 设置ToolStripMenuItem;
        private System.Windows.Forms.Timer tmrEat;
        private System.Windows.Forms.ToolStripMenuItem 动作ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 捶打ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 眨眼ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 操作ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 文件系统ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 查找ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 重新整理ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 邮件ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 添加邮箱配置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 修改邮箱配置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 下载ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 端口监听ToolStripMenuItem;
    }
}

