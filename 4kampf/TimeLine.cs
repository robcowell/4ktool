using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Globalization;

namespace kampfpanzerin {
    public partial class TimeLine : UserControl {
        private static readonly Color[] camColors = { Color.Red, Color.Green, Color.Blue };

        private Bitmap bmp;
        public List<TimelineBar> syncBars;
        public List<TimelineBar> camBars;
        private TimelineBarEvent eventUnderEdit = null;
        private int eventUnderEditBarIndex;
        private Point dragStart;
        private static CultureInfo culture = CultureInfo.CreateSpecificCulture("en-GB");
        public bool camMode { get; set; }

        private Color BAR_COL = Color.FromArgb(150, 150, 150);
        private Color BAR_COL_LABEL = Color.White;
        //private Color BAR_COL_HIGHLIGHT = Color.FromArgb((int)(.894 * 255.0), (int)(.745 * 255.0), (int)(.427 * 255.0));
        //private Color BAR_COL_LABEL_HIGHLIGHT = Color.FromArgb((int)(.894 * 255.0), (int)(.745 * 255.0), (int)(.427 * 255.0));
        private Color BAR_COL_HIGHLIGHT = Color.LimeGreen;
        private Color BAR_COL_LABEL_HIGHLIGHT = Color.LimeGreen;
        
        private const int BAR_LEFT_MARGIN = 50;
        private const int BAR_HORIZ_MARGIN = 2;
        private const int WAVEFORM_MARGIN = 6;
        private const int BOTTOM_CONTROL_MARGIN = 28;
        
        private int barHeight = 0;
        private float timeMax = 100.0f;
        private float timeStart = 0;
        private float timeStop = 100.0f;
        private float timeCurrent = 50.0f;

        public TimeLine() {
            InitializeComponent();
            syncBars = new List<TimelineBar>();
            camBars = new List<TimelineBar>();
            TimelineBar camPos = new TimelineBar("cam pos", TimelineBar.TimeLineMode.CAMERA_POS);
            TimelineBar referencePoint = new TimelineBar("look at", TimelineBar.TimeLineMode.CAMERA_REF);
            TimelineBar upVector = new TimelineBar("up", TimelineBar.TimeLineMode.CAMERA_UP);
            camBars.Add(camPos);
            camBars.Add(referencePoint);
            camBars.Add(upVector);
            Redraw();
        }

        public void SetProject(Project project) {
            this.camBars = project.camBars;
            this.syncBars = project.syncBars;
            Redraw();
        }

        public void SetTime(float time) {
            timeCurrent = time;
        }

        public void SetMaxTime(float time) {
            timeMax = time;
            SetZoomBounds();
        }

        public bool LoadData(string filename) {
            eventUnderEdit = null;
            syncBars = new List<TimelineBar>();

            if (!File.Exists(filename))
                return false;

            string s = File.ReadAllText(filename);
            string[] lines = s.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string line in lines) {
                TimelineBar b = new TimelineBar("sn[" + syncBars.Count + "]");
                syncBars.Add(b);

                string[] events = line.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string ev in events) {
                    string[] evsplit = ev.Split(',');
                    TimelineBarEvent be = new TimelineBarEvent();
                    be.time = float.Parse(evsplit[0], culture);
                    be.value = float.Parse(evsplit[1], culture);
                    if (evsplit[2] == "FIXEDVAL") be.type = BarEventType.HOLD;
                    if (evsplit[2] == "LERP") be.type = BarEventType.LERP;
                    if (evsplit[2] == "SMOOTH") be.type = BarEventType.SMOOTH;
                    b.events.Add(be);
                }

                b.Recalc();
            }

            Redraw();

            return true;
        }

        public void SaveData(string filename) {
            string s = "";
            foreach (TimelineBar b in syncBars) {
                foreach (TimelineBarEvent be in b.events)
                    s += be.time.ToString("0.000000",culture) + "," + be.value.ToString("0.000000",culture) + "," + be.type.ToString() + ";";
                s += "\n";
            }
            File.WriteAllText(filename, s);
        }

        public void Redraw() {
            if (timeCurrent < timeStart || timeCurrent > timeStop)
                SetZoomBounds();

            if (Height == 0)
                return;

            bmp = new Bitmap(Width, Height);
            Graphics graphics = Graphics.FromImage(bmp);
            Font font = new Font("Lucida Console", 8, FontStyle.Regular);
            SolidBrush brush = new SolidBrush(Color.White);
            graphics.Clear(Color.FromArgb(64, 64, 64));

            // Calculate steps
            float pixWidth = (Width - 6) - BAR_LEFT_MARGIN;
            float timeRange = timeStop - timeStart;
            float pixelsPerInterval = pixWidth / timeRange * 0.25f;
            float secsPerInterval = 0.25f;
            while (pixelsPerInterval < Width/20.0f) {
                pixelsPerInterval *= 2.0f;
                secsPerInterval *= 2.0f;
            }

            trkZoom.Maximum = (int)timeMax;

            if (camMode)
                RenderGraphs(graphics, font, brush, camBars);
            else
                RenderGraphs(graphics, font, brush, syncBars);

            // Render ticks
            int timeY = Height - (BOTTOM_CONTROL_MARGIN + 10); 
            Pen pt = new Pen(Color.FromArgb(64, 32, 32, 32), 1.0f);
            pt.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
            brush.Color = Color.White;
            graphics.DrawString("Time", font, brush, 2, timeY - 2);
            float currentTimePos = timeStart;
            font = new Font("Lucida Console", 6, FontStyle.Regular);
            for (float x = BAR_LEFT_MARGIN; x < Width - 6; x += pixelsPerInterval) {
                graphics.DrawLine(pt, x, timeY, x, BAR_HORIZ_MARGIN);
                graphics.DrawString(currentTimePos.ToString(), font, brush, x - 4, timeY);
                currentTimePos += secsPerInterval;
            }

            // Render current time marker
            int xPos = TimeToXCoord(timeCurrent);
            if (xPos != -1) {
                pt = new Pen(Color.Lime, 1.0f);
                brush.Color = Color.LimeGreen; 
                graphics.DrawLine(pt, xPos, BAR_HORIZ_MARGIN, xPos, Height - BOTTOM_CONTROL_MARGIN);
                graphics.DrawString(timeCurrent.ToString("0.00"), font, brush, xPos, Height - BOTTOM_CONTROL_MARGIN - font.Size / 2);
            }

            graphics.Dispose();
            pictureBox.Image = bmp;
            button1.Enabled = (syncBars.Count<16);
        }

        private TimelineBar selectedBar;

        private void RenderGraphs(Graphics g, Font f, SolidBrush b, List<TimelineBar> bars) {
            // Render bars, labels, events
            if (bars.Count > 0) {
                barHeight = Height - (BAR_HORIZ_MARGIN * 2 + BOTTOM_CONTROL_MARGIN + 20 + BAR_HORIZ_MARGIN * (bars.Count - 1));
                barHeight /= bars.Count;
                int currY = BAR_HORIZ_MARGIN + barHeight / 2;
                Pen p = new Pen(Color.Lime, (float)barHeight);
                foreach (TimelineBar bar in bars) {
                    //bar.Draw();
                    if (bar.events.Count > 0) {
                        // Find start and end for trackbar
                        int startX = TimeToXCoord(bar.events[0].time, false);
                        int endX = TimeToXCoord(bar.events[bar.events.Count - 1].time, false);

                        if (startX < BAR_LEFT_MARGIN)
                            startX = BAR_LEFT_MARGIN;

                        if (endX > Width - 6)
                            endX = Width - 6;

                        if (startX != endX && endX > BAR_LEFT_MARGIN && startX < Width - 6) {
                            if (bar == selectedBar)
                                p.Color = BAR_COL_HIGHLIGHT;
                            else
                                p.Color = BAR_COL;

                            // Main track box
                            g.DrawLine(p, startX, currY, endX, currY);
                            //g.DrawRectangle(p, new Rectangle(BAR_LEFT_MARGIN, currY - BAR_HEIGHT / 2, Width - (BAR_LEFT_MARGIN + 6), BAR_HEIGHT));
                        }
                    }

                    // Bar label
                    if (bar == selectedBar)
                        b.Color = BAR_COL_LABEL_HIGHLIGHT;
                    else
                        b.Color = BAR_COL_LABEL;

                    if (barHeight >= 10)
                        g.DrawString(bar.name, f, b, 2, currY - barHeight / 2);
                    if (barHeight >= 20) {
                        if (cameraModeCheckBox.Checked) {
                            Vector3f val = bar.GetVectorValueAtTime(timeCurrent);
                            string text = val.ToString();
                            if (val != Vector3f.INVALID) {
                                b.Color = Color.FromArgb(128, 128, 128);
                                g.DrawString(text, f, b, 2, currY - barHeight / 2 + 12);
                            }
                        } else {
                            float val = bar.GetValueAtTime(timeCurrent);
                            string text = val.ToString("0.000");
                            if (val != -666666.0) {
                                b.Color = Color.FromArgb(128, 128, 128);
                                g.DrawString(text, f, b, 2, currY - barHeight / 2 + 12);
                            }
                        }
                    }

                    // Events
                    foreach (TimelineBarEvent be in bar.events) {
                        int xCoord = TimeToXCoord(be.time);
                        if (xCoord != -1) {
                            Color c = Color.FromArgb(64, 64, 64);
                            if (bar.events.Count == 1)
                                c = Color.White;
                            DrawEvent(g, xCoord, currY, barHeight, c);
                        }
                    }

                    // Waveform
                    if (bar.mode == TimelineBar.TimeLineMode.SYNC)
                        RenderSyncWave(g, b, currY, bar);
                    else
                        RenderCamWave(g, b, currY, bar);

                    currY += barHeight + BAR_HORIZ_MARGIN;
                }
            }
        }


        public void DrawEvent(Graphics g, int x, int y, int height, Color c) {
            Pen p = new Pen(c, 1.0f);
            SolidBrush b = new SolidBrush(c);

            g.DrawLine(p, x, y + height / 2, x, y - height / 2);

            if (height < 16)
                return;

            y = y + height / 2;
            if (height % 2 == 1)
                y++;

            Point[] Points = new Point[3];
            Points[0] = new Point(x, y - 4);
            Points[1] = new Point(x + 4, y);
            Points[2] = new Point(x - 4, y);
            g.FillPolygon(b, Points);

            y -= height;
            Points[0] = new Point(x, y + 4);
            Points[1] = new Point(x + 4, y);
            Points[2] = new Point(x - 4, y);
            g.FillPolygon(b, Points);
        }

        private void RenderSyncWave(Graphics g, SolidBrush b, int currY, TimelineBar bar) {
            if (barHeight > 10 && bar.events.Count > 0) {
                float wavRange = Math.Abs(bar.maxVal - bar.minVal);
                float pixWavConv = 0;
                if (wavRange > 0)
                    pixWavConv = (barHeight - WAVEFORM_MARGIN * 2) / wavRange;
                b.Color = Color.FromArgb(32, 32, 32);
                Pen p2 = new Pen(b);
                for (int wavX = BAR_LEFT_MARGIN; wavX < Width - 7; wavX++) {
                    float currVal = bar.GetValueAtTime(XCoordToTime(wavX));
                    float nextVal = bar.GetValueAtTime(XCoordToTime(wavX + 1));
                    if (currVal != -666666.0) {
                        if (wavRange > 0) {
                            int wavY = (int)(currY + barHeight / 2 - (currVal * pixWavConv + WAVEFORM_MARGIN));
                            int wavY2 = (int)(currY + barHeight / 2 - (nextVal * pixWavConv + WAVEFORM_MARGIN));
                            wavY += (int)(bar.minVal * pixWavConv);
                            wavY2 += (int)(bar.minVal * pixWavConv);
                            g.DrawLine(p2, wavX, wavY, wavX + 1, wavY2);
                        } else
                            g.FillRectangle(b, wavX, currY, 1, 1);
                    }
                }
            }
        }

        private void RenderCamWave(Graphics g, SolidBrush b, int currY, TimelineBar bar) {
            if (barHeight > 10 && bar.events.Count > 0) {
                float wavRange = Math.Abs(bar.maxVal - bar.minVal);
                float pixWavConv = 0;
                if (wavRange > 0)
                    pixWavConv = (barHeight - WAVEFORM_MARGIN * 2) / wavRange;
                b.Color = Color.FromArgb(32, 32, 32);
                Pen p2 = new Pen(b);
                for (int wavX = BAR_LEFT_MARGIN; wavX < Width - 7; wavX++) {
                    Vector3f currVal = bar.GetVectorValueAtTime(XCoordToTime(wavX));
                    Vector3f nextVal = bar.GetVectorValueAtTime(XCoordToTime(wavX + 1));
                    if (currVal != Vector3f.INVALID) {
                        if (wavRange > 0) {
                            for (int i = 0; i < 3; i++) {
                                p2.Color = camColors[i];
                                RenderStep(g, currY, bar, pixWavConv, p2, wavX, currVal[i], nextVal[i]);
                            }
                        } else
                            g.FillRectangle(b, wavX, currY, 1, 1);
                    }
                }
            }
        }

        private void RenderStep(Graphics g, int currY, TimelineBar bar, float pixWavConv, Pen p2, int wavX, float currVal, float nextVal) {
            int wavY = (int)(currY + barHeight / 2 - (currVal * pixWavConv + WAVEFORM_MARGIN));
            int wavY2 = (int)(currY + barHeight / 2 - (nextVal * pixWavConv + WAVEFORM_MARGIN));
            wavY += (int)(bar.minVal * pixWavConv);
            wavY2 += (int)(bar.minVal * pixWavConv);
            g.DrawLine(p2, wavX, wavY, wavX + 1, wavY2);
        }

        public void ApplySettings() {
            btnSnapBeat.BackColor = Properties.Settings.Default.snapBarEvents ? Color.FromArgb(100, 100, 100) : BackColor;
        }

        private int TimeToXCoord(float time, bool clip=true) {
            if (time < timeStart || time > timeStop)
                if (clip)
                    return -1;

            float timeRange = timeStop - timeStart;
            float pixWidth = (Width - 6) - BAR_LEFT_MARGIN;
            return (int)(BAR_LEFT_MARGIN + (time - timeStart) / timeRange * pixWidth);
        }

        private float XCoordToTime(int x) {
            if (x < BAR_LEFT_MARGIN || x > Width-6)
                return -1.0f;

            float timeRange = timeStop - timeStart;
            float pixWidth = (Width - 6) - BAR_LEFT_MARGIN;
            return (float)((x - BAR_LEFT_MARGIN) / pixWidth * timeRange + timeStart);
        }

        private void DeleteBar(int which) {
            if (syncBars == null || which >= syncBars.Count)
                return;

            if (syncBars[which].events.Count > 0) {
                string s = "Are you sure you want to delete this track?";
                if (which < syncBars.Count - 1)
                    s += "\n\nTracks below this one will be renamed!";
                MessageBoxManager.Yes = "Do it man";
                MessageBoxManager.No = "Hell no!";
                if (MessageBox.Show(s, "4kampf", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;
            }

            syncBars.RemoveAt(which);

            for (int i = 0; i < syncBars.Count; i++)
                syncBars[i].name = "sn[" + i + "]";

            Kampfpanzerin.SetDirty();
            Redraw();
        }

        private List<TimelineBar> GetCurrentBars() {
            return cameraModeCheckBox.Checked ? camBars : syncBars;
        }

        private void HandleClick(object sender, EventArgs e) {
            MouseEventArgs mea = (MouseEventArgs)e;
            Point clickPos = new Point(mea.X, mea.Y);
            int whichBar = clickPos.Y / (barHeight + BAR_HORIZ_MARGIN);

            List<TimelineBar> barsUnderEdit = GetCurrentBars();

            // Select clicked bar
            SelectBar(whichBar, barsUnderEdit);

            // Transport / rewind to start
            if (mea.Button == MouseButtons.Left && mea.Y >= Height - (BOTTOM_CONTROL_MARGIN + 20)) {
                float t = 0;
                if (mea.X > BAR_LEFT_MARGIN)
                    t = XCoordToTime(clickPos.X);

                AppForm.GetInstance().klangPlayer.SetPosition(t);
            }

            // Add/edit/del event
            if (whichBar < barsUnderEdit.Count && mea.Button == MouseButtons.Right) {
                float evTime = XCoordToTime(clickPos.X);
                if (evTime >= 0) {
                    if (Properties.Settings.Default.snapBarEvents)
                        evTime = NearestBeat(evTime);
                    TimelineBarEvent chosenEvent = null;
                    float chosenDist = 1000.0f;
                    foreach (TimelineBarEvent be in barsUnderEdit[whichBar].events) {
                        float diff = Math.Abs(be.time - evTime);
                        if (diff < 1.0f && diff < chosenDist)
                            chosenEvent = be;
                    }
                    if (chosenEvent != null)
                        EditEvent(barsUnderEdit[whichBar], chosenEvent);
                    else
                        AddEvent(barsUnderEdit[whichBar], evTime);
                }
            }
            Redraw();
        }

        private void SelectBar(int whichBar, List<TimelineBar> syncBars) {
            if (whichBar < syncBars.Count) {
                selectedBar = syncBars[whichBar];
            }
        }

        private void AddEvent(TimelineBar b, float time) {
            if (cameraModeCheckBox.Checked) {
                Vector3f value = new Vector3f();
                switch (b.mode) {
                    case TimelineBar.TimeLineMode.CAMERA_POS:
                        value = GraphicsManager.GetInstance().GetCamera().position;
                        break;
                    case TimelineBar.TimeLineMode.CAMERA_REF:
                        value = GraphicsManager.GetInstance().GetCamera().forward;
                        break;
                    case TimelineBar.TimeLineMode.CAMERA_UP:
                        value = GraphicsManager.GetInstance().GetCamera().up;
                        break;
                }
                TimelineBarEventCameraEditForm frm = new TimelineBarEventCameraEditForm(time, 
                    value, BarEventType.CAMERA, false);
                frm.StartPosition = FormStartPosition.Manual;
                frm.Location = new Point(Cursor.Position.X - 97, Cursor.Position.Y - 169);
                if (frm.ShowDialog() == DialogResult.OK) {
                    TimelineBarEvent be = new TimelineBarEvent();
                    be.time = frm.GetTime();
                    be.type = frm.GetEventType();
                    be.vecValue = frm.GetValue();
                    b.events.Add(be);
                    Kampfpanzerin.SetDirty();
                    b.Recalc();
                }
            } else {
                TimelineBarEventEditForm frm = new TimelineBarEventEditForm(time, 0, BarEventType.SMOOTH, false);
                frm.StartPosition = FormStartPosition.Manual;
                frm.Location = new Point(Cursor.Position.X - 97, Cursor.Position.Y - 169);
                if (frm.ShowDialog() == DialogResult.OK) {
                    TimelineBarEvent be = new TimelineBarEvent();
                    be.time = frm.GetTime();
                    be.type = frm.GetEventType();
                    be.value = frm.GetValue();
                    b.events.Add(be);
                    Kampfpanzerin.SetDirty();
                    b.Recalc();
                }
            }
        }

        private void EditEvent(TimelineBar b, TimelineBarEvent be) {
            if (cameraModeCheckBox.Checked) {
                Vector3f value = new Vector3f();
                TimelineBarEventCameraEditForm frm = new TimelineBarEventCameraEditForm(be.time,
                    be.vecValue, BarEventType.CAMERA, true);
                frm.StartPosition = FormStartPosition.Manual;
                //Point offset = PointToScreen(new Point(0,0));
                //frm.Location = new Point(TimeToXCoord(be.time) - 97 + offset.X, Cursor.Position.Y - 169 + offset.Y);
                frm.Location = new Point(Cursor.Position.X - 97, Cursor.Position.Y - 169);
                DialogResult d = frm.ShowDialog();
                if (d == DialogResult.OK) {
                    be.time = frm.GetTime();
                    be.type = frm.GetEventType();
                    be.vecValue = frm.GetValue();
                    Kampfpanzerin.SetDirty();
                    b.Recalc();
                } else if (d == DialogResult.Abort) {
                    b.events.Remove(be);
                    Kampfpanzerin.SetDirty();
                    b.Recalc();
                }
            } else {
                TimelineBarEventEditForm frm = new TimelineBarEventEditForm(be.time, be.value, be.type, true);
                frm.StartPosition = FormStartPosition.Manual;
                //Point offset = PointToScreen(new Point(0,0));
                //frm.Location = new Point(TimeToXCoord(be.time) - 97 + offset.X, Cursor.Position.Y - 169 + offset.Y);
                frm.Location = new Point(Cursor.Position.X - 97, Cursor.Position.Y - 169);
                DialogResult d = frm.ShowDialog();
                if (d == DialogResult.OK) {
                    be.time = frm.GetTime();
                    be.type = frm.GetEventType();
                    be.value = frm.GetValue();
                    Kampfpanzerin.SetDirty();
                    b.Recalc();
                } else if (d == DialogResult.Abort) {
                    b.events.Remove(be);
                    Kampfpanzerin.SetDirty();
                    b.Recalc();
                }
            }
        }
        
        private void trkZoom_ValueChanged(object sender, EventArgs e) {
            SetZoomBounds();
        }

        private void SetZoomBounds() {
            float zoomLevel = Math.Max(1.0f, trkZoom.Maximum - trkZoom.Value);

            timeStart = (float)Math.Floor(Math.Max(0, timeCurrent - zoomLevel));
            timeStart = (float)Math.Round(timeStart * 4, MidpointRounding.ToEven) / 4;
            timeStop = Math.Min(timeMax, timeCurrent + zoomLevel);
        }

        private void button1_Click(object sender, EventArgs e) {
            AddBar();
        }

        private void AddBar() {
            if (syncBars.Count > 15)
                return;

            syncBars.Add(new TimelineBar("sn[" + syncBars.Count + "]"));
            Kampfpanzerin.SetDirty();
            Redraw();
        }

        private void AddCamera() {
            //bars.Contains
        }

        private void pictureBox_Resize(object sender, EventArgs e) {
            Redraw();
        }

        private void btnDel_Click(object sender, EventArgs e) {
            for (int i = 0; i < syncBars.Count; i++)
                if (syncBars[i] == selectedBar) {
                    DeleteBar(i);
                    selectedBar = null;
                }
        }

        private void pictureBox_MouseDown(object sender, MouseEventArgs e) {
            MouseEventArgs mea = (MouseEventArgs)e;
            Point clickPos = new Point(mea.X, mea.Y);
            int whichBar = clickPos.Y / (barHeight + BAR_HORIZ_MARGIN);

            if (whichBar < syncBars.Count && mea.Button == MouseButtons.Left) {
                float evTime = XCoordToTime(clickPos.X);
                TimelineBarEvent chosenEvent = null;
                float chosenDist = 1000.0f;
                foreach (TimelineBarEvent be in syncBars[whichBar].events) {
                    float diff = Math.Abs(be.time - evTime);
                    if (diff < 1.0f && diff < chosenDist)
                        chosenEvent = be;
                }
                if (chosenEvent != null) {
                    eventUnderEdit = chosenEvent;
                    eventUnderEditBarIndex = whichBar;
                    dragStart = clickPos;
                }
            }
        }

        private void pictureBox_MouseUp(object sender, MouseEventArgs e) {
            eventUnderEdit = null;
        }

        private float NearestBeat(float f) {
            int bpm = AppForm.GetInstance().klangPlayer.GetBPM();
            float oneBeatLength = 1.0f / ((float)bpm / 60.0f);   // secs
            float rem = f % oneBeatLength;
            f -= rem;
            if (rem > (oneBeatLength / 2))
                f += oneBeatLength;
            return f;
        }

        private void pictureBox_MouseMove(object sender, MouseEventArgs e) {
            if (eventUnderEdit != null) {
                if (Control.ModifierKeys == Keys.Control) { // Edit value
                    float y = (float)e.Y;
                    if (eventUnderEditBarIndex > 0)
                        y -= eventUnderEditBarIndex * (barHeight + BAR_HORIZ_MARGIN);
                    y /= barHeight;
                    float minVal = syncBars[eventUnderEditBarIndex].minVal;
                    float maxVal = syncBars[eventUnderEditBarIndex].maxVal; 
                    float range =  maxVal - minVal;
                    float newVal = maxVal - y * range;
                    eventUnderEdit.value = newVal;
                    syncBars[eventUnderEditBarIndex].Recalc();
                } else {    // Edit time
                    float t = XCoordToTime(e.X);
                    if (Properties.Settings.Default.snapBarEvents)
                        t = NearestBeat(t);
                    if (t > -0.1f)
                        eventUnderEdit.time = t;
                    syncBars[eventUnderEditBarIndex].Recalc();
                }
            }
        }

        private void btnZoomIn_Click(object sender, EventArgs e) {
            trkZoom.Value = Math.Min(trkZoom.Maximum, trkZoom.Value + trkZoom.Maximum / 10);
        }

        private void btnZoomOut_Click(object sender, EventArgs e) {
            trkZoom.Value = Math.Max(trkZoom.Minimum, trkZoom.Value - trkZoom.Maximum / 10);
        }
        
        private void btnSnapBeat_Click(object sender, EventArgs e) {
            Properties.Settings.Default.snapBarEvents = !Properties.Settings.Default.snapBarEvents;
            Kampfpanzerin.ApplySettings();
        }

        private bool TypeIsPresent(BarEventType t) {
            foreach (TimelineBar bar in syncBars)
                foreach (TimelineBarEvent be in bar.events)
                    if (be.type == t)
                        return true;

            return false;
        }

        public string CompileTrackerCode() {
            if (syncBars.Count == 0)
                return "";

            string res = "";
            if (TypeIsPresent(BarEventType.HOLD))
                res += "#define fx(b,bb) u.z<b?bb:bbb\r\n";
            if (TypeIsPresent(BarEventType.LERP))
                res += "#define mx(b,bb,bbb,bbbb) u.z<bb?mix(bbb,bbbb,(u.z-b)/(bb-b)):bbbb\r\n";
            if (TypeIsPresent(BarEventType.SMOOTH))
                res += "#define sx(b,bb,bbb,bbbb) u.z<bb?bbb+(bbbb-bbb)*smoothstep(b,bb,u.z):bbbb\r\n";
            if (syncBars.Count == 1)
                res += "float sn;";
            else
                res += "float sn[" + syncBars.Count + "];";

            int i = 0;
            foreach (TimelineBar bar in syncBars) {
                if (bar.events.Count == 0)
                    continue;

                if (syncBars.Count == 1)
                    res += "sn=";
                else
                    res += "sn[" + (i++) + "]=";

                //string current = "{0}";

                //for (i = 0; i < bar.events.Count; i++) {
                //    TimelineBarEvent be = bar.events[i];
                //    string startVal = FloatToOptimisedString(be.value);
                //    string startTime = FloatToOptimisedString(be.time);
                //    if (i == bar.events.Count - 1) {
                //        current = string.Format(current, startVal);
                //        return res + current;
                //    } else {
                //        string stopVal = FloatToOptimisedString(bar.events[i + 1].value);
                //        string stopTime = FloatToOptimisedString(bar.events[i + 1].time);
                //        switch (be.type) {
                //            case BarEventType.HOLD:
                //                current = string.Format(current, "fx(" + stopTime + "," + startVal + ",{0})");
                //                break;
                //            case BarEventType.LERP:
                //                current = string.Format(current, "mx(" + startTime + "," + stopTime + "," + startVal + ",{0})");
                //                break;
                //            case BarEventType.SMOOTH:
                //                current = string.Format(current, "sx(" + startTime + "," + stopTime + "," + startVal + ",{0})");
                //                break;
                //        }
                //    }
                //}

                for (i=0; i<bar.events.Count; i++) {
                    TimelineBarEvent be = bar.events[i];
                    string startVal = FloatToOptimisedString(be.value);
                    string startTime = FloatToOptimisedString(be.time);
                    if (i == bar.events.Count - 1)
                        res += ";";
                    else {
                        string stopVal = FloatToOptimisedString(bar.events[i + 1].value);
                        string stopTime = FloatToOptimisedString(bar.events[i + 1].time);
                        switch (be.type) {
                            case BarEventType.HOLD:
                                res += "fx(" + stopTime + "," + startVal + ")";
                                break;
                            case BarEventType.LERP:
                                res += "mx(" + startTime + "," + stopTime + "," + startVal + "," + stopVal + ")";
                                break;
                            case BarEventType.SMOOTH:
                                res += "sx(" + startTime + "," + stopTime + "," + startVal + "," + stopVal + ")";
                                break;
                        }
                    }
                }
            }
            return res;
        }

        private string FloatToOptimisedString(float f) {
            string s = f.ToString(".000", culture);
            if (s.StartsWith("-0."))
                s = s.Substring(2);
            if (s.StartsWith("0."))
                s = s.Substring(1);
            if (s.Contains("."))
                while (s.EndsWith("0") && s.Length > 2)
                    s = s.Substring(0, s.Length - 1);
            return s;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e) {
            this.camMode = ((CheckBox)sender).Checked;
        }

    }
}
