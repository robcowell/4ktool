using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace kampfpanzerin {

    public class MenuColorTable : ProfessionalColorTable {
        public override Color MenuItemSelected {
            get { return Color.FromArgb(64, 64, 64); }
        }

        public override Color SeparatorDark {
            get { return Color.FromArgb(48, 48, 48); }
        }
        public override Color SeparatorLight {
            get { return Color.FromArgb(48, 48, 48); }
        }
        public override Color ToolStripDropDownBackground {
            get { return Color.FromArgb(32, 32, 32); }
        }

        public override Color ImageMarginGradientBegin {
            get { return Color.FromArgb(32, 32, 32); }
        }

        public override Color ImageMarginGradientEnd {
            get { return Color.FromArgb(32, 32, 32); }
        }

        public override Color ImageMarginGradientMiddle {
            get { return Color.FromArgb(32, 32, 32); }
        }

        public override Color MenuItemSelectedGradientBegin {
            get { return Color.FromArgb(32, 32, 32); }
        }
        public override Color MenuItemSelectedGradientEnd {
            get { return Color.FromArgb(32, 32, 32); }
        }

        public override Color MenuItemPressedGradientBegin {
            get { return Color.FromArgb(32, 32, 32); }
        }

        public override Color MenuItemPressedGradientMiddle {
            get { return Color.FromArgb(32, 32, 32); }
        }

        public override Color MenuItemPressedGradientEnd {
            get { return Color.FromArgb(32, 32, 32); }
        }

        public override Color MenuItemBorder {
            get { return Color.FromArgb(64, 64, 64); }
        }

        public override Color MenuBorder {
            get { return Color.FromArgb(32, 32, 32); }
        }
    }
}
