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

namespace IPCams {
    public partial class Form1 : Form {

        string VIDEO_URL1 = "";
        string VIDEO_URL2 = "";
        LibVLC _libvlc;
        MediaPlayer _mediaPlayer1;
        //MediaPlayer _mediaPlayer2;
        Janela[] cam = null;

        //from args or param file... or added and saved
        string[] ipcams = new string[] { 
            "rtsp://nvrviewer:armgames@[c0-c9-e3-dd-0c-00]:554/stream1",
            "rtsp://nvrviewer:armgames@[28-ee-52-93-33-e0]:554/stream1",
            "rtsp://acpt:armdom@aca1.dyndns.info:554/cam/realmonitor?channel=1&subtype=1" 
        };
        
        public Form1() {
            InitializeComponent();
            Core.Initialize();
            //load params
            
            //cria janelas carregadas, tamanho e local 

            //calcula com 16*9 divisao do rectangulo actual
            int c = (int)(Math.Sqrt(ipcams.Length)+.999);
            int ct = (int)Math.Sqrt(ipcams.Length);
            int r = (int)(ct + (ipcams.Length > c * ct ? 1 : 0));
            
            tableLayoutPanel1.ColumnCount = c;
            tableLayoutPanel1.RowCount = r;
            int wc = 0; int wr = 0;

            cam = new Janela[ipcams.Length];
            for (int i = 0; i < ipcams.Length; i++) {
                if (wc==0) tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(SizeType.Percent, 50F));
                cam[i] = new Janela();
                ((System.ComponentModel.ISupportInitialize)(this.cam[i])).BeginInit();
                cam[i].init(Funcs.GrokCmd(ipcams[i]));
                cam[i].Location = new System.Drawing.Point(0, 0);
                cam[i].Name = "videoView_" + i;
                cam[i].BackColor = System.Drawing.Color.Red;
                cam[i].Size = new System.Drawing.Size(200, 200);
                cam[i].Dock = System.Windows.Forms.DockStyle.Fill;
                cam[i].TabIndex = 0;
                cam[i].Click += new System.EventHandler(this.videoView_Click);

                tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle( SizeType.Percent, 50F));
                tableLayoutPanel1.Controls.Add(cam[i], wc, wr);

                ((System.ComponentModel.ISupportInitialize)(cam[i])).EndInit();

                wc++;
                if (wc>=c) { 
                    wc=0; wr++;
                }
            }
            tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ResumeLayout(false);



            //rtsp://acpt:armdom@aca1.dyndns.info:554/cam/realmonitor?channel=1&subtype=1
            //Sizing(1165,470);
        }

        private void Form1_Load(object sender, EventArgs e) {

            _libvlc = new LibVLC();

            //_mediaPlayer1 = new MediaPlayer(_libvlc);
            //_mediaPlayer2 = new MediaPlayer(_libvlc);
            //videoView1.MediaPlayer = _mediaPlayer1;
            //videoView2.MediaPlayer = _mediaPlayer2;

            //if (VIDEO_URL1 != "") {
            //    _mediaPlayer1.Play(new Media(_libvlc, new Uri(VIDEO_URL1)));
            //    _mediaPlayer1.SetAudioTrack(-1);

            //}
            //if (VIDEO_URL2 != "") {
            //    videoView2.MediaPlayer.Play(new Media(_libvlc, new Uri(VIDEO_URL2)));
            //    //_mediaPlayer2.Play(new Media(_libvlc, new Uri(VIDEO_URL2)));
            //    //_mediaPlayer1.SetAudioTrack(-1);
            //}

        }

    
        private void button1_Click(object sender, EventArgs e) {
            //Sizing();
        }

        private void videoView_Click(object sender, EventArgs e) {
            _mediaPlayer1.SetAudioTrack(1);
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e) {

        }
    }
}
