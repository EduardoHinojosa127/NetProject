using MongoDB.Driver;
using Microsoft.Extensions.Configuration;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(IConfiguration configuration)
    {
        var client = new MongoClient(configuration.GetConnectionString("MongoDBConnection"));
        _database = client.GetDatabase("APINet");
    }

    public IMongoCollection<Producto> Productos => _database.GetCollection<Producto>("Productos");
    public IMongoCollection<Usuario> Usuarios => _database.GetCollection<Usuario>("Usuarios");
    public IMongoCollection<Compra> Compras => _database.GetCollection<Compra>("Compras");
    public IMongoCollection<Comentario> Comentarios => _database.GetCollection<Comentario>("Comentarios");
    public IMongoCollection<Categoria> Categorias => _database.GetCollection<Categoria>("Categorias");
}
