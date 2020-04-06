FROM node AS builder
WORKDIR /app
COPY . /app
RUN mkdir -p /app/node_modules
#需要设置国内镜像，否则可能会安装失败
RUN npm config set registry https://registry.npm.taobao.org
RUN npm install
RUN npm run build-prod
FROM nginx
COPY --from=builder /app/dist/manage/ /usr/share/nginx/html/
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]