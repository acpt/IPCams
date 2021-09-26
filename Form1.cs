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

using System.Windows.Automation;
using System.Diagnostics;

namespace IPCams {
    public partial class Form1 : Form {

        //string VIDEO_URL1 = "";
        //string VIDEO_URL2 = "";
        //ibVLC _libvlc;
        //MediaPlayer _mediaPlayer1;
        //MediaPlayer _mediaPlayer2;
        List<Janela> cam = null;
        double[] o_x = null;
        double[] o_y = null;
        double[] o_s = null;
        //Panel[] panel = null;

        //from args or param file... or added and saved
        List<string> Loadipcams = new List<string> { 
            "rtsp://nvrviewer:armgames@[c0-c9-e3-dd-0c-00]:554/stream1",
            "rtsp://nvrviewer:armgames@[28-ee-52-93-33-e0]:554/stream1",
            "rtsp://acpt:armdom@aca1.dyndns.info:554/cam/realmonitor?channel=1&subtype=1" 
        };
        
        public Form1() {
            InitializeComponent();
            LoadCams(); 
            DoGrid();


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

                ip = Tools.ARP.GetIPfromMAC(mac);
                //Funcs.Ping255("192.168.1.1");
                if (ip == "") {
                    Tools.AutoClosingMessageBox.Show("Falhou a procura do mac address [" + mac + "] ! A acordar o Arp","...",7000);
                    string localIP = IPFuncs.GetLocalIPAddress();
                    IPFuncs.Ping255(localIP);
                    Thread.Sleep(7000);
                    //segunda tentativa
                    ip = Tools.ARP.GetIPfromMAC(mac);
                    if (ip == "") { //
                        Tools.AutoClosingMessageBox.Show("Não encontrado, tente sair e voltar a entrar", "Procura de Mac's", 5000);
                        ip = "127.0.0.1";
                    }

                }
                ip = cmd.Substring(0, cmd.IndexOf("[")) + ip + cmd.Substring(cmd.IndexOf("]") + 1);
            }
            return ip;
        }


        private void Form1_Load(object sender, EventArgs e) {
            //DoGrid();
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

        
        private void videoView_MouseWheel(object sender, EventArgs e) {
            Janela j = (Janela)sender;
            MouseEventArgs me = (MouseEventArgs)e;
            string camera = j.Name;
            int i = Convert.ToInt32(camera.Substring(camera.IndexOf(" ") + 1));
            MediaPlayer mp = cam[i].MediaPlayer;
            uint n =0; uint w=0; uint h=0;
            mp.Size(n, ref w, ref h); //Tamanho real do video
            int vw=cam[i].Width; int vh=cam[i].Height; //tamanho da janela 

            int ox=0; int oy=0; int ow=0; int oh=0;

            double s =  1;
            int x = 0; int y = 0;

            string cg = mp.CropGeometry;
            if (cg!=null) {
                //restaura "x" "y" e tamanhos para calcular o "s" dos settings anteriores
                //ox = Convert.ToInt32(cg.Substring(cg.LastIndexOf("+") + 1 ));
                //oy = Convert.ToInt32(cg.Substring(cg.IndexOf("+") + 1, cg.LastIndexOf("+") - cg.IndexOf("+") - 1));

                //ow = Convert.ToInt32(cg.Substring(0, cg.IndexOf("x"))) + ox;
                //oh = Convert.ToInt32(cg.Substring(cg.IndexOf("x") + 1, cg.IndexOf("+") - cg.IndexOf("x") - 1)) + oy;
                
                //restaura s - para a escala
                //s = (double)w / (double)ow;
                ow = (int)o_x[i];
                s = o_s[i];
                Console.WriteLine(s);
            }
            s = (double)s + (double)me.Delta / (double)2000;
            x = (int)Math.Round((double)me.X * (double)w / (double)vw); //traduz para a posicao real no video
            y = (int)Math.Round((double)me.Y * (double)h / (double)vh); //traduz para a posicao real no video

            if (s < 0.16 ) { s = 0.16; }
            if (s > 1) { s = 1; x = (int)w / 2; y = (int)h / 2;}

            o_s[i] = s;
            o_x[i] = (double)s * w;
            ///

            int mx = (int)(y - (double)w * s / 2); // centra janela no x
            int my = (int)(y - (double)h * s / 2); // centra janela no y

            string CropStr = (int)( Math.Truncate((double)s * w)) + "x" + (int)(Math.Truncate((double)(s * w) / w  * h)) + "+" + mx + "+" + my;
            mp.CropGeometry = CropStr;
            Console.WriteLine("w" + w +"x" + h + "  (" + x + ", " + y + ")  Delta:" + me.Delta + " s=" + (double)s + "   CROP= "+ cg + " " + CropStr);

        }


        private void videoView_Click(object sender, EventArgs e) {
            //_mediaPlayer1.SetAudioTrack(1);
            MouseEventArgs me = (MouseEventArgs)e;
            switch (me.Button) {

                case MouseButtons.Left:
                    // Left click
                    break;

                case MouseButtons.Right:
                    // Right click
                    break;
            }
        }

        private void ContextMenuStrip_click(object sender, ToolStripItemClickedEventArgs e) {
            ContextMenuStrip cm = (ContextMenuStrip)sender;
            string camera = cm.SourceControl.Name;
            int i = Convert.ToInt32( camera.Substring(camera.IndexOf(" ")+1));

            string camRstp = "";
            switch (e.ClickedItem.Text) {
                case "Add Cam":
                    if (Tools.Funcs.ShowDialog(ref camRstp, "Add Camera") == DialogResult.OK) {
                        cam.Insert(i, new Janela());
                        JanelaDefaults(cam[i], GrokCmd(camRstp));
                        DoGrid(true);
                    }
                    break;
                case "Del Cam":
                    camRstp = "Delete Cam[" + cam[i].URL +  "] ? ";
                    if (Tools.Funcs.ShowDialog(ref camRstp, "Atention", false)==DialogResult.OK) {
                        cam[i].MediaPlayer.Stop();
                        cam[i].MediaPlayer.Dispose();
                        cam.Remove(cam[i]);
                        DoGrid(true);
                    }

                    break;
                case "Edit Cam":
                    camRstp = cam[i].URL;
                    if (Tools.Funcs.ShowDialog(ref camRstp, "Edit") == DialogResult.OK) {
                        cam[i].MediaPlayer.Stop();
                        cam[i].MediaPlayer.Dispose();
                        cam[i].Text = GrokCmd(camRstp);
                        cam[i].init(GrokCmd(camRstp));
                        DoGrid(true);
                    }
                    break;
                case "Move Up":
                    if (i > 0) {
                        Janela store = new Janela();
                        store = cam[i-1];
                        cam[i-1] = cam[i];
                        cam[i] = store;
                        DoGrid(true);
                    }
                    break;
                case "Move Down":
                    if (i < cam.Count - 1) {
                        Janela store = new Janela();
                        store = cam[i + 1];
                        cam[i + 1] = cam[i];
                        cam[i] = store;
                        DoGrid(true);
                    }
                    break;
                case "Mute":
                    if (cam[i].MediaPlayer.Volume==0) {
                        cam[i].MediaPlayer.Volume = 100;
                    }
                    else {
                        cam[i].MediaPlayer.Volume = 0;
                    }
                    break;
            }
        }

        private void Form1_Resize(object sender, EventArgs e) {
            DoGrid();
        }
    }
}
