from PyQt6.QtWidgets import QWidget, QVBoxLayout, QHBoxLayout, QLabel, QPushButton, QTableWidget, QTableWidgetItem, QHeaderView, QDialog, QDialogButtonBox, QCheckBox, QComboBox
from ui.components.card import Card
from ui.components.inputs import LabeledInput
from core.api_client import ApiClient

class UsersAdminPage(QWidget):
    def __init__(self):
        super().__init__()
        self.api = ApiClient()
        root = QVBoxLayout(self)
        header = QHBoxLayout()
        title = QLabel("Usuários"); title.setObjectName("title")
        header.addWidget(title); header.addStretch()
        btn_new = QPushButton("Novo usuário"); btn_new.clicked.connect(self.open_new)
        header.addWidget(btn_new)
        root.addLayout(header)
        self.table = QTableWidget(0, 5)
        self.table.setHorizontalHeaderLabels(["ID","Nome","E-mail","Papel","Status"])
        self.table.horizontalHeader().setSectionResizeMode(QHeaderView.ResizeMode.Stretch)
        root.addWidget(self.table)
        self.refresh()

    def refresh(self):
        ok, data = self.api.list_users()
        if not ok: return
        self.table.setRowCount(0)
        for row in data:
            idx = self.table.rowCount(); self.table.insertRow(idx)
            for c, val in enumerate(row):
                self.table.setItem(idx, c, QTableWidgetItem(val))

    def open_new(self):
        dlg = NewUserDialog(self)
        if dlg.exec() == QDialog.DialogCode.Accepted:
            self.refresh()

class NewUserDialog(QDialog):
    def __init__(self, parent=None):
        super().__init__(parent)
        self.setWindowTitle("Novo usuário")
        layout = QVBoxLayout(self)
        form = Card()
        self.inp_name = LabeledInput("Nome", "Nome completo")
        self.inp_email = LabeledInput("E-mail", "email@exemplo.com")
        self.inp_pass = LabeledInput("Senha provisória", "••••••••", is_password=True)
        self.cmb_role = QComboBox(); self.cmb_role.addItems(["Usuário","Agente","Administrador"])
        self.chk_active = QCheckBox("Ativo"); self.chk_active.setChecked(True)
        form.layout().addWidget(self.inp_name); form.layout().addWidget(self.inp_email)
        form.layout().addWidget(self.inp_pass); form.layout().addWidget(self.cmb_role); form.layout().addWidget(self.chk_active)
        layout.addWidget(form)
        btns = QDialogButtonBox(QDialogButtonBox.StandardButton.Cancel | QDialogButtonBox.StandardButton.Ok)
        btns.accepted.connect(self.accept); btns.rejected.connect(self.reject)
        layout.addWidget(btns)
