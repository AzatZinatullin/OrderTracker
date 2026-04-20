namespace OrderTracker.Shared.Constants
{
    /// <summary>
    /// Константы, определяющие ограничения для заказов.
    /// </summary>
    public static class OrderLimits
    {
        /// <summary>
        /// Максимальная длина описания заказа.
        /// </summary>
        public const int DescriptionMaxLength = 1000;

        /// <summary>
        /// Минимальная длина описания заказа.
        /// </summary>
        public const int DescriptionMinLength = 1;

        /// <summary>
        /// Максимальная длина номера заказа.
        /// </summary>
        public const int NumberMaxLength = 50;

        /// <summary>
        /// Минимальная длина номера заказа.
        /// </summary>
        public const int NumberMinLength = 10;

        /// <summary>
        /// Максимальная длина статуса заказа.
        /// </summary>
        public const int StatusMaxLength = 30;
    }
}
