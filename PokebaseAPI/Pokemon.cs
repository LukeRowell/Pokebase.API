using System.ComponentModel.DataAnnotations;

namespace PokebaseAPI
{
    public class Pokemon
    {
        [Key]
        public int Id { get; set; }
        public int ndexno { get; set; }
        public string name { get; set; } = String.Empty;
        public string type1 { get; set; } = String.Empty;
        public string? type2 { get; set; } = String.Empty;
        public int hp { get; set; }
        public int attack { get; set; }
        public int defense { get; set; }
        public int spatk { get; set; }
        public int spdef { get; set; }
        public int speed { get; set; }
        public int total { get; set; }
        public int gen { get; set; }
    }
}
