from PyQt6.QtWidgets import (
    QWidget, QVBoxLayout, QPushButton, QMessageBox, QLabel, QSpacerItem, QSizePolicy
)
from PyQt6.QtCore import Qt, pyqtSignal
from PyQt6.QtGui import QPixmap
from ui.components.inputs import LabeledInput
from core.api_client import ApiClient


class LoginScreen(QWidget):
    authenticated = pyqtSignal(dict)

    def __init__(self):
        super().__init__()
        self.api = ApiClient()
        self.setWindowTitle("SmartDesk • Autenticação")

        # Layout principal vertical
        v = QVBoxLayout(self)
        v.setAlignment(Qt.AlignmentFlag.AlignCenter)
        v.setContentsMargins(60, 40, 60, 40)
        v.setSpacing(20)

        # === Logo da empresa ===
        logo_label = QLabel()
        logo_pixmap = QPixmap("logo.png")  # Caminho da imagem
        logo_pixmap = logo_pixmap.scaledToWidth(180, Qt.TransformationMode.SmoothTransformation)
        logo_label.setPixmap(logo_pixmap)
        logo_label.setAlignment(Qt.AlignmentFlag.AlignCenter)

        # === Campos de autenticação ===
        self.email = LabeledInput("ID", "Digite o ID")
        self.password = LabeledInput("Senha", "Digite a senha", is_password=True)

        # === Botão de entrar ===
        btn = QPushButton("Entrar")
        btn.clicked.connect(self._login)
        btn.setFixedHeight(40)
        btn.setStyleSheet("""
            QPushButton {
                background-color: #0099ff;
                color: white;
                font-weight: bold;
                border-radius: 8px;
                font-size: 14px;
            }
            QPushButton:hover {
                background-color: #00bfff;
            }
        """)

        # === Organização visual ===
        v.addSpacerItem(QSpacerItem(20, 20, QSizePolicy.Policy.Minimum, QSizePolicy.Policy.Expanding))
        v.addWidget(logo_label)
        v.addWidget(self.email)
        v.addWidget(self.password)
        v.addWidget(btn)
        v.addSpacerItem(QSpacerItem(20, 20, QSizePolicy.Policy.Minimum, QSizePolicy.Policy.Expanding))

        # === Estilo de fundo escuro ===
        self.setStyleSheet("""
            QWidget {
                background-color: #0a1220;
                color: #cfd6e0;
                font-family: 'Segoe UI';
            }
        """)

    def _login(self):
        ok, data = self.api.login(self.email.value(), self.password.value())
        if ok:
            self.api.set_token(data["token"])
            self.authenticated.emit(data)
        else:
            QMessageBox.critical(self, "Falha no login", data.get("message", "Erro"))
