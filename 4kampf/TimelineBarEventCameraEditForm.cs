using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;

namespace kampfpanzerin {
    public partial class TimelineBarEventCameraEditForm : Form {
        private static CultureInfo culture = CultureInfo.CreateSpecificCulture("en-GB");

        public TimelineBarEventCameraEditForm(float time, Vector3f value, BarEventType type, bool editMode) {
            InitializeComponent();

            //switch (type) {
            //    case BarEventType.SMOOTH:   cmbType.SelectedIndex = 0; break;
            //    case BarEventType.LERP: cmbType.SelectedIndex = 1; break;
            //    case BarEventType.HOLD: cmbType.SelectedIndex = 2; break;
            //    case BarEventType.CAMERA: cmbType.SelectedIndex = 3; break;
            //}
            numTime.Value = (decimal)time;
            xValue.Text = value.x.ToString(culture);
            xValue.Text = value.x.ToString(culture);
            xValue.Text = value.x.ToString(culture);

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
            Vector3f ret = new Vector3f();
            ret.x = float.Parse(xValue.Text, culture.NumberFormat);
            ret.y = float.Parse(yValue.Text, culture.NumberFormat);
            ret.z = float.Parse(zValue.Text, culture.NumberFormat);
            return ret;
        }

        public BarEventType GetEventType() {
            return BarEventType.CAMERA;
        }

        private void BarEventEditForm_Shown(object sender, EventArgs e) {
            xValue.Focus();
        }

        private void txtValue_TextChanged(object sender, EventArgs ev) {
            //try {
                
                //string text = xValue.Text;
                //if (text.Length == 0) {
                //    text = "0";
                //}
                //float f = float.Parse(text);
                //int df = (int)(f * 100.0f);
                //xValue.BackColor = Color.FromArgb(unchecked ((int)0xff7f7f7f));
                ValidateInputs();
        //    } catch (Exception) {
        //        xValue.BackColor = Color.Red;
        //        btnSave.Enabled = false;
        //        //MessageBox.Show("Dude! That ain't no float, man!", "4kampf", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        //xValue.Text="0";
        //    }
        }

        private void ValidateInputs() {
            bool allValid = true;
            if (!ValidFloat(xValue.Text)) {
                allValid = false;
                xValue.BackColor = Color.OrangeRed;
            } else {
                xValue.BackColor = Color.FromArgb(unchecked((int)0xff7f7f7f));
            }
            if (!ValidFloat(yValue.Text)) {
                allValid = false;
                yValue.BackColor = Color.OrangeRed;
            } else {
                yValue.BackColor = Color.FromArgb(unchecked((int)0xff7f7f7f));
            }
            if (!ValidFloat(zValue.Text)) {
                allValid = false;
                zValue.BackColor = Color.OrangeRed;
            } else {
                zValue.BackColor = Color.FromArgb(unchecked((int)0xff7f7f7f));
            }
            btnSave.Enabled = allValid;
        }

        private bool ValidFloat(string text) {
            if (text.Length == 0) {
                text = "0";
            }
            float f;
            try {
                float.Parse(text, NumberStyles.Any, culture);
                return true;
            } catch (Exception) {
                return false;
            }
            //int df = (int)(f * 100.0f);
        }
    }
}
