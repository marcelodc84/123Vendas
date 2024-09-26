# 123Vendas


### Avaliação técnica

#### Marcelo Dalpiaz Carneiro

<br>

#### Configuração API de vendas:

Após clonar o repositório, navegar até o diretório e executar os seguintes comandos via terminal:

```
cd [path]\123Vendas\SalesAPI\API

dotnet restore
dotnet build
dotnet run
```

Talvez seja necessário configurar um certificado antes, executando o seguinte comando:
```
dotnet dev-certs https --trust
```
<br>

Após o comando dotnet run a aplicação deve estar rodando e pronta para ser testada no caminho https://localhost:7015

Endpoints:

get all sales
```
curl --location --request GET 'https://localhost:7015/api/sales'
```

get sale by id
```
curl --location --request GET 'https://localhost:7015/api/sales/123'
```

create sale:
```
curl --location --request POST 'https://localhost:7015/api/sales' \
--header 'Content-Type: application/json' \
--data-raw '{
    "SaleNumber": 123,
    "SaleDate": "2024-09-26",
    "Customer": "fooooo",
    "TotalAmount": 123234,
    "Branch": "qweqwe",
    "Items": [
        {
            "Product": "qweqqwrt",
            "Quantity": 234,
            "UnitPrice": 654,
            "Discount": 54,
            "TotalItemAmount": 54
        },
        {
            "Product": "asdfasd",
            "Quantity": 3,
            "UnitPrice": 234,
            "Discount": 12,
            "TotalItemAmount": 22
        }
    ],
    "IsCancelled": false
}'
```

update sale:
```
curl --location --request PUT 'https://localhost:7015/api/sales/123' \
--header 'Content-Type: application/json' \
--data-raw '{
    "SaleNumber": 123,
    "SaleDate": "2024-09-26",
    "Customer": "chuuuuu",
    "TotalAmount": 123234,
    "Branch": "qweqwe",
    "Items": [
        {
            "Product": "qweqqwrt",
            "Quantity": 234,
            "UnitPrice": 654,
            "Discount": 54,
            "TotalItemAmount": 54
        },
        {
            "Product": "asdfasd",
            "Quantity": 12,
            "UnitPrice": 34,
            "Discount": 56,
            "TotalItemAmount": 78
        },
        {
            "Product": "chochocho",
            "Quantity": 12,
            "UnitPrice": 34,
            "Discount": 56,
            "TotalItemAmount": 78
        }
    ],
    "IsCancelled": false
}'
```

delete sale
```
curl --location --request DELETE 'https://localhost:7015/api/sales/123'
```
