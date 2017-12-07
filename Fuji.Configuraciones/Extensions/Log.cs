using System;
using System.Configuration;
using System.IO;
using System.Management;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;

namespace Fuji.Configuraciones.Extensions
{
    public class Log
    {
        public static void EscribeLog(String Mensaje)
        {
            try
            {
                string LogDirectory = ConfigurationManager.AppSettings["LogDirectory"].ToString();
                if (!Directory.Exists(LogDirectory))
                    Directory.CreateDirectory(LogDirectory);
                DateTime Fecha = DateTime.Now;
                string content = "[" + Fecha.ToString("yyyy/MM/dd HH:mm:ss") + "]" + " " + Mensaje;
                string ArchivoLog = LogDirectory + "CONFIGURACION[" +  Fecha.ToShortDateString().Replace("/", "-") + "].txt";
                using (StreamWriter file = new StreamWriter(ArchivoLog, true))
                {
                    file.WriteLine(content);
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine("Error en la escritura de la bitácora: " + exc.Message);
            }
        }

        public static string GetLocalIPAddress()
        {
            string ip_add = "";
            try
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        ip_add =  ip.ToString();
                    }
                }
            }
            catch (Exception eglIP)
            {
                Log.EscribeLog("Error al obtener ip local: " + eglIP.Message);
            }
            return ip_add;
        }

        public static string getSubnetAddres()
        {
            string mascara = "";
            try
            {
                ManagementClass objMC;
                ManagementObjectCollection objMOC;
                string[] ipaddresses;
                string[] subnets;
                string[] gateways;
                string[] dns;
                string[] nicnames;
                string hostname;
                int i = 0;
                objMC = new ManagementClass("Win32_NetworkAdapterConfiguration");
                objMOC = objMC.GetInstances();
                foreach (ManagementObject objMO in objMOC)
                {
                    if (!(bool)objMO["ipEnabled"])
                        continue;
                    i = i + 1; // i = nº de NICs
                               //comboBox6.Items.Add(objMO["Caption"]);
                    ipaddresses = (string[])objMO["IPAddress"];
                    subnets = (string[])objMO["IPSubnet"];
                    if (subnets[0] != null)
                    {
                        mascara = subnets[0].ToString();
                    }
                    gateways = (string[])objMO["DefaultIPGateway"];
                    dns = (string[])objMO["DNSServerSearchOrder"];
                    hostname = (String)objMO["DNSHostName"];
                    nicnames = (String[])objMO["Caption"];
                    //mascara += mascara + "|" + (nicnames[0] != null ? nicnames[0].ToString() : "");
                    //groupBox1.Text = "Info TCP/IPv4: " + hostname;
                    //comboBox6.Items.Add(objMO["Caption"]);
                }

                
            }
            catch(Exception egsu)
            {
                Log.EscribeLog("Existe un error al obtener la mascara de red: " + egsu.Message);
            }
            return mascara;
        }

        public static bool PingHost(string nameOrAddress)
        {
            bool pingable = false;
            Ping pinger = new Ping();
            try
            {
                PingReply reply = pinger.Send(nameOrAddress);
                pingable = reply.Status == IPStatus.Success ? true : false;
            }
            catch (PingException)
            {
                // Discard PingExceptions and return false;
            }
            return pingable;
        }
    }
}
