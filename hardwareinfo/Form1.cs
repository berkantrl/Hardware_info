using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace hardwareinfo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label3.Text = GetInfoHardware.GetProcessorId();
            label4.Text = GetInfoHardware.GetHDDSerialNo();
            label6.Text = GetInfoHardware.GetBoardMaker();
            label8.Text = GetInfoHardware.GetBIOSmaker();
            label10.Text = GetInfoHardware.GetPhysicalMemory();
            label12.Text = GetInfoHardware.GetCpuSpeedInGHz().ToString();
            label14.Text = GetInfoHardware.GetCPUManufacturer();
            label16.Text = GetInfoHardware.GetProcessorInformation();
            label32.Text = GetInfoHardware.GetOSInformation();
            label30.Text = GetInfoHardware.GetCurrentLanguage();
            label28.Text = GetInfoHardware.GetComputerName();

            bool Systembit = Environment.Is64BitOperatingSystem;
            if (Systembit)
            {
                label19.Text = "64";
            }
            else
            {
                label19.Text = "32";

            }

            long totalsize = 0;
            foreach (System.IO.DriveInfo label in System.IO.DriveInfo.GetDrives())
            {

                if (label.IsReady)
                {
                    totalsize += label.TotalSize;
                }
            }
            label21.Text = Convert.ToString(totalsize);

            int Processor = Environment.ProcessorCount;
            label23.Text = Convert.ToString(Processor);
        }
        private void label2_Click(object sender, EventArgs e)
        {

        }
    }

    
}

public static class GetInfoHardware
{
    public static String GetProcessorId()
    {

        ManagementClass mancl = new ManagementClass("win32_processor");
        ManagementObjectCollection manageobcl = mancl.GetInstances();
        String Id = String.Empty;
        foreach (ManagementObject myObj in manageobcl)
        {

            Id = myObj.Properties["processorID"].Value.ToString();
            break;
        }
        return Id;

    }
    public static String GetHDDSerialNo()
    {
        ManagementClass mangnmt = new ManagementClass("Win32_LogicalDisk");
        ManagementObjectCollection mcol = mangnmt.GetInstances();
        string result = "";
        foreach (ManagementObject strt in mcol)
        {
            result += Convert.ToString(strt["VolumeSerialNumber"]);
        }
        return result;
    }
    public static string GetMACAddress()
    {
        ManagementClass mancl = new ManagementClass("Win32_NetworkAdapterConfiguration");
        ManagementObjectCollection manageobcl = mancl.GetInstances();
        string MACAddress = String.Empty;
        foreach (ManagementObject myObj in manageobcl)
        {
            if (MACAddress == String.Empty)
            {
                if ((bool)myObj["IPEnabled"] == true) MACAddress = myObj["MacAddress"].ToString();
            }
            myObj.Dispose();
        }

        MACAddress = MACAddress.Replace(":", "");
        return MACAddress;
    }
    public static string GetBoardMaker()
    {

        ManagementObjectSearcher finder = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BaseBoard");

        foreach (ManagementObject wmi in finder.Get())
        {
            try
            {
                return wmi.GetPropertyValue("Manufacturer").ToString();
            }

            catch { }

        }

        return "Board Maker: Unknown";

    }
    public static string GetBoardProductId()
    {

        ManagementObjectSearcher finder = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BaseBoard");

        foreach (ManagementObject wmi in finder.Get())
        {
            try
            {
                return wmi.GetPropertyValue("Product").ToString();

            }

            catch { }

        }

        return "Product: Unknown";

    }
    public static string GetCdRomDrive()
    {

        ManagementObjectSearcher finder = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_CDROMDrive");

        foreach (ManagementObject wmi in finder.Get())
        {
            try
            {
                return wmi.GetPropertyValue("Drive").ToString();

            }

            catch { }

        }

        return "Sorry CD ROM Drive Letter: Unknown";

    }
    public static string GetBIOSmaker()
    {

        ManagementObjectSearcher finder = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BIOS");

        foreach (ManagementObject wmi in finder.Get())
        {
            try
            {
                return wmi.GetPropertyValue("Manufacturer").ToString();

            }

            catch { }

        }

        return "BIOS Maker: Unknown";

    }
    public static string GetBIOSserNo()
    {

        ManagementObjectSearcher finder = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BIOS");

        foreach (ManagementObject wmi in finder.Get())
        {
            try
            {
                return wmi.GetPropertyValue("SerialNumber").ToString();

            }

            catch { }

        }

        return "BIOS Serial Number: Unknown";

    }
    public static string GetBIOScaption()
    {

        ManagementObjectSearcher finder = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BIOS");

        foreach (ManagementObject wmi in finder.Get())
        {
            try
            {
                return wmi.GetPropertyValue("Caption").ToString();

            }
            catch { }
        }
        return "BIOS Caption: Unknown";
    }
    public static string GetAccountName()
    {

        ManagementObjectSearcher finder = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_UserAccount");

        foreach (ManagementObject wmi in finder.Get())
        {
            try
            {

                return wmi.GetPropertyValue("Name").ToString();
            }
            catch { }
        }
        return "Sorry, User Account Name: Unknown";

    }
    public static string GetPhysicalMemory()
    {
        ManagementScope osmanagesys = new ManagementScope();
        ObjectQuery myobjQry = new ObjectQuery("SELECT Capacity FROM Win32_PhysicalMemory");
        ManagementObjectSearcher oFinder = new ManagementObjectSearcher(osmanagesys, myobjQry);
        ManagementObjectCollection oCollection = oFinder.Get();

        long liveMSize = 0;
        long livecap = 0;

        foreach (ManagementObject obj in oCollection)
        {
            livecap = Convert.ToInt64(obj["Capacity"]);
            liveMSize += livecap;
        }
        liveMSize = (liveMSize / 1024) / 1024;
        return liveMSize.ToString() + "MB";
    }
    public static string GetNoRamSlots()
    {

        int MemSlots = 0;
        ManagementScope osmanagesys = new ManagementScope();
        ObjectQuery oQuery2 = new ObjectQuery("SELECT MemoryDevices FROM Win32_PhysicalMemoryArray");
        ManagementObjectSearcher oSearcher2 = new ManagementObjectSearcher(osmanagesys, oQuery2);
        ManagementObjectCollection oCollection2 = oSearcher2.Get();
        foreach (ManagementObject obj in oCollection2)
        {
            MemSlots = Convert.ToInt32(obj["MemoryDevices"]);

        }
        return MemSlots.ToString();
    }
    //c# get cpu information
    public static string GetCPUManufacturer()
    {
        string robotCpu = String.Empty;
        ManagementClass managemnt = new ManagementClass("Win32_Processor");
        ManagementObjectCollection objCol = managemnt.GetInstances();

        foreach (ManagementObject obj in objCol)
        {
            if (robotCpu == String.Empty)
            {
                robotCpu = obj.Properties["Manufacturer"].Value.ToString();
            }
        }
        return robotCpu;
    }
    //Get Computer Hardware Information CPU Clock
    public static int GetCPUCurrentClockSpeed()
    {
        int speedCpuTimestemp = 0;
        ManagementClass managemnt = new ManagementClass("Win32_Processor");
        ManagementObjectCollection objCol = managemnt.GetInstances();
        foreach (ManagementObject obj in objCol)
        {
            if (speedCpuTimestemp == 0)
            {
                speedCpuTimestemp = Convert.ToInt32(obj.Properties["CurrentClockSpeed"].Value.ToString());
            }
        }
        return speedCpuTimestemp;
    }
    public static string GetDefaultIPGateway()
    {

        ManagementClass managemnt = new ManagementClass("Win32_NetworkAdapterConfiguration");
        ManagementObjectCollection objCol = managemnt.GetInstances();
        string gateway = String.Empty;
        foreach (ManagementObject obj in objCol)
        {
            if (gateway == String.Empty)  // only return MAC Address from first card
            {
                if ((bool)obj["IPEnabled"] == true)
                {
                    gateway = obj["DefaultIPGateway"].ToString();
                }
            }
            obj.Dispose();
        }
        gateway = gateway.Replace(":", "");
        return gateway;
    }
    public static double? GetCpuSpeedInGHz()
    {
        double? GHz = null;
        using (ManagementClass mancl = new ManagementClass("Win32_Processor"))
        {
            foreach (ManagementObject myObj in mancl.GetInstances())
            {
                GHz = 0.001 * (UInt32)myObj.Properties["CurrentClockSpeed"].Value;
                break;
            }
        }
        return GHz;
    }
    public static string GetCurrentLanguage()
    {

        ManagementObjectSearcher finder = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BIOS");

        foreach (ManagementObject wmi in finder.Get())
        {
            try
            {
                return wmi.GetPropertyValue("CurrentLanguage").ToString();

            }

            catch { }

        }

        return "BIOS Maker: Unknown";

    }
    public static string GetOSInformation()
    {
        ManagementObjectSearcher finder = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem");
        foreach (ManagementObject wmi in finder.Get())
        {
            try
            {
                return ((string)wmi["Caption"]).Trim() + ", " + (string)wmi["Version"] + ", " + (string)wmi["OSArchitecture"];
            }
            catch { }
        }
        return "Sorry BIOS Maker: Unknown";
    }
    public static String GetProcessorInformation()
    {
        ManagementClass mancl = new ManagementClass("win32_processor");
        ManagementObjectCollection manageobcl = mancl.GetInstances();
        String info = String.Empty;
        foreach (ManagementObject myObj in manageobcl)
        {
            string name = (string)myObj["Name"];
            name = name.Replace("(TM)", "™").Replace("(tm)", "™").Replace("(R)", "®").Replace("(r)", "®").Replace("(C)", "©").Replace("(c)", "©").Replace("    ", " ").Replace("  ", " ");

            info = name + ", " + (string)myObj["Caption"] + ", " + (string)myObj["SocketDesignation"];

        }
        return info;
    }
    public static String GetComputerName()
    {
        ManagementClass mancl = new ManagementClass("Win32_ComputerSystem");
        ManagementObjectCollection manageobcl = mancl.GetInstances();
        String info = String.Empty;
        foreach (ManagementObject myObj in manageobcl)
        {
            info = (string)myObj["Name"];
            //myObj.Properties["Name"].Value.ToString();
            //break;
        }
        return info;
    }

}


