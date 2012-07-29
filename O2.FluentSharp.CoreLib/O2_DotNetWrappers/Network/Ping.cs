// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Net.NetworkInformation;
using System.Threading;
using O2.DotNetWrappers;
using O2.Kernel;

namespace O2.DotNetWrappers.Network
{
    public class Ping
    {
        #region Delegates

        public delegate void dPingCompleted(Object oObject, PingCompletedEventArgs e);

        public delegate void dPingReply(PingReply prPingReply);

        #endregion

        public event dPingCompleted ePingCompleted;

        public bool ping(String sHostNameOrAddressToPing)
        {
            try
            {
                var pPing = new System.Net.NetworkInformation.Ping();
                PingReply prPingReply = pPing.Send(sHostNameOrAddressToPing);
                return (prPingReply != null && prPingReply.Status == IPStatus.Success) ? true : false;
            }
            catch (Exception ex)
            {
                PublicDI.log.error("in Ping: {0}", ex.Message);
                return false;
            }
        }

        public void ping_Async(String sHostNameOrAddressToPing)
        {
            try
            {
                var pPing = new System.Net.NetworkInformation.Ping();
                pPing.PingCompleted += pPing_PingCompleted;
                pPing.SendAsync(sHostNameOrAddressToPing, sHostNameOrAddressToPing);
            }
            catch (Exception ex)
            {
                PublicDI.log.error("in Ping: {0}", ex.Message);
            }
        }

        private void pPing_PingCompleted(object sender, PingCompletedEventArgs e)
        {
            if (e.Error == null)
                PublicDI.log.info("Ping status: {0}", e.Reply.Status.ToString());
            else
                PublicDI.log.error("in Ping: {0}", e.Error.Message);
            if (ePingCompleted != null)
                foreach (Delegate dDelegate in ePingCompleted.GetInvocationList())
                    dDelegate.DynamicInvoke(new[] {sender, e});
        }

        #region Nested type: ping_thread

        public class ping_thread
        {
            private readonly dPingReply dCallback;
            private readonly String sHostNameOrAddressToPing;
            public bool bCancel;
            public Int32 iSleepPeriood = 2000;

            public ping_thread(String sHostNameOrAddressToPing, dPingReply dCallback)
            {
                this.sHostNameOrAddressToPing = sHostNameOrAddressToPing;
                this.dCallback = dCallback;
            }

            public void start()
            {
                while (bCancel == false)
                {
                    PingReply prPingReply = new System.Net.NetworkInformation.Ping().Send(sHostNameOrAddressToPing);
                    dCallback.Invoke(prPingReply);
                    Thread.Sleep(iSleepPeriood);
                }
            }

            public void stop()
            {
                bCancel = true;
            }
        }

        #endregion
    }
}
