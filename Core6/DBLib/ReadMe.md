

執行以下命令
```
dotnet-ef dbcontext scaffold " data source=(localdb)\\MSSQLLocalDB;initial catalog=BlogDB;integrated security=True;" Microsoft.EntityFrameworkCore.SqlServer -o EF_BlogDB --use-database-names --no-pluralize --force --no-build
```