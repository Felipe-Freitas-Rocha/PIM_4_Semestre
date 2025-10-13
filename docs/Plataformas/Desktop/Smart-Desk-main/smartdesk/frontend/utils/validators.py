import re

EMAIL_RX = re.compile(r"^[^@\s]+@[^@\s]+\.[^@\s]+$")

def is_email(s: str) -> bool:
    return bool(EMAIL_RX.match(s or ""))
