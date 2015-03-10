using System;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;

namespace HostSample
{
    class Program
    {
        // to run this application, specify a path to an item
        //  e.g.:
        //      c:\
        //      env:/computername
        //      hkcu:/software
        static void Main(string[] args)
        {
            if(args == null || args.Length == 0){
                Console.WriteLine("No arguments");
                return;
            }
            // create a RUNSPACE - this is used to maintain state between pipelines.
            //  e.g., variables, function definitions, etc
            using (var runspace = RunspaceFactory.CreateRunspace())
            {
                // open the runspace before you use it
                runspace.Open();

                // set the default runspace for the process;
                //  this is necessary for some features and cmdlets to work properly
                Runspace.DefaultRunspace = runspace;

                
                // create a POWERSHELL pipeline
                using (var powershell = PowerShell.Create())
                {
                    // assign the runspace to the pipeline
                    powershell.Runspace = runspace;

                    // build up the pipeline
                    powershell.AddCommand("get-item")
                        .AddParameter("path", args[0])
                        .AddCommand("format-table")
                        .AddCommand( "out-string");

                    // execute the pipeline
                    var results = powershell.Invoke();

                    // output the results
                    Console.WriteLine( results.FirstOrDefault() );
                }
            }
        }
    }
}
