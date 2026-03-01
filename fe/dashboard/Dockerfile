FROM node:24-bookworm AS builder
WORKDIR /app
COPY package.json package-lock.json* ./
RUN --mount=type=cache,target=/root/.npm npm ci
FROM nginx:1.28.0-alpine3.21 AS runner
COPY nginx.conf /etc/nginx/nginx.conf
COPY --chown=nginx:nginx dist/*/browser /usr/share/nginx/html/dashboard
USER nginx

EXPOSE 8080

ENTRYPOINT ["nginx", "-c", "/etc/nginx/nginx.conf"]
CMD ["-g", "daemon off;"]