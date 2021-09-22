using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibVLCSharp.Shared;

namespace IPCams {

    public partial class Janela: LibVLCSharp.WinForms.VideoView {
        public string URL;
        LibVLC _libvlc;
        MediaPlayer _mediaPlayer;


        public void init(string url) {
            URL=url;
            //if (url=="") return;
            _libvlc = new LibVLC();

            _mediaPlayer = new MediaPlayer(_libvlc);
            this.MediaPlayer = _mediaPlayer;
            this.MediaPlayer.Play(new Media(_libvlc, new Uri(URL)));
        }

        public void stop() {

        }


    }
}
