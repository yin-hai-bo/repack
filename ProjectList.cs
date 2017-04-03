using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace repack {
    class ProjectList {

        private readonly Dictionary<string, Project> container = new Dictionary<string,Project>(8);

        public void Clear() {
            container.Clear();
        }

        public void Load(string dir) {
            string[] files = Directory.GetFiles(dir, "setting@*.xml");
            foreach (string filename in files) {
                Project p = Project.Create(filename);
                if (p != null) {
                    container[p.Name] = p;
                }
            }
        }

        public IEnumerable<string> Keys {
            get { return container.Keys; }
        }

        public Project GetProject(string key) {
            if (String.IsNullOrEmpty(key)) {
                return null;
            }
            Project result;
            if (container.TryGetValue(key, out result)) {
                return result;
            } else {
                return null;
            }
        }
    }
}
