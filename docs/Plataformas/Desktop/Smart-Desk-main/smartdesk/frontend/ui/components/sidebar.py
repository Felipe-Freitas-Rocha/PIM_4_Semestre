from PyQt6.QtWidgets import QWidget, QVBoxLayout, QPushButton, QLabel
from PyQt6.QtCore import pyqtSignal
from core.styles import ACCENT

class Sidebar(QWidget):
    navigate = pyqtSignal(str)

    def __init__(self, role: str = "user"):
        super().__init__()
        v = QVBoxLayout(self)
        v.setContentsMargins(0,0,0,0)
        v.setSpacing(8)
        for key, text in [
            ("dashboard", "Visão Geral"),
            ("tickets", "Chamados"),
            ("new_ticket", "Novo Chamado"),
            ("knowledge", "Base de Conhecimento"),
            ("settings", "Configurações"),
        ]:
            btn = QPushButton(text)
            btn.setObjectName("ghost")
            btn.clicked.connect(lambda _, k=key: self.navigate.emit(k))
            v.addWidget(btn)
        v.addStretch()
        if role == "admin":
            admin_lbl = QLabel("Administração")
            admin_lbl.setStyleSheet(f"color:{ACCENT}; padding:2px 8px; font-weight:700;")
            v.addWidget(admin_lbl)
            for key, text in [("admin_users", "Usuários")]:
                b = QPushButton(text)
                b.setObjectName("ghost")
                b.clicked.connect(lambda _, k=key: self.navigate.emit(k))
                v.addWidget(b)
