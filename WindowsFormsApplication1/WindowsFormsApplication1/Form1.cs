using System;
using Microsoft.Win32;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            string NetmanMachine = (string)Registry.GetValue(keyname,
            "NetmanMachine",
            "Return this default if NetmanMachine does not exist.");
            textBoxServ1.Text = NetmanMachine;
        }

        private const string userRoot = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Wow6432Node\\Schneider Electric\\Power Monitoring Expert\\8.1";
        private const string userRoot2 = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Wow6432Node\\Schneider Electric\\Common";
        private const string keyname = userRoot;
        private const string keyname1 = userRoot + "\\" + "Databases";
        private const string keyname2 = userRoot2 + "\\" + "License";

        private void buttonChan(object sender, EventArgs e)
        {
            closeprocess();
            write();
            read();
        }

        public void closeprocess()
        {
            Console.WriteLine("Kill process");          
            foreach (Process proceso in Process.GetProcesses())
            {
                //Console.WriteLine(proceso.ProcessName);
                if (proceso.ProcessName == "vista")
                {
                    Console.WriteLine("Wait...");
                    sleep();
                    proceso.Kill();
                    Console.WriteLine(proceso.ProcessName + " finalizado!!!");
                }

                if (proceso.ProcessName == "ManagementConsole")
                {
                    Console.WriteLine("Wait...");
                    sleep();
                    proceso.Kill();
                    Console.WriteLine(proceso.ProcessName + " finalizado!!!");
                }
            }       
        }

        private void sleep()
        {

            int millisecondsToWait = 100;
            Stopwatch stopwatch = Stopwatch.StartNew();
            while (true)
            {
                if (stopwatch.ElapsedMilliseconds >= millisecondsToWait)
                {
                    break;
                }
                Thread.Sleep(2000);
            }
        }

        public void read()
        {
 
            string NetmanMachine = (string)Registry.GetValue(keyname,
            "NetmanMachine",
            "Return this default if NetmanMachine does not exist.");    
            labelNetmanMachine.Text = NetmanMachine;
            
            string PrimaryMachine = (string)Registry.GetValue(keyname,
            "PrimaryMachine",
            "Return this default if PrimaryMachine does not exist.");  
            labelPrimaryMachine.Text = PrimaryMachine;

            string Root2 = (string)Registry.GetValue(keyname,
            "Root2",
            "Return this default if Root2 does not exist."); 
            labelRoot2.Text = Root2;

            string IONServer = (string)Registry.GetValue(keyname1,
            "IONServer",
            "Return this default if IONServer does not exist.");
            labelIONServer.Text = IONServer;

            string NOMServer = (string)Registry.GetValue(keyname1,
            "NOMServer",
            "Return this default if NOMServer does not exist.");
            labelNOMServer.Text = NOMServer;

            string SYSLOGServer = (string)Registry.GetValue(keyname1,
            "SYSLOGServer",
            "Return this default if SYSLOGServer does not exist.");
            labelSYSLOGServer.Text = SYSLOGServer;

            string LicenseServers = (string)Registry.GetValue(keyname2,
            "LicenseServers",
            "Return this default if LicenseServers does not exist.");
            labelLicense.Text = LicenseServers;
        }

        private void write()
        {

            string serv = null;         
            string val1 = textBoxServ1.Text;
            string val2 = textBoxServ2.Text;

            string NetmanMachine = (string)Registry.GetValue(keyname,
            "NetmanMachine",
            "Return this default if NetmanMachine does not exist.");

            if (NetmanMachine == val2)
            {
                serv = val1;               
            }
            else if (NetmanMachine == val1)
            {
                serv = val2;             
            }

            Registry.SetValue(keyname, "NetmanMachine", serv, RegistryValueKind.String);

            Registry.SetValue(keyname, "PrimaryMachine", serv, RegistryValueKind.String);

            Registry.SetValue(keyname, "Root2", "\\\\" + serv + "\\ION-Ent\\config", RegistryValueKind.String);

            //DATABASES

            Registry.SetValue(keyname1, "IONServer", serv + "\\ION", RegistryValueKind.String);

            Registry.SetValue(keyname1, "NOMServer", serv + "\\ION", RegistryValueKind.String);

            Registry.SetValue(keyname1, "SYSLOGServer", serv + "\\ION", RegistryValueKind.String);


            //LICENSE

            Registry.SetValue(keyname2, "LicenseServers", "27000@" + serv, RegistryValueKind.String);

            MessageBox.Show("Ok");
        }
            
    }
}
