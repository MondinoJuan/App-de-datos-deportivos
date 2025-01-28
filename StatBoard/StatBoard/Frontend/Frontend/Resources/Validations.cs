using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;


namespace Frontend.Resources
{
    internal class Validations
    {
        // Para validar si es string "string.IsNullOrEmpty(value)"
        // Para validar si es int "int.IsEvenInteger"
        // Para validar si es float "float.IsEvenFloat"
        // Para validar si es double "double.IsEvenDouble"

        public bool ValidateNumber(int value)
        {
            if (value > 0 && value < 100)
            {
                return false;
            }
            return true;
        }

        public bool ValidateAlphabeticString(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }

            // Expresión regular para validar solo letras del alfabeto y la ñ (mayúsculas y minúsculas)
            Regex regex = new Regex("^[a-zA-ZñÑáéíóúÁÉÍÓÚüÜ\\s]+$");
            return regex.IsMatch(value);
        }
    }
}
