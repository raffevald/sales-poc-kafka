# 🛠️ Sales PoC Kafka

## ✅ Requisitos

- .NET 8 SDK instalado  
- (Opcional) Uma IDE como Visual Studio, Rider ou VS Code com extensão C#

---

## 🧰 Estrutura do Projeto

```
sales-poc-kafka/
├── docker-compose.yml
├── src/
│   ├── SalesService/
│   ├── InventoryService/
│   ├── BillingService/
│   ├── NotificationService/
└── tests/
    └── SalesService.Tests/
```

---

## 🧪 Subir as dependências com Docker

Para rodar Kafka e todas as dependências do projeto:

```bash
docker compose up --build
```

Para derrubar todos os containers:

```bash
docker compose down -v
```

---

## 🔄 Executar migrações EF Core

Acesse o container do SalesService:

```bash
docker exec -it <nome_do_container_sales_service> /bin/bash
```

E então execute o comando de migração desejado dentro do container.

---

## 🌐 Acessar Swagger do SalesService

Abra no navegador:

```
http://localhost:5001/swagger
```

Você verá o endpoint:

```
POST /sales
```

---

## 📤 Exemplo de Payload para Criar uma Venda

```json
{
  "product": "Notebook",
  "quantity": 2,
  "totalPrice": 4200
}
```

---

## ✅ O que Deve Acontecer

1. A venda será salva no banco de dados (`sales-db`)
2. Um evento será publicado no Kafka (`sales-created`)
3. Os consumidores vão processar esse evento:
   - `InventoryService` loga que atualizou o estoque
   - `BillingService` cria uma fatura no banco (`billing-db`)
   - `NotificationService` loga que notificou o cliente

---

## 🐞 Se Algo Não Funcionar

Verifique os logs dos serviços:

```bash
docker compose logs -f sales-service
docker compose logs -f billing-service
```

Verifique se os containers estão rodando:

```bash
docker ps
```

---

## 🧭 Acessar Interface Web do Kafka

Abra no navegador:

```
http://localhost:9080/
```
