Para rodar localmente devese ter SDK .NET 8 instalado
(Opcional) Uma IDE como Visual Studio, Rider ou VS Code com extensão C#

1. 🧰 Estrutura do projeto
sales-poc-kafka/
├── docker-compose.yml
├── src/
│   ├── SalesService/
│   ├── InventoryService/
│   ├── BillingService/
│   ├── NotificationService/
└── tests/
    └── SalesService.Tests/

2. 🧪 Para subir as dependencias do projeto use os comandos a seguir
Irar rodar kafka e todos as dependeicas do projeto
docker compose up --build

3. 🔄 Executar migrações EF Core
docker exec -it <nome_do_container_sales_service> /bin/bash


4. 🌐 Acessar o Swagger do SalesService
http://localhost:5001/swagger

Você verá o endpoint POST /sales.

5. 📤 Criar uma venda (exemplo de payload JSON)
{
  "product": "Notebook",
  "quantity": 2,
  "totalPrice": 4200
}

6. ✅ O que deve acontecer
  6.1 - A venda será salva no banco (sales-db)
  6.2 - Um evento será publicado no Kafka (sales-created)
  6.3 - Os consumidores vão processar esse evento:
  6.4 - InventoryService loga que atualizou estoque
  6.5 - BillingService cria uma fatura no banco (billing-db)
  6.6 - NotificationService loga que notificou o cliente

❓ Se algo não funcionar
Você pode verificar logs com:
  docker compose logs -f sales-service
  docker compose logs -f billing-service
E verificar se os containers estão rodando:
  docker ps

Para acessar krafka pela UI acessar 
http://localhost:9080/
