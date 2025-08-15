/// <summary>
/// Representa um assento com um preço e status de ocupação.
/// </summary>
namespace SeatHandler;

public class Seat
{
    /// <summary>
    /// Obtém o preço do assento.
    /// </summary>
    public decimal Price { get; private set; }

    /// <summary>
    /// Obtém um valor que indica se o assento está ocupado.
    /// </summary>
    public bool Occupied { get; private set; }

    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="Seat"/>.
    /// </summary>
    /// <param name="price">O preço do assento.</param>
    /// <param name="occupied">O status inicial de ocupação. O padrão é <c>false</c>.</param>
    public Seat(decimal price, bool occupied = false)
    {
        Price = price;
        Occupied = occupied;
    }

    /// <summary>
    /// Tenta reservar o assento.
    /// </summary>
    /// <returns>
    /// <c>true</c> se o assento foi reservado com sucesso;
    /// <c>false</c> se o assento já estava ocupado.
    /// </returns>
    public bool BookSeat()
    {
        if (Occupied)
        {
            return false;
        }
        return Occupied = true;
    }

    /// <summary>
    /// Tenta liberar a reserva do assento.
    /// </summary>
    /// <returns>
    /// <c>true</c> se o assento foi liberado com sucesso;
    /// <c>false</c> se o assento já estava livre.
    /// </returns>
    public bool UnBookSeat()
    {
        if (!Occupied)
        {
            return false;
        }
        return Occupied = false;
    }
}