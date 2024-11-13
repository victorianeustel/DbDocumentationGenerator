SELECT DB_NAME() [DatabaseName]
    , v.schema_id [SchemaId]
    , s.name [SchemaName]
    , v.object_id [ObjectId]
    , v.name [ViewName]
FROM sys.views v
    JOIN sys.schemas s ON s.schema_id = v.schema_id 