using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;

namespace Tools {

    public class AutoClosingMessageBox {
        const int WM_CLOSE = 0x0010;
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);
        System.Threading.Timer _timeoutTimer;
        string _caption;

        AutoClosingMessageBox(string text, string caption, int timeout) {
            _caption = caption;
            _timeoutTimer = new System.Threading.Timer(OnTimerElapsed,
                null, timeout, System.Threading.Timeout.Infinite);
            MessageBox.Show(text, caption);
        }
        
        public static void Show(string text, string caption, int timeout) {
            new AutoClosingMessageBox(text, caption, timeout);
        }
        
        void OnTimerElapsed(object state) {
            IntPtr mbWnd = FindWindow(null, _caption);
            if (mbWnd != IntPtr.Zero)
                SendMessage(mbWnd, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
            _timeoutTimer.Dispose();
        }
        
    }

    public class Funcs {
        public static DialogResult ShowDialog(ref string input, string Title, bool Input=true) {
            System.Drawing.Size size = new System.Drawing.Size(450, 80);
            Form inputBox = new Form();

            inputBox.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            inputBox.StartPosition = FormStartPosition.CenterParent;
            inputBox.ClientSize = size;
            inputBox.Text = Title;

            System.Windows.Forms.TextBox textBox = new TextBox();
            System.Windows.Forms.Label lblbox = new Label();
            if (Input == false) {
                lblbox.Size = new System.Drawing.Size(size.Width - 10, 23);
                lblbox.Location = new System.Drawing.Point(5, 15);
                lblbox.Text = input;
                inputBox.Controls.Add(lblbox);
            }
            else {
                textBox.Size = new System.Drawing.Size(size.Width - 10, 46);
                textBox.Location = new System.Drawing.Point(5, 5);
                textBox.Text = input;
                inputBox.Controls.Add(textBox);
            }

            Button okButton = new Button();
            okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            okButton.Name = "okButton";
            okButton.Size = new System.Drawing.Size(75, 23);
            okButton.Text = "&OK";
            okButton.Location = new System.Drawing.Point(size.Width - 80 - 80, 39);
            inputBox.Controls.Add(okButton);

            Button cancelButton = new Button();
            cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new System.Drawing.Size(75, 23);
            cancelButton.Text = "&Cancel";
            cancelButton.Location = new System.Drawing.Point(size.Width - 80, 39);
            inputBox.Controls.Add(cancelButton);

            inputBox.AcceptButton = okButton;
            inputBox.CancelButton = cancelButton;

            DialogResult result = inputBox.ShowDialog();
            input = textBox.Text;
            return result;    
        }

        public static void LoadCfg(ref List<string> loadCams) {
            try {
                loadCams = new List<string>(File.ReadAllLines("IPCams.cfg"));
            }
            catch (Exception e) { }
             
         }

        public static void SaveCfg(List<IPCams.Janela> cam) {
            if (cam.Count != 0) {
                List<string> lines=new List<string>();
                for (int i = 0; i < cam.Count; i++) {
                    lines.Add(cam[i].Text + "," + (cam[i].On?"1":"0"));
                }
                File.WriteAllLines("IPCams.cfg",lines);
            }
        }
    }

    public class IPFuncs {
        //[DllImport("user32.dll", SetLastError = true)]
        //static extern int MessageBoxTimeout(IntPtr hwnd, String text, String title, uint type, Int16 wLanguageId, Int32 milliseconds);
        
        public static void Ping255(string localIP) {
            for (int i=1; i<256; i++) {
                Ping p = new Ping();
                p.SendAsync(localIP.Substring(0,localIP.LastIndexOf(".") + 1) + i, 1);
                //p.Send(localIP.Substring(0, localIP.LastIndexOf(".") + 1) + i, 1);
                Console.WriteLine(i);
                //p.SendAsyncCancel(); //cagaiting mode
            }
        }
        public static string GetLocalIPAddress() {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList) {
                if (ip.AddressFamily == AddressFamily.InterNetwork) {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        internal static int MessageBoxTimeout(IntPtr intPtr, string v1, string v2, int v3, int v4, int v5) {
            throw new NotImplementedException();
        }
    }

    static class  ARP {
        // The max number of physical addresses.
        const int MAXLEN_PHYSADDR = 8;

        // Define the MIB_IPNETROW structure.
        [StructLayout(LayoutKind.Sequential)]
        struct MIB_IPNETROW {
            [MarshalAs(UnmanagedType.U4)]
            public int dwIndex;
            [MarshalAs(UnmanagedType.U4)]
            public int dwPhysAddrLen;
            [MarshalAs(UnmanagedType.U1)]
            public byte mac0;
            [MarshalAs(UnmanagedType.U1)]
            public byte mac1;
            [MarshalAs(UnmanagedType.U1)]
            public byte mac2;
            [MarshalAs(UnmanagedType.U1)]
            public byte mac3;
            [MarshalAs(UnmanagedType.U1)]
            public byte mac4;
            [MarshalAs(UnmanagedType.U1)]
            public byte mac5;
            [MarshalAs(UnmanagedType.U1)]
            public byte mac6;
            [MarshalAs(UnmanagedType.U1)]
            public byte mac7;
            [MarshalAs(UnmanagedType.U4)]
            public int dwAddr;
            [MarshalAs(UnmanagedType.U4)]
            public int dwType;
        }

        // Declare the GetIpNetTable function.
        [DllImport("IpHlpApi.dll")]
        [return: MarshalAs(UnmanagedType.U4)]
        static extern int GetIpNetTable(
           IntPtr pIpNetTable,
           [MarshalAs(UnmanagedType.U4)]
         ref int pdwSize,
           bool bOrder);

        [DllImport("IpHlpApi.dll", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern int FreeMibTable(IntPtr plpNetTable);

        // The insufficient buffer error.
        const int ERROR_INSUFFICIENT_BUFFER = 122;



        public static string GetIPfromMAC(string FindMAC) {
            // The number of bytes needed.
            int bytesNeeded = 0;
            string IP = "";
            // The result from the API call.
            int result = GetIpNetTable(IntPtr.Zero, ref bytesNeeded, false);

            // Call the function, expecting an insufficient buffer.
            if (result != ERROR_INSUFFICIENT_BUFFER) {
                // Throw an exception.
                throw new Win32Exception(result);
            }

            // Allocate the memory, do it in a try/finally block, to ensure
            // that it is released.
            IntPtr buffer = IntPtr.Zero;

            // Try/finally.
            try {
                // Allocate the memory.
                buffer = Marshal.AllocCoTaskMem(bytesNeeded);

                // Make the call again. If it did not succeed, then
                // raise an error.
                result = GetIpNetTable(buffer, ref bytesNeeded, false);

                // If the result is not 0 (no error), then throw an exception.
                if (result != 0) {
                    // Throw an exception.
                    throw new Win32Exception(result);
                }

                // Now we have the buffer, we have to marshal it. We can read
                // the first 4 bytes to get the length of the buffer.
                int entries = Marshal.ReadInt32(buffer);

                // Increment the memory pointer by the size of the int.
                IntPtr currentBuffer = new IntPtr(buffer.ToInt64() +
                   Marshal.SizeOf(typeof(int)));

                // Allocate an array of entries.
                MIB_IPNETROW[] table = new MIB_IPNETROW[entries];

                // Cycle through the entries.
                for (int index = 0; index < entries; index++) {
                    // Call PtrToStructure, getting the structure information.
                    table[index] = (MIB_IPNETROW)Marshal.PtrToStructure(new
                       IntPtr(currentBuffer.ToInt64() + (index *
                       Marshal.SizeOf(typeof(MIB_IPNETROW)))), typeof(MIB_IPNETROW));
                }

                for (int index = 0; index < entries; index++) {
                    MIB_IPNETROW row = table[index];
                    IPAddress ip = new IPAddress(BitConverter.GetBytes(row.dwAddr));

                    string addr = row.mac0.ToString("X2") + '-' +
                                  row.mac1.ToString("X2") + '-' +
                                  row.mac2.ToString("X2") + '-' +
                                  row.mac3.ToString("X2") + '-' +
                                  row.mac4.ToString("X2") + '-' +
                                  row.mac5.ToString("X2") ;
                    //Console.Write("IP:" + ip.ToString() + "\t\tMAC:" + addr);
                    if (addr == FindMAC.ToUpper()) {
                        //Console.Write("-----------------");
                        IP = ip.ToString();
                        break;
                    }
                    Console.WriteLine();

                }
            }
            finally {
                // Release the memory.
                FreeMibTable(buffer);
            }
            return IP;
        }
    }


    public class IPMacMapper {
        private static List<IPAndMac> list;

        private static StreamReader ExecuteCommandLine(String file, String arguments = "") {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = true;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.FileName = file;
            startInfo.Arguments = arguments;

            Process process = Process.Start(startInfo);

            return process.StandardOutput;
        }

        private static void InitializeGetIPsAndMac() {
            if (list != null)
                return;

            var arpStream = ExecuteCommandLine("arp", "-a");
            List<string> result = new List<string>();
            while (!arpStream.EndOfStream) {
                var line = arpStream.ReadLine().Trim();
                result.Add(line);
            }

            list = result.Where(x => !string.IsNullOrEmpty(x) && (x.Contains("dynamic") || x.Contains("static")))
                .Select(x => {
                    string[] parts = x.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                    return new IPAndMac { IP = parts[0].Trim(), MAC = parts[1].Trim() };
                }).ToList();
        }

        public static string FindIPFromMacAddress(string macAddress) {
            InitializeGetIPsAndMac();
            IPAndMac item = list.SingleOrDefault(x => x.MAC == macAddress);
            if (item == null)
                return null;
            return item.IP;
        }

        public static string FindMacFromIPAddress(string ip) {
            InitializeGetIPsAndMac();
            IPAndMac item = list.SingleOrDefault(x => x.IP == ip);
            if (item == null)
                return null;
            return item.MAC;
        }

        private class IPAndMac {
            public string IP { get; set; }
            public string MAC { get; set; }
        }
    }
}