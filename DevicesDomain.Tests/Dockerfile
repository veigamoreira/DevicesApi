FROM mcr.microsoft.com/dotnet/sdk:9.0
WORKDIR /tests
COPY . .
RUN dotnet restore
RUN dotnet test --no-build --logger:"console;verbosity=detailed"
