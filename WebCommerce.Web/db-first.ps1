﻿dotnet dotnet-ef dbcontext scaffold "Server=localhost; Port=5432; Database=efcore_presentation; User Id=postgres; Password=postgres" --context CommerceDbContext --output-dir Entities --no-onconfiguring --force --no-build Npgsql.EntityFrameworkCore.PostgreSQL