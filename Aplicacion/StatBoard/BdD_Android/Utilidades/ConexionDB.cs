using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace BdD_Android.Utilidades
{
    public static class ConexionDB
    {
        public static string DevolverRuta(string nombreBD)
        {
            string rutaBD = string.Empty;
            string rutaDevolver = string.Empty;
            if(DeviceInfo.Platform == DevicePlatform.Android)
            {
                rutaBD = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                rutaDevolver = Path.Combine(rutaBD, nombreBD);
            }
            else if (DeviceInfo.Platform == DevicePlatform.iOS)
            {
                rutaBD = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                rutaDevolver = Path.Combine(rutaBD, "..", "Library", nombreBD);
            }

            return rutaDevolver;
        }
    }
}
