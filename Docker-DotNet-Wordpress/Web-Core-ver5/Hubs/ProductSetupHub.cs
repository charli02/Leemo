using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Web_Core_ver5.Helpers;

namespace Web_Core_ver5.Hubs
{
    public class ProductSetupHub : Hub
    {
        private readonly SetupChecker _setupChecker;

        public ProductSetupHub(SetupChecker setupChecker)
        {
            _setupChecker = setupChecker;
        }

        //public async Task Hello()
        //{
        //    //Clients.Caller.DisplayMessage("Hello from the SignalrDemoHub!");
        //}

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public string GetConnectionId()
        {
            return Context.ConnectionId;
        }

        public async Task GetUpdateForOrder(int orderId)
        {
            CheckResult result;
            do
            {
                result = _setupChecker.GetUpdate(orderId);
                System.Threading.Thread.Sleep(1000);
                if (result.New)
                    await Clients.Caller.SendAsync("ReceiveOrderUpdate",
                        result.Update);
            } while (!result.Finished);

            await Clients.Caller.SendAsync("Finished");
        }

        public async Task ShowProgress()
        {
            //CheckResult result;
            bool progressFlag = true;
            int progressCount = 0;
            do
            {
                //result = _setupChecker.GetUpdate(orderId);
                System.Threading.Thread.Sleep(2000);
                await Clients.Caller.SendAsync("Progress", progressCount);
                progressCount += 20;
                if(progressCount >= 100)
                {
                    progressFlag = false;
                }
            } while (progressFlag);
            System.Threading.Thread.Sleep(2000);
            await Clients.Caller.SendAsync("Finished");
        }

    }
}
