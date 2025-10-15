from dataclasses import dataclass

@dataclass
class Article:
    id: str
    title: str
    category: str
    updated_at: str
