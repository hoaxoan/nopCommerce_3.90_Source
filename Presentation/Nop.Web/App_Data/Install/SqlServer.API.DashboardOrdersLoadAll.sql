GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DashboardOrdersLoadAll]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[DashboardOrdersLoadAll]
GO

-- =============================================
-- Author:		<Author,,Name>
-- EXEC [DashboardOrdersLoadAll] 0, null
-- =============================================
CREATE PROCEDURE [dbo].[DashboardOrdersLoadAll]
(
	@MarkAsDeliveryDate	int,
	@DeliveryDate	datetime
)
AS
BEGIN
	
	SET NOCOUNT ON

	CREATE TABLE #TmpOrderStatus
	(
		[OrderStatusId] [int] NULL,
		[OrderCount] [int] NULL
	)
	DECLARE
		@sql nvarchar(max)

	SET @sql = '
	INSERT INTO #TmpOrderStatus
	(
		[OrderStatusId],
		[OrderCount]
	)
	SELECT 
		SO.[OrderStatusId],			
		COUNT(*) AS [OrderCount]
	FROM [Order] SO '

	SET @sql += '
	WHERE SO.[Deleted] = 0 '

	IF(@MarkAsDeliveryDate = 1)
	BEGIN
		SET @sql += ' 
		AND (CAST(ISNULL(SO.[ShippedDateUtc], ''1/1/1900'') AS DATE) >= CAST(@DeliveryDate AS DATE))'
	END

	SET @sql += '
	GROUP BY SO.[OrderStatusId]
	ORDER BY SO.[OrderStatusId]'

	PRINT (@sql)
	EXEC sp_executesql @sql, N'@DeliveryDate datetime', @DeliveryDate = @DeliveryDate
	
	SELECT 
		SO.[OrderStatusId],
		(CASE WHEN SO.[OrderStatusId] = 10 THEN N'Đang chờ xử lý'
			WHEN SO.[OrderStatusId] = 20 THEN N'Đang xử lý'
			WHEN SO.[OrderStatusId] = 30 THEN N'Đã hoàn thành'
			WHEN SO.[OrderStatusId] = 40 THEN N'Đã hủy' 
			ELSE '''' END) AS [OrderStatusName],
		SO.[OrderCount]
	FROM #TmpOrderStatus SO

	DROP TABLE #TmpOrderStatus
END
GO