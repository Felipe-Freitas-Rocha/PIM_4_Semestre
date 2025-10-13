from PyQt6.QtWidgets import QApplication
from ui.screens.auth.login import LoginScreen
from ui.screens.shell import Shell
from core.styles import STYLE
import sys

def run():
    app = QApplication(sys.argv)
    app.setStyleSheet(STYLE)
    login = LoginScreen()

    def on_auth(profile: dict):
        shell = Shell(profile)
        shell.show()
        login.close()

    login.authenticated.connect(on_auth)
    login.show()
    sys.exit(app.exec())
