using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace kampfpanzerin {
    public partial class KlangPlayer : UserControl {
        private float[][] envSamples;
        private bool transportBusy = false;
        private int numInstruments = 0;
        private int bpm;

        public KlangPlayer() {
            InitializeComponent();

            trkVolume.Value = mediaPlayer.settings.volume;
            lblBPM.Visible = false;
            pbBeat.Visible = false;
        }

        private void LoadBPM() {
            lblBPM.Visible = false;

            if (!File.Exists("4klang.h"))
                return;

            string[] lines = File.ReadAllLines("4klang.h");
            string search = "#define BPM ";
            foreach (string s in lines) {
                if (!s.Contains(search))
                    continue;

                float f;
                if (float.TryParse(s.Substring(s.IndexOf(search) + search.Length), out f)) {
                    bpm = (int)f;
                    lblBPM.Text = bpm.ToString() + " bpm";
                    lblBPM.Visible = true;
                }

                return;
            }
        }

        public int GetBPM() {
            return bpm;
        }

        private void LoadEnvelopes() {
            envSamples = new float[16][];
            numInstruments = 0;
            for (int i = 0; i < 16; i++) {
                ArrayList a = new ArrayList();
                string filename = "envelope-" + i + ".dat";
                if (!File.Exists(filename))
                    continue;
                numInstruments++;
                string s = File.ReadAllText(filename);
                string[] split = s.Split(',');
                for (int j = 0; j < split.Count(); j++)
                    if (split[j] != "")
                        a.Add(float.Parse(split[j]));

                envSamples[i] = a.ToArray(typeof(float)) as float[];
            }
        }

        private void mediaPlayer_OpenStateChange(object sender, AxWMPLib._WMPOCXEvents_OpenStateChangeEvent e) {
            if (mediaPlayer.openState == WMPLib.WMPOpenState.wmposMediaOpen) {
                double duration = mediaPlayer.Ctlcontrols.currentItem.duration;
                AppForm.GetInstance().timeLine.SetMaxTime((float)duration);
                trkTransport.Maximum = (int)duration;
                lblTrackLength.Text = duration.ToString("000.00");
            }
        }

        public float[] Get4klangSync() {
            if (envSamples == null || envSamples[0] == null)
                return null;

            int SAMPLE_RATE = 44100;    // Bit hacky but assuming this won't change for now :)
            double currentPosSecs = 0;
            try {   // Neccessary to prevent crash on exit
                currentPosSecs = mediaPlayer.Ctlcontrols.currentPosition;
            } catch (Exception) {
                return null;
            }

            long currentSample = (long)Math.Min(envSamples[0].Length - 1, (currentPosSecs * SAMPLE_RATE) / 256);

            float[] r = new float[numInstruments];
            for (int i = 0; i < numInstruments; i++)
                if (envSamples[i] != null)
                    r[i] = envSamples[i][currentSample];

            vu.channelLevels = r;    // might as well do this here!

            return r;
        }

        public void UpdateStuff() {
            if (Properties.Settings.Default.use4klangEnv)
                vu.Redraw();

            float f = GetPosition();

            transportBusy = true;
            trkTransport.Value = Math.Min((int)f,trkTransport.Maximum);
            transportBusy = false;

            pbBeat.Visible = (f % (1.0f / ((float)bpm / 60.0f)) < 0.1f);
        }

        public void SetLabels(string fps, string time, string cam) {
            lblFPS.Text = fps;
            lblTime.Text = time;
            if (cam != "")
                lblCam.Text = cam;
        }

        public void ApplySettings() {
            vu.Visible = Properties.Settings.Default.use4klangEnv;
            lblCam.Visible = Properties.Settings.Default.enableCamControls;
            mediaPlayer.settings.setMode("Loop", Properties.Settings.Default.enableLooping);
        }

        public bool LoadWAV(string filename) {
            try {
                mediaPlayer.URL = filename;
            } catch (Exception) {
                return false;
            }

            LoadBPM();
            LoadEnvelopes();
            return true;
        }

        public void Stop() {
            mediaPlayer.Ctlcontrols.stop();
        }

        public void Unload() {
            Stop();
            mediaPlayer.currentPlaylist.clear();
        }

        public float GetDuration() {
            return (float)mediaPlayer.currentMedia.duration;
        }

        public float GetPosition() {
            try {
                return (float)mediaPlayer.Ctlcontrols.currentPosition;
            } catch {
                return 0.0f;
            }
        }
        
        public void SetPosition(float t) {
            mediaPlayer.Ctlcontrols.currentPosition = t;
        }

        private void mediaPlayer_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e) {
            if (mediaPlayer.playState == WMPLib.WMPPlayState.wmppsPlaying)
                btnPlay.Image = Properties.Resources.Pause;
            else
                btnPlay.Image = Properties.Resources.Play;
        }

        private void btnPlay_Click(object sender, EventArgs e) {
            if (mediaPlayer.playState == WMPLib.WMPPlayState.wmppsPlaying)
                mediaPlayer.Ctlcontrols.pause();
            else
                mediaPlayer.Ctlcontrols.play();
        }

        private void btnRewind_Click(object sender, EventArgs e) {
            mediaPlayer.Ctlcontrols.currentPosition = 0;
        }

        private void trkTransport_ValueChanged(object sender, EventArgs e) {
            if (transportBusy)
                return;

            try {   // Neccessary to prevent crash on exit
                AppForm.GetInstance().klangPlayer.SetPosition(trkTransport.Value);
            } catch (Exception) { }
        }

        private void trkVolume_ValueChanged(object sender, EventArgs e) {
            mediaPlayer.settings.volume = trkVolume.Value;
            btnMute.BackColor = mediaPlayer.settings.mute ? Color.FromArgb(100, 100, 100) : Color.FromArgb(32, 32, 32);
        }

        private void btnMute_Click(object sender, EventArgs e) {
            mediaPlayer.settings.mute = !mediaPlayer.settings.mute;
            btnMute.BackColor = mediaPlayer.settings.mute ? Color.FromArgb(100, 100, 100) : Color.FromArgb(32, 32, 32);
        }
    }
}
