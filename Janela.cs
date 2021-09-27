using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibVLCSharp.Shared;

namespace IPCams {

    public partial class Janela: LibVLCSharp.WinForms.VideoView {
        public string URL;
        public int x = 0;
        public int y = 0;
        public int w = 0;
        public int h = 0;
        public double s=1;

        LibVLC _libvlc;
        MediaPlayer _mediaPlayer;


        public void init(string url) {
            URL=url;
            if (url=="") return;
            _libvlc = new LibVLC();

            _mediaPlayer = new MediaPlayer(_libvlc);
            _mediaPlayer.EnableHardwareDecoding = true;
            _mediaPlayer.AspectRatio = "16:9";
            _mediaPlayer.EnableMouseInput = false;
            _mediaPlayer.EnableKeyInput = false;

            this.MediaPlayer = _mediaPlayer;
            this.MediaPlayer.Play(new Media(_libvlc, new Uri(URL)));
            MediaPlayer.Scale = 0; //no zoom ?
            MediaPlayer.Volume = 0; // muted by default
            //MediaPlayer.UpdateViewpoint();
        }
    }
}
