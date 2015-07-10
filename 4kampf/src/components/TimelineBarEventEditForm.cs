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

        private TextBox selectedTxt;

        public TimelineBarEventEditForm(float time, Vector3f value, BarEventType type, bool editMode) {
            InitializeComponent();

            switch (type) {
                case BarEventType.SMOOTH:   cmbType.SelectedIndex = 0; break;
                case BarEventType.LERP: cmbType.SelectedIndex = 1; break;
                case BarEventType.HOLD: cmbType.SelectedIndex = 2; break;
            }
            numTime.Value = (decimal)time;
            txtValueX.Text = value.x.ToString();
            txtValueY.Text = value.y.ToString();
            txtValueZ.Text = value.z.ToString();

            if (editMode) {
                btnDelete.Visible = true;
                Text = "Edit Event";
            } else {
                btnSave.Text = "Add";
                Text = "Add Event";
            }
        }

        public float GetTime() {
            return (float)numTime.Value;
        }

        public Vector3f GetValue() {
            return new Vector3f(float.Parse(txtValueX.Text), float.Parse(txtValueY.Text), float.Parse(txtValueZ.Text));
        }

        public BarEventType GetEventType() {
            switch (cmbType.SelectedIndex) {
                case 0: return BarEventType.SMOOTH;
                case 1: return BarEventType.LERP;
                case 2: return BarEventType.HOLD;
            }
            return BarEventType.HOLD;
        }

        private void BarEventEditForm_Shown(object sender, EventArgs e) {
            txtValueX.Focus();
        }

        private void txtValue_TextChanged(object sender, EventArgs e) {
            TextBox txtValue = (TextBox)sender;
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
            if (selectedTxt != null) {
                selectedTxt.Text = f.ToString();
            }
        }


        private void txtValue_Enter(object sender, EventArgs e) {
            selectedTxt = (TextBox)sender;
            txtValue_TextChanged(sender, e);
        }
    }
}
