using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Threading;
using L5sDmComm;

namespace MMV
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            // On application start, create an start separate thread
            //ThreadStart theardStart = new ThreadStart(consolidateTracking);
            //Thread thread = new Thread(theardStart);
            //thread.Start();
        }


        protected static void consolidateTracking()
        {
            L5sInitial.LoadForLoginPage();
            // Wait for certain time interval            
            // In this example, task will repeat in infinite loop
            // You can additional parameter if you want to have an option 
            // to stop the task from some page
            while (true)
            {
                System.Threading.Thread.Sleep(TimeSpan.FromMinutes(5));
                try
                {
                  //  P5sCmm.P5sCmmFns.P5sConsolicateTracking();      
                }
                catch (Exception)
                {
                    
                }
               

            }
        }





        protected void Application_End(object sender, EventArgs e)
        {

        }

    }
}