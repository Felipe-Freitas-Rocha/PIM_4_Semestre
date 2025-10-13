from PyQt6.QtWidgets import QFileDialog

def pick_file(parent, title: str = "Selecionar arquivo"):
    path, _ = QFileDialog.getOpenFileName(parent, title)
    return path or None
