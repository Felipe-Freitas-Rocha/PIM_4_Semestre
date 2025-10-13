from PyQt6.QtWidgets import QWidget, QVBoxLayout, QHBoxLayout, QLineEdit, QComboBox, QPushButton, QTableWidget, QAbstractItemView, QTableWidgetItem, QHeaderView
from PyQt6.QtCore import pyqtSignal
from core.api_client import ApiClient

class TicketsPage(QWidget):
    open_detail = pyqtSignal(str)
    def __init__(self):
        super().__init__()
        self.api = ApiClient()
        root = QVBoxLayout(self)
        filters = QHBoxLayout()
        self.search = QLineEdit(); self.search.setPlaceholderText("Buscar por título ou ID...")
        self.status = QComboBox(); self.status.addItems(["Todos","Aberto","Em andamento","Pendente","Resolvido"])
        self.priority = QComboBox(); self.priority.addItems(["Todas","Baixa","Média","Alta","Crítica"])
        btn_apply = QPushButton("Aplicar"); btn_apply.clicked.connect(self.apply_filters)
        filters.addWidget(self.search); filters.addWidget(self.status); filters.addWidget(self.priority); filters.addWidget(btn_apply)
        root.addLayout(filters)
        self.table = QTableWidget(0, 6)
        self.table.setHorizontalHeaderLabels(["ID","Título","Status","Prioridade","Solicitante","Criado em"])
        self.table.horizontalHeader().setSectionResizeMode(QHeaderView.ResizeMode.Stretch)
        self.table.setEditTriggers(QAbstractItemView.EditTrigger.NoEditTriggers)
        self.table.setSelectionBehavior(QAbstractItemView.SelectionBehavior.SelectRows)
        self.table.doubleClicked.connect(self._on_double)
        root.addWidget(self.table)
        self.apply_filters()

    def apply_filters(self):
        ok, data = self.api.list_tickets({})
        if not ok: return
        self.table.setRowCount(0)
        for row in data:
            idx = self.table.rowCount(); self.table.insertRow(idx)
            for c, val in enumerate(row):
                self.table.setItem(idx, c, QTableWidgetItem(val))

    def _on_double(self):
        idx = self.table.currentRow()
        if idx < 0: return
        ticket_id = self.table.item(idx, 0).text()
        self.open_detail.emit(ticket_id)
