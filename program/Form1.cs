using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace program
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            UpdateProcessList();
        }

        private void UpdateProcessList()
        {
            listBoxProcesses.DataSource = Process.GetProcesses()
        .Select(p => p.ProcessName)
        .Distinct()
        .OrderBy(p => p)
        .ToList();
        }

        private void timerUpdate_Tick(object sender, EventArgs e)
        {
            UpdateProcessList();
        }

        private void listBoxProcesses_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxProcesses.SelectedIndex >= 0)
            {
                var selectedProcessName = listBoxProcesses.SelectedItem as string;
                var selectedProcesses = Process.GetProcessesByName(selectedProcessName);

                if (selectedProcesses.Any())
                {
                    lblDetails.Text = GetProcessDetails(selectedProcesses[0]);
                }
            }
        }

        private string GetProcessDetails(Process process)
        {
            return $"ID: {process.Id}\n" +
                   $"Start time: {process.StartTime}\n" +
                   $"Total processor time: {process.TotalProcessorTime}\n" +
                   $"Threads count: {process.Threads.Count}\n" +
                   $"Instances count: {Process.GetProcessesByName(process.ProcessName).Length}";
        }

        private void lblDetails_Click(object sender, EventArgs e)
        {
            UpdateProcessList();
            timerUpdate.Interval = 1000; // Update interval in milliseconds
            timerUpdate.Start();

        }

        private void buttonKillProcess_Click(object sender, EventArgs e)
        {
            if (listBoxProcesses.SelectedIndex >= 0)
            {
                var selectedProcessName = listBoxProcesses.SelectedItem as string;
                var selectedProcesses = Process.GetProcessesByName(selectedProcessName);

                if (selectedProcesses.Any())
                {
                    selectedProcesses[0].Kill();
                    UpdateProcessList();
                }
            }
        }
    }
}
