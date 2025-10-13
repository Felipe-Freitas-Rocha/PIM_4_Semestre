from core.api_client import ApiClient

class KBService:
    def __init__(self, api: ApiClient | None = None):
        self.api = api or ApiClient()

    def list(self, query: str | None = None):
        return self.api.list_kb(query)
