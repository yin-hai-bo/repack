using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace repack {
    static class Utils {

        public static string AppendDirSeparatorChar(string dir) {
            if (dir.Length > 0 && dir[dir.Length - 1] != Path.DirectorySeparatorChar) {
                return dir + Path.DirectorySeparatorChar;
            } else {
                return dir;
            }
        }

        public static string MakeFilename(string dir, string basename) {
            return AppendDirSeparatorChar(dir) + basename;
        }

        public static void ShowErrorBox(IWin32Window owner, string message) {
            MessageBox.Show(owner, message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void ShowWarningBox(IWin32Window owner, string message) {
            MessageBox.Show(owner, message, "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public static void ShowInformationBox(IWin32Window owner, string message) {
            MessageBox.Show(owner, message, "INFORMATION", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
