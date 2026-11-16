# Deployment

How SleepyDuck runs in production. The app lives on one Hostinger VPS managed by [Dokku](https://dokku.com), and every push to `main` redeploys it.

| Item         | Value                                             |
| ------------ | ------------------------------------------------- |
| Live URL     | https://sleepyduck.nodox.cloud                    |
| Server       | Hostinger VPS (1 vCPU, 4 GB RAM), Ubuntu          |
| Server IP    | `187.77.214.124`                                  |
| Platform     | Dokku 0.38.34 (installs Docker and Nginx)         |
| Dokku app    | `sleepyduck`                                      |
| Database     | Dokku Postgres service `sleepyduck-db`            |
| SSL          | Let's Encrypt, renewed automatically              |
| Deploy       | GitHub Actions, on push to `main`                 |

This file holds no passwords or keys. Keep it that way: secrets live in Dokku and in GitHub Secrets, never in the repo.

## How a request is served

```text
Browser
  │  https://sleepyduck.nodox.cloud
  ▼
DNS wildcard  *.nodox.cloud  →  187.77.214.124
  ▼
Nginx on the VPS (managed by Dokku)      terminates SSL, picks the app by domain
  ▼
sleepyduck container, port 80
  ├── Nginx inside the container
  │     ├── /        →  Angular build in /var/www/html
  │     └── /api/    →  http://127.0.0.1:5080
  └── .NET API on 127.0.0.1:5080
        ▼
sleepyduck-db container (PostgreSQL), reachable only on Docker's private network
```

The frontend and the API share one domain, so there is no CORS setup. This mirrors development, where `proxy.conf.json` forwards `/api` to the backend.

## How a deploy works

1. A commit lands on `main`.
2. `.github/workflows/deploy.yml` runs. It pushes the repo to Dokku over SSH, using the `SSH_PRIVATE_KEY` secret.
3. Dokku builds the image from the `Dockerfile` in the repo root. The build runs on the VPS and takes a few minutes.
4. The new container starts. The API applies any pending EF Core migrations and seed data.
5. If the container stays up, Dokku switches traffic to it and removes the old one. If it fails, the old version keeps running.

Database data is not touched by a deploy.

A push is all a normal update needs. Two changes need one more step:

| Change in the code                  | Extra step                                                              |
| ----------------------------------- | ----------------------------------------------------------------------- |
| A new variable in `.env.example`    | Set it on the server: `dokku config:set sleepyduck NAME='value'`        |
| A change to the data model          | Create the migration locally and commit it. It is applied on deploy     |

## Files in the repo that production uses

| File                              | Purpose                                                                         |
| --------------------------------- | ------------------------------------------------------------------------------- |
| `Dockerfile`                      | Builds Angular, publishes the API, and packs both with Nginx into one image     |
| `nginx.conf`                      | The Nginx config inside the container: static files, plus `/api/` to the API    |
| `.github/workflows/deploy.yml`    | Pushes `main` to Dokku                                                          |

`docker-compose.yml` and `.env` are for local development only. Dokku does not read them.

`Program.cs` runs `Database.Migrate()` in every environment. It used to run only in Development, which would have left the production database empty.

## Configuration and secrets

The API reads its settings from environment variables. Locally they come from `.env`. In production Dokku injects them into the container.

| Variable                       | Set by                              | Purpose                                   |
| ------------------------------ | ----------------------------------- | ----------------------------------------- |
| `AUTH_EMAIL`, `AUTH_PASSWORD`  | `dokku config:set`                  | The single sign-in account for the app    |
| `ConnectionStrings__Default`   | `dokku config:set`                  | Full PostgreSQL connection string         |
| `DATABASE_URL`                 | `dokku postgres:link`, automatic    | Not used by the API                       |
| `ASPNETCORE_URLS`              | `Dockerfile`                        | Makes the API listen on `127.0.0.1:5080`  |
| `ASPNETCORE_FORWARDEDHEADERS_ENABLED` | `Dockerfile`                 | Lets the API see the real client IP and `https` behind the proxies |

`POSTGRES_DB`, `POSTGRES_USER`, and `POSTGRES_PASSWORD` are not needed in production, because `ConnectionStrings__Default` takes priority in `Program.cs`.

```sh
dokku config:show sleepyduck                           # list everything (shows secrets)
dokku config:set sleepyduck AUTH_PASSWORD='new-value'  # change one, restarts the app
```

Use single quotes around values so the shell doesn't alter symbols such as `$` or `!`.

The only secret in GitHub is `SSH_PRIVATE_KEY` (Settings → Secrets and variables → Actions). It lets the workflow push to Dokku and nothing else. Its public half is registered on the server under the name `github-actions`.

## Everyday commands

Run these on the VPS, after `ssh root@187.77.214.124`.

| Task                          | Command                                    |
| ----------------------------- | ------------------------------------------ |
| List apps                     | `dokku apps:list`                          |
| App status                    | `dokku ps:report sleepyduck`               |
| Live logs                     | `dokku logs sleepyduck -t`                 |
| Restart                       | `dokku ps:restart sleepyduck`              |
| Rebuild without a new commit  | `dokku ps:rebuild sleepyduck`              |
| Domains                       | `dokku domains:report sleepyduck`          |
| Port mapping                  | `dokku ports:report sleepyduck`            |
| Certificates and expiry       | `dokku letsencrypt:list`                   |
| Memory and CPU per container  | `docker stats`                             |
| Free disk space               | `df -h`                                    |
| Remove old images             | `dokku cleanup`                            |

## Database

`dokku postgres:create sleepyduck-db` made a PostgreSQL container with a generated password.

| Item      | Value                                             |
| --------- | ------------------------------------------------- |
| Service   | `sleepyduck-db`                                   |
| Database  | `sleepyduck_db`                                   |
| User      | `postgres`                                        |
| Password  | Generated. Shown by `dokku postgres:info`         |
| Exposure  | Private Docker network only, not the internet     |

The database login is separate from `AUTH_EMAIL` and `AUTH_PASSWORD`. Those sign you in to the web app. The database login is what the API uses to reach PostgreSQL.

```sh
dokku postgres:info sleepyduck-db       # host, internal IP, password, status
dokku postgres:connect sleepyduck-db    # open psql, no password needed
```

Inside `psql`: `\dt` lists tables, `\q` quits.

To use a desktop client such as DBeaver, open an SSH tunnel from your computer instead of exposing the database. Take `Internal ip` from `dokku postgres:info sleepyduck-db`:

```sh
ssh -L 5433:INTERNAL_IP:5432 root@187.77.214.124
```

Then connect to `localhost`, port `5433`, database `sleepyduck_db`, user `postgres`. The internal IP can change when the container restarts, so check it each time.

Do not use `dokku postgres:expose`. It publishes the database to the internet, and UFW does not block ports that Docker publishes.

### Backups

```sh
dokku postgres:export sleepyduck-db > /root/sleepyduck-$(date +%F).dump    # back up
dokku postgres:import sleepyduck-db < /root/sleepyduck-2026-11-16.dump     # restore
```

Backups are manual for now. Copy the dump off the server, because a backup on the same disk doesn't survive losing the VPS.

### Resetting all progress

This deletes every row and cannot be undone. Take a backup first.

```sh
dokku ps:stop sleepyduck
dokku postgres:unlink sleepyduck-db sleepyduck
dokku postgres:destroy sleepyduck-db
dokku postgres:create sleepyduck-db
dokku postgres:link sleepyduck-db sleepyduck
```

The new database has a new password, so set `ConnectionStrings__Default` again with the block in [Create the app](#3-create-the-app), then run `dokku ps:start sleepyduck`.

## DNS

DNS for `nodox.cloud` is managed in Hostinger (hPanel → Domains → DNS / Nameservers).

| Type | Name | Points to         | Purpose                                         |
| ---- | ---- | ----------------- | ----------------------------------------------- |
| A    | `*`  | `187.77.214.124`  | Sends every subdomain to the VPS                |

The wildcard is why `sleepyduck.nodox.cloud` has no record of its own, and why it doesn't show in Hostinger's Subdomains tab. Dokku decides which app answers for each subdomain. A new project needs no DNS change.

## Server setup, from a clean Ubuntu

These steps were run once. They are here in case the server has to be rebuilt.

### 1. Base system and Dokku

```sh
apt update && apt upgrade -y

# 2 GB of swap on disk, as a safety net for builds. It does not reduce RAM.
fallocate -l 2G /swapfile && chmod 600 /swapfile
mkswap /swapfile && swapon /swapfile
echo '/swapfile none swap sw 0 0' >> /etc/fstab

# Dokku
wget -NP . https://dokku.com/install/v0.38.34/bootstrap.sh
DOKKU_TAG=v0.38.34 bash bootstrap.sh
dokku version
```

### 2. Global settings, plugins, and firewall

```sh
# Each new app gets <app>.nodox.cloud automatically.
dokku domains:set-global nodox.cloud

dokku plugin:install https://github.com/dokku/dokku-postgres.git
dokku plugin:install https://github.com/dokku/dokku-letsencrypt.git
dokku letsencrypt:set --global email YOUR_EMAIL
dokku letsencrypt:cron-job --add     # automatic renewal

ufw allow OpenSSH
ufw allow 80
ufw allow 443
ufw --force enable
```

### 3. Create the app

```sh
dokku apps:create sleepyduck
dokku postgres:create sleepyduck-db
dokku postgres:link sleepyduck-db sleepyduck
dokku git:set sleepyduck deploy-branch main
dokku ports:set sleepyduck http:80:80
dokku config:set sleepyduck AUTH_EMAIL='YOUR_EMAIL' AUTH_PASSWORD='A_LONG_PASSWORD'

# Build the connection string from the service, without copying the password by hand.
DSN=$(dokku postgres:info sleepyduck-db --dsn)
DB_PASS=$(echo "$DSN" | sed -E 's|^postgres://[^:]+:([^@]+)@.*$|\1|')
DB_HOST=$(echo "$DSN" | sed -E 's|^.*@([^:]+):.*$|\1|')
DB_NAME=$(echo "$DSN" | sed -E 's|^.*/([^/]+)$|\1|')
dokku config:set sleepyduck ConnectionStrings__Default="Host=$DB_HOST;Port=5432;Database=$DB_NAME;Username=postgres;Password=$DB_PASS"
```

### 4. Deploy key for GitHub Actions

```sh
ssh-keygen -t ed25519 -f /root/github-actions -N "" -C "github-actions"
dokku ssh-keys:add github-actions /root/github-actions.pub
cat /root/github-actions     # copy all of it into the SSH_PRIVATE_KEY secret on GitHub
rm /root/github-actions
```

Copy the whole key, including the `BEGIN` and `END` lines, and leave one empty line at the end of the secret.

### 5. First deploy and SSL

Push to `main`, or re-run the workflow from the Actions tab. Once it is green:

```sh
dokku letsencrypt:enable sleepyduck
```

HTTPS does not work until this has run.

## Adding another project to the same server

Each app runs in its own container, so every project can use the same internal ports. Dokku routes by domain. For a project called `inventory`:

1. On the VPS, repeat [Create the app](#3-create-the-app) with the new name. It will answer at `inventory.nodox.cloud`.
2. On the VPS, create a separate deploy key as in step 4, with a different name, and save it as that repo's `SSH_PRIVATE_KEY` secret.
3. In the new repo, add a `Dockerfile` and copy `deploy.yml`, changing the app name at the end of `git_remote_url`.
4. Push, then run `dokku letsencrypt:enable inventory`.

The limits are the single CPU core and 4 GB of RAM. Builds run one at a time and slow the other apps while they run. Watch `docker stats` and `df -h` as projects are added.

## Troubleshooting

| Symptom                                                        | Cause                                                        | Fix                                                                  |
| -------------------------------------------------------------- | ------------------------------------------------------------ | -------------------------------------------------------------------- |
| Workflow: `Error loading key ... error in libcrypto`           | The `SSH_PRIVATE_KEY` secret is incomplete or badly pasted   | Generate a new key (step 4) and paste it whole                       |
| Workflow: `Permission denied (publickey)`                      | The key is not registered in Dokku                           | `dokku ssh-keys:list`, then step 4                                   |
| Workflow: `Unable to select a buildpack`                       | No `Dockerfile` in the repo root                             | Restore the `Dockerfile`                                             |
| Workflow fails during the build                                | A compile error, or the server ran out of memory             | Read the Actions log. Check `free -h` on the VPS                     |
| Browser: `ERR_SSL_UNRECOGNIZED_NAME_ALERT`                     | The app has no certificate yet                               | `dokku letsencrypt:enable sleepyduck`                                |
| Sign-in returns `401`                                          | `AUTH_EMAIL` or `AUTH_PASSWORD` on the server differ         | `dokku config:show sleepyduck`, then set them again                  |
| Sign-in says "Too many attempts" (`429`)                       | The rate limit of 5 attempts per minute                      | Wait a minute, or restart the app                                    |
| `502 Bad Gateway` on `/api`                                    | The API crashed or didn't start                              | `dokku logs sleepyduck`. Check the three required variables are set  |
| Site unreachable after a server reboot                         | A container didn't come back                                 | `dokku ps:report sleepyduck`, then `dokku ps:restart sleepyduck`     |
| Disk full                                                      | Old Docker images                                            | `dokku cleanup`, then `docker system prune -af`                      |

## Security notes

- Only ports 22, 80, and 443 are open in UFW.
- The database is not reachable from the internet.
- `.env` is git-ignored and must never be committed. Production uses a different sign-in password from local development.
- If the deploy key leaks, remove it with `dokku ssh-keys:remove github-actions` and create a new one.
- The server IP is public through DNS, so listing it here reveals nothing new.