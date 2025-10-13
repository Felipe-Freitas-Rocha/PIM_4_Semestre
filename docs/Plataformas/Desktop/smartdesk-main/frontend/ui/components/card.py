from PyQt6.QtWidgets import QFrame, QVBoxLayout

class Card(QFrame):
    def __init__(self):
        super().__init__()
        self.setObjectName("card")
        self.setLayout(QVBoxLayout())
        self.layout().setContentsMargins(16,16,16,16)
        self.layout().setSpacing(8)
