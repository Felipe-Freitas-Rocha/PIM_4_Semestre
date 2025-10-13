from PyQt6.QtWidgets import QWidget, QVBoxLayout, QLabel, QPushButton
from ui.components.card import Card
from ui.components.inputs import LabeledInput

class SettingsPage(QWidget):
    def __init__(self, user_name: str):
        super().__init__()
        root = QVBoxLayout(self)
        title = QLabel("Configurações"); title.setObjectName("title")
        root.addWidget(title)

        form = Card()
        self.inp_name = LabeledInput("Nome", user_name)
        self.inp_email = LabeledInput("E-mail", "")
        self.inp_password = LabeledInput("Trocar senha", is_password=True)
        btn_save = QPushButton("Salvar")
        form.layout().addWidget(self.inp_name)
        form.layout().addWidget(self.inp_email)
        form.layout().addWidget(self.inp_password)
        form.layout().addWidget(btn_save)
        root.addWidget(form); root.addStretch()
