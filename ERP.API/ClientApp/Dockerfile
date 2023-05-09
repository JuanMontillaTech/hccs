FROM node:16 as builder

WORKDIR /app

COPY . .
RUN npm install -g npm@6.14.8
RUN npm install




FROM node:16

WORKDIR /app

COPY --from=builder /app  .

ENV HOST 0.0.0.0


CMD [ "npm","run", "start" ]
