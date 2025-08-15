/// <summary>
/// Representa o mapa de assentos de um local, gerenciando a reserva e liberação dos assentos.
/// </summary>
namespace SeatHandler;

public class Map
{
    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="Map"/> com 15 fileiras e 40 assentos por fileira,
    /// com preços baseados na faixa de fileira.
    /// </summary>
    public Map()
    {
        Seats = new Seat[15, 40];
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 40; j++)
            {
                Seats[i, j] = new Seat(50);
            }
        }

        for (int i = 5; i < 10; i++)
        {
            for (int j = 0; j < 40; j++)
            {
                Seats[i, j] = new Seat(30);
            }
        }
        
        for (int i = 10; i < 15; i++)
        {
            for (int j = 0; j < 40; j++)
            {
                Seats[i, j] = new Seat(15);
            }
        }

        BoxOffice = 0;
    }
    
    /// <summary>
    /// Obtém a matriz bidimensional de assentos.
    /// </summary>
    public Seat[,] Seats { get; private set; }

    /// <summary>
    /// Obtém o valor total da bilheteria.
    /// </summary>
    public decimal BoxOffice {get; private set;}

    /// <summary>
    /// Tenta reservar um assento em uma linha e coluna específicas e atualiza a bilheteria.
    /// </summary>
    /// <param name="row">A linha do assento (índice baseado em 0).</param>
    /// <param name="col">A coluna do assento (índice baseado em 0).</param>
    /// <returns>
    /// <c>true</c> se o assento foi reservado com sucesso;
    /// <c>false</c> se o assento já estava ocupado.
    /// </returns>
    public bool BookSeat(int row, int col)
    {
        var result = Seats[row, col].BookSeat();
        if (result)
        {
            BoxOffice += Seats[row, col].Price;
        }
        return result;
    }

    /// <summary>
    /// Tenta liberar um assento em uma linha e coluna específicas e deduz o valor da bilheteria.
    /// </summary>
    /// <param name="row">A linha do assento (índice baseado em 0).</param>
    /// <param name="col">A coluna do assento (índice baseado em 0).</param>
    /// <returns>
    /// <c>true</c> se o assento foi liberado com sucesso;
    /// <c>false</c> se o assento já estava livre.
    /// </returns>
    public bool UnBookSeat(int row, int col)
    {
        var result = Seats[row, col].UnBookSeat();
        if (result)
        {
            BoxOffice -= Seats[row, col].Price;
        }
        return result;
    }

    /// <summary>
    /// Tenta reservar múltiplos assentos de uma lista de localizações.
    /// </summary>
    /// <param name="locations">Uma coleção de arrays de inteiros, onde cada array representa a linha e a coluna de um assento.</param>
    /// <returns>
    /// Uma lista de localizações de assentos que não puderam ser reservados por já estarem ocupados.
    /// </returns>
    public List<int[]> BookSeatsRange(IEnumerable<int[]> locations)
    {
        var occupied =  new List<int[]>();
        foreach (int[] location in locations)
        {
            var result = BookSeat(location[0], location[1]);
            if (!result)
            {
                occupied.Add(location);
            }
        }
        return occupied;
    }

    /// <summary>
    /// Tenta liberar múltiplos assentos de uma lista de localizações.
    /// </summary>
    /// <param name="locations">Uma coleção de arrays de inteiros, onde cada array representa a linha e a coluna de um assento.</param>
    /// <returns>
    /// Uma lista de localizações de assentos que não puderam ser liberados por já estarem desocupados.
    /// </returns>
    public List<int[]> UnBookSeatsRange(IEnumerable<int[]> locations)
    {
        var unoccupied = new List<int[]>();
        
        foreach (int[] location in locations)
        {
            var result = UnBookSeat(location[0], location[1]);
            if (!result)
            {
                unoccupied.Add(location);
            }
        }
        return unoccupied;
    }
}