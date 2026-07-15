# 🏙️ CidadeInteligente
> Sistema gerenciador de projetos realizados por professores e alunos da Cidade Inteligente da Fatec de Lins - Professor Antônio Seabra, permitindo o cadastro, a divulgação e a administração completa dos projetos e de suas mídias.

## 📸 Demonstração

### Tela de Login
  <img width="1917" height="951" alt="Tela de Login" src="https://github.com/user-attachments/assets/67845940-3a09-446e-9ea5-30bedff67753" />

### Tela Principal + Visualizando Projeto
  https://github.com/user-attachments/assets/6fa99573-e29e-4132-8550-5280856cba47

### Criação, edição e exclusão do Projeto
  https://github.com/user-attachments/assets/89f0874d-ae16-40b2-a8f9-a6dee7a67342

### Tela de Admin
  https://github.com/user-attachments/assets/bedb8112-e5ad-4482-bd80-f180f83568d2

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
### 🔐 Variáveis de ambiente (obrigatório para todos os meios de execução)
  - Copie o arquivo `.env.example` para `.env` na raiz do repositório:
  ```
  copy .env.example .env
  ```
  - Preencha as variáveis no arquivo `.env`:
    - `DATABASE_PASSWORD`: senha do SQL Server (mínimo de 8 caracteres, com letras maiúsculas, minúsculas e números);
    - `CONNECTIONSTRINGS__FILESTORAGE`: connection string do Azure Blob Storage (pode deixar vazio, mas não haverá imagens);
    - `SENDGRID_APIKEY`: API Key do SendGrid para envio de e-mails (pode deixar vazio, mas não enviará e-mails);

### 💻 Via HTTP.sys
  - Configure os user-secrets do projeto MVC a partir do `.env` com o script:
  ```
  .\scripts\set-user-secrets.bat
  ```
  - É necessário que tenha o SQL Server em sua máquina, caso não possua, pule para a etapa <a href="#-via-docker-compose">🐳 Via Docker Compose</a>. Caso possua, navegue até o diretório da camada MVC da aplicação:
  ```
  cd ./CidadeInteligente.Mvc/
  ```
  - Insira o comando de execução do projeto:
  ```
  dotnet run --launch-profile https
  ```
  - Acesse [https://localhost:7233](https://localhost:7233)

### 🐳 Via Docker Compose
  - Suba os containers (o `.env` é carregado automaticamente):
  ```
  docker compose up --build
  ```

### ☸️ Via Kubernetes local (minikube/kind)
  - Navegue até o diretório de scripts:
  ```
  cd ./scripts/
  ```
  - Crie o secret do cluster a partir das variáveis do `.env`:
  ```
  create-k8s-secret.bat
  ```
  - Aplique os manifests e reinicie o deployment:
  ```
  deploy-k8s.bat
  ```
  - Em seguida faça o PortForward:
  ```
  kubectl port-forward svc/cidade-inteligente-mvc-service 8080:80
  ```
  - Acesse [http://localhost:8080](http://localhost:8080)
