﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace repack {
    static class ProcessInvoker {

        /// <summary>
        /// 执行指定的进程，并等待它执行完毕
        /// （出错时会抛出异常）
        /// </summary>
        /// <param name="observer">观察者</param>
        /// <param name="filename">要执行的进程文件名</param>
        /// <param name="args">命令行参数</param>
        /// <param name="showCmdLine">是否输出命令行？</param>
        public static void Execute(IObserver observer, string filename, string args, bool showCmdLine) {
            if (showCmdLine) {
                observer.OnProgress(string.Format("准备启动：{0} {1}", filename, args));
            } else {
                observer.OnProgress("准备启动：" + filename);
            }
            using (Process process = new Process()) {
                ProcessStartInfo psi = process.StartInfo;
                psi.CreateNoWindow = true;
                psi.FileName = filename;
                psi.Arguments = args;
                psi.UseShellExecute = false;
                psi.RedirectStandardOutput = true;
                psi.WorkingDirectory = Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);
                if (!process.Start()) {
                    throw new Exception("启动进程失败");
                }
                while (true) {
                    string line = process.StandardOutput.ReadLine();
                    if (line == null) {
                        break;
                    }
                    observer.OnProgress(line);
                }
                process.WaitForExit();
                if (process.ExitCode != 0) {
                    throw new Exception(string.Format("进程退出码不为零(#{0})", process.ExitCode));
                }
            }
        }

        /// <summary>
        /// 确定给定的exe文件的全路径。（在Path环境变量里依次查找，直到找到）
        /// </summary>
        /// <param name="exeName">EXE文件名，如"java.exe"</param>
        /// <returns>EXE文件的全路径</returns>
        public static string BuildExeFullPath(string exeName) {
            if (File.Exists(exeName)) {
                return exeName;
            }
            var enviromentPath = Environment.GetEnvironmentVariable("PATH");
            var paths = enviromentPath.Split(';');
            foreach (var path in paths) {
                var exePath = Path.Combine(path, exeName);
                if (File.Exists(exePath)) {
                    return exePath;
                }
            }
            throw new IOException(string.Format("可执行程序 \"{0}\" 不存在", exeName));
        }
    }
}
