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
using Tools;

namespace IPCams {
    public partial class Form1 : Form {

        //string VIDEO_URL1 = "";
        //string VIDEO_URL2 = "";
        //ibVLC _libvlc;
        //MediaPlayer _mediaPlayer1;
        //MediaPlayer _mediaPlayer2;
        static List<Janela> cam = new List<Janela>();
        //Panel[] panel = null;

        //from args or param file... or added and saved
        List<string> Loadipcams = new List<string>(); // { 
            //"rtsp://user:pass@[c0-c9-e3-dd-0c-00]:554/stream1",
            //"rtsp://user:pass@[28-ee-52-93-33-e0]:554/stream1",
            //"rtsp://user:pass@dns.com:554/cam/realmonitor?channel=1&subtype=1",
        //};
        static System.Windows.Forms.Timer checkTimer = new System.Windows.Forms.Timer();
        
        public Form1() {
            InitializeComponent();

            Funcs.LoadCfg(ref Loadipcams);

            if (Loadipcams.Count == 0) AddCam(-1);

            LoadCams(); 
            DoGrid();
            AddCamsMenu();

            //checkTimer.Tick += new EventHandler(VerificaCams);
            //checkTimer.Interval = 30000;
            //checkTimer.Start();

        }

        public void AddCamsMenu() {
            //this.contextMenuStrip1.Items.Clear();
            //InitializeComponent();

            for (int i = 0; i < cam.Count; i++) {
                
                camToolStripMenuItem.Add(new System.Windows.Forms.ToolStripMenuItem());

                camToolStripMenuItem[i].Name = "moveDownToolStripMenuItem";
                camToolStripMenuItem[i].Size = new System.Drawing.Size(180, 22);
                camToolStripMenuItem[i].Text = "Camera " + (i+1) + " " + (cam[i].On?"on":"off") + "  " + GetDom(cam[i].Text);

                contextMenuStrip1.Items.Add(camToolStripMenuItem[i]);
            }

        }

        public static string GetDom(string text) {
            string rt = "";
            int i1 = text.IndexOf("@");
            if (i1 != -1) {
                int i2 = text.IndexOf(":", i1);
                if (i2 != -1) {
                    rt = text.Substring(i1 + 1, i2 - i1 - 1);
                }
            }
            return rt;
        }
        public static string GrokCmd(string cmd) {
            string ip = cmd;
  
            int i = cmd.IndexOf("[");
            if (i > 0) {
                string mac = cmd.Substring(i + 1, cmd.IndexOf("]") - i - 1);
                //IPTools.AutoClosingMessageBox.Show("[" + mac + "]","Procurando...",3000);
                //Funcs.MessageBoxTimeout((System.IntPtr)0, "[" + mac + "]" , "Procura de Mac's", 0, 0, 2000);

                ip = Tools.ARP.GetIPfromMAC(mac);
                //Funcs.Ping255("192.168.1.1");
                if (ip == "") {
                    Tools.AutoClosingMessageBox.Show("Falhou a procura do mac address [" + mac + "] ! A acordar o Arp","...",5000);
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
            this.WindowState = Properties.Settings.Default.F1State;

            // we don't want a minimized window at startup
            if (this.WindowState == FormWindowState.Minimized) this.WindowState = FormWindowState.Normal;

            this.Location = Properties.Settings.Default.F1Location;
            this.Size = Properties.Settings.Default.F1Size;
        }

        private void Form1_Closing(object sender, FormClosingEventArgs e) {
            Properties.Settings.Default.F1State = this.WindowState;
            if (this.WindowState == FormWindowState.Normal) {
                // save location and size if the state is normal
                Properties.Settings.Default.F1Location = this.Location;
                Properties.Settings.Default.F1Size = this.Size;
            }
            else {
                // save the RestoreBounds if the form is minimized or maximized!
                Properties.Settings.Default.F1Location = this.RestoreBounds.Location;
                Properties.Settings.Default.F1Size = this.RestoreBounds.Size;
            }

            // don't forget to save the settings
            Properties.Settings.Default.Save();
        }

        private void videoView_MouseWheel(object sender, EventArgs e) {
            double sx = 0.025;

            Janela j = (Janela)sender;
            MouseEventArgs me = (MouseEventArgs)e;
            string camera = j.Name;
            int i = Convert.ToInt32(camera.Substring(camera.IndexOf(" ") + 1));
            MediaPlayer mp = cam[i].MediaPlayer;
            uint n =0; uint w=0; uint h=0;           
            mp.Size(n, ref w, ref h); //Tamanho real do video
            int vw=cam[i].Width; int vh=cam[i].Height; //tamanho da janela 

            int ox,oy,ow,oh;
            int x = 0; int y = 0; double s = 1;

            string cg = mp.CropGeometry;
            
            if (cg!=null) {
                //restaura "x" "y" e tamanhos para calcular o "s" dos settings anteriores
                oy = Convert.ToInt32(cg.Substring(cg.LastIndexOf("+") + 1 ));
                ox = Convert.ToInt32(cg.Substring(cg.IndexOf("+") + 1, cg.LastIndexOf("+") - cg.IndexOf("+") - 1));

                ow = Convert.ToInt32(cg.Substring(0, cg.IndexOf("x"))) - ox;
                oh = Convert.ToInt32(cg.Substring(cg.IndexOf("x") + 1, cg.IndexOf("+") - cg.IndexOf("x") - 1)) - oy;
                
                //restaura s - para a escala
                s = (double)(ow) / (double)w;

                //ou var stored...
                s = cam[i].s;
                x = cam[i].x;
                y = cam[i].y;
            }

            // calcula margem negra
            int Vvw = vw; int Vvh = vh; int vx = 0; int vy = 0;
            if (vw * (9f / 16f) / vh > 1) {
                Vvw = (int)(vh * (16f / 9f));
                vx = (vw - Vvw) / 2;
            }
            else {
                Vvh = (int)(vw * (9f / 16f)); ;
                vy = (vh - Vvh) / 2;
            }

            if (me.Delta > 0) s = s - sx; else s = s + sx;

            if (s < 0.1 ) { s = 0.1; }
            if (s > 1) { s = 1;  x = 0; y = 0;}

            // h e w correspondentes a e<s>cala
            cam[i].w = (int)(w * s);
            cam[i].h = (int)(h * s);

            // traduz para a posicao real no video e retira metade da dimencao para centrar
            x = x + (int)(Math.Round((double)(me.X - vx) - (int)((double)Vvw / 2)) * s);
            y = y + (int)(Math.Round((double)(me.Y - vy) - (int)((double)Vvh / 2)) * s);

            //corrige (x,y) fora de janela
            if (x < 0) x = 0;
            if (y < 0) y = 0;
            if (x + cam[i].w > w) x = (int)w - cam[i].w;
            if (y + cam[i].h > h) y = (int)h - cam[i].h;

            cam[i].s = s;
            cam[i].x = x;
            cam[i].y = y;
           
            string CropStr = (cam[i].w + x) + "x" + (cam[i].h + y) + "+" + x + "+" + y;
            Console.WriteLine("w" + w + "x" + h + "  (" + me.X + ", " + me.Y + ") " + "  (" + x + ", " + y + ") ");
            mp.CropGeometry = CropStr;
            //Console.WriteLine("w" + w +"x" + h + "  (" + x + ", " + y + ")  Delta:" + me.Delta + " s=" + (double)s + "   CROP= "+ cg + " " + CropStr);
        }


        string MouseDown = null;
        Point startPos;
        Point currentPos;
        int startJan=-1;

        private void videoView_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e) {
            //if (MouseDown == "L") {
            //    int i = startJan;
            //    MediaPlayer mp = cam[i].MediaPlayer;
            //    uint n = 0; uint w = 0; uint h = 0;
            //    mp.Size(n, ref w, ref h); //Tamanho real do video
            //    int vw = cam[i].Width; int vh = cam[i].Height; //tamanho da janela 
            //    //double s = cam[i].s;
            //    int x = cam[i].x;
            //    int y = cam[i].y;

            //    x = x + (int)((currentPos.X - startPos.X) * w / vw);  //move delta
            //    y = y + (int)((currentPos.Y - startPos.Y) * h / vh);

            //    //corrige (x,y) fora de janela
            //    if (x < 0) x = 0;
            //    if (y < 0) y = 0;
            //    if (x + cam[i].w > w) x = (int)w - cam[i].w;
            //    if (y + cam[i].h > h) y = (int)h - cam[i].h;

            //    //crop
            //    string CropStr = (cam[i].w + x) + "x" + (cam[i].h + y) + "+" + x + "+" + y;
            //    mp.CropGeometry = CropStr;
            //}
        }
        private void videoView_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e) {
            MouseDown=null;
        }
        private void videoView_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e) {
            //currentPos = e.Location;

            //switch (e.Button) {
            //    case MouseButtons.Left:
            //        MouseDown = "L";
            //        Janela j = (Janela)sender;
            //        string camera = j.Name;
            //        int i = Convert.ToInt32(camera.Substring(camera.IndexOf(" ") + 1));
            //        startJan = i;
            //        break;
            //    case MouseButtons.Right:
            //        MouseDown = "R";
            //        break;
            //    case MouseButtons.Middle:
            //        MouseDown = "M";
            //        break;
            //    case MouseButtons.XButton1:
            //        MouseDown = "X1";
            //        break;
            //    case MouseButtons.XButton2:
            //        MouseDown = "X2";
            //        break;
            //    case MouseButtons.None:
            //    default:
            //        break;
            //}
        }

        private void videoView_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e) {
            //_mediaPlayer1.SetAudioTrack(1);
            MouseEventArgs me = (MouseEventArgs)e;

            Janela j = (Janela)sender;
            string camera = j.Name;
            int i = Convert.ToInt32(camera.Substring(camera.IndexOf(" ") + 1));
            startJan = i;
            MediaPlayer mp = cam[i].MediaPlayer;

            switch (me.Button) {

                case MouseButtons.Left:
                    // Left click

                    uint n = 0; uint w = 0; uint h = 0;
                    mp.Size(n, ref w, ref h); //Tamanho real do video
                    // descobre a barra preta H ou V
                    int vw = cam[i].Width; int vh = cam[i].Height; //tamanho da janela 

                    // calcula margem negra
                    int Vvw=vw; int Vvh= vh; int vx=0; int vy=0;
                    if (vw * (9f / 16f) / vh > 1 ) {
                        Vvw = (int)(vh * (16f / 9f));
                        vx = (vw-Vvw) / 2 - 1;
                        //Console.WriteLine("Desvio horizontal  " + f(Vvw) + " :  subtrair vx " + vx);
                    }
                    else {
                        Vvh = (int)(vw * (9f / 16f)); ;
                        vy = (vh - Vvh) / 2 - 1;
                        //Console.WriteLine("Devio vertical  " + f(Vvh) + " : subtrair  vy " + vy);
                    }

                    int x = 0; int y = 0; double s = 1;

                    s = cam[i].s;
                    x = cam[i].x;
                    y = cam[i].y;

                    Console.WriteLine("Pos. Entrada  (" + f(x) + ", " + f(y) + ")  Rato:" + me.X + "," + me.Y);
                    Console.WriteLine("da janela total      " + f(vw) +  " :" + f(vh) +  " ");

                    // traduz para a posicao real no video e retira metade da dimencao para centrar
                    x = x + (int)(Math.Round((double)(me.X-vx) - (int)((double)Vvw / 2)) * s);
                    y = y + (int)(Math.Round((double)(me.Y-vy) - (int)((double)Vvh / 2)) * s);

                    //corrige (x,y) fora de janela
                    if (x < 0) x = 0;
                    if (y < 0) y = 0;
                    if (x + cam[i].w > w) x = (int)w - cam[i].w;
                    if (y + cam[i].h > h) y = (int)h - cam[i].h;

                    cam[i].x = x;
                    cam[i].y = y;

                    //Console.WriteLine("rw:" + w + " rh:" + h + "  Saida Rato :" + x + "," + y);
                    //Console.WriteLine("");

                    string CropStr = (cam[i].w + x) + "x" + (cam[i].h + y) + "+" + x + "+" + y;
                    mp.CropGeometry = CropStr;
                    break;

                case MouseButtons.Right:
                    break;
                case MouseButtons.Middle:
                    mp.Stop();
                    break;
            }
        }

        string f(double n) {
            return String.Format("{0,3}", n);
        }

        private void ContextMenuStrip_click(object sender, ToolStripItemClickedEventArgs e) {
            ContextMenuStrip cm = (ContextMenuStrip)sender;
            string camera = cm.SourceControl.Name;
            int i = Convert.ToInt32(camera.Substring(camera.IndexOf(" ")+1));

            string camRstp = "";
            switch (e.ClickedItem.Text) {
            case "Add Cam":
                if (Tools.Funcs.ShowDialog(ref camRstp, "Add Camera") == DialogResult.OK) {
                    cam.Insert(i + 1, new Janela());
                    JanelaDefaults(cam[i + 1], GrokCmd(camRstp));
                    DoGrid(true);
                }
                break;
            case "Del Cam":
                camRstp = "Delete Cam[" + cam[i].Text +  "] ? ";
                if (Tools.Funcs.ShowDialog(ref camRstp, "Atention", false)==DialogResult.OK) {
                    cam[i].MediaPlayer.Stop();
                    cam[i].MediaPlayer.Dispose();
                    cam.Remove(cam[i]);
                    DoGrid(true);
                }

                break;
            case "Edit Cam":
                camRstp = cam[i].Text;
                if (Tools.Funcs.ShowDialog(ref camRstp, "Edit") == DialogResult.OK) {
                    //cam[i].MediaPlayer.Stop();
                    cam[i].MediaPlayer.Dispose();
                    cam[i].MediaPlayer = null;

                    cam[i].Text = camRstp;
                    cam[i].init(GrokCmd(camRstp));
                    //DoGrid(true);
                    Tools.Funcs.SaveCfg(cam);
                }
                break;
            case "Move Up":
                if (i > 0) {
                    Janela store = new Janela();
                    store = cam[i - 1];
                    cam[i - 1] = cam[i];
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
            case "Mute/UnMute":
                if (cam[i].MediaPlayer.Volume == 0) {
                    cam[i].MediaPlayer.Volume = 100;
                }
                else {
                    cam[i].MediaPlayer.Volume = 0;
                }
                break;
            case "Restart":
                //cam[i].MediaPlayer.Stop();
                cam[i].MediaPlayer.Dispose();
                cam[i].MediaPlayer = null;
                cam[i].init(GrokCmd(cam[i].Text));
                //DoGrid(true);


                cam[i].MediaPlayer.Play();
                break;
            case "Off":
                cam[i].On = !cam[i].On;
                camToolStripMenuItem[i].Text = "Camera " + (i + 1) + " " + (cam[i].On ? "on" : "off");
                DoGrid(true);
                break;
            default:
                if (e.ClickedItem.Text.Substring(0,6)=="Camera") {
                    int c = Convert.ToInt16(e.ClickedItem.Text.Substring(6, 3))-1;
                    cam[c].On = !cam[c].On;
                    camToolStripMenuItem[c].Text = "Camera " + (c + 1) + " " + (cam[c].On ? "on" : "off");
                    DoGrid(true);
                }
                break;
            }
        }

        void AddCam(int i) {
            string camRstp = "rstp://user:pass@ipORnameOR[mac]:554/stream";
            if (Tools.Funcs.ShowDialog(ref camRstp, "Add Camera") == DialogResult.OK) {
                if (String.IsNullOrEmpty( camRstp ) && i == -1) System.Environment.Exit(0);
                else {
                    Loadipcams.Add(camRstp);                    
                }
            }
            else if (i == -1) System.Environment.Exit(0);
        }

        private static void VerificaCams(Object myObject, EventArgs myEventArgs) {
            for(int i=0; i<cam.Count; i++) {
                MediaPlayer mp = cam[i].MediaPlayer;

                uint n = 0; uint w = 0; uint h = 0;
                mp.Size(n, ref w, ref h); //Tamanho real do video  PING
                //if (cam[i].MediaPlayer.IsPlaying == false) {
                if (w==0 && h == 0) { 
                    Console.WriteLine("Player [" + i + "]  Parou - forca play");
                    cam[i].MediaPlayer.Dispose();
                    cam[i].MediaPlayer = null;
                    cam[i].init(GrokCmd(cam[i].Text));
                } 
                mp.Dispose();
            }

        }

        private void MP_Error(object sender, EventArgs e, int i) {
            Console.Write("Evento MP ");
            if (cam.Count-1>i) {
                MediaPlayer mp = cam[i].MediaPlayer;

                if (mp.IsPlaying ==  true) { 
                }
            }

        }

        private void Form1_Resize(object sender, EventArgs e) {
            DoGrid();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e) {
            Funcs.SaveCfg(cam);
        }

    }
}
