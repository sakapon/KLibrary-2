using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using Keiho.ComponentModel;
using Keiho.Runtime.InteropServices;

namespace Keiho.Security.Principal
{
    public class WindowsImpersonationScope : DisposableBase
    {
        private IntPtr userToken;
        private WindowsImpersonationContext impersonationContext;

        public WindowsImpersonationScope(string username, string password, string domain = "")
        {
            bool isAuthenticated = NativeFunctions.LogonUser(username, domain, password, NativeFunctions.LOGON32_LOGON_INTERACTIVE, NativeFunctions.LOGON32_PROVIDER_DEFAULT, ref userToken);
            if (!isAuthenticated)
            {
                throw NativeFunctions.GetLastWin32Error();
            }

            impersonationContext = WindowsIdentity.Impersonate(userToken);
        }

        protected override void Dispose(bool disposing)
        {
            if (impersonationContext != null)
            {
                impersonationContext.Undo();
            }

            if (disposing)
            {
                if (impersonationContext != null)
                {
                    impersonationContext.Dispose();
                }
            }

            if (userToken != IntPtr.Zero)
            {
                NativeFunctions.CloseHandle(userToken);
            }

            base.Dispose(disposing);
        }
    }
}
