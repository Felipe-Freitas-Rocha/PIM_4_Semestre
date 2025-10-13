from PyQt6.QtWidgets import QMainWindow, QWidget, QVBoxLayout, QHBoxLayout, QStackedWidget
from ui.components.header import Header
from ui.components.sidebar import Sidebar
from ui.screens.dashboard import DashboardPage
from ui.screens.tickets import TicketsPage
from ui.screens.ticket_detail import TicketDetailPage
from ui.screens.new_ticket import NewTicketPage
from ui.screens.knowledge import KnowledgeBasePage
from ui.screens.settings import SettingsPage
from ui.screens.admin_users import UsersAdminPage
from core.styles import STYLE

class Shell(QMainWindow):
    def __init__(self, profile: dict):
        super().__init__()
        self.setWindowTitle("SmartDesk • Frontend")
        self.resize(1200, 740)
        self.setStyleSheet(STYLE)
        central = QWidget(); self.setCentralWidget(central)
        layout = QVBoxLayout(central)

        self.header = Header(profile.get("name","Usuário"))
        self.header.profile_requested.connect(lambda: self._navigate("settings"))
        self.header.logout_requested.connect(self.close)
        layout.addWidget(self.header)

        body = QHBoxLayout()
        self.sidebar = Sidebar(profile.get("role","user"))
        self.sidebar.setFixedWidth(250)
        self.sidebar.navigate.connect(self._navigate)
        body.addWidget(self.sidebar)

        self.stack = QStackedWidget()
        self.page_dashboard = DashboardPage()
        self.page_tickets = TicketsPage()
        self.page_detail = TicketDetailPage()
        self.page_new = NewTicketPage()
        self.page_kb = KnowledgeBasePage()
        self.page_settings = SettingsPage(profile.get("name","Usuário"))
        self.page_users = UsersAdminPage()

        self.stack.addWidget(self.page_dashboard)
        self.stack.addWidget(self.page_tickets)
        self.stack.addWidget(self.page_detail)
        self.stack.addWidget(self.page_new)
        self.stack.addWidget(self.page_kb)
        self.stack.addWidget(self.page_settings)
        self.stack.addWidget(self.page_users)

        body.addWidget(self.stack, 1)
        layout.addLayout(body)

        self.page_tickets.open_detail.connect(self._open_detail)

        self._route_index = {
            "dashboard": self.stack.indexOf(self.page_dashboard),
            "tickets": self.stack.indexOf(self.page_tickets),
            "ticket_detail": self.stack.indexOf(self.page_detail),
            "new_ticket": self.stack.indexOf(self.page_new),
            "knowledge": self.stack.indexOf(self.page_kb),
            "settings": self.stack.indexOf(self.page_settings),
            "admin_users": self.stack.indexOf(self.page_users),
        }
        self._navigate("dashboard")

    def _navigate(self, key: str):
        if key in self._route_index:
            self.stack.setCurrentIndex(self._route_index[key])

    def _open_detail(self, ticket_id: str):
        self.page_detail.load_ticket(ticket_id)
        self._navigate("ticket_detail")
