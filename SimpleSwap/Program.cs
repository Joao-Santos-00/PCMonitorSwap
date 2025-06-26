using DDCCI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleSwap
{
    internal class Program
    {
        private class Monitor
        {
            public MonitorInfo MonitorInfo { get; set; }

            public string Capabilities { get; set; }

            public IEnumerable<VCPCapability> VCPCapabilities { get; set; }
        }

        static void Main(string[] args)
        {
            DisplayService _displayService = new DisplayService();
            var displayMonitors = _displayService.GetMonitors();
            List<Monitor> monitors = new List<Monitor>();
            List<VCPCapability> vcpCapabilities = new List<VCPCapability>();
            VCPCapability vcp = new VCPCapability();
            string capabilities;


            //foreach (var monitor in displayMonitors) { 
            //    try
            //    {
            //        vcpCapabilities = _displayService.GetVCPCapabilities(monitor).ToList();
            //        capabilities = _displayService.GetCapabilities(monitor);
            //        monitors.Add(new Monitor
            //        {
            //            MonitorInfo = monitor,
            //            VCPCapabilities = vcpCapabilities,
            //            Capabilities = capabilities,
            //        });
            //    }
            //    catch 
            //    {
            //        continue;
            //    }
            //}


            foreach (var monitor in displayMonitors)
            {
                try
                {
                    vcp = _displayService.GetVCPCapability(monitor, (char)0x60);
                    vcpCapabilities.Add(vcp);
                    monitors.Add(new Monitor { MonitorInfo = monitor });
                }
                catch
                {
                    continue;
                }
            }

            if (vcp.OptCode != (char)0x60) return;        // This is just a cop out. There is clearly a better way to identify the monitor of interest.
            else
            {
                var monitor = monitors[0];          
                //var capability = monitor.VCPCapabilities.Where(v => v.Name == "Input Source Select (0x60)").First();
                if (vcp.Value != 17)
                {
                    _displayService.SetVCPCapability(monitor.MonitorInfo, (char)0x60, 17);  // Set to HDMI1
                }
                else
                {
                    _displayService.SetVCPCapability(monitor.MonitorInfo, (char)0x60, 18);  // Set to HDMI2
                }
            }
            
        }
    }
}
