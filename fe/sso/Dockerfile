FROM nginx:1.28.0-alpine3.21 AS runner
COPY nginx.conf /etc/nginx/nginx.conf
COPY --chown=nginx:nginx dist/*/browser /usr/share/nginx/html/sso
RUN chown -R nginx:nginx /usr/share/nginx/html && \
    chmod -R 755 /usr/share/nginx/html
USER nginx

EXPOSE 8080

ENTRYPOINT ["nginx", "-c", "/etc/nginx/nginx.conf"]
CMD ["-g", "daemon off;"]