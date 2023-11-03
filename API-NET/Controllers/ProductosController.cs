using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;

[Route("api/[controller]")]
[ApiController]
public class ProductosController : ControllerBase
{
    private readonly MongoDbContext _context;

    public ProductosController(MongoDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Producto>> Get()
    {
        var productos = _context.Productos.Find(producto => true).ToList();
        return productos;
    }

    [HttpGet("{id}", Name = "GetProducto")]
    public ActionResult<Producto> Get(int id)
    {
        var producto = _context.Productos.Find(a => a.Id == id).FirstOrDefault();

        if (producto == null)
        {
            return NotFound();
        }
        return producto;
    }

    [HttpGet("{id}/user", Name = "GetProductoByUser")]
    public ActionResult<IEnumerable<Producto>> GetProductosByUser(int id)
    {
        var productos = _context.Productos.Find(p => p.Usuario_ID == id).ToList();

        if (productos == null || !productos.Any())
        {
            return NotFound();
        }
        return productos;
    }

    [HttpPost]
    public IActionResult Post([FromBody] Producto producto)
    {
        _context.Productos.InsertOne(producto);
        return CreatedAtAction(nameof(Get), new { id = producto.Id }, producto);
    }

    [HttpPut("{id}")]
    public IActionResult Put(int id, [FromBody] Producto producto)
    {
        var existingProducto = _context.Productos.Find(c => c.Id == id).SingleOrDefault();
        if (existingProducto == null)
        {
            return NotFound();
        }

        // Actualiza las propiedades de la categoría
        existingProducto.Nombre = producto.Nombre;
        existingProducto.Descripcion = producto.Descripcion;
        existingProducto.Precio = producto.Precio;
        existingProducto.Stock = producto.Stock;
        existingProducto.Imagen = producto.Imagen;
        existingProducto.Categoria = producto.Categoria;
        existingProducto.Usuario_ID = producto.Usuario_ID;
        _context.Productos.ReplaceOne(c => c.Id == id, existingProducto);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var producto = _context.Productos.Find(c => c.Id == id).SingleOrDefault();
        if (producto == null)
        {
            return NotFound();
        }

        _context.Productos.DeleteOne(c => c.Id == id);

        return NoContent();
    }

    [HttpGet("maxid", Name = "GetProductoWithMaxId")]
    public ActionResult<Producto> GetProductoWithMaxId()
    {
        var maxIdProducto = _context.Productos.Find(_ => true).SortByDescending(u => u.Id).FirstOrDefault();

        if (maxIdProducto == null)
        {
            return NotFound();
        }

        return maxIdProducto;
    }
}
