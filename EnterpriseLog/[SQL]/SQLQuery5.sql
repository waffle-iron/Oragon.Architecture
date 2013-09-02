declare @step int;
declare @StepTime datetime = GETDATE();
select  @step = MAX(step)+1 from #tmp_Status 
if(@step is null)
set @step = 1;
insert into #tmp_Status 
SELECT 	
	*
	--into #tmp_Status
from 
(
	SELECT COUNT(*) as QTD, 'LogEntry' as TableName, @step as Step,@StepTime as TimeInfo 					FROM DBO.LogEntry WITH (NOLOCK) union all
	SELECT COUNT(*) as QTD, 'LogEntryTagValue' as TableName, @step as Step,@StepTime as TimeInfo			FROM DBO.LogEntryTagValue WITH (NOLOCK) union all
	SELECT COUNT(*) as QTD, 'Tag' as TableName, @step as Step,@StepTime as TimeInfo							FROM DBO.Tag WITH (NOLOCK) union all
	SELECT COUNT(*) as QTD, 'TagValue' as TableName, @step as Step,@StepTime as TimeInfo					FROM DBO.TagValue WITH (NOLOCK) 
) as T


select * from #tmp_Status where tableName in ('LogEntry') and (step > @step-300) order by  TableName ,Step desc


/*
--UPDATE RuleSet SET ApplyOnReceive = 0
delete from #tmp_Status
DELETE FROM TAG
DELETE FROM LOGENTRY
*/