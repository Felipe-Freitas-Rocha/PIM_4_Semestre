PRIMARY = "#0ea5e9"
PRIMARY_ALT = "#22d3ee"
BG = "#0f172a"
BG_ALT = "#111827"
TEXT = "#e5e7eb"
MUTED = "#94a3b8"
ACCENT = "#f59e0b"
DANGER = "#ef4444"
SUCCESS = "#22c55e"
BORDER = "#1f2937"

STYLE = f"""
* {{ color: {TEXT}; font-family: 'Segoe UI', Arial, Helvetica, sans-serif; }}
QMainWindow, QWidget {{ background: {BG}; }}
QLineEdit, QComboBox, QTextEdit, QDateEdit {{ background: {BG_ALT}; border: 1px solid {BORDER}; border-radius: 10px; padding: 10px; }}
QTableWidget {{ background: {BG_ALT}; border: 1px solid {BORDER}; gridline-color: {BORDER}; border-radius: 12px; }}
QHeaderView::section {{ background: {BG_ALT}; color: {TEXT}; border: none; padding: 10px; font-weight: 600; }}
QPushButton {{ background: {PRIMARY}; border: none; border-radius: 10px; padding: 10px 14px; font-weight: 600; }}
QPushButton:hover {{ background: {PRIMARY_ALT}; }}
QPushButton#danger {{ background: {DANGER}; }}
QPushButton#success {{ background: {SUCCESS}; }}
QPushButton#ghost {{ background: transparent; border: 1px solid {BORDER}; }}
QFrame#card {{ background: {BG_ALT}; border: 1px solid {BORDER}; border-radius: 16px; }}
QLabel#title {{ font-size: 20px; font-weight: 700; }}
QLabel#kpi {{ font-size: 28px; font-weight: 800; }}
QScrollArea {{ border: none; }}
"""
