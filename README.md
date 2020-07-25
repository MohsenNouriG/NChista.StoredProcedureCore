<div align="center">
  
  <img src="NChista.StoredProcedureCore/Images/icon.png" width="70">

  # Execute stored procedures using EF Core 
</div>

This library simplifies executing stored procedures using EF Core.

## API
### DbContextExtensions
```csharp
IStoredProcedure StoredProcedure(this DbContext dbContext, string name)
```
### IStoredProcedure
```csharp
IStoredProcedure  AddParameters<T>(T model); // Mapping a model with properties that contain SpParameterAttribute
int               ExecuteNonQuery(); // Executes a stored procedure and reurns the number of rows affected.
Task<int>         ExecuteNonQueryAsync(); // An asynchronous version of ExecuteNonQuery.
void              ExecuteReader(Action<DbDataReader> action); // Executes a stored procedure and calls a delegate method by passing DbDataReader as a result.
Task              ExecuteReaderAsync(Func<DbDataReader, Task> action); // An asynchronous version of ExecuteReader.
```
### DbDataReaderExtensions
```csharp
List<TDestination>        GetResult<TDestination>(this DbDataReader reader) where TDestination : new() // Mapping the result stes to the list of type TDestination.
Task<List<TDestination>>  GetResultAsync<TDestination>(this DbDataReader reader) where TDestination : new() // An asynchronous version of GetResult.
```

## Supported Types
|.Net Type|DbType|
|---|---|
|byte|Byte|
|byte[]|Binary|
|sbyte|SByte|
|short|Int16|
|ushort|UInt16|
|int|Int32|
|uint|UInt32|
|long|Int64|
|ulong|UInt64|
|float|Single|
|double|Double|
|decimal|Decimal|
|bool|Boolean|
|string|String|
|char|StringFixedLength|
|Guid|Guid|
|DateTime|DateTime|
|DateTimeOffset|DateTimeOffset|
