namespace OrderTracker.Shared.Exceptions
{
    /// <summary>
    /// Исключение, которое выбрасывается, когда запрошенный ресурс не найден.
    /// </summary>
    public class NotFoundException : Exception
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="NotFoundException"/>.
        /// </summary>
        /// <param name="message">Сообщение, описывающее причину исключения.</param>
        public NotFoundException(string message) : base(message)
        {
        }
    }
}
