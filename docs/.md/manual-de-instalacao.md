## ⚙️ Manual de Instalação e Execução

Siga os passos abaixo para configurar e executar o projeto em um ambiente de desenvolvimento.

### **Pré-requisitos**

Certifique-se de ter as seguintes ferramentas instaladas:
1.  **.NET 8 SDK** (para o Backend)
2.  **SQL Server Express** (para o Banco de Dados)
3.  **Python 3.10+** (para o Frontend)
4.  **Visual Studio 2022** ou **Visual Studio Code**

### **1. Configuração do Backend (API)**

1.  **Clone o repositório:**
    ```bash
    git clone [https://github.com/Felipe-Freitas-Rocha/PIM_4_Semestre]
    ```
2.  **Navegue até a pasta do backend:**
    ```bash
    cd seu-repositorio/backend
    ```
3.  **Crie o Banco de Dados:** Este comando irá ler o código e criar o banco `SmartDeskDB` automaticamente.
    ```bash
    dotnet ef database update
    ```
4.  **Execute a API:**
    ```bash
    dotnet run
    ```
    A API estará rodando em `http://localhost:5201`. Deixe este terminal aberto.

### **2. Configuração do Frontend**

1.  **Navegue até a pasta do frontend:** Em um **novo terminal**, vá para a pasta correspondente.
    ```bash
    cd seu-repositorio/frontend
    ```
2.  **Instale as dependências Python:**
    ```bash
    pip install PyQt6 requests PyJWT
    ```
3.  **Execute a aplicação desktop:**
    ```bash
    python app.py
    ```

### **3. Primeiro Uso e Testes**

1.  **Crie o Usuário Administrador:** Como o banco de dados começa vazio, você precisa criar o primeiro Admin.
    * Com a API rodando, acesse a documentação do Swagger: `http://localhost:5201/swagger`.
    * Vá até `POST /api/usuarios`, clique em "Try it out" e execute com o corpo:
        ```json
        { "nomeCompleto": "Admin Principal", "nivelAcesso": 1 }
        ```
    * **Anote a senha gerada (`senhaGerada`)** que aparecer na resposta!

2.  **Faça o Login:**
    * Execute a aplicação desktop (`python app.py`).
    * Use o `ID: 1` e a senha que você anotou para fazer o login.
    * A partir daí, você pode usar o sistema, criar técnicos, colaboradores e testar o fluxo de chamados.
