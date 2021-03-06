﻿using Microsoft.Win32;
using System;
using System.Windows;

namespace MacRegistryChanger
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly RegistryManipulation _registryManipulation;

        public MainWindow()
        {
            InitializeComponent();
            _registryManipulation = new RegistryManipulation();
        }

        private void btnGetAddress(object sender, RoutedEventArgs e)
        {
        }

        private void btnIsCustomEnabled_Click(object sender, RoutedEventArgs e)
        {
            if (_registryManipulation.IsCustomMacKeyEnabled())
            {
                lblCustomMacEnabled.Content = "Enabled!";
                btnCreateCustomMac.IsEnabled = false;
                btnChangeCustomMac.IsEnabled = true;
                btnDeleteCustomMac.IsEnabled = true;
            }
            else
            {
                lblCustomMacEnabled.Content = "Disabled!";
                btnCreateCustomMac.IsEnabled = true;
                btnChangeCustomMac.IsEnabled = false;
                btnDeleteCustomMac.IsEnabled = false;
            }
        }

        private void btnCreateCustomMac_Click(object sender, RoutedEventArgs e)
        {
            string newMac = txtNewMAC.Text;
            if (newMac.Length < 12)
            {
                MessageBox.Show("A new MAC address must be given!");
                return;
            }
            newMac = newMac.Substring(0, 12);
            _registryManipulation.CreateCustomMacKey(newMac);
        }

        private void btnChangeCustomMac_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnDeleteCustomMac_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void BtnSubkeys_Click(object sender, RoutedEventArgs e)
        {
            RegistryKey y = await _registryManipulation.GetWlanRegistryKeyAsync();
            string a = y.Name;
        }
    }
}
