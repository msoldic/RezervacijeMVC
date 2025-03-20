-----T-SQL--INSERT--


DECLARE @i int = 0
DECLARE @randomString varchar(20)

WHILE @i < 50000
BEGIN
    SET @i = @i + 1
	SET @randomString = SUBSTRING(CONVERT(varchar(255), NEWID()),0,20)
	INSERT INTO [dbo].[Tools]
           ([ToolType]
           ,[PriceRentPerDay])
     VALUES (@randomString, CEILING(RAND()*10));
END

---Tool Reservation---


DECLARE @ToolID INT;
DECLARE @TotalRentPrice INT;
DECLARE @ClientFirstName NVARCHAR(20);
DECLARE @ClientSecondName NVARCHAR(20);
DECLARE @DateReservationFrom DATE;
DECLARE @DateReservationTo DATE;
DECLARE @Counter int = 1

WHILE @Counter <= 10000
BEGIN
    SET @ClientFirstName = 'Client_' + CAST(@Counter AS NVARCHAR(10));
    SET @ClientSecondName = 'Surname_' + CAST(@Counter AS NVARCHAR(10));

    SET @DateReservationFrom = DATEADD(DAY, (RAND() * 30), '2025-03-01');
    SET @DateReservationTo = DATEADD(DAY, (RAND() * 30), @DateReservationFrom);


    IF @DateReservationTo <= @DateReservationFrom
    BEGIN
        SET @DateReservationTo = DATEADD(DAY, 1, @DateReservationFrom);
    END

    -- Random ToolID (between 1 and 50000)
    SET @ToolID = (FLOOR(RAND() * 50000) + 1);

    SET @TotalRentPrice = DATEDIFF(DAY, @DateReservationFrom, @DateReservationTo) * 
                          (SELECT [PriceRentPerDay] FROM [dbo].[Tools] WHERE ID = @ToolID);
						  
    INSERT INTO ToolReservations (ClientFirstName, ClientSecondName, DateReservationFrom, DateReservationTo, ToolID, TotalRentPrice)
    VALUES (@ClientFirstName, @ClientSecondName, @DateReservationFrom, @DateReservationTo, @ToolID, @TotalRentPrice);

    SET @Counter = @Counter + 1;
END
