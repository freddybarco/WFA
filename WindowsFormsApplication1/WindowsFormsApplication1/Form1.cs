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
using System.Globalization;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {

        private const string userRoot = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Wow6432Node\\Schneider Electric\\Power Monitoring Expert\\8.1";
        private const string userRoot2 = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Wow6432Node\\Schneider Electric\\Common";
        private const string keyname = userRoot;
        private const string keyname1 = userRoot + "\\" + "Databases";
        private const string keyname2 = userRoot2 + "\\" + "License";
        private bool Enable;

        public Form1()
        {
            InitializeComponent();
            string NetmanMachine = (string)Registry.GetValue(keyname, "NetmanMachine", "CannotRead");
            Enable = true;

        }

        private void buttonChan(object sender, EventArgs e)
        {
            if(closeprocess())
            {
                if (!read(false))
                    writeToLog("[Error] No se pudo leer algun registro");
                else
                {
                    write();
        
                }
                    
            }
            else
            {
                writeToLog("[Error]  Cierre las aplicaciones PME.");
            }
            
        }

        public void writeToLog(string s)
        {
            textBox1.Text += DateTime.Now.ToString(new CultureInfo("es-ES")) + " " + s + "\r\n";
            textBox1.SelectionStart = textBox1.Text.Length;
            textBox1.ScrollToCaret();

        }

        public Boolean closeprocess()
        {    
            foreach (Process proceso in Process.GetProcesses())
            {
                if(proceso.ProcessName=="vista" || proceso.ProcessName=="desginer" || proceso.ProcessName=="repgen" || proceso.ProcessName=="reportgen" || proceso.ProcessName=="ManagementConsole")
                {
                    return false;
                }
            }
            return true;
        }

        public bool read(bool inf)
        {
            bool success = false;
            try
            {
                string NetmanMachine = (string)Registry.GetValue(keyname, "NetmanMachine", "No se puede leer");
             
                string PrimaryMachine = (string)Registry.GetValue(keyname, "PrimaryMachine", "No se puede leer");
          
                string Root2 = (string)Registry.GetValue(keyname, "Root2", "No se puede leer");           

                string IONServer = (string)Registry.GetValue(keyname1, "IONServer", "No se puede leer");        
                
                string NOMServer = (string)Registry.GetValue(keyname1, "NOMServer", "No se puede leer");
                          
                string SYSLOGServer = (string)Registry.GetValue(keyname1, "SYSLOGServer", "No se puede leer");

                string LicenseServers = (string)Registry.GetValue(keyname2, "LicenseServers", "No se puede leer");

                if (inf)
                {
                    writeToLog("[Info] NetmanMachine: " + NetmanMachine);
                    writeToLog("[Info] PrimaryMachine: " + PrimaryMachine);
                    writeToLog("[Info] Root2: " + Root2);
                    writeToLog("[Info] IONServer: " + IONServer);
                    writeToLog("[Info] NOMServer: " + NOMServer);
                    writeToLog("[Info] SYSLOGServer: " + SYSLOGServer);
                    writeToLog("[Info] LicenseServers: " + LicenseServers);
                }


                success = true;
    
            }
            catch(Exception ex)
            {
                writeToLog("[Exception] " + ex.ToString());
            }
            if (success)
                return true;
            else
                return false;

        }

        private void write()
        {
  
            string serv = textBoxServ1.Text;

            if (String.IsNullOrEmpty(serv))
            {
                writeToLog("[Error] Ingrese nombre del servidor");
            }

            else
            {

                string NetmanMachine = (string)Registry.GetValue(keyname, "NetmanMachine", "CannotRead");
                if (NetmanMachine != "CannotRead")
                {
                    try
                    {
                        Registry.SetValue(keyname, "NetmanMachine", serv, RegistryValueKind.String);

                        Registry.SetValue(keyname, "PrimaryMachine", serv, RegistryValueKind.String);

                        Registry.SetValue(keyname, "Root2", "\\\\" + serv + "\\ION-Ent\\config", RegistryValueKind.String);

                        //DATABASES

                        Registry.SetValue(keyname1, "IONServer", serv + "\\ION", RegistryValueKind.String);

                        Registry.SetValue(keyname1, "NOMServer", serv + "\\ION", RegistryValueKind.String);

                        Registry.SetValue(keyname1, "SYSLOGServer", serv + "\\ION", RegistryValueKind.String);

                        //LICENSE

                        Registry.SetValue(keyname2, "LicenseServers", "27000@" + serv, RegistryValueKind.String);

                        read(true);
                    }
                    catch(Exception ex)
            {
                writeToLog("[Exception] " + ex.ToString());
            }
                }
                else
                {
                    writeToLog("[Error] No se pudo encontrar el nombre del servidor actual");
                }

            }
        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox1.ScrollBars = ScrollBars.Both;
        }

        private void textBoxServ1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
