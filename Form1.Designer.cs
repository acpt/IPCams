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
using IPTools;
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.button1 = new System.Windows.Forms.Button();
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
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(0, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(24, 22);
            this.button1.TabIndex = 4;
            this.button1.Text = "+";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.BackColor = System.Drawing.SystemColors.GrayText;
            this.ClientSize = new System.Drawing.Size(621, 355);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "Form1";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Text = "AC Viewer";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.ResumeLayout(false);

        }

        #endregion
        int current_c = 0;
        int current_r = 0;

        public void AddGrid() {
            Core.Initialize();
            //load params

            //cria janelas carregadas, tamanho e local 

            //calcula com 16*9 divisao do rectangulo actual
            double h = this.Width / this.Height * (16 / 9);
            double v = this.Height / this.Width * (9 / 16);
            int c = (int)(Math.Sqrt(ipcams.Length) + .999); int r = (int)(Math.Sqrt(ipcams.Length) + .999);
            if (h > v+1) { 
                c = (int)(Math.Sqrt(ipcams.Length) + .999) + (int)Math.Truncate(h - v);
                r = (int)(Math.Truncate(ipcams.Length / c +.999));
            }
            else if (v > h+1) {
                r = (int)(Math.Sqrt(ipcams.Length) + .999) + (int)Math.Truncate(v - h);
                c = (int)(Math.Truncate(ipcams.Length / r + .999));
            }

            //c = (int)(Math.Sqrt(ipcams.Length) + .999);
            //int ct = (int)Math.Sqrt(ipcams.Length);
            //r = (int)(ct + (ipcams.Length > c * ct ? 1 : 0));


            if (current_r != r && current_c != c) {
                //clean
                for (int ii = 0; ii < current_r; ii++) {
                    for (int i = 0; i < current_c; i++) {
                        var control = tableLayoutPanel1.GetControlFromPosition(i, ii);
                        tableLayoutPanel1.Controls.Remove(control);
                    }
                }


                tableLayoutPanel1.ColumnCount = c;
                tableLayoutPanel1.RowCount = r;

                tableLayoutPanel1.BackColor = SystemColors.Desktop;
                tableLayoutPanel1.Location = new Point(0, 0);
                tableLayoutPanel1.Name = "Painel";

                for (int i = 0; i < c; i++) {
                    tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, (float)(100 / (r+1))));
                }
                tableLayoutPanel1.ColumnStyles.Clear();
                for (int i = 0; i < r; i++) {
                    tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, (float)(100 / (c+1))));
                }
                tableLayoutPanel1.Size = new System.Drawing.Size(621, 355);
                tableLayoutPanel1.TabIndex = 1;
                tableLayoutPanel1.Dock = DockStyle.Fill;
                int wc = 0; int wr = 0;

                cam = new Janela[ipcams.Length];
                for (int i = 0; i < ipcams.Length; i++) {
                    cam[i] = new Janela();
                    cam[i].Location = new System.Drawing.Point(0, 0);
                    cam[i].Name = "videoView_" + i;
                    cam[i].BackColor = System.Drawing.Color.Blue;
                    cam[i].Size = new System.Drawing.Size(100, 100);
                    cam[i].Dock = DockStyle.Fill;
                    cam[i].TabIndex = 0;
                    cam[i].Click += new System.EventHandler(this.videoView_Click);
                    tableLayoutPanel1.Controls.Add(cam[i], wc, wr);
                    Console.WriteLine("c:" + wc + " r:" + wr);

                    ((System.ComponentModel.ISupportInitialize)(cam[i])).EndInit();

                    wc++;
                    if (wc >= c) {
                        wc = 0; wr++;
                    }
                }

                for (int i = 0; i < ipcams.Length; i++) {
                    cam[i].init(GrokCmd(ipcams[i]));
                }
            }
            current_r =r;
            current_c=c;

            ResumeLayout(true);
        }
        //private LibVLCSharp.WinForms.VideoView videoView1;
        //private Janela videoView2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}

