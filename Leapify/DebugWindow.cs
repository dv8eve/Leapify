﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Leapify
{
    public partial class DebugWindow : Form
    {
        private Core.Spotify.Client _spotify;

        private Core.LeapMotion.Leap _leap;

        public DebugWindow()
        {
            InitializeComponent();

            _spotify = new Core.Spotify.Client();
            _spotify.Initalize();

            _leap = new Core.LeapMotion.Leap();
            _leap.Gesture.OnMessage += Gesture_OnMessage;

            spotifyCheck.Start();
            leapMotionCheck.Start();
        }

        void Gesture_OnMessage(object source, string message)
        {
            try
            {
                Invoke((MethodInvoker)delegate
                {
                    if (message.Contains("Fingers:"))
                    {
                        lblLeapFingersValue.Text = message.Remove(0, 9);
                    }
                    else if (message.Contains("Tools:"))
                    {
                        lblLeapToolsValue.Text = message.Remove(0, 7);
                    }
                    else
                    {
                        txtLeapMessages.AppendText(message + Environment.NewLine);
                    }
                });
            }
            catch (ObjectDisposedException)
            {
                // App just shutting down, no big deal
            }
        }

        private void spotifyCheck_Tick(object sender, EventArgs e)
        {
            lblSpotifyStatus.Text = _spotify.IsRunning ? "Running" : "Not Running";
        }

        private void leapMotionCheck_Tick(object sender, EventArgs e)
        {
            lblLeapStatus.Text = _leap.IsConnected ? "Connected" : "Disconnected";
        }

        private void DebugWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            spotifyCheck.Stop();
            leapMotionCheck.Stop();

            _leap.Gesture.OnMessage -= Gesture_OnMessage;
        }
    }
}