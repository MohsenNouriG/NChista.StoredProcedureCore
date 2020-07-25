# Execute stored procedures using EF Core 
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
