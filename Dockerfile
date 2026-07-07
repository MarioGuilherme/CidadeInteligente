# =========================
# Build stage
# =========================
FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS build
WORKDIR /src

# Copy project files first (better Docker layer cache)
COPY CidadeInteligente.Mvc/CidadeInteligente.Mvc.csproj CidadeInteligente.Mvc/
COPY CidadeInteligente.Application/CidadeInteligente.Application.csproj CidadeInteligente.Application/
COPY CidadeInteligente.Domain/CidadeInteligente.Domain.csproj CidadeInteligente.Domain/
COPY CidadeInteligente.Infrastructure/CidadeInteligente.Infrastructure.csproj CidadeInteligente.Infrastructure/

# Restore dependencies
RUN dotnet restore CidadeInteligente.Mvc/CidadeInteligente.Mvc.csproj

# Copy remaining source code
COPY . .

# Publish
WORKDIR /src/CidadeInteligente.Mvc
RUN dotnet publish -c Release -o /app/publish --no-restore

# =========================
# Runtime stage (Alpine)
# =========================
FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine AS runtime
WORKDIR /app

# ICU (globalization) + tzdata (timezone) + culture pt-BR
RUN apk add --no-cache icu-libs icu-data-full tzdata
ENV TZ=America/Sao_Paulo \
    LANG=pt_BR.UTF-8 \
    LANGUAGE=pt_BR:pt \
    LC_ALL=pt_BR.UTF-8 \
    DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

# Create non-root user (security best practice)
RUN addgroup -S appgroup && adduser -S appuser -G appgroup

# Copy published output (owned by appuser, avoids a chown layer)
COPY --from=build --chown=appuser:appgroup /app/publish .

USER appuser

EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080 \
    ASPNETCORE_ENVIRONMENT=Production

# Healthcheck (useful outside k8s, e.g., docker compose / standalone docker run;
# inside k8s, the manifest probes take on this role)
HEALTHCHECK --interval=30s --timeout=5s --start-period=10s --retries=3 \
  CMD wget --no-verbose --tries=1 --spider http://localhost:8080/health || exit 1

ENTRYPOINT ["dotnet", "CidadeInteligente.Mvc.dll"]
