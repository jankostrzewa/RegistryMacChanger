using System.Collections.Generic;
using System.Linq;
using Microsoft.Win32;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace MacRegistryChanger
{
    class RegistryManipulation
    {
        private RegistryKey _registryKey;
        private string MacAddressValue => _registryKey.GetValue(_networkAddress).ToString();
        private readonly string _networkAddress = "NetworkAddress";

        public RegistryManipulation()
        {
            //try
            //{
            //    registryKey = GetWlanRegistryKey();
            //}
            //catch (InvalidOperationException)
            //{
            //    MessageBox.Show("No wireless card found!");
            //}
            //catch (UnauthorizedAccessException ex)
            //{
            //    MessageBox.Show("Unable to modify registry - please run as an administrator!");
            //}
            //finally
            //{
            //    nwAdr = ;
            //}
            //    Registry.LocalMachine.OpenSubKey(
            //        @"SYSTEM\ControlSet001\Control\Class\{4d36e972-e325-11ce-bfc1-08002be10318}\0001", true);
        }

        public Task<RegistryKey> GetWlanRegistryKeyAsync()
        {
            RegistryKey key = Registry.LocalMachine.OpenSubKey(
                @"SYSTEM\ControlSet001\Control\Class\{4d36e972-e325-11ce-bfc1-08002be10318}", true);
            if (key == null)
                throw new InvalidOperationException();
            var subKeyNames = key.GetSubKeyNames();
            var netKey = Array.Find(subKeyNames, k =>
            {
                RegistryKey regKey = key.OpenSubKey(k, true);
                return regKey != null && regKey
                           .GetValueNames()
                           .Any(v => v.Contains("NetType"));
            });
            throw new InvalidOperationException();
        }

        public bool IsCustomMacKeyEnabled()
        {
            return !string.IsNullOrWhiteSpace(MacAddressValue);
        }

        public bool CreateCustomMacKey(string newAddress)
        {
            string oldNetAddr = MacAddressValue;
            try
            {
                _registryKey.SetValue(_networkAddress, newAddress);
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show("Unable to modify registry - please run as an administrator!");
            }
            string newNetAddr = MacAddressValue;
            if (newNetAddr != oldNetAddr)
            {
                if (newNetAddr == newAddress)
                    return true;
            }
            return false;
        }


        public bool DeleteCustomMacKey()
        {
            _registryKey.DeleteSubKey(_networkAddress);
            string netAddr = MacAddressValue;
            if (netAddr != null)
                return false;
            return true;
        }

        public bool ChangeCustomMacKey(string newMac)
        {
            _registryKey.SetValue(_networkAddress, newMac);
            return true;
        }

    }
}
