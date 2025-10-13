from core.api_client import ApiClient

class AuthService:
    def __init__(self, api: ApiClient | None = None):
        self.api = api or ApiClient()

    def login(self, email: str, password: str):
        ok, data = self.api.login(email, password)
        if ok:
            self.api.set_token(data["token"])
        return ok, data
