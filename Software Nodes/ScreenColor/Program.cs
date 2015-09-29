﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ScreenColorServer;

namespace ScreenColor
{
    class Program
    {
        //SETTINGS
        const int CAPTURE_UPDATE_DELAY = 0;
        const float HEIGHT_FROM_TOP = 0.4f;


        static bool isWorking;
        static Color screenAvarageColor;

        private static DateTime captureStartDate = DateTime.Now;
        private static int screensCount;

        static void Main(string[] args)
        {
            StartScreenCapture();
            Console.WriteLine("Screen capture started");

            while (true)
            {
                Console.ReadLine();
            }
        }

        private static String ColorToHex(Color color)
        {
            return "#" + color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");
        }

        public static void SendColor(Color color)
        {
            Console.WriteLine(ColorToHex(color));
        }

        private static async void StartScreenCapture()
        {
            if (isWorking) return;

            isWorking = true;

            while (isWorking)
            {
                await Task.Delay(CAPTURE_UPDATE_DELAY);

                await Task.Run(() =>
                {
                    CalculateCapturesPerSec();

                    screenAvarageColor = ScreenCapture.GetScreenAverageColor(HEIGHT_FROM_TOP);
                    SendColor(screenAvarageColor);
                });
            }

        }

        private static void StopScreenCapture()
        {
            isWorking = false;
        }

        private static void CalculateCapturesPerSec()
        {
            screensCount++;

            DateTime now = DateTime.Now;
            TimeSpan elapsed = now.Subtract(captureStartDate);

            if (elapsed.TotalSeconds < 1)
                return;

            float capturesPerSecond = screensCount / (float)elapsed.TotalSeconds;

            captureStartDate = DateTime.Now;
            screensCount = 0;

            Console.WriteLine("Capture " + (int)capturesPerSecond + " screens/second");
        }

    }
}
