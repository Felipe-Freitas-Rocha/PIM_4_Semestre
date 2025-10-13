from dataclasses import dataclass
from typing import List

@dataclass
class Interaction:
    autor: str
    data: str
    mensagem: str

@dataclass
class Ticket:
    id: str
    titulo: str
    status: str
    prioridade: str
    solicitante: str
    categoria: str
    criado_em: str
    descricao: str
    interacoes: List[Interaction]
