# 1) Compila el frontend Angular
FROM node:22-slim AS frontend
WORKDIR /src
COPY frontend/package*.json ./
RUN npm ci
COPY frontend/ ./
RUN npx ng build --configuration production

# 2) Compila la API .NET
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS backend
WORKDIR /src
COPY backend/SleepyDuck.Api/ ./
RUN dotnet publish -c Release -o /out

# 3) Imagen final: .NET + Nginx
FROM mcr.microsoft.com/dotnet/aspnet:8.0
RUN apt-get update \
    && apt-get install -y --no-install-recommends nginx tzdata \
    && rm -rf /var/lib/apt/lists/* \
    && rm -f /etc/nginx/sites-enabled/default
WORKDIR /app
COPY --from=backend /out ./
COPY --from=frontend /src/dist/*/browser/ /var/www/html/
COPY nginx.conf /etc/nginx/conf.d/default.conf
RUN touch /app/.env
ENV ASPNETCORE_URLS=http://127.0.0.1:5080 \
    ASPNETCORE_FORWARDEDHEADERS_ENABLED=true
EXPOSE 80
CMD ["sh", "-c", "nginx && exec dotnet SleepyDuck.Api.dll"]