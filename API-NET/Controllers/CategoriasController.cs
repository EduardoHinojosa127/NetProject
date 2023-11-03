using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;

[Route("api/[controller]")]
[ApiController]
public class CategoriasController : ControllerBase
{
    private readonly MongoDbContext _context;

    public CategoriasController(MongoDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Categoria>> Get()
    {
        var categorias = _context.Categorias.Find(categoria => true).ToList();
        return categorias;
    }

    [HttpGet("{id}", Name = "GetCategoria")]
    public ActionResult<Categoria> Get(int id)
    {
        var categoria = _context.Categorias.Find(a => a.Id == id).FirstOrDefault();

        if (categoria == null)
        {
            return NotFound();
        }
        return categoria;
    }

    [HttpPost]
    public IActionResult Post([FromBody] Categoria categoria)
    {
        _context.Categorias.InsertOne(categoria);
        return CreatedAtAction(nameof(Get), new { id = categoria.Id }, categoria);
    }

    [HttpPut("{id}")]
    public IActionResult Put(int id, [FromBody] Categoria categoria)
    {
        var existingCategoria = _context.Categorias.Find(c => c.Id == id).SingleOrDefault();
        if (existingCategoria == null)
        {
            return NotFound();
        }

        // Actualiza las propiedades de la categoría
        existingCategoria.Nombre = categoria.Nombre;

        _context.Categorias.ReplaceOne(c => c.Id == id, existingCategoria);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var categoria = _context.Categorias.Find(c => c.Id == id).SingleOrDefault();
        if (categoria == null)
        {
            return NotFound();
        }

        _context.Categorias.DeleteOne(c => c.Id == id);

        return NoContent();
    }
}
