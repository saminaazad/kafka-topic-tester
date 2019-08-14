using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Kafka.Topic.Tester
{
    public static class ShellHelper
    {
        public static string Exexute()
        {   
            string command = @"tools\avrogen.exe";
            string args = "-s test.asvc .";

            Process process = new Process();
            process.StartInfo.FileName = command;
            process.StartInfo.Arguments = args;
            process.Start();
            process.WaitForExit();
            return "";
        }
    }
}
