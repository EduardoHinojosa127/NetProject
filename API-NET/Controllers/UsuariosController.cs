using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;
using BCrypt.Net;
using System;
using System.IO;
using System.Net;

[Route("api/[controller]")]
[ApiController]
public class UsuariosController : ControllerBase
{
    private readonly MongoDbContext _context;

    public UsuariosController(MongoDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Usuario>> Get()
    {
        var usuarios = _context.Usuarios.Find(usuario => true).ToList();
        return usuarios;
    }

    [HttpGet("{id}", Name = "GetUsuario")]
    public ActionResult<Usuario> Get(int id)
    {
        var usuario = _context.Usuarios.Find(a => a.Id == id).FirstOrDefault();

        if (usuario == null)
        {
            return NotFound();
        }
        return usuario;
    }

    [HttpPost]
    public IActionResult Post([FromBody] Usuario usuario)
    {
        var existingUser = _context.Usuarios.Find(u => u.Email == usuario.Email).FirstOrDefault();

        if (existingUser != null)
        {
            return BadRequest("El email ya está en uso.");
        }
        
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(usuario.Contraseña);
        usuario.Contraseña = hashedPassword;
        string imageUrl = "https://t4.ftcdn.net/jpg/02/29/75/83/360_F_229758328_7x8jwCwjtBMmC6rgFzLFhZoEpLobB6L8.jpg";

        // Descarga la imagen desde la URL
        byte[] imageBytes;
        using (WebClient webClient = new WebClient())
        {
            imageBytes = webClient.DownloadData(imageUrl);
        }

        // Convierte la imagen descargada a Base64
        string imageBase64 = Convert.ToBase64String(imageBytes);

        // Convierte la cadena Base64 a un arreglo de bytes
        byte[] imageByteArray = Convert.FromBase64String(imageBase64);

        // Asigna el arreglo de bytes al campo 'usuario.Imagen'
        usuario.Imagen = imageByteArray;
        _context.Usuarios.InsertOne(usuario);
        return CreatedAtAction(nameof(Get), new { id = usuario.Id }, usuario);
    }

    [HttpPut("{id}")]
    public IActionResult Put(int id, [FromBody] Usuario usuario)
    {
        var existingUsuario = _context.Usuarios.Find(c => c.Id == id).SingleOrDefault();
        if (existingUsuario == null)
        {
            return NotFound();
        }

        // Actualiza las propiedades de la categoría
        existingUsuario.Nombres = usuario.Nombres;
        existingUsuario.Apellidos = usuario.Apellidos;
        existingUsuario.Email = usuario.Email;
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(usuario.Contraseña);
        existingUsuario.Contraseña = hashedPassword;
        existingUsuario.Telefono = usuario.Telefono;
        existingUsuario.Rol = usuario.Rol;

        _context.Usuarios.ReplaceOne(c => c.Id == id, existingUsuario);

        return NoContent();
    }

    [HttpPut("{id}/2")]
    public IActionResult UpdateWithoutPassword(int id, [FromBody] UsuarioUpdateRequest usuario)
    {
        var existingUsuario = _context.Usuarios.Find(c => c.Id == id).SingleOrDefault();
        if (existingUsuario == null)
        {
            return NotFound();
        }

        // Actualiza las propiedades de la categoría
        existingUsuario.Nombres = usuario.Nombres;
        existingUsuario.Apellidos = usuario.Apellidos;
        existingUsuario.Email = usuario.Email;
        existingUsuario.Telefono = usuario.Telefono;
        existingUsuario.Rol = usuario.Rol;
        existingUsuario.Imagen = usuario.Imagen;
        _context.Usuarios.ReplaceOne(c => c.Id == id, existingUsuario);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var usuario = _context.Usuarios.Find(c => c.Id == id).SingleOrDefault();
        if (usuario == null)
        {
            return NotFound();
        }

        _context.Usuarios.DeleteOne(c => c.Id == id);

        return NoContent();
    }
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest loginRequest)
    {
        var usuario = _context.Usuarios.Find(u => u.Email == loginRequest.Email).SingleOrDefault();

        if (usuario == null)
        {
            return Unauthorized(new { message = "Acceso denegado." });
        }

        if (BCrypt.Net.BCrypt.Verify(loginRequest.Contraseña, usuario.Contraseña))
        {
            return Ok(new { message = "Acceso permitido.", userId = usuario.Id });
        }
        else
        {
            return Unauthorized(new { message = "Acceso denegado." });
        }
    }
    
    [HttpGet("maxid", Name = "GetUsuarioWithMaxId")]
    public ActionResult<Usuario> GetUsuarioWithMaxId()
    {
        var maxIdUsuario = _context.Usuarios.Find(_ => true).SortByDescending(u => u.Id).FirstOrDefault();

        if (maxIdUsuario == null)
        {
            return NotFound();
        }

        return maxIdUsuario;
    }
    
    public class LoginRequest
    {
        public string Email { get; set; }
        public string Contraseña { get; set; }
    }
    public class UsuarioUpdateRequest
    {
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string Rol { get; set; }
        public byte[]? Imagen { get; set; }
    }

}
