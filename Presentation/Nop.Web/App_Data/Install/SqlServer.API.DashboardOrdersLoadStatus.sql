GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DashboardOrdersLoadStatus]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[DashboardOrdersLoadStatus]
GO

-- =============================================
-- Author:		<Author,,Name>
-- EXEC [DashboardOrdersLoadStatus]
-- =============================================
CREATE PROCEDURE [dbo].[DashboardOrdersLoadStatus]
AS
BEGIN
	
	SET NOCOUNT ON

	CREATE TABLE #TmpOrderStatus
	(
		[OrderStatusId] [int] NULL,
		[OrderCount] [int] NULL
	)

	SELECT 
		SO.[OrderStatusId],			
		COUNT(*) AS [OrderCount],
		SUM(CASE WHEN ISNULL(SO.[CreatedOnUtc], '1/1/1900') >= getdate() THEN 1 ELSE 0 END) AS [OverOrderCount]
	INTO #TmpOrderStatusData
	FROM [Order] SO 
	--INNER JOIN [Shipment] SH ON SO.[ID] = SH.[OrderId]
	WHERE SO.[Deleted] = 0 
		--AND (ISNULL(SO.[CreatedOnUtc], '1/1/1900') >= getdate())
	GROUP BY SO.[OrderStatusId]
	ORDER BY SO.[OrderStatusId]

	-- Order Pendding in Days
	INSERT INTO #TmpOrderStatus
	(
		[OrderStatusId],
		[OrderCount]
	)
	SELECT 1 AS [OrderStatusId],
		ISNULL(SUM(SO.[OrderCount]), 0) AS [OrderCount]
	FROM #TmpOrderStatusData SO
	WHERE SO.[OrderStatusId] = 10

	-- Order in Days
	INSERT INTO #TmpOrderStatus
	(
		[OrderStatusId],
		[OrderCount]
	)
	SELECT 2 AS [OrderStatusId],
		ISNULL(SUM(SO.[OrderCount]), 0) AS [OrderCount]
	FROM #TmpOrderStatusData SO

	-- Order in Days
	INSERT INTO #TmpOrderStatus
	(
		[OrderStatusId],
		[OrderCount]
	)
	SELECT 3 AS [OrderStatusId],
		ISNULL(SUM(ISNULL(SO.[OrderCount], 0) + ISNULL(SO.[OverOrderCount], 0)), 0) AS [OrderCount]
	FROM #TmpOrderStatusData SO
	WHERE SO.[OrderStatusId] IN (10, 20)

	SELECT 
		SO.[OrderStatusId],
		SO.[OrderCount]
	FROM #TmpOrderStatus SO

	DROP TABLE #TmpOrderStatus
	DROP TABLE #TmpOrderStatusData
END
GO