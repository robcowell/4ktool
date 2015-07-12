using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Net;

namespace kampfpanzerin.core.UI {
    class CredentialPrompt {

        public const int CREDUIWIN_GENERIC = 0x1;
        public const int CREDUIWIN_CHECKBOX = 0x2;
        public const int CREDUIWIN_AUTHPACKAGE_ONLY = 0x10;
        public const int CREDUIWIN_IN_CRED_ONLY = 0x20;
        public const int CREDUIWIN_ENUMERATE_ADMINS = 0x100;
        public const int CREDUIWIN_ENUMERATE_CURRENT_USER = 0x200;
        public const int CREDUIWIN_SECURE_PROMPT = 0x1000;
        public const int CREDUIWIN_PREPROMPTING = 0x2000;
        public const int CREDUIWIN_PACK_32_WOW = 0x10000000;

        [DllImport("ole32.dll")]
        public static extern void CoTaskMemFree(IntPtr ptr);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct CREDUI_INFO {
            public int cbSize;
            public IntPtr hwndParent;
            public string pszMessageText;
            public string pszCaptionText;
            public IntPtr hbmBanner;
        }


        [DllImport("credui.dll", CharSet = CharSet.Auto)]
        private static extern bool CredUnPackAuthenticationBuffer(int dwFlags,
                                                                   IntPtr pAuthBuffer,
                                                                   uint cbAuthBuffer,
                                                                   StringBuilder pszUserName,
                                                                   ref int pcchMaxUserName,
                                                                   StringBuilder pszDomainName,
                                                                   ref int pcchMaxDomainame,
                                                                   StringBuilder pszPassword,
                                                                   ref int pcchMaxPassword);

        [DllImport("credui.dll", CharSet = CharSet.Auto)]
        private static extern int CredUIPromptForWindowsCredentials(ref CREDUI_INFO notUsedHere,
                                                                     int authError,
                                                                     ref uint authPackage,
                                                                     IntPtr InAuthBuffer,
                                                                     uint InAuthBufferSize,
                                                                     out IntPtr refOutAuthBuffer,
                                                                     out uint refOutAuthBufferSize,
                                                                     ref bool fSave,
                                                                     int flags);

        [DllImport("NativeHelpers.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SecureZeroMem(IntPtr ptr, uint cnt);

        public static KampfCredentialDescriptor GetCredentialsVistaAndUp(string serverName) {
            CREDUI_INFO credui = new CREDUI_INFO();
            credui.pszCaptionText = "Please enter the credentails for " + serverName;
            credui.pszMessageText = "DisplayedMessage";
            credui.cbSize = Marshal.SizeOf(credui);
            uint authPackage = 0;
            IntPtr outCredBuffer = new IntPtr();
            uint outCredSize;
            bool save = false;
            int result = CredUIPromptForWindowsCredentials(ref credui,
                                                           0,
                                                           ref authPackage,
                                                           IntPtr.Zero,
                                                           0,
                                                           out outCredBuffer,
                                                           out outCredSize,
                                                           ref save,
                                                           CREDUIWIN_GENERIC | CREDUIWIN_CHECKBOX);

            var usernameBuf = new StringBuilder(100);
            var passwordBuf = new StringBuilder(100);
            var domainBuf = new StringBuilder(100);

            int maxUserName = 100;
            int maxDomain = 100;
            int maxPassword = 100;
            if (result == 0) {
                if (CredUnPackAuthenticationBuffer(0, outCredBuffer, outCredSize, usernameBuf, ref maxUserName,
                                                   domainBuf, ref maxDomain, passwordBuf, ref maxPassword)) {
                    //TODO: ms documentation says we should call this but i can't get it to work
                    //SecureZeroMem(outCredBuffer, outCredSize);

                    //clear the memory allocated by CredUIPromptForWindowsCredentials 
                    CoTaskMemFree(outCredBuffer);
                    NetworkCredential networkCredential = new NetworkCredential() {
                        UserName = usernameBuf.ToString(),
                        Password = passwordBuf.ToString(),
                        Domain = domainBuf.ToString()
                    };
                    KampfCredentialDescriptor desc = new KampfCredentialDescriptor(networkCredential, save, true);
                    return desc;
                }
            }

            return new KampfCredentialDescriptor(null, save, false);
        }
    }

    public class KampfCredentialDescriptor {

        public readonly bool Remember;
        public readonly bool Valid;
        public readonly NetworkCredential Credentials;

        internal KampfCredentialDescriptor(NetworkCredential credentials, bool remember, bool valid) {
            this.Remember = remember;
            this.Credentials = credentials;
            this.Valid = valid;
        }
    }
}
