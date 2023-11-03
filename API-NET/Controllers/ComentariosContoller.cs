using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;

[Route("api/[controller]")]
[ApiController]
public class ComentariosController : ControllerBase
{
    private readonly MongoDbContext _context;

    public ComentariosController(MongoDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Comentario>> Get()
    {
        var comentarios = _context.Comentarios.Find(comentario => true).ToList();
        return comentarios;
    }

    [HttpGet("{id}", Name = "GetComentario")]
    public ActionResult<Comentario> Get(int id)
    {
        var comentario = _context.Comentarios.Find(a => a.Id == id).FirstOrDefault();

        if (comentario == null)
        {
            return NotFound();
        }
        return comentario;
    }

    [HttpPost]
    public IActionResult Post([FromBody] Comentario comentario)
    {
        _context.Comentarios.InsertOne(comentario);
        return CreatedAtAction(nameof(Get), new { id = comentario.Id }, comentario);
    }

    [HttpPut("{id}")]
    public IActionResult Put(int id, [FromBody] Comentario comentario)
    {
        var existingComentario = _context.Comentarios.Find(c => c.Id == id).SingleOrDefault();
        if (existingComentario == null)
        {
            return NotFound();
        }

        // Actualiza las propiedades de la categoría
        existingComentario.TextoComentario = comentario.TextoComentario;
        existingComentario.Valoracion = comentario.Valoracion;
        existingComentario.Producto_ID = comentario.Producto_ID;
        existingComentario.Usuario_ID = comentario.Usuario_ID;
        existingComentario.Fecha_comentario = comentario.Fecha_comentario;
        _context.Comentarios.ReplaceOne(c => c.Id == id, existingComentario);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var comentario = _context.Comentarios.Find(c => c.Id == id).SingleOrDefault();
        if (comentario == null)
        {
            return NotFound();
        }

        _context.Comentarios.DeleteOne(c => c.Id == id);

        return NoContent();
    }
}
