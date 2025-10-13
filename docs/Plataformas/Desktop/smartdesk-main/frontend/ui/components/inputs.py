from PyQt6.QtWidgets import QWidget, QVBoxLayout, QLabel, QLineEdit
from core.styles import MUTED

class LabeledInput(QWidget):
    def __init__(self, label_text: str, placeholder: str = "", is_password: bool = False):
        super().__init__()
        root = QVBoxLayout(self)
        root.setSpacing(6)
        lbl = QLabel(label_text)
        lbl.setStyleSheet(f"color:{MUTED}; font-size:12px;")
        inp = QLineEdit()
        inp.setPlaceholderText(placeholder)
        if is_password:
            inp.setEchoMode(QLineEdit.EchoMode.Password)
        root.addWidget(lbl)
        root.addWidget(inp)
        self.input = inp

    def value(self) -> str:
        return self.input.text().strip()

    def set_value(self, text: str):
        self.input.setText(text)
