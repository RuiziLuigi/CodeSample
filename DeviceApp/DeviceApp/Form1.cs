using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.IO;
using System.Runtime.InteropServices;
using System.Management;
using System.Threading;
using Microsoft.Win32;

namespace WindowsFormsApplication1
{
    delegate void UpdateView(string strVID, string strPID, string strHub);

    public partial class Form1 : Form
    {
        /// <summary>
        /// イベント感知（デバイス変更時）
        /// </summary>
        private const int WM_DEVICECHANGE = 0x0219;

        /// <summary>
        /// デバイスイベント種別
        /// </summary>
        private enum DBT
        {
            /// <summary>
            /// デバイス使用可能
            /// </summary>
            DBT_DEVICEARRIVAL = 0x8000,
            /// <summary>
            /// デバイス停止要求
            /// </summary>
            DBT_DEVICEQUERYREMOVE = 0x8001,
            /// <summary>
            /// デバイス停止要求失敗
            /// </summary>
            DBT_DEVICEQUERYREMOVEFAILED = 0x8002,
            /// <summary>
            /// デバイス停止中
            /// </summary>
            DBT_DEVICEREMOVEPENDING = 0x8003,
            /// <summary>
            /// デバイス停止
            /// </summary>
            DBT_DEVICEREMOVECOMPLETE = 0x8004,
        }

        /// <summary>
        /// ボリューム情報構造体
        /// </summary>
        private struct DEV_BROADCAST_VOLUME
        {
            /// <summary>
            /// サイズ
            /// </summary>
            public uint dbcv_size;
            /// <summary>
            /// 種別
            /// </summary>
            public uint dbcv_devicetype;
            /// <summary>
            /// 状態可否
            /// </summary>
            public uint dbcv_reserved;
            /// <summary>
            /// ボリューム名
            /// </summary>
            public uint dbcv_unitmask;

            /// <summary>
            /// パラメータセット
            /// </summary>
            /// <param name="a"></param>
            /// <param name="b"></param>
            /// <param name="c"></param>
            /// <param name="d"></param>
            public DEV_BROADCAST_VOLUME(uint a, uint b, uint c, uint d)
            {
                this.dbcv_size = a;
                this.dbcv_devicetype = b;
                this.dbcv_reserved = c;
                this.dbcv_unitmask = d;
            }
        }


        /// <summary>
        /// ドライブ
        /// </summary>
        private char m_cDrive = '\0';

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Windowsイベント処理
        /// </summary>
        /// <param name="m">イベントメッセージ</param>
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                // デバイス変更時イベント
                case WM_DEVICECHANGE:
                    // 変更時イベント
                    switch ((DBT)m.WParam.ToInt32())
                    {
                        // デバイス開始時処理
                        case DBT.DBT_DEVICEARRIVAL:
                            // 初回実行なら
                            if (this.driveText.Text == string.Empty)
                            {
                                // とりあえず実行中にする   
                                Application.UseWaitCursor = true;

                                DEV_BROADCAST_VOLUME volume = (DEV_BROADCAST_VOLUME)Marshal.PtrToStructure(m.LParam, typeof(DEV_BROADCAST_VOLUME));
                                // ビットが立っているところを取得
                                this.m_cDrive = 'A';
                                for (int iIdx = 0; iIdx < 25; iIdx++)
                                {
                                    if (volume.dbcv_unitmask >> iIdx == 1)
                                    {
                                        break;
                                    }
                                    this.m_cDrive++;
                                }
                                Thread thread = new Thread(GetUSBPort);
                                thread.Start();
                            }
                            else
                            {
                                MessageBox.Show("起動につき一度しか使用できません。アプリケーションを再起動してください");
                            }
                            break;
                        // デバイス停止時処理
                        case DBT.DBT_DEVICEREMOVECOMPLETE:
                            break;
                    }
                    break;
            }
            base.WndProc(ref m);
        }

        /// <summary>
        /// 画面更新
        /// </summary>
        /// <param name="strVID">ベンダーid</param>
        /// <param name="strPID">プロダクトid</param>
        /// <param name="strHub">ハブ</param>
        private void updateScreen(string strVID,string strPID, string strHub)
        {
            if (this.InvokeRequired)
            {
                UpdateView uv = new UpdateView(updateScreen);
                this.Invoke(uv, new string[] { strVID, strPID, strHub });
            }
            else
            {
                this.driveText.Text = this.m_cDrive.ToString();
                this.vidText.Text = strVID;
                this.pidText.Text = strPID;
                this.portText.Text = strHub;
                // 実行中を終了
                Application.UseWaitCursor = false;
            }
        }

        /// <summary>
        /// USBからベンダーIDとプロダクトID取得
        /// </summary>
        private void GetUSBPort()
        {
            //ドライブが装着された時の処理を書く
            // ドライバ認識まで待機
            bool blFin = false;
            while (!blFin)
            {
                Thread.Sleep(1000);
                DriveInfo di = new DriveInfo(this.m_cDrive.ToString());
                if (di.IsReady)
                {
                    blFin = true;
                    Thread.Sleep(1000);
                }
            }
            try
            {
                ManagementObjectSearcher searcher = null;
                searcher =
                    new ManagementObjectSearcher("root\\CIMV2",
                        "SELECT * FROM Win32_LogicalDiskToPartition");

                string strDeviceID = string.Empty;

                foreach (ManagementObject queryObj in searcher.Get())
                {
                    ManagementObject buf = new ManagementObject(queryObj["Dependent"].ToString());
                    if (buf["DeviceID"].ToString() == this.m_cDrive.ToString() + ":")
                    {
                        ManagementObject buf2 = new ManagementObject(queryObj["Antecedent"].ToString());
                        strDeviceID = buf2["DeviceID"].ToString();
                    }
                }

                searcher =
                    new ManagementObjectSearcher("root\\CIMV2",
                        "SELECT * FROM Win32_DiskPartition WHERE DeviceID='" + strDeviceID + "'");
                string strDiskIdx = string.Empty;
                foreach (ManagementObject queryObj in searcher.Get())
                {
                    strDiskIdx = queryObj["DiskIndex"].ToString();
                    break;
                }

                searcher =
                    new ManagementObjectSearcher("root\\CIMV2",
                    "SELECT * FROM Win32_DiskDrive WHERE Index='" + strDiskIdx + "'");
                string strPNPDeviceID = string.Empty;
                foreach (ManagementObject queryObj in searcher.Get())
                {
                    strPNPDeviceID = queryObj["PNPDeviceID"].ToString();
                    Console.WriteLine("DeviceID:" + queryObj["DeviceID"]);
                    Console.WriteLine("PNPDeviceID:" + queryObj["PNPDeviceID"]);
                }

                searcher =
                    new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_USBControllerDevice");


                string strContainerID = string.Empty;
                foreach (ManagementObject queryObj in searcher.Get())
                {
                    ManagementObject obj = new ManagementObject(queryObj["Dependent"].ToString());
                    if (obj["DeviceID"].ToString() == strPNPDeviceID)
                    {
                        RegistryKey rKey = Registry.LocalMachine.OpenSubKey(
                            "SYSTEM\\CurrentControlSet\\Enum\\" + strPNPDeviceID);

                        strContainerID = (string)rKey.GetValue("ContainerID");
                        rKey.Close();
                        break;
                    }
                }
                blFin = false;
                RegistryKey rk = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Enum\\USB");
                foreach (string strBuf in rk.GetSubKeyNames())
                {
                    RegistryKey rk2 = Registry.LocalMachine.OpenSubKey(
                            "SYSTEM\\CurrentControlSet\\Enum\\USB\\" + strBuf);
                    foreach (string strBuf2 in rk2.GetSubKeyNames())
                    {
                        RegistryKey rk3 = Registry.LocalMachine.OpenSubKey(
                            "SYSTEM\\CurrentControlSet\\Enum\\USB\\" + strBuf + "\\" + strBuf2);
                        if (rk3.GetValue("ContainerID").ToString() == strContainerID)
                        {
                            string[] strHardware = (string[])rk3.GetValue("HardwareID");
                            StringBuilder sb = new StringBuilder();
                            if (strHardware.Length > 0)
                            {
                                int iPid = strHardware[0].IndexOf("PID_");
                                int iVid = strHardware[0].IndexOf("VID_");
                                string strHubPort = (string)rk3.GetValue("LocationInformation");
                                updateScreen(strHardware[0].Substring(iVid + 4, 4), strHardware[0].Substring(iPid + 4, 4), strHubPort);
                            }
                            blFin = true;
                        }
                        rk3.Close();
                        if (blFin)
                        {
                            break;
                        }
                    }
                    rk2.Close();
                    if (blFin)
                    {
                        break;
                    }
                }
                rk.Close();
            }
            catch (ManagementException e)
            {
                MessageBox.Show("エラー：" + e.Message);
            }

        }
    }
}
