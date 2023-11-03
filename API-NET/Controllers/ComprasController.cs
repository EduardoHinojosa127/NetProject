using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;

[Route("api/[controller]")]
[ApiController]
public class ComprasController : ControllerBase
{
    private readonly MongoDbContext _context;

    public ComprasController(MongoDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Compra>> Get()
    {
        var compras = _context.Compras.Find(compra => true).ToList();
        return compras;
    }

    [HttpGet("{id}", Name = "GetCompra")]
    public ActionResult<Compra> Get(int id)
    {
        var compra = _context.Compras.Find(a => a.Id == id).FirstOrDefault();

        if (compra == null)
        {
            return NotFound();
        }
        return compra;
    }

    [HttpPost]
    public IActionResult Post([FromBody] Compra compra)
    {
        _context.Compras.InsertOne(compra);
        return CreatedAtAction(nameof(Get), new { id = compra.Id }, compra);
    }

    [HttpPut("{id}")]
    public IActionResult Put(int id, [FromBody] Compra compra)
    {
        var existingCompra = _context.Compras.Find(c => c.Id == id).SingleOrDefault();
        if (existingCompra == null)
        {
            return NotFound();
        }

        // Actualiza las propiedades de la categoría
        existingCompra.Producto_ID = compra.Producto_ID;
        existingCompra.Usuario_ID = compra.Usuario_ID;
        existingCompra.Fecha_compra = compra.Fecha_compra;
        _context.Compras.ReplaceOne(c => c.Id == id, existingCompra);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var compra = _context.Compras.Find(c => c.Id == id).SingleOrDefault();
        if (compra == null)
        {
            return NotFound();
        }

        _context.Compras.DeleteOne(c => c.Id == id);

        return NoContent();
    }
}
