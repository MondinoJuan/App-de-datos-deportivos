using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;


namespace Frontend.Resources
{
    public class Validations
    {
        // Para validar si es string "string.IsNullOrEmpty(value)"
        // Para validar si es int "int.IsEvenInteger"
        // Para validar si es float "float.IsEvenFloat"
        // Para validar si es double "double.IsEvenDouble"

        public static bool ValidateNumber(string value)
        {
            Regex regex = new Regex(@"^\d+$");
            if (!regex.IsMatch(value))
            {
                return false;
            }

            if (int.TryParse(value, out int ayuda))
            {
                return ayuda > 0 && ayuda < 100;
            }

            return false;
        }

        public static bool ValidateAlphabeticString(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }

            // Expresión regular para validar solo letras del alfabeto y la ñ (mayúsculas y minúsculas)
            Regex regex = new Regex("^[a-zA-ZñÑáéíóúÁÉÍÓÚüÜ\\s]+$");
            return regex.IsMatch(value);
        }

        // Validar que el maximo de jugadores a agregar sea 16.
    }
}
