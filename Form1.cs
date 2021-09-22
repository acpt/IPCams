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

using System.Windows.Automation;
using System.Diagnostics;

namespace IPCams {
    public partial class Form1 : Form {

        //string VIDEO_URL1 = "";
        //string VIDEO_URL2 = "";
        //ibVLC _libvlc;
        //MediaPlayer _mediaPlayer1;
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
            SetCams(); 
            AddGrid();


            //rtsp://acpt:armdom@aca1.dyndns.info:554/cam/realmonitor?channel=1&subtype=1
            //Sizing(1165,470);
        }

        public string GrokCmd(string cmd) {
            string ip = cmd;
  
            int i = cmd.IndexOf("[");
            if (i > 0) {
                string mac = cmd.Substring(i + 1, cmd.IndexOf("]") - i - 1);
                //IPTools.AutoClosingMessageBox.Show("[" + mac + "]","Procurando...",3000);
                //Funcs.MessageBoxTimeout((System.IntPtr)0, "[" + mac + "]" , "Procura de Mac's", 0, 0, 2000);

                ip = IPTools.ARP.GetIPfromMAC(mac);
                //Funcs.Ping255("192.168.1.1");
                if (ip == "") {
                    IPTools.AutoClosingMessageBox.Show("Falhou a procura do mac address [" + mac + "] ! A acordar o Arp","...",7000);
                    string localIP = Funcs.GetLocalIPAddress();
                    Funcs.Ping255(localIP);
                    Thread.Sleep(7000);
                    //segunda tentativa
                    ip = IPTools.ARP.GetIPfromMAC(mac);
                    if (ip == "") { //
                        IPTools.AutoClosingMessageBox.Show("Não encontrado, tente sair e voltar a entrar", "Procura de Mac's", 5000);
                        ip = "127.0.0.1";
                    }

                }
                ip = cmd.Substring(0, cmd.IndexOf("[")) + ip + cmd.Substring(cmd.IndexOf("]") + 1);
            }
            return ip;
        }


        private void Form1_Load(object sender, EventArgs e) {
            //AddGrid();
            //_libvlc = new LibVLC();

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
            //AdicionaJanela(cmd)
        }

        private void videoView_Click(object sender, EventArgs e) {
            //_mediaPlayer1.SetAudioTrack(1);
        }

        private void Form1_Resize(object sender, EventArgs e) {
            AddGrid();
        }
    }
}
