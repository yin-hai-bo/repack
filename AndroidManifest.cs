using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Text.RegularExpressions;

namespace repack {
    class AndroidManifest {
        private readonly String filename;
        private readonly XmlDocument doc;

        private readonly string appLabel;
        public string AppLabel { get { return appLabel; } }

        private readonly string packageName;
        public string PackageName { get { return packageName; } }

        private readonly string appIcon;
        public string AppIcon { get { return appIcon; } }

        private readonly string versionName;
        public string VersionName { get { return versionName; } }

        private readonly string versionCode;
        public string VersionCode { get { return versionCode; } }

        private readonly string umKey;
        public string UMKey { get { return umKey; } }

        private readonly XmlAttribute attrChannel;
        public String Channel {
            get { return attrChannel.Value; }
            set { attrChannel.Value = value; }
        }

        public AndroidManifest(String dir) {
            this.filename = Utils.MakeFilename(dir, "AndroidManifest.xml");
            doc = new XmlDocument();
            doc.Load(filename);
            this.packageName = doc.DocumentElement.GetAttribute("package");
            //
            XmlNode application_element = null;
            foreach (XmlNode n in doc.DocumentElement.ChildNodes) {
                if (n.NodeType == XmlNodeType.Element && n.Name == "application") {
                    application_element = n;
                    break;
                }
            }
            if (application_element == null) {
                throw new Exception("未找到<application>元素");
            }
            this.appLabel = application_element.Attributes["android:label"].Value;
            this.appIcon = application_element.Attributes["android:icon"].Value;
            foreach (XmlNode meta_node in application_element.ChildNodes) {
                if (meta_node.NodeType != XmlNodeType.Element) {
                    continue;
                }
                if (meta_node.Name != "meta-data") {
                    continue;
                }
                switch (meta_node.Attributes["android:name"].Value) {
                case "UMENG_CHANNEL":
                    this.attrChannel = meta_node.Attributes["android:value"];
                    break;
                case "UMENG_APPKEY":
                    this.umKey = meta_node.Attributes["android:value"].Value;
                    break;
                }
                if (this.attrChannel != null && this.umKey != null) {
                    break;
                }
            }
            if (this.attrChannel == null) {
                throw new Exception("No channel define found in AndroidManifest.xml");
            }
            ParseYML(Utils.MakeFilename(dir, "apktool.yml"), out versionCode, out versionName);
        }

        private static void ParseYML(string filename, out string versionCode, out string versionName) {
            using (StreamReader reader = new StreamReader(new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read))) {
                versionCode = null;
                versionName = null;
                Regex regexVersionCode = new Regex(@"\s*versionCode:\s*'(\d+)'", RegexOptions.Compiled);
                Regex regexVersionName = new Regex(@"\s*versionName:\s*([^\r\n]+)", RegexOptions.Compiled);
                while (true) {
                    if (versionName != null && versionCode != null) {
                        break;
                    }
                    string line = reader.ReadLine();
                    if (line == null) {
                        break;
                    }
                    string vc = Match(regexVersionCode, line);
                    if (vc != null) {
                        versionCode = vc;
                        continue;
                    }
                    string vn = Match(regexVersionName, line);
                    if (vn != null) {
                        versionName = vn;
                        continue;
                    }
                }
            }
        }

        private static string Match(Regex regex, string s) {
            Match m = regex.Match(s);
            if (m != null && m.Groups != null && m.Groups.Count == 2) {
                return m.Groups[1].Value;
            }
            return null;
        }

        public void Save() {
            doc.Save(filename);
        }

    }
}
