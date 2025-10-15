from core.api_client import ApiClient

class TicketService:
    def __init__(self, api: ApiClient | None = None):
        self.api = api or ApiClient()

    def list(self, filters: dict):
        return self.api.list_tickets(filters)

    def get(self, ticket_id: str):
        return self.api.get_ticket(ticket_id)

    def create(self, payload: dict):
        return self.api.create_ticket(payload)
