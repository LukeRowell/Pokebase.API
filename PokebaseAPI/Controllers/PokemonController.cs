using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PokebaseAPI.Data;
using Microsoft.Data.SqlClient;

namespace PokebaseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : ControllerBase
    {
        private readonly DataContext _context;

        public PokemonController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        //public async Task<ActionResult<List<Pokemon>>> GetPokemon(string type1, string type2, int genFrom, int genThru, string sortVal, string orderVal)
        public async Task<ActionResult<List<Pokemon>>> GetPokemon(string type1, string type2, string genValues, string sortVal, string orderVal)
        {
            string query, usableType;
            string includedGens = "";

            for (int i = 0; i < genValues.Length; i++)
            {
                if (genValues[i] == '1')
                {
                    includedGens += (i + 1).ToString() + ", ";
                }
            }

            includedGens = includedGens.Remove(includedGens.Length - 2);

            System.Console.WriteLine(includedGens);

            if (type1 == "None" && type2 == "None") //if both types are None
            {
                query = "SELECT * FROM Pokemon WHERE 1 = 2;";
            }

            else if (type1 == "Any" && type2 == "Any")  //if both types are Any
            {
                /*
                query = "SELECT * " +
                        "FROM Pokemon " +
                        "WHERE gen BETWEEN " + genFrom.ToString() + " AND " + genThru.ToString() + " " +
                        "ORDER BY " + sortVal + " " + orderVal + ";";
                */
                query = "SELECT * " +
                        "FROM Pokemon " +
                        "WHERE gen IN (" + includedGens + ") " +
                        "ORDER BY " + sortVal + " " + orderVal + ";";
            }

            else if ((type1 == "Any" && type2 == "None") || (type1 == "None" && type2 == "Any"))    //if one type is Any and the other is None
            {
                /*
                query = "SELECT * " +
                        "FROM Pokemon " +
                        "WHERE (type1 is NULL OR type2 is NULL) " + 
                        "AND gen BETWEEN " + genFrom.ToString() + " AND " + genThru.ToString() + " " +
                        "ORDER BY " + sortVal + " " + orderVal + ";";
                */
                query = "SELECT * " +
                        "FROM Pokemon " +
                        "WHERE (type1 is NULL OR type2 is NULL) " +
                        "AND gen IN (" + includedGens + ") " +
                        "ORDER BY " + sortVal + " " + orderVal + ";";
            }

            else if ((type1 == "Any" && type2 != "None") || (type1 != "None" && type2 == "Any"))    //if one type is Any and the other is not None
            {
                usableType = (type1 == "Any" ? type2 : type1);
                /*
                query = "SELECT * " +
                        "FROM Pokemon " +
                        "WHERE ((type1 = '" + usableType + "') OR (type2 = '" + usableType + "')) " +
                        "AND gen BETWEEN " + genFrom.ToString() + " AND " + genThru.ToString() + " " +
                        "ORDER BY " + sortVal + " " + orderVal + ";";
                */
                query = "SELECT * " +
                        "FROM Pokemon " +
                        "WHERE ((type1 = '" + usableType + "') OR (type2 = '" + usableType + "')) " +
                        "AND gen IN (" + includedGens + ") " +
                        "ORDER BY " + sortVal + " " + orderVal + ";";
            }

            else if ((type1 == "None" && type2 != "Any") || (type2 == "None" && type1 != "Any"))  //if one type is None and the other is specific
            {
                usableType = (type1 == "None" ? type2 : type1);
                /*
                query = "SELECT * " +
                        "FROM Pokemon " +
                        "WHERE (type1 = '" + usableType + "' AND type2 IS NULL) " +
                        "AND gen BETWEEN " + genFrom.ToString() + " AND " + genThru.ToString() + " " +
                        "ORDER BY " + sortVal + " " + orderVal + ";";
                */
                query = "SELECT * " +
                        "FROM Pokemon " +
                        "WHERE (type1 = '" + usableType + "' AND type2 IS NULL) " +
                        "AND gen IN (" + includedGens + ") " +
                        "ORDER BY " + sortVal + " " + orderVal + ";";
            }

            else  //if both types are specified
            {
                /*
                query = "SELECT * " +
                        "FROM Pokemon " +
                        "WHERE ((type1 = '" + type1 + "' AND type2 = '" + type2 + "') OR (type1 = '" + type2 + "' AND type2 = '" + type1 + "'))" +
                        "AND gen BETWEEN " + genFrom.ToString() + " AND " + genThru.ToString() + " " +
                        "ORDER BY " + sortVal + " " + orderVal + ";";
                */
                query = "SELECT * " +
                        "FROM Pokemon " +
                        "WHERE ((type1 = '" + type1 + "' AND type2 = '" + type2 + "') OR (type1 = '" + type2 + "' AND type2 = '" + type1 + "'))" +
                        "AND gen IN (" + includedGens + ") " +
                        "ORDER BY " + sortVal + " " + orderVal + ";";
            }

            return Ok(await _context.Pokemon.FromSqlRaw(query).ToListAsync());
        }

        [HttpGet("name")]
        public async Task<ActionResult<Pokemon>> SearchPokemonByName(string name)
        {
            var pokemon = await _context.Pokemon.FirstOrDefaultAsync(p => p.name == name);

            if (pokemon == null)
            {
                return NotFound();
            }

            return Ok(pokemon);
        }
    }
}