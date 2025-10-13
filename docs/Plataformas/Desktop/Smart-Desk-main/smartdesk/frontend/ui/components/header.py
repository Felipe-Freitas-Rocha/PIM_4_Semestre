from PyQt6.QtWidgets import QWidget, QHBoxLayout, QLabel, QPushButton
from PyQt6.QtCore import pyqtSignal
from core.styles import MUTED

class Header(QWidget):
    logout_requested = pyqtSignal()
    profile_requested = pyqtSignal()

    def __init__(self, user_name: str = "Usuário"):
        super().__init__()
        box = QHBoxLayout(self)
        title = QLabel("HelpDesk")
        title.setObjectName("kpi")
        title.setStyleSheet("color:#fff;")
        box.addWidget(title)
        box.addStretch()
        user = QLabel(user_name)
        user.setStyleSheet(f"color:{MUTED}; padding:6px 10px;")
        btn_profile = QPushButton("Perfil")
        btn_profile.setObjectName("ghost")
        btn_logout = QPushButton("Sair")
        btn_logout.setObjectName("danger")
        btn_profile.clicked.connect(self.profile_requested.emit)
        btn_logout.clicked.connect(self.logout_requested.emit)
        box.addWidget(user)
        box.addWidget(btn_profile)
        box.addWidget(btn_logout)
