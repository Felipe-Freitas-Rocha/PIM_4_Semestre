from PyQt6.QtWidgets import QWidget, QVBoxLayout, QHBoxLayout, QLabel, QPushButton, QComboBox, QTableWidget, QTableWidgetItem, QHeaderView, QLineEdit
from ui.components.card import Card
from core.api_client import ApiClient
from core.styles import MUTED
from PyQt6.QtCore import pyqtSignal

class TicketDetailPage(QWidget):
    back = pyqtSignal()
    def __init__(self):
        super().__init__()
        self.api = ApiClient()
        root = QVBoxLayout(self)
        header = QHBoxLayout()
        self.title = QLabel("Detalhes do Chamado"); self.title.setObjectName("title")
        header.addWidget(self.title); header.addStretch()
        btn_back = QPushButton("Voltar"); btn_back.setObjectName("ghost"); btn_back.clicked.connect(self.back.emit)
        header.addWidget(btn_back)
        root.addLayout(header)

        content = QHBoxLayout()
        left = QVBoxLayout(); right = QVBoxLayout()

        meta = Card()
        self.lbl_id = QLabel("#0000")
        self.lbl_status = QLabel("Aberto")
        self.lbl_priority = QLabel("Alta")
        self.lbl_requester = QLabel("-")
        self.lbl_category = QLabel("-")
        for n, w in [("ID", self.lbl_id), ("Status", self.lbl_status), ("Prioridade", self.lbl_priority), ("Solicitante", self.lbl_requester), ("Categoria", self.lbl_category)]:
            row = QHBoxLayout(); a = QLabel(n); a.setStyleSheet(f"color:{MUTED}")
            row.addWidget(a); row.addStretch(); row.addWidget(w); meta.layout().addLayout(row)
        left.addWidget(meta)

        actions = Card()
        self.cb_status = QComboBox(); self.cb_status.addItems(["Aberto","Em andamento","Pendente","Resolvido"])
        self.cb_priority = QComboBox(); self.cb_priority.addItems(["Baixa","Média","Alta","Crítica"])
        btn_save = QPushButton("Salvar alterações")
        actions.layout().addWidget(QLabel("Atualizar"))
        actions.layout().addWidget(self.cb_status)
        actions.layout().addWidget(self.cb_priority)
        actions.layout().addWidget(btn_save)
        left.addWidget(actions)

        inter = Card()
        inter.layout().addWidget(QLabel("Interações"))
        self.inter_table = QTableWidget(0, 3)
        self.inter_table.setHorizontalHeaderLabels(["Autor","Data","Mensagem"])
        self.inter_table.horizontalHeader().setSectionResizeMode(0, QHeaderView.ResizeMode.ResizeToContents)
        self.inter_table.horizontalHeader().setSectionResizeMode(1, QHeaderView.ResizeMode.ResizeToContents)
        self.inter_table.horizontalHeader().setSectionResizeMode(2, QHeaderView.ResizeMode.Stretch)
        inter.layout().addWidget(self.inter_table)
        add_box = QHBoxLayout()
        self.txt_inter = QLineEdit(); self.txt_inter.setPlaceholderText("Adicionar nova interação...")
        btn_add = QPushButton("Enviar")
        add_box.addWidget(self.txt_inter); add_box.addWidget(btn_add)
        inter.layout().addLayout(add_box)
        right.addWidget(inter)

        content.addLayout(left, 1); content.addLayout(right, 2)
        root.addLayout(content)

    def load_ticket(self, ticket_id: str):
        ok, data = self.api.get_ticket(ticket_id)
        if not ok: return
        self.title.setText(f"Chamado {data['id']}")
        self.lbl_id.setText(data["id"])
        self.lbl_status.setText(data["status"])
        self.lbl_priority.setText(data["prioridade"])
        self.lbl_requester.setText(data["solicitante"])
        self.lbl_category.setText(data["categoria"])
        self.cb_status.setCurrentText(data["status"])
        self.cb_priority.setCurrentText(data["prioridade"])
        self.inter_table.setRowCount(0)
        for it in data["interacoes"]:
            idx = self.inter_table.rowCount(); self.inter_table.insertRow(idx)
            self.inter_table.setItem(idx, 0, QTableWidgetItem(it["autor"]))
            self.inter_table.setItem(idx, 1, QTableWidgetItem(it["data"]))
            self.inter_table.setItem(idx, 2, QTableWidgetItem(it["mensagem"]))
