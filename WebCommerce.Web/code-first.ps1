# Add new migration.
dotnet dotnet-ef migrations add Initial --context CommerceDbContext

# Remove last migration.
dotnet dotnet-ef migrations remove --context CommerceDbContext

# Create database & schema.
dotnet dotnet-ef database update --context CommerceDbContext

# Generate SQL script.
dotnet dotnet-ef migrations script -o "Scripts/1-initial.sql" --context CommerceDbContext