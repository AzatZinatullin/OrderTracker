-- Решение задачи 2.
CREATE OR REPLACE FUNCTION get_client_daily_payments(
    p_client_id BIGINT,
    p_sd DATE,
    p_ed DATE
)
RETURNS TABLE (dt DATE, total_amount NUMERIC) AS $$
BEGIN
    RETURN QUERY
    WITH 
    -- 1. Генерируем календарь на заданный период
    calendar AS (
        SELECT generate_series(p_sd, p_ed, '1 day'::interval)::date AS day
    ),
    -- 2. Считаем суммы платежей, сгруппированные по дням
    daily_payments AS (
        SELECT 
            cp.Dt::date AS pay_date,
            SUM(cp.Amount) AS amount_sum
        FROM ClientPayments cp
        WHERE cp.ClientId = p_client_id
          AND cp.Dt >= p_sd 
          -- Учитываем, что в Dt может быть время, поэтому берем до начала следующего дня после Ed
          AND cp.Dt < (p_ed + interval '1 day')
        GROUP BY cp.Dt::date
    )
    -- 3. Соединяем календарь с платежами
    SELECT 
        c.day,
        COALESCE(dp.amount_sum, 0)::NUMERIC
    FROM calendar c
    LEFT JOIN daily_payments dp ON c.day = dp.pay_date
    ORDER BY c.day;
END;
$$ LANGUAGE plpgsql;


-- Тестирование в PostgreSQL.
-- Создаем временную таблицу
CREATE TEMPORARY TABLE ClientPayments (
    Id bigint,
    ClientId bigint,
    Dt timestamp(0), -- В Postgres datetime2(0) обычно заменяют на timestamp(0)
    Amount numeric   -- Используем numeric для точности (аналог money)
);

-- Наполняем данными из задания
INSERT INTO ClientPayments (Id, ClientId, Dt, Amount) VALUES
(1, 1, '2022-01-03 17:24:00', 100),
(2, 1, '2022-01-05 17:24:14', 200),
(3, 1, '2022-01-05 18:23:34', 250),
(4, 1, '2022-01-07 10:12:38', 50),
(5, 2, '2022-01-05 17:24:14', 278),
(6, 2, '2022-01-10 12:39:29', 300);

-- Вызов функции
SELECT * FROM get_daily_client_payments(1, '2022-01-02', '2022-01-07');
SELECT * FROM get_daily_client_payments(2, '2022-01-04', '2022-01-11');