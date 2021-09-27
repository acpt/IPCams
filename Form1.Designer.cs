using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LibVLCSharp.Shared;
using Tools;
using System.Threading;

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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addCamToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.remCamToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editCamToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.moveUpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveDownToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.muteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 621F));
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Size = new System.Drawing.Size(621, 355);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addCamToolStripMenuItem,
            this.remCamToolStripMenuItem,
            this.editCamToolStripMenuItem,
            this.toolStripSeparator1,
            this.moveUpToolStripMenuItem,
            this.moveDownToolStripMenuItem,
            this.toolStripSeparator2,
            this.muteToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(139, 148);
            // 
            // addCamToolStripMenuItem
            // 
            this.addCamToolStripMenuItem.Name = "addCamToolStripMenuItem";
            this.addCamToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.addCamToolStripMenuItem.Text = "Add Cam";
            // 
            // remCamToolStripMenuItem
            // 
            this.remCamToolStripMenuItem.Name = "remCamToolStripMenuItem";
            this.remCamToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.remCamToolStripMenuItem.Text = "Del Cam";
            // 
            // editCamToolStripMenuItem
            // 
            this.editCamToolStripMenuItem.Name = "editCamToolStripMenuItem";
            this.editCamToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.editCamToolStripMenuItem.Text = "Edit Cam";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(135, 6);
            // 
            // moveUpToolStripMenuItem
            // 
            this.moveUpToolStripMenuItem.Name = "moveUpToolStripMenuItem";
            this.moveUpToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.moveUpToolStripMenuItem.Text = "Move Up";
            // 
            // moveDownToolStripMenuItem
            // 
            this.moveDownToolStripMenuItem.Name = "moveDownToolStripMenuItem";
            this.moveDownToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.moveDownToolStripMenuItem.Text = "Move Down";
            // 
            // muteToolStripMenuItem
            // 
            this.muteToolStripMenuItem.Name = "muteToolStripMenuItem";
            this.muteToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.muteToolStripMenuItem.Text = "Mute/UnMute";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(135, 6);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.BackColor = System.Drawing.SystemColors.GrayText;
            this.ClientSize = new System.Drawing.Size(621, 355);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "Form1";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Text = "AC Viewer";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.contextMenuStrip1.ResumeLayout(false);
            this.contextMenuStrip1.ItemClicked += new ToolStripItemClickedEventHandler(ContextMenuStrip_click);
            this.ResumeLayout(false);

            //this.ContextMenuStrip = contextMenuStrip1;
        }
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;

        #endregion





        int current_c = 0;
        int current_r = 0;
        int current_cams = 0;

        public void DoGrid(bool force=false) {
            //load params

            //cria janelas carregadas, tamanho e local 

            //calcula com 16*9 divisao do rectangulo actual
            double h = this.Width / (this.Height * (16F / 9F));
            double v = this.Height / (this.Width * (9F / 16F));

            Console.WriteLine("h:" + h.ToString("n2")+ " v:" + v.ToString("n2"));
            int c = (int)(Math.Sqrt(cam.Count) + .999); int r = (int)(Math.Sqrt(cam.Count) + .999);
            if (h > v + 1) { 
                c = (int)(Math.Sqrt(cam.Count) + .999) + (int)Math.Truncate(h - v);
                r = (int)(Math.Truncate(cam.Count / c +.999));
                if (r==0) r = 1;
            }
            if (v > h + 1) {
                r = (int)(Math.Sqrt(cam.Count) + .999) + (int)Math.Truncate(v - h);
                if (r > cam.Count) { r = cam.Count;}                
                c = (int)(Math.Truncate(cam.Count / r + .999));
                if (c == 0) c = 1;
            }
            if (c * r - cam.Count >= c) { r = r - 1; }

            Console.WriteLine ("DoGrid " + r +","+ c);

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
                    if (cam[i].MediaPlayer == null ) {
                        cam[i].init(GrokCmd(cam[i].Text));
                    }
                    tableLayoutPanel1.Controls.Add(cam[i], wc, wr);
                    wc++;
                    if (wc >= c) {
                        wc = 0; wr++;
                    }
                }

            }
            current_r = r;
            current_c = c;

            ResumeLayout(true);
        }
    

        public void LoadCams() {
            //if (inicializado) 
            Core.Initialize();

            //cam = new Janela[ipcams.Count];
            cam = new List<Janela>();
            //panel = new Panel[ipcams.Length];
            for (int i = 0; i< Loadipcams.Count; i++) {                
                cam.Add(new Janela());
                JanelaDefaults(cam[i], GrokCmd(Loadipcams[i]));
                //((System.ComponentModel.ISupportInitialize)(cam[i]).EndInit()));


                //panel[i] = new Panel();
                ////panel[i].Controls.Add(cam[i]);
                //panel[i].Dock = DockStyle.Fill;
                //panel[i].BackColor = Color.Transparent;                
                //panel[i].ContextMenuStrip = contextMenuStrip1;

                //cam[i].Controls.Add(panel[i]);
                //panel[i].BringToFront();
            }

            for (int i = 0; i< Loadipcams.Count; i++) {
                cam[i].init(GrokCmd(Loadipcams[i]));
            }
    
        }

        public void JanelaDefaults(Janela j, string url) {

            j.Location = new System.Drawing.Point(0, 0);
            //cam[i].Name = "videoView " + i;
            j.BackColor = System.Drawing.Color.Blue;
            j.Size = new System.Drawing.Size(100, 100);
            j.Dock = DockStyle.Fill;
            j.TabIndex = 0;
            j.Click += new System.EventHandler(this.videoView_Click);
            j.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.videoView_MouseWheel);

            j.ContextMenuStrip = contextMenuStrip1;
            j.Text = GrokCmd(url);

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
    }
}

