SELECT fk.object_id [ObjectId]
, fk.schema_id [SchemaId]
, fk.parent_object_id [TableId]
, fk.name [Name]
FROM sys.foreign_keys fk
JOIN sys.tables t ON fk.parent_object_id = t.object_id