from PyQt6.QtWidgets import QWidget, QVBoxLayout, QPushButton, QMessageBox
from PyQt6.QtCore import pyqtSignal
from ui.components.inputs import LabeledInput
from core.api_client import ApiClient

class LoginScreen(QWidget):
    authenticated = pyqtSignal(dict)

    def __init__(self):
        super().__init__()
        self.api = ApiClient()
        self.setWindowTitle("HelpDesk • Autenticação")
        v = QVBoxLayout(self)
        self.email = LabeledInput("E-mail", "email@exemplo.com")
        self.password = LabeledInput("Senha", is_password=True)
        btn = QPushButton("Entrar"); btn.clicked.connect(self._login)
        v.addWidget(self.email); v.addWidget(self.password); v.addWidget(btn)

    def _login(self):
        ok, data = self.api.login(self.email.value(), self.password.value())
        if ok:
            self.api.set_token(data["token"])
            self.authenticated.emit(data)
        else:
            QMessageBox.critical(self, "Falha no login", data.get("message", "Erro"))
