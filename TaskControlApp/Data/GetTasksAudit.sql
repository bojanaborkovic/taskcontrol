
CREATE PROCEDURE [dbo].[GetTasksAudit]
  @FromDate datetime2(7),
  @ToDate datetime2(7),
  @pageNumber int,
  @pageSize int,
  @orderBy varchar(50)
AS

SET NOCOUNT ON

DECLARE @sql nvarchar(max);
DECLARE @andstring nvarchar(5);

SET @sql = 'WITH SelectSqlWithRowNums AS (SELECT';

DECLARE @minNumber int;
DECLARE @maxNumber int;
IF (@pageNumber IS NOT NULL AND @pageSize IS NOT NULL)
BEGIN
  SET @minNumber = (@pageNumber - 1) * @pageSize;
  SET @maxNumber = @pageNumber * @pageSize;
  SET @sql = @sql + ' TOP(@maxNumber) ROW_NUMBER () OVER (ORDER BY ' + @orderBy +') AS [RowNumber], ';
END
ELSE
  SET @sql = @sql + ' ROW_NUMBER () OVER (ORDER BY ' + @orderBy + ') AS [RowNumber], ';

SET @sql = @sql + ' COUNT(*) OVER () AS [RecordCount],* FROM (SELECT asigneeHistory.TaskId, asigneeHistory.AsigneeBefore, asigneeHistory.AsigneeAfter,
asigneeHistory.ChangeBy AS AssigneChangedBy, asigneeHistory.ChangeDate AS AssigneChangedOn,
 statusHistory.StatusBefore, statusHistory.StatusAfter, statusHistory.ChangeBy
AS StatusChangeBy, statusHistory.ChangeDate AS StatusChangedOn FROM [dbo].TaskAsigneeHistory asigneeHistory
INNER JOIN [dbo].TaskStatusHistory statusHistory
on asigneeHistory.TaskId = statusHistory.TaskId';

IF (@FromDate IS NOT NULL OR @ToDate IS NOT NULL)
  SET @sql = @sql + ' WHERE ';

SET @andstring = '';
IF (@FromDate IS NOT NULL)
BEGIN
  SET @sql = @sql + @andstring + 'AssigneChangedOn >= @FromDate AND StatusChangedOn >= @FromDate';
  SET @andstring = ' AND ';
END

IF (@ToDate IS NOT NULL)
BEGIN
  SET @sql = @sql + @andstring + 'AssigneChangedOn <= @ToDate AND StatusChangedOn <= @ToDate';
  SET @andstring = ' AND ';
END

SET @sql = @sql + ') AS SelectSql) SELECT * FROM SelectSqlWithRowNums';

IF (@minNumber IS NOT NULL)
  SET @sql = @sql + ' WHERE [RowNumber] > @minNumber AND [RowNumber] <= @maxNumber';


EXEC sp_executesql @sql, N'@FromDate datetime2(7), @ToDate datetime2(7), @minNumber int, @maxNumber int',
                   @FromDate, @ToDate,  @minNumber, @maxNumber;

