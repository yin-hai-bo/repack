using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Text.RegularExpressions;

namespace repack {
    class Project : IEnumerable<String> {

        private readonly string name;

        private readonly List<String> list = new List<string>(32);

        private readonly string keyStore;
        public string KeyStore { get { return keyStore; } }

        private readonly string storePassword;
        public string StorePassword { get { return storePassword; } }

        private readonly string aliasPassword;
        public string AliasPassword { get { return aliasPassword; } }

        private readonly string alias;
        public string Alias { get { return alias; } }

        public static Project Create(string filename) {
            Regex regex = new Regex(@"setting@(.+)\.xml");
            Match m = regex.Match(Path.GetFileName(filename));
            if (m != null && m.Groups != null && m.Groups.Count == 2) {
                try {
                    return new Project(m.Groups[1].Value, filename);
                } catch (Exception) {
                }
            }
            return null;

        }

        private Project(string name, string filename) {
            this.name = name;
            using (XmlReader reader = XmlReader.Create(filename)) {
                while (reader.Read()) {
                    if (reader.Depth == 1 && reader.IsStartElement()) {
                        switch (reader.Name) {
                        case "keystore":
                            keyStore = reader.ReadElementString();
                            break;
                        case "keystore_password":
                            storePassword = reader.ReadElementString();
                            break;
                        case "alias":
                            alias = reader.ReadElementString();
                            break;
                        case "alias_password":
                            aliasPassword = reader.ReadElementString();
                            break;
                        case "channels":
                            LoadChannels(reader);
                            break;
                        }
                    }
                }
            }
        }

        private void LoadChannels(XmlReader reader) {
            int depth = reader.Depth + 1;
            while (reader.Read()) {
                if (reader.Depth == depth) {
                    if (reader.IsStartElement() && reader.Name == "item") {
                        AddChannel(reader.ReadElementString());
                    }
                } else {
                    break;
                }
            }
        }

        private bool AddChannel(string name) {
            if (String.IsNullOrEmpty(name)) {
                return false;
            }
            name = name.Trim();
            if (name.Length == 0) {
                return false;
            }
            int idx = list.BinarySearch(name);
            if (idx < 0) {
                list.Insert(~idx, name);
                return true;
            }
            return false;
        }

        public string Name {
            get { return this.name; }
        }

        public IEnumerator<string> GetEnumerator() {
            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return this.GetEnumerator();
        }

        public int Count {
            get { return list.Count; }
        }

        public string GetChannel(int index) {
            return list[index];
        }
    }
}
