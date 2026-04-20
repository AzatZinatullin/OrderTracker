namespace OrderTracker.Shared.Exceptions
{
    /// <summary>
    /// Исключение, возникающее при ошибках в доменной логике.
    /// </summary>
    public class DomainException : Exception
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="DomainException"/>.
        /// </summary>
        /// <param name="message">Сообщение об ошибке.</param>
        public DomainException(string message) : base(message) { }
    }
}
