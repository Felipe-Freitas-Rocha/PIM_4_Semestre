from dataclasses import dataclass

@dataclass
class Session:
    name: str = "Usuário"
    role: str = "user"
    token: str | None = None

SESSION = Session()
