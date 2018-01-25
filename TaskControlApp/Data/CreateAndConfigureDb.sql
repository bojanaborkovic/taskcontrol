print 'Starting database deploy.....'
GO

:setvar path D:\repos\taskcontrol\TaskControlApp\Data\data.common

print 'Creating database TaskControl - CreateTaskControlDb.sql'
:r $(path)\CreateTaskControlDb.sql
GO

print 'Creating database schema - TaskControlInit.sql'
:r $(path)\TaskControlInit.sql
GO

print 'Inserting common data - PostDeploy.Data.sql'
:r $(path)\PostDeploy.Data.sql
GO

print 'Creating triggers - AuditTaskAsigneeTrigger.sql'
:r $(path)\AuditTaskAsigneeTrigger.sql
GO

print 'Creating triggers - AuditTaskStatusTrigger.sql'
:r $(path)\AuditTaskStatusTrigger.sql
GO

print 'Finished deploying database'
GO