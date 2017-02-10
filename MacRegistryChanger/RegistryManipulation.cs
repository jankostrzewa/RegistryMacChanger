using System.Collections.Generic;
using System.Linq;
using Microsoft.Win32;
using System;
using System.Windows;

namespace MacRegistryChanger
{
    class RegistryManipulation
    {
        private RegistryKey registryKey;
        private string nwAdr;

        public RegistryManipulation()
        {
            try
            {
                registryKey = GetWlanRegistryKey();
            }
            catch(InvalidOperationException e)
            {
                MessageBox.Show("No wireless card found!");
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show("Unable to modify registry - please run as an administrator!");
            }
            finally
            {
                nwAdr = "NetworkAddress";
            }
            //    Registry.LocalMachine.OpenSubKey(
            //        @"SYSTEM\ControlSet001\Control\Class\{4d36e972-e325-11ce-bfc1-08002be10318}\0001", true);
        }
       

        public RegistryKey GetWlanRegistryKey()
        {
            RegistryKey x = Registry.LocalMachine.OpenSubKey(
                    @"SYSTEM\ControlSet001\Control\Class\{4d36e972-e325-11ce-bfc1-08002be10318}", true);
            List<string> subKeyNames = x.GetSubKeyNames().ToList();
            foreach(string subKeyName in subKeyNames)
            {
                RegistryKey subkey = x.OpenSubKey(subKeyName, true);
                List<string> keyValues = subkey.GetValueNames().ToList();
                if (keyValues.Contains("NetType"))
                    return subkey;
            }
            throw new InvalidOperationException();
        }

        public bool IsCustomMacKeyEnabled()
        {
            string netAddr = GetMacAddressFromRegistry();
            if (netAddr != null)
                return true;
            return false;
        }

        public bool CreateCustomMacKey(string newAddress)
        {
            string oldNetAddr = GetMacAddressFromRegistry();
            try
            {
                registryKey.SetValue(nwAdr, newAddress);
            }
            catch (UnauthorizedAccessException ex)
            {
               MessageBox.Show("Unable to modify registry - please run as an administrator!");
            }
            string newNetAddr = GetMacAddressFromRegistry();
            if (newNetAddr != oldNetAddr)
            {
                if (newNetAddr == newAddress)
                    return true;
            }
            return false;
        }
    

        public bool DeleteCustomMacKey()
        {
            registryKey.DeleteSubKey(nwAdr);
            string netAddr = GetMacAddressFromRegistry();
            if (netAddr != null)
                return false;
            return true;
        }

        public bool ChangeCustomMacKey(string newMac)
        {
            registryKey.SetValue(nwAdr, newMac);
            return true;
        }

        public string GetMacAddressFromRegistry()
        {
            return (string)registryKey.GetValue(nwAdr);
            
        }

        ~RegistryManipulation()
        {
            registryKey.Close();
            registryKey.Dispose();
        }
    }
}
