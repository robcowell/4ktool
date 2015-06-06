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

            numTime.Value = (decimal)time;
            xValue.Text = value.x.ToString(culture);
            yValue.Text = value.y.ToString(culture);
            zValue.Text = value.z.ToString(culture);

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
            ValidateInputs();
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
        }
    }
}
