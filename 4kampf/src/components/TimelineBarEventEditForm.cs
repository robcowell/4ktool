using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace kampfpanzerin {
    public partial class TimelineBarEventEditForm : Form {
        public TimelineBarEventEditForm(float time, float value, BarEventType type, bool editMode) {
            InitializeComponent();

            switch (type) {
                case BarEventType.SMOOTH:   cmbType.SelectedIndex = 0; break;
                case BarEventType.LERP: cmbType.SelectedIndex = 1; break;
                case BarEventType.HOLD: cmbType.SelectedIndex = 2; break;
                case BarEventType.CAMERA: cmbType.SelectedIndex = 3; break;
            }
            numTime.Value = (decimal)time;
            txtValue.Text = value.ToString();

            if (editMode) {
                btnDelete.Visible = true;
                Text = "Edit Event";
            } else {
                btnSave.Text = "Add";
                Text = "Add Event";
            }
            if (type == BarEventType.CAMERA) {
                txtValue.Enabled = false;
                cmbType.Enabled = false;
            }
        }

        public float GetTime() {
            return (float)numTime.Value;
        }

        public float GetValue() {
            return float.Parse(txtValue.Text);
        }

        public BarEventType GetEventType() {
            switch (cmbType.SelectedIndex) {
                case 0: return BarEventType.SMOOTH;
                case 1: return BarEventType.LERP;
                case 2: return BarEventType.HOLD;
                case 3: return BarEventType.CAMERA;
            }
            return BarEventType.HOLD;
        }

        private void BarEventEditForm_Shown(object sender, EventArgs e) {
            txtValue.Focus();
        }

        private void txtValue_TextChanged(object sender, EventArgs e) {
            try {
                string text = txtValue.Text;
                if (text.Length == 0) {
                    text = "0";
                }
                float f = float.Parse(text);
                int df = (int)(f * 100.0f);
                if (df < trkValue.Maximum && df > trkValue.Minimum)
                    trkValue.Value = df;
            } catch (Exception) {
                MessageBox.Show("Dude! That ain't no float, man!", "4kampf", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtValue.Text="0";
            }
        }

        private void trkValue_ValueChanged(object sender, EventArgs e) {
            float f = (float)trkValue.Value / 100.0f;
            txtValue.Text = f.ToString();
        }
    }
}
