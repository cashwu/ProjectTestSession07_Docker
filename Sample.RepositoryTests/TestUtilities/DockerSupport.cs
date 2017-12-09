using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Threading;

namespace Sample.RepositoryTests.TestUtilities
{
    /// <summary>
    /// Class DockerSupport.
    /// </summary>
    internal class DockerSupport
    {
        private static int _port;

        private static int Port
        {
            get
            {
                if (!_port.Equals(0))
                {
                    return _port;
                }

                Random rnd = new Random();
                int result = rnd.Next(49152, 65535);
                _port = result;
                return _port;
            }
        }

        internal static string ContainerId { get; set; }

        /// <summary>
        /// Stops the container.
        /// </summary>
        /// <param name="containerId">The container identifier.</param>
        public static void StopContainer(string containerId)
        {
            // 移除測試資料庫的 Container
            using (var powerShellinstance = PowerShell.Create())
            {
                powerShellinstance.AddScript($"docker stop {containerId}");

                var psOutput = powerShellinstance.Invoke();

                foreach (var outputItem in psOutput)
                {
                    if (outputItem != null)
                    {
                        Console.WriteLine(outputItem.BaseObject.ToString());
                    }
                }
            }
        }

        /// <summary>
        /// Creates the container.
        /// </summary>
        /// <param name="containerType">The Container Type (windows or linux)</param>
        /// <param name="databaseIp">The database ip.</param>
        /// <param name="containerId">The container identifier.</param>
        public static void CreateContainer(string containerType,
                                           out string databaseIp,
                                           out string containerId)
        {
            if (containerType.Equals("Windows", StringComparison.OrdinalIgnoreCase))
            {
                // 使用 Windows Container 服務
                CreateSqlServerContainerOnWindows(out databaseIp, out containerId);
            }
            else
            {
                CreateSqlServerContainerOnLinux(out databaseIp, out containerId);
            }
        }

        /// <summary>
        /// 使用 microsoft/mssql-server-linux 建立測試用的 SQL Server.
        /// </summary>
        /// <param name="databaseIp">The database ip.</param>
        /// <param name="containerId">The container identifier.</param>
        private static void CreateSqlServerContainerOnLinux(out string databaseIp,
                                                            out string containerId)
        {
            var runSpacePool = RunspaceFactory.CreateRunspacePool(1, 5);
            runSpacePool.Open();

            using (runSpacePool)
            {
                var powerShellCommands = new List<PowerShell>();
                var powerShellCommandResults = new List<IAsyncResult>();

                // 1.先停止並移除之前所建立的測試資料庫 sql-server
                var powerShellInstance1 = PowerShell.Create();
                powerShellInstance1.RunspacePool = runSpacePool;
                powerShellInstance1.AddScript($"docker stop {ContainerId}");
                powerShellCommands.Add(powerShellInstance1);
                powerShellCommandResults.Add(powerShellInstance1.BeginInvoke());

                // 2.使用 microsoft/sql-server-linux 建立測試資料庫 sql-server
                var powerShellInstance2 = PowerShell.Create();
                powerShellInstance2.RunspacePool = runSpacePool;
                powerShellInstance2.AddScript($"docker run --rm -d -e SA_PASSWORD=1q2w3e4r5t_ -e ACCEPT_EULA=Y -ti -p {Port}:1433 microsoft/mssql-server-linux");
                powerShellCommands.Add(powerShellInstance2);
                powerShellCommandResults.Add(powerShellInstance2.BeginInvoke());

                int i = 0;
                string message = string.Empty;

                foreach (var powerShellCommand in powerShellCommands)
                {
                    PSDataCollection<PSObject> results = powerShellCommand.EndInvoke(powerShellCommandResults[i]);
                    foreach (var result in results)
                    {
                        if (result != null)
                        {
                            message = result.BaseObject.ToString();
                        }
                    }

                    if (i.Equals(1))
                    {
                        ContainerId = message;
                    }
                    i++;
                }
            }

            // 3. 使用 docker logs 指令，查看在 container 的 sql-server 是否已經準備好

            int retryTimes = 30;
            bool ready = false;

            for (int i = 0; i < retryTimes; i++)
            {
                var powerShellInstance = PowerShell.Create();
                powerShellInstance.AddScript($"docker logs {ContainerId}");
                var executeResults = powerShellInstance.Invoke();
                foreach (var psObject in executeResults)
                {
                    if (psObject != null)
                    {
                        var message = psObject.BaseObject.ToString();
                        if (message.Contains("The default language (LCID 0) has been set for engine and full-text services"))
                        {
                            ready = true;
                        }
                    }
                }

                if (ready.Equals(false))
                {
                    Thread.Sleep(1000);
                }
                else
                {
                    Console.WriteLine($"wait {i} second");
                    break;
                }
            }

            databaseIp = $"127.0.0.1,{Port}";
            containerId = string.IsNullOrWhiteSpace(ContainerId) ? "" : ContainerId;
        }

        /// <summary>
        /// 使用 microsoft/mssql-server-windows-developer 建立測試用的 SQL Server.
        /// </summary>
        /// <param name="databaseIp">The database ip.</param>
        /// <param name="containerId">The container identifier.</param>
        private static void CreateSqlServerContainerOnWindows(out string databaseIp,
                                                              out string containerId)
        {
            var runSpacePool = RunspaceFactory.CreateRunspacePool(1, 5);
            runSpacePool.Open();

            using (runSpacePool)
            {
                var powerShellCommands = new List<PowerShell>();
                var powerShellCommandResults = new List<IAsyncResult>();

                // 1.先停止並移除之前所建立的測試資料庫 sql-server
                var powerShellInstance1 = PowerShell.Create();
                powerShellInstance1.RunspacePool = runSpacePool;
                powerShellInstance1.AddScript($"docker stop {ContainerId}");
                powerShellCommands.Add(powerShellInstance1);
                powerShellCommandResults.Add(powerShellInstance1.BeginInvoke());

                // 2.使用 microsoft/mssql-server-windows-express 建立測試用資料庫
                var powerShellInstance2 = PowerShell.Create();
                powerShellInstance2.RunspacePool = runSpacePool;
                powerShellInstance2.AddScript($"docker run --rm -d -e SA_PASSWORD=1q2w3e4r5t_ -e ACCEPT_EULA=Y -ti -p {Port}:1433 microsoft/mssql-server-windows-developer");
                powerShellCommands.Add(powerShellInstance2);
                powerShellCommandResults.Add(powerShellInstance2.BeginInvoke());

                int i = 0;
                string message = string.Empty;

                foreach (var powerShellCommand in powerShellCommands)
                {
                    PSDataCollection<PSObject> results = powerShellCommand.EndInvoke(powerShellCommandResults[i]);
                    foreach (var result in results)
                    {
                        if (result != null)
                        {
                            message = result.BaseObject.ToString();
                        }
                    }

                    if (i.Equals(1))
                    {
                        ContainerId = message;
                    }

                    i++;
                }
            }

            // 3. 使用 docker logs 指令，查看在 container 的 sql-server 是否已經準備好

            int retryTimes = 60;
            bool ready = false;

            for (int i = 0; i < retryTimes; i++)
            {
                var powerShellInstance = PowerShell.Create();
                powerShellInstance.AddScript($"docker logs {ContainerId}");
                var executeResults = powerShellInstance.Invoke();
                foreach (var psObject in executeResults)
                {
                    if (psObject != null)
                    {
                        var message = psObject.BaseObject.ToString();
                        if (message.Contains("VERBOSE: Started SQL Server"))
                        {
                            ready = true;
                        }
                    }
                }

                if (ready.Equals(false))
                {
                    Thread.Sleep(1000);
                }
                else
                {
                    Console.WriteLine($"wait {i} second");
                    break;
                }
            }

            // 4. 取得新建立 container 的 內部 ip
            string containerInsideIp = "";
            using (PowerShell powerShellinstance = PowerShell.Create())
            {
                powerShellinstance.AddScript($"docker exec {ContainerId} ipconfig | findstr IPv4");
                powerShellinstance.Invoke();

                var psOutput = powerShellinstance.Invoke();
                foreach (var outputItem in psOutput)
                {
                    if (string.IsNullOrWhiteSpace(outputItem?.BaseObject.ToString()))
                    {
                        continue;
                    }

                    var serverIp = outputItem.BaseObject.ToString();
                    containerInsideIp = serverIp.Split(':')[1].Trim();
                }
            }

            databaseIp = string.IsNullOrWhiteSpace(containerInsideIp) ? "" : containerInsideIp;
            containerId = string.IsNullOrWhiteSpace(ContainerId) ? "" : ContainerId;
        }
    }
}