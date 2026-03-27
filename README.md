# Opinai

> Plataforma de questionários online em larga escala, construída como objeto de estudo para a pós-graduação MIT em Arquitetura de Software, disciplina de Arquitetura .NET.

---

## Visão Geral

O **Opinai** é uma solução de questionários estruturada como **três microserviços independentes** que se comunicam de forma assíncrona via mensageria. Cada serviço possui seu próprio domínio, banco de dados e ciclo de vida, respeitando os princípios de **Clean Architecture** e **Domain-Driven Design (DDD)**.

```
┌─────────────────────────┐     SurveyPublished      ┌──────────────────────────────┐
│   SurveyManagement      │ ───────────────────────► │    ResponseManagement        │
│   (Gerencia pesquisas)  │     SurveyFinished       │    (Coleta respostas)        │
└─────────────────────────┘ ───────────────────────► └───────────────┬──────────────┘
                                                                     │ SurveyResultsAggregated
                                                                     ▼
                                                      ┌──────────────────────────────┐
                                                      │      SurveyAnalytics         │
                                                      │    (Analisa resultados)      │
                                                      └──────────────────────────────┘
```

---

## Stack Tecnológica

| Tecnologia | Uso |
|---|---|
| **ASP.NET Core 9** | APIs REST dos microserviços |
| **Entity Framework Core 9** | Persistência com banco in-memory |
| **MassTransit 8** | Mensageria in-memory entre serviços |
| **AutoMapper 16** | Mapeamento entre camadas no SurveyManagement |
| **Swagger / Swashbuckle** | Documentação interativa das APIs |

---

## Estrutura da Solução

```
src/
├── shared/ # Abstrações e implementações reutilizáveis
│   ├── Opinai.Shared.Application/
│   ├── Opinai.Shared.Infrastructure/
│   └── Opinai.Shared.Api/
│
├── messaging/ # Contratos de integração centralizados
│   └── Opinai.Messaging.Contracts/
|
├── survey-management/ # Microserviço de gerenciamento de pesquisas
│   ├── Opinai.SurveyManagement.Api/
│   ├── Opinai.SurveyManagement.Application/
│   ├── Opinai.SurveyManagement.Domain/
│   └── Opinai.SurveyManagement.Infrastructure/
|
├── response-management/ # Microserviço de coleta de respostas
│   ├── Opinai.ResponseManagement.Api/
│   ├── Opinai.ResponseManagement.Application/
│   ├── Opinai.ResponseManagement.Domain/
│   └── Opinai.ResponseManagement.Infrastructure/
|
└── survey-analytics/ # Microserviço de análise de resultados
    ├── Opinai.SurveyAnalytics.Api/
    ├── Opinai.SurveyAnalytics.Application/
    ├── Opinai.SurveyAnalytics.Domain/
    └── Opinai.SurveyAnalytics.Infrastructure/
docs/
└── postman/                   # Coleção Postman para testes manuais
```

---

## Arquitetura por Camadas

Cada microserviço segue a mesma divisão em quatro camadas, respeitando a regra de dependência da Clean Architecture (as camadas internas nunca conhecem as externas):

```
  ┌──────────────┐
  │     Api      │  ← Adaptador de entrada (Controllers, DI, Middleware)
  ├──────────────┤
  │ Application  │  ← Casos de uso, interfaces, DTOs, serviços de aplicação
  ├──────────────┤
  │   Domain     │  ← Entidades, Value Objects, regras de negócio puras
  ├──────────────┤
  │Infrastructure│  ← Persistência (EF Core), consumers de mensageria
  └──────────────┘
```

### Api
Ponto de entrada HTTP. Contém os `Controllers`, o `Program.cs` com registro de dependências, configuração do MassTransit e exposição do Swagger.

### Application
Orquestra os casos de uso. Define as **interfaces** que a camada de Infrastructure deve implementar, contém os **DTOs**, **enums de resultado** e os **serviços de aplicação** que coordenam domínio e repositórios.

### Domain
O núcleo da solução. Contém as **entidades** com regras de negócio encapsuladas, **Value Objects** imutáveis e **serviços de domínio** com lógica de cálculo pura — sem dependência de frameworks.

### Infrastructure
Implementa as interfaces da camada Application. Contém:
- **Persistência**: `DbContext` e repositórios concretos usando Entity Framework Core
- **Mensageria**: Consumers do MassTransit que recebem eventos e acionam serviços de aplicação

---

## Microserviço: SurveyManagement

> **Responsabilidade:** gerenciar o ciclo de vida das pesquisas.

### Entidade principal: `Survey`

Uma pesquisa possui um ciclo de vida de três estados controlados por lógica de domínio:

```
Draft ──── PublishSurvey() ───► Published ──── FinishSurvey() ───► Finished
```

| Estado | Operações permitidas |
|---|---|
| `Draft` | Editar metadados, substituir perguntas, publicar |
| `Published` | Concluir (coleta de respostas ativa) |
| `Finished` | Nenhuma (somente leitura) |

Uma pesquisa é composta por **Questions** (Value Objects), cada uma com uma lista de **Answers** (Value Objects) indexadas automaticamente.

### Eventos publicados

| Evento | Publicado quando |
|---|---|
| `SurveyPublished` | A pesquisa é publicada (`Draft → Published`) |
| `SurveyFinished` | A pesquisa é concluída (`Published → Finished`) |

### Endpoints principais

| Método | Rota | Descrição |
|---|---|---|
| `POST` | `/api/survey` | Cria uma nova pesquisa |
| `PUT` | `/api/survey/{id}` | Atualiza título, descrição e perguntas |
| `DELETE` | `/api/survey/{id}` | Remove a pesquisa |
| `GET` | `/api/survey/{id}` | Retorna detalhes da pesquisa |
| `POST` | `/api/survey/{id}/publish` | Publica a pesquisa |
| `POST` | `/api/survey/{id}/finish` | Conclui a pesquisa |

---

## Microserviço: ResponseManagement

> **Responsabilidade:** habilitar a coleta de respostas e agregar os dados para análise.

### Consumers

| Consumer | Evento recebido | Ação |
|---|---|---|
| `SurveyPublishedConsumer` | `SurveyPublished` | Marca a pesquisa como disponível para resposta (`OpenSurvey`) |
| `SurveyFinishedConsumer` | `SurveyFinished` | Agrega todas as respostas e publica `SurveyResultsAggregated` |

### Entidade: `SurveyResponse`

Armazena cada resposta individual com `SurveyId`, `QuestionIndex` e `AnswerIndex`. O repositório oferece uma query de agregação que consolida os dados por alternativa antes de encaminhar ao Analytics.

### Evento publicado

| Evento | Publicado quando |
|---|---|
| `SurveyResultsAggregated` | A pesquisa é concluída; contém contagem de respostas por alternativa |

### Endpoints principais

| Método | Rota | Descrição |
|---|---|---|
| `POST` | `/api/surveyresponse` | Registra as respostas de um participante |

---

## Microserviço: SurveyAnalytics

> **Responsabilidade:** calcular e expor estatísticas das pesquisas concluídas.

### Consumer

| Consumer | Evento recebido | Ação |
|---|---|---|
| `SurveyResultsConsumer` | `SurveyResultsAggregated` | Mapeia o payload e aciona `SurveyAnalyticsService.BuildAsync()` |

### Serviço de domínio: `SurveyAnalyticsCalculator`

Lógica de cálculo pura, sem dependências de framework:
- Recebe as contagens de respostas por alternativa
- Calcula o **total de respostas** por pergunta
- Calcula o **percentual** de cada alternativa com duas casas decimais
- Retorna um `SurveyAnalyticsResult` com todos os dados calculados

### Endpoints principais

| Método | Rota | Descrição |
|---|---|---|
| `POST` | `/api/surveyanalytics` | Recebe o payload e retorna a análise calculada |

---

## Contratos de Integração (`Messaging.Contracts`)

Projeto centralizado que define todos os objetos de comunicação entre serviços, servindo como **única fonte da verdade** para a interface de mensageria.

```
Opinai.Messaging.Contracts/
├── Events/
│   ├── SurveyPublished.cs          → record { SurveyId }
│   ├── SurveyFinished.cs           → record { SurveyId }
│   └── SurveyResultsAggregated.cs  → record { SurveyId, Questions[] }
└── Payloads/
    ├── SurveyResultsPayload.cs     → dados brutos de respostas agregadas
    ├── QuestionResultsPayload.cs   → contagens por pergunta
    └── AnswerResultsPayload.cs     → índice e contagem por alternativa
```

---

## Camada Shared

Implementações genéricas reutilizadas por todos os microserviços:

| Projeto | Conteúdo |
|---|---|
| `Shared.Application` | Interface `ICrudRepository<T>`, `IUnitOfWork`, `IEntity`; base `QueryServiceBase<T, TDto>` |
| `Shared.Infrastructure` | `CrudRepositoryBase<T>` (EF Core), `UnitOfWork<TContext>` |
| `Shared.Api` | Base controllers e utilitários HTTP compartilhados |

---

## Fluxo Completo

```
1. [Usuário] POST /api/survey          → Cria pesquisa em Draft
2. [Usuário] POST /api/survey/{id}/publish
              └─► SurveyManagement publica evento SurveyPublished
              └─► ResponseManagement (SurveyPublishedConsumer) abre a pesquisa para respostas

3. [Participantes] POST /api/surveyresponse  → Registram respostas

4. [Usuário] POST /api/survey/{id}/finish
              └─► SurveyManagement publica evento SurveyFinished
              └─► ResponseManagement (SurveyFinishedConsumer) agrega respostas
                    └─► Publica evento SurveyResultsAggregated
                          └─► SurveyAnalytics (SurveyResultsConsumer) calcula estatísticas
                                └─► Retorna percentuais por alternativa via POST /api/surveyanalytics
```

---

## Como Executar

Clone o repositório e execute cada microserviço em um terminal separado:

```bash
# SurveyManagement (porta padrão: 5xxx)
dotnet run --project src/survey-management/Opinai.SurveyManagement.Api

# ResponseManagement
dotnet run --project src/response-management/Opinai.ResponseManagement.Api

# SurveyAnalytics
dotnet run --project src/survey-analytics/Opinai.SurveyAnalytics.Api
```

Cada serviço expõe o **Swagger UI** em `/swagger` para testes interativos. A coleção Postman com o fluxo completo está disponível em `docs/postman/`.

```bash
# Compilar a solução completa
dotnet build Opinai.sln
```
