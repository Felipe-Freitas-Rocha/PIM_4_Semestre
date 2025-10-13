from PyQt6.QtWidgets import QWidget, QVBoxLayout, QLabel, QHBoxLayout, QComboBox, QTextEdit, QPushButton, QFileDialog, QMessageBox
from ui.components.card import Card
from ui.components.inputs import LabeledInput
from core.api_client import ApiClient

class NewTicketPage(QWidget):
    def __init__(self):
        super().__init__()
        self.api = ApiClient()
        root = QVBoxLayout(self)
        header = QLabel("Abrir novo chamado"); header.setObjectName("title")
        root.addWidget(header)

        form = Card()
        self.inp_title = LabeledInput("Título", "Descreva em uma linha")
        self.cmb_category = QComboBox(); self.cmb_category.addItems(["Categoria","Hardware","Software","Rede","Acesso","Outros"])
        self.cmb_priority = QComboBox(); self.cmb_priority.addItems(["Prioridade","Baixa","Média","Alta","Crítica"])
        self.txt_description = QTextEdit(); self.txt_description.setPlaceholderText("Descreva o problema com detalhes")

        attach_box = QHBoxLayout()
        self.attach_label = QLabel("Nenhum anexo")
        btn_attach = QPushButton("Anexar arquivo"); btn_attach.clicked.connect(self.pick_file)
        attach_box.addWidget(self.attach_label); attach_box.addStretch(); attach_box.addWidget(btn_attach)

        grid = QHBoxLayout()
        left = QVBoxLayout(); right = QVBoxLayout()
        left.addWidget(self.inp_title); left.addWidget(self.txt_description)
        right.addWidget(self.cmb_category); right.addWidget(self.cmb_priority); right.addLayout(attach_box)
        form.layout().addLayout(grid)
        grid.addLayout(left, 2); grid.addLayout(right, 1)

        self.btn_submit = QPushButton("Enviar chamado"); self.btn_submit.clicked.connect(self.submit)
        root.addWidget(form); root.addWidget(self.btn_submit); root.addStretch()
        self.attachment_path: str | None = None

    def pick_file(self):
        path, _ = QFileDialog.getOpenFileName(self, "Selecionar arquivo")
        if path:
            self.attachment_path = path
            self.attach_label.setText(path.split("/")[-1])

    def submit(self):
        title = self.inp_title.value()
        if not title or self.cmb_category.currentIndex()==0 or self.cmb_priority.currentIndex()==0 or len(self.txt_description.toPlainText().strip())<8:
            QMessageBox.warning(self, "Campos obrigatórios", "Preencha título, categoria, prioridade e descrição.")
            return
        payload = {
            "titulo": title,
            "categoria": self.cmb_category.currentText(),
            "prioridade": self.cmb_priority.currentText(),
            "descricao": self.txt_description.toPlainText().strip(),
            "anexo": self.attachment_path,
        }
        ok, data = self.api.create_ticket(payload)
        if ok:
            QMessageBox.information(self, "Chamado criado", f"Chamado criado com ID {data['id']}.")
            self.inp_title.set_value(""); self.cmb_category.setCurrentIndex(0); self.cmb_priority.setCurrentIndex(0)
            self.txt_description.clear(); self.attach_label.setText("Nenhum anexo"); self.attachment_path = None
        else:
            QMessageBox.critical(self, "Erro", "Não foi possível criar o chamado.")
