using System.Collections.Generic;

public class Categorias
{
    public List<string> Categorías { get; set; }

    public Categorias()
    {
        Categorías = new List<string>
        {
            "Tambores",
            "Guitarras",
            "Bajos",
            "Micrófonos"
            // Agrega más categorías según sea necesario
        };
    }
}

