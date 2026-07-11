# 🏙️ CidadeInteligente
> Sistema gerenciador de projetos realizados por professores e alunos da Cidade Inteligente da Fatec de Lins - Professor Antônio Seabra, permitindo o cadastro, a divulgação e a administração completa dos projetos e de suas mídias.

## 🎓 Funcionalidades
  - Cadastro e autenticação de usuários (login com cookie de autenticação);
  - Recuperação e redefinição de senha via e-mail com token de expiração;
  - Cadastro de projetos com mídias (imagens e vídeos), área, curso e usuários envolvidos;
  - Listagem paginada e consulta detalhada de projetos;
  - Atualização e exclusão de projetos pelo criador ou usuários envolvidos;
  - Administração de áreas, cursos e usuários (perfil administrador);
  - Upload e remoção de arquivos de mídia em armazenamento na nuvem.

## ⚙️ Requisitos não funcionais
  - O sistema garante a segurança via autenticação por cookies com política de expiração deslizante.
  - O sistema garante a integridade dos dados com validações internas e constraints na base de dados.
  - O sistema suporta escalabilidade horizontal conforme aumento de carga com HPA.
  - O sistema garante consistência transacional com Unit of Work e ações de compensação pós-rollback.
  - O sistema protege endpoints sensíveis com rate limiting (limite de tentativas de login por IP).
  - O sistema prove observabilidade, com métricas, health checks e logs distribuídos rastreáveis por CorrelationId.
  - O sistema armazena senhas com hash BCrypt.

## 🛠️ Detalhes Técnicos
### ⭐ Arquitetura e Padrões
 - Notification Pattern (Exceptionless);
 - Padrão CQRS (Command Query Responsibility Segregation);
 - Mediator Pattern com MediatR;
 - Clean Architecture;
 - Unit of Work;
 - Specification Pattern para consultas reutilizáveis;
 - Uso de Middlewares e Action Filters para cross-cutting concerns;
 - Aplicação containerizada.

### ⚙️ Backend & Framework
 - .NET 10 com C# 14;
 - ASP.NET Core MVC com Razor Views;
 - Entity Framework Core;
 - FluentValidation para validações robustas;
 - Autenticação e autorização via Cookies;
 - Rate Limiting nativo do ASP.NET Core;
 - Health Checks com verificação do banco de dados.

### 🗄️ Banco de Dados & Armazenamento
 - SQL Server;
 - Migrations aplicadas automaticamente na inicialização;
 - Azure Blob Storage para armazenamento das mídias dos projetos;
 - SendGrid para envio de e-mails transacionais.

### 📊 Observabilidade & Monitoramento
 - Prometheus para coleta de métricas;
 - Serilog com logging estruturado;
 - Logs distribuídos com CorrelationId para rastreabilidade.

### 🧪 Testes
 - Testes unitários com xUnit;
 - FluentAssertions para assertions mais expressivas;
 - Moq para criação de mocks e isolamento de dependências.

### 🚀 DevOps & Infraestrutura
 - Docker e Docker Compose para containerização;
 - Kubernetes (Deployments, Services, HPA, ConfigMaps e Secrets).

## 🧪 Testes
  - Navegue até o diretório dos testes:
  ```
  cd ./CidadeInteligente.Tests/
  ```
  - E insira o comando de execução de testes:
  ```
  dotnet test
  ```

## ▶️ Execução
  - Via HTTP.sys:
    - Navegue até o diretório da camada MVC da aplicação:
    ```
    cd ./CidadeInteligente.Mvc/
    ```
    - Insira o comando de execução do projeto:
    ```
    dotnet run --launch-profile https
    ```
    - Acesse [https://localhost:7233](https://localhost:7233)

  - Via Docker Compose:
    - Defina a senha do banco de dados e suba os containers:
    ```
    docker compose up --build
    ```

  - Via Kubernetes local (minikube/kind):
    - Execute o comando para aplicar todos os arquivos yamls presentes no diretório:
    ```
    kubectl apply -f .\k8s\
    ```
    - Em seguida faça o PortForward:
    ```
    kubectl port-forward svc/cidade-inteligente-mvc-service 8080:80
    ```
    - Acesse [http://localhost:8080](http://localhost:8080)
