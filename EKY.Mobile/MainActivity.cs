using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Webkit;
using Android.Provider;
using System.Collections.Generic;
using Java.Text;

namespace EKY.Mobile
{
    [Activity(Label = "EKY.Mobile", MainLauncher = true, Icon = "@drawable/icon")]
    [IntentFilter(new string[] { "android.provider.Telephony.READ_SMS" }, Priority = (int)IntentFilterPriority.HighPriority)]
    public class MainActivity : Activity
    {

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.EKY);

            // Get our button from the layout resource,
            // and attach an event to it

            Button button = FindViewById<Button>(Resource.Id.buttonEKY);

            button.Text = "GİT!";
            button.Click += delegate
            {
                var editText = FindViewById<EditText>(Resource.Id.editText);
                editText.Text = "http://eniskurtayyilmaz.com";

                editText.Text = "Gelen çağrılar:" + "\n";


                string queryFilter = String.Format("{0}={1}", CallLog.Calls.Type, (int)CallType.Incoming);


                string querySorter = String.Format("{0} desc limit 10", CallLog.Calls.Date);


                Android.Database.ICursor queryData = ContentResolver.Query(CallLog.Calls.ContentUri, null, queryFilter, null, querySorter);

                while (queryData.MoveToNext())
                {
                    editText.Text += queryData.GetString(queryData.GetColumnIndex(CallLog.Calls.Number)) + "\n";


                }

                editText.Text += "Giden Çağrılar:" + "\n";
                queryFilter = String.Format("{0}={1}", CallLog.Calls.Type, (int)CallType.Outgoing);
                querySorter = String.Format("{0} desc limit 10", CallLog.Calls.Date);
                queryData = ContentResolver.Query(CallLog.Calls.ContentUri, null, queryFilter, null, querySorter);

                while (queryData.MoveToNext())
                {
                    editText.Text += queryData.GetString(queryData.GetColumnIndex(CallLog.Calls.Number)) + "\n";

                }


                /* 
                SMS İÇİN OLAN
                */
                editText.Text += "-------------" + "\n";
                Android.Database.ICursor c = ContentResolver.Query(Telephony.Sms.Inbox.ContentUri, new String[] { "address", "body", "date" }, null, null, null);
                while (c.MoveToNext())
                {
                    long unixDate = long.Parse(c.GetString(c.GetColumnIndex("date")));
                    DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                    DateTime date = start.AddMilliseconds(unixDate).ToLocalTime();

                    editText.Text += "Gönderen:" + c.GetString(c.GetColumnIndex("address")) + "\n";
                    editText.Text += "Mesaj:" + c.GetString(c.GetColumnIndex("body")) + "\n";
                    editText.Text += "Tarih:" + date.ToString() + "\n";
                    editText.Text += "-------------" + "\n";
                    // do your stuffs, get more data, whatever...
                }

            };
        }
    }
}

