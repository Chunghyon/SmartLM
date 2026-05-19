using AutoUpdaterDotNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoNetDrive.Protocol.Fingerprint.Test.Common
{
    public class AutoUpdaterTool
    {
        Action _action;

        public AutoUpdaterTool(Action action)
        {
            _action = action;
            AutoUpdater.CheckForUpdateEvent += AutoUpdater_CheckForUpdateEvent;
        }

        public void Start()
        {
            AutoUpdater.Start("http://oss2.pc15.net/ToolDownload/Update/FaceDebugToolForNet/update.xml");
        }


        private void AutoUpdater_CheckForUpdateEvent(UpdateInfoEventArgs args)
        {
            if (args.Error == null)
            {
                if (args.IsUpdateAvailable)//需要更新
                {
                    try
                    {
                        if (AutoUpdater.DownloadUpdate(args))
                        {
                            Application.Exit();
                            return;
                        }
                    }
                    catch
                    {//更新下载失败

                    }

                }
            }
            _action?.Invoke();
        }
    }
}
