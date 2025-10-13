from PyQt6.QtWidgets import QWidget, QVBoxLayout, QHBoxLayout, QLabel, QLineEdit, QPushButton, QTableWidget, QTableWidgetItem, QHeaderView
from core.api_client import ApiClient

class KnowledgeBasePage(QWidget):
    def __init__(self):
        super().__init__()
        self.api = ApiClient()
        root = QVBoxLayout(self)
        header = QHBoxLayout()
        title = QLabel("Base de Conhecimento"); title.setObjectName("title")
        header.addWidget(title); header.addStretch()
        self.search = QLineEdit(); self.search.setPlaceholderText("Buscar artigos...")
        btn = QPushButton("Buscar"); btn.clicked.connect(self.refresh)
        header.addWidget(self.search); header.addWidget(btn)
        root.addLayout(header)

        self.table = QTableWidget(0, 4)
        self.table.setHorizontalHeaderLabels(["ID","Título","Categoria","Atualizado em"])
        self.table.horizontalHeader().setSectionResizeMode(QHeaderView.ResizeMode.Stretch)
        root.addWidget(self.table)
        self.refresh()

    def refresh(self):
        ok, data = self.api.list_kb(self.search.text().strip())
        if not ok: return
        self.table.setRowCount(0)
        for row in data:
            idx = self.table.rowCount(); self.table.insertRow(idx)
            for c, val in enumerate(row):
                self.table.setItem(idx, c, QTableWidgetItem(val))
