using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace repack {
    public partial class FormMain : Form {

        [DllImport("shell32.dll")]
        private static extern IntPtr ShellExecute(
            IntPtr hwnd,
            string lpOperation,
            string lpFile,
            string lpParameters,
            string lpDirectory,
            int nShowCmd);

        /// <summary>
        /// Parse a special format filename, extract all elements, and build new filename.
        /// The format is:
        ///     prefix-versionname-versioncode-time-channel-other
        /// Example:
        ///     gm-4.9.0.2-vc140-0718-1447-google-861147d.apk
        /// </summary>
        private class FileNameElements {

            public readonly string Prefix;
            public readonly string VersionName;
            public readonly string VersionCode;
            public readonly string BuildTime;
            public readonly string Channel;
            public readonly string CommitId;

            /// <summary>
            /// Parse filename, return null when format invalid
            /// </summary>
            /// <param name="filenameWithoutDir">filename, special format, like: gm-4.9.0.2-vc140-0718-1447-google-861147d.apk</param>
            /// <returns></returns>
            public static FileNameElements Parse(string filenameWithoutDir) {
                string[] fields = filenameWithoutDir.Split('-');
                if (fields.Length == 6) {
                    var match = Regex.Match(fields[2], @"[^\d]*(\d+)");
                    string verCode = match.Success ? match.Groups[1].Value : fields[2];
                    return new FileNameElements(fields[0], fields[1], verCode, fields[3], fields[4], fields[5]);
                } else {
                    return null;
                }
            }

            private FileNameElements(
                string prefix,
                string versionName,
                string versionCode,
                string buildTime,
                string channel,
                string commitId
            ) {
                this.Prefix = prefix;
                this.VersionName = versionName;
                this.VersionCode = versionCode;
                this.BuildTime = buildTime;
                this.Channel = channel;
                this.CommitId = commitId;
            }

            public String BuildWithNewChannel(string newChannel) {
                return String.Format("{0}-{1}-vc{2}-{3}-{4}-{5}",
                    Prefix, VersionName, VersionCode, BuildTime, newChannel, CommitId);
            }
        }

        /// <summary>
        /// 状态的抽象基类
        /// </summary>
        private abstract class State {
            protected readonly FormMain owner;
            public State(FormMain owner) {
                this.owner = owner;
            }
            public abstract void ResetCtrls();
            public abstract void DragEnter(DragEventArgs e);
            public abstract void DragDrop(DragEventArgs e);
            public abstract void ButtonStartClick(string apkFilename);
            public abstract void ButtonAbortClick();
            public abstract bool CanExitApp { get; }
        }

        private class State_Null : State {
            public State_Null(FormMain owner) : base(owner) { }
            public override void ResetCtrls() { }
            public override void DragEnter(DragEventArgs e) { }
            public override void DragDrop(DragEventArgs e) { }
            public override void ButtonStartClick(string apkFilename) { }
            public override void ButtonAbortClick() { }
            public override bool CanExitApp {
                get { return true; }
            }
        }

        /// <summary>
        /// 常规（非打包）状态
        /// </summary>
        private class State_Normal : State {

            public State_Normal(FormMain owner)
                : base(owner) {
            }

            public override void ResetCtrls() {
                owner.cboxProjectName.Enabled = true;
                owner.btnExecute.Enabled =
                    (owner.currentProject != null)
                    && (owner.currentProject.Count > 0)
                    && (!String.IsNullOrEmpty(owner.ApkFilename));
                owner.btnAbort.Enabled = false;
            }

            private String GetFilename(DragEventArgs e) {
                String[] files = (String[])e.Data.GetData(DataFormats.FileDrop);
                if (files != null && files.Length > 0) {
                    String filename = files[0];
                    if (0 == String.Compare(Path.GetExtension(filename), ".apk", true)) {
                        if (File.Exists(filename)) {
                            return filename;
                        }
                    }
                }
                return null;
            }

            public override void DragEnter(DragEventArgs e) {
                if (GetFilename(e) != null) {
                    e.Effect = DragDropEffects.Link;
                } else {
                    e.Effect = DragDropEffects.None;
                }
            }

            public override void DragDrop(DragEventArgs e) {
                String filename = GetFilename(e);
                if (filename != null) {
                    owner.ApkFilename = filename;
                }
            }

            public override void ButtonStartClick(string apkFilename) {
                owner.ChangeState(new State_Running(owner, owner.currentProject));
            }

            void worker_DoWork(object sender, DoWorkEventArgs e) {
                // Do nothing
            }

            public override void ButtonAbortClick() {
                // Do nothing
            }

            public override bool CanExitApp {
                get { return true; }
            }
        }

        /// <summary>
        /// 运行状态（在打包中）
        /// </summary>
        private class State_Running : State, IObserver {

            private delegate void Delegate_SetListViewState(int index, string state);
            private readonly Project project;
            private readonly BackgroundWorker worker = new BackgroundWorker();
            private readonly Action<string> appendLineToConsole;
            private readonly Action<AndroidManifest> showAndroidManifest;
            private readonly Delegate_SetListViewState setListViewState;

            public State_Running(FormMain owner, Project project)
                : base(owner) {
                this.project = project;
                appendLineToConsole = new Action<string>(owner.AppendLineToConsole);
                showAndroidManifest = new Action<AndroidManifest>(owner.ShowAndroidManifest);
                setListViewState = new Delegate_SetListViewState(owner.SetListViewState);
                //
                worker.WorkerReportsProgress = true;
                worker.WorkerSupportsCancellation = true;
                worker.DoWork += worker_DoWork;
                worker.ProgressChanged += worker_ProgressChanged;
                worker.RunWorkerCompleted += worker_RunWorkerCompleted;
                worker.RunWorkerAsync(this);
            }

            public void OnProgress(string message) {
                if (owner.InvokeRequired) {
                    owner.Invoke(appendLineToConsole, message);
                } else {
                    appendLineToConsole(message);
                }
            }

            private void ShowAndroidManifest(AndroidManifest am) {
                if (owner.InvokeRequired) {
                    owner.Invoke(showAndroidManifest, am);
                } else {
                    showAndroidManifest(am);
                }
            }

            private void SetListViewState(int index, string state) {
                if (owner.InvokeRequired) {
                    owner.Invoke(setListViewState, index, state);
                } else {
                    setListViewState(index, state);
                }
            }

            private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
                if (e.Error != null) {
                    OnProgress("发生错误！！！");
                    OnProgress(e.Error.Message);
                } else if (e.Cancelled) {
                    OnProgress("用户中断！！！");
                } else {
                    OnProgress("所有渠道已打包完成！！！");
                }
                owner.ChangeState(new State_Normal(owner));
                if (e.Error != null) {
                    Utils.ShowErrorBox(owner, "打包过程中出现错误");
                    return;
                }
                if (e.Cancelled) {
                    Utils.ShowWarningBox(owner, "打包过程被用户中断");
                    return;
                }
                if (e.Result == null) {
                    Utils.ShowInformationBox(owner, "打包完成");
                } else {
                    DialogResult dr = MessageBox.Show(owner, "打包完成！\r\n是否现在打开目标文件夹？", "完成", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (dr == DialogResult.Yes) {
                        ShellExecute(IntPtr.Zero, null, e.Result.ToString(),
                            null, null, 1);
                    }
                }
            }

            private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e) {
                owner.Text = e.ProgressPercentage.ToString();
            }

            private void worker_DoWork(object sender, DoWorkEventArgs e) {
                string srcBaseFileName = Path.GetFileNameWithoutExtension(owner.ApkFilename);
                FileNameElements elements = FileNameElements.Parse(srcBaseFileName);
                if (elements == null) {
                    throw new Exception("原始文件名格式无效，无法解析。\r\nExpected: prefix-verName-verCode-time-channel-commitId.apk");
                }

                string outputDir = string.Format(@"output\{0}-{1}-{2}-{3}-{4}",
                    elements.Prefix, elements.VersionName, elements.VersionCode, elements.BuildTime, elements.CommitId);
                string workDir = "temp";

                AndroidManifest am = DoUnpack(owner.ApkFilename, workDir);
                ShowAndroidManifest(am);
                if (am.VersionCode != elements.VersionCode) {
                    throw new Exception("原始文件名中VersionCode与解包后实际值不符");
                }
                if (am.VersionName != elements.VersionName) {
                    throw new Exception("原始文件名中VersionName与解包后实际值不符");
                }
                if (am.Channel != elements.Channel) {
                    throw new Exception("原始文件名中渠道名称与解包后实际值不符");
                }

                //
                if (Directory.Exists(outputDir)) {
                    Directory.Delete(outputDir, true);
                }
                Directory.CreateDirectory(outputDir);
                //
                for (int i = 0; i < project.Count; ++i) {
                    if (worker.CancellationPending) {
                        break;
                    }
                    string channel = project.GetChannel(i);
                    string newBaseName = elements.BuildWithNewChannel(channel);

                    SetListViewState(i, "打包");
                    string unsignedFilename = Utils.MakeFilename(outputDir, string.Format("{0}-unsigned.apk", newBaseName));
                    DoPack(srcBaseFileName, workDir, am, channel, unsignedFilename);
                    if (worker.CancellationPending) {
                        break;
                    }
                    SetListViewState(i, "签名");
                    string signedFilename = Utils.MakeFilename(outputDir, string.Format("{0}-unaligned.apk", newBaseName));
                    DoSign(project, unsignedFilename, signedFilename);
                    File.Delete(unsignedFilename);
                    if (worker.CancellationPending) {
                        break;
                    }
                    SetListViewState(i, "对齐");
                    string finalFilename = Utils.MakeFilename(outputDir, string.Format("{0}.apk", newBaseName));
                    DoAlign(signedFilename, finalFilename);
                    //
                    SetListViewState(i, "完成");
                }
                e.Cancel = worker.CancellationPending;
                if (!e.Cancel) {
                    e.Result = outputDir;
                }
            }

            private AndroidManifest DoUnpack(string apkName, string outputDir) {
                if (Directory.Exists(outputDir)) {
                    OnProgress("删除临时目录……");
                    Directory.Delete(outputDir, true);
                }
                OnProgress("准备对APK进行解包……");
                ProcessInvoker.Execute(this, "java.exe", String.Format("-jar tools\\apktool.jar d -f -s -o {0} {1}", outputDir, apkName), true);
                OnProgress("解包完成！");
                //
                OnProgress("解析AndroidManifest.xml……");
                AndroidManifest am = new AndroidManifest(outputDir);
                return am;
            }

            private void DoPack(string baseName, string sourDir, AndroidManifest am, string channel, string destFilename) {
                OnProgress("准备打包：" + channel);
                string buildDir = Utils.MakeFilename(sourDir, "build");
                if (Directory.Exists(buildDir)) {
                    Directory.Delete(buildDir, true);
                }
                am.Channel = channel;
                am.Save();
                ProcessInvoker.Execute(this, "java.exe", String.Format("-jar tools\\apktool.jar b -f -o {0} {1}", destFilename, sourDir), true);
                OnProgress("打包完成");
            }

            private void DoSign(Project project, string unsignedApkFilename, string signedApkFilename) {
                OnProgress("准备签名：" + unsignedApkFilename);
                File.Delete(signedApkFilename);
                ProcessInvoker.Execute(this, "jarsigner.exe",
                    string.Format("-keystore \"{0}\" -storepass \"{1}\" -digestalg SHA1 -sigalg MD5withRSA -keypass \"{2}\" -signedjar \"{3}\" \"{4}\" \"{5}\"",
                            project.KeyStore, project.StorePassword, project.AliasPassword, signedApkFilename, unsignedApkFilename, project.Alias),
                    false
                );
                if (!File.Exists(signedApkFilename)) {
                    throw new Exception("签名失败！");
                }
                OnProgress("签名完成");
            }

            private void DoAlign(string signedApkFilename, string finalApkFilename) {
                OnProgress("准备对齐……");
                File.Delete(finalApkFilename);
                ProcessInvoker.Execute(this, "tools\\zipalign.exe",
                    string.Format("-f -v 4 {0} {1}", signedApkFilename, finalApkFilename),
                    true);
                if (!File.Exists(finalApkFilename)) {
                    throw new Exception("对齐失败！");
                }
                File.Delete(signedApkFilename);
                OnProgress("对齐完成");
            }

            public override void ResetCtrls() {
                owner.cboxProjectName.Enabled = false;
                owner.btnExecute.Enabled = false;
                owner.btnAbort.Enabled = true;
            }

            public override void DragEnter(DragEventArgs e) {
                e.Effect = DragDropEffects.None;
            }

            public override void DragDrop(DragEventArgs e) {
                return;
            }

            public override void ButtonStartClick(string apkFilename) {
                // Do nothing
            }

            public override void ButtonAbortClick() {
                if (!worker.CancellationPending) {
                    this.worker.CancelAsync();
                }
            }

            public override bool CanExitApp {
                get { return false; }
            }
        }

        private readonly ProjectList projectList = new ProjectList();
        private Project currentProject;
        private State currentState;

        public FormMain() {
            InitializeComponent();
            currentState = new State_Null(this);
        }

        private void AppendLineToConsole(string message) {
            editConsole.AppendText(message + "\r\n");
        }

        private void ShowAndroidManifest(AndroidManifest am) {
            labelApkName.Text = string.Format("{0}\r\n{1}\r\nVersion Code: {2}\r\nVersion Name: {3}",
                am.AppLabel, am.PackageName,
                am.VersionCode, am.VersionName);
        }

        private void SetListViewState(int index, string state) {
            lvChannels.Items[index].SubItems[2].Text = state;
        }

        public string ApkFilename {
            get { return editApkFilename.Text; }
            set {
                if (editApkFilename.Text != value) {
                    editApkFilename.Text = value;
                    labelApkName.Text = Path.GetFileNameWithoutExtension(value);
                    currentState.ResetCtrls();
                }
            }
        }

        private void ChangeState(State newState) {
            if (newState != currentState) {
                currentState = newState;
                currentState.ResetCtrls();
            }
        }

        private void FormMain_Load(object sender, EventArgs e) {
            try {
                ReloadProjectList();
            } catch (IOException ex) {
                Utils.ShowErrorBox(this, ex.Message);
            }
            ChangeState(new State_Normal(this));
        }

        private void ReloadProjectList() {
            currentProject = null;
            projectList.Load("projects");
            cboxProjectName.BeginUpdate();
            try {
                cboxProjectName.Items.Clear();
                foreach (string s in projectList.Keys) {
                    cboxProjectName.Items.Add(s);
                }
            } finally {
                cboxProjectName.EndUpdate();
            }
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e) {
            e.Cancel = !currentState.CanExitApp;
        }

        private void FormMain_DragEnter(object sender, DragEventArgs e) {
            currentState.DragEnter(e);
        }

        private void FormMain_DragDrop(object sender, DragEventArgs e) {
            currentState.DragDrop(e);
        }

        private void cboxProjectName_SelectedIndexChanged(object sender, EventArgs e) {
            string name = cboxProjectName.SelectedItem.ToString();
            this.currentProject = projectList.GetProject(name);
            lvChannels.BeginUpdate();
            try {
                lvChannels.Items.Clear();
                if (currentProject != null) {
                    int count = 0;
                    foreach (String channel in currentProject) {
                        ++count;
                        ListViewItem item = lvChannels.Items.Add(count.ToString());
                        item.Tag = channel;
                        item.SubItems.Add(channel);
                        item.SubItems.Add(String.Empty);

                    }
                }
            } finally {
                lvChannels.EndUpdate();
            }
            currentState.ResetCtrls();
        }

        private void btnExecute_Click(object sender, EventArgs e) {
            currentState.ButtonStartClick(this.ApkFilename);
        }

        private void btnAbort_Click(object sender, EventArgs e) {
            currentState.ButtonAbortClick();
        }
    }
}
