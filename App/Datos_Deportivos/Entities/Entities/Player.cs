using System.ComponentModel.DataAnnotations;
// Para hacer mas rapido cargando los datos del contrario podrias no cargar los nombres o demás datos, y cargar unicamente los números.
// Podría dejar los demas datos para cuando cree una BdD de jugadores.
namespace Entities.Entities
{
    public class Player : EntityBase
    {
        /*
        public static string? CompleteName { get; set; }         

        public static string? Birthdate { get; set; }

        public static decimal? Hight { get; set; }

        public static decimal? Weight { get; set; }

        public static string State { get; set; }            // Si esta habilitado para jugar (estudios medicos, etc)
*/
        [Required]
        public int Number { get; set; }              // No todos los jugadores mantienen su número es sus diferentes camisetas.
    }
}
