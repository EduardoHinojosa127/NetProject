public class Comentario
{
    public int Id { get; set; }
    public string TextoComentario { get; set; }
    public int Valoracion { get; set; }
    public int Producto_ID { get; set; }
    public int Usuario_ID { get; set; }
    public DateTime Fecha_comentario { get; set; }
}