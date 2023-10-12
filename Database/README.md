database to code

```
dotnet ef dbcontext scaffold "Server=localhost;Database=vv_airline;User ID=admin;Password=password;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer --data-annotations -o Models -c "AppDBContext"
```
