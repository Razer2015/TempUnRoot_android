using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.IO;

namespace TempUnRoot
{
    [Activity(Label = "TempUnRoot", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        string rooted = "Your device is rooted - Click me to unroot";
        string not_rooted = "Your device isn't rooted - Click me to root";

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.button1);

            if (RootUtil.isDeviceRooted())
                button.Text = rooted;
            else
                button.Text = not_rooted;

            button.Click += delegate { button.Text = OnClick(); };
        }

        private string OnClick()
        {
            if (RootUtil.isDeviceRooted())
            {
                if(TempUnroot.unRoot())
                    return (not_rooted);
                else
                    return (rooted);
            }
            else
            {
                if (TempUnroot.Root())
                    return (rooted);
                else
                    return (not_rooted);
            }
                
        }
    }

    public class TempUnroot
    {
        public static Boolean unRoot()
        {
            Java.Lang.Process rootProcess;
            try
            {
                rootProcess = Java.Lang.Runtime.GetRuntime().Exec(new String[] { "su", "-c", "mount -o rw,remount /system && mv /system/xbin/su /system/xbin/subackup && mv /system/bin/su /system/bin/subackup && mount -o ro,remount /system" });
                rootProcess.WaitFor();
                return (true);
            }
            catch (Java.Lang.InterruptedException e)
            {
                e.PrintStackTrace();
                return (false);
            }
        }

        public static Boolean Root()
        {
            Java.Lang.Process rootProcess;
            try
            {
                rootProcess = Java.Lang.Runtime.GetRuntime().Exec(new String[] { "subackup", "-c", "mount -o rw,remount /system && mv /system/xbin/subackup /system/xbin/su && mv /system/bin/subackup /system/bin/su && mount -o ro,remount /system" });
                rootProcess.WaitFor();
                return (true);
            }
            catch (Java.Lang.InterruptedException e)
            {
                e.PrintStackTrace();
                return (false);
            }
        }
    }

    /** @author Kevin Kowalewski - Modified by Razerman */
    public class RootUtil
    {
        public static Boolean isDeviceRooted()
        {
            return checkRootMethod();
        }

        private static Boolean checkRootMethod()
        {
            String[] paths = { "/system/bin/su", "/system/xbin/su" };
            foreach (String path in paths)
            {
                if (File.Exists(path)) return true;
            }
            return false;
        }
    }
}

