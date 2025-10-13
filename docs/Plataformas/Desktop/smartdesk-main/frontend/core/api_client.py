from typing import Any, Dict, Tuple, List

class ApiClient:
    def __init__(self):
        self.token: str | None = None

    def set_token(self, token: str):
        self.token = token

    # ---- Auth ----
    def login(self, email: str, password: str) -> Tuple[bool, Dict[str, Any]]:
    # Usuário administrador
        if email == "admin@smartdesk.com" and password == "admin123":
            return True, {"name": "Administrador", "role": "admin", "token": "demo-token-admin"}

    # Usuário comum
        if email == "usuario@smartdesk.com" and password == "user123":
            return True, {"name": "Usuário Padrão", "role": "user", "token": "demo-token-user"}

    # Caso não exista
            return False, {"message": "Credenciais inválidas"}


    # ---- Tickets ----
    def list_tickets(self, filters: dict) -> Tuple[bool, List[list]]:
        data = [
            ["#1001", "Impressora não funciona", "Aberto", "Alta", "Gabriel", "10/10/2025"],
            ["#1002", "Erro no login", "Em andamento", "Média", "Ana", "10/10/2025"],
            ["#1003", "Acesso Wi-Fi", "Resolvido", "Baixa", "Pedro", "09/10/2025"],
        ]
        return True, data

    def get_ticket(self, ticket_id: str) -> Tuple[bool, Dict[str, Any]]:
        return True, {
            "id": ticket_id,
            "titulo": "Impressora não funciona",
            "status": "Aberto",
            "prioridade": "Alta",
            "solicitante": "Gabriel",
            "categoria": "Hardware",
            "criado_em": "10/10/2025",
            "descricao": "A impressora do laboratório 5C está com atolamento de papel.",
            "interacoes": [
                {"autor": "Gabriel", "data": "10/10/2025 09:15", "mensagem": "Relatei o problema."},
                {"autor": "Agente Júlia", "data": "10/10/2025 09:40", "mensagem": "Solicitado reinício do equipamento."},
            ],
        }

    def create_ticket(self, payload: dict) -> Tuple[bool, Dict[str, Any]]:
        return True, {"id": "#1004"}

    # ---- Users ----
    def list_users(self) -> Tuple[bool, list[list]]:
        return True, [
            ["1", "Gabriel", "gabriel@email.com", "Usuário", "Ativo"],
            ["2", "Júlia", "julia@email.com", "Agente", "Ativo"],
            ["3", "Admin", "admin@admin.com", "Administrador", "Ativo"]
        ]

    # ---- Knowledge Base ----
    def list_kb(self, query: str | None = None) -> Tuple[bool, list[list]]:
        data = [
            ["KB-001", "Conectar à rede Wi-Fi", "Rede", "Atualizado em 08/10/2025"],
            ["KB-002", "Troca de toner HP M402dne", "Hardware", "Atualizado em 05/10/2025"],
            ["KB-003", "Política de backup", "Governança", "Atualizado em 01/10/2025"],
        ]
        if query:
            data = [r for r in data if query.lower() in r[1].lower()]
        return True, data
