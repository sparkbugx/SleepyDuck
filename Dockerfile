# 1) Build the Angular frontend
FROM node:22-slim AS frontend
WORKDIR /src
COPY frontend/package*.json ./
RUN npm ci
COPY frontend/ ./
RUN npx ng build --configuration production

# 2) Build the .NET API
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS backend
WORKDIR /src
COPY backend/SleepyDuck.Api/ ./
RUN dotnet publish -c Release -o /out

# 3) Final image: .NET + Nginx
FROM mcr.microsoft.com/dotnet/aspnet:10.0
RUN apt-get update \
    && DEBIAN_FRONTEND=noninteractive apt-get install -y --no-install-recommends nginx tzdata \
    && rm -rf /var/lib/apt/lists/* \
    && rm -f /etc/nginx/sites-enabled/default
WORKDIR /app
COPY --from=backend /out ./
COPY --from=frontend /src/dist/*/browser/ /var/www/html/
COPY nginx.conf /etc/nginx/conf.d/default.conf
ENV ASPNETCORE_URLS=http://127.0.0.1:5080 \
    ASPNETCORE_FORWARDEDHEADERS_ENABLED=true
EXPOSE 80
CMD ["sh", "-c", "nginx && exec dotnet SleepyDuck.Api.dll"]