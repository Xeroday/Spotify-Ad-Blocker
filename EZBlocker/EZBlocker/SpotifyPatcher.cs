using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Core;
using System.Threading;

namespace EZBlocker
{
    class SpotifyPatcher
    {
        private readonly string spotifyPath = Environment.GetEnvironmentVariable("APPDATA") + @"\Spotify\spotify.exe";
        private readonly string targetPath = Environment.GetEnvironmentVariable("APPDATA") + @"\Spotify\Apps\zlink.spa";
        private readonly string tmpPath = Environment.GetEnvironmentVariable("TMP") + @"\EZBlocker\";
        private readonly string backupPath = "";

        public SpotifyPatcher()
        {
            Directory.CreateDirectory(tmpPath);
            backupPath = tmpPath + "backup.spa";
        }

        public bool Patch()
        {
            Dictionary<String, String> patches = new Dictionary<string, string>
            {
                { "openProductUpgradePage", "openWebsite" },
                { "UPGRADE_LABEL", "'EZBlocker'" },
                { "UPGRADE_TOOLTIP_TEXT", "'Open EZBlocker Website'" },
                { "<script type=\"text/javascript\" src=\"/zlink.bundle.js\"></script>", @"<script src=/zlink.bundle.js></script><script>function openWebsite(){window.open('{WEBSITE}')}w=new Worker('worker.js'),w.onmessage=function(e){w.postMessage(document.getElementById('player-button-next').disabled)},w.postMessage('')</script>".Replace("{WEBSITE}", Main.website) }
            };
            string worker = @"var sendEZB=function(e){var t=new XMLHttpRequest;t.open('GET','http://localhost:19691/'+e,!0),t.send()};self.addEventListener('message',function(e){sendEZB(e.data),setTimeout(function(){postMessage('ready')},300)},!1);";

            string workingDir = Path.Combine(tmpPath, new Random().Next(999, 9999).ToString());
            try
            {
                System.IO.Compression.ZipFile.ExtractToDirectory(targetPath, workingDir);

                // Check if already patched
                if (File.Exists(Path.Combine(workingDir, "worker.js"))) {
                    return true;
                }

                // Add web worker
                File.WriteAllText(Path.Combine(workingDir, "worker.js"), worker);

                // Patch index
                string patchFile = Path.Combine(workingDir, "index.html");
                string contents = File.ReadAllText(patchFile, System.Text.Encoding.UTF8);
                foreach (KeyValuePair<string, string> patch in patches)
                {
                    contents = contents.Replace(patch.Key, patch.Value);
                }
                File.WriteAllText(patchFile, contents, System.Text.Encoding.UTF8);

                string patchedPath = Path.Combine(tmpPath, "zlink.spa");
                File.Delete(patchedPath);
                ZipCompress(patchedPath, workingDir);

                File.Replace(patchedPath, targetPath, backupPath);

                Directory.Delete(workingDir, true);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                Directory.Delete(workingDir, true);
                Restore();
                return false;
            }

            // Restart Spotify if running
            try
            {
                foreach (Process p in Process.GetProcessesByName("spotify"))
                {
                    p.Kill();
                }
            } catch (Exception e) {
                Debug.WriteLine(e);
            }

            Thread.Sleep(3000);
            Process.Start(spotifyPath);

            return true;
        }

        public bool Restore()
        {
            if (File.Exists(backupPath))
            {
                File.Copy(backupPath, targetPath, true);
                return true;
            }
            return false;
        }

        private void ZipCompress(string outPathname, string folderName)
        {
            FileStream fsOut = File.Create(outPathname);
            ZipOutputStream zipStream = new ZipOutputStream(fsOut);

            // This setting will strip the leading part of the folder path in the entries, to
            // make the entries relative to the starting folder.
            // To include the full path for each entry up to the drive root, assign folderOffset = 0.
            int folderOffset = folderName.Length + (folderName.EndsWith("\\") ? 0 : 1);

            CompressFolder(folderName, zipStream, folderOffset);

            zipStream.IsStreamOwner = true;
            zipStream.Close();
        }

        private void CompressFolder(string path, ZipOutputStream zipStream, int folderOffset)
        {
            foreach (string filename in Directory.GetFiles(path))
            {
                FileInfo fi = new FileInfo(filename);

                string entryName = filename.Substring(folderOffset); // Makes the name in zip based on the folder
                entryName = ZipEntry.CleanName(entryName); // Removes drive from name and fixes slash direction
                ZipEntry newEntry = new ZipEntry(entryName)
                {
                    DateTime = fi.LastWriteTime, // Note the zip format stores 2 second granularity
                    Size = fi.Length
                };

                zipStream.PutNextEntry(newEntry);

                // Zip the file in buffered chunks
                // the "using" will close the stream even if an exception occurs
                byte[] buffer = new byte[4096];
                using (FileStream streamReader = File.OpenRead(filename))
                {
                    StreamUtils.Copy(streamReader, zipStream, buffer);
                }
                zipStream.CloseEntry();
            }
            string[] folders = Directory.GetDirectories(path);
            foreach (string folder in folders)
            {
                CompressFolder(folder, zipStream, folderOffset);
            }
        }
    }
}
