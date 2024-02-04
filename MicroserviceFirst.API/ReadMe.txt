- sql server docker
 https://hub.docker.com/_/microsoft-mssql-server



- open change data capture in sql server 2012

USE ChangeDataCaptureDb
EXEC sys.sp_cdc_enable_db

- open change data capture for table

USE ChangeDataCaptureDb
GO

EXEC sys.sp_cdc_enable_table
@source_schema = N'dbo',
@source_name   = N'Products',
@role_name     = NULL,
@filegroup_name = N'',
@supports_net_changes = 0
GO



This image defines a runnable Kafka Connect service preconfigured with all Debezium connectors
The service has a RESTful API for managing connector instances
https://hub.docker.com/r/debezium/connect

 - simply start up a container,
 - configure a connector for each data source you want to monitor
 - let Debezium monitor those sources for changes and forward them to the appropriate Kafka topics.

 GROUP_ID :  Set this to an ID that uniquely identifies the Kafka Connect cluster the service and its workers belong to.

 CONFIG_STORAGE_TOPIC :  Set this to the name of the Kafka topic where the Kafka Connect services in the group store connector configurations

 OFFSET_STORAGE_TOPIC : . Set this to the name of the Kafka topic where the Kafka Connect services in the group store connector offsets.


 Kafka Connect REST API For Debezium

 {
    "name": "product-connector", 
    "config": {
        "connector.class": "io.debezium.connector.sqlserver.SqlServerConnector", 
        "database.hostname": "sqlserver-db", 
        "database.port": "1433", 
        "database.user": "sa", 
        "database.password": "Your_password123", 
        "database.names": "ChangeDataCaptureDb",  
        "table.include.list": "dbo.products", 
        "topic.prefix": "fullfillment",
        "schema.history.internal.kafka.bootstrap.servers": "kafka:9092", 
        "schema.history.internal.kafka.topic": "schemahistory.fullfillment",
        "database.encrypt":false,
        "decimal.handling.mode":"string"
    }
}



satırdaki ‘op’ alanındaki c, u, d ve r değerleri ifade etmektedir.





CDC işlemi için MSSQL sürümünüzün Enterprise, Developer, Enterprise Evaluation, ya da Standard sürümlerinden birisi olması gerekiyor.