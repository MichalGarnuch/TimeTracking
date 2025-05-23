﻿using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TimeTrackingMobile.Services;
using TimeTrackingMobile.Views;

namespace TimeTrackingMobile
{
    public partial class App : Application
    {

        public App ()
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback +=
    (sender, cert, chain, sslPolicyErrors) => true;

            InitializeComponent();

            MainPage = new AppShell();
        }


        protected override void OnStart ()
        {
        }

        protected override void OnSleep ()
        {
        }

        protected override void OnResume ()
        {
        }
    }
}
