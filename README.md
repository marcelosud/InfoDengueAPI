# InfoDengueAPI

API para consulta de dados epidemiológicos da plataforma INFODENGUE, com integração e armazenamento de dados no SQL Server.

## 📦 Estrutura do Projeto
- `/InfoDengueAPI.Domain` - Entidades do domínio
- `/InfoDengueAPI.Infrastructure` - Configurações de banco de dados e migrations
- `/InfoDengueAPI.Application` - Serviços e interfaces
- `/InfoDengueAPI.WebAPI` - API Web

## 🚀 Como Executar o Projeto

1. Clone o repositório:
```bash
git clone https://github.com/seu-usuario/InfoDengueAPI.git

2. Acesse a pasta do projeto:

cd InfoDengueAPI

3. Instale as dependências:

dotnet restore

4. Atualize o banco de dados:

cd InfoDengueAPI.WebAPI
dotnet ef database update

Execute a aplicação:

dotnet run
Acesse a documentação da API no navegador:

https://localhost:7162/swagger

-  O End-Point "/api/Relatorio/consulta-externa" é o end-poin principal que faz a consulta na API (INFODENGUE: https://info.dengue.mat.br/services/api)

-  Os outros end-points do projeto realizam as outras funcionalidades:

Relatórios:
-  Listar todos os dados epidemiológicos do município do Rio de Janeiro e São Paulo;
-  Listar os dados epidemiológicos dos municípios pelo código IBGE;
-  Listar o total de casos epidemiológicos dos municípios do Rio de Janeiro e São Paulo;
-  Listar o total de casos epidemiológicos dos municípios por arbovirose;
-  Listar os solicitantes;


🗄️ Banco de Dados
O arquivo BancoDados\Scripts\InfoDengueDB.sql contém o dump do banco de dados SQL Server utilizado no projeto.

Outra opção é a restauração do Backup do Banco de Dados.
O arquivo BancoDados\Scripts\InfoDengueDB.bak contém o backup do banco de dados para ser utilizado para o dump do banco de dados SQL Server utilizado no projeto.

Alterar o arquivo "\InfoDengueAPI\InfoDengueAPI.WebAPI\appsettings.json" que contém a "ConnectionStrings" para a conexeção de servidor mais adequada para o ambiente onde será realizado os testes e validações.
