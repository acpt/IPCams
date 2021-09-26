namespace System {
    internal class MouseEventHandler {
        private Action<object, EventArgs> videoView_MouseClick;

        public MouseEventHandler(Action<object, EventArgs> videoView_MouseClick) {
            this.videoView_MouseClick = videoView_MouseClick;
        }
    }
}