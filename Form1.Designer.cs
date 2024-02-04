using System;
using System.Collections.Generic;
//using System.Collections.Generic;
//using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using System.Windows.Forms;
using LibVLCSharp.Shared;
//using Tools;
//using System.Threading;

namespace IPCams {
    partial class Form1 {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.restartToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.addCamToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.remCamToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editCamToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.moveUpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveDownToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.muteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.Black;
            this.tableLayoutPanel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 782F));
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.ForeColor = System.Drawing.Color.Black;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Size = new System.Drawing.Size(782, 404);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.restartToolStripMenuItem,
            this.toolStripMenuItem1,
            this.toolStripSeparator3,
            this.addCamToolStripMenuItem,
            this.remCamToolStripMenuItem,
            this.editCamToolStripMenuItem,
            this.toolStripSeparator1,
            this.moveUpToolStripMenuItem,
            this.moveDownToolStripMenuItem,
            this.toolStripSeparator2,
            this.muteToolStripMenuItem,
            this.toolStripSeparator4});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(151, 204);
            this.contextMenuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.ContextMenuStrip_click);
            // 
            // restartToolStripMenuItem
            // 
            this.restartToolStripMenuItem.Name = "restartToolStripMenuItem";
            this.restartToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.restartToolStripMenuItem.Text = "Restart";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(150, 22);
            this.toolStripMenuItem1.Text = "Off";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(147, 6);
            // 
            // addCamToolStripMenuItem
            // 
            this.addCamToolStripMenuItem.Name = "addCamToolStripMenuItem";
            this.addCamToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.addCamToolStripMenuItem.Text = "Add Cam";
            // 
            // remCamToolStripMenuItem
            // 
            this.remCamToolStripMenuItem.Name = "remCamToolStripMenuItem";
            this.remCamToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.remCamToolStripMenuItem.Text = "Del Cam";
            // 
            // editCamToolStripMenuItem
            // 
            this.editCamToolStripMenuItem.Name = "editCamToolStripMenuItem";
            this.editCamToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.editCamToolStripMenuItem.Text = "Edit Cam";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(147, 6);
            // 
            // moveUpToolStripMenuItem
            // 
            this.moveUpToolStripMenuItem.Name = "moveUpToolStripMenuItem";
            this.moveUpToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.moveUpToolStripMenuItem.Text = "Move Up";
            // 
            // moveDownToolStripMenuItem
            // 
            this.moveDownToolStripMenuItem.Name = "moveDownToolStripMenuItem";
            this.moveDownToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.moveDownToolStripMenuItem.Text = "Move Down";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(147, 6);
            // 
            // muteToolStripMenuItem
            // 
            this.muteToolStripMenuItem.Name = "muteToolStripMenuItem";
            this.muteToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.muteToolStripMenuItem.Text = "Mute/UnMute";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(147, 6);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.BackColor = System.Drawing.SystemColors.GrayText;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(782, 404);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_Closing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;

        #endregion


        int current_c = 0;
        int current_r = 0;
        //int current_cams = 0;

        public void DoGrid(bool force=false) {
            int CamCount = 0;
            foreach (Janela j in cam) {
                if (j.On) CamCount++;
            }

            //load params

            //cria janelas carregadas, tamanho e local 

            //calcula com 16*9 divisao do rectangulo actual
            int dv = (int)(this.Width / this.Height);
            int dh = (int)(this.Height * (16f / 9f) / this.Width);

            
            int c = (int)(Math.Sqrt(CamCount) + .999);      
            int r = (int)((double)CamCount / c + .999);

            if (dh > 1) {
                //balanca pelas diferencas 
                c = (int)(Math.Sqrt((double)CamCount / dh) + .999);
                r = (int)((double)CamCount / c + .999);
            }
            else
                if (dv > 1) {
                    r = (int)(Math.Sqrt((double)CamCount / dv) + .999);
                    c = (int)((double)CamCount / r + .999);
            }

            //Console.WriteLine ("DoGrid " + r +","+ c);

            if ((current_r != r && current_c != c) || force ){
                tableLayoutPanel1.Controls.Clear();
                tableLayoutPanel1.ColumnStyles.Clear();
                tableLayoutPanel1.RowStyles.Clear();

                tableLayoutPanel1.ColumnCount = c;
                tableLayoutPanel1.RowCount = r;

                tableLayoutPanel1.BackColor = SystemColors.Desktop;
                tableLayoutPanel1.Name = "Painel";
                //add rows and 
                float pc = 100 / c ;
                for (int i = 0; i < c; i++) {
                    tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, pc));
                }
                float pr = 100 / r;
                for (int i = 0; i < r; i++) {
                    tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, pr));
                }
                //tableLayoutPanel1.Size = new System.Drawing.Size(621, 355);
                tableLayoutPanel1.TabIndex = 1;
                tableLayoutPanel1.Dock = DockStyle.Fill;

                //fill cams
                int wc = 0; int wr = 0;
                for (int i = 0; i < cam.Count; i++) {
                    cam[i].Name = "videoView " + i; //name here case of moves - number is used to get position
                    if (cam[i].On) {
                        if (force) {
                            if (cam[i].MediaPlayer != null) {
                                cam[i].MediaPlayer.Dispose();
                                cam[i].MediaPlayer = null;
                            }
                        }
                        if (cam[i].MediaPlayer == null ) {
                            cam[i].init(GrokCmd(cam[i].Text));
                            //cam[i].MediaPlayer.Title = i;
                            //cam[i].MediaPlayer.EncounteredError += delegate(object sender, EventArgs e) { MP_Error(sender, e, i); };
                        }
                        tableLayoutPanel1.Controls.Add(cam[i], wc, wr);
                        wc++;
                        if (wc >= c) {
                            wc = 0; wr++;
                        }
                    }
                }

            }
            current_r = r;
            current_c = c;

            ResumeLayout(true);
        }
    

        public void LoadCams() {
            Core.Initialize();

            for (int i = 0; i< Loadipcams.Count; i++) {                
                cam.Add(new Janela());
                JanelaDefaults(cam[i], Loadipcams[i]);
            }
        }

        public void JanelaDefaults(Janela j, string url) {

            j.Location = new System.Drawing.Point(0, 0);
            //cam[i].Name = "videoView " + i;
            j.BackColor = System.Drawing.Color.Blue;
            j.Size = new System.Drawing.Size(100, 100);
            j.Dock = DockStyle.Fill;
            j.TabIndex = 0;
            //j.Click += new System.EventHandler(this.videoView_Click);
            j.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.videoView_MouseWheel);
            j.MouseClick += new System.Windows.Forms.MouseEventHandler(this.videoView_MouseClick);
            j.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.videoView_MouseClick);
            j.MouseDown += new System.Windows.Forms.MouseEventHandler(this.videoView_MouseDown);
            j.MouseUp += new System.Windows.Forms.MouseEventHandler(this.videoView_MouseUp);
            j.MouseMove += new System.Windows.Forms.MouseEventHandler(this.videoView_MouseMove);

            j.ContextMenuStrip = contextMenuStrip1;
            int x = url.IndexOf(",");
            if (x == -1) {
                j.Text = url;
                j.On = true;
            }
            else {
                j.Text = url.Substring(0,x);
                j.On = url.Substring(x+1)=="0"?false:true;
            }

        }

        private void OpeningMenu(object sender, EventArgs e) {
            throw new NotImplementedException();
        }

        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem addCamToolStripMenuItem;
        private ToolStripMenuItem remCamToolStripMenuItem;
        private ToolStripMenuItem editCamToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem moveUpToolStripMenuItem;
        private ToolStripMenuItem moveDownToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripMenuItem muteToolStripMenuItem;
        private ToolStripMenuItem restartToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripSeparator toolStripSeparator4;
        private List<ToolStripMenuItem> camToolStripMenuItem =  new List<ToolStripMenuItem>();
        private ToolStripMenuItem toolStripMenuItem1;
    }
}

