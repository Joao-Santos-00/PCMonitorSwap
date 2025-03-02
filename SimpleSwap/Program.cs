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

            foreach (var monitor in displayMonitors)
            {
                var vcpCapabilities = _displayService.GetVCPCapabilities(monitor).ToList();
                var capabilities = _displayService.GetCapabilities(monitor);
                monitors.Add(new Monitor
                {
                    MonitorInfo = monitor,
                    VCPCapabilities = vcpCapabilities,
                    Capabilities = capabilities,
                });
            }

            foreach (var monitor in monitors)
            {
                if (monitor.MonitorInfo.Name == "\\\\.\\DISPLAY1")
                {
                    var capability = monitor.VCPCapabilities.Where(v => v.Name == "Input Source Select (0x60)").First();
                    if(capability.Value != 15)
                    {
                        _displayService.SetVCPCapability(monitor.MonitorInfo, (char)0x60, 15);  // Set to Display Port
                    }
                    else
                    {
                        _displayService.SetVCPCapability(monitor.MonitorInfo, (char)0x60, 16);  // Set to HDMI
                    }
                }
            }
        }
    }
}
