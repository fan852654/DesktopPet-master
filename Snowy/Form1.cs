﻿using PlayHelper;
using QueryEngine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snowy
{
    public partial class Form1 : Form
    {
        bool haveHandle = true;  //用于SetBits
        bool bFormDragging = false;  // 用于窗体移动
        Point oPointClicked; // 用于窗体移动
        int dragFrame = 0; //拖拽帧数计数
        int blinkFrame = 0; //眨眼帧数计数
        int singFrame = 0; //音乐帧数计数
        int eatFrame = 0; //吃帧数计数
        int hatNum = -1;
        int clothesNum = -1;
        private enum PetStates { General = 0, Drag = 1 };
        private PlayHelper.PlayHelper playhelper { get; set; }
        private EatThread et;
        public static List<FileAndDirectoryEntry> entries;
        public static Dictionary<EmailHelper.EmailConfig,string> emailList;
        public static int AllFileCout = 0;
        public static Task initflush = null;
        private EmailHelper.EmailTask EmailTask;
        Bitmap[] pet = new Bitmap[30];
        Bitmap[] petDrag = new Bitmap[3];
        Bitmap[] petBlink = new Bitmap[2];
        Bitmap[] petHit = new Bitmap[3];
        Bitmap[] petSing = new Bitmap[2];
        Bitmap[] petEat = new Bitmap[3];
        Bitmap[,] petHat = new Bitmap[10, 2];
        Bitmap[,] petClothes = new Bitmap[5, 4];

        Bitmap[] petWithClothes = new Bitmap[30];
        Bitmap[] petDragWithClothes = new Bitmap[3];
        Bitmap[] petBlinkWithClothes = new Bitmap[3];
        Bitmap[] petHitWithClothes = new Bitmap[3];
        Bitmap[] petSingWithClothes = new Bitmap[2];
        Bitmap[] petEatWithClothes = new Bitmap[3];
        public static VoicePlay vplay;
        public static ConfigHelper.ConfigHelper Config { get; set; }
        public Form1()
        {
            InitializeComponent();
            entries = null;
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(SystemInformation.WorkingArea.Width - this.Width, SystemInformation.WorkingArea.Height - this.Height);
        }
        #region 重载

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            base.OnClosing(e);
            haveHandle = false;
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            InitializeStyles();
            base.OnHandleCreated(e);
            haveHandle = true;
        }

        private void InitializeStyles()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.UserPaint, true);
            UpdateStyles();
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cParms = base.CreateParams;
                cParms.ExStyle |= 0x00080000; // WS_EX_LAYERED
                return cParms;
            }
        }

        #endregion
        public void SetBits(Bitmap bitmap)
        {
            if (!haveHandle) return;

            if (!Bitmap.IsCanonicalPixelFormat(bitmap.PixelFormat) || !Bitmap.IsAlphaPixelFormat(bitmap.PixelFormat))
                MessageBox.Show("Error Bitmap");

            IntPtr oldBits = IntPtr.Zero;
            IntPtr screenDC = Win32.GetDC(IntPtr.Zero);
            IntPtr hBitmap = IntPtr.Zero;
            IntPtr memDc = Win32.CreateCompatibleDC(screenDC);

            try
            {
                Win32.Point topLoc = new Win32.Point(Left, Top);
                Win32.Size bitMapSize = new Win32.Size(bitmap.Width, bitmap.Height);
                Win32.BLENDFUNCTION blendFunc = new Win32.BLENDFUNCTION();
                Win32.Point srcLoc = new Win32.Point(0, 0);

                hBitmap = bitmap.GetHbitmap(Color.FromArgb(0));
                oldBits = Win32.SelectObject(memDc, hBitmap);

                blendFunc.BlendOp = Win32.AC_SRC_OVER;
                blendFunc.SourceConstantAlpha = 255;
                blendFunc.AlphaFormat = Win32.AC_SRC_ALPHA;
                blendFunc.BlendFlags = 0;

                Win32.UpdateLayeredWindow(Handle, screenDC, ref topLoc, ref bitMapSize, memDc, ref srcLoc, 0, ref blendFunc, Win32.ULW_ALPHA);
            }
            finally
            {
                if (hBitmap != IntPtr.Zero)
                {
                    Win32.SelectObject(memDc, oldBits);
                    Win32.DeleteObject(hBitmap);
                }
                Win32.ReleaseDC(IntPtr.Zero, screenDC);
                Win32.DeleteDC(memDc);
            }
        }
        private Bitmap CombinedPic(Bitmap bottom, Bitmap top, int x, int y)
        {
            Bitmap bitmap = new Bitmap(bottom.Width, bottom.Height);
            Graphics g = Graphics.FromImage(bitmap);
            g.DrawImage(bottom, new Rectangle(0, 0, bottom.Width, bottom.Height), new Rectangle(0, 0, bottom.Width, bottom.Height), GraphicsUnit.Pixel);
            g.DrawImage(top, new Rectangle(x, y, top.Width, top.Height), new Rectangle(0, 0, top.Width, top.Height), GraphicsUnit.Pixel);
            return bitmap;
        }
        private Bitmap Dress(Bitmap img, int state)
        {
            Bitmap bitmap = new Bitmap(img.Width, img.Height);
            bitmap = img;
            if (clothesNum != -1)
            {
                bitmap = CombinedPic(bitmap, petClothes[clothesNum, state], 0, 0);
            }
            if (hatNum != -1)
            {
                bitmap = CombinedPic(bitmap, petHat[hatNum, state], 0, 0);
            }
            return bitmap;
        }
        private void InitOrFlushFileEngine()
        {
            if(entries != null)
            {
                entries.Clear();
                GC.Collect();
            }
            entries = Engine.GetAllFilesAndDirectories();
            AllFileCout = entries.Count();
            return;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            emailList = new Dictionary<EmailHelper.EmailConfig, string>();
            initflush = new Task(InitOrFlushFileEngine);
            initflush.Start();
            pet[0] = new Bitmap(Application.StartupPath + "\\shell\\surface0000.png");
            pet[1] = new Bitmap(Application.StartupPath + "\\shell\\surface0001.png");
            pet[2] = new Bitmap(Application.StartupPath + "\\shell\\surface0002.png");
            pet[3] = new Bitmap(Application.StartupPath + "\\shell\\surface0003.png");
            pet[4] = new Bitmap(Application.StartupPath + "\\shell\\surface0004.png");
            pet[5] = new Bitmap(Application.StartupPath + "\\shell\\surface0005.png");
            pet[6] = new Bitmap(Application.StartupPath + "\\shell\\surface0006.png");
            pet[7] = new Bitmap(Application.StartupPath + "\\shell\\surface0007.png");
            pet[8] = new Bitmap(Application.StartupPath + "\\shell\\surface0008.png");
            pet[9] = new Bitmap(Application.StartupPath + "\\shell\\surface0009.png");
            petDrag[0] = new Bitmap(Application.StartupPath + "\\shell\\surface0091.png");
            petDrag[1] = new Bitmap(Application.StartupPath + "\\shell\\surface0092.png");
            petDrag[2] = new Bitmap(Application.StartupPath + "\\shell\\surface0093.png");
            petBlink[0] = new Bitmap(Application.StartupPath + "\\shell\\surface1003.png");
            petBlink[1] = new Bitmap(Application.StartupPath + "\\shell\\surface1004.png");
            petHit[0] = new Bitmap(Application.StartupPath + "\\shell\\surface0002.png");
            petHit[1] = new Bitmap(Application.StartupPath + "\\shell\\surface0001.png");
            petHit[2] = new Bitmap(Application.StartupPath + "\\shell\\surface0004.png");
            petEat[0] = new Bitmap(Application.StartupPath + "\\shell\\surface0033.png");
            petEat[1] = new Bitmap(Application.StartupPath + "\\shell\\surface0034.png");
            petEat[2] = new Bitmap(Application.StartupPath + "\\shell\\surface0035.png");
            petHat[0, 0] = new Bitmap(Application.StartupPath + "\\shell\\surface3000.png");
            petHat[0, 1] = new Bitmap(Application.StartupPath + "\\shell\\surface3001.png");
            petHat[1, 0] = new Bitmap(Application.StartupPath + "\\shell\\surface3002.png");
            petHat[1, 1] = new Bitmap(Application.StartupPath + "\\shell\\surface3003.png");
            petHat[2, 0] = new Bitmap(Application.StartupPath + "\\shell\\surface3004.png");
            petHat[2, 1] = new Bitmap(Application.StartupPath + "\\shell\\surface3005.png");
            petHat[3, 0] = new Bitmap(Application.StartupPath + "\\shell\\surface3006.png");
            petHat[3, 1] = new Bitmap(Application.StartupPath + "\\shell\\surface3007.png");
            petClothes[0, 0] = new Bitmap(Application.StartupPath + "\\shell\\surface3100.png");
            petClothes[0, 1] = new Bitmap(Application.StartupPath + "\\shell\\surface3101.png");
            petClothes[0, 2] = new Bitmap(Application.StartupPath + "\\shell\\surface3102.png");
            petClothes[0, 3] = new Bitmap(Application.StartupPath + "\\shell\\surface3103.png");
            petClothes[1, 0] = new Bitmap(Application.StartupPath + "\\shell\\surface3200.png");
            petClothes[1, 1] = new Bitmap(Application.StartupPath + "\\shell\\surface3201.png");
            petClothes[1, 2] = new Bitmap(Application.StartupPath + "\\shell\\surface3202.png");
            petClothes[1, 3] = new Bitmap(Application.StartupPath + "\\shell\\surface3203.png");
            petSing[0] = new Bitmap(Application.StartupPath + "\\shell\\surface0025.png");
            petSing[1] = new Bitmap(Application.StartupPath + "\\shell\\surface0026.png");
            DressAll();
            SetBits(petWithClothes[0]);
            Config = new ConfigHelper.ConfigHelper();
            //吃文件线程
            et = new EatThread(Config,EatThing,notifyIcon1);
            et.Start();
            //邮件监控线程
            EmailTask = new EmailHelper.EmailTask(notifyIcon1);
            EmailTask.Start();
            initEmailItems();
            vplay = new VoicePlay();
        }
        #region 初始化邮箱等配置
        public void initEmailItems()
        {
            var emailconfig = EmailHelper.EmailHelper.GetConfigsFormConfig();
            foreach (var item in emailconfig)
            {
                string md = item.GetMD5();
                string name = item.Url + item.UserName;
                ToolStripItem tsi = 邮件ToolStripMenuItem.DropDownItems.Add(name);
                tsi.Name = md;
                tsi.Click += new EventHandler(邮箱AddItemEvent);
                emailList.Add(item, md);
            }
        }
        public void 邮箱AddItemEvent(object sender, EventArgs e)
        {
            EmailHelper.EmailConfig ec = emailList.SingleOrDefault(m => m.Value == ((ToolStripItem)sender).Name).Key;
            EmailMessageResult emailMessageResult = new EmailMessageResult(ec);
            emailMessageResult.Show();
        }
        #endregion
        #region 拖拽

        private void Form1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                bFormDragging = true;
                oPointClicked = new Point(e.X, e.Y);
                tmrDrag.Interval = 110;
                tmrDrag.Enabled = true;
            }
        }

        private void Form1_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                bFormDragging = false;
                tmrDrag.Enabled = false;
                SetBits(petWithClothes[0]);
            }
        }
        private void Form1_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (bFormDragging)
            {
                Point oMoveToPoint = default(Point);
                //以当前鼠标位置为基础，找出目标位置
                oMoveToPoint = PointToScreen(new Point(e.X, e.Y));
                oMoveToPoint.Offset(oPointClicked.X * -1, (oPointClicked.Y + SystemInformation.CaptionHeight + SystemInformation.BorderSize.Height) * -1 + 24);
                Location = oMoveToPoint;
            }
        }
        private void tmrDrag_Tick(object sender, EventArgs e)
        {
            if (dragFrame < 2)
            {
                SetBits(petDragWithClothes[dragFrame]);
                dragFrame += 1;
            }
            else
            {
                SetBits(petDragWithClothes[dragFrame]);
                dragFrame = 0;
            }
        }

        #endregion
        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void 捶打ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tmrHit.Interval = 300;
            tmrHit.Start();
        }
        private void TmrHit_Tick(object sender, EventArgs e)
        {
            if (blinkFrame < 3)
            {
                SetBits(petHitWithClothes[blinkFrame]);
                blinkFrame += 1;
                if(blinkFrame == 2)
                {
                    tmrHit.Interval = 4000;
                    return;
                }
            }
            else
            {
                SetBits(petWithClothes[0]);
                blinkFrame = 0;
                tmrHit.Stop();
            }
        }
        private void 眨眼ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tmrBlink.Interval = 40;
            tmrBlink.Start();
        }
        private void EatThing()
        {
            tmrEat.Interval = 500;
            tmrEat.Start();
        }
        private void TmrEat_Tick(object sender, EventArgs e)
        {
            if (eatFrame < 21)
            {
                SetBits(petEatWithClothes[eatFrame % 3 == 0 ? 0 : eatFrame % 3 == 1 ? 1 : 2]);
                eatFrame += 1;
            }
            else
            {
                SetBits(petWithClothes[0]);
                eatFrame = 0;
                tmrEat.Stop();
            }
        }
        private void tmrBlink_Tick(object sender, EventArgs e)
        {
            if (blinkFrame < 2)
            {
                SetBits(petBlinkWithClothes[blinkFrame]);
                blinkFrame += 1;
            }
            else
            {
                SetBits(petWithClothes[0]);
                blinkFrame = 0;
                tmrBlink.Stop();
            }
        }
        private void 帽子ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool itemChecked = (sender as ToolStripMenuItem).Checked;
            if (itemChecked)
            {
                hatNum = -1;
            }
            else
            {
                hatNum = this.衣柜ToolStripMenuItem.DropDownItems.IndexOf(sender as ToolStripMenuItem);
            }
            this.圣诞帽子ToolStripMenuItem.Checked = false;
            this.樱花帽子ToolStripMenuItem.Checked = false;
            this.水手帽ToolStripMenuItem.Checked = false;
            this.风车帽子ToolStripMenuItem.Checked = false;
            (sender as ToolStripMenuItem).Checked = !itemChecked;
            DressAll();
            SetBits(petWithClothes[0]);
        }
        private void 衣服ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool itemChecked = (sender as ToolStripMenuItem).Checked;
            if (itemChecked)
            {
                clothesNum = -1;
            }
            else
            {
                clothesNum = this.衣柜ToolStripMenuItem.DropDownItems.IndexOf(sender as ToolStripMenuItem) - 4;
            }
            this.圣诞衣服ToolStripMenuItem.Checked = false;
            this.和服ToolStripMenuItem.Checked = false;
            (sender as ToolStripMenuItem).Checked = !itemChecked;
            DressAll();
            SetBits(petWithClothes[0]);
        }
        private void DressAll()
        {
            int i;
            for (i = 0; i < 10; i++)
            {
                petWithClothes[i] = Dress(pet[i], (int)PetStates.General);
            }
            for (i = 0; i < 3; i++)
            {
                petDragWithClothes[i] = Dress(petDrag[i], (int)PetStates.Drag);
            }
            for (i = 0; i < 2; i++)
            {
                petBlinkWithClothes[i] = Dress(petBlink[i], (int)PetStates.General);
            }
            for (i = 0; i < 2; i++)
            {
                petSingWithClothes[i] = Dress(petSing[i], (int)PetStates.General);
            }
            for (i = 0; i < 3; i++)
            {
                petHitWithClothes[i] = Dress(petHit[i], (int)PetStates.General);
            }
            for (i = 0; i < 3; i++)
            {
                petEatWithClothes[i] = Dress(petEat[i], (int)PetStates.General);
            }
        }
        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Link;
            else
                e.Effect = DragDropEffects.None;
        }
        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            tmrSong.Interval = 500;
            tmrSong.Start();
            string filePath = ((string[])e.Data.GetData("FileNameW", true))[0];
            FileInfo fi = new FileInfo(filePath);
            string musicfile = ".mp3,.wma,.avi,.rm,.rmvb,.flv,.mpg,.mov,.mkv,.mp4";
            string zipfile = ".zip";
            if (musicfile.Split(',').Contains(fi.Extension.Trim().ToLower()))
            {
                if (playhelper != null)
                {
                    playhelper.StopAll();
                }
                playhelper = new PlayHelper.PlayHelper(fi);
                playhelper.StartMusic();
            }
            else if (zipfile.Split(',').Contains(fi.Extension.Trim().ToLower()))
            {
                CompressHelper.CompressHelper.UnZip(filePath, string.Empty);
            }
        }
        private void TmrSong_Tick(object sender, EventArgs e)
        {
            if (singFrame < 60)
            {
                SetBits(petSingWithClothes[singFrame % 2 == 0 ? 0 : 1]);
                singFrame++;
            }
            else
            {
                singFrame = 0;
                SetBits(petWithClothes[0]);
                tmrSong.Stop();
            }
        }
        private void 设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeConfig cc = new ChangeConfig();
            cc.Show();
        }
        private void 捶打ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            tmrHit.Interval = 300;
            tmrHit.Start();
        }
        private void 眨眼ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            tmrBlink.Interval = 40;
            tmrBlink.Start();
        }
        private void 查找ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!initflush.IsCompleted)
            {
                MessageBox.Show("正在构建文件缓存，请稍后使用", "Warning", MessageBoxButtons.OK);
            }
            else
            {
                FastFindFile fff = new FastFindFile();
                fff.Show();
            }
        }
        private void 重新整理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            initflush = new Task(InitOrFlushFileEngine);
            initflush.Start();
        }
        private void 添加邮箱配置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddEmail ae = new AddEmail();
            ae.Show();
        }
        private void 修改邮箱配置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EmailConfigResult rcr = new EmailConfigResult();
            rcr.Show();
        }
        private void 下载ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DownloadController dc = new DownloadController(notifyIcon1,Config.GetConfig("DownloadDefaultPath",false).ToString());
            dc.Show();
        }
    }
}
