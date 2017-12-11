using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;

namespace Keiho.IO
{
    public static class DirectoryUtility
    {
        // 例: DirectoryUtility.AddAccessRule(@"C:\Temp", @"IIS AppPool\DefaultAppPool", FileSystemRights.Modify);
        public static void AddAccessRule(string path, string identity, FileSystemRights fileSystemRights)
        {
            var control = Directory.GetAccessControl(path);
            control.AddAccessRule(new FileSystemAccessRule(identity, fileSystemRights, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
            Directory.SetAccessControl(path, control);
        }
    }
}
