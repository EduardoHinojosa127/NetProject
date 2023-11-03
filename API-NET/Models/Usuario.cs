public class Usuario
{
    public int Id { get; set; }
    public string Nombres { get; set; }
    public string Apellidos { get; set; }
    public string Email { get; set; }
    public string Contraseña { get; set; }
    public string Telefono { get; set; }
    public string Rol { get; set; }
    public byte[]? Imagen { get; set; }
}