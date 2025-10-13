from PyQt6.QtWidgets import QWidget, QVBoxLayout, QHBoxLayout, QLabel, QTableWidget, QTableWidgetItem, QHeaderView
from core.styles import MUTED
from ui.components.card import Card

class DashboardPage(QWidget):
    def __init__(self):
        super().__init__()
        grid = QVBoxLayout(self)
        row = QHBoxLayout()
        for label, value in [("Abertos", "7"), ("Em andamento", "12"), ("Pendentes", "3"), ("Resolvidos", "120")]:
            card = Card()
            t = QLabel(label)
            t.setStyleSheet(f"color:{MUTED}")
            v = QLabel(value); v.setObjectName("kpi")
            card.layout().addWidget(t); card.layout().addWidget(v)
            row.addWidget(card)
        grid.addLayout(row)

        feed = Card()
        title = QLabel("Atualizações recentes"); title.setObjectName("title")
        table = QTableWidget(0, 4)
        table.setHorizontalHeaderLabels(["ID", "Título", "Status", "Quando"])
        table.horizontalHeader().setSectionResizeMode(QHeaderView.ResizeMode.Stretch)
        for r in [["#1003","Acesso Wi-Fi","Resolvido","há 2h"],["#1002","Erro no login","Em andamento","há 4h"],["#1001","Impressora","Aberto","há 6h"]]:
            idx = table.rowCount(); table.insertRow(idx)
            for c, val in enumerate(r):
                table.setItem(idx, c, QTableWidgetItem(val))
        feed.layout().addWidget(title)
        feed.layout().addWidget(table)
        grid.addWidget(feed)
