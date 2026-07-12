# 1. Etapa de compilación (Usa la imagen completa de Go que viene con GCC e instrumentación de C)
FROM golang:1.25.5 AS builder

# Instalar herramientas de compilación de C para Linux
RUN apt-get update && apt-get install -y gcc g++ make

WORKDIR /app

# Copiar archivos de dependencias
COPY go.mod go.sum ./
RUN go mod download

# Copiar todo el código fuente del worker
COPY . .

# Compilar con CGO habilitado apuntando a tu archivo main.go dentro de la carpeta main
RUN CGO_ENABLED=1 GOOS=linux go build -o whatsapp-worker ./main/main.go

# 2. Etapa de ejecución (Contenedor final ultra liviano)
FROM debian:bookworm-slim
RUN apt-get update && apt-get install -y ca-certificates && rm -rf /var/lib/apt/lists/*

WORKDIR /app

# Copiar el binario compilado y crear la carpeta para la base de datos de sesión
COPY --from=builder /app/whatsapp-worker .
RUN mkdir data

CMD ["./whatsapp-worker"]