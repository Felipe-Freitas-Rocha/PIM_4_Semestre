## 📂 Estrutura do Projeto

O projeto está organizado em duas partes principais: `backend` e `frontend`, seguindo as melhores práticas de desenvolvimento de software.

---

### **Backend (API em C# / ASP.NET Core)**

A API é o cérebro do sistema, responsável por toda a lógica de negócio, segurança e comunicação com o banco de dados.

```
/backend (SmartDesk.API)
│
├── Controllers/
│   ├── AuthController.cs
│   ├── ChamadosController.cs
│   ├── FaqController.cs
│   ├── RelatoriosController.cs
│   └── UsuariosController.cs
│
├── Data/
│   └── ApplicationDbContext.cs
│
├── DTOs/
│   ├── ChamadoDto.cs
│   ├── MensagemDto.cs
│   ├── FaqDto.cs
│   └── UserDtos.cs
│
├── Migrations/
│
├── Models/
│   ├── Chamado.cs
│   ├── FAQ.cs
│   ├── MensagemChamado.cs
│   └── Usuario.cs
│
├── Services/
│   ├── AiService.cs
│   └── PasswordService.cs
│
├── appsettings.json
└── Program.cs
```

**Descrição das pastas e arquivos:**

- **Controllers/**: Define os *endpoints* da API (URLs) que o frontend irá chamar.  
- **Data/**: Contém a configuração de conexão com o banco de dados (`DbContext`).  
- **DTOs/**: “Formulários” que definem os dados enviados e recebidos pela API.  
- **Migrations/**: Scripts gerados pelo Entity Framework para criar e atualizar o banco de dados.  
- **Models/**: Classes C# que representam as tabelas do banco de dados (*Code-First*).  
- **Services/**: Classes que contêm a lógica de negócio complexa (IA, criptografia, etc.).  
- **appsettings.json**: Arquivo de configuração (Connection String, segredos do JWT).  
- **Program.cs**: Ponto de entrada da API, onde todos os serviços são configurados.  

---

### **Frontend (Aplicação Desktop em Python / PyQt6)**

O frontend é a interface com a qual o usuário interage. Ele é responsável por apresentar os dados e enviar as requisições do usuário para o backend.

```
/frontend
│
├── assets/
│
├── core/
│   └── api_client.py
│
├── ui/
│   ├── components/
│   └── screens/
│       ├── auth/
│       ├── dashboard.py
│       ├── profile.py
│       ├── shell.py
│       └── tickets.py
│
├── app.py
└── styles.qss
```

**Descrição das pastas e arquivos:**

- **assets/**: Recursos visuais como ícones, logos e outras imagens.  
- **core/api_client.py**: O “conector” universal, responsável por toda a comunicação com a API.  
- **ui/screens/**: As telas principais da aplicação (login, dashboard, etc.).  
- **app.py**: O ponto de entrada que inicia a aplicação desktop.  
- **styles.qss**: Folha de estilos (similar a CSS) para definir a aparência da UI.
